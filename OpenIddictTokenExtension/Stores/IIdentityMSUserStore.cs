using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using IdentityApiOpenIddict_TokenExtension.Models;

namespace IdentityApiOpenIddict_TokenExtension.Stores
{
    public interface IIdentityMSUserStore : IUserStore<IdentityMSUser>
    {
        Task<IdentityMSUser> FindByIdAsync(int userId,
            CancellationToken cancellationToken = new CancellationToken());
        Task<IList<IdentityMSUser>> FindByTenantIdAsync(int tenantId,
            CancellationToken cancellationToken = new CancellationToken());
        //Task<IdentityMSUser> FindByIdWithModuleAccessAsync(int userId,
        //    CancellationToken cancellationToken = new CancellationToken());

    }
}

