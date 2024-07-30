
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using DDD_Personal_portfolio;
using System.Collections.Generic;
using System.Xml;

namespace UnitTesting
{
    [TestClass]
    public class PersonalPortfolioTests
    {
        public  List<Student> students;
        public List<PersonalSupervisor> personalSupervisors;
        public List<SeniorTutor> seniorTutors;
        public List<StudentReport> studentReports;
        public List<Meeting> meetings;

        [TestInitialize]
        public void Initialize()
        {
            // Initialize the lists for testing
            students = new List<Student>
            {
                new Student("a", "A", "Test Student", 1234, new List<string> { "9AM", "10AM" }, "A"),
                new Student("b", "B", "Another Student", 5678, new List<string> { "11AM", "12PM" }, "B")
            };

            personalSupervisors = new List<PersonalSupervisor>
            {
                new PersonalSupervisor("ps1", "PS1", "Test PS", "A", 0, new List<string> { "9AM", "10AM" }),
                new PersonalSupervisor("ps2", "PS2", "Another PS", "B", 0, new List<string> { "10AM", "12PM" })
            };

            seniorTutors = new List<SeniorTutor>
            {
                new SeniorTutor("st1", "ST1", "Test ST"),
                new SeniorTutor("st2", "ST2", "Another ST")
            };

            studentReports = new List<StudentReport>
            {
                new StudentReport(1234, "Test Student", 5, "Doing okay", "No additional info", DateTime.Now.ToString()),
                new StudentReport(5678, "Another Student", 7, "Doing well", "No additional info", DateTime.Now.ToString())
            };

            meetings = new List<Meeting>
            {
                new Meeting("A", 1234, "Progress Review", "9AM"),
                new Meeting("B", 5678, "Thesis Discussion", "11AM")
            };
        }

        [TestMethod]
        public void TestStudentLogin()
        {
            string username = "a";
            string password = "A";
            Student student = students.Find(s => s.Username == username && s.Password == password);
            Assert.IsNotNull(student);
            Assert.AreEqual("Test Student", student.Name);
        }

        [TestMethod]
        public void TestPersonalSupervisorLogin()
        {
            string username = "ps1";
            string password = "PS1";
            PersonalSupervisor ps = personalSupervisors.Find(p => p.Username == username && p.Password == password);
            Assert.IsNotNull(ps);
            Assert.AreEqual("Test PS", ps.Name);
        }

        [TestMethod]
        public void TestSeniorTutorLogin()
        {
            string username = "st1";
            string password = "ST1";
            SeniorTutor st = seniorTutors.Find(s => s.Username == username && s.Password == password);
            Assert.IsNotNull(st);
            Assert.AreEqual("Test ST", st.Name);
        }

        [TestMethod]
        public void TestStudentSelfReport()
        {
            Student student = students[0];
            int rating = 7;
            string userResponse = "Feeling good";
            string additionalResponse = "No issues";
            StudentReport report = new StudentReport(student.StudentNumber, student.Name, rating, userResponse, additionalResponse, DateTime.Now.ToString());
            studentReports.Add(report);
            StudentReport savedReport = studentReports.Find(r => r.StudentNumber == student.StudentNumber && r.Progressing == rating);
            Assert.IsNotNull(savedReport);
            Assert.AreEqual("Feeling good", savedReport.StudentFeelings);
        }

        [TestMethod]
        public void TestReviewStudentReports()
        {
            PersonalSupervisor ps = personalSupervisors[0];
            int studentNumber = 1234;
            List< StudentReport> reports = studentReports.FindAll(r => r.StudentNumber == studentNumber);
            Assert.IsTrue(reports.Count > 0);
        }

        [TestMethod]
        public void TestBookMeeting()
        {
            Student student = students[0];
            PersonalSupervisor ps = personalSupervisors[0];
            string selectedTime = "10AM";
            string reasonForMeeting = "Discussion about project";
            if (ps.Availability.Contains(selectedTime) && student.Availability.Contains(selectedTime))
            {
                var meeting = new Meeting(ps.SupervisorCode, student.StudentNumber, reasonForMeeting, selectedTime);
                meetings.Add(meeting);
                ps.Availability.Remove(selectedTime);
                student.Availability.Remove(selectedTime);
            }
            var bookedMeeting = meetings.Find(m => m.StudentNumber == student.StudentNumber && m.Time == selectedTime);
            Assert.IsNotNull(bookedMeeting);
            Assert.AreEqual("Discussion about project", bookedMeeting.MeetingRegarding);
        }

        [TestMethod]
        public void TestViewBookedMeetings()
        {
            var student = students[0];
            var studentMeetings = meetings.FindAll(m => m.StudentNumber == student.StudentNumber);
            Assert.IsTrue(studentMeetings.Count > 0);
        }
    }
}
