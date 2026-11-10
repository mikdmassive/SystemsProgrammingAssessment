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
                Console.ReadLine();
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
        public static float TryConvertToFloat(string input, float defaultNumber)
        {
            try
            {
                return Convert.ToSingle(input);
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
            void BookVehicle(Vehicle vehicle)
            {
                int minbookinghour = bookingManager.ReturnMinBookingTimeInHours();
                int maxbookinghour = bookingManager.ReturnMaxBookingTimeInHours();
                Console.WriteLine($"Please enter how long you would like to book the vehicle for in hours ({minbookinghour}-{maxbookinghour}), or type EXIT to cancel:");
                bool looping = true;
                while (looping)
                {
                    string input = Console.ReadLine();
                    if (!string.IsNullOrEmpty(input))
                    {
                        if (input == "CANCEL")
                        {
                            looping = false;
                        }
                        else
                        {

                            int intinput = TryConvertToInt(input, -1);
                            if (intinput>=minbookinghour&&intinput<=maxbookinghour)
                            {
                                looping = false;

                                //prepare booking
                                Console.WriteLine($"THIS WILL COST £{(vehicle.ReturnCostPerHour()*intinput).ToString("0.00")}\n1) Confirm\n2) Cancel");
                                bool bookingLoop = true;

                                while (bookingLoop)
                                {
                                    input = Console.ReadLine();
                                    int caseInput = TryConvertToInt(input, -1);
                                    switch (caseInput)
                                    {
                                        case 1:
                                            Booking booking = bookingManager.CreateBooking(loggedinUser, vehicle, intinput, bookings);
                                            bookings.Add(booking);
                                            Console.WriteLine("Booking Confirmed!");
                                          
                                            bookingLoop = false;
                                            break;
                                        case 2:
                                            bookingLoop = false;
                                            break;
                                        default:
                                            Console.WriteLine("Invalid selection. Please try again.");
                                            break;

                                    }
                                }
                            }
                            else { Console.WriteLine($"Invalid,please enter a time frame between {minbookinghour}-{maxbookinghour}"); }
                        }
                    }
                    else { Console.WriteLine("Invalid,Try Again"); }
                }
            }
            void VehicleSelection()
            {
                bool looping = true;
               
                int membershipFilter =-1;
                string makeFilter = "";
                float priceFilter = -1;
                while (looping)
                {
                    List<Vehicle> filteredVehicles = vehicles;
                 

                    Console.Clear();
                    TitleCard();
                    Console.WriteLine("     ---  VEHICLE SELECTION SCREEN  ---     \n");
                    //apply filters 
                    if (membershipFilter != -1 || makeFilter != "" || priceFilter != -1)
                    {
                        Console.WriteLine("FILTERS APPLIED:");
                        if (membershipFilter != -1)
                        {
                            filteredVehicles = filteredVehicles.Where(v => v.ReturnMembershipLevel() == membershipFilter).ToList();
                            Console.WriteLine($" - MEMBERSHIP: {membershipManager.memberships[membershipFilter].ReturnName()}");
                        }
                        if (makeFilter != "")
                        {
                            filteredVehicles = filteredVehicles.Where(v => v.ReturnMake().ToLower() == makeFilter.ToLower()).ToList();
                            Console.WriteLine($" - VEHICLE MAKE: {makeFilter.ToUpper()}");

                        }
                        if (priceFilter != -1)
                        {
                            filteredVehicles = filteredVehicles.Where(v => v.ReturnCostPerHour() <= priceFilter).ToList();
                            Console.WriteLine($" - MAX PRICE: £{priceFilter.ToString("0.00")}");
                        }
                        Console.WriteLine();
                    }
  
                    
                    Console.WriteLine($"1) Select vehicles from list\n2) Adjust filters \n3) Return to main menu");

                    string input = Console.ReadLine();
                    int caseInput = TryConvertToInt(input, -1);
                    switch (caseInput)
                    {
                        case 1:
                            Vehicle vehicle = SelectVehicleFromGivenList(filteredVehicles);
                            if (vehicle != null)
                            {
                                //vehicle selected screen
                                string vehicleMembership = membershipManager.memberships[vehicle.ReturnMembershipLevel()].ReturnName().ToUpper();
                                Console.Clear();
                                TitleCard();
                                Console.WriteLine($"     ---  YOU HAVE SELECTED {vehicle.ReturnRegPlate()} ---     \nMAKE - {vehicle.ReturnMake()}\nMODEL - {vehicle.ReturnModel()}");
                                if (vehicle is Car car) { Console.WriteLine($"NUMBER OF SEATS - {car.ReturnNumberOfSeats()}"); }
                                else if (vehicle is HGV hgv) { Console.WriteLine($"MAX LOAD WEIGHT - {hgv.ReturnMaxLoadWeight()}kg"); }
                                else if (vehicle is Motorcycle motorcycle) { Console.WriteLine($"MAX LOAD WEIGHT - {motorcycle.ReturnEngineCapacity()}cc"); }
                                Console.WriteLine($"MEMBERSHIP REQUIRED - {vehicleMembership}\nCOST PER HOUR - £{vehicle.ReturnCostPerHour().ToString("0.00")}");
                                if (loggedinUser.ReturnMembershipLevel() >= vehicle.ReturnMembershipLevel())
                                {
                                    //membership valid
                                    //check if vehicle booked 
                                    if (!bookingManager.IsVehicleAlreadyBooked(vehicle, bookings))
                                    {
                                        //check if user has booking
                                        if (!bookingManager.IsUserAlreadyGotABooking(loggedinUser, bookings))
                                        {
                                            //valid
                                            Console.WriteLine("AVAILABLE TO BOOK\n1) Start Booking\n2) Cancel");

                                            bool bookingLoop = true;
                                            while (bookingLoop)
                                            {
                                                input = Console.ReadLine();
                                                caseInput = TryConvertToInt(input, -1);
                                                switch (caseInput)
                                                {
                                                    case 1:
                                                        BookVehicle(vehicle);
                                                        bookingLoop = false;
                                                        break;
                                                    case 2:
                                                        bookingLoop = false;
                                                        break;
                                                    default:
                                                        Console.WriteLine("Invalid selection. Please try again.");
                                                        break;

                                                }
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("UNABLE TO BOOK - YOU ALREADY HAVE AN ACTIVE BOOKING");
                                            Console.ReadLine();

                                        }
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
                            break;
                        case 2:
                            Console.WriteLine("Please select a filter:\n1) Remove filters \n2) Membership\n3) Search Make\n4) Maximum Price");
                            string filterInput = Console.ReadLine();
                            int casefilterInput = TryConvertToInt(filterInput, -1);
                            switch (casefilterInput)
                            {
                                case 1:
                                    membershipFilter = -1;
                                    priceFilter = -1;
                                    makeFilter = "";
                                    Console.WriteLine("Filters removed!");
                                    break;
                                case 2:
                                    Console.WriteLine($"Select a membership:");
                                    foreach (Membership membership in membershipManager.ReturnMemberships())
                                    {
                                        Console.WriteLine($"{membership.ReturnMembershipLevel()}) {membership.ReturnName()}");
                                    }
                                    string filterInputMembership = Console.ReadLine();
                                    int casefilterInputMembership = TryConvertToInt(filterInputMembership, -1);
                                    if (membershipManager.memberships.ContainsKey(casefilterInput))
                                    {
                                        membershipFilter = casefilterInputMembership;
                                        Console.WriteLine("Filter applied!");
                                    }
                                    else { Console.WriteLine("Invalid Input."); }

                                    break;
                                case 3:
                                    Console.WriteLine("Please enter a make to search for:");
                                    string makeInput = Console.ReadLine();
                                    if (!string.IsNullOrWhiteSpace(makeInput)){
                                        makeFilter = makeInput;
                                        Console.WriteLine("Filter applied!");

                                    }
                                    else { Console.WriteLine("Invalid Input."); }
                                    break;
                                case 4:
                                    Console.WriteLine("Enter a maximum per hour price:");
                                    string priceInput = Console.ReadLine();
                                    float price = TryConvertToFloat(priceInput, -1);
                                    if (price >= 5&&price<=1000)
                                    {
                                        priceFilter = price;
                                        Console.WriteLine("Filter applied!");

                                    }
                                    else { Console.WriteLine("Invalid Input. Enter a price £5-1000"); }
                                    break;
                                default:
                                    Console.WriteLine("Invalid selection.");
                                    break;
                            }
                            Console.WriteLine("PRESS ENTER TO CONTINUE");
                            Console.ReadLine();
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
                    Console.WriteLine($"Welcome, {loggedinUser.ReturnUsername()}! You are using Autorenter as a {membershipManager.memberships[loggedinUser.ReturnMembershipLevel()].ReturnName()} member.\n1) Select a vehicle\n2) Log out\n3) Check active booking\n4) Return previous bookings");
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
                        case 3:
                            Booking activeBooking = bookingManager.returnUserActiveBooking(loggedinUser, bookings);
                            if (activeBooking != null)
                            {
                                Vehicle v = GetVehicleFromID(activeBooking.ReturnVehicleID());
                                if (v != null)
                                {
                                    Console.WriteLine($"{v.ReturnBasicVehicleInfoInList()}\n{activeBooking.ReturnBookingInfo()}");
                                }
                            }
                            else { Console.WriteLine("You have no active booking."); }
                            break;
                        case 4:
                            List<Booking> usersBookings = bookingManager.returnUsersPreviousBookings(loggedinUser, bookings);
                            if (usersBookings.Count>0)
                            {
                                for (int i = 0;i<usersBookings.Count;i++)
                                {
                                    Booking booking = usersBookings[i];
                                    Vehicle v = GetVehicleFromID(booking.ReturnVehicleID());
                                    if (v!=null)
                                    {
                                        Console.WriteLine($"{i + 1}) {v.ReturnBasicVehicleInfo()}");
                                        //TODO MORE INFO AND CHECK ACTIVE
                                    }
                                }
                            }
                            else { Console.WriteLine("No previous bookings found"); }
                          
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
