using JamilNativeAPI.Models;
using System.Collections.ObjectModel;
using JamilNativeAPI.Respositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JamilNativeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaterController : ControllerBase
    {
        private readonly IWaterManager _waterManager;

        public WaterController(IWaterManager waterManager)
        {
            _waterManager = waterManager;
        }

        [HttpGet("WaterAmount")]
        public async Task<ActionResult<decimal>> retrieveAmount(int id = 1)
        {
            var amount = await _waterManager.GetWaterAmount(id);

            return Ok(amount);
        }

        [HttpPut("WaterAmount")]
        public async Task<ActionResult> UpdateWaterAmount(WaterFactorDto water, int id = 1)
        {
            await _waterManager.UpdateWaterAmount(water, id);
            return Ok();
        }

        [HttpPost("WaterPayment")]
        public async Task<ActionResult> AddWaterPayment(WaterBillDto bill, string FirstTenant, string LastTenanat,
            decimal current, decimal previuos, string house, DateTime realTime)
        {
            await _waterManager.AddWaterPayment(bill, FirstTenant, LastTenanat, current, previuos, house, realTime);
            return Ok();
        }

        [HttpGet("WaterBill")]
        public async Task<ActionResult<ObservableCollection<WaterBillDto>>> RetrieveWaterBill(DateTime startDate, DateTime endDate)
        {
            try
            {
                var houses = await _waterManager.GetWaterBill(startDate, endDate);

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
            await _waterManager.AddPaymentDetails(amountPaid, idPayment);

            return Ok();
        }

        [HttpGet("PaymentDetails")]
        public async Task<ActionResult<ObservableCollection<PaymentDetailsDto>>> RetrievePaymentDetails(int IdPayment)
        {
            try
            {
                var houses = await _waterManager.GetPaymentDetailsById(IdPayment);

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
                var unit = await _waterManager.GetPreviousReading(house);

                return Ok(unit);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"ebyembi: {ex.Message}");
            }

        }

        [HttpGet("LatestAmount")]
        public async Task<ActionResult<decimal>> RetreiveLatestAmount(int idPayment)
        {
            try
            {
                var unit = await _waterManager.GetLatestAmount(idPayment);

                return Ok(unit);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"ebyembi: {ex.Message}");
            }
        }

        [HttpGet("TenantByBill")]
        public async Task<ActionResult<decimal>> RetreiveTenantByWaterBill(int idPayment)
        {
            try
            {
                var unit = await _waterManager.GetTenantByWaterBillId(idPayment);

                return Ok(unit);

            }
            catch (Exception ex)
            {

                return StatusCode(500, $"ebyembi: {ex.Message}");
            }

        }
    }
}
