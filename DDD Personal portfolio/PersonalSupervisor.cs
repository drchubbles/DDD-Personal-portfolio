using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD_Personal_portfolio
{
    public class PersonalSupervisor
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string SupervisorCode { get; set; }

        public int AmountOfMeetingRequests { get; set; }
        public List<string> Availability { get; set; }

        public PersonalSupervisor(string username, string password, string name, string supervisorCode, int amountOfMeetingRequests, List<string> availability)
        {
            Username = username;
            Password = password;
            Name = name;
            SupervisorCode = supervisorCode;
            AmountOfMeetingRequests = amountOfMeetingRequests;
            Availability = availability;
        }
    }
}
