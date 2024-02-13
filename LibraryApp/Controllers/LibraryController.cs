using LibraryApp.Core.Concrete;
using LibraryApp.Models;
using LibraryApp.Services.Abstract;
using LibraryApp.Services.Concrete;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryApp.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILibraryService _libraryService;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger<LibraryController> _logger;

        public LibraryController(ILibraryService libraryService, IWebHostEnvironment webHostEnvironment, ILogger<LibraryController> logger)
        {
            _libraryService = libraryService;
            _webHostEnvironment = webHostEnvironment;
            _logger = logger;
        }

        public IActionResult Index()//bütün kitaplar listelenir.
        {            
            try
            {
                var books = _libraryService.GetAllBooks();
                return View(books);
            }
            catch (Exception ex)
            {
                string message = "Kitaplar Listelenirken Hata Oluştu";
                ViewBag.ErrorMessage = message;
                _logger.LogError(ex, message);
                return RedirectToAction("Index", "Error");
            }
        }
        public IActionResult Borrow(int id)//ödünç verme işleminin get kısmı. bu kısımda uygun id ye göre kitap bulunur ve getirilir.
        {
            var book = _libraryService.GetBookById(id);
            return View();
        }
        [HttpPost]
        public IActionResult Borrow(Book book)//ödünç verme işleminin post kısmı. Kullanıcıdan alınan bilgileri service e gönderir.
        {
            if (book.BorrowerName != null && book.ReturnDate != null)
            {
                try
                {
                    _libraryService.BorrowBook(book.Id, book.BorrowerName, DateTime.Now);
                }
                catch (Exception ex)
                {
                    var message = "Ödünç alma işlemi sırasında bir hata oluştu.";
                    _logger.LogError(ex, message);
                    // Hata mesajını kullanıcıya gösterme
                    ViewBag.ErrorMessage = message;
                    return RedirectToAction("Index", "Error");
                }

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return View(book);
            }
            
        }
        public IActionResult Deliver(int id)
        {
            // Eşleşen id ye göre kitabın ödünç alınan kişi bilgilerini getir
            var book = _libraryService.GetBookById(id);

            if (book == null || string.IsNullOrEmpty(book.BorrowerName))
            {
                return RedirectToAction("Index");
            }

            return View(book);
        }

        [HttpPost]
        public IActionResult Deliver(Book book)
        {   
            try
            {
                // Teslim etme işlemi için gerekli kitabı service katmanına gönderir.
                _libraryService.DeliverBook(book.Id);
            }
            catch (Exception ex)
            {
                string message = "Teslim etme işlemi sırasında bir hata oluştu.";
                _logger.LogError(ex, message);
                ViewBag.ErrorMessage = message;
                return RedirectToAction("Index", "Error");
            }
            return RedirectToAction("Index");
        }

        public IActionResult AddBook()
        {
            var viewModel = new BookViewModel
            {
                Book = new Book(),
                FileUpload = new FileUploadModel()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddBook(BookViewModel viewModel)
        {
            // Bu action metodunda yeni kitap ekleme işlemini gerçekleştir
            if (viewModel.Book.Author != null && viewModel.Book.Title != null && viewModel.FileUpload.ImageFile != null)
            {
                try
                {
                    var book = viewModel.Book;
                    var fileUpload = viewModel.FileUpload;

                    //eklenen dosyanın pathini kaydetmek için
                    var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileUpload.ImageFile.FileName;
                    var filePath = Path.Combine(uploadFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        fileUpload.ImageFile.CopyTo(fileStream);
                    }

                    book.Author = viewModel.Book.Author;
                    book.Title = viewModel.Book.Title;
                    var date = new DateTime(1111 - 11 - 11);
                    book.ReturnDate = date;
                    book.BorrowerName = "----";
                    book.ImageUrl = uniqueFileName;
                    book.IsAvailable = true;

                    _libraryService.AddBook(book);
                }
                catch (Exception ex)
                {
                    string message = "Kitap ekleme işlemi sırasında bir hata oluştu.";
                    _logger.LogError(ex,message);
                    ViewBag.ErrorMessage = message;
                    return RedirectToAction("Index", "Error");
                }
                return RedirectToAction("Index");
            }
            else
            {
                return View(viewModel);
            }       
        }
    }
}
