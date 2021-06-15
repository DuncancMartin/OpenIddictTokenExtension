using OpenIddict.Abstractions;
using System.Collections.Generic;
using System.Threading;

namespace IdentityApiOpenIddict_TokenExtension.Managers
{
    public interface IExtendedOpenIddictTokenManager : IOpenIddictTokenManager
    {
        IAsyncEnumerable<object> FindPATAsync(string userId, string tenantId, CancellationToken cancellationToken = default);
    }
}
