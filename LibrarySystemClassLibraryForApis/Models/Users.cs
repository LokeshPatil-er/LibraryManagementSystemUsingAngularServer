using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemClassLibraryForApis
{
    public class Users
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Contact { get; set; }
        public int RoleId { get; set; }
       // public List<Roles> RoleList { get; set; }
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
        public bool RememberMe { get; set; }
    }
}
