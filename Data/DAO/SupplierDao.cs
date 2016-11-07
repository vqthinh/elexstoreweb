using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Data.Models;
using PagedList;

namespace Data.DAO
{
    public class SupplierDao
    {
        ELEXStoreModels db = null;
        ProductsDao productsDao = new ProductsDao();

        public SupplierDao()
        {
            db = new ELEXStoreModels();
        }

        public IPagedList<Supplier> ListAllPaging(int pageSize,int page)
        {
            return db.Suppliers.OrderBy(x=>x.ID).ToPagedList<Supplier>(page, pageSize);
        }

        public List<Supplier> ListAll()
        {
            return db.Suppliers.ToList();
        } 
        
        public Supplier GetByID(string id)
        {
            var model = db.Suppliers.FirstOrDefault(x => x.ID == id);
            return model;
        }

        public bool Insert(Supplier entity)
        {
            try
            {
                if (String.IsNullOrEmpty(entity.Logo))
                {
                    entity.Logo = "noimage.png";
                }
                db.Suppliers.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Supplier entity)
        {
            try
            {
                //var supplier = db.Suppliers.Single(x => x.ID == entity.ID);
                //supplier.Email = entity.Email;
                //supplier.Logo = entity.Logo;
                //supplier.Name = entity.Name;
                //supplier.Phone = entity.Phone;
                db.Suppliers.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(string id)
        {
            try
            {
                var products = db.Products.Where(x => x.SupplierID == id).ToList();
                foreach (var product in products)
                {
                    DeleteProduct(product.ID);
                }
                var supplier = db.Suppliers.Find(id);
                db.Suppliers.Remove(supplier);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteProduct(int id)
        {
            try
            {
                var orderdetails = db.OrderDetailses.Where(x => x.ProductID == id).ToList();
                foreach (var details in orderdetails)
                {
                    var order = db.Orders.Find(details.OrderID);
                    var details1 = details;
                    var orderdts = db.OrderDetailses.Where(x => x.OrderID == details1.OrderID);
                    foreach (var dt in orderdts)
                    {
                        db.OrderDetailses.Remove(dt);
                    }
                    db.Orders.Remove(order);
                }
                db.SaveChanges();
                var product = db.Products.Find(id);
                db.Products.Remove(product);
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
