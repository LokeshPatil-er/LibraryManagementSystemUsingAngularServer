using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace LibrarySystemClassLibraryForApis
{
    public class IssueSupportFiles
    {
        public HttpPostedFileBase Files { get; set; }

        public int IssueSupportFileId { get; set; }
        public int BookIssueId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
