using LibraryApp.Core.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApp.Services.Abstract
{
    public interface ILibraryService
    {
        IEnumerable<Book> GetAllBooks();
        void BorrowBook(int bookId, string borrowerName, DateTime returnDate);
        void AddBook(Book newBook);
        void DeliverBook(int bookId);
        Book GetBookById(int bookId);
    }
}
