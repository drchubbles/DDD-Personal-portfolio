using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD_Personal_portfolio
{
    public class SeniorTutor
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }

        public SeniorTutor(string username, string password, string name)
        {
            Username = username;
            Password = password;
            Name = name;
        }
    }
}
