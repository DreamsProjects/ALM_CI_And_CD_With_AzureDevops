using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALM.Models;
using ALM.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ALM.Controllers
{
    public class TransactionsController : Controller
    {
        private readonly BankRepository _repo;
        List<Customer> customerList = new List<Customer>();

        public TransactionsController(BankRepository repo)
        {
            _repo = repo;
            customerList = _repo.Customers;
        }

        [HttpGet]
        public IActionResult Deposit(int value)
        {
            if (value == 2)
            {
                ViewBag.ErrorMessage = TempData["error"].ToString();
            }

            return View(customerList);
        }

        [HttpPost]
        public IActionResult Deposit(int number, string sum) //number, sum
        {
            var returnedString = _repo.Deposit(number, sum);

            if (returnedString == "OK")
            {
                return RedirectToAction("Verify", new { number = number });
            }

            else if (returnedString == "NO EXISTING")
            {
                TempData["error"] = "Kontot existerar inte";
                return RedirectToAction("Deposit", new { value = 2 });
            }

            else TempData["error"] = "Felaktigt inmatning"; return RedirectToAction("Deposit", new { value = 2 });
        }

        [HttpPost]
        public IActionResult Withdraw(int number, string sum)
        {
            var returnedString = _repo.Withdraw(number, sum);

            if (returnedString == "OK")
            {
                return RedirectToAction("Verify", new { number = number });
            }

            else if (returnedString == "ERROR")
            {
                TempData["error"] = "Du kan inte ta ut mer pengar än vad du har";
                return RedirectToAction("Deposit", new { value = 2 });
            }

            else if (returnedString == "NO EXISTING")
            {
                TempData["error"] = "Kontot finns inte";
                return RedirectToAction("Deposit", new { value = 2 });
            }

            else TempData["error"] = "Felaktigt inmatning"; return RedirectToAction("Deposit", new { value = 2 });
        }

        public IActionResult Verify(int number)
        {
            var getValue = _repo.Accounts.Where(x => x.AccountId == number).FirstOrDefault();

            var person = _repo.Customers.Where(x => x.PersonId == getValue.PersonId).FirstOrDefault();

            getValue.Name = person.Name;
            getValue.LastName = person.LastName;

            return View(getValue);
        }
    }
}