using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Security.Claims;
using DAL.Interfaces;

namespace Backend
{
    public class UserContext : IUserContext
    {
        private readonly IActionContextAccessor _contextAccessor;

        public UserContext(IActionContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
        }

        public Guid UserId
        {
            get
            {
                if (_contextAccessor.ActionContext != null)
                {
                    Claim? userClaim =
                        _contextAccessor.ActionContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
                    if (userClaim != null)
                    {
                        return Guid.Parse(userClaim.Value);
                    }
                }

                return Guid.Empty;

                // Todo; for now return the same user always. Remove this once authentication has been implemented
            }

        }
    }
}
