using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.Unity;
namespace TheShoeWebsite.Models
{
    public interface IBanGiayRepository
    {
        //IQueryable<ICollection> GetProduct();
        IQueryable<Category> GetCategory();
        IQueryable<Product> GetProductBySubCat(string SubCatId);
        Account FindAccountByID(int AccountID);
        Account FindAccountByLoginName(string loginName);
        void AddAccount(Account Account);
        //IQueryable<Product> FindProductById(int id);
        //IQueryable<Category> GetAllCategory();
        //void AddProduct(Product product);

        //void AddCategory(Category category);
        //void AddSubCat(SubCategory subcat);
        //void CommitChanges();
    }

    public class BanGiayRepository : IBanGiayRepository
    {
        public BanGiayEntities db = new BanGiayEntities();

        //public IQueryable<ICollection> GetProduct()
        //{
        //    var results=from p in db.Products.Include("Category")
        //                join s in db.SubCategories on
        //                p.SubCategoryId equals s.SubCatId  into groupC
        //                select groupC;
        //    results.ToList();
        //    return results.AsQueryable();
        public IQueryable<Category> GetCategory()
        {
            return db.Categories;
        }
        public IQueryable<Product> GetProductBySubCat(string SubCat)
        {
            return db.Products.Where(p => p.SubCategoryId == SubCat);
        }


        #region Account
        public Account ValidateAccount(string loginName, string password)
        {
            return db.Accounts.FirstOrDefault(u => u.LoginName == loginName && u.Password == password);
        }

        public Account FindAccountByID(int AccountID)
        {
            return db.Accounts.FirstOrDefault(u => u.AccountId == AccountID);
        }

        public Account FindAccountByLoginName(string loginName)
        {
            return db.Accounts.FirstOrDefault(u => u.LoginName == loginName);
        }

        public void AddAccount(Account Account)
        {
            db.AddToAccounts(Account);
        }
    }
}
        #endregion
