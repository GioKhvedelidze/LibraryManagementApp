using LibraryManagementApp.Data;
using LibraryManagementApp.Models;
using LibraryManagementApp.Models.Dtos;
using LibraryManagementApp.Models.Dtos.AuthorDtos;
using LibraryManagementApp.Models.Dtos.BookDtos;
using LibraryManagementApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementApp.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private readonly AppDbContext _dbContext;

    public AuthorRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IEnumerable<AuthorListDto>> GetAllAuthorsAsync()
    {
        return await _dbContext.Authors.Include(a => a.BookAuthors)
            .ThenInclude(ba => ba.Book)
            .Select(a => new AuthorListDto
            {
                AuthorId = a.Id,
                FirstName = a.FirstName,
                LastName = a.LastName,
                YearOfBirth = a.YearOfBirth,
                Books = a.BookAuthors.Select(ba => new BookDto
                {
                    BookId = ba.BookId,
                    Description = ba.Book.Description,
                    Image = ba.Book.Image,
                    PublicationDate = ba.Book.PublicationDate,
                    Title = ba.Book.Title
                }).ToList()

            }).ToListAsync();
    }

    public async Task<AuthorDto?> GetAuthorByIdAsync(int id)
    {
        var author = await _dbContext.Authors.Where(b => b.Id == id).FirstOrDefaultAsync();
        if (author == null)
            return null;

        return new AuthorDto
        {
            AuthorId = author.Id,
            FirstName = author.FirstName,
            LastName = author.LastName
        };
    }

    public async Task<AuthorDto?> AddAuthorAsync(CreateAuthorDto createAuthorDto)
    {
        var author = new Author
        {
            FirstName = createAuthorDto.FirstName,
            LastName = createAuthorDto.LastName,
            YearOfBirth = createAuthorDto.YearOfBirth,
            BookAuthors = new List<BookAuthor>()
        };
        
        if (createAuthorDto.BookIds.Any())
        {
            foreach (var bookId in createAuthorDto.BookIds)
            {
                var book = await _dbContext.Books.FindAsync(bookId);
                if (book != null)
                {
                    author.BookAuthors.Add(new BookAuthor { Book = book });
                }
            }
        }
        
        await _dbContext.Authors.AddAsync(author);
        await _dbContext.SaveChangesAsync();

        return new AuthorDto
        {
            AuthorId = author.Id,
            FirstName = $"{author.FirstName} {author.LastName}",
        };
    }

    public async Task<UpdateAuthorDto?> UpdateAuthorAsync(int id, UpdateAuthorDto updateAuthorDto)
    {
        var existingAuthor = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Id == id);

        if (existingAuthor == null)
            return null;

        existingAuthor.FirstName = updateAuthorDto.FirstName;
        existingAuthor.LastName = updateAuthorDto.LastName;
        existingAuthor.YearOfBirth = updateAuthorDto.YearOfBirth;

        await _dbContext.SaveChangesAsync();

        return new UpdateAuthorDto
        {
            FirstName = existingAuthor.FirstName,
            LastName = existingAuthor.LastName,
            YearOfBirth = existingAuthor.YearOfBirth
            
        };
    }

    public async Task<Author?> DeleteAuthor(int id)
    {
        var authorModel = await _dbContext.Authors.FirstOrDefaultAsync(a => a.Id == id);
        
        if (authorModel == null)
            return null;

        _dbContext.Authors.Remove(authorModel);
        await _dbContext.SaveChangesAsync();

        return authorModel;
    }
}