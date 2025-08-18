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
   public class BooksIssueOps
    {
        // private Database db=DatabaseFactory.CreateDatabase("constr");
        private Database db;
        public int BookIssueId { get; set; }
        public int MemberId { get; set; }
        public int BookId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public BooksIssueOps()
        {
            this.db = DatabaseFactory.CreateDatabase("constr");

        }

        public BooksIssueOps(int BookIssueId)
        {
            this.db = DatabaseFactory.CreateDatabase("constr");
            this.BookIssueId = BookIssueId;
        }

        public bool Save()
        {
            if (this.BookIssueId == 0)
            {
                return this.Insert();
            }
            else
            {
                if (this.BookIssueId > 0)
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


        private bool Insert()
        {
            try
            {
                DbCommand dbCommand = this.db.GetStoredProcCommand("booksIssuesInsert");
                this.db.AddOutParameter(dbCommand, "BookIssueId", DbType.Int32, 1024);

                if (this.MemberId > 0)
                {
                    this.db.AddInParameter(dbCommand, "MemberId", DbType.Int32, this.MemberId);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "MemberId", DbType.Int32, DBNull.Value);
                }



                if (this.IssueDate > DateTime.MinValue)
                {
                    this.db.AddInParameter(dbCommand, "IssueDate", DbType.DateTime, this.IssueDate);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "IssueDate", DbType.DateTime, DBNull.Value);
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
                this.BookIssueId = Convert.ToInt32(this.db.GetParameterValue(dbCommand, "BookIssueId"));

            }
            catch (Exception ex)
            {
              //  FileOpertions.LogExceptionInFile(ex);
                return false;
            }

            return this.BookIssueId > 0;
        }

        private bool Update()
        {
            try
            {
                DbCommand dbCommand = this.db.GetStoredProcCommand("booksIssuesUpdate");

                // Required
                this.db.AddInParameter(dbCommand, "BookIssueId", DbType.Int32, this.BookIssueId);

                if (this.MemberId > 0)
                {
                    this.db.AddInParameter(dbCommand, "MemberId", DbType.Int32, this.MemberId);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "MemberId", DbType.Int32, DBNull.Value);
                }

                if (this.IssueDate != DateTime.MinValue)
                {
                    this.db.AddInParameter(dbCommand, "IssueDate", DbType.DateTime, this.IssueDate);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "IssueDate", DbType.DateTime, DBNull.Value);
                }

                this.db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, this.IsActive);

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
               // FileOpertions.LogExceptionInFile(ex);
                return false;
            }

            return true;
        }
    }
}
