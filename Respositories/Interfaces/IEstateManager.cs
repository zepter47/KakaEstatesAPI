using System.Collections.ObjectModel;
//using JamilNativeAPI.Entities;
using JamilNativeAPI.Models;

namespace JamilNativeAPI.Respositories.Interfaces
{
    public interface IEstateManager
    {
        public Task AddMaritalStatus(MaritalStatusDto status);

        public Task AddNokRelationship(NokRelationshipDto nok);

        public Task<ObservableCollection<MaritalStatusDto>> GetMaritalStatus();

        public Task<ObservableCollection<NokRelationshipDto>> GetNokRelationships();

        public Task AddTenantDetails(TenantDetailsDto tenant, string MaritalStatus, string Nok, string House);

        public Task AddWaterPayment(WaterBillDto bill, string FirstTenant, 
            string LastTenanat, decimal current, decimal previuos, string house, DateTime realTime);

        public Task<ObservableCollection<WaterBillDto>> GetWaterBill(DateTime startDate, DateTime endDate);

        public Task AddHouseNumber(HouseDto house);

        public Task<ObservableCollection<HouseDto>> GetHouseNumber();

        public Task<ObservableCollection<TenantDetailsDto>> GetTenantDetails();

        public Task AddWaterAmount(WaterFactorDto amount);

        public Task<decimal> GetWaterAmount(int id);

        public Task UpdateWaterAmount(WaterFactorDto amount, int waterId);

        public Task AddPaymentDetails(decimal amountPaid, int idPayment);

        public Task<ObservableCollection<PaymentDetailsDto>> GetPaymentDetails(int idPayment);

        public Task<decimal> GetPreviousReading(string house);

        public Task<ObservableCollection<TenantDetailsDto>> GetTenantFirstName();

        public Task<ObservableCollection<TenantDetailsDto>> GetTenantLastName();

        public Task<TenantDetailsDto>GetTenant(int id);

    }
}
