// See https://aka.ms/new-console-template for more information
using System.Xml.Linq;
using System.Xml;
using DDD_Personal_portfolio;
using System.Reflection.Metadata;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Runtime.InteropServices;

Console.WriteLine("Hello, World!");
Boolean debug = true;

Boolean looplockTrue = true;

// Load the XML document
XmlDocument xmlDoc = new XmlDocument();
xmlDoc.Load("Information.xml");

XmlDocument xmlDoc2 = new XmlDocument();
xmlDoc2.Load("StudentReports.xml");

// Select all <Student> nodes in the document
XmlNodeList studentNodes = xmlDoc.SelectNodes("/root/Student");
XmlNodeList psNodes = xmlDoc.SelectNodes("/root/PersonalSupervisor");
XmlNodeList studentReportNodes = xmlDoc2.SelectNodes("/root/StudentReport");

// Create a list to store the Personal suporvisor objects
List<Student> students = new List<Student>();
List<PersonalSupervisor> personalSupervisors = new List<PersonalSupervisor>();
List<StudentReport> studentReports = new List<StudentReport>();

// Iterate through each <Student> node and create Student objects
foreach (XmlNode studentNode in studentNodes)
{
    string username = studentNode.Attributes["username"].Value;
    string password = studentNode.SelectSingleNode("Password").InnerText;
    string name = studentNode.SelectSingleNode("Name").InnerText;
    string supervisorCode = studentNode.SelectSingleNode("SupervisorCode").InnerText;
    int studentNumber = int.Parse(studentNode.SelectSingleNode("StudentNumber").InnerText);

    // Create a new Student object and add it to the list
    Student student = new Student(username, password, name, studentNumber, supervisorCode);
    students.Add(student);
}

foreach (XmlNode psNode in psNodes)
{
    string username = psNode.Attributes["username"].Value;
    string password = psNode.SelectSingleNode("Password").InnerText;
    string name = psNode.SelectSingleNode("Name").InnerText;
    string supervisorCode = psNode.SelectSingleNode("SupervisorCode").InnerText;

    // Create a new Student object and add it to the list
    PersonalSupervisor personalSupervisor = new PersonalSupervisor(username, password, name, supervisorCode);
    personalSupervisors.Add(personalSupervisor);
}

foreach (XmlNode studentReportNode in studentReportNodes)
{
    int studentnumber = int.Parse(studentReportNode.SelectSingleNode("StudentNumber").InnerText);
    string name = studentReportNode.SelectSingleNode("Name").InnerText;
    int rating = int.Parse(studentReportNode.SelectSingleNode("Rating").InnerText);
    string userResponse = studentReportNode.SelectSingleNode("UserResponse").InnerText;
    string userAdditionalResponse = studentReportNode.SelectSingleNode("UserAdditionalResponse").InnerText;
    string time = studentReportNode.SelectSingleNode("Time").InnerText;

    // Create a new Student object and add it to the list
    StudentReport studentReport = new StudentReport(studentnumber, name, rating, userResponse, userAdditionalResponse, time);
    studentReports.Add(studentReport);
}

// Now you have a list of Student objects, and you can use them as needed
if (debug == true)
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
}

Boolean isAuthenticated = false;

while (!isAuthenticated)
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

    if (isAuthenticated == false)
    {
        Console.WriteLine("Invalid Username or Password. Please try again.");
    }

}



void StudentUserInterface(Student student)
{
    while (looplockTrue)
    {
        Console.WriteLine($"Welcome to My Personal Supervisor Manager student!");
        Console.WriteLine("Please choose from the options:");
        Console.WriteLine("1. Self Report how you are feeling / progressing.");
        Console.WriteLine("2. Book a meeting with your personal supervisor.");
        Console.WriteLine("3.Exit the program.");

        String userResponse = Console.ReadLine();

        if (userResponse == "1")
        {
            StudentStatusReport(student);
        }
        if (userResponse == "2")
        {
            BookappointmentSt(student);
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
            Console.WriteLine("Invalid input. Please enter 1, 2, or 3.");
        }
    }
}
void PersSupInterface(PersonalSupervisor personalSupervisor)
{
    while (looplockTrue)
    {
        Console.WriteLine("Welcome to My Personal Supervisor Manager Personal Supervisor!");
        Console.WriteLine("Please choose from the options:");
        Console.WriteLine("1.Review your students Status.");
        Console.WriteLine("2.Book an appointment with your student.");
        Console.WriteLine("3.Exit the program.");

        String userResponse = Console.ReadLine();

        if (userResponse == "1")
        {
            reviewStudentStatus(personalSupervisor);
        }
        else if (userResponse == "2")
        {
            BookappointmentPs(personalSupervisor);
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter 1.");
        }

    }
}
void BookappointmentSt(Student student)
{
    Console.WriteLine("Not implimented yet.");
}

void BookappointmentPs(PersonalSupervisor personalSupervisor)
{
    int i = 1;
    Console.WriteLine("Please choose a student from the list below you wish to meet:");
    foreach (Student x in students)
    {
        if (x.SupervisorCode == personalSupervisor.SupervisorCode)
        {
            Console.WriteLine($"{i}. {x.Name}");
            i++;
        }
    }
}

void reviewStudentStatus(PersonalSupervisor personalSupervisor)
{
    Console.WriteLine("Please choose a student from the list below you wish to view the status of:");
    List<Student> matchingStudents = new List<Student>();
    foreach (Student x in students)
    {
        if (x.SupervisorCode == personalSupervisor.SupervisorCode)
        {
            matchingStudents.Add(x);
        }
    }

    for (int i = 0; i < matchingStudents.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {matchingStudents[i].Name}");
    }

    Student selectedStudent = matchingStudents[int.Parse(Console.ReadLine()) - 1];

    int numberOfReports = 0;

    foreach (StudentReport x in studentReports)
    {
        if (selectedStudent.StudentNumber == x.StudentNumber)
        {
            numberOfReports++;
            Console.WriteLine($"Report taken from student at {x.Time}");
            Console.WriteLine("Your Student is Currently feeling like they are progressing at a rating of:");
            Console.WriteLine($"{x.Progressing}/10");
            Console.WriteLine("They have said that they are currently feeling:");
            Console.WriteLine($"{x.StudentFeelings}");
            Console.WriteLine("They have left the following additional information:");
            Console.WriteLine($"{x.AdditionalReport}");

            if (x.Progressing > 4)
            {
                Console.WriteLine("The low progression score indicates that your student feeels that they may be struggling, maybe book a meeting?");
            }

            if (numberOfReports == 0)
            {
                Console.WriteLine("This student has not given feedback yet.");
            }
            Console.WriteLine("Please select one of the following:");
            Console.WriteLine("1.Book a meeting");
            Console.WriteLine("2.Return to main screen");
            Boolean looplock = false;
            while (!looplock) 
            {
                try
                {
                    int userchoice = int.Parse(Console.ReadLine());
                    if (userchoice < 0 || userchoice > 2)
                    {
                        Console.WriteLine("That is an invalid choice please choose again.");
                    }
                    else if (userchoice == 1)
                    {
                        BookappointmentPs(personalSupervisor);
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
    }
}

void StudentStatusReport(Student student)
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

        //code Inspired by this video: https://www.youtube.com/watch?v=hbIPpcJAXp0
        StudentReport studentReport = new StudentReport(student.StudentNumber, student.Name, rating, UserResponse, userAdditionalResponse, currenttime);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load("StudentReports.xml");

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

        truePath = truePath + @"\StudentReports.xml";


        xmlDoc.Save(truePath);

        if (debug) Console.WriteLine($"New student report added to XML document.");
    }
}

