using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ExpenseTracker.Controllers
{
    public class ExpensesController : Controller
    {
        private readonly MyContext _context;

        public ExpensesController(MyContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(DateTime start ,DateTime end)
        {
            if(start==DateTime.MinValue && end==DateTime.MinValue)
            {
                start = DateTime.Parse("2022-06-01");
                end = DateTime.Parse("2022-06-30");
            }
            ViewBag.StartDate = start.ToString("yyyy-MM-dd") ;
            ViewBag.EndDate = end.ToString("yyyy-MM-dd");
            var myContext = _context.Expenses.Include(e => e.ExpenseCategorie);
            return View(await myContext.Where(d => d.ExpenseDate>=start).Where(d => d.ExpenseDate<= end).ToListAsync());
        }

        public IActionResult Create()
        {
            ViewData["ExpenseCategorieID"] = new SelectList(_context.ExpensesCategories, "ExpenseCategorieID", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ExpenseID,ExpenseCategorieID,ExpenseDate,Amount")] Expense expense)
        {
            if (expense.ExpenseDate <= DateTime.Now)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(expense);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            else
            {
                ModelState.AddModelError("ExpenseDate", "You cannot record any expenditure with a future date");
                ViewData["ExpenseCategorieID"] = new SelectList(_context.ExpensesCategories, "ExpenseCategorieID", "Name", expense.ExpenseCategorieID);
                return View(expense);
            }
            return View(expense);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null)
            {
                return NotFound();
            }
            ViewData["ExpenseCategorieID"] = new SelectList(_context.ExpensesCategories, "ExpenseCategorieID", "Name", expense.ExpenseCategorieID);
            return View(expense);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ExpenseID,ExpenseCategorieID,ExpenseDate,Amount")] Expense expense)
        {
            if (id != expense.ExpenseID)
            {
                return NotFound();
            }
            if (expense.ExpenseDate <= DateTime.Now)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(expense);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ExpenseExists(expense.ExpenseID))
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
            }
            else
            {
                ModelState.AddModelError("ExpenseDate", "You cannot record any expenditure with a future date");
                ViewData["ExpenseCategorieID"] = new SelectList(_context.ExpensesCategories, "ExpenseCategorieID", "Name", expense.ExpenseCategorieID);
                return View(expense);
            }
            return View(expense);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var expense = await _context.Expenses
                .Include(e => e.ExpenseCategorie)
                .FirstOrDefaultAsync(m => m.ExpenseID == id);
            if (expense == null)
            {
                return NotFound();
            }

            return View(expense);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExpenseExists(int id)
        {
            return _context.Expenses.Any(e => e.ExpenseID == id);
        }
    }
}
