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
    public class MembersOps
    {
       

        public int MemberId { get; set; }
        public string Name { get; set; }
        public int MemberTypeId { get; set; }
        public int DepartmentId { get; set; }
        public string Address { get; set; }
        public DateTime DOB { get; set; }
        public int GenderId { get; set; }
        public string MobileNo { get; set; }
        public string EmailId { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }

        private Database db = DatabaseFactory.CreateDatabase("constr");

        public MembersOps()
        {
            this.db = DatabaseFactory.CreateDatabase();

        }

        public MembersOps(int MemberId)
        {
            this.db = DatabaseFactory.CreateDatabase();
            this.MemberId = MemberId;
        }

        public bool Load()
        {
            try
            {
                if (this.MemberId != 0)
                {
                    DbCommand dbCommand = this.db.GetStoredProcCommand("membersGetById");
                    this.db.AddInParameter(dbCommand, "MemberId", DbType.Int32, this.MemberId);
                    DataSet dataSet = this.db.ExecuteDataSet(dbCommand);
                    if (dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows.Count > 0)
                    {
                        DataTable dataTable = dataSet.Tables[0];
                        this.MemberId = Convert.ToInt32(dataTable.Rows[0]["MemberId"]);
                        this.Name = Convert.ToString(dataTable.Rows[0]["Name"]);
                        this.MemberTypeId = Convert.ToInt32(dataTable.Rows[0]["MemberTypeId"]);
                        this.DepartmentId = Convert.ToInt32(dataTable.Rows[0]["DepartmentId"]);
                        this.Address = Convert.ToString(dataTable.Rows[0]["Address"]);
                        this.DOB = Convert.ToDateTime(dataTable.Rows[0]["DOB"]);
                        this.GenderId = Convert.ToInt32(dataTable.Rows[0]["GenderId"]);
                        this.MobileNo = Convert.ToString(dataTable.Rows[0]["MobileNo"]);
                        this.EmailId = Convert.ToString(dataTable.Rows[0]["EmailId"]);
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
              
                return false;
            }
        }

        public List<Members> GetMembersList()
        {
            List<Members> membersList = new List<Members>();

            try
            {
                DbCommand dbCommand = db.GetStoredProcCommand("MembersGetList");

                DataSet ds = db.ExecuteDataSet(dbCommand);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        membersList.Add(new Members
                        {
                            MemberId = Convert.ToInt32(row["MemberId"]),
                            Name = Convert.ToString(row["MemberName"]),
                            MemberTypeId = Convert.ToInt32(row["MemberTypeId"]),
                            DepartmentId = Convert.ToInt32(row["DepartmentId"]),
                            Address = Convert.ToString(row["Address"]),
                            DOB = Convert.ToDateTime(row["DOB"]),
                            GenderId = Convert.ToInt32(row["GenderId"]),
                            MobileNo = Convert.ToString(row["MobileNo"]),
                            EmailId = Convert.ToString(row["EmailId"]),
                            IsActive = Convert.ToBoolean(row["IsActive"]),
                            CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                            CreatedOn = Convert.ToDateTime(row["CreatedOn"]),
                            ModifiedBy = Convert.ToInt32(row["ModifiedBy"]),
                            ModifiedOn = Convert.ToDateTime(row["ModifiedOn"])
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                
            }

            return membersList;
        }

    }
}
