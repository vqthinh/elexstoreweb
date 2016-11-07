namespace Data.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    // ReSharper disable once PartialTypeWithSinglePart
    public partial class Category
    {
        public Category()
        {
            Products = new HashSet<Product>();
        }

        public int ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }
        [StringLength(250)] 
        public string Image { get; set; }
        [StringLength(250)]
        public string Icon { get; set; }

        // ReSharper disable once MemberCanBeProtected.Global
        public virtual ICollection<Product> Products { get; set; }
    }
}
