using Microsoft.AspNetCore.Mvc;
using WebsiteOpcUa.Data;
using WebsiteOpcUa.Models;

namespace WebsiteOpcUa.Controllers
{
    public class ControlController : Controller
    {
        Influx _db;
        public ControlController(Influx db)
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

            await _db.ReadDataAsync("mem3", new List<string> { "y", "SetPoint", "Gain", "Ti", "przerugolowanie[%]", "czasNarastania", "czasOpoznienia", "czasRegulacji","u","ISE" });
         
            return Json(_db.Items);  // Zwracamy dane jako JSON
        }

        [HttpPost]
        public void SubmitAction(string name,string inputValue)
        {
            Item itemDb = new Item();
            itemDb.Name = name;
            try
            {
                itemDb.Value = Convert.ToSingle(inputValue);
                _db.writeData(itemDb);
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Value incorrect!";
            }

        }
        public void ChangeName(Influx db)
        {
            foreach (var itemDb in _db.Items)
            {
                switch (itemDb.Name)
                {
                    case "przerugolowanie[%]":
                        itemDb.Name = "Overshoot";
                        break;
                    case "czasNarastania":
                        itemDb.Name = "Rise time";
                        break;
                    case "czasOpoznienia":
                        itemDb.Name = "Delay time";
                        break;
                    case "czasRegulacji":
                        itemDb.Name = "Adjustment time";
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
