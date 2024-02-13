using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class FileUploadModel
    {
        [Required(ErrorMessage = "Resim alanı zorunludur.")]
        public IFormFile ImageFile { get; set; }
    }
}
