using System.Security.Claims;
using System.Text;
using TaskManagerCourse.Api.Controllers;
using TaskManagerCourse.Api.Models.Data;

namespace TaskManagerCourse.Api.Models.Services
{
    public class UserServices
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
    }
}
