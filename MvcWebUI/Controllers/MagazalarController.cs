using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MvcWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MagazalarController : Controller
    {
        private readonly IMagazaService _magazaService;

        public MagazalarController(IMagazaService magazaService)
        {
            _magazaService = magazaService;
        }

        // GET: MagazalarController
        public ActionResult Index()
        {
            var model = _magazaService.Query().ToList();
            return View(model);
        }

        // GET: MagazalarController/Details/5
        public ActionResult Details(int id)
        {
            var model = _magazaService.Query().SingleOrDefault(m => m.Id == id);
            return View(model);
        }

        // GET: MagazalarController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: MagazalarController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MagazaModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _magazaService.Add(model);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            return View(model);
        }

        // GET: MagazalarController/Edit/5
        public ActionResult Edit(int id)
        {
            var model = _magazaService.Query().SingleOrDefault(m => m.Id == id);
            return View(model);
        }

        // POST: MagazalarController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MagazaModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _magazaService.Update(model);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Index));
                ModelState.AddModelError("", result.Message);
            }
            return View(model);
        }

        // GET: MagazalarController/Delete/5
        public ActionResult Delete(int id)
        {
            var model = _magazaService.Query().SingleOrDefault(m => m.Id == id);
            return View(model);
        }

        // POST: MagazalarController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            _magazaService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
