// See https://aka.ms/new-console-template for more information
using System.Xml.Linq;
using System.Xml;
using DDD_Personal_portfolio;
using System.Reflection.Metadata;
using System.Xml.Serialization;
using System.Diagnostics;

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

// Create a list to store the Personal suporvisor objects
List<Student> students = new List<Student>();
List<PersonalSupervisor> personalSupervisors = new List<PersonalSupervisor>();

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
// Now you have a list of Student objects, and you can use them as needed
if (debug == true)
{
    Console.WriteLine("Students:");
    foreach (Student student in students)
    {
        Console.WriteLine($"Username: {student.Username}, Password: {student.Password}, Name: {student.Name}, Supervisor Code: {student.SupervisorCode}");
    }
    Console.WriteLine("Peronal Supervisors:");
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
    Console.WriteLine("Not implimented yet.");
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
        studentnumber.InnerText = studentReport.StudentNumber.ToString();
        StudentReport.AppendChild(studentnumber);

        xmlDoc.DocumentElement.AppendChild(StudentReport);

        xmlDoc.Save("StudentReports.xml"); // Save the document to a file

        if (debug) Console.WriteLine($"New student report added to XML document.");
    }
}
