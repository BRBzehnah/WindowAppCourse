﻿namespace TaskManagerCourse.Common.Models
{
    public class DeskModel : CommonModel
    {
        public bool IsPrivat { get; set; }

        public string[] Columns { get; set; }

        public int ProjectId { get; set; }

        public int AdminId { get; set; }

        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();
    }
}