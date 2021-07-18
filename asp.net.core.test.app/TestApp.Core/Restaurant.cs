using System.ComponentModel.DataAnnotations;

namespace TestApp.Core
{
    public class Restaurant
    {
        [Required]
        [StringLength(80)]
        public string Name { get; set; }
        [Required]
        [StringLength(255)]
        public string Location { get; set; }
        public int Id { get; set; }
        public CuisineType Cuisine { get; set; }

    }
}
