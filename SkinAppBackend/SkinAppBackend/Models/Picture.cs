using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SkinAppBackend.Models
{
    public class Picture
    {
        [Key]
        public int PictureId { get; set; }
        [Required]
        public string ImageString { get; set; }
        [Required]
        public DateTime PictureTakenDate { get; set; }
        [Required]
        public Boolean UserTaken { get; set; }
        public int SectionId { get; set; }
        public virtual Section Section { get; set; }
    }
}
