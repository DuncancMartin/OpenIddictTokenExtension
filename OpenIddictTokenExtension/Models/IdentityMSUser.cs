using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityApiOpenIddict_TokenExtension.Models
{
    public class IdentityMSUser : IdentityUser<int>
    {
        public int TenantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int LocalizationId { get; set; }
        public bool PasswordConfirmed { get; set; }
        public bool IsAdministrator { get; set; } //This is support admin.
        public bool? TenantAdministrator { get; set; }
        public bool? Active { get; set; }
        public string PasswordToken { get; set; }
        public DateTimeOffset? PasswordTokenExpiryTime { get; set; }
        public DateTimeOffset? LastAccess { get; set; }
    }
}
