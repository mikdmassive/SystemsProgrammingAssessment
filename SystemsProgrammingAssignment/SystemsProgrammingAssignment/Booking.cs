public class Booking
{
    private string BookingID;
    private string VehicleID;
    private string UserID;
    private DateTime BookingStart;
    private DateTime BookingEnd;
    private DateTime? TimeReturned;
    public Booking(string BookingID,string VehicleID,string UserID, DateTime BookingStart,DateTime BookingEnd)
    {
        this.BookingID = BookingID; 
        this.VehicleID = VehicleID;
        this.UserID = UserID;
        this.BookingStart = BookingStart;
        this.BookingEnd = BookingEnd;
        TimeReturned = null;
    }
    public string ReturnBookingID(){return BookingID;}
    public string ReturnUserID(){return UserID; }
    public string ReturnVehicleID(){return VehicleID; }
    public DateTime ReturnBookingStart(){return BookingStart; }
    public DateTime ReturnBookingEnd(){return BookingEnd; }
    public DateTime? ReturnTimeReturned() { return TimeReturned;}
    public bool IsBookingActive()
    {
        bool active = false;
        DateTime currentTime = DateTime.Now;
        if (currentTime < BookingEnd)
        {
            active = true;
        }
        else if (TimeReturned == null)
        {
            active = true;
        }
        return active;
    }
    public bool IsOverdue()
    {
        DateTime currentTime = DateTime.Now;

        return currentTime > BookingEnd && TimeReturned == null;
    }
    public string ReturnBookingInfo()
    {
        TimeSpan remaining = BookingEnd - DateTime.Now;
        return $"{BookingStart}-{BookingEnd} \n{(IsOverdue()? "OVERDUE" :$"Return in {(remaining.Days>0 ? $"{remaining.Days} days, {remaining.Hours} hours and {remaining.Minutes}" : $"{remaining.Hours} hours and {remaining.Minutes}")} minutes.")}";
    }
}
