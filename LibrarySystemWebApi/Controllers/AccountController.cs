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
        public HttpResponseMessage Login(Users users)
        {
            HttpResponseMessage response = null;
            try
            {

                UsersOps objUsersOps = new UsersOps();
                objUsersOps.Email = users.Email;
                
                if(!objUsersOps.Load())
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                if(!Bcry.Verify(users.Password,objUsersOps.Password))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                JWTTokenServices objJWTTokenServices = new JWTTokenServices();
                string jwtToken = objJWTTokenServices.TokenGenertor(users.Email);
                
                if(!String.IsNullOrEmpty(jwtToken))
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, jwtToken, objUsersOps.UserId });
                }


            }
            catch(Exception ex)
            {

            }
            return response;
        }
    }
}
