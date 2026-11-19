using System.Diagnostics;
using SystemsProgrammingAssignment;

public class VehicleManager
{
    int MinLength = 1;
    MembershipManager membershipManager = new MembershipManager();
    public Vehicle CreateVehicle(List<Vehicle> vehicles)
    {
        bool typeValid = false;
        string vehicleType = "";
        while (!typeValid)
        {
            Console.WriteLine("Select a vehicle type:\n1) Car \n2) HGV\n3) Motorcycle");
            string InputType = Console.ReadLine();
            int caseInputType = Program.TryConvertToInt(InputType, -1);
            switch (caseInputType){
                case 1:
                    vehicleType = "Car";
                    typeValid = true;
                    break;
                case 2:
                    vehicleType = "HGV";
                    typeValid = true;
                    break;
                case 3:
                    vehicleType = "Motorcycle";
                    typeValid = true;
                    break;
                default:
                    Console.WriteLine("Invalid choice, try again.");
                    break;
            }
        }
       
        string regplate = ReturnStringInput("registration plate",7);
        string make = ReturnStringInput("make",20);
        string model = ReturnStringInput("model",20);
        float cph = ReturnFloatInput("cost per hour", 0.01f, 1000f);

        bool membershipValid = false;
        int membershipLevel = -1;
        while (!membershipValid)
        {
            Console.WriteLine("Select a required minimum membership for the vehicle:");
            foreach (Membership membership in membershipManager.ReturnMemberships())
            {
                Console.WriteLine($"{membership.ReturnMembershipLevel()}) {membership.ReturnName()}");
            }
            string InputMembership = Console.ReadLine();
            int caseInputMembership = Program.TryConvertToInt(InputMembership, -1);
            if (membershipManager.memberships.ContainsKey(caseInputMembership))
            {
                membershipLevel = caseInputMembership;
                membershipValid = true;
            }
            else
            {
                Console.WriteLine("Invalid, try again.");
            }
        }
     
        if (vehicleType == "HGV")
        {

            return new HGV(GenerateID(vehicles), regplate, make, model, membershipLevel, cph,ReturnFloatInput("max load weight",1,100000));
        }
        else if (vehicleType == "Motorcycle")
       
        {
            return new Motorcycle(GenerateID(vehicles), regplate, make, model, membershipLevel, cph,ReturnIntInput("engine size",1,2000));
        }
        else
        {

            return new Car(GenerateID(vehicles), regplate, make, model, membershipLevel, cph,ReturnIntInput("number of seats",1,20));
        }



    }

    
    public string GenerateID(List<Vehicle> vehicles)
    {
        Random random = new Random();
        string id;
        do
        {
            id = random.Next(0, 1000000000).ToString();
        }
        while (vehicles.Any(vehicle => vehicle.ReturnVehicleID() == id));
        return id;
    }
    public bool VerifyVehicleStringInput(string input, int MaxLength)
    {
        bool valid = false;

        if (!string.IsNullOrEmpty(input))
        {
            int len = input.Length;
            if (len <= MaxLength && len >= MinLength)
            {
                if (input.All(char.IsLetterOrDigit))
                {
                    valid = true;
                }
            }
        }
        return valid;
    }
    string ReturnStringInput(string typee,int Max)
    {
        bool valid = false;
        string input = "";
        while (!valid)
        {
            Console.WriteLine($"Please enter the {typee} of the vehicle:");
            input = Console.ReadLine();
            if (VerifyVehicleStringInput(input,Max))
            {
                valid = true;
            }
            if (!valid)
            {
                Console.WriteLine($"Invalid {typee}. Please enter a {typee} with {MinLength}-{Max} characters, with no spaces and only letters and numbers");
            }
        }
        return input;

    }
    float ReturnFloatInput(string typee, float min, float max)
    {
        bool looping = true;
        float convertInput = -1;
        while (looping)
        {
            Console.WriteLine($"Enter the vehicles {typee}:");
            string input = Console.ReadLine();
            convertInput = Program.TryConvertToFloat(input, -1);
            if (convertInput >= min && convertInput <= max)
            {
                looping = false;
            }
            else
            {
                Console.WriteLine($"Invalid {typee}. Please enter a number inbetween {min}-{max}.");
            }
        }
        return convertInput;
    }
    int ReturnIntInput(string typee, int min, int max)
    {
        bool looping = true;
        int convertInput = -1;
        while (looping)
        {
            Console.WriteLine($"Enter the {typee}:");
            string input = Console.ReadLine();
            convertInput = Program.TryConvertToInt(input, -1);
            if (convertInput >= min && convertInput <= max)
            {
                looping = false;
            }
            else
            {
                Console.WriteLine($"Invalid {typee}. Please enter a whole number inbetween {min}-{max}.");
            }
        }
        return convertInput;
    }
}