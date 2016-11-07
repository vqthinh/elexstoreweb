using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Data.Models;
using PagedList;

namespace Data.DAO
{
   public class ProductsDao
    {
       ELEXStoreModels db = null;

       public ProductsDao()
       {
           db = new ELEXStoreModels();
          
       }

       public int countProduct(int id)
       {
           return db.Products.Find(id).Quantity;
       }
       public IPagedList<Product> ListAllPaging(int pageSize, int page)
       {
           return db.Products.OrderBy(x => x.ID).ToPagedList(page, pageSize);
       }

       public List<Product> ListSpecial()
       {
           return db.Products.OrderByDescending(x => x.ProductDate).ToList();
       }
       public List<Product> ListTopSells()
       {
           return db.Products.OrderByDescending(x => x.Sells).ToList();
       }
       public List<Product> ListNews()
       {
           return db.Products.OrderByDescending(x => x.ProductDate).ToList();
       }
       public List<Product> ListTopViews()
       {
           return db.Products.OrderByDescending(x => x.Views).ToList();
       }

       public List<Product> ListRandomProducts()
       {
           Random rand = new Random();
           int toSkip = rand.Next(1, db.Products.Count());
           return db.Products.OrderBy(x=>Guid.NewGuid()).Take(4).ToList();
       }

       public void IncreaseViews(int id)
       {
           var product = db.Products.Find(id);
           product.Views += 1;
           db.SaveChanges();

       }
       public List<Product> ListRelatedProducts(int CategoryID)
       {
           var list = db.Products.Where(x => x.CategoryID == CategoryID).ToList();
           return list;
       }

       public IPagedList<Product> ListProductsByCategory(int CategoryID, int pageSize, int page)
       {
           var list = db.Products.OrderBy(x=>x.ID).Where(x => x.CategoryID == CategoryID).ToPagedList(page,pageSize);
           return list;
       } 
       public Product GetByID(int id)
       {
           var model = db.Products.FirstOrDefault(x => x.ID == id);
           return model;
       }

       public IEnumerable<Product> SearchByName(string search = "")
       {
           db.Configuration.ProxyCreationEnabled = false;
           return String.IsNullOrEmpty(search) ? db.Products.ToList() : 
            db.Products.Where(p => p.Name.Contains(search)).ToList();
       } 

       public bool Insert(Product entity)
       {
           try
           {
               if (String.IsNullOrEmpty(entity.Image))
               {
                   entity.Image = "noimage.png";
               }
               db.Products.Add(entity);
               db.SaveChanges();
               return true;
           }
           catch
           {
               return false;
           }
       }

       public bool Update(Product entity)
       {
           try
           {
               var product = db.Products.Find(entity.ID);
               product.Name = entity.Name;
               product.Description = entity.Description;
               product.Image = entity.Image;
               product.Price = entity.Price;
               product.CategoryID = entity.CategoryID;
               product.Discount = entity.Discount;
               product.ProductDate = entity.ProductDate;
               product.SupplierID = entity.SupplierID;
               product.Quantity = entity.Quantity;
               product.Special = entity.Special;
               product.Views = entity.Views;
               //db.Products.Attach(entity);
               //db.Entry(entity).State = EntityState.Modified;
               db.SaveChanges();
               return true;
           }
           catch
           {
               return false;
           }
       }


       public bool Delete(int id)
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
