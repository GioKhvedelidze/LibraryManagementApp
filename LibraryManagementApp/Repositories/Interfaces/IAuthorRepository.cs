using LibraryManagementApp.Models;
using LibraryManagementApp.Models.Dtos;
using LibraryManagementApp.Models.Dtos.AuthorDtos;

namespace LibraryManagementApp.Repositories.Interfaces;

public interface IAuthorRepository
{
    Task<IEnumerable<AuthorListDto>> GetAllAuthorsAsync();
    Task<AuthorDto?> GetAuthorByIdAsync(int id);
    Task<AuthorDto?> AddAuthorAsync(CreateAuthorDto createAuthorDto);
    Task<UpdateAuthorDto?> UpdateAuthorAsync(int id, UpdateAuthorDto updateAuthorDto);
    Task<Author?> DeleteAuthor(int id);
}