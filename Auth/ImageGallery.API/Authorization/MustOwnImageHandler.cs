using ImageGallery.API.Services;
using Microsoft.AspNetCore.Authorization;

namespace ImageGallery.API.Authorization
{
    public class MustOwnImageHandler(IHttpContextAccessor httpContextAccessor,
        IGalleryRepository galleryRepository)
        : AuthorizationHandler<MustOwnImageRequirement>
    {
        // Inject IHttpContextAccessor to access HttpContext if needed

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            MustOwnImageRequirement requirement)
        {
            var imageId = httpContextAccessor.HttpContext?
                .GetRouteValue("id")?.ToString();

            if (!Guid.TryParse(imageId, out var imageGuid))
            {
                context.Fail();
                return;
            }

            // get the sub claim
            var ownerId = context.User.FindFirst(c => c.Type == "sub")?.Value;

            if (ownerId == null)
            {
                context.Fail();
                return;
            }

            if (await galleryRepository.IsImageOwnerAsync(imageGuid, ownerId))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
