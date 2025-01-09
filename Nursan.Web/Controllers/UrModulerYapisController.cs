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
    public class UrModulerYapisController : Controller
    {
        private readonly UretimContext _context;

        public UrModulerYapisController(UretimContext context)
        {
            _context = context;
        }

        // GET: UrModulerYapis
        public async Task<IActionResult> Index()
        {
              return View(await _context.UrModulerYapis.ToListAsync());
        }

        // GET: UrModulerYapis/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UrModulerYapis == null)
            {
                return NotFound();
            }

            var urModulerYapi = await _context.UrModulerYapis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (urModulerYapi == null)
            {
                return NotFound();
            }

            return View(urModulerYapi);
        }

        // GET: UrModulerYapis/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: UrModulerYapis/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Etap,CreateDate,UpdateDate")] UrModulerYapi urModulerYapi)
        {
            if (ModelState.IsValid)
            {
                _context.Add(urModulerYapi);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(urModulerYapi);
        }

        // GET: UrModulerYapis/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UrModulerYapis == null)
            {
                return NotFound();
            }

            var urModulerYapi = await _context.UrModulerYapis.FindAsync(id);
            if (urModulerYapi == null)
            {
                return NotFound();
            }
            return View(urModulerYapi);
        }

        // POST: UrModulerYapis/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Etap,CreateDate,UpdateDate")] UrModulerYapi urModulerYapi)
        {
            if (id != urModulerYapi.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(urModulerYapi);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UrModulerYapiExists(urModulerYapi.Id))
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
            return View(urModulerYapi);
        }

        // GET: UrModulerYapis/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UrModulerYapis == null)
            {
                return NotFound();
            }

            var urModulerYapi = await _context.UrModulerYapis
                .FirstOrDefaultAsync(m => m.Id == id);
            if (urModulerYapi == null)
            {
                return NotFound();
            }

            return View(urModulerYapi);
        }

        // POST: UrModulerYapis/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UrModulerYapis == null)
            {
                return Problem("Entity set 'UretimContext.UrModulerYapis'  is null.");
            }
            var urModulerYapi = await _context.UrModulerYapis.FindAsync(id);
            if (urModulerYapi != null)
            {
                _context.UrModulerYapis.Remove(urModulerYapi);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UrModulerYapiExists(int id)
        {
          return _context.UrModulerYapis.Any(e => e.Id == id);
        }
    }
}
