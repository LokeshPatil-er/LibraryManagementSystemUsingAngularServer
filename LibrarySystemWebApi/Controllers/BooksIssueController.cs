using LibrarySystemClassLibraryForApis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace LibrarySystemWebApi.Controllers
{
    public class BooksIssueController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage MemberListGet()
        {
            HttpResponseMessage response = null;
            try
            {
                MembersOps objMembersOps = new MembersOps();

                List<Members> members = objMembersOps.GetMembersList();
                if (members != null)
                    response = Request.CreateResponse(HttpStatusCode.OK, members);
                else
                    response = Request.CreateResponse(HttpStatusCode.OK, new List<string>());
            }
            catch(Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage MemberDetailsById(int MemberId)
        {
            HttpResponseMessage response = null;
            try
            {
                MembersOps objMembersOps = new MembersOps();
                objMembersOps.MemberId = MemberId;
               bool isMemberDetailsGet = objMembersOps.Load();
                if (isMemberDetailsGet)
                    response = Request.CreateResponse(HttpStatusCode.OK, objMembersOps);
                else
                    response = Request.CreateResponse(HttpStatusCode.OK, new { });
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return response;
        }
    }
}
