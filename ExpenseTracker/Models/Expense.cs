using System;
using System.ComponentModel.DataAnnotations;

namespace ExpenseTracker.Models
{
    public class Expense
    {
        public int ExpenseID { get; set; }
        public int ExpenseCategorieID { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ExpenseDate { get; set; }
        public decimal Amount { get; set; }
        public ExpenseCategorie ExpenseCategorie { get; set; }
    }
}
