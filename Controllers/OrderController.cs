using Microsoft.AspNetCore.Mvc;
using OnlineShop.DataAccess.Models;
using OnlineShop.DataAccess.Repository.Abstractions;
using OnlineShopAdmin.Models;

namespace OnlineShopAdmin.Controllers
{
    public class OrderController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<SalesOrderHeader> _salesOrderRepository;
        private readonly IRepository<SalesOrderDetail> _salesOrderDetail;
        private readonly IRepository<Customer> _customersRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<Person> _personRepository;
        private readonly IRepository<SpecialOfferProduct> _specialOfferProductRepository;
        public OrderController(ILogger<HomeController> logger,
            IRepository<SalesOrderHeader> salesOrderRepository,
            IRepository<SalesOrderDetail> salesOrderDetail,
            IRepository<Product> productRepository,
            IRepository<Customer> customersRepository,
            IRepository<Person> personRepository,
            IRepository<SpecialOfferProduct> specialOfferProductRepository)
        {
            _logger = logger;
            _salesOrderRepository = salesOrderRepository;
            _salesOrderDetail = salesOrderDetail;
            _customersRepository = customersRepository;
            _productRepository = productRepository;
            _personRepository = personRepository;
            _specialOfferProductRepository = specialOfferProductRepository;
        }

        public IActionResult Index()
        {
            var orders = _salesOrderRepository.GetAll().Take(100).ToList();
            return View(orders);
        }

        public IActionResult CreateOrder()
        {
            ViewBag.Products = _productRepository.GetAll();
            ViewBag.Customers = _customersRepository.GetAll().Take(30);
            return View(new OrderViewModel());
        }

        public IActionResult EditOrder(int id = 1)
        {
            ViewBag.Products = _productRepository.GetAll();
            ViewBag.Customers = _customersRepository.GetAll().Take(30);
            var order = _salesOrderRepository.Get(o => o.SalesOrderId == id).First();
            var details = _salesOrderDetail.Get(o => o.SalesOrderId == id).First();
            return View(new OrderViewModel{CustomerID = order.CustomerId, ProductID = details.ProductId});
        }

        [HttpPost]
        public async Task<IActionResult> EditOrder(OrderViewModel model)
        {
            var order = _salesOrderRepository.Get(o => o.SalesOrderId == model.ID).First();
            var details = _salesOrderDetail.Get(o => o.SalesOrderId == model.ID).First();
            order.CustomerId = model.CustomerID;
            details.ProductId = model.ProductID;
            await _salesOrderRepository.UpdateAsync(order);
            await _salesOrderDetail.UpdateAsync(details);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(OrderViewModel model)
        {
            var sale = await _salesOrderRepository.AddAsync(new SalesOrderHeader { CustomerId = model.CustomerID, ModifiedDate = DateTime.Now, 
                OrderDate = DateTime.Now,
            DueDate = DateTime.Now,
            ShipDate = DateTime.Now, BillToAddressId = 10, ShipToAddressId = 10, ShipMethodId = 1});
            await _salesOrderDetail.AddAsync(new SalesOrderDetail { SpecialOfferId = _specialOfferProductRepository.Get(o => o.ProductId == model.ProductID).First().SpecialOfferId,OrderQty = 1, SalesOrderId = sale.SalesOrderId, ProductId = model.ProductID, ModifiedDate = DateTime.Now });
            return RedirectToAction("Index");
        }

        public IActionResult DeleteOrder(int id)
        {
            return View(new OrderViewModel { ID = id});
        }

        [HttpPost]
        public async Task<IActionResult> DeleteOrderConfirm(int id)
        {
            var details = _salesOrderDetail.Get(o => o.SalesOrderId == id);
            foreach (var item in details)
            {
                await _salesOrderDetail.DeleteAsync(item);
            }
            var header = _salesOrderRepository.Get(o => o.SalesOrderId == id).First();
            await _salesOrderRepository.DeleteAsync(header);
            return RedirectToAction("Index");
        }
    }
}
