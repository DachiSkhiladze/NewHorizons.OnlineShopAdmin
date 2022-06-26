using Microsoft.AspNetCore.Mvc;
using OnlineShop.DataAccess.Models;
using OnlineShop.DataAccess.Repository.Abstractions;
using OnlineShopAdmin.Models;
using System.Diagnostics;

namespace OnlineShopAdmin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository<ProductCategory> productCategoryRepository;
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductSubcategory> ProductSubCategory;

        public HomeController(ILogger<HomeController> logger, IRepository<ProductCategory> repository, IRepository<Product> productRepository, IRepository<ProductSubcategory> subCategory)
        {
            _logger = logger;
            productCategoryRepository = repository;
            ProductSubCategory = subCategory;
            _productRepository = productRepository;
        }

        public IActionResult Index()
        {
            var dict = new Dictionary<int, int>();
            var cats = productCategoryRepository.GetAll().ToList();
            foreach (var item in cats)
            {
                dict.Add(item.ProductCategoryId, 0);
                foreach (var subItems in ProductSubCategory.Get(o => o.ProductCategoryId == item.ProductCategoryId))
                {
                    var result = _productRepository.Get(o => o.ProductSubcategoryId == subItems.ProductSubcategoryId);
                    if (result != null)
                    {
                        dict[item.ProductCategoryId] += result.Count();
                    }
                }
            }
            ViewBag.ProductCategoryCount = dict;
            return View(cats);
        }

        public IActionResult CreateProductCategory()
        {
            return View(new ProductCategory());
        }

        public IActionResult EditProductCategory(int id = 1)
        {
            var bubu = productCategoryRepository.Get(o => o.ProductCategoryId == id).First();
            return View(bubu);
        }

        [HttpPost]
        public IActionResult EditProductCategory(ProductCategory productCategory)
        {
            var model = productCategoryRepository.Get(o => o.ProductCategoryId == productCategory.ProductCategoryId).First();
            model.Name = productCategory.Name;
            productCategoryRepository.UpdateAsync(model);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> CreateProductCategory(ProductCategory productCategory)
        {
            await productCategoryRepository.AddAsync(productCategory);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DeleteCategory(int id)
        {
            var model = productCategoryRepository.Get(o => o.ProductCategoryId == id).First();
            await productCategoryRepository.DeleteAsync(model);
            return RedirectToAction("Index");
        }
    }
}