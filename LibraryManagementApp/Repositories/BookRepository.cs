using LibraryManagementApp.Data;
using LibraryManagementApp.Models;
using LibraryManagementApp.Models.Dtos;
using LibraryManagementApp.Models.Dtos.BookDtos;
using LibraryManagementApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Repositories;

public class BookRepository : IBookRepository
{
    private readonly AppDbContext _dbContext;

    public BookRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<BookDto>> GetAllBooksAsync()
    {
        return await _dbContext.Books
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .Select(b => new BookDto
            {
                BookId = b.Id,
                Title = b.Title,
                Description = b.Title,
                Image = b.Image,
                Rating = b.Rating,
                PublicationDate = b.PublicationDate,
                Authors = b.BookAuthors.Select(ab => new AuthorDto
                {
                    AuthorId = ab.AuthorId,
                    FirstName  = ab.Author.FirstName,
                    LastName = ab.Author.LastName
                }).ToList()
            }).ToListAsync();
    }

    public async Task<IEnumerable<BookDto>> CheckAllAvailableBooksAsync()
    {
        return await _dbContext.Books
            .Where(b => !b.IsTaken)
            .Include(b => b.BookAuthors)
            .ThenInclude(ab => ab.Author)
            .Select(b => new BookDto
            {
                BookId = b.Id,
                Title = b.Title,
                Description = b.Title,
                Image = b.Image,
                Rating = b.Rating,
                PublicationDate = b.PublicationDate,
                Authors = b.BookAuthors.Select(ab => new AuthorDto
                {
                    AuthorId = ab.AuthorId,
                    FirstName = ab.Author.FirstName,
                    LastName = ab.Author.LastName
                }).ToList()
            }).ToListAsync();
    }

    public async Task<List<BookDto>> GetBookByIdAsync(int id)
    {
        return await _dbContext.Books.Where(b => b.Id == id)
            .Include(b => b.BookAuthors)
            .ThenInclude(ba => ba.Author)
            .Select(b => new BookDto
            {
                BookId = b.Id,
                Title = b.Title,
                Description = b.Title,
                Image = b.Image,
                Rating = b.Rating,
                PublicationDate = b.PublicationDate,
                Authors = b.BookAuthors.Select(ab => new AuthorDto
                {
                    AuthorId = ab.AuthorId,
                    FirstName = ab.Author.FirstName,
                    LastName = ab.Author.LastName
                }).ToList()
            }).ToListAsync();
        
    }

    public async Task<BookDto> AddBookAsync(CreateBookDto createBookDto)
    {
        var book = new Book
        {
            Title = createBookDto.Title,
            Description = createBookDto.Description,
            Image = createBookDto.Image,
            Rating = createBookDto.Rating,
            PublicationDate = createBookDto.PublicationDate,
            IsTaken = false,
            BookAuthors = new List<BookAuthor>()
        };

        // If there are authors specified, add them to the book
        if (createBookDto.AuthorIds.Any())
        {
            foreach (var authorId in createBookDto.AuthorIds)
            {
                var author = await _dbContext.Authors.FindAsync(authorId);
                if (author != null)
                {
                    book.BookAuthors.Add(new BookAuthor { Author = author });
                }
            }
        }
        await _dbContext.Books.AddAsync(book);
        await _dbContext.SaveChangesAsync();

        return new BookDto
        {
            BookId = book.Id,
            Title = book.Title,
            Authors = book.BookAuthors.Select(ab => new AuthorDto
            {
                AuthorId = ab.Author.Id,
                FirstName = ab.Author.FirstName
            }).ToList()
        };
    }
    
    public async Task<Book?> UpdateBookAsync(int id, BookDto bookDto)
    {
        var existingBook = await _dbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
        
        if (existingBook == null)
            return null;

        existingBook.Title = bookDto.Title;
        existingBook.Description = bookDto.Description;
        existingBook.Image = bookDto.Image;
        existingBook.Rating = bookDto.Rating;
        existingBook.PublicationDate = bookDto.PublicationDate;
        
        await _dbContext.SaveChangesAsync();

        return existingBook;
    }

    public async Task<Book?> CheckoutBookAsync(int id)
    {
        var book = await _dbContext.Books.FindAsync(id);
        if (book != null && !book.IsTaken)
        {
            book.IsTaken = true;
            await _dbContext.SaveChangesAsync();
        }

        return book;
    }

    public async Task<Book?> ReturnBookAsync(int id)
    {
        var book = await _dbContext.Books.FindAsync(id);
        if (book != null && book.IsTaken)
        {
            book.IsTaken = false;
            await _dbContext.SaveChangesAsync();
        }

        return book;
    }

    public async Task<Book?> DeleteBook(int id)
    {
        var bookModel = await _dbContext.Books
            .FirstOrDefaultAsync(b => b.Id == id);
        
        if (bookModel == null)
            return null;

        _dbContext.Books.Remove(bookModel);
        await _dbContext.SaveChangesAsync();

        return bookModel;
    }
}