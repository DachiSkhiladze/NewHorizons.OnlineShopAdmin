using Microsoft.AspNetCore.Mvc;
using OnlineShop.DataAccess.Models;
using OnlineShop.DataAccess.Repository.Abstractions;

namespace OnlineShopAdmin.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<ProductCategory> productCategoryRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductSubcategory> ProductSubCategory;
        private readonly IRepository<ProductModel> productModelRepository;

        public ProductController(ILogger<HomeController> logger, 
            IRepository<ProductCategory> repository, 
            IRepository<Product> productRepository, 
            IRepository<ProductSubcategory> subCategory,
            IRepository<ProductModel> productModel)
        {
            _logger = logger;
            productCategoryRepository = repository;
            ProductSubCategory = subCategory;
            _productRepository = productRepository;
            productModelRepository = productModel;
        }

        public IActionResult Index()
        {
            var prod = _productRepository.GetAll().ToList();
            return View(prod);
        }

        public IActionResult CreateProduct()
        {
            ViewBag.SubCategories = ProductSubCategory.GetAll();
            ViewBag.ProductModel = productModelRepository.GetAll();
            return View(new Product());
        }

        public IActionResult EditProduct(int id = 1)
        {
            ViewBag.SubCategories = ProductSubCategory.GetAll();
            ViewBag.ProductModel = productModelRepository.GetAll();
            var bubu = _productRepository.Get(o => o.ProductId == id).First();
            return View(bubu);
        }

        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            var model = _productRepository.Get(o => o.ProductId == product.ProductId).First();
            model.Name = product.Name;
            model.Size = product.Size;
            model.ListPrice = product.ListPrice;
            model.ProductSubcategoryId = product.ProductSubcategoryId;
            model.ProductModelId = product.ProductModelId;
            _productRepository.UpdateAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product productCategory)
        {
            productCategory.ModifiedDate = DateTime.Now;
            productCategory.SellStartDate = DateTime.Now;
            productCategory.ProductNumber = Guid.NewGuid().ToString().Substring(0,6);
            productCategory.SafetyStockLevel = 100;
            productCategory.ReorderPoint = 660;
            await _productRepository.AddAsync(productCategory);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var model = _productRepository.Get(o => o.ProductId == id).First();
            await _productRepository.DeleteAsync(model);
            return RedirectToAction("Index");
        }
    }
}
