using System.Linq;
using Data.Models;

namespace Data.DAO
{
    public class UsersDao
    {
        ELEXStoreModels db = null;

        public UsersDao()
        {
            db = new ELEXStoreModels();
        }

        public string Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.UserName;
        }

        public User GetById(string userName)
        {
            return db.Users.FirstOrDefault(x => x.UserName == userName);
        }
        public int Login(string userName, string passWord)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if (result==null)
            {
                return 1;
            }
            else
            {
                if (result.Password == passWord)
                {
                    return 0;
                }
                else
                {
                    return 2;
                }
            }
        }
    }
}
