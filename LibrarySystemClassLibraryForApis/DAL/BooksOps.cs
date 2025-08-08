using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemClassLibraryForApis
{
    public class BooksOps
    {
        //create database instance
       private Database db = DatabaseFactory.CreateDatabase("constr");
        
        //declare property related to book opertion

        public int BookId { get; set; }

        public string BookName { get; set; }

        public int PublisherId { get; set; }

        public string PublisherName { get; set; }

        public int CourseId { get; set; }

        public string CourseName { get; set; }

        public int Pages { get; set; }

        public string Edition { get; set; }

        public string EditionYear { get; set; }

        public int TotalCopies { get; set; }

        public int AvailableCount { get; set; }

        public int LostCount { get; set; }

        public int DamageCount { get; set; }

        public bool IsActive { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        //method work for load book details using perticular book id
        public bool Load()
        {
            try
            {
                if (this.BookId != 0)
                {
                    DbCommand dbCommand = this.db.GetStoredProcCommand("booksGetListById");
                    this.db.AddInParameter(dbCommand, "BookId", DbType.Int32, this.BookId);
                    DataSet dataSet = this.db.ExecuteDataSet(dbCommand);
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        DataTable dataTable = dataSet.Tables[0];
                        this.BookId = Convert.ToInt32(dataTable.Rows[0]["BookId"]);
                        this.BookName = Convert.ToString(dataTable.Rows[0]["BookName"]);
                        this.PublisherId = Convert.ToInt32(dataTable.Rows[0]["PublisherId"]);
                        this.PublisherName = Convert.ToString(dataTable.Rows[0]["PublisherName"]);
                        this.CourseId = Convert.ToInt32(dataTable.Rows[0]["CourseId"]);
                        this.Pages = Convert.ToInt32(dataTable.Rows[0]["Pages"]);
                        this.Edition = Convert.ToString(dataTable.Rows[0]["Edition"]);
                        this.EditionYear = Convert.ToString(dataTable.Rows[0]["EditionYear"]);
                        this.TotalCopies = Convert.ToInt32(dataTable.Rows[0]["TotalCopies"]);
                        this.AvailableCount = Convert.ToInt32(dataTable.Rows[0]["AvailableCount"]);
                        this.LostCount = Convert.ToInt32(dataTable.Rows[0]["LostCount"]);
                        this.DamageCount = Convert.ToInt32(dataTable.Rows[0]["DamageCount"]);
                        this.IsActive = Convert.ToBoolean(dataTable.Rows[0]["IsActive"]);
                        this.CreatedBy = Convert.ToInt32(dataTable.Rows[0]["CreatedBy"]);
                        this.CreatedOn = Convert.ToDateTime(dataTable.Rows[0]["CreatedOn"]);
                        this.ModifiedBy = Convert.ToInt32(dataTable.Rows[0]["ModifiedBy"]);
                        this.ModifiedOn = Convert.ToDateTime(dataTable.Rows[0]["ModifiedOn"]);
                        return true;


                    }
                }
                return false;
            }
            catch (Exception ex)
            {

               // FileOpertions.LogExceptionInFile(ex);
                return false;
            }

        }

        //method work to get all books details from table in list form
        public List<Books> GetBooksList(Books model)
        {
           
            List<Books> BooksList = new List<Books>();

            try
            {
                DbCommand dbCommand = db.GetStoredProcCommand("booksGetList");

                //if (model.IsActive == false)
                //{
                //    this.db.AddInParameter(dbCommand, "IsActive", DbType.Int32, 0);
                //}
                //else
                //{
                //    this.db.AddInParameter(dbCommand, "IsActive", DbType.Int32, 1);
                //}


                if (model.PageNumber > 0)
                {
                    this.db.AddInParameter(dbCommand, "pageNumber", DbType.Int32, model.PageNumber);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "pageNumber", DbType.Int32, 1);
                }
                this.db.AddInParameter(dbCommand, "IsActive", DbType.Int32, model.IsActive);
                this.db.AddInParameter(dbCommand, "pageSize", DbType.Int32, model.PageSize);

                if (!String.IsNullOrEmpty(model.BookName))
                {
                    this.db.AddInParameter(dbCommand, "BookName", DbType.String, model.BookName);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "BookName", DbType.String, DBNull.Value);
                }

                //if(model.PublisherId > 0)
                //{
                //this.db.AddInParameter(dbCommand, "PublisherId", DbType.Int32, model.PublisherId);
                //}
                //else
                //{
                //    this.db.AddInParameter(dbCommand, "PublisherId", DbType.Int32, DBNull.Value);
                //}

                if (!String.IsNullOrEmpty(model.publishersIds))
                {
                    this.db.AddInParameter(dbCommand, "PublisherIds", DbType.String, model.publishersIds);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "PublisherIds", DbType.String, DBNull.Value);
                }

                //if (model.CourseId > 0)
                //{
                //    this.db.AddInParameter(dbCommand, "CourseId", DbType.Int32, model.CourseId);
                //}
                //else
                //{
                //    this.db.AddInParameter(dbCommand, "CourseId", DbType.Int32, DBNull.Value);
                //}

                if (!String.IsNullOrEmpty(model.CoursesIdListIntoString))
                {
                    this.db.AddInParameter(dbCommand, "CoursesIdListIntoString", DbType.String, model.CoursesIdListIntoString);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "CoursesIdListIntoString", DbType.String, DBNull.Value);
                }

                if (!String.IsNullOrEmpty(model.ColumnNameForSort))
                {
                    this.db.AddInParameter(dbCommand, "ColumnNameForSort", DbType.String, model.ColumnNameForSort);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "ColumnNameForSort", DbType.String, "BookName");
                }


                if (!String.IsNullOrEmpty(model.sortOrderForColumn))
                {
                    this.db.AddInParameter(dbCommand, "SortOrderForColumn", DbType.String, model.sortOrderForColumn);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "SortOrderForColumn", DbType.String, "ASC");
                }


                this.db.AddOutParameter(dbCommand, "TotalRecord", DbType.Int32, sizeof(int));
                DataSet ds = db.ExecuteDataSet(dbCommand);
                int totalRecord = (int)this.db.GetParameterValue(dbCommand, "TotalRecord");

                 model.TotalRecords = totalRecord;

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        BooksList.Add(new Books
                        {
                            BookId = Convert.ToInt32(row["BookId"]),
                            BookName = row["BookName"].ToString(),
                            PublisherName = Convert.ToString(row["PublisherName"]),
                            CourseName = Convert.ToString(row["CourseName"]),
                            Pages = Convert.ToInt32(row["Pages"]),
                            Edition = row["Edition"].ToString(),
                            EditionYear = Convert.ToString(row["EditionYear"]),
                            TotalCopies = Convert.ToInt32(row["TotalCopies"]),
                            AvailableCount = Convert.ToInt32(row["AvailableCount"]),
                            LostCount = Convert.ToInt32(row["LostCount"]),
                            DamageCount = Convert.ToInt32(row["DamageCount"]),
                            IsActive = Convert.ToBoolean(row["IsActive"]),
                            CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                            CreatedOn = Convert.ToDateTime(row["CreatedOn"]),
                            ModifiedBy = Convert.ToInt32(row["ModifiedBy"]),
                            ModifiedOn = Convert.ToDateTime(row["ModifiedOn"])
                        });
                    }
                }
               // return BooksList;
            }
            catch (Exception ex)
            {
                //ExceptionsLogsOps objExceptionsLogsOps = new ExceptionsLogsOps()
                //{
                //    ExceptionLogSource = ex.Source,
                //    ExceptionMessage = ex.Message,
                //    StackTrace = ex.StackTrace,
                //    CreatedBy = 1,
                //    CreatedOn = DateTime.Now

                //};
                //bool IsExceptionLog = objExceptionsLogsOps.Insert();

                //if (!IsExceptionLog)
                //{
                //    FileOpertions.LogExceptionInFile(ex);

                //}

                // Handle exception
            }
            return BooksList;
        }

        //method to decide to which method call from insert ,update
        public bool Save()
        {
            if (this.BookId == 0)
            {
                return this.Insert();
            }
            else
            {
                if (this.BookId > 0)
                {
                    return this.Update();
                }
                else
                {
                    this.BookId = 0;
                    return false;
                }
            }
        }

        //method work for insert book details into table
        private bool Insert()
        {
            try
            {
                DbCommand dbCommand = this.db.GetStoredProcCommand("booksInsert");
                this.db.AddOutParameter(dbCommand, "BookId", DbType.Int32, 1024);
                if (!String.IsNullOrEmpty(this.BookName))
                {
                    this.db.AddInParameter(dbCommand, "BookName", DbType.String, this.BookName);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "BookName", DbType.String, DBNull.Value);
                }
                if (this.PublisherId > 0)
                {
                    this.db.AddInParameter(dbCommand, "PublisherId", DbType.String, this.PublisherId);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "PublisherId", DbType.String, DBNull.Value);
                }
                if (this.CourseId > 0)
                {
                    this.db.AddInParameter(dbCommand, "CourseId", DbType.String, this.CourseId);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "CourseId", DbType.String, DBNull.Value);
                }
                if (this.Pages > 0)
                {
                    this.db.AddInParameter(dbCommand, "Pages", DbType.String, this.Pages);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "Pages", DbType.String, DBNull.Value);
                }
                if (!String.IsNullOrEmpty(this.Edition))
                {
                    this.db.AddInParameter(dbCommand, "Edition", DbType.String, this.Edition);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "Edition", DbType.String, DBNull.Value);
                }
                if (!String.IsNullOrEmpty(this.EditionYear))
                {
                    this.db.AddInParameter(dbCommand, "EditionYear", DbType.String, this.EditionYear);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "EditionYear", DbType.String, DBNull.Value);
                }
                if (this.TotalCopies > 0)
                {
                    this.db.AddInParameter(dbCommand, "TotalCopies", DbType.Int32, this.TotalCopies);

                }
                else
                {
                    this.db.AddInParameter(dbCommand, "TotalCopies", DbType.Int32, DBNull.Value);


                }
                this.db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, this.IsActive);
                if (this.CreatedBy > 0)
                {
                    this.db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, this.CreatedBy);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, DBNull.Value);
                }
                if (this.CreatedOn > DateTime.MinValue)
                {
                    this.db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, this.CreatedOn);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, DBNull.Value);
                }
                this.db.ExecuteNonQuery(dbCommand);
                this.BookId = Convert.ToInt32(this.db.GetParameterValue(dbCommand, "BookId"));
            }
            catch (Exception ex)
            {
               
                return false;


            }
            return this.BookId > 0;
        }

        //method work for update book details into table
        private bool Update()
        {
            try
            {
                DbCommand dbCommand = this.db.GetStoredProcCommand("booksUpdate");
                this.db.AddInParameter(dbCommand, "BookId", DbType.Int32, this.BookId);
                if (!String.IsNullOrEmpty(this.BookName))
                {
                    this.db.AddInParameter(dbCommand, "BookName", DbType.String, this.BookName);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "BookName", DbType.String, DBNull.Value);
                }
                this.db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, this.IsActive);

                if (this.PublisherId > 0)
                {
                    this.db.AddInParameter(dbCommand, "PublisherId", DbType.Int32, this.PublisherId);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "PublisherId", DbType.Int32, DBNull.Value);
                }

                if (this.CourseId > 0)
                {
                    this.db.AddInParameter(dbCommand, "CourseId", DbType.Int32, this.CourseId);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "CourseId", DbType.Int32, DBNull.Value);
                }


                if (this.Pages > 0)
                {
                    this.db.AddInParameter(dbCommand, "Pages", DbType.Int32, this.Pages);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "Pages", DbType.Int32, DBNull.Value);
                }

                if (!String.IsNullOrEmpty(this.Edition))
                {
                    this.db.AddInParameter(dbCommand, "Edition", DbType.String, this.Edition);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "Edition", DbType.String, DBNull.Value);
                }

                if (!String.IsNullOrEmpty(this.EditionYear))
                {
                    this.db.AddInParameter(dbCommand, "EditionYear", DbType.String, this.EditionYear);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "EditionYear", DbType.String, DBNull.Value);
                }

                if (this.TotalCopies > 0)
                {
                    this.db.AddInParameter(dbCommand, "TotalCopies", DbType.Int32, this.TotalCopies);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "TotalCopies", DbType.Int32, DBNull.Value);
                }

                if (this.ModifiedBy > 0)
                {
                    this.db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, this.ModifiedBy);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "ModifiedBy", DbType.Int32, DBNull.Value);
                }

                this.db.ExecuteNonQuery(dbCommand);
            }
            catch (Exception ex)
            {
                // To Do: Handle Exception
                //FileOpertions.LogExceptionInFile(ex);
                return false;
            }

            return true;
        }

        //method work for soft delete book record
        public bool Delete()
        {
            try
            {
                DbCommand com = this.db.GetStoredProcCommand("booksDelete");

                this.db.AddInParameter(com, "BookId", DbType.Int32, this.BookId);

                if (this.ModifiedBy > 0)
                {
                    this.db.AddInParameter(com, "ModifiedBy", DbType.Int32, this.ModifiedBy);
                }
                else
                {
                    this.db.AddInParameter(com, "ModifiedBy", DbType.Int32, DBNull.Value);
                }
                if (this.ModifiedOn > DateTime.MinValue)
                {
                    this.db.AddInParameter(com, "ModifiedOn", DbType.DateTime, this.ModifiedOn);
                }
                else
                {
                    this.db.AddInParameter(com, "ModifiedOn", DbType.DateTime, DBNull.Value);
                }
                this.db.ExecuteNonQuery(com);
            }
            catch (Exception ex)
            {
                // To Do: Handle Exception
                return false;
            }

            return true;
        }
    }
}
