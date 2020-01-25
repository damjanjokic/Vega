using System.ComponentModel.DataAnnotations;

namespace vega.Controllers.Resources
{
    public class ContactResource{
        [Required]
        [StringLength(225)]
        public string Name { get; set; }
        [StringLength(225)]
        public string Email { get; set; }
        [Required]
        [StringLength(225)]
        public string Phone { get; set; }
        
    }
}