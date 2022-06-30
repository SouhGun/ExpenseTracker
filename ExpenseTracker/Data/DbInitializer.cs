using ExpenseTracker.Models;
using System;
using System.Linq;

namespace ExpenseTracker.Data
{
    public class DbInitializer
    {
        public static void Initialize(MyContext context)
        {
            context.Database.EnsureCreated();

            if (context.ExpensesCategories.Any())
            {
                return;
            }

            var categories = new ExpenseCategorie[]
            {
                new ExpenseCategorie{Name="House Rent"},
                new ExpenseCategorie{Name="Water Bill"},
                new ExpenseCategorie{Name="Electric Bill"},
                new ExpenseCategorie{Name="Groceries"},
                new ExpenseCategorie{Name="Uber"},
                new ExpenseCategorie{Name="Medications"}
            };
            foreach (ExpenseCategorie ex in categories)
            {
                context.ExpensesCategories.Add(ex);
            }
            context.SaveChanges();
            var expenses = new Expense[]
            {
                new Expense{Amount=150,ExpenseCategorieID=1,ExpenseDate=DateTime.Parse("2022-06-18")},
                new Expense{Amount=200,ExpenseCategorieID=6,ExpenseDate=DateTime.Parse("2022-06-15")},
                new Expense{Amount=500,ExpenseCategorieID=3,ExpenseDate=DateTime.Parse("2022-06-10")},
                new Expense{Amount=300,ExpenseCategorieID=1,ExpenseDate=DateTime.Parse("2022-06-01")},
                new Expense{Amount=200,ExpenseCategorieID=4,ExpenseDate=DateTime.Parse("2022-06-11")},
                new Expense{Amount=800,ExpenseCategorieID=2,ExpenseDate=DateTime.Parse("2022-05-01")}
            };

            
            foreach (Expense e in expenses)
            {
                context.Expenses.Add(e);
            }
            context.SaveChanges();
        }
    }
}