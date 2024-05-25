using LibraryManagementApp.Models.Dtos.BookDtos;

namespace LibraryManagementApp.Models.Dtos;

public class AuthorListDto
{
    public int AuthorId { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int YearOfBirth { get; set; }
    public ICollection<BookDto> Books { get; set; }
}