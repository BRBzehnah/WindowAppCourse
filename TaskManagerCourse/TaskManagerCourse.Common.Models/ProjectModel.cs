﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerCourse.Common.Models
{
    public class ProjectModel : CommonModel
    {
        public int? AdminId { get; set; }

        public ProjectStatus Status { get; set; }

        public List<UserModel> AllUsers { get; set; } = new List<UserModel>();

        public List<DeskModel> AllDesks { get; set; } = new List<DeskModel>();
    }
}
