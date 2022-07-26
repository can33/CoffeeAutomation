using AspNetCoreHero.ToastNotification.Abstractions;
using CoffeeAutomation.Context;
using CoffeeAutomation.Entities;
using CoffeeAutomation.Models;
using CoffeeAutomation.Models.ProductVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using X.PagedList;


namespace CoffeeAutomation.Controllers
{
    public class ProductController : Controller
    {
        private readonly CoffeeDbContext _context;
        private readonly INotyfService _notyfService;
        public ProductController(CoffeeDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> GetProducts(int page=1)
        {
            var entity = await _context.Products.Include(x => x.Category).ToList().ToPagedListAsync(page, 5); ;
            if(entity.Count == 0)
            {
                return NotFound();
            }

            var productVm = new ProductVm
            {
                Products = entity
            };
            

            List<SelectListItem> categories = (from x in await
                                                        _context.Categories.ToListAsync()
                                               select new SelectListItem
                                               {
                                                   Text = x.Name,
                                                   Value = x.Id.ToString()
                                               }).ToList();
            ViewBag.categoriesList = categories;



            return View(productVm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(ProductVm productVm)
        {
            var product = new Product
            {
                Name = productVm.Product.Name,
                Price = productVm.Product.Price,
                Stock = productVm.Product.Stock,
                CategoryId = productVm.Product.CategoryId,
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();
            _notyfService.Success("Başarıyla eklendi.");

            return RedirectToAction("GetProducts");
        }
        public IActionResult DeleteProduct(int id)
        {
            var entity = _context.Products.Find(id);

            if (entity is null)
            {
                return NotFound();
            }

            _context.Products.Remove(entity);
            _context.SaveChanges();
            _notyfService.Success("Başarıyla silindi.");


            return RedirectToAction("GetProducts");
        }
        [HttpGet]
        public async Task<IActionResult> EditProduct(int id)
        {
            var product = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);

            if (product is null)
            {
                return NotFound();
            }

            List<SelectListItem> categories = (from x in await
                                                        _context.Categories.ToListAsync()
                                               select new SelectListItem
                                               {
                                                   Text = x.Name,
                                                   Value = x.Id.ToString()
                                               }).ToList();
            
            ViewBag.categoriesList = categories;

            var model = new ProductUpdateVm
            {
                Id = id,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Name = product.Name,
                Stock = product.Stock,
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(ProductUpdateVm productVm, int id)
        {

            var entity = await _context.Products.SingleOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                _notyfService.Error("Böyle bir ürün yok!");
            }

            entity.Name = productVm.Name;
            entity.Price = productVm.Price;
            entity.Stock = productVm.Stock;
            entity.CategoryId = productVm.CategoryId;

            _context.Products.Update(entity);
            await _context.SaveChangesAsync();
            _notyfService.Success("Başarıyla güncellendi.");


            return RedirectToAction("GetProducts");
        }
    }
}
