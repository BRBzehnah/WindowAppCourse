using TaskManagerCourse.Api.Abstractions;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Obstractions;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Models.Services
{
    public class ProjectServices : AbsrtactServices, ICommonService<ProjectModel>
    {
        public ProjectServices(ApplicationContext db) 
        {
            _db = db;
        }

        private readonly ApplicationContext _db;

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
    }
}
