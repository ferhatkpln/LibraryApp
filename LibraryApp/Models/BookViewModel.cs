using LibraryApp.Core.Concrete;
using System.ComponentModel.DataAnnotations;

namespace LibraryApp.Models
{
    public class BookViewModel
    {
        public Book Book { get; set; }

        [Required(ErrorMessage = "Resim alanı zorunludur.")]
        public FileUploadModel FileUpload { get; set; }
    }
}
