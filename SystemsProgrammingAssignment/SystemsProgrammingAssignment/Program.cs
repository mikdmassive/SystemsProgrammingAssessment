using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemsProgrammingAssignment
{
    internal class Program
    {
        //data
      
        static List<Vehicle> vehicles = new List<Vehicle>() { };
        static List<User> users = new List<User>() { };
        static List<Booking> bookings = new List<Booking>() { };



        //class handling
        public static Dictionary<string, string> DisplayAndReturnVehicleDictionary(List<Vehicle> VehicleList)
        {
            Dictionary<string, string> vehicleDictionary = new Dictionary<string, string>();
            for (int i = 0; i < VehicleList.Count; i++)
            {
                Vehicle v = VehicleList[i];
                string vehicleNumber = (i+1).ToString();
                vehicleDictionary.Add(vehicleNumber, v.ReturnVehicleID());
                Console.WriteLine($"{vehicleNumber}) {v.ReturnBasicVehicleInfoInList()}");
            }
            return vehicleDictionary;
        }
        public static Vehicle SelectVehicleFromGivenList(List<Vehicle> VehicleList)
        {
            Vehicle SelectedVehicle = null;
            Dictionary<string, string> vehicleDictionary = DisplayAndReturnVehicleDictionary(VehicleList);
            if (vehicleDictionary.Count>0)
            {
                while (SelectedVehicle==null)
                {
                    Console.WriteLine("Please select a vehicle via corrosponding number, or type \"exit\" to stop:");
                    string userInput = Console.ReadLine();
                    if (userInput != null)
                    {
                        if (userInput.ToLower() == "exit")
                        {
                            break;
                        }
                        else if (vehicleDictionary.ContainsKey(userInput))
                        {
                            string selectedVehicleID = vehicleDictionary[userInput];
                            if (GetVehicleFromID(selectedVehicleID)!=null)
                            {
                                SelectedVehicle = GetVehicleFromID(selectedVehicleID);
                            }
                            else
                            {
                                Console.WriteLine("Error - Vehicle not found");
                            }
;

                        }
                        else
                        {
                            Console.WriteLine("Invalid selection. Please try again.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Please enter something.");
                    }
                }
            }
            else
            {
                Console.WriteLine("No vehicles found under these conditions.");
            }
            return SelectedVehicle;
        }
        public static Vehicle GetVehicleFromID(string ID)
        {
            for (int i = 0; i < vehicles.Count; i++)
            {
                Vehicle v = vehicles[i];
                if (v.ReturnVehicleID()==ID)
                {
                    return v;
                }
            }
            return null;
        }

        public static void BeginVehicleDataLoading()
        {
            DummyData dummyData = new DummyData();
            vehicles = dummyData.vehicles; //DUMMY DATA DELETE LATER
        }

        //catch verfications
        public static int TryConvertToInt(string input,int defaultNumber)
        {
            try
            {
                return Convert.ToInt32(input);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return defaultNumber;
            }
        }
        //general
        public static void TitleCard()
        {
            Console.WriteLine("============================================");
            Console.WriteLine("            ---  AUTORENTER  ---            ");
            Console.WriteLine("============================================");
        }

        //main
        static void Main(string[] args)
        {
            User loggedinUser = null;
            //MANAGERS
            PasswordService passwordService = new PasswordService();
            UserService userService = new UserService();
            MembershipManager membershipManager = new MembershipManager();
            BookingManager bookingManager = new BookingManager();
            //INTERNAL FUNCTIONS 
            void LogIn()
            {
                Console.Clear();
                TitleCard();
                Console.WriteLine("          ---  LOG IN SCREEN  ---          \n");
                Console.WriteLine("Enter your username:");
                string id = null;
                while (id==null)
                {
                    id = userService.GetUserIDFromUsername(users,Console.ReadLine());
                    if (id==null)
                    {
                        Console.WriteLine("User not found.");
                    }
                }
                User user = users.FirstOrDefault(u => u.ReturnUserID() == id);
                if (user != null)
                {
                    Console.WriteLine("Enter your password:");
                    while (true)
                    {
                        string password = Console.ReadLine();
                        if (passwordService.VerifyHashedPassword(password, user.ReturnPasswordHash()))
                        {
                            loggedinUser = user;
                            Console.WriteLine("Login successful!");
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Incorrect password, try again");
                        }
                    }
                    
                }
                else {Console.WriteLine("Error - User not found"); }
            }
            void AccountCreation()
            {
                Console.Clear();
                TitleCard();
                Console.WriteLine("     ---  ACCOUNT CREATION SCREEN  ---     \n");
                string username = "";
                Console.WriteLine(userService.ReturnUsernameRequirements());
                while (true)
                {
                    
                    username = Console.ReadLine();
                    if (userService.VerifyUsername(username,users))
                    {
                        break;
                    }
                }
                string password = "";
                Console.WriteLine(passwordService.ReturnPasswordRequirements());
                while (true)
                {

                    password = Console.ReadLine();
                    if (passwordService.VerifyPassword(password))
                    {
                        password = passwordService.HashPassword(password);
                        break;
                    }
                }
                User createdUser = new User(userService.GenerateID(users), username, password, 0, 1);
                users.Add(createdUser);
                loggedinUser = createdUser;
                Console.WriteLine("Account Created Successfully!");
                //SAVE HERE

            }
            void VehicleSelection()
            {
                bool looping = true;
                List<Vehicle> filteredVehicles = null; ; //TODO filter vehicles based on user preferences

                while (looping)
                {
                    Console.Clear();
                    TitleCard();
                    Console.WriteLine("     ---  VEHICLE SELECTION SCREEN  ---     \n");
                    Console.WriteLine($"1) Select vehicles from list\n2) Adjust filters \n3) Return to main menu");
                    string input = Console.ReadLine();
                    int caseInput = TryConvertToInt(input, -1);
                    switch (caseInput)
                    {
                        case 1:
                            if (filteredVehicles == null)
                            {
                                Vehicle vehicle = SelectVehicleFromGivenList(vehicles);
                                if(vehicle!=null)
                                {
                                    //vehicle selected screen
                                    string vehicleMembership = membershipManager.memberships[vehicle.ReturnMembershipLevel()].ReturnName().ToUpper();
                                    Console.Clear();
                                    TitleCard();
                                    Console.WriteLine($"     ---  YOU HAVE SELECTED {vehicle.ReturnRegPlate()} ---     \nMAKE - {vehicle.ReturnMake()}\nMODEL - {vehicle.ReturnModel()}");
                                    if(vehicle is Car car){Console.WriteLine($"NUMBER OF SEATS - {car.ReturnNumberOfSeats()}");}
                                    else if(vehicle is HGV hgv){Console.WriteLine($"MAX LOAD WEIGHT - {hgv.ReturnMaxLoadWeight()}");}
                                    else if(vehicle is Motorcycle motorcycle){Console.WriteLine($"MAX LOAD WEIGHT - {motorcycle.ReturnEngineCapacity()}");}
                                    Console.WriteLine($"MEMBERSHIP REQUIRED - {vehicleMembership}\nCOST PER HOUR - £{vehicle.ReturnCostPerHour().ToString("0.00")}");
                                    if (loggedinUser.ReturnMembershipLevel()>=vehicle.ReturnMembershipLevel())
                                    {
                                        //membership valid
                                        //check if vehicle booked 
                                        if (!bookingManager.IsVehicleAlreadyBooked(vehicle,bookings))
                                        {
                                            //TODO, CHECK IF USER CURRENT BOOKING
                                            Console.WriteLine("need to if u booked check lol");
                                            Console.ReadLine();
                                        }
                                        else
                                        {
                                            Console.WriteLine("UNABLE TO BOOK - VEHICLE ALREADY BOOKED");
                                            Console.ReadLine();

                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine($"UNABLE TO BOOK - NEED {vehicleMembership} MEMBERSHIP OR HIGHER");
                                        Console.ReadLine();
                                    }

                                }
                                else
                                {
                                    Console.WriteLine("Error - Vehicle not found");

                                }
                            }
                            else
                            {
                                SelectVehicleFromGivenList(filteredVehicles);
                            }
                            break;
                        case 2:
                            Console.WriteLine("Filter adjustment not yet implemented.");
                            break;
                        case 3:
                            looping = false;
                            Console.WriteLine("Returning to main menu...");
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Please try again. Press ENTER to continue.");
                            Console.ReadLine();
                            break;

                    }
                }
                
            }

            TitleCard();
            //ARGUMENTS
            if (args.Length>0) 
            {

            }
            else
            {
                Console.WriteLine("No arguments provided, press any key to start");
                Console.ReadLine();
            }
            BeginVehicleDataLoading();
            
            //CORE LOOP
            
            //TODO login
            while (true)
            {
                bool exit = false;
                Console.Clear();
                TitleCard();
                //LOGGED OUT
                if (loggedinUser == null)
                {
                    Console.WriteLine("1) Create account\n2) Log in");
                    string input = Console.ReadLine();
                    int caseInput = TryConvertToInt(input, -1);
                    switch (caseInput)
                    {
                        case 1:
                            AccountCreation();
                            break;
                        case 2:
                            LogIn();
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Please try again.");
                            break;

                    }
                }
                //logged in
                else
                {
                    Console.WriteLine($"Welcome, {loggedinUser.ReturnUsername()}! You are using Autorenter as a {membershipManager.memberships[loggedinUser.ReturnMembershipLevel()].ReturnName()} member.\n1) Select a vehicle\n2) Log out");
                    string input = Console.ReadLine();
                    int caseInput = TryConvertToInt(input, -1);
                    switch (caseInput)
                    {
                        case 1:
                            VehicleSelection();
                            break;
                        case 2:
                            loggedinUser = null;
                            Console.WriteLine("Logged out successfully!");
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Please try again.");
                            break;

                    }
                    
                }
                if (!exit)
                {
                    Console.WriteLine("PRESS ENTER TO CONTINUE");
                    Console.ReadLine();

                }


            }
            

        }
    }
}
