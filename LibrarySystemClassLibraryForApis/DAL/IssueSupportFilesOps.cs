using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemClassLibraryForApis
{
    public class IssueSupportFilesOps
    {
        public List<IssueSupportFilesOps> IssueSuppportFilesList { get; set; }
        public int IssueSupportFileId { get; set; }
        public int BookIssueId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        private Database db;

        public IssueSupportFilesOps()
        {
            this.db = DatabaseFactory.CreateDatabase("constr");
        }

        public IssueSupportFilesOps(int IssueSupportFileId)
        {
            this.db = DatabaseFactory.CreateDatabase("constr");
            this.IssueSupportFileId = IssueSupportFileId;
        }

        public bool InsertAndUpdate()
        {
            try
            {
                DataTable issueSupportFileTable = new DataTable();
                issueSupportFileTable.Columns.Add("IssueFileId", typeof(int));
                issueSupportFileTable.Columns.Add("BookIssueId", typeof(int));
                issueSupportFileTable.Columns.Add("FileName", typeof(string));
                issueSupportFileTable.Columns.Add("FilePath", typeof(string));
                issueSupportFileTable.Columns.Add("IsActive", typeof(bool));
                issueSupportFileTable.Columns.Add("CreatedBy", typeof(int));
                issueSupportFileTable.Columns.Add("CreatedOn", typeof(DateTime));
                issueSupportFileTable.Columns.Add("ModifiedBy", typeof(int));
                issueSupportFileTable.Columns.Add("ModifiedOn", typeof(DateTime));

                // Assuming IssueFiles is a List<IssueSupportFile> or similar
                foreach (var item in IssueSuppportFilesList)
                {


                    issueSupportFileTable.Rows.Add(
                        item.IssueSupportFileId,
                        item.BookIssueId,
                        item.FileName,
                        item.FilePath,
                       1,
                        1,
                       DateTime.Now,
                        item.ModifiedBy,
                        DateTime.Now
                    );
                }

                // Get the stored procedure command from Enterprise Library
                DbCommand cmd = db.GetStoredProcCommand("IssueSupportFilesInsertAndUpdate");

                // Add TVP parameter
                var tvpParam = new SqlParameter("@IssueSupportFiles", SqlDbType.Structured)
                {
                    TypeName = "IssueSupportFileTVP", // Your SQL user-defined table type name
                    Value = issueSupportFileTable
                };
                cmd.Parameters.Add(tvpParam);



                // Execute stored procedure
                db.ExecuteNonQuery(cmd);

                return true;
            }
            catch (Exception ex)
            {
            
                return false;
            }

        }
    }
}
