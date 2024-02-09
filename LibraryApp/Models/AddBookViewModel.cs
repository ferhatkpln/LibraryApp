using LibraryApp.Core.Concrete;

namespace LibraryApp.Models
{
    public class AddBookViewModel
    {
        public Book Book { get; set; }
        public FileUploadModel FileUpload { get; set; }
    }
}
