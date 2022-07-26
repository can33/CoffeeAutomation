using AspNetCoreHero.ToastNotification.Abstractions;
using CoffeeAutomation.Context;
using CoffeeAutomation.Entities;
using CoffeeAutomation.Models.OrderVMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace CoffeeAutomation.Controllers
{
    public class OrderController : Controller
    {
        private readonly CoffeeDbContext _context;
        private readonly INotyfService _notyfService;

        public OrderController(CoffeeDbContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        public async Task<IActionResult> GetOrders(int page=1)
        {
            var orders = await _context.Orders
                .Include(x=>x.Product)
                .Include(x=>x.Table)
                .OrderByDescending(x=>x.CreatedDate)
                .ToList()
                .ToPagedListAsync(page,5);

            if(orders.Count == 0)
            {
                _notyfService.Information("Herhangi bir sipariş bulunmamaktadır.");
            }

            List<SelectListItem> products = (from x in await
                                                        _context.Products.ToListAsync()
                                               select new SelectListItem
                                               {
                                                   Text = x.Name,
                                                   Value = x.Id.ToString()
                                               }).ToList();
            ViewBag.productList = products;

            List<SelectListItem> tables = (from x in await
                                                        _context.Tables.ToListAsync()
                                             select new SelectListItem
                                             {
                                                 Text = x.Name,
                                                 Value = x.Id.ToString()
                                             }).ToList();
            ViewBag.tableList = tables;

            var model = new OrderVm
            {
                Orders = orders
            };

            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder(OrderVm model)
        {
             await _context.Orders.AddAsync(new Order
            {
                ProductId = model.Order.ProductId,
                Amount = model.Order.Amount,
                CreatedDate = model.Order.CreatedDate,
                TableId = model.Order.TableId,
            });
            await _context.SaveChangesAsync();
            _notyfService.Success("Başarıyla sipariş oluşturuldu.");
            return RedirectToAction("GetOrders");
        }

    }
}
