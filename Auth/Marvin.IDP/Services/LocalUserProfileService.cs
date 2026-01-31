using System.Security.Claims;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

namespace Marvin.IDP.Services
{
    public class LocalUserProfileService(ILocalUserService localUserService) : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject?.GetSubjectId();
            var claimsUser = (await localUserService.GetUserClaimsBySubjectAsync(subjectId)).ToList();

            context.AddRequestedClaims(claimsUser.Select(c => new Claim(c.Type, c.Value)).ToList());
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            //var subjectId = context.Subject?.GetSubjectId();
            //var isActive = await localUserService.IsUserActive(subjectId);

            context.IsActive = true;
        }
    }
}
