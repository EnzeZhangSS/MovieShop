using ApplicationCore.Contracts.Services;
using Microsoft.AspNetCore.Mvc;

namespace MovieShopMVC.Controllers
{
    public class CastController : Controller
    {
        private readonly ICastService _castService;

        public CastController(ICastService castService)
        {
            _castService = castService;
        }

        [HttpGet]
        public async Task<IActionResult> CastDetails(int id)
        {
            var castDetails = await _castService.GetCastDetails(id);
            return View(castDetails);
        }
    }
}
