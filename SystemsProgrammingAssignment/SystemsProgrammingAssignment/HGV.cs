public class HGV:Vehicle
{
    private float MaxLoadWeight;
    public HGV(string VehicleID, string RegPlate, string Make, string Model, int MembershipLevel, float CostPerHour,float MaxLoadWeight):base(VehicleID, RegPlate, Make, Model, MembershipLevel, CostPerHour)
    {
        this.MaxLoadWeight = MaxLoadWeight;
    }
    public float ReturnMaxLoadWeight()
    {
        return MaxLoadWeight;
    }
}