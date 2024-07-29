using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD_Personal_portfolio
{
    internal class Student
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Name { get; set; }

        public int StudentNumber { get; set; }

        public List<string> Availability { get; set; }
        public string SupervisorCode { get; set; }

        public Student(string username, string password, string name, int studentNumber, List<string> availability, string supervisorCode) 
        { 
            Username = username;
            Password = password;
            Name = name;
            SupervisorCode = supervisorCode;
            Availability = availability;
            StudentNumber = studentNumber;
        }
    }
}
