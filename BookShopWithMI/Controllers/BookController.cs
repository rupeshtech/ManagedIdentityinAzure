using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BookShopWithMI.Models;
using BookShopWithMI.Data;

namespace BookShopWithMI.Controllers
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
            List<Book> books = _context.Books.ToList();
            return View(books);
        }

        public IActionResult MyDetail()
        {
            Customer customer = new Customer();
            return View(customer);
        }

        [HttpGet]
        public IActionResult Buy(int bookId)
        {
            var book = _context.Books.FirstOrDefault(b=>b.BookId==bookId);
            var order = new Order { Book = book };
            return View(order);
        }

        [HttpPost]
        public IActionResult Buy(Order order)
        {
            _context.Order.Add(order);
            _context.SaveChanges();
            return RedirectToAction("Success", "Your Order is successfull");
        }
        public IActionResult Orders()
        {
            List<Order> orders = new List<Order>();
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
