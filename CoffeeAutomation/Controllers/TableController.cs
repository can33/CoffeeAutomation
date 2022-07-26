using AspNetCoreHero.ToastNotification.Abstractions;
using CoffeeAutomation.Context;
using CoffeeAutomation.Entities;
using CoffeeAutomation.Models.OrderVMs;
using CoffeeAutomation.Models.TableVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeAutomation.Controllers
{
    public class TableController : Controller
    {
        private readonly CoffeeDbContext _context;
        private readonly INotyfService _notyfService;

        public TableController(CoffeeDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }
        public async Task<IActionResult> GetTables()
        {
            var tables = await _context.Tables.ToListAsync();

            var tableModel = new TableVm
            {
                Tables = tables,
            };

            if (tables.Count == 0)
            {
                _notyfService.Error("Sistemde mevcut bir masa bulunmamaktadır.");
            }

            return View(tableModel);
        }
        [HttpGet]
        public async Task<IActionResult> GetOrderByTable(int id)
        {
            var entity = await _context.Orders
                .Include(x => x.Product)
                .Include(x => x.Table)
                .Where(x => x.TableId == id).ToListAsync();

            ViewBag.TableName = await _context.Tables
                .Include(x => x.Orders)
                .Where(x => x.Id == id)
                .Select(x => x.Name)
                .FirstOrDefaultAsync();

            if (entity.Count == 0)
            {
                _notyfService.Information($"Mevcut bir siparişi bulunmamaktadır.");
            }

            //bool isThereOrder = await _context.Orders.AnyAsync(x => x.Id == id);

            //if (isThereOrder == false )
            //{
            //    return NotFound();
            //}
            var orderVm = new OrderCreateVm
            {
                Orders = entity,
            };

            

            return View(orderVm);
        }
        //[HttpPost]
        //public async Task<IActionResult> RemoveAllOrder()
        //{
           
        //    _context.Orders.RemoveRange(TempData["idd"] as ICollection<Order>);
        //    await _context.SaveChangesAsync();
        //    return RedirectToAction("GetTables");
        //}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateTable(TableVm tableVm)
        {

            await _context.Tables.AddAsync(new Table
            {
                Name = tableVm.Table.Name,
            });
            await _context.SaveChangesAsync();
            _notyfService.Success("Başarılı bir şekilde eklendi.");

            return RedirectToAction("GetTables");
        }
    }
}
