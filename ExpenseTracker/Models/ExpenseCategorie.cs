using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class ExpenseCategorie
    {
        public int ExpenseCategorieID { get; set; }
        public string Name { get; set; }
        public ICollection<Expense> Expenses { get; set; }

    }
}
