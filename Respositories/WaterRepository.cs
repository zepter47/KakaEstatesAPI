using System.Collections.ObjectModel;
using JamilNativeAPI.Context;
using JamilNativeAPI.Entities;
using JamilNativeAPI.Models;
using JamilNativeAPI.Respositories.Interfaces;
using JamilNativeAPI.TimeHelper;
using Microsoft.EntityFrameworkCore;

namespace JamilNativeAPI.Respositories
{
    public class WaterRepository: IWaterManager
    {

        //create instance of KakaireEstatesContext to interact with the database
        private readonly KakaireEstatesContext _context;

        public WaterRepository(KakaireEstatesContext context)
        {
            _context = context;
        }

        public async Task AddPaymentDetails(decimal amountPaid, int idPayment)
        {
            try
            {
                var AmountOwed = await _context.TblPaymentDetails.Where(x => x.PaymentId == idPayment)
                    .OrderBy(r => r.PaymentDetailsId).Select(y => y.AmountRemaining).LastOrDefaultAsync();

                if (amountPaid <= AmountOwed)
                {
                    var amount = new TblPaymentDetail
                    {
                        PaymentId = idPayment,
                        AmountPaid = amountPaid,
                        AmountRemaining = (AmountOwed - amountPaid),
                        AddedOn = GetEastFricanTime.RetrieveEastFricanTime()

                    };

                    await _context.TblPaymentDetails.AddAsync(amount);
                    await _context.SaveChangesAsync();
                }
                else { return; }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AddWaterAmount(WaterFactorDto amount)
        {
            try
            {
                var water = new TblWaterFactor { FactorAmount = amount.FactorAmount };

                var winter = await _context.TblWaterFactors.AddAsync(water);

                var deck = await _context.SaveChangesAsync();


            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AddWaterPayment(WaterBillDto bill, string FirstTenant, string LastTenanat,
            decimal current, decimal previuos, string house, DateTime realTime)
        {
            int IdWater = 0;
            int IdPayment = 0;
            decimal FactorAmount = 0;
            decimal AmountToPay = 0;
            //previuos = 0;
            var obudde = GetEastFricanTime.RetrieveRealTime(realTime);

            // Begining a database transaction
            await using var transaction = await _context.Database.BeginTransactionAsync();


            try
            {
                //Retrieving the Tenant_id for which tenats names given match the names in the database
                var Tenant = await _context.TblTenants.Where(k => k.FirstName == FirstTenant && k.LastName == LastTenanat).Select(h => h.TenantId).FirstOrDefaultAsync();

                //Retrieving the House_id for which house nmber given match the names in the database
                var IdHouse = await _context.TblHouses.Where(g => g.HouseNumber == house).Select(a => a.HouseId).FirstOrDefaultAsync();

                //Retrieving the Amount per unit from the TblWaterFactor
                FactorAmount = await _context.TblWaterFactors.Where(x => x.WaterFactorId == 1).Select(y => y.FactorAmount).FirstOrDefaultAsync();

                //Inserting data into TblwaterBill
                var waterBill = new TblWaterbill
                {
                    TenantId = Tenant,
                    HouseId = IdHouse,
                    PreviousReading = previuos,
                    CurrentReading = current,
                    AddedOn = obudde
                };

                await _context.TblWaterbills.AddAsync(waterBill);

                await _context.SaveChangesAsync();

                // Retrieving the WaterbillId after the Tblwaterbill record has been inserted into the database
                IdWater = waterBill.WaterbillId;

                //Inserting data into TblPayment
                var payment = new TblPayment
                {
                    WaterbillId = IdWater,
                    AmountOwed = Math.Abs((current - previuos) * FactorAmount),
                    AddedOn = obudde
                };

                await _context.TblPayments.AddAsync(payment);

                await _context.SaveChangesAsync();

                // Retrieving the PaymentId after the TblPayment has been inserted into the database
                IdPayment = payment.PaymentId;

                // Retrieving the AmountOwed after the TblPayment has been inserted into the database
                AmountToPay = payment.AmountOwed;

                //Inserting data into tblPaymentDetails
                var details = new TblPaymentDetail
                {
                    PaymentId = IdPayment,
                    AmountRemaining = AmountToPay,
                    AddedOn = obudde
                };

                await _context.TblPaymentDetails.AddAsync(details);

                await _context.SaveChangesAsync();

                //commiting the transaction if no exception or bugs occured
                await transaction.CommitAsync();

            }
            catch (Exception)
            {
                //Canceling the transaction due to an exception or error
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<ObservableCollection<PaymentDetailsDto>> GetPaymentDetailsById(int idPayment)
        {
            var PaymentList = new ObservableCollection<PaymentDetailsDto>();

            try
            {
                var PaymentCol = await _context.TblPaymentDetails.Where(t => t.PaymentId == idPayment)
                    .Select(e => new PaymentDetailsDto()
                    {
                        AmountPaid = e.AmountPaid,
                        AmountRemaining = e.AmountRemaining,
                        AddedOn = e.AddedOn,
                    }).ToListAsync();

                PaymentList = new ObservableCollection<PaymentDetailsDto>(PaymentCol);
            }
            catch (Exception)
            {

                throw;
            }
            return PaymentList;
        }

        public async Task<decimal> GetPreviousReading(string house)
        {
            decimal previousReading = 0;
            int idHouse = 0;

            try
            {
                idHouse = _context.TblHouses.Where(d => d.HouseNumber == house).Select(q => q.HouseId).FirstOrDefault();


                var PreviousUnits = await _context.TblWaterbills.Where(f => f.HouseId == idHouse).OrderBy(p => p.WaterbillId)
                .Select(d => d.CurrentReading).LastOrDefaultAsync();

                previousReading = PreviousUnits;
            }
            catch (Exception)
            {

                throw;
            }
            return previousReading;
        }

        public async Task<decimal> GetWaterAmount(int id)
        {
            decimal waterAmount = 0;
            try
            {
                var Amount = await _context.TblWaterFactors.Where(y => y.WaterFactorId == id).Select(x => x.FactorAmount).FirstOrDefaultAsync();

                waterAmount = Amount;
            }
            catch (Exception)
            {

                throw;
            }
            return waterAmount;

        }

        public async Task<ObservableCollection<WaterBillDto>> GetWaterBill(DateTime startDate, DateTime endDate)
        {
            var billList = new ObservableCollection<WaterBillDto>();

            try
            {
                var Lists = await _context.TblWaterbills.Include(y => y.Tenant).Include(y => y.TblPayment).Include(y => y.House).
                    Include(y=>y.TblPayment.TblPaymentDetails)
                    .Where(x => x.AddedOn >= startDate && x.AddedOn <= endDate)
                    .Select(x => new WaterBillDto()
                    {
                        //TenantFirstName = x.Tenant.FirstName ,
                        //TenantLastName = x.Tenant.LastName ,
                        WaterBillId = x.WaterbillId,
                        tenant = $"{x.Tenant.FirstName} {x.Tenant.LastName}",
                        payment = x.TblPayment.AmountOwed,
                        house = x.House.HouseNumber,
                        PreviousReading = x.PreviousReading,
                        CurrentReading = x.CurrentReading,
                        AddedOn = x.AddedOn,
                        BillNumber = x.WaterbillId,
                        UnitsUsed = (x.CurrentReading - x.PreviousReading),
                        AmountRemaining = x.TblPayment.TblPaymentDetails.
                        OrderByDescending(d=>d.AddedOn).Select(f=>f.AmountRemaining).FirstOrDefault()
                    }).ToListAsync();

                billList = new ObservableCollection<WaterBillDto>(Lists);
            }
            catch (Exception)
            {

                throw;
            }

            return billList;
        }

        public async Task UpdateWaterAmount(WaterFactorDto amount, int waterId)
        {
            try
            {
                var AmountToUpdate = await _context.TblWaterFactors.FindAsync(waterId);

                if (AmountToUpdate.FactorAmount != amount.FactorAmount)
                {
                    AmountToUpdate.FactorAmount = amount.FactorAmount;
                }
                else { Console.WriteLine($"The amount in field is same as database, hence no operation"); }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<decimal> GetLatestAmount(int idPayment)
        {
            decimal LatestAmount = 0;
            try
            {

                var AmountOwed = await _context.TblPaymentDetails.Where(x => x.PaymentId == idPayment)
                    .OrderBy(r => r.PaymentDetailsId).Select(y => y.AmountRemaining).LastOrDefaultAsync();

                LatestAmount = AmountOwed;
            }
            catch (Exception)
            {

                throw;
            }
            return LatestAmount;
        }

    }
}
