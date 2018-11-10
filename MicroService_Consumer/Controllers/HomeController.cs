using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Consul;
using Microsoft.AspNetCore.Mvc;
using MicroService_Consumer.Models;

namespace MicroService_Consumer.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            ConsulClient consulClient = new ConsulClient(configuration =>
            {
                configuration.Address = new Uri("http://192.168.1.23:8500");
            });
            var values = consulClient.Agent.Services().Result.Response.Values;
            var result = consulClient.Agent.Services().Result.Response.Values
                .Where(p => p.Service.Equals("dotnetcoreservice"));
            var random = new Random();
            var index = random.Next(result.Count());
            var sevice = result.ElementAt(index);
            HttpClient httpClient = new HttpClient();
            var requestResult = await httpClient.GetAsync($"http://{sevice.Address}:{sevice.Port}/api/health");
            var response = await requestResult.Content.ReadAsStringAsync();
            Console.WriteLine(response);
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}