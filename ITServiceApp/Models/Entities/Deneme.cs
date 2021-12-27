using System.ComponentModel.DataAnnotations;

namespace ITServiceApp.Models.Entities
{
    public class Deneme
    {
        public int Id { get; set; }
        [StringLength(20)]
        public string Ad { get; set; }
    }
}
