//using JamilNativeAPI.DataContext;
//using JamilNativeAPI.Entities;
using JamilNativeAPI.Respositories.Interfaces;
using JamilNativeAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using JamilNativeAPI.Context;
using JamilNativeAPI.Entities;
using Microsoft.VisualBasic;
using JamilNativeAPI.TimeHelper;

namespace JamilNativeAPI.Respositories
{
    public class TenantRepository : IEstateManager
    {
        //create instance of KakaireEstatesContext to interact with the database
        private readonly KakaireEstatesContext _context;

        public TenantRepository(KakaireEstatesContext context)
        {
            _context = context;
        }

        public async Task AddHouseNumber(HouseDto houseNo)
        {
            var house = new TblHouse { HouseNumber = houseNo.HouseNumber};

           await _context.TblHouses.AddAsync(house);

            await _context.SaveChangesAsync();  
        }

        public async Task AddMaritalStatus(MaritalStatusDto status)
        {
			try
            {

                var TenantStatus = new TblMaritalstatus { Status = status.TStatus };

                //Adding the TblMariralStauts objects to the TblMariralStautses Dbset
                     //This prepares the object to be inserted into the database
                    await _context.TblMaritalstatuses.AddAsync(TenantStatus);

                //Save the changes to the database
                   // This actually performs the INSERT operation in the database
                    await _context.SaveChangesAsync();



            }
            catch (Exception)
			{

				throw;
			}
        }

        public async Task AddNokRelationship(NokRelationshipDto nok)
        {
            try
            {
                var relation = new TblNokRelationship { Relatioship = nok.Status };

                await _context.TblNokRelationships.AddAsync(relation);

                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task AddPaymentDetails(decimal amountPaid, int idPayment)
        {
            try
            {
                var AmountOwed = await _context.TblPaymentDetails.Where(x => x.PaymentId == idPayment)
                    .OrderBy(r=>r.PaymentDetailsId).Select(y => y.AmountRemaining).LastOrDefaultAsync();

                if(amountPaid <= AmountOwed)
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

        public async Task AddTenantDetails(TenantDetailsDto tenant, string MaritalStatus, string Nok, string House)
        {
            var marital = tenant.MaritalstatusId;
            var house = tenant.HouseId;
            var obudde = GetEastFricanTime.RetrieveEastFricanTime();


            //Get MaritalStatusId for the selected maritals status
            marital =  _context.TblMaritalstatuses.Where(y => y.Status == MaritalStatus).Select(x => x.MaritalstatusId).FirstOrDefault();

            //Get NokRelationshipId for the selected relationship
            tenant.NokRelationshipId = _context.TblNokRelationships.Where(x => x.Relatioship == Nok).Select(y => y.NokRelationshipId).FirstOrDefault();
            //Get HouseId for the selected house
            house = _context.TblHouses.Where(y => y.HouseNumber == House).Select(x => x.HouseId).FirstOrDefault();

            //Give the tbltenants columns new values in the textBoxes
            var tenantDetails = new TblTenant
            {
                FirstName = tenant.FirstName,
                LastName = tenant.LastName,
                NinNumber = tenant.NinNumber,
                BirthDate = tenant.BirthDate,
                Gender = tenant.Gender,
                PhoneNumber = tenant.PhoneNumber,
                OccupantsNumber = tenant.OccupantsNumber,
                MaritalstatusId = marital,
                NextofkinName = tenant.NextofkinName,
                NokRelationshipId = tenant.NokRelationshipId,
                NokPhonenumber = tenant.NokPhonenumber,
                HouseId = house,
                AddedOn = obudde
            };

            Console.WriteLine($"The date in uganda is {obudde}");

            //Adding the TblTenant objects to the TblTenantss Dbset
            //This prepares the object to be inserted into the database
            await _context.TblTenants.AddAsync(tenantDetails);

            //Save the changes to the database
            // This actually performs the INSERT operation in the database
            await _context.SaveChangesAsync();

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
                var IdHouse = await _context.TblHouses.Where(g=>g.HouseNumber == house).Select(a=>a.HouseId).FirstOrDefaultAsync();

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

        public async Task<ObservableCollection<HouseDto>> GetHouseNumber()
        {
            var HouseCollection = new ObservableCollection<HouseDto>();
            try
            {
                var HouseCol = await _context.TblHouses.Select(x => new HouseDto() {
                    HouseNumber = x.HouseNumber }).ToListAsync();

                HouseCollection = new ObservableCollection<HouseDto>(HouseCol);
            }
            catch (Exception)
            {

                throw;
            }
            return HouseCollection;
        }

        public async Task<ObservableCollection<MaritalStatusDto>> GetMaritalStatus()
        {
            var maritalCollection = new ObservableCollection<MaritalStatusDto>();
            try
            {
                 var Mstatus = await _context.TblMaritalstatuses.Select(x => new MaritalStatusDto()
                 {
                     TStatus = x.Status
                 }).ToListAsync();

                maritalCollection = new ObservableCollection<MaritalStatusDto>(Mstatus);
            }
            catch (Exception)
            {

                throw;
            }
            return maritalCollection;
        }

        public async Task<ObservableCollection<NokRelationshipDto>> GetNokRelationships()
        {
            var relationshipCollection = new ObservableCollection<NokRelationshipDto>();

            try
            {
                var NRelation = await _context.TblNokRelationships.Select(y => new NokRelationshipDto()
                {
                    Status = y.Relatioship
                }).ToListAsync();

                relationshipCollection = new ObservableCollection<NokRelationshipDto>(NRelation);
            }
            catch (Exception)
            {

                throw;
            }
            return relationshipCollection;
        }

        public async Task<ObservableCollection<PaymentDetailsDto>> GetPaymentDetails(int idPayment)
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
                idHouse = _context.TblHouses.Where(d=>d.HouseNumber == house).Select(q=>q.HouseId).FirstOrDefault();


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

        public async Task<TenantDetailsDto> GetTenant(int id)
        {
            TenantDetailsDto tenant = new();

            try
            {
                var resident = await _context.TblTenants.Where(y => y.TenantId == id).Include(t => t.Maritalstatus).Include(t => t.NokRelationship)
                    .Include(t => t.House).Select(x => new TenantDetailsDto()
                    {
                        TenantId = x.TenantId,
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        NinNumber = x.NinNumber,
                        BirthDate = x.BirthDate,
                        Gender = x.Gender,
                        PhoneNumber = x.PhoneNumber,
                        OccupantsNumber = x.OccupantsNumber,
                        TdMaritalstatus = x.Maritalstatus.Status,
                        NextofkinName = x.NextofkinName,
                        TdNokRelationship = x.NokRelationship.Relatioship,
                        NokPhonenumber = x.NokPhonenumber,
                        TdHouse = x.House.HouseNumber,
                        AddedOn = x.AddedOn,
                    }).FirstOrDefaultAsync();

                tenant = resident;
            }
            catch (Exception)
            {

                throw;
            }

            return tenant;
        }

        public async Task<ObservableCollection<TenantDetailsDto>> GetTenantDetails()
        {
            var tenantCollection = new ObservableCollection<TenantDetailsDto>();

            try
            {
                var tenantCol = await _context.TblTenants.Include(t => t.Maritalstatus)
                    .Include(t => t.NokRelationship)
                    .Include(t => t.House)
                    .Select(x => new TenantDetailsDto()
                {
                    TenantId = x.TenantId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    NinNumber = x.NinNumber,
                    BirthDate  = x.BirthDate,
                    Gender = x.Gender,
                    PhoneNumber = x.PhoneNumber,
                    OccupantsNumber = x.OccupantsNumber,
                    TdMaritalstatus = x.Maritalstatus.Status,
                    NextofkinName = x.NextofkinName,
                    TdNokRelationship = x.NokRelationship.Relatioship,
                    NokPhonenumber = x.NokPhonenumber,
                    TdHouse = x.House.HouseNumber,
                    AddedOn = x.AddedOn,
                    //Period = EF.Functions.DateDiffDay(x.AddedOn, DateAndTime.Now)


                }).ToListAsync();

                Console.WriteLine($"The curren date is :{GetEastFricanTime.RetrieveEastFricanTime()}");

                tenantCollection = new ObservableCollection<TenantDetailsDto>(tenantCol);
            }
            catch (Exception)
            {

                throw;
            }
            return tenantCollection;
        }

        public async Task<ObservableCollection<TenantDetailsDto>> GetTenantFirstName()
        {
            var tenantCollections = new ObservableCollection<TenantDetailsDto>();

            try
            {
                var FirstNames = await _context.TblTenants.Select(w => new TenantDetailsDto()
                {
                    FirstName = w.FirstName
                }).ToListAsync();

                tenantCollections = new ObservableCollection<TenantDetailsDto>(FirstNames);
            }
            catch (Exception)
            {

                throw;
            }
            return tenantCollections;
        }

        public async Task<ObservableCollection<TenantDetailsDto>> GetTenantLastName()
        {
            var tenantCollection = new ObservableCollection<TenantDetailsDto>();

            try
            {
                var LastNames = await _context.TblTenants.Select(w => new TenantDetailsDto()
                {
                    LastName = w.LastName
                }).ToListAsync();

                tenantCollection = new ObservableCollection<TenantDetailsDto>(LastNames);
            }
            catch (Exception)
            {

                throw;
            }
            return tenantCollection;

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
                var Lists = await _context.TblWaterbills.Include(y => y.Tenant).Include(y => y.TblPayment).Include(y => y.House)
                    .Where(x => x.AddedOn >= startDate && x.AddedOn <= endDate)
                    .Select(x => new WaterBillDto()
                    {
                        //TenantFirstName = x.Tenant.FirstName ,
                        //TenantLastName = x.Tenant.LastName ,
                        tenant = $"{x.Tenant.FirstName} {x.Tenant.LastName}",
                        payment = x.TblPayment.AmountOwed,
                        house = x.House.HouseNumber,
                        PreviousReading=x.PreviousReading,
                        CurrentReading=x.CurrentReading,
                        AddedOn= x.AddedOn,
                        BillNumber = x.WaterbillId,
                        UnitsUsed = (x.CurrentReading - x.PreviousReading)
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

                if(AmountToUpdate.FactorAmount != amount.FactorAmount)
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
    }
}
