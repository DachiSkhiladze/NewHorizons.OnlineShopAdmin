using Microsoft.AspNetCore.Mvc;
using OnlineShop.DataAccess.Models;
using OnlineShop.DataAccess.Repository.Abstractions;

namespace OnlineShopAdmin.Controllers
{
    public class ReportsController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<SalesOrderHeader> _salesOrderRepository;
        public ReportsController(ILogger<HomeController> logger)
        {
            _logger = logger;
            //_salesOrderRepository = salesOrderRepository;
            //_salesOrderDetail = salesOrderDetail;
        }

        public IActionResult Index()
        {
           // var CustomersBySalesAmount = prod.GroupBy(x => x)
             //             .OrderByDescending(x => x.Count())
               //           .First().Key;

            return View();
        }
    }
}
