using eTickets.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTickets.Data.Cart
{
    public class ShoppingCart
    {
        public AppDbContext _context { get; set; }
        public string ShoppingCartId {get; set;}
        public List<ShoppingCartItem> ShoppingCartItems { get; set; }
        public ShoppingCart(AppDbContext context)
        {
            _context = context;
        }

        //method 
        public static ShoppingCart GetShoppingCart(IServiceProvider services)
        {
            ISession session = services.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;
            var context = services.GetService<AppDbContext>();

            //check if we have a CartId session otherwise we are going to generate a new one
            string cartId = session.GetString("CartId") ?? Guid.NewGuid().ToString();
            session.SetString("CartId", cartId);

            return new ShoppingCart(context) { ShoppingCartId = cartId };
        }

        //Method to add an item to the cart
        public void AddItemToCart(Movie movie)
        {
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault
                (n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem == null)
            {
                shoppingCartItem = new ShoppingCartItem()
                {
                    ShoppingCartId = ShoppingCartId,
                    Movie = movie,
                    Amount = 1
                };
                _context.ShoppingCartItems.Add(shoppingCartItem);
            }
            else
            {
                shoppingCartItem.Amount++;
            }
            _context.SaveChanges();
        }

        //method to remove an item from the cart
        public void RemoveItemFromCart(Movie movie)
        {
            //we check if we have this movie in the shoppingcart
            var shoppingCartItem = _context.ShoppingCartItems.FirstOrDefault
                (n => n.Movie.Id == movie.Id && n.ShoppingCartId == ShoppingCartId);

            if (shoppingCartItem != null)
            {
                if(shoppingCartItem.Amount > 1)
                {
                    shoppingCartItem.Amount--;
                }else
                {
                    _context.ShoppingCartItems.Remove(shoppingCartItem);
                }
            }
            _context.SaveChanges();
        }

        //method to get all shoppingcartitems
        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            return ShoppingCartItems ?? 
                (ShoppingCartItems = _context.ShoppingCartItems.Where(
                n => n.ShoppingCartId == ShoppingCartId).Include(n => n.Movie).ToList());
        }

        //method to get the shoppingcart total
        public double GetShoppingCartTotal() => _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId)
                .Select(n => n.Movie.Price * n.Amount).Sum();

        public async Task ClearShoppingCartAsync()
        {
            var items = await _context.ShoppingCartItems.Where(n => n.ShoppingCartId == ShoppingCartId).ToListAsync();
            _context.ShoppingCartItems.RemoveRange(items);
            await _context.SaveChangesAsync();
        }
    }
}
