using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDD_Personal_portfolio
{
    public class StudentReport
    {
        public int StudentNumber { get; set; }
        public string Name { get; set; }
        public int Progressing { get; set; }
        public string StudentFeelings { get; set; }
        public string AdditionalReport { get; set; }
        public string Time { get; set; }

        public StudentReport(int studentNumber, string name, int progressing, string studentFeelings, string addtionalReport, string time) 
        { 
            StudentNumber = studentNumber; 
            StudentFeelings = studentFeelings;
            AdditionalReport = addtionalReport;
            Progressing = progressing;
            Name = name;
            Time = time;
        }
    }
}
