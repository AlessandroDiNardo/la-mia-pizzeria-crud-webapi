using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ApiPizzaController : ControllerBase
    {
        [HttpGet]
        public IActionResult AllPizzas(string? name)
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> pizzas = new List<Pizza>();
                if (name == null)
                    pizzas = db.Pizza.ToList();
                else
                    pizzas = db.Pizza.Where(p => p.Nome == name).ToList();
                return Ok(pizzas);
            }
        }
        [HttpPut("{id}")]
        public IActionResult PizzaId(int id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                var pizza = db.Pizza.Where(p => p.Id == id).FirstOrDefault();
                return Ok(pizza);
            }
        }
    }
}
