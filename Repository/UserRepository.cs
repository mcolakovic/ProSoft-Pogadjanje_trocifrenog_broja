using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Repository
{
    public class UserRepository
    {
        private static UserRepository instance;
        List<User> users = new List<User>();
        public User CurrentUser { get; set; }

        private UserRepository()
        {
            users.Add(new User
            {
                Ime = "Pera",
                Prezime = "Peric",
                Email = "pera@gmail.com",
                Password = "123456"
            });
            users.Add(new User
            {
                Ime = "Mara",
                Prezime = "Maric",
                Email = "mara@gmail.com",
                Password = "654321"
            });
        }
        public static UserRepository Instance 
        {
            get
            {
                if (instance == null)
                    instance = new UserRepository();
                return instance;
            }
        }

        public User VratiUsera(User user)
        {
            return users.SingleOrDefault(x => x.Email == user.Email && x.Password == user.Password);
        }
    }
}
