namespace IncliSafe.Shared.Models.Enums
{
    public enum InspectionType
    {
        Regular,
        Preventive,
        Emergency,
        Certification,
        Custom
    }

    public enum InspectionStatus
    {
        Pending,
        InProgress,
        Completed,
        Failed,
        Cancelled
    }
} 