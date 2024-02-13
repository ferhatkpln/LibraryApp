using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Core.Concrete
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Kitap adı zorunludur.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Yazar adı zorunludur.")]
        public string Author { get; set; }
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Alıcı adı zorunludur.")]
        public string BorrowerName { get; set; }

        [Required(ErrorMessage = "Geri getirme tarihi zorunludur.")]
        public DateTime? ReturnDate { get; set; }

        [Required(ErrorMessage = "Resim alanı zorunludur.")]
        public string ImageUrl { get; set; }
    }
}
