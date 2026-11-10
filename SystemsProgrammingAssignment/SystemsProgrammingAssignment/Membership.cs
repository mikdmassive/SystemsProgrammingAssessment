public class Membership
{
    private int Level;
    private float Price;
    private string Name;
    private string Description;
    public Membership(int Level, float Price, string Name, string Description)
    {
        this.Level = Level;
        this.Price = Price;
        this.Name = Name;
        this.Description = Description;
    }
    public string ReturnName()
    {
        return Name;
    }
    public string ReturnInfo()
    {
        return $"{Name} Membership (£{Price.ToString("0.00")} per month - {Description})";
    }
    public int ReturnMembershipLevel() { return Level;}
}