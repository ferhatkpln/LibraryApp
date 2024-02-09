using LibraryApp.Core.Concrete;
using LibraryApp.Models;
using LibraryApp.Services.Abstract;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApp.Controllers
{
    public class LibraryController : Controller
    {
        private readonly ILibraryService _libraryService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public LibraryController(ILibraryService libraryService, IWebHostEnvironment webHostEnvironment)
        {
            _libraryService = libraryService;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            var books = _libraryService.GetAllBooks();
            return View(books);
        }
        public IActionResult Borrow(int id)
        {
            var book = _libraryService.GetBookById(id);
            return View();
        }
        [HttpPost]
        public IActionResult Borrow(Book book)
        {
            // Ödünç alma işlemini gerçekleştir
            _libraryService.BorrowBook(book.Id, book.BorrowerName, DateTime.Now);

            return RedirectToAction("Index");
        }
        public IActionResult Deliver(int id)
        {
            // Kitabın ödünç alınan bilgilerini getir
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
            // Teslim etme işlemini gerçekleştir
            _libraryService.DeliverBook(book.Id);

            return RedirectToAction("Index");
        }

        public IActionResult AddBook()
        {
            var viewModel = new AddBookViewModel
            {
                Book = new Book(),
                FileUpload = new FileUploadModel()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddBook(AddBookViewModel viewModel)
        {
            // Bu action metodunda yeni kitap ekleme işlemini gerçekleştir
            var book = viewModel.Book;
            var fileUpload = viewModel.FileUpload;


            var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "img");
            var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileUpload.ImageFile.FileName;
            var filePath = Path.Combine(uploadFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                fileUpload.ImageFile.CopyTo(fileStream);
            }

            book.Author = viewModel.Book.Author;
            book.Title = viewModel.Book.Title;
            book.ReturnDate = null;
            book.BorrowerName = "";
            book.ImageUrl = uniqueFileName;
            book.IsAvailable = true;

            _libraryService.AddBook(book);
            return RedirectToAction("Index");
        }
    }
}
