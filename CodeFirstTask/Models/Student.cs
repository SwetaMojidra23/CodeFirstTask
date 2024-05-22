using System.ComponentModel.DataAnnotations;

namespace CodeFirstTask.Models
{
    public class Student
    {
        [Key]
        public int ID { get; set; }

       
        public string StudentName { get; set; }

         
        public Standard Standard { get; set; }
         
        public string hobbies { get; set; }

        
        public Gender Gender { get; set; }

        public int PhoneNumber { get; set; }
        public string ProfilePicture { get; set; }
    }
}
