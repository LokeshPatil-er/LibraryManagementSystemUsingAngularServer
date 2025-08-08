using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace LibrarySystemClassLibraryForApis
{
    public class Books
    {
        public int BookId { get; set; }


        [StringLength(200, MinimumLength = 2, ErrorMessage = "Book name cannot be longer than 200 characters and less than 2 characters.")]
        [RegularExpression(@"^[A-Za-z0-9 .,-,#]*$", ErrorMessage = "Only alphanumeric characters, spaces, and . , - ,# are allowed.")]
        [Required(ErrorMessage = "Book name is required.")]
        [Display(Name = "Book Name")]
        public string BookName { get; set; }

        [Required(ErrorMessage = "Please select at least one course.")]
        public List<int> PublisherIdList { get; set; } = new List<int>();

        public string publishersIds => string.Join(",", PublisherIdList);

        public int PublisherId { get; set; }

        [Display(Name = "Publisher Name")]
        public string PublisherName { get; set; }


        public int CourseId { get; set; }

        public List<int> CoursesIdList { get; set; } = new List<int>();

        public string CoursesIdListIntoString => string.Join(",", CoursesIdList);

        [Display(Name = "Course Name")]
        public string CourseName { get; set; }


        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only numeric value allowed.")]
        public int Pages { get; set; }

        [Required(ErrorMessage = "Edition is required.(like 1st,2nd ,3rd etc.")]
        [RegularExpression(@"^\d+(st|nd|rd|th)$", ErrorMessage = "Only numeric values followed by 'st', 'nd', 'rd', or 'th' are allowed.")]
        public string Edition { get; set; }


        [Display(Name = "Edition Year")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Enter a valid 4-digit year.")]
        public string EditionYear { get; set; }

        [RegularExpression(@"^[0-9]*$", ErrorMessage = "Only numeric value allowed.")]
        [Display(Name = "Copies Count")]
        public int TotalCopies { get; set; }

        public int AvailableCount { get; set; }

        public int LostCount { get; set; }

        public int DamageCount { get; set; }

        public string ColumnNameForSort { get; set; } = "BookName";

        public string sortOrderForColumn { get; set; } = "asc";
        public bool IsActive { get; set; } = true;

        public int CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public int? ModifiedBy { get; set; }

      

        public DateTime? ModifiedOn { get; set; }

        public List<Books> bookList { get; set; }

        //public string SelectedPublisher { get; set; }

        public List<Publishers> PublishersList { get; set; }

        //public List<Authors> AuthorsList { get; set; }

        public List<Courses> CoursesList { get; set; }

        //public List<SelectListItem> IsAcitveList { get; set; }

        //  public int PublisherId { get; set; }

        public int AuthorId { get; set; }

        //  public int CourseId { get; set; }

        public string Author { get; set; }

        // [Display(Name = "Book Name :")]
        //  public string BookName { get; set; }

        [Display(Name = "Publisher :")]
        public string publisher { get; set; }

        [Display(Name = "Course :")]
        public string Course { get; set; }

        [Display(Name = "Page Number :")]

        public int PageNumber { get; set; }

        [Display(Name = "Page Size :")]
        public int PageSize { get; set; } = 5;

        [Display(Name = "Total Records")]
        public int TotalRecords { get; set; }

        public int TotalPages { get; set; } = 1;


        // public bool? IsActive { get; set; }
        public int BookIssueDetailId { get; set; }
        public int BookIssueId { get; set; }
        public int IssueQuantity { get; set; } = 1;
        public int ReturnQuantity { get; set; }
        public bool ReturnStatus { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public bool IsPenalty { get; set; }
    }
}
