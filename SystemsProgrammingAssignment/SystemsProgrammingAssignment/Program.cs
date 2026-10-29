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
            //INTERNAL FUNCTIONS 

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
                                SelectVehicleFromGivenList(vehicles);
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
                            Console.WriteLine("Invalid selection. Please try again.");
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
                Console.WriteLine("1) Select a vehicle");
                string input = Console.ReadLine();
                int caseInput = TryConvertToInt(input, -1);
                switch (caseInput){
                    case 1:
                        VehicleSelection();
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please try again."); 
                        break;
               
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
