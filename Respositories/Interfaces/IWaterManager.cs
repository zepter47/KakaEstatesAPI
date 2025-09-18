using JamilNativeAPI.Models;
using System.Collections.ObjectModel;

namespace JamilNativeAPI.Respositories.Interfaces
{
    public interface IWaterManager
    {
        public Task AddWaterPayment(WaterBillDto bill, string FirstTenant,
            string LastTenanat, decimal current, decimal previuos, string house, DateTime realTime);

        public Task<ObservableCollection<WaterBillDto>> GetWaterBill(DateTime startDate, DateTime endDate);

        public Task AddWaterAmount(WaterFactorDto amount);

        public Task<decimal> GetWaterAmount(int id);

        public Task UpdateWaterAmount(WaterFactorDto amount, int waterId);

        public Task AddPaymentDetails(decimal amountPaid, int idPayment);

        public Task<ObservableCollection<PaymentDetailsDto>> GetPaymentDetailsById(int idPayment);

        public Task<decimal> GetPreviousReading(string house);

        public Task<decimal> GetLatestAmount(int idPayment);


    }
}
