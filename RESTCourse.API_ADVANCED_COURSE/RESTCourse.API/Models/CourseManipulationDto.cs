using RESTCourse.API.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTCourse.API.Models
{
    [CourseTitleMustBeDifferent(ErrorMessage = "babababa")]

    public abstract class CourseManipulationDto
    {
            [Required(ErrorMessage = "You should fill out a title")]
            [MaxLength(100, ErrorMessage = "100 max")]
            public string Title { get; set; }
            public virtual string Description { get; set; } //Zeby zmniejszyc powtarzalnosc kodu (w Update desc jest Required w creation nie)
    }
}
