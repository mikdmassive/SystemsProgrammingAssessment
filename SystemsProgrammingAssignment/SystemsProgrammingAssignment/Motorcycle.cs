public class Motorcycle : Vehicle
{
    private int EngineCapacity;
    public Motorcycle(string VehicleID, string RegPlate, string Make, string Model, int MembershipLevel, float CostPerHour, int EngineCapacity) : base(VehicleID, RegPlate, Make, Model, MembershipLevel, CostPerHour)
    {
        this.EngineCapacity = EngineCapacity;
    }
    public int ReturnMaxLoadWeight()
    {
        return EngineCapacity;
    }

}