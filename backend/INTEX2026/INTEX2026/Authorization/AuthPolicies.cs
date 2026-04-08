namespace INTEX2026.Authorization;

/// <summary>ASP.NET Identity role name constants (must match seeded roles).</summary>
public static class AuthRoles
{
    public const string ExecutiveAdmin = "ExecutiveAdmin";
    public const string RegionalManager = "RegionalManager";
    public const string SocialWorker = "SocialWorker";
    public const string Donor = "Donor";
}

/// <summary>Authorization policy names for <see cref="Microsoft.AspNetCore.Authorization.AuthorizeAttribute"/>.</summary>
public static class AuthPolicies
{
    /// <summary>Executive admin, regional manager, or social worker.</summary>
    public const string RequireStaff = "RequireStaff";

    /// <summary>Executive admin or regional manager only.</summary>
    public const string ExecutiveOrRegional = "ExecutiveOrRegional";

    /// <summary>Executive admin only.</summary>
    public const string ExecutiveAdminOnly = "ExecutiveAdminOnly";

    /// <summary>Regional manager only.</summary>
    public const string RegionalManagerOnly = "RegionalManagerOnly";

    /// <summary>Social worker only.</summary>
    public const string SocialWorkerOnly = "SocialWorkerOnly";

    /// <summary>Donor only.</summary>
    public const string DonorOnly = "DonorOnly";

    /// <summary>Executive admin, regional manager, social worker, or donor (e.g. donor self-service alongside staff).</summary>
    public const string StaffOrDonor = "StaffOrDonor";
}
