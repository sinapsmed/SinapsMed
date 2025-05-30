namespace Entities.Enums.Appointment
{
    public enum AppointmentStatus
    {
        PaymentStart, // Means Payment is Started
        Confirmed, // Means Paymet Verified 
        Completed, // Means Appointment is Done
        Pending, //Created But payment  Not Verified
    }
}