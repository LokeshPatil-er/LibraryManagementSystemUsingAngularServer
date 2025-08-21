using LibrarySystemClassLibraryForApis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Transactions;


namespace LibrarySystemWebApi.Controllers
{
    [JwtAuthorization]
    public class BooksIssueController : ApiController
    {
        //Api for get all Active member list
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

        //Api for get member complete info using Id
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

        //Api for get Books list
        [HttpGet]
        public HttpResponseMessage BooksList()
        {
            HttpResponseMessage response = null;
            try
            {
                Books books = new Books();
                BooksOps objBooksOps = new BooksOps();
                List<Books> booksList = objBooksOps.GetBooksList(books);
                if (booksList != null)
                    response = Request.CreateResponse(HttpStatusCode.OK, booksList);
                else
                    response = Request.CreateResponse(HttpStatusCode.OK, new List<Books>());
            }
            catch(Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return response;
        }

        //Api For Books Issue Data store
        [HttpPost]
        public HttpResponseMessage BooksIssueDetailsStore()
        {
            HttpResponseMessage response = null;
            using (var scope = new TransactionScope())
            {
                string issueFileSavePath = "";
                try
                {
                    var request = HttpContext.Current.Request;
                    var model = JsonConvert.DeserializeObject<BooksIssueDetails>(request["bookIssueDetail"]);

                    //stored Books issue 
                    BooksIssueOps objBooksIssueOps = new BooksIssueOps()
                    {
                        BookIssueId = model.BookIssueId,
                        MemberId = model.MemberId,
                        IssueDate = model.IssueDate,
                        DueDate = model.DueDate,
                        IsActive = true
                    };
                    if (model.BookIssueId > 0)
                    {
                        objBooksIssueOps.ModifiedBy = 1;
                        objBooksIssueOps.ModifiedOn = DateTime.Now;
                    }
                    else if (model.BookIssueId == 0)
                    {
                        objBooksIssueOps.CreatedBy = 1;
                        objBooksIssueOps.CreatedOn = DateTime.Now;
                    }

                    if (!objBooksIssueOps.Save())
                    {
                      return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Failed to save book issue record." });

                    }


                    //save uploaded file to file system and add it into list
                
                    if (request.Files.Count > 0)
                    {
                        List<IssueSupportFilesOps> supportFilesList = new List<IssueSupportFilesOps>();

                        for (int i = 0; i < request.Files.Count; i++)
                        {
                            HttpPostedFile postedFile = request.Files[i];
                            if (postedFile != null && postedFile.ContentLength > 0)
                            {
                                string uploadPath = ConfigurationManager.AppSettings["IssueFilesPath"];
                                int issueId = objBooksIssueOps.BookIssueId;
                                issueFileSavePath = Path.Combine(uploadPath, issueId.ToString());

                                if (!Directory.Exists(issueFileSavePath))
                                    Directory.CreateDirectory(issueFileSavePath);

                                string extension = Path.GetExtension(postedFile.FileName);
                                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmssfff");
                                string fileName = $"{timestamp}{extension}";
                                string fileFullPath = Path.Combine(issueFileSavePath, fileName);

                                postedFile.SaveAs(fileFullPath);

                                supportFilesList.Add(new IssueSupportFilesOps
                                {
                                    IssueSupportFileId = 0,
                                    BookIssueId = objBooksIssueOps.BookIssueId,
                                    FileName = postedFile.FileName,
                                    FilePath = fileName,
                                    CreatedBy = 1,
                                    CreatedOn = DateTime.Now
                                });
                            }
                        }

                        //stored issue support files
                        if (!(supportFilesList.Count > 0 && supportFilesList.Count == request.Files.Count))
                        {
                            Directory.Delete(issueFileSavePath, true);
                           return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = $"File upload failed ..only { supportFilesList.Count } uploaded" });
                        }
                        IssueSupportFilesOps objIssueSupportFilesOps = new IssueSupportFilesOps();
                        objIssueSupportFilesOps.IssueSuppportFilesList = supportFilesList;

                        if (!objIssueSupportFilesOps.InsertAndUpdate())
                        {
                           return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Failed to save support files." });

                        }
                    }

                    //stored Books Issued details
                    BooksIssueDetailsOps objBooksIssueDetailOps = new BooksIssueDetailsOps();
                    objBooksIssueDetailOps.BookDetails = model.BookList.Select(book => new BooksIssueDetailsOps
                    {

                        BookIssueDetailId = book.BookIssueDetailId,
                        BookIssueId = objBooksIssueOps.BookIssueId,
                        BookId = book.BookId,
                        IssueQuantity = book.IssueQuantity,
                        ReturnQuantity = 0,
                        ReturnStatus = false,
                        DueDate = model.DueDate,
                        IsPenalty = false,
                        IsActive = true,
                        CreatedBy = 1,
                        CreatedOn = DateTime.Now
                    }).ToList();


                    if (!objBooksIssueDetailOps.InsertANDUpdate())
                    {
                       return response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Failed to save book issue details." });
                    }

                    scope.Complete();
                   return response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "Book issue details saved successfully." });


                }
                catch (Exception ex)
                {
                    if(Directory.Exists(issueFileSavePath))
                    {
                        Directory.Delete(issueFileSavePath);
                    }
                   return response = Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "Unexpected error occurred: " + ex.Message });
                }
            }
                
            
        }

        //Api for get All issued list
        [HttpGet]
        public HttpResponseMessage BooksIssuedList()
        {
            HttpResponseMessage response = null;
            try
            {
                var model = new BookIssuesDetailList();
                BooksIssueDetailsOps objBooksIssueDetailsOps = new BooksIssueDetailsOps();

                model.BooksIssuedList = objBooksIssueDetailsOps.GetBookIssuesDetailList();

                if(model!=null)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, model.BooksIssuedList);
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, new List<BookIssuesDetailList>());
                }
            }
            catch(Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage loadBookIssueDetails(int bookIssueId)
        {
            HttpResponseMessage response = null;
            try
            {
                BooksIssueDetailsOps objBooksIssueDetailsOps = new BooksIssueDetailsOps();

                if (bookIssueId > 0)
                {
                    objBooksIssueDetailsOps.BookIssueId = bookIssueId;
                }

                // fetch list
                List<BooksIssueDetails> bookIssueDetails = objBooksIssueDetailsOps.GetBookIssuesDetailList();

                // get single object
                var singleDetail = bookIssueDetails.FirstOrDefault();

                if (singleDetail != null)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, singleDetail);
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.NotFound, "No record found");
                }
            }
            catch (Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage DownloadFile(string FilePath,string BookIssueId)
        {
            HttpResponseMessage response = null;
            try
            {
                if (string.IsNullOrEmpty(FilePath) || string.IsNullOrEmpty(BookIssueId))
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                string fileIntialPath = ConfigurationManager.AppSettings["IssueFilesPath"];
                string fullPath = Path.Combine(fileIntialPath, BookIssueId, FilePath);
                if (!System.IO.File.Exists(fullPath))
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
                response = Request.CreateResponse(HttpStatusCode.OK, new { Content = new ByteArrayContent(fileBytes) });
            }
            catch(Exception ex)
            {
                response= Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

            return response;
        }

    }
}
