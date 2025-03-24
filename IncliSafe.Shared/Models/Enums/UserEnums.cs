namespace IncliSafe.Shared.Models.Enums
{
    public enum UserRole
    {
        Admin,
        Manager,
        Operator,
        Technician
    }

    public enum UserStatus
    {
        Active,
        Inactive,
        Suspended,
        Deleted
    }

    public enum LicenseStatus
    {
        Active,
        Expired,
        Suspended,
        Revoked,
        Pending
    }

    public enum LicenseType
    {
        Trial,
        Basic,
        Professional,
        Enterprise,
        Custom
    }
} 