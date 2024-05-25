using LibraryManagementApp.Models;
using LibraryManagementApp.Models.Dtos.BookDtos;

namespace LibraryManagementApp.Repositories.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<BookDto>> GetAllBooksAsync();
    Task<IEnumerable<BookDto>> CheckAllAvailableBooksAsync();
    Task<List<BookDto>> GetBookByIdAsync(int id);
    Task<BookDto> AddBookAsync(CreateBookDto createBookDto);
    Task<Book?> UpdateBookAsync(int id, BookDto bookDto);
    Task<Book?> CheckoutBookAsync(int id);
    Task<Book?> ReturnBookAsync(int id);
    Task<Book?> DeleteBook(int id);
}