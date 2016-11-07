using System.Linq;
using Data.Models;

namespace Data.DAO
{
    public class StatisticDao
    {
        ELEXStoreModels db = new ELEXStoreModels();
        public Statistic Statistic()
        {
            var st = new Statistic
            {
                Views = db.Products.Sum(x => x.Views),
                Orders = db.Orders.Count(x=>x.Status==0),
                Solds = db.Products.Sum(x => x.Sells),
                Profit = db.Products.Sum(x => (x.Price - x.Price*x.Discount/100)*x.Sells)
            };
            return st;
        }
    }

    public class Statistic
    {
        public int Views { get; set; }

        public int Orders { get; set; }

        public int? Solds { get; set; }

        public double? Profit { get; set; }
    }
}
