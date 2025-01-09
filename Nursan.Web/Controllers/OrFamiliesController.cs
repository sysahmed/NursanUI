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
    public class OrFamiliesController : Controller
    {
        private readonly UretimContext _context;

        public OrFamiliesController(UretimContext context)
        {
            _context = context;
        }

        // GET: OrFamilies
        public async Task<IActionResult> Index()
        {
              return View(await _context.OrFamilies.ToListAsync());
        }

        // GET: OrFamilies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrFamilies == null)
            {
                return NotFound();
            }

            var orFamily = await _context.OrFamilies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orFamily == null)
            {
                return NotFound();
            }

            return View(orFamily);
        }

        // GET: OrFamilies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrFamilies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FamilyName,Id,CreateDate,UpdateDate")] OrFamily orFamily)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orFamily);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orFamily);
        }

        // GET: OrFamilies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrFamilies == null)
            {
                return NotFound();
            }

            var orFamily = await _context.OrFamilies.FindAsync(id);
            if (orFamily == null)
            {
                return NotFound();
            }
            return View(orFamily);
        }

        // POST: OrFamilies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FamilyName,Id,CreateDate,UpdateDate")] OrFamily orFamily)
        {
            if (id != orFamily.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orFamily);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrFamilyExists(orFamily.Id))
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
            return View(orFamily);
        }

        // GET: OrFamilies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrFamilies == null)
            {
                return NotFound();
            }

            var orFamily = await _context.OrFamilies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orFamily == null)
            {
                return NotFound();
            }

            return View(orFamily);
        }

        // POST: OrFamilies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrFamilies == null)
            {
                return Problem("Entity set 'UretimContext.OrFamilies'  is null.");
            }
            var orFamily = await _context.OrFamilies.FindAsync(id);
            if (orFamily != null)
            {
                _context.OrFamilies.Remove(orFamily);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrFamilyExists(int id)
        {
          return _context.OrFamilies.Any(e => e.Id == id);
        }
    }
}
