public class BookingManager
{
    private int MinBookingTimeInHours = 1;
    private int MaxBookingTimeInHours = 48;
    public int ReturnMinBookingTimeInHours(){ return MinBookingTimeInHours; }
    public int ReturnMaxBookingTimeInHours(){ return MaxBookingTimeInHours; }
    public Booking CreateBooking(User user,Vehicle vehicle,int hours,List<Booking> bookings)
    {
        DateTime bookingtime = DateTime.Now;
        Booking booking = new Booking(GenerateID(bookings),vehicle.ReturnVehicleID(), user.ReturnUserID(), bookingtime,bookingtime.AddHours(hours));

        return booking;
    }
    public string GenerateID(List<Booking> bookings)
    {
        Random random = new Random();
        string id;
        do
        {
            id = random.Next(0, 1000000000).ToString();
        }
        while (bookings.Any(booking => booking.ReturnBookingID() == id));
        return id;
    }
    
    public bool IsVehicleAlreadyBooked(Vehicle vehicle,List<Booking> bookings)
    {
        bool booked = false;
        foreach (Booking booking in bookings) 
        {
            if (vehicle.ReturnVehicleID() == booking.ReturnVehicleID())
            {
                booked = booking.IsBookingActive();
            }
        }

        return booked;
    }
    public bool IsUserAlreadyGotABooking(User user, List<Booking> bookings)
    {
        bool booked = false;
        foreach (Booking booking in bookings)
        {
            if (user.ReturnUserID() == booking.ReturnUserID())
            {
                booked = booking.IsBookingActive();
            }
        }

        return booked;
    }
    public List<Booking> returnUsersBookings(User user, List<Booking> bookings)
    {
        List<Booking> usersBookings = new List<Booking>() { };
        foreach (Booking booking in bookings)
        {
            if (user.ReturnUserID() == booking.ReturnUserID())
            {
                usersBookings.Add(booking);
            }
        }

        return usersBookings;
    }
    public Booking returnUserActiveBooking(User user, List<Booking> bookings)
    {
        Booking activeBooking = null;
        List<Booking> usersBookings = returnUsersBookings(user,bookings);
        foreach (Booking booking in usersBookings)
        {
           if (booking.IsBookingActive())
            {
                activeBooking = booking;
            }
        }
        return activeBooking;
    }
}