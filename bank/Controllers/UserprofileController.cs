using bank.Data;
using bank.Models;
using Microsoft.AspNetCore.Mvc;

namespace bank.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class UserprofileController : Controller
    {
        public IEnumerable<UserProfile> getUserProfiles()
        {
            List<UserProfile> users = DBManager.GetAllUsers();
            return users;
        }

        [HttpPost]
        public IActionResult Post([FromBody] UserProfile prof)
        {
            if (DBManager.InsertUserProfile(prof))
            {
                return Ok("Successfully inserted");
            }
            return BadRequest("Error in data insertion");
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(int id)
        {
            UserProfile prof = DBManager.GetProfileById (id);
            if (prof == null)
            {
                return NotFound();
            }
            return Ok(prof);
        }


        [HttpGet]
        [Route("getByName/{name}")]
        public IActionResult GetByName(string name)
        {
            UserProfile prof = DBManager.GetProfileByName(name);
            if (prof == null)
            {
                return NotFound();
            }
            return Ok(prof);
        }


        [HttpGet]
        [Route("getByEmail/{email}")]
        public IActionResult GetByEmail(string email)
        {
            UserProfile prof = DBManager.GetProfileByEmail(email);
            if (prof == null)
            {
                return NotFound();
            }
            return Ok(prof);
        }



        [HttpPut]
        public IActionResult UpdateUser(UserProfile prof)
        {
            if (DBManager.UpdateUserProfile (prof))
            {
                return Ok("Successfully updated");
            }
            return BadRequest("Could not update");
        }



        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteUser(int id)
        {
            if (DBManager.DeleteUser(id))
            {
                return Ok("User successfully deleted");
            }
            return BadRequest("Failed to delete user");
        }

        //get userprofile by acc number
        [HttpGet]
        [Route("account/{accNumber}")]
        public IActionResult GetByAccountNumber(int accNumber)
        {
            UserProfile prof = DBManager.GetProfileByAccountNumber(accNumber);

            if (prof == null)
            {
                return NotFound();
            }
            return Ok(prof);

        }
    }
}


