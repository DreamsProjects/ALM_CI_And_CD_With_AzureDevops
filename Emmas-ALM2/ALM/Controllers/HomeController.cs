using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ALM.Models;
using ALM.Repository;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using ALM.ViewModels;
using ALM.Services;

namespace ALM.Controllers
{
    public class HomeController : Controller
    {
        private readonly BankRepository _repo;

        private IHostingEnvironment _environment;
        private readonly IConfiguration _config;
        private IEmail _emailsender;

        public HomeController(IHostingEnvironment environment, IConfiguration config, BankRepository repo, IEmail email)
        {
            _environment = environment;
            _config = config;
            _repo = repo;
            _emailsender = email;
        }

        public IActionResult Index() // användes bara vid test
        {

            var model = new HomeIndexViewModel();

            model.Customers = _repo.Customers;
            model.EnvironmentInfo = _environment.EnvironmentName;
            model.MyKey = _config.GetValue<string>("MyKey");
            model.EmailUsername = _config.GetValue<string>("Email:Username");
            if (_environment.IsDevelopment())
            {
                // Gör någon debug i mijön Development grej...
                model.EnvironmentInfo = model.EnvironmentInfo + " Gör en Debug grej... ";
            }
            if (_environment.IsEnvironment("Integration"))
            {
                // Gör någon debug i miljön Environment grej...
                model.EnvironmentInfo = model.EnvironmentInfo + " Gör en specialare i Integration... ";
            }
            return View(model);
        }

        public IActionResult About()
        {
            var customers = _repo.Customers;

            return View(customers);
        }

        [HttpPost]
        public IActionResult EmailSender(int number)
        {
            var customer = _repo.Customers.FirstOrDefault(x => x.PersonId == number);

            _emailsender.EmailSender(customer);
            return RedirectToAction("Contact");
        }

        public IActionResult Contact() //Skriver ut att det gick
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
