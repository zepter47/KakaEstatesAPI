using System.Collections.ObjectModel;
using JamilNativeAPI.Models;
using JamilNativeAPI.Respositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JamilNativeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TenantsController : ControllerBase
    {
        private readonly IEstateManager _estateManager;

        public TenantsController(IEstateManager estateManager)
        {
            _estateManager = estateManager;
        }

        [HttpPost("MaritalStatus")]
        public async Task<ActionResult> AddMaritalStatus(MaritalStatusDto MStatus)
        {
            await _estateManager.AddMaritalStatus(MStatus);

            return Ok();
        }

        [HttpPost("NokRelationship")]
        public async Task<ActionResult> AddNokRelationship(NokRelationshipDto relation)
        {
            await _estateManager.AddNokRelationship(relation);

            return Ok();
        }

        [HttpGet("MaritalStatus")]
        public async Task<ActionResult<ObservableCollection<MaritalStatusDto>>> GetmaritalDetails()
        {
            try
            {
                var MaritalCol = await _estateManager.GetMaritalStatus();

                return Ok(MaritalCol);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"server error: {ex.Message}");
            }
        }

        [HttpGet("NokRelationship")]
        public async Task<ActionResult<ObservableCollection<NokRelationshipDto>>> GetNokDetails()
        {
            try
            {
                var NokCol = await _estateManager.GetNokRelationships();

                return Ok(NokCol);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"ebyembi: {ex.Message}");
            }
        }

        [HttpPost("TenantDetails")]
        public async Task<ActionResult> AddTenantsDetals(TenantDetailsDto tenant, string MaritalStatus, string Nok, string House)
        {
            await _estateManager.AddTenantDetails(tenant, MaritalStatus, Nok, House);

            return Ok();
        }

        [HttpPost("HouseNumber")]
        public async Task<ActionResult> AddHouseNumber(HouseDto house)
        {
            await _estateManager.AddHouseNumber(house);

            return Ok();
        }

        [HttpGet("HouseNumber")]
        public async Task<ActionResult<ObservableCollection<HouseDto>>> RetrieveHouseNumber()
        {
            try
            {
                var houses = await _estateManager.GetHouseNumber();

                return Ok(houses);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"server error: {ex.Message}");
            }

        }

        [HttpGet("TenantsDetails")]
        public async Task<ActionResult<ObservableCollection<TenantDetailsDto>>> RetrieveTenantsDetails()
        {
            try
            {
                var tenants = await _estateManager.GetTenantDetails();
                return Ok(tenants);
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"server error: {ex.Message}");

            }
        }

        //[HttpPost("WaterAmount")]
        //public async Task<ActionResult> AddWaterAmounts(WaterFactorDto water)
        //{
        //    await _estateManager.AddWaterAmount(water);

        //    return Ok();
        //}

        [HttpGet("WaterAmount")]
        public async Task<ActionResult<decimal>> retrieveAmount(int id = 1)
        {
            var amount = await _estateManager.GetWaterAmount(id);

            return Ok(amount);
        }

        [HttpPut("WaterAmount")]
        public async Task<ActionResult> UpdateWaterAmount(WaterFactorDto water, int id = 1)
        {
            await _estateManager.UpdateWaterAmount(water, id);
            return Ok();
        }

        [HttpPost("WaterPayment")]
        public async Task<ActionResult> AddWaterPayment(WaterBillDto bill, string FirstTenant, string LastTenanat,
            decimal current, decimal previuos, string house)
        {
            await _estateManager.AddWaterPayment(bill, FirstTenant, LastTenanat, current, previuos, house);
            return Ok();
        }

        [HttpGet("WaterPayment")]
        public async Task<ActionResult<ObservableCollection<WaterBillDto>>> RetrieveWaterBill()
        {
            try
            {
                var houses = await _estateManager.GetWaterPayment();

                return Ok(houses);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"server error: {ex.Message}");
            }


        }

        [HttpPost("PaymentDetails")]
        public async Task<ActionResult> AddPaymentDetails(decimal amountPaid, int idPayment)
        {
            await _estateManager.AddPaymentDetails(amountPaid, idPayment);

            return Ok();
        }

        [HttpGet("PaymentDetails")]
        public async Task<ActionResult<ObservableCollection<PaymentDetailsDto>>> RetrievePaymentDetails(int IdPayment)
        {
            try
            {
                var houses = await _estateManager.GetPaymentDetails(IdPayment);

                return Ok(houses);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"server error: {ex.Message}");

            }

        }

        [HttpGet("PreviousUnitReading")]
        public async Task<ActionResult<decimal>> RetrievePreviousReading(string house)
        {
            try
            {
                var unit = await _estateManager.GetPreviousReading(house);

                return Ok(unit);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"ebyembi: {ex.Message}");
            }

        }

        [HttpGet("TenantFirstName")]
        public async Task<ActionResult<ObservableCollection<TenantNamesDto>>> RetrieveTenantFirstName()
        {
            try
            {
                var FirstCol = await _estateManager.GetTenantFirstName();

                return Ok(FirstCol);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"ebyembi: {ex.Message}");
            }
        }

        [HttpGet("TenantLastName")]
        public async Task<ActionResult<ObservableCollection<TenantNamesDto>>> RetrieveTenantLastName()
        {
            try
            {
                var LastCol = await _estateManager.GetTenantLastName();

                return Ok(LastCol);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"ebyembi: {ex.Message}");
            }
        }
    }
}
