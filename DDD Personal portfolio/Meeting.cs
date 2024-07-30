using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD_Personal_portfolio
{
    public class Meeting
    {
        public string SupervisorCode { get; set; }
        public int StudentNumber { get; set; }

        public string MeetingRegarding { get; set; }
        public string Time { get; set; }
        public Meeting(string supervisorCode, int studentNumber, string meetingRegarding, string time)
        {
            SupervisorCode = supervisorCode;
            StudentNumber = studentNumber;
            MeetingRegarding = meetingRegarding;
            Time = time;
        }
    }
}
