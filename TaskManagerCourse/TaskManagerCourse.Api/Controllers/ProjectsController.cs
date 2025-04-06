using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManagerCourse.Api.Models;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Models.Services;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProjectsController : ControllerBase
    {
        private readonly ApplicationContext _db;
        private readonly UserServices _us;
        private readonly ProjectServices _ps;

        public ProjectsController(ApplicationContext db)
        {
            _db = db;
            _ps = new ProjectServices(db);
            _us = new UserServices(db);
        }

        [HttpGet]
        public async Task<IEnumerable<ProjectModel>> Get()
        {
            var user = _us.GetUser(HttpContext.User.Identity.Name);
            if (user.Status == UserStatus.Admin)
                return await _ps.GetAll().ToListAsync();
            else
                return await _ps.GetByUserId(user.Id);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var project = _ps.Get(id);
            return project == null ? NotFound() : Ok(project);
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProjectModel model)
        {
            if (model != null)
            {
                var user = _us.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                    var admin = _db.ProjectAdmins.FirstOrDefault(a => a.User.Id == user.Id);
                    if (admin == null)
                    {
                        admin = new ProjectAdmin(user);
                        _db.ProjectAdmins.Add(admin);
                    }
                    model.AdminId = admin.Id;

                    bool res = _ps.Create(model);
                    return res ? Ok() : BadRequest();
                    }
                }
                return Unauthorized();
            }
            return BadRequest();
        }

        [HttpPatch]
        public IActionResult Update(int id,[FromBody] ProjectModel model)
        {
            if (model != null)
            {
                var user =_us.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        bool res = _ps.Update(model, id);
                        return res ? Ok() : BadRequest();
                    }
                }
                return Unauthorized();
            }
            return BadRequest();
        } 

        [HttpDelete]
        public IActionResult Delete(int id)
        {
                bool res = _ps.Delete(id);
                return res ? Ok() : NotFound();
        }

        [HttpPatch("{id}/users")]
        public IActionResult AddUsersToProject(int id, [FromBody] List<int> usersIds)
        {
            if (usersIds != null)
            {
                var user = _us.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        _ps.AddUsersToProject(id, usersIds);
                        return Ok();
                    }
                }
                return Unauthorized();

            }
            return BadRequest();
        }
        
        [HttpPatch("{id}/users/remove/{userId}")]
        public IActionResult RemoveUsersToProject(int id, [FromBody] List<int> usersIds)
        {
            if (usersIds != null)
            {
                var user = _us.GetUser(HttpContext.User.Identity.Name);
                if (user != null)
                {
                    if (user.Status == UserStatus.Admin || user.Status == UserStatus.Editor)
                    {
                        _ps.RemoveUsersFromProject(id, usersIds);
                        return Ok();
                    }
                }
                return Unauthorized();

            }
            return BadRequest();
        }
    }
}
