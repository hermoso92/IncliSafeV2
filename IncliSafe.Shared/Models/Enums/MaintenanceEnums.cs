namespace IncliSafe.Shared.Models.Enums
{
    public enum MaintenanceType
    {
        Preventive,
        Corrective,
        Predictive,
        Emergency
    }

    public enum MaintenanceStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }

    public enum MaintenancePriority
    {
        Low,
        Medium,
        High,
        Critical
    }
} 