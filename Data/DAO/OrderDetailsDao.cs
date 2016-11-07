using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Models;

namespace Data.DAO
{
    public class OrderDetailsDao
    {
        ELEXStoreModels db;

        public OrderDetailsDao()
        {
            db = new ELEXStoreModels();
        }

        public bool Insert(OrderDetails orderDetails)
        {
            try
            {
                var product = db.Products.FirstOrDefault(x => x.ID == orderDetails.ProductID);
                product.Sells += orderDetails.Quantity;
                product.Quantity -= orderDetails.Quantity;
                db.OrderDetailses.Add(orderDetails);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }



    }
}
