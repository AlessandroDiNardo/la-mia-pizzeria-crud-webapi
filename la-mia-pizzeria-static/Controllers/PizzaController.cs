using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace la_mia_pizzeria_static.Controllers
{
    public class PizzaController : Controller
    {
        public ICustomLogger Logger;

        public PizzaController(ICustomLogger logger)
        {
            Logger = logger;
        }

        [Authorize(Roles = "ADMIN,USER")]
        [HttpGet]
        public IActionResult Index()
        {
            using (PizzaContext db = new PizzaContext())
            {
                List<Pizza> pizze = db.Pizza.OrderBy(pizza => pizza.Id).ToList<Pizza>();

                if (pizze.Count == 0)
                    return View("Error", "Non ci sono pizze!");

                return View(pizze);
            }

        }

        ùù
        [HttpGet]
        public IActionResult Details(long id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(pizza => pizza.Id == id).Include(pizza => pizza.Category).FirstOrDefault();

                if (pizza == null)
                    return View("Error", "Nessuna pizza trovata con questo ID!");

                return View("Details", pizza);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Create()
        {
            using (PizzaContext db = new PizzaContext())
            {
                var ingredients = db.Ingredients.ToList();

                List<Category> categories = db.Categories.ToList();

                List<SelectListItem> listIngredients = new List<SelectListItem>();
                foreach (Ingredient ing in ingredients)
                {
                    listIngredients.Add(new SelectListItem() { Text = ing.Nome, Value = ing.Id.ToString() });
                }

                PizzaFormModel model = new PizzaFormModel();

                model.Pizza = new Pizza();
                model.Categories = categories;
                model.Ingredients = listIngredients;

                return View("Create", model);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using(PizzaContext db = new PizzaContext())
                {
                    var ingredients = db.Ingredients.ToList();
                    List<SelectListItem> listIngredients = new List<SelectListItem>();
                    foreach (Ingredient ing in ingredients)
                    {
                        listIngredients.Add(new SelectListItem() { Text = ing.Nome, Value = ing.Id.ToString() });
                    }

                    List<Category> categories = db.Categories.ToList();

                    data.Categories = categories;
                    data.Ingredients = listIngredients;
                    return View(data);
                }
            }

            using (PizzaContext db = new PizzaContext())
            {
                    
                Pizza pizza = new Pizza();
                pizza.Nome = data.Pizza.Nome;
                pizza.Descrizione = data.Pizza.Descrizione;
                pizza.Prezzo = data.Pizza.Prezzo;
                pizza.Img = data.Pizza.Img;

                pizza.CategoryId = data.Pizza.CategoryId;
                pizza.Ingredients = data.Pizza.Ingredients;
                foreach (var ingredientid in data.SelectedIngredients)
                {
                    int intidIngredient = int.Parse(ingredientid);
                    Ingredient ing = pizza.Ingredients.Where(i => i.Id == intidIngredient).FirstOrDefault();
                    db.Ingredients.Add(ing);
                }

                db.Pizza.Add(pizza);
                db.SaveChanges();

                Logger.WriteLog($"Elemento '{pizza.Nome}' creato!");

                return RedirectToAction("Index");
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
        public IActionResult Update(long id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(pizza => pizza.Id == id).Include(p => p.Ingredients).First();

                if (pizza == null)
                    return NotFound();

                else
                {

                    List<Ingredient> ingredients = db.Ingredients.ToList();
                    List<SelectListItem> listIngredients = new List<SelectListItem>();
                    foreach (Ingredient ing in ingredients)
                    {
                        listIngredients.Add(new SelectListItem() { Text = ing.Nome, Value = ing.Id.ToString(), Selected = pizza.Ingredients.Any(p => p.Id == ing.Id) });
                    }

                    List<Category> categories = db.Categories.ToList();
                    PizzaFormModel model = new PizzaFormModel();

                    model.Pizza = pizza;
                    model.Categories = categories;
                    model.Ingredients = listIngredients;

                    return View(model);
                }
            }
             
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(long id, PizzaFormModel data)
        {
            if (!ModelState.IsValid)
            {
                using(PizzaContext db = new PizzaContext())
                {
                    var ingredients = db.Ingredients.ToList();
                    List<SelectListItem> listIngredients = new List<SelectListItem>();
                    foreach (var ing in ingredients)
                    {
                        listIngredients.Add(new SelectListItem() { Text = ing.Nome, Value = ing.Id.ToString() });
                    }

                    List<Category> categories = db.Categories.ToList();

                    data.Categories = categories;
                    data.Ingredients = listIngredients;

                    return View("Update", data);
                }
            }

            using(PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(pizza => pizza.Id == id).Include(pizza => pizza.Ingredients).FirstOrDefault();

                if (pizza != null)
                {
                    pizza.Nome = data.Pizza.Nome;
                    pizza.Descrizione = data.Pizza.Descrizione;
                    pizza.Prezzo = data.Pizza.Prezzo;
                    pizza.CategoryId = data.Pizza.CategoryId;
                    pizza.Ingredients.Clear();
                    foreach (string idIngredient in data.SelectedIngredients)
                    {
                        int intIdIngredient = int.Parse(idIngredient);
                        Ingredient ing = db.Ingredients.Where(i => i.Id == intIdIngredient).FirstOrDefault();
                        pizza.Ingredients.Add(ing);

                    }

                    db.SaveChanges();

                    Logger.WriteLog($"Elemento '{pizza.Nome}' modificato!");

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }


            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Delete(long id)
        {
            using (PizzaContext db = new PizzaContext())
            {
                Pizza pizza = db.Pizza.Where(pizza => pizza.Id == id).FirstOrDefault();

                if (pizza != null)
                {
                    db.Pizza.Remove(pizza);

                    db.SaveChanges();

                    Logger.WriteLog($"Elemento '{pizza.Nome}' eliminato!");

                    return RedirectToAction("Index");
                }
                else
                {
                    return NotFound();
                }
            }
        }
    }
}

   
