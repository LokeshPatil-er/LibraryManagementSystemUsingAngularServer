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
    public class UsersOps
    {
        private Database db;

        public int UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public int RoleId { get; set; }
        public DateTime? JoiningDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }

        public UsersOps()
        {
            this.db = DatabaseFactory.CreateDatabase("constr");
        }

        public UsersOps(int UserId)
        {
            this.db = DatabaseFactory.CreateDatabase("constr");
            this.UserId = UserId;
        }


        public bool Load()
        {

            try
            {
                DbCommand dbCommand = this.db.GetStoredProcCommand("usersGetListByEmail");
                this.db.AddInParameter(dbCommand, "Email", DbType.String, this.Email);
                DataSet ds = this.db.ExecuteDataSet(dbCommand);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    DataRow row = ds.Tables[0].Rows[0];
                    this.UserId = Convert.ToInt32(row["UserID"]);
                    this.Name = Convert.ToString(row["Name"]);
                    this.Address = Convert.ToString(row["Address"]);
                    this.Contact = Convert.ToString(row["Contact"]);
                    this.Email = Convert.ToString(row["Email"]);
                    this.Password = Convert.ToString(row["Password"]);
                    this.RoleId = Convert.ToInt32(row["RoleId"]);
                    this.JoiningDate = row["JoiningDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["JoiningDate"]);
                    this.ExitDate = row["ExitDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["ExitDate"]);
                    this.IsActive = Convert.ToBoolean(row["IsActive"]);
                    this.CreatedBy = Convert.ToInt32(row["CreatedBy"]);
                    this.CreatedOn = Convert.ToDateTime(row["CreatedOn"]);
                    this.ModifiedBy = row["ModifiedBy"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["ModifiedBy"]);
                    this.ModifiedOn = row["ModifiedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["ModifiedOn"]);


                }
            }
            catch (Exception ex)
            {
              
                return false;
            }
            return true;
        }

        public bool save()
        {
            if (this.UserId == 0)
            {
                return this.Insert();
            }
            else if (this.UserId > 0)
            {
                return true;
            }
            return false;
        }

        private bool Insert()
        {
            try
            {
                DbCommand dbCommand = this.db.GetStoredProcCommand("usersInsert");


                this.db.AddOutParameter(dbCommand, "UserId", DbType.Int32, sizeof(int));


                this.db.AddInParameter(dbCommand, "Name", DbType.String, this.Name ?? (object)DBNull.Value);


                this.db.AddInParameter(dbCommand, "Address", DbType.String, string.IsNullOrEmpty(this.Address) ? (object)DBNull.Value : this.Address);


                this.db.AddInParameter(dbCommand, "Contact", DbType.String, string.IsNullOrEmpty(this.Contact) ? (object)DBNull.Value : this.Contact);


                this.db.AddInParameter(dbCommand, "RoleId", DbType.Int32, this.RoleId > 0 ? (object)this.RoleId : DBNull.Value);

                this.db.AddInParameter(dbCommand, "JoiningDate", DbType.DateTime, this.JoiningDate > DateTime.MinValue ? (object)this.JoiningDate : DBNull.Value);


                this.db.AddInParameter(dbCommand, "ExitDate", DbType.DateTime, this.ExitDate > DateTime.MinValue ? (object)this.ExitDate : DBNull.Value);


                this.db.AddInParameter(dbCommand, "Email", DbType.String, this.Email ?? (object)DBNull.Value);


                this.db.AddInParameter(dbCommand, "Password", DbType.String, this.Password ?? (object)DBNull.Value);


                this.db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, this.IsActive);


                this.db.AddInParameter(dbCommand, "CreatedBy", DbType.Int32, this.CreatedBy > 0 ? (object)this.CreatedBy : DBNull.Value);


                this.db.ExecuteNonQuery(dbCommand);


                this.UserId = Convert.ToInt32(this.db.GetParameterValue(dbCommand, "UserId"));
            }
            catch (Exception ex)
            {
              
                return false;
            }

            return this.UserId > 0;
        }

        public List<Users> usersGetList()
        {
            var userList = new List<Users>();
            try
            {
                DbCommand dbCommand = this.db.GetStoredProcCommand("usersGetList");
                this.db.AddInParameter(dbCommand, "Email", DbType.String, this.Email);
                DataSet ds = this.db.ExecuteDataSet(dbCommand);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        userList.Add(new Users
                        {
                            UserId = Convert.ToInt32(row["UserID"]),
                            Name = Convert.ToString(row["Name"]),
                            Address = Convert.ToString(row["Address"]),
                            Contact = Convert.ToString(row["Contact"]),
                            Email = Convert.ToString(row["Email"]),
                            Password = Convert.ToString(row["Password"]),
                            RoleId = Convert.ToInt32(row["RoleId"]),
                            JoiningDate = row["JoiningDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["JoiningDate"]),
                            ExitDate = row["ExitDate"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["ExitDate"]),
                            IsActive = Convert.ToBoolean(row["IsActive"]),
                            CreatedBy = Convert.ToInt32(row["CreatedBy"]),
                            CreatedOn = Convert.ToDateTime(row["CreatedOn"]),
                            ModifiedBy = row["ModifiedBy"] == DBNull.Value ? (int?)null : Convert.ToInt32(row["ModifiedBy"]),
                            ModifiedOn = row["ModifiedOn"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(row["ModifiedOn"]),
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                
                return userList;
            }
            return userList;
        }
    }
}
