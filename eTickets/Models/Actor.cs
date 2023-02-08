using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using eTickets.Data.Base;

namespace eTickets.Models
{
    public class Actor:IEntityBase
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Profile Picture")]
        [Required(ErrorMessage = "Profile picture is required")]
        public string ProfilePictureURL { get; set; }
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Full Name is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Full name must be 3-50 characters")]
        public string FullName { get; set; }
        [Display(Name = "Biography")]
        [Required(ErrorMessage = "Biography is required")]
        public string Bio { get; set; }
        public List<Actor_Movie> Actors_Movies{ get; set; }

    }
}
