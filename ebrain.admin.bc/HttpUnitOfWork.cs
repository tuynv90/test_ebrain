// ====================================================
// Caicho development team
// Email: johnpham@ymail.com
// ====================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AspNet.Security.OpenIdConnect.Primitives;
using ebrain.admin.bc.Utilities;

namespace ebrain.admin.bc
{
    public class HttpUnitOfWork : UnitOfWork
    {
        public HttpUnitOfWork(ApplicationDbContext context, IHttpContextAccessor httpAccessor) : base(context)
        {
            var userId = httpAccessor.HttpContext.User.FindFirst(OpenIdConnectConstants.Claims.Subject)?.Value?.Trim();
            context.CurrentUserId = userId.ConvertStringToGuid();
        }
    }
}
