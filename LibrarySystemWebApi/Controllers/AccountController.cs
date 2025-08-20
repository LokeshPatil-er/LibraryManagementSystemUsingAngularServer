using LibrarySystemClassLibraryForApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

                UsersOps objUsers = new UsersOps();
                objUsers.Email = users.Email;
                
                if(!objUsers.Load())
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

         

            }catch(Exception ex)
            {

            }
            return response;
        }
    }
}
