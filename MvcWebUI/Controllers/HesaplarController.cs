using Business.Models;
using Business.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace MvcWebUI.Controllers
{
    public class HesaplarController : Controller
    {
        private readonly IHesapService _hesapService;
        private readonly IUlkeService _ulkeService;
        private readonly ISehirService _sehirService;

        public HesaplarController(IHesapService hesapService, IUlkeService ulkeService, ISehirService sehirService)
        {
            _hesapService = hesapService;
            _ulkeService = ulkeService;
            _sehirService = sehirService;
        }

        public IActionResult Giris()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Giris(KullaniciGirisModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _hesapService.Giris(model);
                if (result.IsSuccessful)
                {
                    List<Claim> claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Name, result.Data.KullaniciAdi),
                        new Claim(ClaimTypes.Role, result.Data.RolAdiDisplay)
                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", result.Message);
            }
            return View(model);
        }

        public async Task<IActionResult> Cikis()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult YetkisizIslem()
        {
            return View("Hata", "Bu işlem için yetkiniz bulunmamaktadır!");
        }

        public IActionResult Kayit()
        {
            ViewBag.Ulkeler = new SelectList(_ulkeService.Query().ToList(), "Id", "Adi");
            return View();
        }

        [HttpPost]
        public IActionResult Kayit(KullaniciKayitModel model)
        {
            if (ModelState.IsValid)
            {
                var result = _hesapService.Kayit(model);
                if (result.IsSuccessful)
                    return RedirectToAction(nameof(Giris));
                ModelState.AddModelError("", result.Message);
            }
            ViewBag.Ulkeler = new SelectList(_ulkeService.Query().ToList(), "Id", "Adi", model.UlkeId);

            //var sehirResult = _sehirService.List(model.UlkeId.HasValue ? model.UlkeId.Value : -1);
            var sehirResult = _sehirService.List(model.UlkeId ?? -1);

            ViewBag.Sehirler = new SelectList(sehirResult.Data, "Id", "Adi", model.SehirId);
            
            return View(model);
        }
    }
}
