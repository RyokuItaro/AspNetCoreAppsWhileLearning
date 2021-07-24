using RESTCourse.API.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTCourse.API.ValidationAttributes
{
    public class CourseTitleMustBeDifferent : ValidationAttribute // LEPSZY SPOSÓB NA VALIDACJE [uzywajac data annotations]
    {
        protected override ValidationResult IsValid(object value,
            ValidationContext validationContext)
        {
            var course = (CourseManipulationDto)validationContext.ObjectInstance;

            if(course.Title == course.Description)
            {
                return new ValidationResult(ErrorMessage, new[] { nameof(CourseManipulationDto) });
            }
            return ValidationResult.Success;
        }
    }
}
