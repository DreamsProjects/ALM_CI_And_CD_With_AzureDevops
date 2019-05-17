using System;
using System.Collections.Generic;
using System.Linq;
using ALM.Models;
using ALM.Repository;
using ALM.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ALM.Controllers
{
    public class TransfersController : Controller
    {
        private readonly BankRepository _repo;
        List<Customer> customerList = new List<Customer>();

        public TransfersController(BankRepository repo)
        {
            _repo = repo;
            customerList = _repo.Customers;
        }

        [HttpGet]
        public IActionResult Transfer(int value)
        {
            if (value == 2)
            {
                ViewBag.ErrorMessage = TempData["error"].ToString();
            }

            return View(customerList);
        }

        [HttpPost]
        public IActionResult Transfer(int fromNumber, int toNumber, string sum)
        {
            var returnedString = _repo.Transfer(fromNumber, toNumber, sum);

            if (returnedString == "OK")
            {
                return RedirectToAction("Verify", new { fromNumber = fromNumber, toNumber = toNumber });
            }

            else if (returnedString == "OVERDRAW ERROR")
            {
                TempData["error"] = "Ej tillåtet att överföra mer pengar än vad som finns på kontot";
                return RedirectToAction("Transfer", new { value = 2 });
            }

            else if (returnedString == "FROMACCOUNT NOT EXISTING")
            {
                TempData["error"] = "Från Kontonummer: Kontot finns inte";
                return RedirectToAction("Transfer", new { value = 2 });
            }

            else if (returnedString == "TOACCOUNT NOT EXISTING")
            {
                TempData["error"] = "Till Kontonummer: Kontot finns inte";
                return RedirectToAction("Transfer", new { value = 2 });
            }

            else if (returnedString == "FROMACCOUNT SAME AS TOACCOUNT")
            {
                TempData["error"] = "Ej tillåtet att överföra pengar till samma konto";
                return RedirectToAction("Transfer", new { value = 2 });
            }

            else
            {
                TempData["error"] = "Felaktigt inmatning";
                return RedirectToAction("Transfer", new { value = 2 });
            }
        }

        public IActionResult Verify(int fromNumber, int toNumber)
        {
            Account fromAccount = _repo.Accounts.Where(x => x.AccountId == fromNumber).FirstOrDefault();
            Customer fromPerson = _repo.Customers.Where(x => x.PersonId == fromAccount.PersonId).FirstOrDefault();
            fromAccount.Name = fromPerson.Name;
            fromAccount.LastName = fromPerson.LastName;

            Account toAccount = _repo.Accounts.Where(x => x.AccountId == toNumber).FirstOrDefault();
            Customer toPerson = _repo.Customers.Where(x => x.PersonId == toAccount.PersonId).FirstOrDefault();
            toAccount.Name = toPerson.Name;
            toAccount.LastName = toPerson.LastName;

            VerifyTransactionViewModel vm = new VerifyTransactionViewModel();
            vm.FromAccount = fromAccount;
            vm.ToAccount = toAccount;

            return View(vm);
        }
    }
}