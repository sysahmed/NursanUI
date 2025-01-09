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
    public class OrHarnessModelsController : Controller
    {
        private readonly UretimContext _context;

        public OrHarnessModelsController(UretimContext context)
        {
            _context = context;
        }

        // GET: OrHarnessModels
        public async Task<IActionResult> Index()
        {
              return View(await _context.OrHarnessModels.ToListAsync());
        }

        // GET: OrHarnessModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.OrHarnessModels == null)
            {
                return NotFound();
            }

            var orHarnessModel = await _context.OrHarnessModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orHarnessModel == null)
            {
                return NotFound();
            }
            return View(orHarnessModel);
        }
        // GET: OrHarnessModels/Create
        public IActionResult Create()
        {
            ViewData["AlertId"] = new SelectList(_context.OrAlerts, "id", "Name");
            ViewData["Family"] = new SelectList(_context.OrFamilies, "Id", "FamilyName");
         return View();
        }

        // POST: OrHarnessModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Prefix,Family,Suffix,HarnessModelName,AlertId,Release,Access,Active,SideCode,CreateDate,UpdateDate")] OrHarnessModel orHarnessModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(orHarnessModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(orHarnessModel);
        }

        // GET: OrHarnessModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.OrHarnessModels == null)
            {
                return NotFound();
            }

            var orHarnessModel = await _context.OrHarnessModels.FindAsync(id);
            if (orHarnessModel == null)
            {
                return NotFound();
            }
            return View(orHarnessModel);
        }

        // POST: OrHarnessModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Prefix,Family,Suffix,HarnessModelName,AlertId,Release,Access,Active,SideCode,CreateDate,UpdateDate")] OrHarnessModel orHarnessModel)
        {
            if (id != orHarnessModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(orHarnessModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrHarnessModelExists(orHarnessModel.Id))
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
            return View(orHarnessModel);
        }

        // GET: OrHarnessModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.OrHarnessModels == null)
            {
                return NotFound();
            }

            var orHarnessModel = await _context.OrHarnessModels
                .FirstOrDefaultAsync(m => m.Id == id);
            if (orHarnessModel == null)
            {
                return NotFound();
            }

            return View(orHarnessModel);
        }

        // POST: OrHarnessModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.OrHarnessModels == null)
            {
                return Problem("Entity set 'UretimContext.OrHarnessModels'  is null.");
            }
            var orHarnessModel = await _context.OrHarnessModels.FindAsync(id);
            if (orHarnessModel != null)
            {
                _context.OrHarnessModels.Remove(orHarnessModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrHarnessModelExists(int id)
        {
          return _context.OrHarnessModels.Any(e => e.Id == id);
        }
    }
}
