using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using TaskManagerCourse.Api.Abstractions;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Obstractions;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Models.Services
{
    public class ProjectServices : AbsrtactServices, ICommonService<ProjectModel>
    {
        private readonly ApplicationContext _db;


        public ProjectServices(ApplicationContext db) 
        {
            _db = db;
        }

        public bool Create(ProjectModel model)
        {
            bool res = DoAction(delegate ()
            {
                Project newProject = new Project(model);
                _db.Projects.Add(newProject);
                _db.SaveChanges();
            });
            return res;
        }

        public bool Delete(int id)
        {
            bool res = DoAction(delegate ()
            {
                Project newProject = _db.Projects.FirstOrDefault(p => p.Id == id);
                _db.Projects.Remove(newProject);
                _db.SaveChanges();
            });
            return res;
        }

        public bool Update(ProjectModel model, int id)
        {
            bool res = DoAction(delegate ()
            {
                Project newProject = _db.Projects.FirstOrDefault(p=>p.Id == id);
                newProject.Name = newProject.Name;
                newProject.Description = newProject.Description;
                newProject.Photo = newProject.Photo;
                newProject.Status = newProject.Status;
                newProject.AdminId = newProject.AdminId;
                _db.Projects.Update(newProject);
                _db.SaveChanges();
            });
            return res;
        }

        public ProjectModel Get(int id)
        {
            Project project = _db.Projects.FirstOrDefault(p => p.Id == id);
            return project?.ToDto();
        }

        public async Task<IEnumerable<ProjectModel>> GetByUserId(int userId)
        {
            List<ProjectModel> res = new List<ProjectModel>();
            var admin = _db.ProjectAdmins.FirstOrDefault(a => a.UserId == userId);
            if (admin != null)
            {   
                var projectForAdmin = await _db.Projects.Where(p => p.AdminId == admin.Id).Select(p=>p.ToDto()).ToListAsync();
            }

            var projectForUser = await _db.Projects.Include(p=>p.AllUsers).Where(p=>p.AllUsers.Any(u=>u.Id == userId)).Select(p => p.ToDto()).ToListAsync();
            return res;
        }

        public IQueryable<ProjectModel> GetAll()
        {
            return _db.Projects.Select(p => p.ToDto());
        }

        public void AddUsersToProject(int id, List<int> userIds)
        {
            Project project = _db.Projects.FirstOrDefault(p => p.Id == id);
            foreach (int userId in userIds)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == userId);
                project.AllUsers.Add(user);
            }
            _db.SaveChanges();
        }

        public void RemoveUsersFromProject(int id, List<int> userIds)
        {
            Project project = _db.Projects.Include(p => p.AllUsers).FirstOrDefault(p => p.Id == id);
            foreach (int userId in userIds)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == userId);
                if (project.AllUsers.Contains(user))
                    project.AllUsers.Remove(user);
            }
            _db.SaveChanges();
        }
    }
}
