public class Car : Vehicle
{
    private int NumberOfSeats;
    public Car(string VehicleID, string RegPlate, string Make, string Model, int MembershipLevel, float CostPerHour, int NumberOfSeats) : base(VehicleID, RegPlate, Make, Model, MembershipLevel, CostPerHour)
    {
        this.NumberOfSeats = NumberOfSeats;
    }
    public int ReturnNumberOfSeats()
    {
        return NumberOfSeats;
    }

}