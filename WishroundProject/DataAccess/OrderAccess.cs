using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WishroundProject.DataAccess
{
    public class OrderAccess
    {
        private WishDBDataContext dataContext;
        public OrderAccess()
        {
            dataContext = new WishDBDataContext();
        }

        public string GetStatusByOrderId(Guid id)
        {
            string status = null;
            string orderId = id.ToString();
            var order = (from dbOrder in dataContext.Orders
                        where dbOrder.PublicId.Equals(orderId)
                        select dbOrder).FirstOrDefault();
            if (order != null)
            {
                status = order.Status;
            }
            return status;
        }

        public bool SetStatusByOrderId(Guid id, string status)
        {
            bool isSuccess = false;

            string orderId = id.ToString();
            var order = (from dbOrder in dataContext.Orders
                         where dbOrder.PublicId.Equals(orderId)
                         select dbOrder).FirstOrDefault();
            if (order != null)
            {
                try
                {
                    order.Status = status;
                    dataContext.SubmitChanges();
                    isSuccess = true;
                }
                catch { }
            }
            return isSuccess;
        }

        public Order CreateForWish(Guid wishId)
        {
            Order newOrder = null;
            Wish wish = WishAccess.GetWishByPublishId(wishId);
            try
            {
                newOrder = new Order { Status = "InProgress", WishId = wish.Id, PublicId = Guid.NewGuid().ToString() };
                dataContext.Orders.InsertOnSubmit(newOrder);
                dataContext.SubmitChanges();
            }
            catch { }

            return newOrder;
        }
    }
}