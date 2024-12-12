using AgroManTech.Models;
using Microsoft.AspNetCore.Mvc;

namespace AgroManTech.Controllers
{
    public class TaCultivoController : Controller
    {
        private readonly TaCultivoData _data = new TaCultivoData();

        public IActionResult Index()
        {
            var cultivos = _data.GetAll();
            return View(cultivos);
        }

        public IActionResult Create()
        {
            ViewBag.Distritos = _data.GetDistritos();
            ViewBag.Extensions = _data.GetExtensions();
            return View();
        }

        [HttpPost]
        public IActionResult Create(TaCultivo cultivo)
        {
            if (ModelState.IsValid)
            {
                _data.Insert(cultivo);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Distritos = _data.GetDistritos();
            ViewBag.Extensions = _data.GetExtensions();
            return View(cultivo);
        }
        public IActionResult Details(int id)
        {
            var cultivo = _data.GetById(id);
            if (cultivo == null)
                return NotFound();

            return View(cultivo);
        }

        public IActionResult Delete(int id)
        {
            var cultivo = _data.GetById(id);
            if (cultivo == null)
            {
                return NotFound();
            }

            return View(cultivo);
        }


        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            _data.Delete(id); // Llama al método de eliminación en la capa de datos
            return RedirectToAction(nameof(Index));
        }
    }
}
