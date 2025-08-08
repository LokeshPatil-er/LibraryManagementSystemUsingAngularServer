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
    public class PublishersOps
    {
        private Database db= DatabaseFactory.CreateDatabase("constr");
        private Publishers publishers = new Publishers();
        public PublishersOps()
        {
            this.db = DatabaseFactory.CreateDatabase();
        }
        public PublishersOps(int PublishersId)
        {
            this.db = DatabaseFactory.CreateDatabase();
            publishers.PublisherId = PublishersId;
        }

        public List<Publishers> GetPublishersList()
        {
            List<Publishers> publishersList = new List<Publishers>();

            DbCommand dbCommand = this.db.GetStoredProcCommand("PublishersGetList");
            try
            {
                DataSet ds = this.db.ExecuteDataSet(dbCommand);

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        publishersList.Add(new Publishers
                        {
                            PublisherId = Convert.ToInt32(row["PublisherId"]),
                            Publisher = (row["Publisher"]).ToString(),
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
            return publishersList;
        }
    }
}
