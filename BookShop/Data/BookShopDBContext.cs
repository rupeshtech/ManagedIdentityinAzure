using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace BookShop.Data
{
    public class BookShopDBContext : DbContext
    {
        public BookShopDBContext(DbContextOptions<BookShopDBContext> options): base(options)
        {
        }
        
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Order> Order { get; set; }
        
    }

    public class Book
    {
        public int BookId { get; set; }
        public string Name { get; set; }

        public int Price { get; set; }

        public Author Author { get; set; }
    }

    public class Author
    {
        public int AuthorId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public virtual List<Book> Posts { get; set; }
    }


    public class Customer
    {
        public int CustomerId { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string AddressName { get; set; }

        public string CreditCardNumber { get; set; }
    }

    public class Order
    {
        public int OrderId { get; set; }
        public Book Book { get; set; }

        public Customer Customer { get; set; }

        public DateTime OrderDate { get; set; }

        public string ShippingAddress { get; set; }
    }
}
