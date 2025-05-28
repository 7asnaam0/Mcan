using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Service
    {
        [Key]
        public int Id { get; set; }
        public string? TitleAr { get; set; }
        public string? TitleEn { get; set; }
        public string? DescriptionAr { get; set; }
        public string? DescriptionEn { get; set; }
        public string? ImageUrl { get; set; }

        public string? IconClass {  get; set; } 

    }
}
