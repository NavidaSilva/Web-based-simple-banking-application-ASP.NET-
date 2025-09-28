using bank.Data;
using bank.Models;
using Microsoft.AspNetCore.Mvc;

namespace bank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : Controller
    {
        public IEnumerable<Account> GetAccounts()
        {
            List<Account> accounts = DBManager.GetAllAccounts() ;
            return accounts ;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Account acc)
        {

            if (DBManager.InsertAccount (acc) )
            {
                return Ok("Successfully inserted");
            }
            return BadRequest("Error in data insertion");
        }
        /*{
    "accNumber" : 22222222,
    "userId" : 3,
    "accType" : "savings",
    "accBalance": 5231 ,
    "accStatus" : "closed"
}*/


        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            Account acc = DBManager.GetAccById(id); //get acc by account number
            if (acc == null)
            {
                return NotFound();
            }
            return Ok(acc);
        }


        [HttpGet]
        [Route("getByUserId/{id}")]
        public IEnumerable<Account> GetByUserId(int id)
        {
            List<Account> accounts = DBManager.GetAccByuserId(id);
            return accounts;
        }

        [HttpPut]
        public IActionResult UpdateAccount(Account acc)
        {
            if (DBManager.UpdateAccount(acc))
            {
                return Ok("Successfully updated");
            }
            return BadRequest("Could not update");
        }
    
        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            if (DBManager.DeleteAccount (id))
            {
                return Ok("Successfully Deleted");
            }
            return BadRequest("Could not delete");
        }

        [HttpPost] // Use POST for the deposit operation
        [Route("deposit")]
        public IActionResult Deposit([FromBody] DepositRequest depositRequest)
        {
            if (DBManager.Deposit(depositRequest.AccNumber, depositRequest.Amount))
            {
                return Ok("Deposit successful");
            }
            return BadRequest("Deposit failed");
        }

        [HttpPost] // Use POST for the withdrawal operation
        [Route("withdraw")]
        public IActionResult Withdraw([FromBody] WithdrawRequest withdrawRequest)
        {
            if (DBManager.Withdraw(withdrawRequest.AccNumber, withdrawRequest.Amount))
            {
                return Ok("Withdrawal successful");
            }
            return BadRequest("Withdrawal failed");
        }

        [HttpPost]
        [Route("transfer")]   //transfer
        public IActionResult Transfer([FromBody] TransferRequest transferRequest)
        {
            if (DBManager.Transfer(transferRequest.FromAccNumber, transferRequest.ToAccNumber, transferRequest.Amount))
            {
                return Ok("Transfer successful");
            }
            return BadRequest("Transfer failed or insufficient balance");
        }
    }
}

