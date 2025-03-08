using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuizPlatform.Domain.Commands;
using QuizPlatform.Domain.Models.Entities;
using QuizPlatform.Repository.Database;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace QuizPlatform.MVC.Controllers
{
    public class TestsController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        const string uri = "api/tests/";
        public TestsController(IHttpClientFactory httpClientFactory)
        {
            _clientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {

            HttpClient client = _clientFactory.CreateClient(
            name: "QuizPlatform");

            HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: uri);
            HttpResponseMessage response = await client.SendAsync(request);
            IEnumerable<Test>? model = await response.Content.ReadFromJsonAsync<IEnumerable<Test>>();

            return View(model);
        }




        // GET: Tests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HttpClient client = _clientFactory.CreateClient(
            name: "QuizPlatform");

            HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: (uri + $"{id}"));
            HttpResponseMessage response = await client.SendAsync(request);
            Test? test = await response.Content.ReadFromJsonAsync<Test?>();

            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // GET: Tests/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TestName,TestTime,IsActive,QuestionCount")] TestCommand command)
        {

            if (ModelState.IsValid)
            {
                HttpClient client = _clientFactory.CreateClient(
           name: "QuizPlatform");
                using HttpResponseMessage response = await client.PostAsJsonAsync(
                     uri,
                       command);


                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }

        // GET: Tests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HttpClient client = _clientFactory.CreateClient(
            name: "QuizPlatform");

            HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: (uri + $"{id}"));
            HttpResponseMessage response = await client.SendAsync(request);
            Test? test = await response.Content.ReadFromJsonAsync<Test?>();

            if (test == null)
            {
                return NotFound();
            }
            return View(test);
        }

        // POST: Tests/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TestId,TestName,TestTime,IsActive,QuestionCount")] TestCommand command)
        {
            if (id != command.TestId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                HttpClient client = _clientFactory.CreateClient(
           name: "QuizPlatform");
                using HttpResponseMessage response = await client.PutAsJsonAsync(
                     uri,
                       command);
                return RedirectToAction(nameof(Index));
            }
            return View(command);
        }

        // GET: Tests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HttpClient client = _clientFactory.CreateClient(
            name: "QuizPlatform");

            HttpRequestMessage request = new(method: HttpMethod.Get, requestUri: (uri + $"{id}"));
            HttpResponseMessage response = await client.SendAsync(request);
            Test? test = await response.Content.ReadFromJsonAsync<Test?>();

            if (test == null)
            {
                return NotFound();
            }

            return View(test);
        }

        // POST: Tests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            HttpClient client = _clientFactory.CreateClient(
          name: "QuizPlatform");

            HttpRequestMessage request = new(method: HttpMethod.Delete, requestUri: (uri + $"{id}"));
            HttpResponseMessage response = await client.DeleteAsync(request.RequestUri);
            return RedirectToAction(nameof(Index));
        }

        //private bool TestExists(int id)
        //{
        //    return _context.Tests.Any(e => e.TestId == id);
        //}
    }
}
