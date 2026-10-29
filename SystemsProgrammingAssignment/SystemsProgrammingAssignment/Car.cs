public class Car : Vehicle
{
    private int NumberOfSeats;
    public Car(string VehicleID, string RegPlate, string Make, string Model, int MembershipLevel, float CostPerHour, int MaxLoadWeight) : base(VehicleID, RegPlate, Make, Model, MembershipLevel, CostPerHour)
    {
        this.NumberOfSeats = MaxLoadWeight;
    }
    public int ReturnMaxLoadWeight()
    {
        return NumberOfSeats;
    }

}