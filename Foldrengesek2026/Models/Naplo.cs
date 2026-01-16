using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Foldrengesek2026.Models
{
    public class Naplo
    {
        public int ID { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Dátum")]
        public DateTime Datum { get; set; }
        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Idő")]
        public TimeSpan Ido { get; set; }
        [Required]
        [Display(Name = "Magnitúdó")]
        [Column(TypeName = "decimal(4, 2)")]
        public decimal Magnitudo { get; set; }
        [Required]
        [Display(Name = "Intenzitás")]
        [Column(TypeName = "decimal(4, 2)")]
        public decimal Intenzitas { get; set; }

        // FK + navigáció
        [Required]
        [DisplayFormat(NullDisplayText = "Nincs megadva település")]
        [Display(Name = "Település")]
        public int TelepulesID { get; set; }
        public virtual Telepules? Telepules { get; set; } = null!;

    }
}
