using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNETCore_API_Documentation_Swagger.Model;

namespace DotNETCore_API_Documentation_Swagger.Controllers.V1
{   
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName ="V1")]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        List<UserModel> _oUsers = new List<UserModel>()
        { 
            new UserModel(){Id = 1, UserName = "SampleUserName1", Password = "SamplePassword1"},
            new UserModel(){Id = 2, UserName = "SampleUserName2", Password = "SamplePassword2"},
            new UserModel(){Id = 3, UserName = "SampleUserName3", Password = "SamplePassword3"},
        };

        /// <summary>
        /// This is a sample XML Comment API Description
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("GetUsers")]
        public IActionResult GetUsers()
        {
            if (_oUsers.Count == 0)
            {
                return NotFound("No List Found");
            }
            return Ok(_oUsers);
        }

        [HttpGet]
        [Route("GetUser/{id}")]
        public IActionResult GetUserById(int id)
        {
            var oUser = _oUsers.SingleOrDefault(c => c.Id == id);
            if (oUser == null)
            {
                return NotFound("No List Found");
            }
            return Ok(oUser);
        }

        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUsers(UserModel user)
        {
            _oUsers.Add(user);
            if (_oUsers.Count == 0)
            {
                return NotFound("No List Found");
            }
            return Ok(_oUsers);
        }

        [HttpPut]
        [Route("UpdateUser")]
        public IActionResult UpdateUser(UserModel user)
        {
            var a = _oUsers.Where(c => c.Id == user.Id).FirstOrDefault();

            if (a == null)
            {
                return NotFound("No List Found");
            }
            else
            {
                a.UserName = user.UserName;
                a.Password = user.Password;
              
            }

            return Ok(_oUsers);
        }


        [HttpDelete]
        [Route("DeleteUser")]
        public IActionResult DeleteUser(int Id)
        {
            var a = _oUsers.Where(c => c.Id == Id).SingleOrDefault();

            if (a == null)
            {
                return NotFound("No List Found");
            }
            _oUsers.Remove(a);

            return Ok(_oUsers);
        }
    }
}
