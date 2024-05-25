namespace LibraryManagementApp.Models.Dtos.BookDtos;

public class CreateBookDto
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? Image { get; set; }
    public double Rating { get; set; }
    public DateTime PublicationDate { get; set; }
    public List<int> AuthorIds { get; set; } = new List<int>();
}