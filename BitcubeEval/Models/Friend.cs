using System.ComponentModel.DataAnnotations;

namespace BitcubeEval.Models
{
    public class Friend
    {
        public int ID { get; set; }

        [Required]
        public int UserID1 { get; set; }

        [Required]
        public int UserID2 { get; set; }

        [Required]
        public bool Confirmed { get; set; }
    }
}
