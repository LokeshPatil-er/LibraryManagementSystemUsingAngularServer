using LibrarySystemClassLibraryForApis;
using System;
using System.Net.Http;
using System.Web.Http;
using System.Net;
using System.Collections.Generic;

namespace LibrarySystemWebApi.Controllers
{
    
    public class BooksController : ApiController
    {
        //api for get booklist also apply filter on booklist data
        [HttpPost]
        public HttpResponseMessage BookList( Books model)
        {
            HttpResponseMessage response = null;
            try
            {
                BooksOps objBooksOps = new BooksOps();
                PublishersOps objPublishersOps = new PublishersOps();
                CoursesOps objCoursesOps = new CoursesOps();
                if (model == null)
                    model = new Books();

                model.bookList = objBooksOps.GetBooksList(model);

                if (model.bookList != null)
                    response = Request.CreateResponse(HttpStatusCode.OK, model);
                else
                    response = Request.CreateResponse(HttpStatusCode.OK, new List<string>());
            }
            catch(Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
           

            return response;
        }

        //api for get publishers and courses list 
        [HttpGet]
        public HttpResponseMessage GetPublisherListAndCourseList()
        {
            HttpResponseMessage response = null;
            try
            {
                Books bookmodel = new Books();
                PublishersOps objPublishersOps = new PublishersOps();
                CoursesOps objCoursesOps = new CoursesOps();

                bookmodel.PublishersList = objPublishersOps.GetPublishersList();
                bookmodel.CoursesList = objCoursesOps.GetCoursesList();

                if (bookmodel.PublishersList != null && bookmodel.CoursesList != null)
                    response = Request.CreateResponse(HttpStatusCode.OK, bookmodel);
                else
                    response = Request.CreateResponse(HttpStatusCode.OK, new List<string>());


            }
            catch(Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            return response;
            
        }

        //api for add book details opertion
        [HttpPost]
        public HttpResponseMessage BookDetailsSave( Books bookModel)
        {
            HttpResponseMessage response = null;
            try
            {
                BooksOps objBooksOps = new BooksOps()
                {
                    BookId=bookModel.BookId,
                    BookName = bookModel.BookName,
                    PublisherId = bookModel.PublisherId,
                    CourseId = bookModel.CourseId,
                    Edition = bookModel.Edition,
                    EditionYear = bookModel.EditionYear,
                    Pages = bookModel.Pages,
                    TotalCopies = bookModel.TotalCopies,
                    IsActive = bookModel.IsActive,
                   // CreatedBy = 1,
                   // CreatedOn = DateTime.Now
                };

                if (bookModel.BookId>0)
                {
                    objBooksOps.ModifiedBy = 1;
                    objBooksOps.ModifiedOn = DateTime.Now;
                }
                else
                {
                    objBooksOps.CreatedBy = 1;
                    objBooksOps.CreatedOn = DateTime.Now;
                }

               bool isSave = objBooksOps.Save();

                if (isSave)
                    response = Request.CreateResponse(HttpStatusCode.OK,new { success = isSave,message="Book details save successfully" });
                else
                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = isSave, message = "Unable to save Book details ..Try later !" });
            }
            catch (Exception ex)
            {
                // log exception if needed
                response = Request.CreateResponse(HttpStatusCode.BadRequest,new { success = false, message = "Due to server error can't add book ..Try later !" });
            }
            return response;
        }

        //api for get single book details based on perticular bookid 
        [HttpGet]
        public HttpResponseMessage loadBookDetails(int bookId)
        {
            HttpResponseMessage response = null;
            try
            {
                BooksOps objBooksOps = new BooksOps();
                objBooksOps.BookId = bookId;
                bool isBookExit = objBooksOps.Load();

                if(isBookExit)
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, objBooksOps);
                }
                else
                {
                    response = Request.CreateResponse(HttpStatusCode.OK, new { });
                }

            }
            catch(Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return response;
        }


        [HttpGet]
        public HttpResponseMessage deleteBook(int bookId)
        {
            HttpResponseMessage response = null;

            try
            {
                BooksOps objBooksOps = new BooksOps();
                objBooksOps.BookId = bookId;

                bool isBookDeleted = objBooksOps.Delete();
                if (isBookDeleted)
                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, message="Book Record Deleted successfully...!"});
                else
                    response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "Unable to delete book record.. Try again" });
            }
            catch(Exception ex)
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "Server error delete book record failed...Contact admin " });
            }

            return response;
        }
        ////api for update book details opertion
        //[HttpPost]
        //public HttpResponseMessage BookDetailsUpdate(Books bookModel)
        //{
        //    HttpResponseMessage response = null;

        //    try
        //    {
        //        BooksOps objBooksOps = new BooksOps() {
        //            BookId=bookModel.BookId,
        //            BookName=bookModel.BookName,
        //            PublisherId=bookModel.PublisherId,
        //            Edition=bookModel.Edition,
        //            EditionYear=bookModel.EditionYear,
        //            Pages =bookModel.Pages,
        //            CourseId=bookModel.CourseId,
        //            TotalCopies=bookModel.TotalCopies,
        //            ModifiedBy=bookModel.ModifiedBy
        //        };

        //        bool isUpdated=objBooksOps.Update();

        //        if (isUpdated)
        //            response = Request.CreateResponse(HttpStatusCode.OK, new { success = true, message = "Book Details Updated successfully.." });
        //        else
        //            response = Request.CreateResponse(HttpStatusCode.OK, new { success = false, message = "unable to update book details ..try again" });

        //    }
        //    catch(Exception ex)
        //    {
        //        response = Request.CreateResponse(HttpStatusCode.BadRequest, new { success = false, message = "Due to server error update book details failed .... check filled details or contact to admin" });
        //    }
        //    return response;
        //}
    }
}
