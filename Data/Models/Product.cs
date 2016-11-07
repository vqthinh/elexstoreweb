using System.Collections.Generic;
using System.Web.Script.Serialization;

namespace Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public partial class Product
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public double Price { get; set; }

        [Required]
        public string Image { get; set; }

        public DateTime ProductDate { get; set; }

        [StringLength(2500)]
        public string Description { get; set; }

        public int CategoryID { get; set; }

        [Required]
        [StringLength(50)]
        public string SupplierID { get; set; }

        public int Quantity { get; set; }

        public double Discount { get; set; }

        public bool Special { get; set; }

        public int Views { get; set; }
       
        public int? Sells { get; set; }

        public virtual Category Category { get; set; }

        public virtual Supplier Supplier { get; set; }

        public virtual ICollection<OrderDetails> OrderDetailses { get; set; } 
    }
}
