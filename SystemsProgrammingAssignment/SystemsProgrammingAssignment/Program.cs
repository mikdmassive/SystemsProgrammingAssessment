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
        static bool usedummydata = true;//change false later
        static bool createadmin = false;



        //class handling

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
        public static User GetUserFromID(string id) { return users.FirstOrDefault(u => u.ReturnUserID() == id); }
        
        public static void LoadAllData()
        {
            if (!usedummydata)//REAL DATA
            {

            }
            else//dummy data
            {
                DummyData dummyData = new DummyData();
                vehicles = dummyData.vehicles; 
            }
        }
       
        public static void SaveAll()
        {
            if (!usedummydata)
            {

            }
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
            //CLASS MANAGEMENT
            Vehicle SelectVehicleFromGivenList(List<Vehicle> VehicleList)
            {
                Vehicle SelectedVehicle = null;
                Dictionary<string, string> vehicleDictionary = DisplayAndReturnVehicleDictionary(VehicleList);
                if (vehicleDictionary.Count > 0)
                {
                    while (SelectedVehicle == null)
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
                                if (GetVehicleFromID(selectedVehicleID) != null)
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

            Dictionary<string, string> DisplayAndReturnVehicleDictionary(List<Vehicle> VehicleList)
            {
                Dictionary<string, string> vehicleDictionary = new Dictionary<string, string>();
                for (int i = 0; i < VehicleList.Count; i++)
                {
                    Vehicle v = VehicleList[i];
                    string vehicleNumber = (i + 1).ToString();
                    vehicleDictionary.Add(vehicleNumber, v.ReturnVehicleID());
                    Console.WriteLine($"{vehicleNumber}) {v.ReturnBasicVehicleInfoInList()} ({membershipManager.ReturnMembership(v.ReturnMembershipLevel()).ReturnName().ToUpper()}+)");
                }
                return vehicleDictionary;
            }
            //INTERNAL FUNCTIONS 
            void AdminMenu()
            {
                bool looping = true;
                while (looping)
                {
                    Console.Clear();
                    TitleCard();
                    Console.WriteLine("          --  ADMIN SCREEN  --        \n1) Confirm Vehicle Return\n2) Vehicle Manager\n3) User Manager\n4) Exit");
                    string input = Console.ReadLine();
                    int caseInput = TryConvertToInt(input, -1);
                    switch (caseInput)
                    {
                        case 1:
                            List<Booking> confirmingbookings = bookings.Where(b => b.IsAwaitingReturnVerification()).ToList();
                            Dictionary<int,Booking> confirmBookingDictionary = new Dictionary<int, Booking>() { };
                            foreach (Booking booking in confirmingbookings)
                            {
                                int key = confirmBookingDictionary.Count() + 1;
                                confirmBookingDictionary.Add(key, booking);
                                Vehicle v = GetVehicleFromID(booking.ReturnVehicleID());
                                User u = GetUserFromID(booking.ReturnUserID());
                                if (v!=null&& u!=null)
                                {
                                    Console.WriteLine($"{key}) {v.ReturnBasicVehicleInfo()} booked by {u.ReturnUsername()} - Claims return at {booking.ReturnTimeReturned()}");
                                }
                            }
                            if (confirmBookingDictionary.Count > 0)
                            {
                                Console.WriteLine("Please select a vehicle from this list using the corisponding number.");
                                caseInput = TryConvertToInt(input, -1);
                                if (confirmBookingDictionary.ContainsKey(caseInput))
                                {
                                    Booking booking = confirmBookingDictionary[caseInput];
                                    Console.WriteLine("Is this vehicle actually returned?\n1) Yes\n2) No");
                                    input = Console.ReadLine();
                                    caseInput = TryConvertToInt(input, -1);
                                    switch (caseInput)
                                    {
                                        case 1:
                                            booking.VerifyReturn();
                                            SaveAll();

                                            break;
                                        case 2:
                                            booking.SetReturnTime(null);
                                            SaveAll();

                                            break;
                                        default:
                                            Console.WriteLine("Invalid selection. Please try again.");
                                            break;
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Error, booking not found.");
                                }
                                
                            }
                            else { Console.WriteLine("No booking waiting for return confirmation"); }
                            Console.WriteLine("PRESS ENTER TO CONTINUE");
                            Console.ReadLine();
                            break;
                        case 2:
                            //vehicle management

                            break;
                        case 3:

                            break;
                        case 4:
                            looping = false;
                            break;
                        default:
                            Console.WriteLine("Invalid selection. Please try again.");
                            break;

                    }
                }
            }
            void ManageMembership()
            {
                bool looping = true;
                while (looping)
                {
                    Console.Clear();
                    TitleCard();
                    Console.WriteLine("          --  MEMBERSHIP MANAGER  --        \n");
                    Console.WriteLine($"Select a membership:");
                    foreach (Membership membership in membershipManager.ReturnMemberships())
                    {
                        Console.WriteLine($"{membership.ReturnMembershipLevel()}) {membership.ReturnInfo()}");
                    }
                    Console.WriteLine("Or type \"exit\" to exit");
                    string input = Console.ReadLine();
                    int caseInput = TryConvertToInt(input, -1);
                    if (membershipManager.memberships.ContainsKey(caseInput))
                    {
                        Membership membership = membershipManager.ReturnMembership(caseInput);
                        Console.WriteLine($"You have selected {membership.ReturnName()} membership. This will cost {membership.ReturnPrice().ToString("0.00")} per month. Would you like to purchase this?\n (THIS IS A MOCK PURCHASE, THIS WILL NOT CHARGE YOU)\n1) Yes\n2) No");
                        string purchaseInput = Console.ReadLine();
                        if (purchaseInput == "1" || purchaseInput.ToLower()=="yes")
                        {
                            loggedinUser.SetMembershipLevel(membership.ReturnMembershipLevel());
                            Console.WriteLine($"You have successfully purchased {membership.ReturnName()} membership!");
                            SaveAll();
                        }
                        else
                        {
                            Console.WriteLine("Purchase cancelled.");
                        }

                    }
                    else if (!string.IsNullOrEmpty(input))
                    {
                        if (input.ToLower() == "exit")
                        {
                            looping = false;
                        }
               
                    }
                    else
                    {
                        Console.WriteLine("Invalid selection. Please try again.");
                    }
                    Console.WriteLine("Press ENTER to continue.");
                    Console.ReadLine();
                }
            }

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
                SaveAll();

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
                                            SaveAll();
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
                string vehicleTypeFilter = "";
                while (looping)
                {
                    List<Vehicle> filteredVehicles = vehicles;
                 

                    Console.Clear();
                    TitleCard();
                    Console.WriteLine("     ---  VEHICLE SELECTION SCREEN  ---     \n");
                    //apply filters 
                    if (membershipFilter != -1 || makeFilter != "" || priceFilter != -1|| vehicleTypeFilter!="")
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
                        if (vehicleTypeFilter != "")
                        {
                            if (vehicleTypeFilter == "Car")
                            {
                                filteredVehicles = filteredVehicles.Where(v => v is Car car).ToList();

                            }
                            else if (vehicleTypeFilter == "HGV")
                            {
                                filteredVehicles = filteredVehicles.Where(v => v is HGV hgv).ToList();

                            }
                            else if (vehicleTypeFilter == "Motorcycle")
                            {
                                filteredVehicles = filteredVehicles.Where(v => v is Motorcycle motorcycle).ToList();

                            }
                            Console.WriteLine($" - VEHICLE TYPE: {vehicleTypeFilter.ToUpper()}");

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
                            Console.WriteLine("Please select a filter:\n1) Remove filters \n2) Membership\n3) Search Make\n4) Maximum Price\n5) Vehicle Type");
                            string filterInput = Console.ReadLine();
                            int casefilterInput = TryConvertToInt(filterInput, -1);
                            switch (casefilterInput)
                            {
                                case 1:
                                    membershipFilter = -1;
                                    priceFilter = -1;
                                    makeFilter = "";
                                    vehicleTypeFilter = "";
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
                                    if (membershipManager.memberships.ContainsKey(casefilterInputMembership))
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
                                case 5:
                                    Console.WriteLine("Select a vehicle type\n1) Car\n2) HGV\n3) Motorcycle");
                                    int VehicleTypeInput = TryConvertToInt(Console.ReadLine(), -1);
                                    switch (VehicleTypeInput)
                                    {
                                        case 1:
                                            vehicleTypeFilter = "Car";
                                            Console.WriteLine("Filter applied!");
                                            break;
                                        case 2:
                                            vehicleTypeFilter = "HGV";
                                            Console.WriteLine("Filter applied!");
                                            break;
                                        case 3:
                                            vehicleTypeFilter = "Motorcycle";
                                            Console.WriteLine("Filter applied!");
                                            break;
                                        default:
                                            Console.WriteLine("Invalid selection.");
                                            break;
                                    }
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
                if (args[0]=="-help")
                {
                    Console.WriteLine("Here is a this following lines of arguments you can use:\n1) Activate dummy data \"-dummydata\"\n2) Create admin account on dummy data \"-dummydata admin\"");
                }
                //dummydata
                if (args[0]== "-dummydata")
                {
                    usedummydata = true;
                    Console.WriteLine("Dummy data activated!");
                    if (args.Length > 1)
                    { 
                        if (args[1]=="admin")
                        {
                            createadmin = true;
                            Console.WriteLine("Admin account created!\nUsername = admin\nPassword = password123!");
                            User admin = new User(userService.GenerateID(users), "admin", passwordService.HashPassword("password123!"),2,1);
                            users.Add(admin);
                            loggedinUser = admin;
                        }
                    }
                }
                Console.WriteLine("PRESS ENTER TO CONTINUE");
                Console.ReadLine();

            }
            else
            {
                Console.WriteLine("No arguments provided, press any key to start");
                Console.ReadLine();
            }
            LoadAllData();
            
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
                    Console.WriteLine($"Welcome, {loggedinUser.ReturnUsername()}! You are using Autorenter as a {membershipManager.memberships[loggedinUser.ReturnMembershipLevel()].ReturnName()} member.\n1) Select a vehicle\n2) Log out\n3) Manage active booking\n4) Return previous bookings\n5) Manage membership {(loggedinUser.IsAdmin()?"\n6) Admin Menu":"")}");
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
                                    Console.WriteLine($"{v.ReturnBasicVehicleInfoInList()}\n{activeBooking.ReturnBookingInfo()}\n");

                                    if (!activeBooking.IsAwaitingReturnVerification())
                                    {
                                        Console.WriteLine("Would you like to log your return of this vehicle? This will have to be checked by a member of staff.\nType \"Yes\" to return it, enter anything else to continue");
                                        input = Console.ReadLine();
                                        if (!string.IsNullOrWhiteSpace(input))
                                        {
                                            if (input.ToLower() == "yes")
                                            {
                                                activeBooking.SetReturnTime(DateTime.Now);
                                                SaveAll();
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Console.WriteLine("Your return is waiting to be verified by an admin");
                                    }
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
                                        Console.WriteLine($"{i + 1}) {v.ReturnBasicVehicleInfo()} booked at {booking.ReturnBookingStart()} and returned at {booking.ReturnTimeReturned()}");
                                        //TODO MORE INFO AND CHECK ACTIVE
                                    }
                                }
                            }
                            else { Console.WriteLine("No previous bookings found"); }
                          
                            break;
                        case 5:
                            ManageMembership();
                            break;
                        case 6:
                            if (loggedinUser.IsAdmin())
                            {
                                AdminMenu();
                            }
                            else
                            {
                                Console.WriteLine("Invalid selection. Please try again.");
                            }
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
