using AspNetCoreHero.ToastNotification.Abstractions;
using CoffeeAutomation.Context;
using CoffeeAutomation.Entities;
using CoffeeAutomation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace CoffeeAutomation.Controllers
{
    public class CategoryController : Controller
    {
        private readonly CoffeeDbContext _context;
        private readonly INotyfService _notyfService;

        public CategoryController(CoffeeDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> GetCategories(int page=1)
        {
            var categoryList = await _context.Categories.OrderByDescending(x=>x.Id).ToList().ToPagedListAsync(page,5);

            if (categoryList.Count == 0)
            {
                _notyfService.Information("Herhangi bir kategori bulunmamaktadır.");

            }

            var categoryVm = new CategoryVm
            {
                Categories = categoryList,
            };

            return View(categoryVm);
        }
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var entity = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (entity is null)
            {
                return NotFound();
            }

            _context.Categories.Remove(entity);
            _context.SaveChanges();
            _notyfService.Success("Başarıyla silindi.");


            return RedirectToAction("GetCategories");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateCategory(CategoryVm categoryVm)
        {

            await _context.Categories.AddAsync(new Category
            {
                Name = categoryVm.Category.Name,
                Products = categoryVm.Category.Products
            });
            await _context.SaveChangesAsync();
            _notyfService.Success("Başarıyla eklendi.");

            return RedirectToAction("GetCategories");
        }
        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var entity = await _context.Categories.FindAsync(id);

            if (entity is null)
            {
                return NotFound();
            }

            var model = new CategoryUpdateVm
            {
                Id = entity.Id,
                Name = entity.Name,
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(CategoryUpdateVm categoryVm, int id)
        {
            var entity = await _context.Categories.FindAsync(id);

            entity.Id = categoryVm.Id;
            entity.Name = categoryVm.Name;

            _context.Categories.Update(entity);
            await _context.SaveChangesAsync();
            _notyfService.Success("Başarıyla güncellendi.");


            return RedirectToAction("GetCategories");
        }
    }
}
