﻿using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;
using TaskManagerCourse.Api.Abstractions;
using TaskManagerCourse.Api.Controllers;
using TaskManagerCourse.Api.Models.Data;
using TaskManagerCourse.Api.Obstractions;
using TaskManagerCourse.Common.Models;

namespace TaskManagerCourse.Api.Models.Services
{
    public class UserServices : AbsrtactServices, ICommonService<UserModel>
    {
        private readonly ApplicationContext _db;
        public UserServices(ApplicationContext db)
        {
            _db = db;
        }

        public Tuple<string, string> GetUserLogginPassFromBasicAuth(HttpRequest request)
        {
            string userLogin = "";
            string userPass = "";
            string authHeader = request.Headers["Authorization"].ToString();

            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUserNamePass = authHeader.Replace("Basic", "");
                var encoding = Encoding.GetEncoding("iso-8859-1");
                string[] namePassArray = encoding.GetString(Convert.FromBase64String(encodedUserNamePass)).Split(':');
                userLogin = namePassArray[0];
                userPass = namePassArray[1];
            }
            return new Tuple<string, string>(userLogin, userPass);
        }

        public User GetUser(string login, string pass)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == login && u.Password == pass);
            return user;
        }

        public User GetUser(string login)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == login);
            return user;
        }

        public ClaimsIdentity GetIdentity(string username, string password)
        {
            User currentUser = GetUser(username, password);
            if (currentUser != null)
            {
                currentUser.LastLoginDate = DateTime.Now;
                _db.Users.Update(currentUser);
                _db.SaveChanges();

                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, currentUser.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, currentUser.Status.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }

        public bool Create(UserModel model)
        {
            return DoAction(delegate ()
            {
                User newUser = new User(model.FirstName, model.LastName, model.Email, model.Password,
                    model.Status, model.Phone, model.Photo);
                _db.Users.Add(newUser);
                _db.SaveChanges();
            });
        }

        public bool Update(UserModel model, int id)
        {
            User userForUpdate = _db.Users.FirstOrDefault(u => u.Id == id);
            if (userForUpdate != null)
            {
                return DoAction(delegate ()
            {
                userForUpdate.FirstName = model.FirstName;
                userForUpdate.LastName = model.LastName;
                userForUpdate.Email = model.Email;
                userForUpdate.Password = model.Password;
                userForUpdate.Phone = model.Phone;
                userForUpdate.Photo = model.Photo;
                userForUpdate.Status = model.Status;

                _db.Users.Update(userForUpdate);
                _db.SaveChanges();
            });
            }
            return false;

        }

        public bool Delete(int id)
        {
            User user = _db.Users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                return DoAction(delegate ()
                {
                    _db.Users.Remove(user);
                    _db.SaveChanges();
                });
            }
            return false;
        }

        public bool CreateMultipleUsers(List<UserModel> userModels)
        {
            return DoAction(delegate ()
            {
                    var newUsers = userModels.Select(u => new User(u));
                    _db.Users.AddRange(newUsers);
                    _db.SaveChangesAsync();
            });
        }

        public UserModel Get(int id)
        {
            User user = _db.Users.FirstOrDefault(p => p.Id == id);
            return user?.ToDto();
        }

        public IEnumerable<UserModel> GetAllByIds(List<int> usersIds)
        {
            foreach (int id in usersIds)
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == id).ToDto();
                yield return user;
            }
        }
    }
}
