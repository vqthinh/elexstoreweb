namespace Data.Models
{
    using System.ComponentModel.DataAnnotations;

    public partial class User
    {
        [Key]
        [StringLength(12)]
        public string UserName { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [Required]
        [StringLength(20)]
        public string Role { get; set; }

        [Required]
        [StringLength(250)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string Email { get; set; }
    }
}
