using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeFirstTask.Models
{
    public class StudentCopy
    {

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Please Enter Full Name"),MaxLength(100)]
        public string StudentName { get; set; }

        [Required(ErrorMessage = "Please Select Standard")]
        public Standard Standard { get; set; }

        [NotMapped]
        public List<CheckBoxOption>? checkBoxHobbiesList { get; set; }

        [Required(ErrorMessage = "Please select at least one hobby")]
        public List<string> hobbies { get; set; }

        [Required(ErrorMessage = "Please select Gender")]
        public Gender Gender { get; set; }

        //[Required(ErrorMessage = "Mobile Number must Required")]
        //[RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Please enter a valid 10-digit mobile number.")]

        //[RegularExpression("^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Invalid Mobile Number")]
        public int PhoneNumber { get; set; }

        public string? ProfilePicture { get; set; }

        [NotMapped]
        public IFormFile? UploadedFile { get; set; } // New property for file upload
    }

    public enum Gender
    {
        Male, Female
    }

    public enum Standard
    {
        First, Second, Third, Fourth, Fifth, Sixth, Seventh, Eightth, Nineth, Tenth, Eleventh, Twelveth
    }
}
