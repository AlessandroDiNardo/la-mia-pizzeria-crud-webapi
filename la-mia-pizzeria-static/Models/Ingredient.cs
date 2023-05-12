using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    [Table("ingredient")]
    public class Ingredient
    {
        [Key] public int Id { get; set; }
        [Required] public string Nome { get; set; }
        public List<Pizza> Pizzas { get; set; }
    }
}
