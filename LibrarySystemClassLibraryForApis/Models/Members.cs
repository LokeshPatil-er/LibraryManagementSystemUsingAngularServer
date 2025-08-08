using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemClassLibraryForApis
{
    public class Members
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
    }
}
