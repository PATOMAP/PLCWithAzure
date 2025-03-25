using Microsoft.AspNetCore.Mvc;
using WebsiteOpcUa.Data;
using WebsiteOpcUa.Models;

namespace WebsiteOpcUa.Controllers
{
    public class SimulationController : Controller
    {
        Influx _db;
        public SimulationController(Influx db)
        {
            _db = db;
        }
        public IActionResult Index()
        {

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {

            await _db.ReadDataAsync("mem1", new List<string> { "y", "u_Int"});
            return Json(_db.Items);  // Zwracamy dane jako JSON
        }

        [HttpPost]
        public void SubmitAction(string inputValue)
        {
            Item itemDb = new Item();
            itemDb.Name = "u_Int";
            try
            {
                itemDb.Value = Convert.ToSingle(inputValue);
                _db.writeData(itemDb);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Błędny zapis wartości!";
            }

        }
    }
}
