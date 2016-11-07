using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Data.Models;
using PagedList;

namespace Data.DAO
{
    public class CategoriesDao
    {
        ELEXStoreModels db = null;
        ProductsDao productsDao = new ProductsDao();

        public CategoriesDao()
        {
            db = new ELEXStoreModels();
            ;
        }

        public IPagedList<Category> ListAllPaging(int page, int pageSize)
        {
            return db.Categories.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }

        public List<Category> ListAll()
        {
            return db.Categories.ToList();
        }

        public Category GetById(int id)
        {
            var model = db.Categories.FirstOrDefault(x => x.ID == id);
            return model;
        }

        public bool Insert(Category entity)
        {
            try
            {
                if (String.IsNullOrEmpty(entity.Image))
                {
                    entity.Image = "noimage.png";
                }
                if (String.IsNullOrEmpty(entity.Icon))
                {
                    entity.Icon = "noimage.png";
                }
                db.Categories.Add(entity);
                db.SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Category entity)
        {
            try
            {
                //var category = db.Categories.Single(p=>p.ID==entity.ID);
                //category.Name = entity.Name;
                //category.Image = entity.Image;
                //category.Icon = entity.Icon;
                db.Categories.Attach(entity);
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var products = db.Products.Where(x => x.CategoryID == id).ToList();
                foreach (var product in products)
                {
                    DeleteProduct(product.ID);
                }
                var category = db.Categories.Find(id);
                db.Categories.Remove(category);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
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

