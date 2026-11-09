public class BookingManager
{
    public Booking CreateBooking(User user,Vehicle vehicle,int hours,List<Booking> bookings)
    {
        DateTime bookingtime = DateTime.Now;
        Booking booking = new Booking(GenerateID(bookings),user.ReturnUserID(),vehicle.ReturnVehicleID(),bookingtime,bookingtime.AddHours(hours));
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
                booked = true;
                DateTime currentTime = DateTime.Now;
                if (currentTime< booking.ReturnBookingEnd())
                {
                    if(booking.ReturnTimeReturned()!=null)
                    {
                        booked = false;
                    }
                }
            }
        }

        return booked;
    }
}