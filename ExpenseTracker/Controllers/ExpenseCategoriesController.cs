using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers
{
    public class ExpenseCategoriesController : Controller
    {
        private readonly MyContext _context;

        public ExpenseCategoriesController(MyContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.ExpensesCategories.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseCategorie = await _context.ExpensesCategories
                .FirstOrDefaultAsync(m => m.ExpenseCategorieID == id);
            if (expenseCategorie == null)
            {
                return NotFound();
            }

            return View(expenseCategorie);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpenseCategorieID,Name")] ExpenseCategorie expenseCategorie)
        {
            string verifiedCategory = _context.ExpensesCategories.Where(v => v.Name == expenseCategorie.Name).Select(v => v.Name).FirstOrDefault();
            if (string.IsNullOrWhiteSpace(verifiedCategory))
            {
                if (ModelState.IsValid)
                {
                    _context.Add(expenseCategorie);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            else
            {
                ModelState.AddModelError("Name", "This expense categorie is already exist");
                return View(expenseCategorie);
            }
            return View(expenseCategorie);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseCategorie = await _context.ExpensesCategories.FindAsync(id);
            if (expenseCategorie == null)
            {
                return NotFound();
            }
            return View(expenseCategorie);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpenseCategorieID,Name")] ExpenseCategorie expenseCategorie)
        {
            if (id != expenseCategorie.ExpenseCategorieID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(expenseCategorie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExpenseCategorieExists(expenseCategorie.ExpenseCategorieID))
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
            return View(expenseCategorie);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expenseCategorie = await _context.ExpensesCategories
                .FirstOrDefaultAsync(m => m.ExpenseCategorieID == id);
            if (expenseCategorie == null)
            {
                return NotFound();
            }

            return View(expenseCategorie);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expenseCategorie = await _context.ExpensesCategories.FindAsync(id);
            _context.ExpensesCategories.Remove(expenseCategorie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseCategorieExists(int id)
        {
            return _context.ExpensesCategories.Any(e => e.ExpenseCategorieID == id);
        }
    }
}
