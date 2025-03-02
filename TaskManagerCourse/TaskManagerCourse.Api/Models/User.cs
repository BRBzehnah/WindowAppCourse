﻿using TaskManagerCourse.Api.Models;

namespace TaskManagerCourse.Api.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime LastLoginDate { get; set; }

        public byte[]? Photo { get; set; }

        public List<Project> Projects { get; set; } = new List<Project>();

        public List<Desk> Desks { get; set; } = new List<Desk>();

        public List<TaskModel> Tasks { get; set; } = new List<TaskModel>();

        public UserStatus Status { get; set; }

        public User() { }

        public User(string fName, string lName, string email,string password,
            UserStatus status = UserStatus.User,string phone = "none", byte[] photo = null) 
        {
            FirstName = fName;
            LastName = lName;
            Email = email;
            Password = password;
            Phone = phone;
            Photo = photo;
            Status = status;
            RegistrationDate = DateTime.Now;  
        }
    }
}
