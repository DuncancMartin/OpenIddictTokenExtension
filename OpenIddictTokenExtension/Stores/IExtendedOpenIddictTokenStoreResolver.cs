using OpenIddict.Abstractions;

namespace IdentityApiOpenIddict_TokenExtension.Stores
{
    public interface IExtendedOpenIddictTokenStoreResolver : IOpenIddictTokenStoreResolver
    {
        IExtendedOpenIddictTokenStore<TToken> GetExtended<TToken>() where TToken : class;
    }
}
