using TaskManagerCourse.Api.Obstractions;

namespace TaskManagerCourse.Api.Models
{
    public class Desk:CommonObject
    {
        public int Id { get; set; }

        public bool IsPrivat { get; set; }

        public string Columns { get; set; }

        public User Admin { get; set; }

        public int AdminId { get; set; }

        public Project Project  { get; set; }

        public int ProjectId { get; set; }

        public List<Task> Tasks { get; set; } = new List<Task>();
    }
}
