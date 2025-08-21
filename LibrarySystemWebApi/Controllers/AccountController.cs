using LibrarySystemClassLibraryForApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Bcry = BCrypt.Net.BCrypt;
using System.Web.Http;

namespace LibrarySystemWebApi.Controllers
{
    public class AccountController : ApiController
    {
        //API for Login verifications 
        [HttpGet]
        public HttpResponseMessage Login(string Email,string Password)
        {
            HttpResponseMessage response = null;
            try
            {

                UsersOps objUsersOps = new UsersOps();
                objUsersOps.Email = Email;
                
                if(!objUsersOps.Load())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                if(!Bcry.Verify(Password, objUsersOps.Password))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                JWTTokenServices objJWTTokenServices = new JWTTokenServices();
                string jwtToken = objJWTTokenServices.TokenGenertor(Email);
                
                if(String.IsNullOrEmpty(jwtToken))
                {
                    return Request.CreateResponse(HttpStatusCode.Unauthorized);
                }

                response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, jwtToken, objUsersOps.UserId, objUsersOps.Name });

            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
            return response;
        }
    }
}
