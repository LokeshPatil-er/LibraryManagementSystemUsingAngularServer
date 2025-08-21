using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;

namespace LibrarySystemClassLibraryForApis
{
    public class JwtAuthorizationAttribute: AuthorizationFilterAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //base.OnAuthorization(actionContext);
            try
            {
                var authHeader = actionContext.Request.Headers.Authorization;
                if ( authHeader.Parameter=="null" || authHeader.Scheme != "Bearer")
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Missing token");
                    return;
                }

                var token = authHeader.Parameter;
                var claimPrincipal = JWTTokenServices.TokenValidation(token);
                if (claimPrincipal == null)
                {
                    actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid token");
                    return;
                }

               
                actionContext.RequestContext.Principal = claimPrincipal;

            }
            catch(Exception ex)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "Invalid or expired token: " + ex.Message);
            }
        }
    }
}
