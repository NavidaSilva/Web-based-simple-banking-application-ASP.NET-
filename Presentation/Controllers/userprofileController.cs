using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Threading.Tasks;
using bank.Models;
using System.Security.Principal;

namespace Presentation.Controllers
{
    public class userprofileController : Controller
    {

        private readonly string apiBaseUrl = "http://localhost:5292/api";

         [HttpGet]
         public async Task<IActionResult> GetUserProfile(int id)
         {
             var client = new RestClient(apiBaseUrl);
             var request = new RestRequest($"userprofile/{id}", Method.Get);
             var response = await client.ExecuteAsync<UserProfile>(request);

             if (response.IsSuccessful && response.Data != null)
             {
                 return Json(response.Data);
             }
             return NotFound("User profile not found.");
         }


        [HttpGet]
        public async Task<IActionResult> GetAccountInfo(int userId)
        {
            var client = new RestClient(apiBaseUrl);
            var request = new RestRequest($"accounts/getByUserId/{userId}", Method.Get);

            var response = await client.ExecuteAsync<List<Account>>(request); // Ensure the response is a List<Account>


            if (response.IsSuccessful && response.Data != null)
            {
                return Json(response.Data);
            }
            return NotFound("Account not found.");
        }



        //update user 
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UserProfile prof)
        {
            var client = new RestClient(apiBaseUrl);
var request = new RestRequest("Userprofile", Method.Put);
            request.AddJsonBody(prof); // Send the updated user profile in the request body

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return Ok("Profile updated successfully.");
            }

            return BadRequest("Profile update failed.");
        }



        // get trans of acc 
        [HttpGet]
        public async Task<IActionResult> GetTrasnofAcc(int accNu)
        {
            var client = new RestClient(apiBaseUrl);
            var request = new RestRequest($"Transaction/{accNu}", Method.Get);

            var response = await client.ExecuteAsync<List<Transact>>(request); 


            if (response.IsSuccessful && response.Data != null)
            {
                return Json(response.Data);
            }
            return NotFound("Account not found.");
        }


        //deposit money
        [HttpPost]
        public async Task<IActionResult> Deposit([FromBody] DepositRequest depositRequest)
        {
            var client = new RestClient(apiBaseUrl);
            var request = new RestRequest("Accounts/deposit", Method.Post);
            request.AddJsonBody(depositRequest);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return Ok("Deposit successful");
            }
            return BadRequest("Deposit failed");
        }



        //Withdraw money
        [HttpPost]
        public async Task<IActionResult> Withdraw([FromBody] DepositRequest depositRequest)
        {
            var client = new RestClient(apiBaseUrl);
            var request = new RestRequest("Accounts/withdraw", Method.Post);
            request.AddJsonBody(depositRequest);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return Ok("Withdraw successful");
            }
            return BadRequest("Withdraw failed");
        }

        // Transfer money
        [HttpPost]
        public async Task<IActionResult> Transfer([FromBody] TransferRequest transferRequest)
        {
            var client = new RestClient(apiBaseUrl);
            var request = new RestRequest("Accounts/transfer", Method.Post);
            request.AddJsonBody(transferRequest);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return Ok(new { message = "Transfer successful" }); // Return as JSON
            }
            return BadRequest(new { message = "Transfer failed" }); // Return as JSON
        }

    }
}


