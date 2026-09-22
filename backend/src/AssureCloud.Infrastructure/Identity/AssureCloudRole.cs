using System;

namespace AssureCloud.Infrastructure.Identity;

public class AssureCloudRole : Microsoft.AspNetCore.Identity.IdentityRole<Guid>
{
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
