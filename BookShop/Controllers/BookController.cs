using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookShop.Models;
using BookShop.Data;

namespace BookShop.Controllers
{
    public class BookController : Controller
    {
        private readonly ILogger<BookController> _logger;
        private readonly BookShopDBContext _context;

        public BookController(ILogger<BookController> logger, BookShopDBContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            List<Book> books= _context.Books.ToList();
            return View(books);
        }

        public IActionResult MyDetail()
        {
            Customer customer = _context.Customer.FirstOrDefault(x=>x.FirstName=="Rupesh");
            return View(customer);
        }
        public IActionResult Orders()
        {
            List<Order> orders = _context.Order.Where(o=>o.Customer.FirstName=="Rupesh").ToList();
            return View(orders);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
