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
            return await _db.Projects.Select(p => p.ToDto()).ToListAsync();
        }

        [HttpPost]
        public IActionResult Create([FromBody] ProjectModel model)
        {
            if (model != null)
            {
                var user = _us.GetUser(HttpContext.User.Identity.Name);
                if (user == null)
                {
                    var admin = _db.ProjectAdmins.FirstOrDefault(a => a.User.Id == user.Id);
                    if (admin == null)
                    {
                        admin = new ProjectAdmin(user);
                        _db.ProjectAdmins.Add(admin);
                    }
                    model.AdminId = admin.Id;
                }
                    bool res = _ps.Create(model);
                    return res ? Ok() : BadRequest();
            }
            return BadRequest();
        }

        [HttpPatch]
        public IActionResult Update(int id,[FromBody] ProjectModel model)
        {
            if (model != null)
            {
                bool res = _ps.Update(model, id);
                return res ? Ok() : BadRequest();
            }
            return BadRequest();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
                bool res = _ps.Delete(id);
                return res ? Ok() : NotFound();
        }
    }
}
