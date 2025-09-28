using bank.Data;
using bank.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Transactions;
using Presentation.Helper;

namespace Presentation.Controllers
{
    public class adminController : Controller
    {

        private readonly string apiBaseUrl = "http://localhost:5292/api";

        [HttpGet]
        public async Task<IActionResult> GetAdminProfile(int id)
        {
            var client = new RestClient(apiBaseUrl);
            var request = new RestRequest($"userprofile/{id}", Method.Get);
            var response = await client.ExecuteAsync<UserProfile>(request);

            if (response.IsSuccessful && response.Data != null)
            {
                await Logger.LogAsync($"Admin logged in)");  //log        
                return Json(response.Data);
            }
            return NotFound("User profile not found.");
        }


        [HttpGet]
        public async Task<IActionResult> getUserProfilebyName(string name)
        {
            var client = new RestClient(apiBaseUrl);
            var request = new RestRequest($"userprofile/getByName/{name}", Method.Get);
            var response = await client.ExecuteAsync<UserProfile>(request);


            if (response.IsSuccessful && response.Data != null)
            {
                await Logger.LogAsync($"Admin retrieves user by username)");  //log        
                return Json(response.Data);
            }
            return NotFound("User profile not found.");
        }


         [HttpGet]
         public async Task<IActionResult> getUserProfilebyAcc(int acc)
         {
             var client = new RestClient(apiBaseUrl);
             var request = new RestRequest($"userprofile/account/{acc}", Method.Get);
             var response = await client.ExecuteAsync<UserProfile>(request);

             if (response.IsSuccessful && response.Data != null)
             {
                await Logger.LogAsync($"Admin retrieves user by account number)");  //log        
                return Json(response.Data);
             }
             return NotFound("User profile not found.  !!!!!!!!! ");
         }



        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserProfile userProfile)
        {
            var client = new RestClient(apiBaseUrl);
            var request = new RestRequest("userprofile", Method.Post);
            request.AddJsonBody(userProfile);  // Serialize the user profile data as JSON

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                await Logger.LogAsync($"Admin created user:{userProfile.name}  )");  //log        
                return Ok("User created successfully.");
            }

            return BadRequest("Failed to create user profile.");
        }

    
        
        [HttpGet]
        public async Task<IActionResult> GetAllTransactions()
        {
            var client = new RestClient(apiBaseUrl);
            var request = new RestRequest("transaction", Method.Get); // Assuming the endpoint for transactions is "transaction"
            var response = await client.ExecuteAsync<List<Transact>>(request); // Assuming "Transact" is the model for your transactions

            if (response.IsSuccessful && response.Data != null)
            {
                await Logger.LogAsync($"Admin Retrieved transactions)");  //log        
                return Json(response.Data);
            }
            return NotFound("No transactions found.");
        }


        public async Task<IActionResult> EditUser([FromBody] UserProfile userProfile)
        {
            var client = new RestClient(apiBaseUrl);
            var request = new RestRequest("userprofile", Method.Put);
            request.AddJsonBody(userProfile);

            var response = await client.ExecuteAsync(request);


            if (response.IsSuccessful)
            {
                await Logger.LogAsync($"Admin updated user: userProfile.UserName:{userProfile.userId}  )");  //log        
                return Ok("User updated successfully.");
            }
            return BadRequest("Failed to update user profile.");
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var client = new RestClient(apiBaseUrl);
            var request = new RestRequest($"userprofile/{id}", Method.Delete);

            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                await Logger.LogAsync($"Admin deletes user)");  //log        
                return Ok("User deleted successfully.");
            }
            return BadRequest("Failed to delete user.");
        }



        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] Account acc)
        {
            var client = new RestClient(apiBaseUrl);
            var request = new RestRequest("accounts", Method.Post);
            request.AddJsonBody(acc);
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                await Logger.LogAsync($"Admin created Bank Account:{acc.accNumber}  )");  //log        
                return Ok("Bank Account created successfully");
            }
            return BadRequest("Failed to create bank account");
        }
    }
}

