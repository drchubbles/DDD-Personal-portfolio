// See https://aka.ms/new-console-template for more information
using System.Xml.Linq;
using System.Xml;
using DDD_Personal_portfolio;
using System.Reflection.Metadata;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Globalization;

Boolean debug = false;
Boolean ShowAll = false;
Boolean looplockTrue = true;

XmlDocument xmlDoc = new XmlDocument();
XmlDocument xmlDoc2 = new XmlDocument();

try
{
    xmlDoc.Load("_Information.xml");
    xmlDoc2.Load("_StudentReports.xml");
}
catch (Exception ex)
{
    Console.WriteLine("Error loading XML files: " + ex.Message);
    return;
}

XmlNodeList studentNodes = xmlDoc.SelectNodes("/root/Student");
XmlNodeList psNodes = xmlDoc.SelectNodes("/root/PersonalSupervisor");
XmlNodeList stNodes = xmlDoc.SelectNodes("/root/SeniorTutor");
XmlNodeList studentReportNodes = xmlDoc2.SelectNodes("/root/StudentReport");
XmlNodeList meetingNodes = xmlDoc.SelectNodes("/root/Meeting");

List<Student> students = new List<Student>();
List<PersonalSupervisor> personalSupervisors = new List<PersonalSupervisor>();
List<SeniorTutor> seniorTutors = new List<SeniorTutor>();
List<Meeting> meetings = new List<Meeting>();
List<StudentReport> studentReports = new List<StudentReport>();

foreach (XmlNode studentNode in studentNodes)
{
    try
    {
        string username = studentNode.Attributes["username"].Value;
        string password = studentNode.SelectSingleNode("Password").InnerText;
        string name = studentNode.SelectSingleNode("Name").InnerText;
        string supervisorCode = studentNode.SelectSingleNode("SupervisorCode").InnerText;
        int studentNumber = int.Parse(studentNode.SelectSingleNode("StudentNumber").InnerText);
        List<string> Availability = new List<string>();

        XmlNodeList hoursNodes = studentNode.SelectNodes("Availability/string");
        foreach (XmlNode hourNode in hoursNodes)
        {
            Availability.Add(hourNode.InnerText);
        }

        Student student = new Student(username, password, name, studentNumber, Availability, supervisorCode);
        students.Add(student);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error processing student node: " + ex.Message);
    }
}

foreach (XmlNode psNode in psNodes)
{
    try
    {
        string username = psNode.Attributes["username"].Value;
        string password = psNode.SelectSingleNode("Password").InnerText;
        string name = psNode.SelectSingleNode("Name").InnerText;
        string supervisorCode = psNode.SelectSingleNode("SupervisorCode").InnerText;
        int amountOfMeetingRequests = 0;
        List<string> Availability = new List<string>();

        XmlNodeList hoursNodes = psNode.SelectNodes("Availability/string");
        foreach (XmlNode hourNode in hoursNodes)
        {
            Availability.Add(hourNode.InnerText);
        }

        PersonalSupervisor personalSupervisor = new PersonalSupervisor(username, password, name, supervisorCode, amountOfMeetingRequests, Availability);
        personalSupervisors.Add(personalSupervisor);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error processing personal supervisor node: " + ex.Message);
    }
}

foreach (XmlNode stNode in stNodes)
{
    try
    {
        string username = stNode.Attributes["username"].Value;
        string password = stNode.SelectSingleNode("Password").InnerText;
        string name = stNode.SelectSingleNode("Name").InnerText;
        SeniorTutor seniorTutor = new SeniorTutor(username, password, name);
        seniorTutors.Add(seniorTutor);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error processing senior tutor node: " + ex.Message);
    }
}

foreach (XmlNode studentReportNode in studentReportNodes)
{
    try
    {
        int studentnumber = int.Parse(studentReportNode.SelectSingleNode("StudentNumber").InnerText);
        string name = studentReportNode.SelectSingleNode("Name").InnerText;
        int rating = int.Parse(studentReportNode.SelectSingleNode("Rating").InnerText);
        string userResponse = studentReportNode.SelectSingleNode("UserResponse").InnerText;
        string userAdditionalResponse = studentReportNode.SelectSingleNode("UserAdditionalResponse").InnerText;
        string time = studentReportNode.SelectSingleNode("Time").InnerText;

        StudentReport studentReport = new StudentReport(studentnumber, name, rating, userResponse, userAdditionalResponse, time);
        studentReports.Add(studentReport);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error processing student report node: " + ex.Message);
    }
}

foreach (XmlNode meetingNode in meetingNodes)
{
    try
    {
        string supervisorCode = meetingNode.SelectSingleNode("SupervisorCode").InnerText;
        int studentNumber = int.Parse(meetingNode.SelectSingleNode("StudentNumber").InnerText);
        string meetingRegarding = meetingNode.SelectSingleNode("MeetingRegarding").InnerText;
        string time = meetingNode.SelectSingleNode("Time").InnerText;

        Meeting meeting = new Meeting(supervisorCode, studentNumber, meetingRegarding, time);
        meetings.Add(meeting);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error processing meeting node: " + ex.Message);
    }
}

if (ShowAll == true)
{
    Console.WriteLine("Students:");
    foreach (Student student in students)
    {
        Console.WriteLine($"Username: {student.Username}, Password: {student.Password}, Name: {student.Name}, Supervisor Code: {student.SupervisorCode}");
    }
    Console.WriteLine("Personal Supervisors:");
    foreach (PersonalSupervisor personalSupervisor in personalSupervisors)
    {
        Console.WriteLine($"Username: {personalSupervisor.Username}, Password: {personalSupervisor.Password}, Name: {personalSupervisor.Name}, Supervisor Code: {personalSupervisor.SupervisorCode}");
    }
    Console.WriteLine("Senior Tutors:");
    foreach (SeniorTutor seniorTutor in seniorTutors)
    {
        Console.WriteLine($"Username: {seniorTutor.Username}, Password: {seniorTutor.Password}, Name: {seniorTutor.Name}");
    }
}
else if (debug == true)
{
    Console.WriteLine("For an easy view of student and how to interact - Enter a as the username and A as the password.");
    Console.WriteLine("For an easy view of Personal Supervisor and how to interact - Enter b as the username and B as the password.");
    Console.WriteLine("For an easy view of Senior Tutor  - Enter c as the username and C as the password.");
    Console.WriteLine("To view the difference in handeling of the students with/without a booked meeting, - enter DavidWebb as the Username and David as the password..");
}

Boolean isAuthenticated = false;

while (!isAuthenticated)
{
    try
    {
        Console.WriteLine("Please enter your username:");
        string inputUsername = Console.ReadLine();
        Console.WriteLine("Please enter your Password:");
        string inputPassword = Console.ReadLine();

        foreach (Student student in students)
        {
            if (inputUsername == student.Username && inputPassword == student.Password)
            {
                isAuthenticated = true;
                Student User = student;
                Console.Clear();
                if (debug) Console.WriteLine("Successful Login: Booting up appropriate interface");
                StudentUserInterface(User);
                break;
            }
        }

        foreach (PersonalSupervisor personalSupervisor in personalSupervisors)
        {
            if (inputUsername == personalSupervisor.Username && inputPassword == personalSupervisor.Password)
            {
                isAuthenticated = true;
                PersonalSupervisor User = personalSupervisor;
                Console.Clear();
                if (debug) Console.WriteLine("Successful Login: Booting up appropriate interface");
                PersSupInterface(User);
                break;
            }
        }

        foreach (SeniorTutor seniorTutor in seniorTutors)
        {
            if (inputUsername == seniorTutor.Username && inputPassword == seniorTutor.Password)
            {
                isAuthenticated = true;
                SeniorTutor User = seniorTutor;
                Console.Clear();
                if (debug) Console.WriteLine("Successful Login: Booting up appropriate interface");
                SenTutInterface(User);
                break;
            }
        }

        if (isAuthenticated == false)
        {
            Console.WriteLine("Invalid Username or Password. Please try again.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error during authentication: " + ex.Message);
    }
}

void StudentUserInterface(Student student)
{
    while (looplockTrue)
    {
        try
        {
            Console.WriteLine($"Welcome to My Personal Supervisor Manager student!");
            Console.WriteLine("Please choose from the options:");
            Console.WriteLine("1. Self Report how you are feeling / progressing.");
            Console.WriteLine("2. Book a meeting with your personal supervisor.");
            Console.WriteLine("3. View booked meetings.");
            Console.WriteLine("4. Update availability.");
            Console.WriteLine("5. Remove availability time.");
            Console.WriteLine("6. Exit the program.");

            String userResponse = Console.ReadLine();

            if (userResponse == "1")
            {
                StudentStatusReport(student);
            }
            else if (userResponse == "2")
            {
                BookappointmentSt(student);
            }
            else if (userResponse == "3")
            {
                ViewBookedMeetingStu(student);
            }
            else if (userResponse == "4")
            {
                UpdateAvailabilityStudent(student);
            }
            else if (userResponse == "5")
            {
                RemoveAvailabilityTimeStu(student);
            }
            else if (userResponse == "6")
            {
                looplockTrue = false;
                Console.WriteLine("Exiting the program...");
                Thread.Sleep(3000);
                System.Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 6.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in student user interface: " + ex.Message);
        }
    }
}

void PersSupInterface(PersonalSupervisor personalSupervisor)
{
    while (looplockTrue)
    {
        try
        {
            Console.WriteLine("Welcome to My Personal Supervisor Manager Personal Supervisor!");
            Console.WriteLine("Please choose from the options:");
            Console.WriteLine("1. Review your students Status.");
            Console.WriteLine("2. Book Meetings.");
            Console.WriteLine("3. View booked meetings.");
            Console.WriteLine("4. Update availability.");
            Console.WriteLine("5. Remove availability time.");
            Console.WriteLine("6. Exit the program.");

            String userResponse = Console.ReadLine();

            if (userResponse == "1")
            {
                reviewStudentStatus(personalSupervisor);
            }
            else if (userResponse == "2")
            {
                BookappointmentPs(personalSupervisor);
            }
            else if (userResponse == "3")
            {
                ViewBookedMeetingsPs(personalSupervisor);
            }
            else if (userResponse == "4")
            {
                UpdateAvailabilityPs(personalSupervisor);
            }
            else if (userResponse == "5")
            {
                RemoveAvailabilityTimePs(personalSupervisor);
            }
            else if (userResponse == "6")
            {
                looplockTrue = false;
                Console.WriteLine("Exiting the program...");
                Thread.Sleep(3000);
                System.Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 6.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in personal supervisor user interface: " + ex.Message);
        }
    }
}

void SenTutInterface(SeniorTutor seniorTutor)
{
    while (looplockTrue)
    {
        try
        {
            Console.WriteLine("Welcome to My Senior Tutor Manager Senior Tutor!");
            Console.WriteLine("Please choose from the options:");
            Console.WriteLine("1. View booked meetings between personal supervisors and students.");
            Console.WriteLine("2. View student report based on personal supervisor.");
            Console.WriteLine("3. Exit the program.");

            String userResponse = Console.ReadLine();

            if (userResponse == "1")
            {
                ViewBookedMeetingsSenTut(seniorTutor);
            }
            else if (userResponse == "2")
            {
                reviewAllStudentStatus(seniorTutor);
            }
            else if (userResponse == "3")
            {
                looplockTrue = false;
                Console.WriteLine("Exiting the program...");
                Thread.Sleep(3000);
                System.Environment.Exit(0);
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter 1, 2 or 3");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error in senior tutor user interface: " + ex.Message);
        }
    }
}

void ViewBookedMeetingsPs(PersonalSupervisor personalSupervisor)
{
    try
    {
        Console.WriteLine($"Meetings for Personal Supervisor: {personalSupervisor.Name}");

        List<Meeting> supervisorMeetings = meetings.Where(m => m.SupervisorCode == personalSupervisor.SupervisorCode).ToList();

        if (supervisorMeetings.Count == 0)
        {
            Console.WriteLine("No meetings found.");
            return;
        }

        for (int i = 0; i < supervisorMeetings.Count; i++)
        {
            Student student = students.FirstOrDefault(s => s.StudentNumber == supervisorMeetings[i].StudentNumber);
            Console.WriteLine($"{i + 1}. Meeting with {student.Name} (Student Number: {supervisorMeetings[i].StudentNumber}) at {supervisorMeetings[i].Time}. Regarding: {supervisorMeetings[i].MeetingRegarding}");
        }

        Console.WriteLine("Enter the number of the meeting you want to cancel, or enter 0 to go back:");
        int selectedMeetingIndex = int.Parse(Console.ReadLine()) - 1;

        if (selectedMeetingIndex == -1)
        {
            return;
        }

        if (selectedMeetingIndex < 0 || selectedMeetingIndex >= supervisorMeetings.Count)
        {
            Console.WriteLine("Invalid selection.");
            return;
        }

        Meeting meetingToCancel = supervisorMeetings[selectedMeetingIndex];
        Console.WriteLine($"Are you sure you want to cancel the meeting with student number {meetingToCancel.StudentNumber} at {meetingToCancel.Time}? (yes/no)");

        if (Console.ReadLine().ToLower() == "yes")
        {
            CancelMeeting(meetingToCancel);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error in viewing booked meetings for personal supervisor: " + ex.Message);
    }
}

void ViewBookedMeetingStu(Student student)
{
    try
    {
        Console.WriteLine($"Meetings for Student: {student.Name}");

        List<Meeting> studentMeetings = meetings.Where(m => m.StudentNumber == student.StudentNumber).ToList();

        if (studentMeetings.Count == 0)
        {
            Console.WriteLine("No meetings found.");
            return;
        }

        for (int i = 0; i < studentMeetings.Count; i++)
        {
            PersonalSupervisor ps = personalSupervisors.FirstOrDefault(p => p.SupervisorCode == studentMeetings[i].SupervisorCode);
            Console.WriteLine($"{i + 1}. Meeting with {ps.Name} (Supervisor Code: {studentMeetings[i].SupervisorCode}) at {studentMeetings[i].Time}. Regarding: {studentMeetings[i].MeetingRegarding}");
        }

        Console.WriteLine("Enter the number of the meeting you want to cancel, or enter 0 to go back:");
        int selectedMeetingIndex = int.Parse(Console.ReadLine()) - 1;

        if (selectedMeetingIndex == -1)
        {
            return;
        }

        if (selectedMeetingIndex < 0 || selectedMeetingIndex >= studentMeetings.Count)
        {
            Console.WriteLine("Invalid selection.");
            return;
        }

        Meeting meetingToCancel = studentMeetings[selectedMeetingIndex];
        Console.WriteLine($"Are you sure you want to cancel the meeting with supervisor code {meetingToCancel.SupervisorCode} at {meetingToCancel.Time}? (yes/no)");

        if (Console.ReadLine().ToLower() == "yes")
        {
            CancelMeeting(meetingToCancel);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error in viewing booked meetings for student: " + ex.Message);
    }
}

void ViewBookedMeetingsSenTut(SeniorTutor seniorTutor)
{
    try
    {
        Console.WriteLine("List of Personal Supervisors:");
        for (int i = 0; i < personalSupervisors.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {personalSupervisors[i].Name}");
        }

        Console.WriteLine("Please select a Personal Supervisor by entering the corresponding number:");
        int psIndex = int.Parse(Console.ReadLine()) - 1;

        if (psIndex < 0 || psIndex >= personalSupervisors.Count)
        {
            Console.WriteLine("Invalid selection. Please try again.");
            return;
        }

        PersonalSupervisor selectedPS = personalSupervisors[psIndex];

        Console.WriteLine($"Meetings for Personal Supervisor: {selectedPS.Name}");

        List<Meeting> supervisorMeetings = meetings.Where(m => m.SupervisorCode == selectedPS.SupervisorCode).ToList();

        if (supervisorMeetings.Count == 0)
        {
            Console.WriteLine("No meetings found.");
            return;
        }

        for (int i = 0; i < supervisorMeetings.Count; i++)
        {
            Student student = students.FirstOrDefault(s => s.StudentNumber == supervisorMeetings[i].StudentNumber);
            Console.WriteLine($"{i + 1}. Meeting with {student.Name} (Student Number: {supervisorMeetings[i].StudentNumber}) at {supervisorMeetings[i].Time}. Regarding: {supervisorMeetings[i].MeetingRegarding}");
        }

        Console.WriteLine("Press Any Key in order to return to the main display");
        Console.ReadKey();
        Console.Clear();
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error in viewing booked meetings for senior tutor: " + ex.Message);
    }
}

void UpdateAvailabilityPs(PersonalSupervisor personalSupervisor)
{
    try
    {
        Console.WriteLine("Please enter either 1 for AM or 2 for PM.");
        int selection = 0;
        string AmOrPm = null;
        do
        {
            try
            {
                selection = int.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Please enter a valid number.");
            }

            if (selection < 1 || selection > 2)
            {
                Console.WriteLine("Invalid input. Please enter either 1 or 2.");
            }
            else if (selection == 1)
            {
                AmOrPm = "AM";
            }
            else if (selection == 2)
            {
                AmOrPm = "PM";
            }
        } while (AmOrPm == null);

        int hour = 0;
        bool isValidHour = false;
        bool ValidHour = false;
        do
        {
            try
            {
                Console.WriteLine("Please enter an hour between 1 and 12:");
                isValidHour = int.TryParse(Console.ReadLine(), out hour);
            }
            catch
            {
                Console.WriteLine("please enter a number.");
            }

            if (!isValidHour || hour < 1 || hour > 12)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 12.");
            }
            else
            {
                ValidHour = true;
            }
        } while (ValidHour == false);

        Console.WriteLine("Please select the time you would like to add to your availability below");

        int[] minuteIntervals = { 00, 10, 20, 30, 40, 50 };

        for (int i = 0; i < minuteIntervals.Length; i++)
        {
            string option = $"{hour}:{minuteIntervals[i]:D2}{AmOrPm}";
            Console.WriteLine($"{i + 1}: {option}");
        }

        bool FinalTimeSet = false;

        do
        {
            int userInput = 0;
            string selectedOption = null;
            bool validchoice = false;
            do
            {
                try
                {
                    Console.Write("Please enter the number of your choice: ");
                    userInput = int.Parse(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Please enter a valid number.");
                }

                if (userInput > 0 && userInput <= minuteIntervals.Length)
                {
                    if (minuteIntervals[userInput - 1] == 00)
                    {
                        selectedOption = $"{hour}{AmOrPm}";
                        validchoice = true;
                    }
                    else
                    {
                        selectedOption = $"{hour}:{minuteIntervals[userInput - 1]:D2}{AmOrPm}";
                        validchoice = true;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. select a valid option ");
                }
            } while (validchoice == false);

            if (selectedOption != null)
            {
                string newAvailability = selectedOption;
                personalSupervisor.Availability.Add(newAvailability);
                Console.WriteLine($"New availability {newAvailability} added.");
                FinalTimeSet = true;

                string path = Directory.GetCurrentDirectory();
                string truePath = path.Remove(path.Length - 17);
                truePath = truePath + @"\_Information.xml";

                XDocument xmlDoc = XDocument.Load(truePath);

                XElement psElement = xmlDoc.Descendants("PersonalSupervisor")
                                           .FirstOrDefault(e => e.Element("SupervisorCode")?.Value == personalSupervisor.SupervisorCode);

                if (psElement != null)
                {
                    XElement availabilityElement = psElement.Element("Availability");
                    if (availabilityElement != null)
                    {
                        availabilityElement.Add(new XElement("string", newAvailability));
                    }
                    else
                    {
                        availabilityElement = new XElement("Availability", new XElement("string", newAvailability));
                        psElement.Add(availabilityElement);
                    }
                }

                xmlDoc.Save(truePath);
            }
        } while (FinalTimeSet = false);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error in updating availability for personal supervisor: " + ex.Message);
    }
}

void BookappointmentSt(Student student)
{
    try
    {
        PersonalSupervisor ps = personalSupervisors.FirstOrDefault(x => x.SupervisorCode == student.SupervisorCode);

        if (ps == null)
        {
            Console.WriteLine("No personal supervisor found for the given student.");
            return;
        }

        List<string> commonAvailableTimes = ps.Availability.Intersect(student.Availability).ToList();

        if (commonAvailableTimes.Count == 0)
        {
            Console.WriteLine("No common available times found.");
            return;
        }

        for (int i = 0; i < commonAvailableTimes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {commonAvailableTimes[i]}");
        }

        string selectedTime = commonAvailableTimes[int.Parse(Console.ReadLine()) - 1];

        Console.WriteLine("Please enter the reason for the meeting");
        string reasonForMeeting = Console.ReadLine();

        Meeting meeting = new Meeting(student.SupervisorCode, student.StudentNumber, reasonForMeeting, selectedTime);

        Console.WriteLine($"To confirm - You would like to suggest a Meeting for {meeting.Time} with Your Personal Supervisor for the following reason:");
        Console.WriteLine(reasonForMeeting);

        SaveMeetingToXml(meeting, ps, student, selectedTime);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error in booking appointment for student: " + ex.Message);
    }
}

void BookappointmentPs(PersonalSupervisor personalSupervisor, Student student = null)
{
    try
    {
        Student selectedStudent;
        if (student == null)
        {
            Console.WriteLine("Please choose a student from the list below you wish to meet:");
            List<Student> matchingStudents = students.Where(x => x.SupervisorCode == personalSupervisor.SupervisorCode).ToList();

            for (int i = 0; i < matchingStudents.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {matchingStudents[i].Name}");
            }

            selectedStudent = matchingStudents[int.Parse(Console.ReadLine()) - 1];
        }
        else
        {
            selectedStudent = student;
        }

        List<string> commonAvailableTimes = personalSupervisor.Availability.Intersect(selectedStudent.Availability).ToList();

        if (commonAvailableTimes.Count == 0)
        {
            Console.WriteLine("No common available times found.");
            return;
        }

        for (int i = 0; i < commonAvailableTimes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {commonAvailableTimes[i]}");
        }

        string selectedTime = commonAvailableTimes[int.Parse(Console.ReadLine()) - 1];

        Console.WriteLine("Please enter the reason for the meeting");
        string reasonForMeeting = Console.ReadLine();

        Meeting meeting = new Meeting(selectedStudent.SupervisorCode, selectedStudent.StudentNumber, reasonForMeeting, selectedTime);

        Console.WriteLine($"To confirm - You would like to suggest a Meeting for {meeting.Time} with {selectedStudent.Name} for the following reason:");
        Console.WriteLine(reasonForMeeting);

        SaveMeetingToXml(meeting, personalSupervisor, selectedStudent, selectedTime);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error in booking appointment for personal supervisor: " + ex.Message);
    }
}

void SaveMeetingToXml(Meeting meeting, PersonalSupervisor ps, Student student, string selectedTime)
{
    try
    {
        XmlDocument xmlDocs = new XmlDocument();
        xmlDocs.Load("_Information.xml");

        XmlNode Meeting = xmlDocs.CreateElement("Meeting");
        XmlNode SupervisorCode = xmlDocs.CreateElement("SupervisorCode");
        XmlNode StudentNumber = xmlDocs.CreateElement("StudentNumber");
        XmlNode MeetingRegarding = xmlDocs.CreateElement("MeetingRegarding");
        XmlNode Time = xmlDocs.CreateElement("Time");

        SupervisorCode.InnerText = meeting.SupervisorCode.ToString();
        StudentNumber.InnerText = meeting.StudentNumber.ToString();
        MeetingRegarding.InnerText = meeting.MeetingRegarding.ToString();
        Time.InnerText = meeting.Time.ToString();
        Meeting.AppendChild(SupervisorCode);
        Meeting.AppendChild(StudentNumber);
        Meeting.AppendChild(MeetingRegarding);
        Meeting.AppendChild(Time);
        xmlDocs.DocumentElement.AppendChild(Meeting);
        string pathMeeting = Directory.GetCurrentDirectory();

        string truePathMeeting = pathMeeting.Remove(pathMeeting.Length - 17);

        truePathMeeting = truePathMeeting + @"\_Information.xml";

        ps.Availability.Remove(selectedTime);
        student.Availability.Remove(selectedTime);
        XDocument xmlDoc = XDocument.Load(truePathMeeting);

        XElement psElement = xmlDoc.Descendants("PersonalSupervisor").FirstOrDefault(e => e.Element("SupervisorCode")?.Value == ps.SupervisorCode);
        XElement studentElement = xmlDoc.Descendants("Student").FirstOrDefault(e => e.Element("StudentNumber")?.Value == student.StudentNumber.ToString());

        if (psElement != null)
        {
            XElement availabilityElement = psElement.Element("Availability");

            if (availabilityElement != null)
            {
                XElement timeElement = availabilityElement.Elements("string").FirstOrDefault(e => e.Value == selectedTime);

                timeElement?.Remove();
                xmlDoc.Save(truePathMeeting);
            }
        }
        if (studentElement != null)
        {
            XElement availabilityElement = studentElement.Element("Availability");

            if (availabilityElement != null)
            {
                XElement timeElement = availabilityElement.Elements("string").FirstOrDefault(e => e.Value == selectedTime);

                timeElement?.Remove();
                xmlDoc.Save(truePathMeeting);
            }
        }
        xmlDoc.Save(truePathMeeting);
        xmlDocs.Save(truePathMeeting);
        xmlDoc.Save("_Information.xml");
        xmlDocs.Save("_Information.xml");

        Console.WriteLine("The Meeting has been set.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error in saving meeting to XML: " + ex.Message);
    }
}

void reviewStudentStatus(PersonalSupervisor personalSupervisor)
{
    try
    {
        Console.WriteLine("Would you like to see:");
        Console.WriteLine("1. A list of all students");
        Console.WriteLine("2. A list of students needing a meeting (most recent report score 4 or below)");

        int choice = int.Parse(Console.ReadLine());

        List<Student> matchingStudents = new List<Student>();

        foreach (Student x in students)
        {
            if (x.SupervisorCode == personalSupervisor.SupervisorCode)
            {
                if (choice == 1)
                {
                    matchingStudents.Add(x);
                }
                else if (choice == 2)
                {
                    StudentReport mostRecentReport = null;
                    DateTime mostRecentTime = DateTime.MinValue;

                    foreach (StudentReport report in studentReports)
                    {
                        if (report.StudentNumber == x.StudentNumber)
                        {
                            string debug = report.Name;
                            int debug2 = report.StudentNumber;
                            DateTime reportTime = DateTime.ParseExact(report.Time, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            if (reportTime > mostRecentTime)
                            {
                                mostRecentReport = report;
                                mostRecentTime = reportTime;
                            }
                        }
                    }

                    if (mostRecentReport != null && mostRecentReport.Progressing <= 4)
                    {
                        matchingStudents.Add(x);
                    }
                }
            }
        }

        Console.WriteLine("Please choose a student from the list below you wish to view the status of:");
        for (int i = 0; i < matchingStudents.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {matchingStudents[i].Name}");
        }

        Student selectedStudent = matchingStudents[int.Parse(Console.ReadLine()) - 1];

        Console.WriteLine("Do you want to view:");
        Console.WriteLine("1. All feedback reports");
        Console.WriteLine("2. Only the most recent feedback report");

        int viewChoice = int.Parse(Console.ReadLine());

        int numberOfReports = 0;
        StudentReport mostRecentReportView = null;
        DateTime mostRecentTimeView = DateTime.MinValue;

        foreach (StudentReport x in studentReports)
        {
            if (selectedStudent.StudentNumber == x.StudentNumber)
            {
                DateTime reportTime = DateTime.ParseExact(x.Time, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                if (viewChoice == 1)
                {
                    DisplayReport(x);
                }
                else if (viewChoice == 2)
                {
                    if (reportTime > mostRecentTimeView)
                    {
                        mostRecentReportView = x;
                        mostRecentTimeView = reportTime;
                    }
                }
                numberOfReports++;
            }
        }

        if (viewChoice == 2 && mostRecentReportView != null)
        {
            DisplayReport(mostRecentReportView);
        }

        if (numberOfReports == 0)
        {
            Console.WriteLine("This student has not given feedback yet.");
        }

        bool hasMeeting = meetings.Any(m => m.StudentNumber == selectedStudent.StudentNumber && m.SupervisorCode == personalSupervisor.SupervisorCode);

        if (!hasMeeting)
        {
            Console.WriteLine("Please select one of the following:");
            Console.WriteLine("1. Book a meeting");
            Console.WriteLine("2. Return to main screen");
        }
        else
        {
            Console.WriteLine("This student already has a meeting booked.");
            Console.WriteLine("Please select one of the following:");
            Console.WriteLine("2. Return to main screen");
        }

        bool looplock = false;
        while (!looplock)
        {
            try
            {
                int userchoice = int.Parse(Console.ReadLine());
                if (userchoice < 0 || userchoice > 2 || (hasMeeting && userchoice == 1))
                {
                    Console.WriteLine("That is an invalid choice, please choose again.");
                }
                else if (userchoice == 1)
                {
                    BookappointmentPs(personalSupervisor, selectedStudent);
                    looplock = true;
                }
                else if (userchoice == 2)
                {
                    looplock = true;
                    break;
                }
            }
            catch
            {
                Console.WriteLine("Please enter a number.");
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error in reviewing student status: " + ex.Message);
    }
}

void reviewAllStudentStatus(SeniorTutor seniorTutor)
{
    try
    {
        Console.WriteLine("List of Personal Supervisors:");
        for (int i = 0; i < personalSupervisors.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {personalSupervisors[i].Name}");
        }

        Console.WriteLine("Please select a Personal Supervisor by entering the corresponding number:");
        int psIndex = int.Parse(Console.ReadLine()) - 1;

        if (psIndex < 0 || psIndex >= personalSupervisors.Count)
        {
            Console.WriteLine("Invalid selection. Please try again.");
            return;
        }

        PersonalSupervisor selectedPS = personalSupervisors[psIndex];
        List<Student> matchingStudents = students.Where(s => s.SupervisorCode == selectedPS.SupervisorCode).ToList();

        if (matchingStudents.Count == 0)
        {
            Console.WriteLine("This Personal Supervisor has no students assigned.");
            return;
        }

        Console.WriteLine($"Students under {selectedPS.Name}:");
        for (int i = 0; i < matchingStudents.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {matchingStudents[i].Name}");
        }

        Console.WriteLine("Please select a Student by entering the corresponding number:");
        int studentIndex = int.Parse(Console.ReadLine()) - 1;

        if (studentIndex < 0 || studentIndex >= matchingStudents.Count)
        {
            Console.WriteLine("Invalid selection. Please try again.");
            return;
        }

        Student selectedStudent = matchingStudents[studentIndex];

        Console.WriteLine("Do you want to view:");
        Console.WriteLine("1. All feedback reports");
        Console.WriteLine("2. Only the most recent feedback report");
        int viewChoice = int.Parse(Console.ReadLine());

        int numberOfReports = 0;
        StudentReport mostRecentReport = null;
        DateTime mostRecentTime = DateTime.MinValue;

        foreach (StudentReport report in studentReports)
        {
            if (report.StudentNumber == selectedStudent.StudentNumber)
            {
                DateTime reportTime = DateTime.ParseExact(report.Time, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                if (viewChoice == 1)
                {
                    DisplayReport(report);
                }
                else if (viewChoice == 2)
                {
                    if (reportTime > mostRecentTime)
                    {
                        mostRecentReport = report;
                        mostRecentTime = reportTime;
                    }
                }
                numberOfReports++;
            }
        }

        if (viewChoice == 2 && mostRecentReport != null)
        {
            DisplayReport(mostRecentReport);
        }

        if (numberOfReports == 0)
        {
            Console.WriteLine("This student has not given feedback yet.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error in reviewing all student status: " + ex.Message);
    }
}

void DisplayReport(StudentReport report)
{
    try
    {
        int StudentProgressing = report.Progressing;
        Console.WriteLine($"Report taken from student at {report.Time}");
        Console.WriteLine("Your student is currently feeling like they are progressing at a rating of:");
        Console.WriteLine($"{report.Progressing}/10");
        Console.WriteLine("They have said that they are currently feeling:");
        Console.WriteLine($"{report.StudentFeelings}");
        Console.WriteLine("They have left the following additional information:");
        Console.WriteLine($"{report.AdditionalReport}");

        if (StudentProgressing < 4)
        {
            Console.WriteLine("The low progression score indicates that your student feels that they may be struggling, maybe book a meeting?");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error in displaying report: " + ex.Message);
    }
}

void StudentStatusReport(Student student)
{
    try
    {
        DateTime time = DateTime.Now;
        string currenttime = time.ToString();
        Boolean hasAnswered = false;
        int rating = 0;
        string UserResponse = "";
        string userAdditionalResponse = "No additional information";

        Console.WriteLine($"Hello {student.Name}, Please answer the questions below.");
        while (!hasAnswered)
        {
            Console.WriteLine("On a scale of 1 - 10 how do you feel you are progressing?");
            try
            {
                rating = 0;
                try
                {
                    rating = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.WriteLine("Please enter a valid number such as 1, 2, 3, etc.");
                }
                if (rating < 0 || rating > 10)
                {
                    Console.WriteLine("Please enter a number between 1 and 10.");
                }
                else if (rating >= 1 && rating <= 10)
                {
                    if (debug) Console.WriteLine($"Entering number {rating} into database for student {student.Name}");
                    hasAnswered = true;
                }
                else
                {
                    Console.WriteLine("Invalid Input, Please try again.");
                }
            }
            catch (Exception ex)
            {
                if (debug) Console.WriteLine(ex.Message);
            }
        }
        hasAnswered = false;

        while (!hasAnswered)
        {
            Console.WriteLine("Please fill this section with how you are feeling, please try and expand on as much as possible as the more information your personal superviser has the better."); ;
            try
            {
                string UserInput = Console.ReadLine();

                Console.WriteLine("To confirm, you have reported that you are feeling:");
                Console.WriteLine(UserInput);
                Console.WriteLine("If this message seems incorrect or you wish to change it, Please say no, otherwise, say yes");
                string confirmation = Console.ReadLine().ToLower();

                if (confirmation == "yes")
                {
                    UserResponse = UserInput;
                    if (debug) Console.WriteLine("Writing the user response to the database");
                    hasAnswered = true;
                }
                else if (confirmation == "no")
                {
                    Console.WriteLine("Please resubmit your changed response to the following Request:");
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
                }
            }
            catch (Exception ex)
            {
                if (debug) Console.WriteLine(ex.Message);
            }
        }
        hasAnswered = false;

        while (!hasAnswered)
        {
            try
            {
                Console.WriteLine("Do you have anything else you would like to report, if so please say yes, otherwise, say no");
                string confirmation = Console.ReadLine().ToLower();

                if (confirmation == "yes")
                {
                    Console.WriteLine("Please enter your additional report here:");
                    userAdditionalResponse = Console.ReadLine();

                    if (debug) Console.WriteLine("Writing the user response to the database");
                    hasAnswered = true;
                }
                else if (confirmation == "no")
                {
                    hasAnswered = true;
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter 'yes' or 'no'.");
                }
            }
            catch (Exception ex)
            {
                if (debug) Console.WriteLine(ex.Message);
            }

            if (rating <= 4)
            {
                Console.WriteLine("We Recommend you next update in 1 weeks time.");
            }
            else if (rating <= 7 && rating > 4)
            {
                Console.WriteLine("We Recommend you next update in 2 weeks time.");
            }
            else if (rating <= 10 && rating > 7)
            {
                Console.WriteLine("We Recommend you next update in 4 weeks time.");
            }

            StudentReport studentReport = new StudentReport(student.StudentNumber, student.Name, rating, UserResponse, userAdditionalResponse, currenttime);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("_StudentReports.xml");

            XmlNode StudentReport = xmlDoc.CreateElement("StudentReport");
            XmlNode studentnumber = xmlDoc.CreateElement("StudentNumber");
            XmlNode name = xmlDoc.CreateElement("Name");
            XmlNode ratings = xmlDoc.CreateElement("Rating");
            XmlNode userresponse = xmlDoc.CreateElement("UserResponse");
            XmlNode useradditionalresponse = xmlDoc.CreateElement("UserAdditionalResponse");
            XmlNode timeofreport = xmlDoc.CreateElement("Time");
            studentnumber.InnerText = studentReport.StudentNumber.ToString();
            name.InnerText = studentReport.Name.ToString();
            ratings.InnerText = studentReport.Progressing.ToString();
            userresponse.InnerText = studentReport.StudentFeelings.ToString();
            useradditionalresponse.InnerText = studentReport.AdditionalReport.ToString();
            timeofreport.InnerText = studentReport.Time.ToString();

            StudentReport.AppendChild(studentnumber);
            StudentReport.AppendChild(name);
            StudentReport.AppendChild(ratings);
            StudentReport.AppendChild(userresponse);
            StudentReport.AppendChild(useradditionalresponse);
            StudentReport.AppendChild(timeofreport);

            xmlDoc.DocumentElement.AppendChild(StudentReport);

            string path = Directory.GetCurrentDirectory();
            string truePath = path.Remove(path.Length - 17);
            truePath = truePath + @"\_StudentReports.xml";

            xmlDoc.Save(truePath);

            if (debug) Console.WriteLine($"New student report added to XML document.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error in student status report: " + ex.Message);
    }
}

void CancelMeeting(Meeting meeting)
{
    try
    {
        Meeting meetingToCancel = meetings.FirstOrDefault(m => m.SupervisorCode == meeting.SupervisorCode && m.StudentNumber == meeting.StudentNumber && m.Time == meeting.Time);

        if (meetingToCancel != null)
        {
            meetings.Remove(meetingToCancel);

            Student student = students.FirstOrDefault(s => s.StudentNumber == meeting.StudentNumber);
            PersonalSupervisor personalSupervisor = personalSupervisors.FirstOrDefault(p => p.SupervisorCode == meeting.SupervisorCode);

            if (student != null && personalSupervisor != null)
            {
                student.Availability.Add(meeting.Time);
                personalSupervisor.Availability.Add(meeting.Time);

                string pathMeeting = Directory.GetCurrentDirectory();
                string truePathMeeting = pathMeeting.Remove(pathMeeting.Length - 17) + @"\_Information.xml";

                XDocument xmlDoc = XDocument.Load(truePathMeeting);

                XElement meetingElement = xmlDoc.Descendants("Meeting")
                    .FirstOrDefault(e => e.Element("SupervisorCode")?.Value == meeting.SupervisorCode &&
                                          e.Element("StudentNumber")?.Value == meeting.StudentNumber.ToString() &&
                                          e.Element("Time")?.Value == meeting.Time);

                meetingElement?.Remove();

                XElement psElement = xmlDoc.Descendants("PersonalSupervisor")
                    .FirstOrDefault(e => e.Element("SupervisorCode")?.Value == meeting.SupervisorCode);
                XElement psAvailabilityElement = psElement?.Element("Availability");
                psAvailabilityElement?.Add(new XElement("string", meeting.Time));

                XElement studentElement = xmlDoc.Descendants("Student")
                    .FirstOrDefault(e => e.Element("StudentNumber")?.Value == meeting.StudentNumber.ToString());
                XElement studentAvailabilityElement = studentElement?.Element("Availability");
                studentAvailabilityElement?.Add(new XElement("string", meeting.Time));

                xmlDoc.Save(truePathMeeting);
            }

            Console.WriteLine("Meeting cancelled and availability updated.");
        }
        else
        {
            Console.WriteLine("Meeting not found.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error in canceling meeting: " + ex.Message);
    }
}

void UpdateAvailabilityStudent(Student student)
{
    try
    {
        Console.WriteLine("Please enter either 1 for AM or 2 for PM.");
        int selection = 0;
        string AmOrPm = null;
        do
        {
            try
            {
                selection = int.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Please enter a valid number.");
            }

            if (selection < 1 || selection > 2)
            {
                Console.WriteLine("Invalid input. Please enter either 1 or 2.");
            }
            else if (selection == 1)
            {
                AmOrPm = "AM";
            }
            else if (selection == 2)
            {
                AmOrPm = "PM";
            }
        } while (AmOrPm == null);

        int hour = 0;
        bool isValidHour = false;
        bool ValidHour = false;
        do
        {
            try
            {
                Console.WriteLine("Please enter an hour between 1 and 12:");
                isValidHour = int.TryParse(Console.ReadLine(), out hour);
            }
            catch
            {
                Console.WriteLine("Please enter a number.");
            }

            if (!isValidHour || hour < 1 || hour > 12)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 12.");
            }
            else
            {
                ValidHour = true;
            }
        } while (ValidHour == false);

        Console.WriteLine("Please select the time you would like to add to your availability below");

        int[] minuteIntervals = { 00, 10, 20, 30, 40, 50 };

        for (int i = 0; i < minuteIntervals.Length; i++)
        {
            string option = $"{hour}:{minuteIntervals[i]:D2}{AmOrPm}";
            Console.WriteLine($"{i + 1}: {option}");
        }

        bool FinalTimeSet = false;

        do
        {
            int userInput = 0;
            string selectedOption = null;
            bool validchoice = false;
            do
            {
                try
                {
                    Console.Write("Please enter the number of your choice: ");
                    userInput = int.Parse(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Please enter a valid number.");
                }

                if (userInput > 0 && userInput <= minuteIntervals.Length)
                {
                    if (minuteIntervals[userInput - 1] == 00)
                    {
                        selectedOption = $"{hour}{AmOrPm}";
                        validchoice = true;
                    }
                    else
                    {
                        selectedOption = $"{hour}:{minuteIntervals[userInput - 1]:D2}{AmOrPm}";
                        validchoice = true;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid choice. Select a valid option.");
                }
            } while (validchoice == false);

            if (selectedOption != null)
            {
                string newAvailability = selectedOption;
                student.Availability.Add(newAvailability);
                Console.WriteLine($"New availability {newAvailability} added.");
                FinalTimeSet = true;

                string path = Directory.GetCurrentDirectory();
                string truePath = path.Remove(path.Length - 17);
                truePath = truePath + @"\_Information.xml";

                XDocument xmlDoc = XDocument.Load(truePath);

                XElement studentElement = xmlDoc.Descendants("Student")
                    .FirstOrDefault(e => e.Element("StudentNumber")?.Value == student.StudentNumber.ToString());

                if (studentElement != null)
                {
                    XElement availabilityElement = studentElement.Element("Availability");
                    if (availabilityElement != null)
                    {
                        availabilityElement.Add(new XElement("string", newAvailability));
                    }
                    else
                    {
                        availabilityElement = new XElement("Availability", new XElement("string", newAvailability));
                        studentElement.Add(availabilityElement);
                    }
                }

                xmlDoc.Save(truePath);
            }
        } while (FinalTimeSet == false);
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error in updating availability for student: " + ex.Message);
    }
}

void RemoveAvailabilityTimeStu(Student student)
{
    try
    {
        Console.WriteLine("Your current availability:");
        for (int i = 0; i < student.Availability.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {student.Availability[i]}");
        }

        Console.WriteLine("Please select a time to remove by entering the corresponding number:");
        int selectedIndex = int.Parse(Console.ReadLine()) - 1;

        if (selectedIndex < 0 || selectedIndex >= student.Availability.Count)
        {
            Console.WriteLine("Invalid selection. Please try again.");
            return;
        }

        string timeToRemove = student.Availability[selectedIndex];
        student.Availability.RemoveAt(selectedIndex);

        string path = Directory.GetCurrentDirectory();
        string truePath = path.Remove(path.Length - 17) + @"\_Information.xml";

        XDocument xmlDoc = XDocument.Load(truePath);
        XElement studentElement = xmlDoc.Descendants("Student")
            .FirstOrDefault(e => e.Element("StudentNumber")?.Value == student.StudentNumber.ToString());
        XElement availabilityElement = studentElement?.Element("Availability");
        XElement timeElement = availabilityElement?.Elements("string").FirstOrDefault(e => e.Value == timeToRemove);
        timeElement?.Remove();

        xmlDoc.Save(truePath);

        Console.WriteLine($"Availability time {timeToRemove} removed.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error in removing availability time for student: " + ex.Message);
    }
}

void RemoveAvailabilityTimePs(PersonalSupervisor personalSupervisor)
{
    try
    {
        Console.WriteLine("Your current availability:");
        for (int i = 0; i < personalSupervisor.Availability.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {personalSupervisor.Availability[i]}");
        }

        Console.WriteLine("Please select a time to remove by entering the corresponding number:");
        int selectedIndex = int.Parse(Console.ReadLine()) - 1;

        if (selectedIndex < 0 || selectedIndex >= personalSupervisor.Availability.Count)
        {
            Console.WriteLine("Invalid selection. Please try again.");
            return;
        }

        string timeToRemove = personalSupervisor.Availability[selectedIndex];
        personalSupervisor.Availability.RemoveAt(selectedIndex);

        string path = Directory.GetCurrentDirectory();
        string truePath = path.Remove(path.Length - 17) + @"\_Information.xml";

        XDocument xmlDoc = XDocument.Load(truePath);
        XElement psElement = xmlDoc.Descendants("PersonalSupervisor")
            .FirstOrDefault(e => e.Element("SupervisorCode")?.Value == personalSupervisor.SupervisorCode);
        XElement availabilityElement = psElement?.Element("Availability");
        XElement timeElement = availabilityElement?.Elements("string").FirstOrDefault(e => e.Value == timeToRemove);
        timeElement?.Remove();

        xmlDoc.Save(truePath);

        Console.WriteLine($"Availability time {timeToRemove} removed.");
    }
    catch (Exception ex)
    {
        Console.WriteLine("Error in removing availability time for personal supervisor: " + ex.Message);
    }
}