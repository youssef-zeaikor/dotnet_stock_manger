using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Inventory_M.Data;
using Inventory_M.Models.tradespeople;

namespace Inventory_M.Controllers
{
    public class TradespeopleController : Microsoft.AspNetCore.Mvc.Controller
    {
        private readonly InventoryDbContext _context;

        public TradespeopleController(InventoryDbContext context)
        {
            _context = context;
        }

        // GET: Tradespeople
        public async Task<IActionResult> Index()
        {
              return _context.Tradespeoples != null ? 
                          View(await _context.Tradespeoples.ToListAsync()) :
                          Problem("Entity set 'InventoryDbContext.Tradespeoples'  is null.");
        }

        // GET: Tradespeople/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tradespeoples == null)
            {
                return NotFound();
            }

            var tradespeople = await _context.Tradespeoples
                .FirstOrDefaultAsync(m => m.TradesId == id);
            if (tradespeople == null)
            {
                return NotFound();
            }

            return View(tradespeople);
        }

        // GET: Tradespeople/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tradespeople/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TradesId,FullName,Email,Tele,ADRESS,matricule")] Tradespeople tradespeople)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tradespeople);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tradespeople);
        }

        // GET: Tradespeople/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tradespeoples == null)
            {
                return NotFound();
            }

            var tradespeople = await _context.Tradespeoples.FindAsync(id);
            if (tradespeople == null)
            {
                return NotFound();
            }
            return View(tradespeople);
        }

        // POST: Tradespeople/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TradesId,FullName,Email,Tele,ADRESS,matricule")] Tradespeople tradespeople)
        {
            if (id != tradespeople.TradesId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tradespeople);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TradespeopleExists(tradespeople.TradesId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tradespeople);
        }

        // GET: Tradespeople/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tradespeoples == null)
            {
                return NotFound();
            }

            var tradespeople = await _context.Tradespeoples
                .FirstOrDefaultAsync(m => m.TradesId == id);
            if (tradespeople == null)
            {
                return NotFound();
            }

            return View(tradespeople);
        }

        // POST: Tradespeople/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tradespeoples == null)
            {
                return Problem("Entity set 'InventoryDbContext.Tradespeoples'  is null.");
            }
            var tradespeople = await _context.Tradespeoples.FindAsync(id);
            if (tradespeople != null)
            {
                _context.Tradespeoples.Remove(tradespeople);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TradespeopleExists(int id)
        {
          return (_context.Tradespeoples?.Any(e => e.TradesId == id)).GetValueOrDefault();
        }
    }
}
