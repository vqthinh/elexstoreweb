using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace Data.Models
{
    [Table("Order")]
    public partial class Order
    {
        [Key]
        public int ID { get; set; }

        public DateTime CreatedDate { get; set; }

        [StringLength(200)]
        public string ShipName { get; set; }

        public string ShipMobile { get; set; }

        public string ShipAddress { get; set; }

        [StringLength(250)]
        public string ShipEmail { get; set; }

        public string ShipDescription { get; set; }

        public string PaymentMethod { get; set; }
        public int Status { get; set; }

        public virtual ICollection<OrderDetails> OrderDetailses { get; set; }
        
    }
}
