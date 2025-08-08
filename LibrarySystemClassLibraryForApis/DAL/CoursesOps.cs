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
    public class CoursesOps
    {
        private Database db= DatabaseFactory.CreateDatabase("constr");
        private Courses objCourses = new Courses();
        public CoursesOps()
        {
            this.db = DatabaseFactory.CreateDatabase();
        }

        public CoursesOps(int CourseId)
        {
            this.db = DatabaseFactory.CreateDatabase();
            objCourses.CourseId = CourseId;
        }

        public List<Courses> GetCoursesList()
        {
            List<Courses> coursesList = new List<Courses>();
            try
            {
                DbCommand dbCommand = this.db.GetStoredProcCommand("coursesGetList");
                DataSet ds = this.db.ExecuteDataSet(dbCommand);
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        coursesList.Add(new Courses
                        {
                            CourseId = Convert.ToInt32(row["CourseId"]),
                            Course = (row["Course"]).ToString(),
                            DepartmentId = Convert.ToInt32(row["DepartmentId"]),
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
                Console.WriteLine(ex.StackTrace);
               
            }
            return coursesList;
        }
    }
}
