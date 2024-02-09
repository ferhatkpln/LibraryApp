using LibraryApp.Core.Concrete;
using LibraryApp.Data;
using LibraryApp.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace LibraryApp.Services.Concrete
{
    public class LibraryService : ILibraryService
    {
        private readonly LibraryContext _context;

        public LibraryService(LibraryContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books.OrderBy(book => book.Title).ToList();
        }

        public void BorrowBook(int bookId, string borrowerName, DateTime returnDate)
        {
            var book = _context.Books.Find(bookId);
            if (book != null)
            {
                book.IsAvailable = false;
                book.BorrowerName = borrowerName;
                book.ReturnDate = returnDate;
                _context.SaveChanges();
            }
        }

        public void AddBook(Book newBook)
        {           
            _context.Books.Add(newBook);
            _context.SaveChanges();
        }
        public void DeliverBook(int bookId)
        {
            var book = _context.Books.Find(bookId);

            if (book != null)
            {
                // Teslim etme işlemini gerçekleştir
                book.BorrowerName = "";
                book.ReturnDate = null;
                book.IsAvailable = true;

                _context.SaveChanges();
            }
        }

        public Book GetBookById(int bookId)
        {
            return _context.Books.Find(bookId);
        }
    }
}
