using eTickets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Services
{
    public interface IOrdersService
    {
        //one method to add orders to the database
        Task StoreOrderAsync(List<ShoppingCartItem> items, string userId, string userEmailAddress);

        //to get all the items from the DB
        Task<List<Order>> GetOrdersByUserIdAndRoleAsync(string userId, string userRole);

    }
}
