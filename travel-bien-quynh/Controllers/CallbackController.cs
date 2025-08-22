using travel_bien_quynh.Entities;
using travel_bien_quynh.Repositories.Interface;
using travel_bien_quynh.Requests;
using travel_bien_quynh.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Transactions;

namespace travel_bien_quynh.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CallbackController : ControllerBase
    {

        private readonly IAtmService _atmService;
        private readonly IAtmHistory _atmHistory;
        private readonly IConfiguration _config;
        

        public CallbackController(IAtmService atmService,IAtmHistory atmHistory,IConfiguration configuration) { 
               _atmService = atmService;
               _atmHistory = atmHistory;
                _config = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> CheckHistory()
        {
         
            try
            {
                  await _atmService.CallBack();

            }catch (Exception ex)
            {
                return BadRequest(new { success = false ,msg = ex.Message });
            }



            return StatusCode(200, new { success = true, msg = "ok" });
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllHistory()
        {
            try
            {
                var histories = await _atmHistory.GetAsync();

                if (histories == null || !histories.Any())
                {
                    return NotFound(new { success = false, message = "No history found" });
                }

                return Ok(new { success = true, msg = "ok", data = histories });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error: " + ex.Message });
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetHistoryUser()
        {
            
            string username = User.FindFirst(JwtRegisteredClaimNames.PreferredUsername)?.Value;

            if (username == null)
            {
                return Unauthorized(new { msg = "User not found" });
            }

            var histories = await _atmHistory.GetAsync();
            var history = histories.Where(e => e.Username == username);


            return Ok(new { success = true, msg = "ok", data = history });
        }
        //[Authorize(Policy = "AdminPolicy")]
        [HttpGet]
        public async Task<IActionResult> GetHistory()
        {
            var history = await _atmService.GetHistoryAtm();
            return Ok(new { success = true, msg = "ok" ,data = history});
        }

        [HttpPost]

        public async Task<IActionResult> ReceiveWebhook([FromBody] JsonElement data)
        {
          
            if (data.ValueKind != JsonValueKind.Object)
            {
                return BadRequest(new { success = false, message = "No data" });
            }
            var authorizationHeader = HttpContext.Request.Headers["Authorization"].ToString();

            if (!authorizationHeader.Equals("Apikey " + _config["Atm:key"]))
            {
                return Unauthorized(new {success = false , message = "Authorization header is missing" });
            }

            try
            {
                var gateway = data.GetProperty("gateway").GetString();
                var transactionDate = data.GetProperty("transactionDate").GetString();
                var accountNumber = data.GetProperty("accountNumber").GetString();
                var subAccount = data.TryGetProperty("subAccount", out var subAccountProp) ? subAccountProp.GetString() : null;
                var code = data.TryGetProperty("code", out var codeProp) ? codeProp.GetString() : null;
                var transactionContent = data.GetProperty("content").GetString();
                var transferType = data.GetProperty("transferType").GetString();
                var transferAmount = data.GetProperty("transferAmount").GetDouble();
                var accumulated = data.GetProperty("accumulated").GetDouble();
                var referenceNumber = data.GetProperty("referenceCode").GetString();
                var body = data.GetProperty("description").GetString();
                var id = data.GetProperty("id").GetInt32();

            
                if (string.IsNullOrEmpty(gateway) || string.IsNullOrEmpty(transactionDate) || string.IsNullOrEmpty(accountNumber) || string.IsNullOrEmpty(transactionContent) || string.IsNullOrEmpty(transferType) || string.IsNullOrEmpty(referenceNumber) || string.IsNullOrEmpty(body))
                {
                    return BadRequest(new { success = false, message = "One or more required fields are missing." });
                }


                var lastTransaction = (await _atmHistory.GetAsync())
                        .Where(h => h.accountNumber == accountNumber)
                        .OrderByDescending(h => h.transactionDate)
                        .FirstOrDefault();

                double balanceBefore = lastTransaction?.BalanceAfter ?? 0;
                double balanceAfter = transferType == "in" ? balanceBefore + transferAmount : balanceBefore - transferAmount;

                double amountIn = transferType == "in" ? transferAmount : 0;
                double amountOut = transferType == "out" ? transferAmount : 0;

                AtmHistory history = new AtmHistory
                {
                    Username = transactionContent,
                    transactionDate = DateTime.Parse(transactionDate),
                    accountNumber = accountNumber,
                    amountIn = amountIn,
                    referenceNumber = referenceNumber,
                    bankBrandName = gateway,
                    BalanceBefore = balanceBefore,
                    BalanceAfter = balanceAfter
                };
                _atmService.xuLyGiaoDich(history);

                return Ok(new { success = true,msg = "ok" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Internal server error: " + ex.Message });
            }
        }
    }
    
}
