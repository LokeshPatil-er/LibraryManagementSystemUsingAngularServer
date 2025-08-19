using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibrarySystemClassLibraryForApis
{
    public class BookIssuesDetailList
    {
        public List<BooksIssueDetails> BooksIssuedList { get; set; }
    }

    public class BooksIssueDetails
    {
        public int BookIssueDetailId { get; set; }
        public int BookIssueId { get; set; }
        public int BookId { get; set; }

        public string BookName { get; set; }
        public string Publisher { get; set; }
        public string Author { get; set; }

        public DateTime IssueDate { get; set; }

        public List<int> SelectedBookId { get; set; }
        public List<IssueSupportFiles> SelectedFilesForUpload { get; set; }

        public List<Books> BookList { get; set; }

        public int MemberId { get; set; }

        public List<Members> MembersList { get; set; }

        [Display(Name = "Name:")]
        public string MemberName { get; set; }

        [Display(Name = "Contact:")]
        public string MemberContact { get; set; }

      //  public int IssueQuantity { get; set; } = 1;
        public int ReturnQuantity { get; set; }
        public bool ReturnStatus { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public bool IsPenalty { get; set; }
        public bool IsActive { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public int ModifiedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
    }
}
