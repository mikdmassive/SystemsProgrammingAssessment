using SystemsProgrammingAssignment;

public class Vehicle
{
    private string VehicleID;
    private string RegPlate;
    private string Make;
    private string Model;
    private int MembershipLevel;
    private float CostPerHour;
    public Vehicle(string VehicleID, string RegPlate, string Make, string Model, int MembershipLevel, float CostPerHour)
    {
        this.VehicleID = VehicleID;
        this.RegPlate = RegPlate;
        this.Make = Make;
        this.Model = Model;
        this.MembershipLevel = MembershipLevel;
        this.CostPerHour = CostPerHour;
        
    }


    public string ReturnVehicleID(){ return VehicleID;}
    public string ReturnRegPlate(){return RegPlate;}
    public string ReturnMake() { return Make; }
    public string ReturnModel() { return Model; }
    public int ReturnMembershipLevel() { return MembershipLevel; }
    public float ReturnCostPerHour() { return CostPerHour; }

}