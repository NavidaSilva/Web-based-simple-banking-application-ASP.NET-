using bank.Data;
using bank.Models;
using Microsoft.AspNetCore.Mvc;

namespace bank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class TransactionController : Controller
    {
        [HttpGet]
        public IEnumerable<Transact> GetAllTransactions()
        {
            List<Transact> trans = DBManager.GetAllTransactions();
            return trans ;
        }



        [HttpGet]
        [Route("{id}")]
        public IEnumerable<Transact> GetTransByAcc(int id)  //get transactions by account number
        {
            List<Transact> trans = DBManager.GetTransByAccNum(id);
            return trans;
        }
    }
}
