// See https://aka.ms/new-console-template for more information
using System.Xml.Linq;
using System.Xml;
using DDD_Personal_portfolio;
using System.Reflection.Metadata;
using System.Xml.Serialization;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Globalization;
// Boolean variables to control debugging, showing all data, and loop lock status
Boolean debug = false;  // Indicates if debugging information should be displayed
Boolean ShowAll = false;  // Indicates if al data should be shown
Boolean looplockTrue = true;  // Controls if teh loop should be locked

// XmlDocument instances for loading and handling XML data
XmlDocument xmlDoc = new XmlDocument();
XmlDocument xmlDoc2 = new XmlDocument();

try
{
    // Load teh XML files into the XmlDocument objects
    xmlDoc.Load("_Information.xml");
    xmlDoc2.Load("_StudentReports.xml");
}
catch (Exception ex)
{
    // Print error message if XML files fail to load
    Console.WriteLine("Error loading XML files: " + ex.Message);
    return;
}

// Select nodes from XML documents corresponding to different entities
XmlNodeList studentNodes = xmlDoc.SelectNodes("/root/Student");
XmlNodeList psNodes = xmlDoc.SelectNodes("/root/PersonalSupervisor");
XmlNodeList stNodes = xmlDoc.SelectNodes("/root/SeniorTutor");
XmlNodeList studentReportNodes = xmlDoc2.SelectNodes("/root/StudentReport");
XmlNodeList meetingNodes = xmlDoc.SelectNodes("/root/Meeting");

// Lists to store data of different entities, like students, personal supervisors, and senior tutors
List<Student> students = new List<Student>();
List<PersonalSupervisor> personalSupervisors = new List<PersonalSupervisor>();
List<SeniorTutor> seniorTutors = new List<SeniorTutor>();
List<Meeting> meetings = new List<Meeting>();
List<StudentReport> studentReports = new List<StudentReport>();

// Process each student node in teh XML and create Student objects
foreach (XmlNode studentNode in studentNodes)
{
    try
    {
        // Get student information from XML node including username, password, name, supervisor code, and student number
        string username = studentNode.Attributes["username"].Value;
        string password = studentNode.SelectSingleNode("Password").InnerText;
        string name = studentNode.SelectSingleNode("Name").InnerText;
        string supervisorCode = studentNode.SelectSingleNode("SupervisorCode").InnerText;
        int studentNumber = int.Parse(studentNode.SelectSingleNode("StudentNumber").InnerText);
        List<string> Availability = new List<string>();

        // Get availability hours for teh student
        XmlNodeList hoursNodes = studentNode.SelectNodes("Availability/string");
        foreach (XmlNode hourNode in hoursNodes)
        {
            Availability.Add(hourNode.InnerText);
        }

        // Create and add Student object to teh list
        Student student = new Student(username, password, name, studentNumber, Availability, supervisorCode);
        students.Add(student);
    }
    catch (Exception ex)
    {
        // Print error message if student node processing fails, helpful to identify which specific node had issues
        Console.WriteLine("Error processing student node: " + ex.Message);
    }
}

// Process each personal supervisor node in the XML and create PersonalSupervisor objects
foreach (XmlNode psNode in psNodes)
{
    try
    {
        // Get personal supervisor information from XML node including username, password, name, and supervisor code
        string username = psNode.Attributes["username"].Value;
        string password = psNode.SelectSingleNode("Password").InnerText;
        string name = psNode.SelectSingleNode("Name").InnerText;
        string supervisorCode = psNode.SelectSingleNode("SupervisorCode").InnerText;
        int amountOfMeetingRequests = 0;
        List<string> Availability = new List<string>();

        // Get availability hours for the personal supervisor
        XmlNodeList hoursNodes = psNode.SelectNodes("Availability/string");
        foreach (XmlNode hourNode in hoursNodes)
        {
            Availability.Add(hourNode.InnerText);
        }

        // Create and add PersonalSupervisor object to teh list
        PersonalSupervisor personalSupervisor = new PersonalSupervisor(username, password, name, supervisorCode, amountOfMeetingRequests, Availability);
        personalSupervisors.Add(personalSupervisor);
    }
    catch (Exception ex)
    {
        // Print error message if personal supervisor node processing fails, useful for debugging and understanding whedre the problem occured
        Console.WriteLine("Error processing personal supervisor node: " + ex.Message);
    }
}

// Process each senior tutor node in the XML and create SeniorTutor objects
foreach (XmlNode stNode in stNodes)
{
    try
    {
        // Get senior tutor information from XML node including username, password, and name
        string username = stNode.Attributes["username"].Value;
        string password = stNode.SelectSingleNode("Password").InnerText;
        string name = stNode.SelectSingleNode("Name").InnerText;
        SeniorTutor seniorTutor = new SeniorTutor(username, password, name);
        seniorTutors.Add(seniorTutor);
    }
    catch (Exception ex)
    {
        // Print error message if senior tutor node processing fails
        Console.WriteLine("Error processing senior tutor node: " + ex.Message);
    }
}

// Process each student report node in the XML and create StudentReport objects
foreach (XmlNode studentReportNode in studentReportNodes)
{
    try
    {
        // Get student report information from XML node including student number, name, rating, user response, additional response, and time
        int studentnumber = int.Parse(studentReportNode.SelectSingleNode("StudentNumber").InnerText);
        string name = studentReportNode.SelectSingleNode("Name").InnerText;
        int rating = int.Parse(studentReportNode.SelectSingleNode("Rating").InnerText);
        string userResponse = studentReportNode.SelectSingleNode("UserResponse").InnerText;
        string userAdditionalResponse = studentReportNode.SelectSingleNode("UserAdditionalResponse").InnerText;
        string time = studentReportNode.SelectSingleNode("Time").InnerText;

        // Create ad add StudentReport object to teh list
        StudentReport studentReport = new StudentReport(studentnumber, name, rating, userResponse, userAdditionalResponse, time);
        studentReports.Add(studentReport);
    }
    catch (Exception ex)
    {
        // Print error message if student report node processing fails
        Console.WriteLine("Error processing student report node: " + ex.Message);
    }
}

// Process each meeing node in the XML and create Meeting objects
foreach (XmlNode meetingNode in meetingNodes)
{
    try
    {
        // Get meeting information from XML node including supervisor code, stuent number, meeting details, and time
        string supervisorCode = meetingNode.SelectSingleNode("SupervisorCode").InnerText;
        int studentNumber = int.Parse(meetingNode.SelectSingleNode("StudentNumber").InnerText);
        string meetingRegarding = meetingNode.SelectSingleNode("MeetingRegarding").InnerText;
        string time = meetingNode.SelectSingleNode("Time").InnerText;

        // Create and add Meeting object to the list
        Meeting meeting = new Meeting(supervisorCode, studentNumber, meetingRegarding, time);
        meetings.Add(meeting);
    }
    catch (Exception ex)
    {
        // Print error message if meeting node processing fails, useful for debugging specific meeting data issues
        Console.WriteLine("Error processing meeting node: " + ex.Message);
    }
}

// Check if all data should be displayed
if (ShowAll == true)
{
    // Display all student information in teh console
    Console.WriteLine("Students:");
    foreach (Student student in students)
    {
        // Detailed student information including username, password, name, and supervisor code
        Console.WriteLine($"Username: {student.Username}, Password: {student.Password}, Name: {student.Name}, Supervisor Code: {student.SupervisorCode}");
    }
    // Display all personal supervisor information in teh console
    Console.WriteLine("Personal Supervisors:");
    foreach (PersonalSupervisor personalSupervisor in personalSupervisors)
    {
        // Detailed personal supervisor information including username, password, name, and supervisor code
        Console.WriteLine($"Username: {personalSupervisor.Username}, Password: {personalSupervisor.Password}, Name: {personalSupervisor.Name}, Supervisor Code: {personalSupervisor.SupervisorCode}");
    }
    // Display all senior tutor information in the console
    Console.WriteLine("Senior Tutors:");
    foreach (SeniorTutor seniorTutor in seniorTutors)
    {
        // Detailed senior tutor information including username, paassword, and name
        Console.WriteLine($"Username: {seniorTutor.Username}, Password: {seniorTutor.Password}, Name: {seniorTutor.Name}");
    }
}
// Check if debugging information should be displayed
else if (debug == true)
{
    // Debugging instructions for easier view of students, personal supervisors, and senioar tutors
    Console.WriteLine("For an easy view of student and how to interact - Enter a as teh username and A as teh password.");
    Console.WriteLine("For an easy view of Personal Supervisor and how to interact - Enter b as teh username and B as teh password.");
    Console.WriteLine("For an easy view of Senior Tutor  - Enter c as teh username and C as teh password.");
    Console.WriteLine("To view teh difference in handling of teh students with/without a booked meeting, - enter DavidWebb as teh Username and David as teh password.");
}

// Boolean to check if user is authenticated
Boolean isAuthenticated = false;

// Loop until user is authenticated
while (!isAuthenticated)
{
    try
    {
        // AsK teh user for username and password, capturindg their input
        Console.WriteLine("Please enter your username:");
        string inputUsername = Console.ReadLine();
        Console.WriteLine("Please enter your Password:");
        string inputPassword = Console.ReadLine();

        // Check if teh user is a student
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

        // Check if teh user is a persona supervisor
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

        // Check if teh user is a senior tutor
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

        // If authentication fails, Ask teh user to try again
        if (isAuthenticated == false)
        {
            // Inform the user that the provided username or password is incrrect and ask them to try again
            Console.WriteLine("Invalid Username or Password. Please try again.");
        }
    }
    catch (Exception ex)
    {
        // Print error message if authentication process fails, capturing teh excepton message for debugging purposes
        Console.WriteLine("Error during authentication: " + ex.Message);
    }
}
// Function to handle the student user interface
void StudentUserInterface(Student student)
{
    // Continuously loop until looplockTrue is set to false
    while (looplockTrue)
    {
        try
        {
            // Display the main menu options for the student
            Console.WriteLine($"Welcome to My Personal Supervisor Manager student!");
            Console.WriteLine("Please choose from teh options:");
            Console.WriteLine("1. Self Report how you are feeling / progressing.");
            Console.WriteLine("2. Book a meeting with your personal supervisor.");
            Console.WriteLine("3. View booked meetings.");
            Console.WriteLine("4. Update availability.");
            Console.WriteLine("5. Remove availability time.");
            Console.WriteLine("6. Exit teh program.");

            // Read teh user's response
            String userResponse = Console.ReadLine();

            // Execute teh appropriate action based on the user's response
            if (userResponse == "1")
            {
                // If the user selects 1, call the function to report their status
                StudentStatusReport(student);
            }
            else if (userResponse == "2")
            {
                // If the user selects 2, call the function to book an appointment
                BookappointmentSt(student);
            }
            else if (userResponse == "3")
            {
                // If the user selects 3, call the function to view booked meetings
                ViewBookedMeetingStu(student);
            }
            else if (userResponse == "4")
            {
                // If the user selects 4, call the function to update availability
                UpdateAvailabilityStudent(student);
            }
            else if (userResponse == "5")
            {
                // If teh user selects 5, call the function to remove availability time
                RemoveAvailabilityTimeStu(student);
            }
            else if (userResponse == "6")
            {
                // If the user selects 6, set looplockTrue to false and exit the program
                looplockTrue = false;
                Console.WriteLine("Exiting the program...");
                Thread.Sleep(3000);  // Wait for 3 seconds before exiting
                System.Environment.Exit(0);  // Exit the program
            }
            else
            {
                // If teh user enters an invalid option, Ask tehm to enter a valid nusmber
                Console.WriteLine("Invalid input. Please enter a number between 1 and 6.");
            }
        }
        catch (Exception ex)
        {
            // Print error message if an exception occurs within the try block
            Console.WriteLine("Error in student user interface: " + ex.Message);
        }
    }
}

// Function to handle the personal supervissor user interface
void PersSupInterface(PersonalSupervisor personalSupervisor)
{
    // Continuously loop until looplockTrue is set to false
    while (looplockTrue)
    {
        try
        {
            // Display the main menu options for the persoanal supervisor
            Console.WriteLine("Welcome to My Personal Supervisor Manager Personal Supervisor!");
            Console.WriteLine("Please choose from teh options:");
            Console.WriteLine("1. Review your students Status.");
            Console.WriteLine("2. Book Meetings.");
            Console.WriteLine("3. View booked meetings.");
            Console.WriteLine("4. Update availability.");
            Console.WriteLine("5. Remove availability time.");
            Console.WriteLine("6. Exit teh program.");

            // Read the user's response
            String userResponse = Console.ReadLine();

            // Execute the appropriate action based on teh user's response
            if (userResponse == "1")
            {
                // If teh user selects 1, call teh function to review student status
                reviewStudentStatus(personalSupervisor);
            }
            else if (userResponse == "2")
            {
                // If the user selects 2, call the function to book meetings
                BookappointmentPs(personalSupervisor);
            }
            else if (userResponse == "3")
            {
                // If the user selects 3, call teh function to view booked meetings
                ViewBookedMeetingsPs(personalSupervisor);
            }
            else if (userResponse == "4")
            {
                // If the user selects 4, call the function to update availability
                UpdateAvailabilityPs(personalSupervisor);
            }
            else if (userResponse == "5")
            {
                // If the user selects 5, call the function to remove availability time
                RemoveAvailabilityTimePs(personalSupervisor);
            }
            else if (userResponse == "6")
            {
                // If the user selects 6, set looplockTrue to false and exit the program
                looplockTrue = false;
                Console.WriteLine("Exiting the program...");
                Thread.Sleep(3000);  // Wait for 3 seconds before exiting
                System.Environment.Exit(0);  // Exit the program
            }
            else
            {
                // If the user entrs an invalid option, Aks them to enter a valid number
                Console.WriteLine("Invalid input. Please enter a number between 1 and 6.");
            }
        }
        catch (Exception ex)
        {
            // Print error messafe if an exception occurs within the ty block
            Console.WriteLine("Error in personal supervisor user interface: " + ex.Message);
        }
    }
}

// Function to handle the senior tutor user interfacce
void SenTutInterface(SeniorTutor seniorTutor)
{
    // Continuously loop until looplockTrue is set to false keepping the user stuck in the code
    while (looplockTrue)
    {
        try
        {
            // Display the main menu options for the senior tutor
            Console.WriteLine("Welcome to My Senior Tutor Manager Senior Tutor!");
            Console.WriteLine("Please choose from the options:");
            Console.WriteLine("1. View booked meetings between personal supervisors and students.");
            Console.WriteLine("2. View student report based on personal supervisor.");
            Console.WriteLine("3. Exit the program.");

            // Read the user's response
            String userResponse = Console.ReadLine();

            // Execute the appropriate action based on the user's response
            if (userResponse == "1")
            {
                // If the user selects 1, call the function to view booked meetings
                ViewBookedMeetingsSenTut(seniorTutor);
            }
            else if (userResponse == "2")
            {
                // If the user selects 2, call the function to review all student status
                reviewAllStudentStatus(seniorTutor);
            }
            else if (userResponse == "3")
            {
                // If teh user selects 3, set looplockTrue to false and exit teh program
                looplockTrue = false;
                Console.WriteLine("Exiting the program...");
                Thread.Sleep(3000);  // Wait for 3 seconds before exiting
                System.Environment.Exit(0);  // Exit the program
            }
            else
            {
                // If the user enters an invalid option, ASk them to enter a valid number
                Console.WriteLine("Invalid input. Please enter 1, 2 or 3");
            }
        }
        catch (Exception ex)
        {
            // Print error message if an exception occurs within teh try block
            Console.WriteLine("Error in senior tutor user interface: " + ex.Message);
        }
    }
}

// Function to view booked meetings for a personal supervisor
void ViewBookedMeetingsPs(PersonalSupervisor personalSupervisor)
{
    try
    {
        // Display te meetings for the psersonal supervisor
        Console.WriteLine($"Meetings for Personal Supervisor: {personalSupervisor.Name}");

        // Get the list of meetings for the personal supervisor
        List<Meeting> supervisorMeetings = meetingss.Where(m => m.SupervisorCode == personalSupervisor.SupervisorCode).ToList();

        // Check if there are no meetngs found
        if (supervisorMeetings.Count == 0)
        {
            // Inform the usser that no meetings were foumnd
            Console.WriteLine("No meetings found.");
            return;
        }

        // Loop through each meting and display the details
        for (int i = 0; i < supervisorMeetings.Count; i++)
        {
            Student student = students.FirstOrDefault(s => s.StudentNumber == supervisorMeetings[i].StudentNumber);
            Console.WriteLine($"{i + 1}. Meeting with {student.Name} (Student Number: {supervisorMeetings[i].StudentNumber}) at {supervisorMeetings[i].Time}. Regarding: {supervisorMeetings[i].MeetingRegarding}");
        }

        // Ask the user to enter the number of the meeting they want to cancel or 0 to go back
        Console.WriteLine("Enter the number of the meeting you want to cancel, or enter 0 to go back:");
        int selectedMeetingIndex = int.Parse(Console.ReadLine()) - 1;

        // Check if the user wants to go back
        if (selectedMeetingIndex == -1)
        {
            return;
        }

        // Check if teh selected meeeting index is invalid
        if (selectedMeetingIndex < 0 || selectedMeetingIndex >= supervisorMeetings.Count)
        {
            // Inform the user that the selection is invalid
            Console.WriteLine("Invalid selection.");
            return;
        }

        // Get the meeting to cancel
        Meeting meetingToCancel = supervisorMeetings[selectedMeetingIndex];
        // Ask the user to confirm the cancellation
        Console.WriteLine($"Are you sure you want to cancel the meeting with student number {meetingToCancel.StudentNumber} at {meetingToCancel.Time}? (yes/no)");

        // Check if the user confirms the cancellation
        if (Console.ReadLine().ToLower() == "yes")
        {
            // Call the function to cancel the meeting
            CancelMeeting(meetingToCancel);
        }
    }
    catch (Exception ex)
    {
        // Print error message if an exception occurs within the try block
        Console.WriteLine("Error in viewing booked meetings for personal supervisor: " + ex.Message);
    }
}

// Function to view booked meetings for a student
void ViewBookedMeetingStu(Student student)
{
    try
    {
        // Display the meetings for teh studnet
        Console.WriteLine($"Meetings for Student: {student.Name}");

        // Get the list of meetings for the student
        List<Meeting> studentMeetings = meetings.Where(m => m.StudentNumber == student.StudentNumber).ToList();

        // Check if there are no mer=tings found
        if (studentMeetings.Count == 0)
        {
            // Inform the user that no meetings were found
            Console.WriteLine("No meetings found.");
            return;
        }

        // Loop throh each meeting and display the details
        for (int i = 0; i < studentMeetings.Count; i++)
        {
            PersonalSupervisor ps = personalSupervisors.FirstOrDefault(p => p.SupervisorCode == studentMeetings[i].SupervisorCode);
            Console.WriteLine($"{i + 1}. Meeting with {ps.Name} (Supervisor Code: {studentMeetings[i].SupervisorCode}) at {studentMeetings[i].Time}. Regarding: {studentMeetings[i].MeetingRegarding}");
        }

        // Ask the user to enter the number of the meeting they want to cancel or 0 to go back
        Console.WriteLine("Enter the number of the meeting you want to cancel, or enter 0 to go back:");
        int selectedMeetingIndex = int.Parse(Console.ReadLine()) - 1;

        // Check if the user wants to goo back
        if (selectedMeetingIndex == -1)
        {
            return;
        }

        // Check if the selcted meeting index is invalid
        if (selectedMeetingIndex < 0 || selectedMeetingIndex >= studentMeetings.Count)
        {
            // Inform teh user that the selection is invalid
            Console.WriteLine("Invalid selection.");
            return;
        }

        // Get the meeting to cancel
        Meeting meetingToCancel = studentMeetings[selectedMeetingIndex];
        // Ask the user to confirm the cancellation
        Console.WriteLine($"Are you sure you want to cancel the meeting with supervisor code {meetingToCancel.SupervisorCode} at {meetingToCancel.Time}? (yes/no)");

        // Check if the user confirms the cancellation
        if (Console.ReadLine().ToLower() == "yes")
        {
            // Call the function to cancel the meeting
            CancelMeeting(meetingToCancel);
        }
    }
    catch (Exception ex)
    {
        // Print error message if an exception occurs within teh try block
        Console.WriteLine("Error in viewing booked meetings for student: " + ex.Message);
    }
}

// Function to view booked meeitings for a senior tutor
void ViewBookedMeetingsSenTut(SeniorTutor seniorTutor)
{
    try
    {
        // Display the list of personal supervisors
        Console.WriteLine("List of Personal Supervisors:");
        for (int i = 0; i < personalSupervisors.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {personalSupervisors[i].Name}");
        }

        // Ask the user to select a personal supervisor by entering the correspondinhg number
        Console.WriteLine("Please select a Personal Supervisor by entering the corresponding number:");
        int psIndex = int.Parse(Console.ReadLine()) - 1;

        // Check if the selected index is invalid
        if (psIndex < 0 || psIndex >= personalSupervisors.Count)
        {
            // Inform the user that the selection is invalid
            Console.WriteLine("Invalid selection. Please try again.");
            return;
        }

        // Get the selected personal supervisor
        PersonalSupervisor selectedPS = personalSupervisors[psIndex];

        // Display teh meetings for the selected personal supervisor
        Console.WriteLine($"Meetings for Personal Supervisor: {selectedPS.Name}");

        // Get the list of meetings for the selected persoal supervisor
        List<Meeting> supervisorMeetings = meetings.Where(m => m.SupervisorCode == selectedPS.SupervisorCode).ToList();

        // Check if there are no meetings found
        if (supervisorMeetings.Count == 0)
        {
            // Inform the user that no meetings were found
            Console.WriteLine("No meetings found.");
            return;
        }

        // Loop through each meeting and dislay the details
        for (int i = 0; i < supervisorMeetings.Count; i++)
        {
            Student student = students.FirstOrDefault(s => s.StudentNumber == supervisorMeetings[i].StudentNumber);
            Console.WriteLine($"{i + 1}. Meeting with {student.Name} (Student Number: {supervisorMeetings[i].StudentNumber}) at {supervisorMeetings[i].Time}. Regarding: {supervisorMeetings[i].MeetingRegarding}");
        }

        // Ask teh user to press any key to retrn to the main display
        Console.WriteLine("Press Any Key in order to return to the main display");
        Console.ReadKey();
        Console.Clear();
    }
    catch (Exception ex)
    {
        // Print error message if an exception occurs within the try block
        Console.WriteLine("Error in viewing booked meetings for senior tutor: " + ex.Message);
    }
}

// Function to update availability for a personal supervisor
void UpdateAvailabilityPs(PersonalSupervisor personalSupervisor)
{
    try
    {
        // Ask the user to enter either 1 for AM or 2 for PM
        Console.WriteLine("Please enter either 1 for AM or 2 for PM.");
        int selection = 0;
        string AmOrPm = null;
        do
        {
            try
            {
                // Read and parse the user's input for AM or PM selection
                selection = int.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            {
                // Inform the user to enter a valid number
                Console.WriteLine("Please enter a valid number.");
            }

            // Validate teh user's input for AM or PM selection
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
                // Ask the user to enter an hour between 1 and 12
                Console.WriteLine("Please enter an hour between 1 and 12:");
                isValidHour = int.TryParse(Console.ReadLine(), out hour);
            }
            catch
            {
                // Inform the user to enter a valid number
                Console.WriteLine("please enter a number.");
            }

            // Validate the user's input for the hour selection
            if (!isValidHour || hour < 1 || hour > 12)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 12.");
            }
            else
            {
                ValidHour = true;
            }
        } while (ValidHour == false);

        // Ask the user to select the time to add to their availability
        Console.WriteLine("Please select the time you would like to add to your availability below");

        int[] minuteIntervals = { 00, 10, 20, 30, 40, 50 };

        // Display the available time options for the user to select
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
                    // Ask the user to enter teh number of their choice for teh time selection
                    Console.Write("Please enter the number of your choice: ");
                    userInput = int.Parse(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    // Inform the user to enter a valid number
                    Console.WriteLine("Please enter a valid number.");
                }

                // Validate teh user's choice for teh time selection
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
                // Add the selected time to the personal supervisor's availability
                string newAvailability = selectedOption;
                personalSupervisor.Availability.Add(newAvailability);
                Console.WriteLine($"New availability {newAvailability} added.");
                FinalTimeSet = true;

                // Get the current directory and construct the path to the XML file
                string path = Directory.GetCurrentDirectory();
                string truePath = path.Remove(path.Length - 17);
                truePath = truePath + @"\_Information.xml";

                // Load teh XML document
                XDocument xmlDoc = XDocument.Load(truePath);

                // Find teh personal supervisor element in the XML document
                XElement psElement = xmlDoc.Descendants("PersonalSupervisor")
                                           .FirstOrDefault(e => e.Element("SupervisorCode")?.Value == personalSupervisor.SupervisorCode);

                if (psElement != null)
                {
                    // Add the new availability to the XML document
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

                // Save the updated XML document
                xmlDoc.Save(truePath);
            }
        } while (FinalTimeSet == false);
    }
    catch (Exception ex)
    {
        // Print error message if an exception occurs within the try block
        Console.WriteLine("Error in updating availability for personal supervisor: " + ex.Message);
    }
}

// Function to book an appointment for a student
void BookappointmentSt(Student student)
{
    try
    {
        // Find the personal supervisor for the given student
        PersonalSupervisor ps = personalSupervisors.FirstOrDefault(x => x.SupervisorCode == student.SupervisorCode);

        // Check if teh personal supervisor was found
        if (ps == null)
        {
            // If no personal supervisor is found, display an error message
            Console.WriteLine("No personal supervisor found for the given student.");
            return;
        }

        // Find common available times between the personal supervisor and the student
        List<string> commonAvailableTimes = ps.Availability.Intersect(student.Availability).ToList();

        // Check if there are no common available times
        if (commonAvailableTimes.Count == 0)
        {
            // If no common available times are found, display an error message
            Console.WriteLine("No common available times found.");
            return;
        }

        // Display the common available times to the student
        for (int i = 0; i < commonAvailableTimes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {commonAvailableTimes[i]}");
        }

        // Ask the student to select a time for teh meeting
        string selectedTime = commonAvailableTimes[int.Parse(Console.ReadLine()) - 1];

        // Ask teh student to enter the reason for the meeting
        Console.WriteLine("Please enter the reason for the meeting");
        string reasonForMeeting = Console.ReadLine();

        // Create a new meeting object with the selected time and reason
        Meeting meeting = new Meeting(student.SupervisorCode, student.StudentNumber, reasonForMeeting, selectedTime);

        // Display a confirmation message to the student
        Console.WriteLine($"To confirm - You would like to suggest a Meeting for {meeting.Time} with Your Personal Supervisor for the following reason:");
        Console.WriteLine(reasonForMeeting);

        // Save the meeting details to the XML file
        SaveMeetingToXml(meeting, ps, student, selectedTime);
    }
    catch (Exception ex)
    {
        // Print error message if an exception occurs within the try block
        Console.WriteLine("Error in booking appointment for student: " + ex.Message);
    }
}

// Function to book an appointment for a personal supervisor
void BookappointmentPs(PersonalSupervisor personalSupervisor, Student student = null)
{
    try
    {
        Student selectedStudent;

        // If no student is provided, Ask teh personal supervisor to select a student from the list
        if (student == null)
        {
            Console.WriteLine("Please choose a student from the list below you wish to meet:");
            List<Student> matchingStudents = students.Where(x => x.SupervisorCode == personalSupervisor.SupervisorCode).ToList();

            // Display the list of students supervised by the personal supervisor
            for (int i = 0; i < matchingStudents.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {matchingStudents[i].Name}");
            }

            // Ask the personal supervisor to select a student
            selectedStudent = matchingStudents[int.Parse(Console.ReadLine()) - 1];
        }
        else
        {
            selectedStudent = student;
        }

        // Find common available times between the personal supervisor and the selected student
        List<string> commonAvailableTimes = personalSupervisor.Availability.Intersect(selectedStudent.Availability).ToList();

        // Check if there are no common available times
        if (commonAvailableTimes.Count == 0)
        {
            // If no common available times are found, display an error message
            Console.WriteLine("No common available times found.");
            return;
        }

        // Display the common available times to the personal supervisor
        for (int i = 0; i < commonAvailableTimes.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {commonAvailableTimes[i]}");
        }

        // Ask teh personal supervisor to select a time for the meeting
        string selectedTime = commonAvailableTimes[int.Parse(Console.ReadLine()) - 1];

        // Ask the personal supervisor to enter the reason for the meeting
        Console.WriteLine("Please enter the reason for the meeting");
        string reasonForMeeting = Console.ReadLine();

        // Create a new meeting object with the selected time and reason
        Meeting meeting = new Meeting(selectedStudent.SupervisorCode, selectedStudent.StudentNumber, reasonForMeeting, selectedTime);

        // Display a confirmation message to the personal supervisor
        Console.WriteLine($"To confirm - You would like to suggest a Meeting for {meeting.Time} with {selectedStudent.Name} for the following reason:");
        Console.WriteLine(reasonForMeeting);

        // Save the meeting details to the XML file
        SaveMeetingToXml(meeting, personalSupervisor, selectedStudent, selectedTime);
    }
    catch (Exception ex)
    {
        // Print error message if an exception occurs within the try block
        Console.WriteLine("Error in booking appointment for personal supervisor: " + ex.Message);
    }
}

// Function to save the meeting details to teh XML file
void SaveMeetingToXml(Meeting meeting, PersonalSupervisor ps, Student student, string selectedTime)
{
    try
    {
        // Load teh XML documsent
        XmlDocument xmlDocs = new XmlDocument();
        xmlDocs.Load("_Information.xml");

        // Create new XML nodes for the meeting details
        XmlNode Meeting = xmlDocs.CreateElement("Meeting");
        XmlNode SupervisorCode = xmlDocs.CreateElement("SupervisorCode");
        XmlNode StudentNumber = xmlDocs.CreateElement("StudentNumber");
        XmlNode MeetingRegarding = xmlDocs.CreateElement("MeetingRegarding");
        XmlNode Time = xmlDocs.CreateElement("Time");

        // Set the inner text of the XML nodes to teh meeting detasils
        SupervisorCode.InnerText = meeting.SupervisorCode.ToString();
        StudentNumber.InnerText = meeting.StudentNumber.ToString();
        MeetingRegarding.InnerText = meeting.MeetingRegarding.ToString();
        Time.InnerText = meeting.Time.ToString();

        // Append the meeting details to the XML document
        Meeting.AppendChild(SupervisorCode);
        Meeting.AppendChild(StudentNumber);
        Meeting.AppendChild(MeetingRegarding);
        Meeting.AppendChild(Time);
        xmlDocs.DocumentElement.AppendChild(Meeting);

        // Get teh current directory and construct the path to the XML file
        string pathMeeting = Directory.GetCurrentDirectory();
        string truePathMeeting = pathMeeting.Remove(pathMeeting.Length - 17);
        truePathMeeting = truePathMeeting + @"\_Information.xml";

        // Remove the selected time from teh availability of teh personal supervisor and student
        ps.Availability.Remove(selectedTime);
        student.Availability.Remove(selectedTime);

        // Load the XML document again
        XDocument xmlDoc = XDocument.Load(truePathMeeting);

        // Find the personal supervisor and student elements in the XML document
        XElement psElement = xmlDoc.Descendants("PersonalSupervisor").FirstOrDefault(e => e.Element("SupervisorCode")?.Value == ps.SupervisorCode);
        XElement studentElement = xmlDoc.Descendants("Student").FirstOrDefault(e => e.Element("StudentNumber")?.Value == student.StudentNumber.ToString());

        // Remove the selected time from the availability of the personal supervisor in teh XML document
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

        // Remove teh selected time from the availability of the student in the XML document
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

        // Save the updated XML documents
        xmlDoc.Save(truePathMeeting);
        xmlDocs.Save(truePathMeeting);
        xmlDoc.Save("_Information.xml");
        xmlDocs.Save("_Information.xml");

        // Inform the user that the meeting has been set
        Console.WriteLine("The Meeting has been set.");
    }
    catch (Exception ex)
    {
        // Print error message if an exception occurs within the try block
        Console.WriteLine("Error in saving meeting to XML: " + ex.Message);
    }
}

// Function to review the status of students for a personal supervisor
void reviewStudentStatus(PersonalSupervisor personalSupervisor)
{
    try
    {
        // Ask teh personal supervisor to choose between viewing all students or students needing a meeting
        Console.WriteLine("Would you like to see:");
        Console.WriteLine("1. A list of all students");
        Console.WriteLine("2. A list of students needing a meeting (most recent report score 4 or below)");

        int choice = int.Parse(Console.ReadLine());

        List<Student> matchingStudents = new List<Student>();

        // Loop through all students and filter based on teh choice
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

                    // Find the most recent report for the student
                    foreach (StudentReport report in studentReports)
                    {
                        if (report.StudentNumber == x.StudentNumber)
                        {
                            DateTime reportTime = DateTime.ParseExact(report.Time, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                            if (reportTime > mostRecentTime)
                            {
                                mostRecentReport = report;
                                mostRecentTime = reportTime;
                            }
                        }
                    }

                    // Check if the most recent report indicates the student needs a meeting
                    if (mostRecentReport != null && mostRecentReport.Progressing <= 4)
                    {
                        matchingStudents.Add(x);
                    }
                }
            }
        }

        // Ask the personal supervisor to select a student to view their status
        Console.WriteLine("Please choose a student from the list below you wish to view the status of:");
        for (int i = 0; i < matchingStudents.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {matchingStudents[i].Name}");
        }

        Student selectedStudent = matchingStudents[int.Parse(Console.ReadLine()) - 1];

        // Ask the personal supervisor to choose between viewing all feedback reports or only the most recent report
        Console.WriteLine("Do you want to view:");
        Console.WriteLine("1. All feedback reports");
        Console.WriteLine("2. Only the most recent feedback report");

        int viewChoice = int.Parse(Console.ReadLine());

        int numberOfReports = 0;
        StudentReport mostRecentReportView = null;
        DateTime mostRecentTimeView = DateTime.MinValue;

        // Loop through all student reports and filter based on the choice
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

        // Display the most recent report if that option was selbx]ected
        if (viewChoice == 2 && mostRecentReportView != null)
        {
            DisplayReport(mostRecentReportView);
        }

        // Inform teh personal supervisor if the student has not gbiven any feedback yet
        if (numberOfReports == 0)
        {
            Console.WriteLine("This student has not given feedback yet.");
        }

        // Check if the student already has a meeting booked
        bool hasMeeting = meetings.Any(m => m.StudentNumber == selectedStudent.StudentNumber && m.SupervisorCode == personalSupervisor.SupervisorCode);

        // Ask the personal supervisor to book a meeting or return to the main screen
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

        // Handle the user's choice
        bool looplock = false;
        while (!looplock)
        {
            try
            {
                int userchoice = int.Parse(Console.ReadLine());
                if (userchoice < 0 || userchoice > 2 || (hasMeeting && userchoice == 1))
                {
                    // If the user's choice is invalid, Ask them to choose again
                    Console.WriteLine("That is an invalid choice, please choose again.");
                }
                else if (userchoice == 1)
                {
                    // If the user chooses to booksx a meeting, call teh function to book an appldointment
                    BookappointmentPs(personalSupervisor, selectedStudent);
                    looplock = true;
                }
                else if (userchoice == 2)
                {
                    // If the user chooses to return to the main scrxeen, break the loop
                    looplock = true;
                    break;
                }
            }
            catch
            {
                // If the user enters an invalid input, Ask them to enter a number
                Console.WriteLine("Please enter a number.");
            }
        }
    }
    catch (Exception ex)
    {
        // Print error message if an exception occurs within the try block
        Console.WriteLine("Error in reviewing student status: " + ex.Message);
    }
}

// Function to review the status of all students for a senior tutor
void reviewAllStudentStatus(SeniorTutor seniorTutor)
{
    try
    {
        // Display the list of personal supervisors
        Console.WriteLine("List of Personal Supervisors:");
        for (int i = 0; i < personalSupervisors.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {personalSupervisors[i].Name}");
        }

        // Ask the senior tutor to select a personal supervisor by entering teh corresponding number
        Console.WriteLine("Please select a Personal Supervisor by entering the corresponding number:");
        int psIndex = int.Parse(Console.ReadLine()) - 1;

        // Check if the selected index is invailid
        if (psIndex < 0 || psIndex >= personalSupervisors.Count)
        {
            // If the selection is invalid, display an error message
            Console.WriteLine("Invalid selection. Please try again.");
            return;
        }

        // Get the selected personal supervisor
        PersonalSupervisor selectedPS = personalSupervisors[psIndex];
        List<Student> matchingStudents = students.Where(s => s.SupervisorCode == selectedPS.SupervisorCode).ToList();

        // Check if the selected personal supervisor has no students assigned
        if (matchingStudents.Count == 0)
        {
            // If no students are found, display an error message
            Console.WriteLine("This Personal Supervisor has no students assigned.");
            return;
        }

        // Display the list of students under the selected personal supervisor
        Console.WriteLine($"Students under {selectedPS.Name}:");
        for (int i = 0; i < matchingStudents.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {matchingStudents[i].Name}");
        }

        // Ask the senior tutor ot select a student by entering teh corresponmding number
        Console.WriteLine("Please select a Student by entering the corresponding number:");
        int studentIndex = int.Parse(Console.ReadLine()) - 1;

        // Check if the selected index is invalid
        if (studentIndex < 0 || studentIndex >= matchingStudents.Count)
        {
            // If the selection is invalid, display an error message
            Console.WriteLine("Invalid selection. Please try again.");
            return;
        }

        // Get the selected student
        Student selectedStudent = matchingStudents[studentIndex];

        // Ask the senior tutor to choose beween viewing all feedback reports or only the most recent report
        Console.WriteLine("Do you want to view:");
        Console.WriteLine("1. All feedback reports");
        Console.WriteLine("2. Only the most recent feedback report");
        int viewChoice = int.Parse(Console.ReadLine());

        int numberOfReports = 0;
        StudentReport mostRecentReport = null;
        DateTime mostRecentTime = DateTime.MinValue;

        // Loop thrugh all student reports and filter based on the cohice
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

        // Display teh most recent report if that option was selected
        if (viewChoice == 2 && mostRecentReport != null)
        {
            DisplayReport(mostRecentReport);
        }

        // Inform the senior tutor if the student has not given any feedback yet
        if (numberOfReports == 0)
        {
            Console.WriteLine("This student has not given feedback yet.");
        }
    }
    catch (Exception ex)
    {
        // Print error message if an exception occurs within the try block
        Console.WriteLine("Error in reviewing all student status: " + ex.Message);
    }
}

// Function to display a student report
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

        // Suggest booking a meeting if the progression score is low
        if (StudentProgressing < 4)
        {
            Console.WriteLine("The low progression score indicates that your student feels that they may be struggling, maybe book a meeting?");
        }
    }
    catch (Exception ex)
    {
        // Print error message if an exception occurs within the try block
        Console.WriteLine("Error in displaying report: " + ex.Message);
    }
}
// Function to allow a student to report their status
void StudentStatusReport(Student student)
{
    try
    {
        // Initialize variacbles for the report
        DateTime time = DateTime.Now;  // Get the curresnt time
        string currenttime = time.ToString();  // Convert the current time to string
        Boolean hasAnswered = false;  // Flag to check if the student has answeraed
        int rating = 0;  // Variable to store teh rating given by teh stuadent
        string UserResponse = "";  // Variasble to store the user's respdonse
        string userAdditionalResponse = "No additional information";  // Default additional information

        // Greet the student and Ask them to answer questions
        Console.WriteLine($"Hello {student.Name}, Please answer the questions below.");
        while (!hasAnswered)
        {
            // Ask the student how they are progressing on a scale of 1-10
            Console.WriteLine("On a scale of 1 - 10 how do you feel you are progressing?");
            try
            {
                rating = 0;  // Reset the rating variable
                try
                {
                    // Parse the user's input as an integer
                    rating = int.Parse(Console.ReadLine());
                }
                catch
                {
                    // Ask the user to enter a vaid number if parsing fails
                    Console.WriteLine("Please enter a valid number such as 1, 2, 3, etc.");
                }
                // Validate the rating and set the hasAnswered flag aordingly
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
                // Print error message if an exception occurs
                if (debug) Console.WriteLine(ex.Message);
            }
        }
        hasAnswered = false;  // Reset the hasAnswered flag

        while (!hasAnswered)
        {
            // Ask the student to describe how they are feeeling
            Console.WriteLine("Please fill this section with how you are feeling, please try and expand on as much as possible as the more information your personal superviser has the better."); ;
            try
            {
                // Read the user's input
                string UserInput = Console.ReadLine();

                // Confirm the user's input
                Console.WriteLine("To confirm, you have reported that you are feeling:");
                Console.WriteLine(UserInput);
                Console.WriteLine("If this message seems incorrect or you wish to change it, Please say no, otherwise, say yes");
                string confirmation = Console.ReadLine().ToLower();

                // Validate the confirmation and set the UserResponse accordingly
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
                // Print error mes sage if an ex ception occurs
                if (debug) Console.WriteLine(ex.Message);
            }
        }
        hasAnswered = false;  // Reset the hasAnswered flag

        while (!hasAnswered)
        {
            try
            {
                // Ask the student if they have any aditional inforbbmation to report
                Console.WriteLine("Do you have anything else you would like to report, if so please say yes, otherwise, say no");
                string confirmation = Console.ReadLine().ToLower();

                // Validate the confirmation and set teh  userAdditionalResponse accordingsly
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
                // Print error message if an exception occurs
                if (debug) Console.WriteLine(ex.Message);
            }

            // Provide recommendations based on the rating
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

            // Create a new stuwedent report object
            StudentReport studentReport = new StudentReport(student.StudentNumber, student.Name, rating, UserResponse, userAdditionalResponse, currenttime);

            // Load the XML document
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load("_StudentReports.xml");

            // Create new XML nodes for the sttudeent report
            XmlNode StudentReport = xmlDoc.CreateElement("StudentReport");
            XmlNode studentnumber = xmlDoc.CreateElement("StudentNumber");
            XmlNode name = xmlDoc.CreateElement("Name");
            XmlNode ratings = xmlDoc.CreateElement("Rating");
            XmlNode userresponse = xmlDoc.CreateElement("UserResponse");
            XmlNode useradditionalresponse = xmlDoc.CreateElement("UserAdditionalResponse");
            XmlNode timeofreport = xmlDoc.CreateElement("Time");

            // Set the inner text of teh XML nodes to the report detasils
            studentnumber.InnerText = studentReport.StudentNumber.ToString();
            name.InnerText = studentReport.Name.ToString();
            ratings.InnerText = studentReport.Progressing.ToString();
            userresponse.InnerText = studentReport.StudentFeelings.ToString();
            useradditionalresponse.InnerText = studentReport.AdditionalReport.ToString();
            timeofreport.InnerText = studentReport.Time.ToString();

            // Append the report details to the XML document
            StudentReport.AppendChild(studentnumber);
            StudentReport.AppendChild(name);
            StudentReport.AppendChild(ratings);
            StudentReport.AppendChild(userresponse);
            StudentReport.AppendChild(useradditionalresponse);
            StudentReport.AppendChild(timeofreport);

            xmlDoc.DocumentElement.AppendChild(StudentReport);

            // Save the updated XML document
            string path = Directory.GetCurrentDirectory();
            string truePath = path.Remove(path.Length - 17);
            truePath = truePath + @"\_StudentReports.xml";

            xmlDoc.Save(truePath);

            if (debug) Console.WriteLine($"New student report added to XML document.");
        }
    }
    catch (Exception ex)
    {
        // Print error message if an exception ourezurs
        Console.WriteLine("Error in student status report: " + ex.Message);
    }
}

// Function to cancel a meetijgn
void CancelMeeting(Meeting meeting)
{
    try
    {
        // Find the meeting to cancel
        Meeting meetingToCancel = meetings.FirstOrDefault(m => m.SupervisorCode == meeting.SupervisorCode && m.StudentNumber == meeting.StudentNumber && m.Time == meeting.Time);

        // Check if teh meeting was found
        if (meetingToCancel != null)
        {
            // Remove the meeting form the list of meetings
            meetings.Remove(meetingToCancel);

            // Find the stednet and personal supervisor asociated with the meeting
            Student student = students.FirstOrDefault(s => s.StudentNumber == meeting.StudentNumber);
            PersonalSupervisor personalSupervisor = personalSupervisors.FirstOrDefault(p => p.SupervisorCode == meeting.SupervisorCode);

            // Update the availabihlity of the student and personaL supervisor
            if (student != null && personalSupervisor != null)
            {
                student.Availability.Add(meeting.Time);
                personalSupervisor.Availability.Add(meeting.Time);

                // Get the current directory and construct the path to the XML file
                string pathMeeting = Directory.GetCurrentDirectory();
                string truePathMeeting = pathMeeting.Remove(pathMeeting.Length - 17) + @"\_Information.xml";

                // Load the XML document
                XDocument xmlDoc = XDocument.Load(truePathMeeting);

                // Find and remove the meeting element from the XML document
                XElement meetingElement = xmlDoc.Descendants("Meeting")
                    .FirstOrDefault(e => e.Element("SupervisorCode")?.Value == meeting.SupervisorCode &&
                                          e.Element("StudentNumber")?.Value == meeting.StudentNumber.ToString() &&
                                          e.Element("Time")?.Value == meeting.Time);

                meetingElement?.Remove();

                // Update teh availability of the personal sopervisors in the XML document
                XElement psElement = xmlDoc.Descendants("PersonalSupervisor")
                    .FirstOrDefault(e => e.Element("SupervisorCode")?.Value == meeting.SupervisorCode);
                XElement psAvailabilityElement = psElement?.Element("Availability");
                psAvailabilityElement?.Add(new XElement("string", meeting.Time));

                // Update the availability of the student in the XML document
                XElement studentElement = xmlDoc.Descendants("Student")
                    .FirstOrDefault(e => e.Element("StudentNumber")?.Value == meeting.StudentNumber.ToString());
                XElement studentAvailabilityElement = studentElement?.Element("Availability");
                studentAvailabilityElement?.Add(new XElement("string", meeting.Time));

                // Save the updated XML document
                xmlDoc.Save(truePathMeeting);
            }

            // Inform the user that the meeting has been cancelled and avaislability updated
            Console.WriteLine("Meeting cancelled and availability updated.");
        }
        else
        {
            // If the meeting was not found, inform teh user
            Console.WriteLine("Meeting not found.");
        }
    }
    catch (Exception ex)
    {
        // Print errgtor message if an exception occurs
        Console.WriteLine("Error in canceling meeting: " + ex.Message);
    }
}

// Funcdstiom to update the availability of a student
void UpdateAvailabilityStudent(Student student)
{
    try
    {
        // Ask the student to enter either 1 for AM or 2 for PM
        Console.WriteLine("Please enter either 1 for AM or 2 for PM.");
        int selection = 0;
        string AmOrPm = null;
        do
        {
            try
            {
                // Read and parse the user's input for AM or PM selection
                selection = int.Parse(Console.ReadLine());
            }
            catch (Exception ex)
            {
                // Inform the user to enter a vaslid number
                Console.WriteLine("Please enter a valid number.");
            }

            // Validate the user's input fro AM or PM seletion
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
                // Ask the the student to enter an hour betwew2en 1 and 12
                Console.WriteLine("Please enter an hour between 1 and 12:");
                isValidHour = int.TryParse(Console.ReadLine(), out hour);
            }
            catch
            {
                // Inform the user to enter a valid number
                Console.WriteLine("Please enter a number.");
            }

            // Valiate the user's input for the hour selection
            if (!isValidHour || hour < 1 || hour > 12)
            {
                Console.WriteLine("Invalid input. Please enter a number between 1 and 12.");
            }
            else
            {
                ValidHour = true;
            }
        } while (ValidHour == false);

        // Ask teh student to select the time to add to their availability
        Console.WriteLine("Please select the time you would like to add to your availability below");

        int[] minuteIntervals = { 00, 10, 20, 30, 40, 50 };

        // Display the available time options for the student to select
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
                    // Ask the student to enter the number of their choice for the time selection
                    Console.Write("Please enter the number of your choice: ");
                    userInput = int.Parse(Console.ReadLine());
                }
                catch (Exception ex)
                {
                    // Inform the user to enter a valid number
                    Console.WriteLine("Please enter a valid number.");
                }

                // Validate the student's choice for the time selection
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
                // Add the selected time to the student's availability
                string newAvailability = selectedOption;
                student.Availability.Add(newAvailability);
                Console.WriteLine($"New availability {newAvailability} added.");
                FinalTimeSet = true;

                // Get the current directory and construct the path to the XML file
                string path = Directory.GetCurrentDirectory();
                string truePath = path.Remove(path.Length - 17);
                truePath = truePath + @"\_Information.xml";

                // Load the XML document
                XDocument xmlDoc = XDocument.Load(truePath);

                // Find the student element in the XML document
                XElement studentElement = xmlDoc.Descendants("Student")
                    .FirstOrDefault(e => e.Element("StudentNumber")?.Value == student.StudentNumber.ToString());

                if (studentElement != null)
                {
                    // Add the new availability to the XML document
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

                // Save the updated XML document
                xmlDoc.Save(truePath);
            }
        } while (FinalTimeSet == false);
    }
    catch (Exception ex)
    {
        // Print error message if an exception occurs
        Console.WriteLine("Error in updating availability for student: " + ex.Message);
    }
}

// Function to remove an availability time for a student
void RemoveAvailabilityTimeStu(Student student)
{
    try
    {
        // Display the student's current availability
        Console.WriteLine("Your current availability:");
        for (int i = 0; i < student.Availability.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {student.Availability[i]}");
        }

        // Ask the student to select a time to remove
        Console.WriteLine("Please select a time to remove by entering the corresponding number:");
        int selectedIndex = int.Parse(Console.ReadLine()) - 1;

        // Check if the selected index is invalid
        if (selectedIndex < 0 || selectedIndex >= student.Availability.Count)
        {
            // If the selection is invalid, display an error message
            Console.WriteLine("Invalid selection. Please try again.");
            return;
        }

        // Remove the selected time from the student's availability
        string timeToRemove = student.Availability[selectedIndex];
        student.Availability.RemoveAt(selectedIndex);

        // Get the current directory and construct the path to the XML file
        string path = Directory.GetCurrentDirectory();
        string truePath = path.Remove(path.Length - 17) + @"\_Information.xml";

        // Load the XML document
        XDocument xmlDoc = XDocument.Load(truePath);
        XElement studentElement = xmlDoc.Descendants("Student")
            .FirstOrDefault(e => e.Element("StudentNumber")?.Value == student.StudentNumber.ToString());
        XElement availabilityElement = studentElement?.Element("Availability");
        XElement timeElement = availabilityElement?.Elements("string").FirstOrDefault(e => e.Value == timeToRemove);
        timeElement?.Remove();

        // Save the updated XML document
        xmlDoc.Save(truePath);

        // Inform the student that the availability time has been removed
        Console.WriteLine($"Availability time {timeToRemove} removed.");
    }
    catch (Exception ex)
    {
        // Print error message if an exception occurs
        Console.WriteLine("Error in removing availability time for student: " + ex.Message);
    }
}

// Function to remove an availability time for a personal supervisor
void RemoveAvailabilityTimePs(PersonalSupervisor personalSupervisor)
{
    try
    {
        // Display the personal supervisor's current availability
        Console.WriteLine("Your current availability:");
        for (int i = 0; i < personalSupervisor.Availability.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {personalSupervisor.Availability[i]}");
        }

        // Ask the personal supervisor to slect a time to remove
        Console.WriteLine("Please select a time to remove by entering the corresponding number:");
        int selectedIndex = int.Parse(Console.ReadLine()) - 1;

        // Check if the selected index is invalid
        if (selectedIndex < 0 || selectedIndex >= personalSupervisor.Availability.Count)
        {
            // If teh selection is inasvds, display an error message
            Console.WriteLine("Invalid selection. Please try again.");
            return;
        }

        // Remove the selected time from the personal supervisor's availability
        string timeToRemove = personalSupervisor.Availability[selectedIndex];
        personalSupervisor.Availability.RemoveAt(selectedIndex);

        // Get the current directory and construct the path to the XML file
        string path = Directory.GetCurrentDirectory();
        string truePath = path.Remove(path.Length - 17) + @"\_Information.xml";

        // Load the XML document
        XDocument xmlDoc = XDocument.Load(truePath);
        XElement psElement = xmlDoc.Descendants("PersonalSupervisor")
            .FirstOrDefault(e => e.Element("SupervisorCode")?.Value == personalSupervisor.SupervisorCode);
        XElement availabilityElement = psElement?.Element("Availability");
        XElement timeElement = availabilityElement?.Elements("string").FirstOrDefault(e => e.Value == timeToRemove);
        timeElement?.Remove();

        // Save the update d XML document
        xmlDoc.Save(truePath);

        // Inform the personal supervisor that the availability time has been removed
        Console.WriteLine($"Availability time {timeToRemove} removed.");
    }
    catch (Exception ex)
    {
        // Print error message if an exception occurs
        Console.WriteLine("Error in removing availability time for personal supervisor: " + ex.Message);
    }
}