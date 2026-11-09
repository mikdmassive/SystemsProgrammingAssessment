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
    public string ReturnVehicleID(){return VehicleID; }
    public DateTime ReturnBookingStart(){return BookingStart; }
    public DateTime ReturnBookingEnd(){return BookingEnd; }
    public DateTime? ReturnTimeReturned() { return TimeReturned;}

}
