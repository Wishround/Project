using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WishroundProject.DataAccess
{
    public class WishAccess
    {
        private WishDBDataContext dataContext;
        private string userId;

        public WishAccess(Guid userId)
        {
            dataContext = new WishDBDataContext();
            this.userId = userId.ToString();
        }

        public Wish InsertWish(string name, string code, float cost, string currency, string imageUrl)
        {
            Wish wish = null;
            try
            {
                wish = new Wish
                {
                    Name = name,
                    Code = code,
                    Cost = cost,
                    Currency = currency,
                    ImageURL = imageUrl,
                    UserId = userId,
                    IsCompleted = false,
                    PublicId = Guid.NewGuid().ToString()
                };
                dataContext.Wishes.InsertOnSubmit(wish);
                dataContext.SubmitChanges();
            }
            catch(Exception e)  {
                //sss
            }
            return wish;
        }

        public static Wish GetWishByPublishId(Guid id)
        {
            Wish wish = null;
            WishDBDataContext dataContext = new WishDBDataContext();
            wish = (from dbWish in dataContext.Wishes
                    where dbWish.PublicId.Equals(id)
                     select dbWish).FirstOrDefault();

            return wish;
        }

        public IEnumerable<Wish> GetAllWishes()
        {
            IEnumerable<Wish> wishes = new List<Wish>();

            wishes = from dbWish in dataContext.Wishes
                     where dbWish.UserId.Equals(userId)
                     select dbWish;

            return wishes;
        }
    }
}