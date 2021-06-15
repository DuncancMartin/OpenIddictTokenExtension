using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using IdentityApiOpenIddict_TokenExtension.Models;
using IdentityApiOpenIddict_TokenExtension.Stores;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IdentityApiOpenIddict_TokenExtension.Managers
{
    /// <summary>
    /// Extended user manager that does not save changes automatically.
    /// </summary>
    public class IdentityMSUserManager : UserManager<IdentityMSUser>
    {
        private readonly IIdentityMSUserStore _store;

        public IdentityMSUserManager(IUserStore<IdentityMSUser> store,
                                     IOptions<IdentityOptions> optionsAccessor,
                                     IPasswordHasher<IdentityMSUser> passwordHasher,
                                     IEnumerable<IUserValidator<IdentityMSUser>> userValidators,
                                     IEnumerable<IPasswordValidator<IdentityMSUser>> passwordValidators,
                                     ILookupNormalizer keyNormalizer,
                                     IdentityErrorDescriber errors,
                                     IServiceProvider services,
                                     ILogger<UserManager<IdentityMSUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators,
                   keyNormalizer, errors, services, logger)
        {
            _store = store as IIdentityMSUserStore;
        }

        /// <summary>
        /// Creates the specified <paramref name="user"/> in the backing store with no password,
        /// as an asynchronous operation.
        /// </summary>
        public override Task<IdentityResult> CreateAsync(IdentityMSUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return base.CreateAsync(user);
        }

        /// <summary>
        /// Finds and returns a user, if any, who has the specified <paramref name="userId"/>.
        /// Optionally also finds the users module and entity access.
        /// </summary>
        public Task<IdentityMSUser> FindByIdAsync(int userId, bool includeModuleAccess = false)
        {
            ThrowIfDisposed();
            //if (includeModuleAccess)
            //    return _store.FindByIdWithModuleAccessAsync(userId, CancellationToken);

            return _store.FindByIdAsync(userId, CancellationToken);
        }

        public Task<IList<IdentityMSUser>> FindByTenantIdAsync(int tenantId)
        {
            ThrowIfDisposed();
            return _store.FindByTenantIdAsync(tenantId, CancellationToken);
        }

        /// <summary>
        /// Changes a user's password after confirming the specified <paramref name="currentPassword"/> is correct,
        /// as an asynchronous operation.
        /// </summary>
        public override Task<IdentityResult> ChangePasswordAsync(IdentityMSUser user, string currentPassword,
            string newPassword)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            return base.ChangePasswordAsync(user, currentPassword, newPassword);
        }

        /// <summary>
        /// Gets a flag indicating whether the password for the specified <paramref name="user"/> has been verified,
        /// true if the password is verified otherwise false.
        /// </summary>
        public Task<bool> GetPasswordConfirmedAsync(IdentityMSUser user)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            return Task.FromResult(user.PasswordConfirmed);
        }

        /// <summary>
        /// Updates a user's password hash while checking it against password constraints.
        /// </summary>
        public async Task<IdentityResult> UpdatePasswordAsync(IdentityMSUser user, string password)
        {
            ThrowIfDisposed();
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            if (password == null)
                throw new ArgumentNullException(nameof(password));

            var result = await UpdatePasswordHash(user, password, true);

            if (!result.Succeeded)
                return result;

            // The user has to reset his password on the next login
            // TODO: Set this to false again once password changes are implemented in the UI
            user.PasswordConfirmed = true;

            return IdentityResult.Success;
        }

    }
}
