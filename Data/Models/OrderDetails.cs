using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using Data.Models;

namespace Data.Models
{
    [Table("OrderDetails")]
    public partial class OrderDetails
    {   [Key]
        public int ID { get; set; }
        public int ProductID { get; set; }

        public int OrderID { get; set; }

        public int Quantity { get; set; }

        public double Price { get; set; }

        public virtual Product Product { get; set; }

        public virtual Order Order { get; set; }
    }
}
