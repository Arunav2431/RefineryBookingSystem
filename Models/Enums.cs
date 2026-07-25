// File: Models/Enums.cs
namespace RefineryBooking.Models
{
    public enum BookingStatus
    {
        Pending = 0,
        Approved = 1,
        Rejected = 2,
        Cancelled = 3,
        PendingAllocatorReview = 4
    }

    public enum TechSetupStatus
    {
        Pending = 0,
        Ready = 1,
        IssueReported = 2
    }
}