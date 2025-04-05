using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Models.Services;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserServices _us;

        public UsersController(ApplicationContext db)
        {
            _db = db;
            _us = new UserServices(db);
        }

        [AllowAnonymous]
        [HttpGet("test")]
        public IActionResult TestApi()
        {
            return Ok("Hello world");
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserModel userModel)
        {
            if (userModel != null)
            {
                bool res = _us.Create(userModel);
                return res ? Ok() : BadRequest();
            }
            return BadRequest();
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateUser(int id, [FromBody] UserModel userModel)
        {
            if (userModel != null) 
            {
                bool res = _us.Update(userModel,id);
                return res ? Ok() : BadRequest();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                bool res = _us.Delete(id);
                return res ? Ok() : BadRequest();
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IEnumerable<UserModel>> GetUsers()
        {
            return await _db.Users.Select(u => u.ToDto()).ToListAsync();
        }

        [HttpPost("/several")]
        public async Task<IActionResult> CreateMultipleUsers([FromBody]List<UserModel> userModels)
        {
            if (userModels != null && userModels.Count > 0)
            {
                bool res = _us.CreateMultipleUsers(userModels);
                return res ? Ok() : BadRequest();
            }
            return BadRequest();
        }
    }
}
