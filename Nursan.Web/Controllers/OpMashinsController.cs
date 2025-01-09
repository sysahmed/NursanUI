using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Nursan.Domain.Entity;

namespace Nursan.Web.Controllers
{
    public class OpMashinsController : Controller
    {
        private readonly UretimContext _context;

        public OpMashinsController(UretimContext context)
        {
            _context = context;
        }

        // GET: OpMashins
        public async Task<IActionResult> Index()
        {
            var uretimContext = _context.OpMashins.Include(o => o.ModulerYapi);
            return View(await uretimContext.ToListAsync());
        }

        // GET: OpMashins/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OpMashins == null)
            {
                return NotFound();
            }

            var opMashin = await _context.OpMashins
                .Include(o => o.ModulerYapi)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (opMashin == null)
            {
                return NotFound();
            }

            return View(opMashin);
        }

        // GET: OpMashins/Create
        public IActionResult Create()
        {
            ViewData["ModulerYapiId"] = new SelectList(_context.UrModulerYapis, "Id", "Etap");
            return View();
        }

        // POST: OpMashins/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MasineName,ModulerYapiId,CreateDate,UpdateDate")] OpMashin opMashin)
        {
            if (ModelState.IsValid)
            {
                _context.Add(opMashin);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ModulerYapiId"] = new SelectList(_context.UrModulerYapis, "Id", "Etap", opMashin.ModulerYapiId);
            return View(opMashin);
        }

        // GET: OpMashins/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OpMashins == null)
            {
                return NotFound();
            }

            var opMashin = await _context.OpMashins.FindAsync(id);
            if (opMashin == null)
            {
                return NotFound();
            }
            ViewData["ModulerYapiId"] = new SelectList(_context.UrModulerYapis, "Id", "Etap", opMashin.ModulerYapiId);
            return View(opMashin);
        }

        // POST: OpMashins/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MasineName,ModulerYapiId,CreateDate,UpdateDate")] OpMashin opMashin)
        {
            if (id != opMashin.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(opMashin);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OpMashinExists(opMashin.Id))
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
            ViewData["ModulerYapiId"] = new SelectList(_context.UrModulerYapis, "Id", "Etap", opMashin.ModulerYapiId);
            return View(opMashin);
        }

        // GET: OpMashins/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OpMashins == null)
            {
                return NotFound();
            }

            var opMashin = await _context.OpMashins
                .Include(o => o.ModulerYapi)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (opMashin == null)
            {
                return NotFound();
            }

            return View(opMashin);
        }

        // POST: OpMashins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OpMashins == null)
            {
                return Problem("Entity set 'UretimContext.OpMashins'  is null.");
            }
            var opMashin = await _context.OpMashins.FindAsync(id);
            if (opMashin != null)
            {
                _context.OpMashins.Remove(opMashin);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OpMashinExists(int id)
        {
          return _context.OpMashins.Any(e => e.Id == id);
        }
    }
}
