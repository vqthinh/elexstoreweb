using System.Collections.Generic;

namespace Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public partial class Customer
    {
        [Key]
        public int CustomerID { get; set; }
        [StringLength(12)]
        public string UserID { get; set; }

        [Required]
        [StringLength(32)]
        public string Password { get; set; }

        [Required]
        [StringLength(200)]
        public string FullName { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        public bool Activated { get; set; }

    }
}
