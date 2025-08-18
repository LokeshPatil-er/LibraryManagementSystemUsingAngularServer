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
    public class BooksIssueDetailsOps
    {
        private Database db=DatabaseFactory.CreateDatabase("constr");

        public int BookIssueDetailId { get; set; }
        public int BookIssueId { get; set; }
        public int BookId { get; set; }
        public List<BooksIssueDetailsOps> BookDetails { get; set; }
        public int IssueQuantity { get; set; } = 1;
        public int ReturnQuantity { get; set; }
        public bool ReturnStatus { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsPenalty { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        public List<BooksIssueDetails> GetBookIssuesDetailList()
        {
            var viewModel = new BookIssuesDetailList()
            {
                BooksIssuedList = new List<BooksIssueDetails>()

            };



            try
            {

                DbCommand dbCommand = db.GetStoredProcCommand("GetBookIssuesDetailList");
                if (this.BookIssueId > 0)
                {
                    this.db.AddInParameter(dbCommand, "BookIssueId", DbType.Int32, this.BookIssueId);
                }
                else
                {
                    this.db.AddInParameter(dbCommand, "BookIssueId", DbType.Int32, DBNull.Value);
                }
                DataSet ds = db.ExecuteDataSet(dbCommand);

                if (ds != null && ds.Tables.Count >= 3)
                {
                    DataTable issueTable = ds.Tables[0];
                    DataTable bookTable = ds.Tables[1];
                    DataTable supportFileTable = ds.Tables[2]; // new

                    Dictionary<int, BooksIssueDetails> issueDict = new Dictionary<int, BooksIssueDetails>();

                    // Process issue table
                    foreach (DataRow row in issueTable.Rows)
                    {
                        int issueId = Convert.ToInt32(row["BookIssueId"]);

                        var issue = new BooksIssueDetails
                        {
                            BookIssueId = row["BookIssueId"] != DBNull.Value ? Convert.ToInt32(row["BookIssueId"]) : 0,
                            MemberId = Convert.ToInt32(row["MemberId"]),
                            MemberName = row["Name"].ToString(),
                            MemberContact = row["MemberContact"] != DBNull.Value ? row["MemberContact"].ToString() : "",
                            IssueDate = row["IssueDate"] != DBNull.Value ? Convert.ToDateTime(row["IssueDate"]) : DateTime.MinValue,
                            CreatedBy = row["CreatedBy"] != DBNull.Value ? Convert.ToInt32(row["CreatedBy"]) : 0,
                            ModifiedBy = row["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(row["ModifiedBy"]) : 0,
                            CreatedOn = row["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(row["CreatedOn"]) : DateTime.MinValue,
                            ModifiedOn = row["ModifiedOn"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["ModifiedOn"]) : null,
                            BookList = new List<Books>(),
                            SelectedFilesForUpload = new List<IssueSupportFiles>() // init here
                        };

                        issueDict[issueId] = issue;
                    }

                    // Process book table
                    foreach (DataRow row in bookTable.Rows)
                    {
                        int issueId = Convert.ToInt32(row["BookIssueId"]);

                        if (issueDict.ContainsKey(issueId))
                        {
                            var book = new Books
                            {
                                BookId = Convert.ToInt32(row["BookId"]),
                                BookName = row["BookName"]?.ToString(),
                                PublisherName = row["PublisherName"]?.ToString(),
                                IssueQuantity = Convert.ToInt32(row["IssueQuantity"]),
                                ReturnQuantity = Convert.ToInt32(row["ReturnQuantity"]),
                                BookIssueDetailId = Convert.ToInt32(row["BookIssueDetailId"]),
                                BookIssueId = Convert.ToInt32(row["BookIssueId"]),
                                DueDate = Convert.ToDateTime(row["DueDate"]),
                                ReturnDate = row["ReturnDate"] != DBNull.Value ? Convert.ToDateTime(row["ReturnDate"]) : DateTime.MinValue,
                                ReturnStatus = Convert.ToBoolean(row["ReturnStatus"]),
                                IsPenalty = Convert.ToBoolean(row["IsPenalty"])
                            };

                            issueDict[issueId].BookList.Add(book);
                        }
                    }

                    // Process support file table
                    foreach (DataRow row in supportFileTable.Rows)
                    {
                        int issueId = Convert.ToInt32(row["BookIssueId"]);

                        if (issueDict.ContainsKey(issueId))
                        {
                            var file = new IssueSupportFiles
                            {
                                IssueSupportFileId = Convert.ToInt32(row["IssueSupportFileId"]),
                                BookIssueId = issueId,
                                FileName = row["FileName"]?.ToString(),
                                FilePath = row["FilePath"]?.ToString(),
                                IsActive = Convert.ToBoolean(row["IsActive"]),
                                CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                                CreatedOn = Convert.ToDateTime(row["CreatedOn"]),
                                ModifiedBy = row["ModifiedBy"] != DBNull.Value ? Convert.ToInt32(row["ModifiedBy"]) : 0,
                                // ModifiedOn = row["ModifiedOn"] != DBNull.Value ? (DateTime?)Convert.ToDateTime(row["ModifiedOn"]) : null
                            };

                            issueDict[issueId].SelectedFilesForUpload.Add(file);
                        }
                    }

                    viewModel.BooksIssuedList = issueDict.Values.ToList();
                }

            }
            catch (Exception ex)
            {
                // Log or handle exception
                Console.WriteLine("Error in GetBookIssuesDetailList: " + ex.Message);
            }

            return viewModel.BooksIssuedList;
        }

        public bool InsertANDUpdate()
        {
            try
            {
                // Create and fill the DataTable for TVP
                DataTable bookDetailsTable = new DataTable();
                bookDetailsTable.Columns.Add("BookIssueDetailId", typeof(int));
                bookDetailsTable.Columns.Add("BookIssueId", typeof(int));
                bookDetailsTable.Columns.Add("BookId", typeof(int));
                bookDetailsTable.Columns.Add("IssueQuantity", typeof(int));
                bookDetailsTable.Columns.Add("ReturnQuantity", typeof(int));
                bookDetailsTable.Columns.Add("ReturnStatus", typeof(bool));
                bookDetailsTable.Columns.Add("DueDate", typeof(DateTime));
                bookDetailsTable.Columns.Add("IsPenalty", typeof(bool));
                bookDetailsTable.Columns.Add("IsActive", typeof(bool));
                bookDetailsTable.Columns.Add("ModifiedBy", typeof(int));

                foreach (var item in BookDetails)
                {
                    bookDetailsTable.Rows.Add(
                        item.BookIssueDetailId,
                        item.BookIssueId,
                        item.BookId,
                        item.IssueQuantity,
                        item.ReturnQuantity,
                        item.ReturnStatus,
                        item.DueDate,
                        item.IsPenalty,
                        item.IsActive,
                        item.ModifiedBy
                    );
                }

                // Get the stored procedure command from Enterprise Library
                DbCommand cmd = db.GetStoredProcCommand("booksIssuesDetailInseretANDUpdate");

                // Add TVP parameter
                var tvpParam = new SqlParameter("@BookDetails", SqlDbType.Structured)
                {
                    TypeName = "BookIssueDetailTableValuedParameter", // Your SQL user-defined table type name
                    Value = bookDetailsTable
                };
                cmd.Parameters.Add(tvpParam);

                // Add ModifiedBy parameter — pick from the first item or default 0
                int modifiedBy = BookDetails.FirstOrDefault()?.ModifiedBy ?? 0;
                db.AddInParameter(cmd, "@ModifiedBy", DbType.Int32, modifiedBy);

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
