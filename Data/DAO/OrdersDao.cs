using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Data.Models;
using PagedList;

namespace Data.DAO
{
    public class OrdersDao
    {
        ELEXStoreModels db = null;

        public OrdersDao ()
        {
            db = new ELEXStoreModels();
        }

        public IPagedList<Order> ListAllPaging(int pageSize, int page)
        {
            return db.Orders.OrderBy(x => x.Status).ToPagedList(page, pageSize);
        }

        public Order GetByID(int id)
        {
            var model = db.Orders.FirstOrDefault(x => x.ID == id);
            return model;
        }

        public bool UpdateStatus(Order order)
        {
            try
            {
                db.Orders.Attach(order);
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Insert(Order order)
        {
            try
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<OrderDetails> getOrderDetails(int orderID)
        {
            return db.OrderDetailses.Where(x => x.OrderID == orderID).ToList();
        }

        public double getTotal(int orderID)
        {
            return db.OrderDetailses.Where(x => x.OrderID == orderID).Sum(x => x.Price*x.Quantity);
        }

        public bool Delete(int ID)
        {
            try
            {
                var orderdetails = db.OrderDetailses.Where(x => x.OrderID == ID).ToList();
                foreach (var details in orderdetails)
                {
                    db.OrderDetailses.Remove(details);
                }
                var order = db.Orders.Find(ID);
                db.Orders.Remove(order);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
