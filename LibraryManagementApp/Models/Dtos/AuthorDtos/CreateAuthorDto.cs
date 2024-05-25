namespace LibraryManagementApp.Models.Dtos.AuthorDtos;

public class CreateAuthorDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public int YearOfBirth { get; set; }
    public List<int> BookIds { get; set; } = new List<int>();
}