using LibraryManagementApp.Models;
using LibraryManagementApp.Models.Dtos.BookDtos;
using LibraryManagementApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApp.Controllers;

[Route("/api/[Controller]")]
[ApiController]
public class BookController : ControllerBase
{
    private readonly IBookRepository _bookRepository;

    public BookController(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    /// <summary>
    /// Gets all existing books.
    /// </summary>
    /// <returns>A list of books.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetBooks()
    {
        var books = await _bookRepository.GetAllBooksAsync();
        return Ok(books);
    }
    
    /// <summary>
    /// Checks all available books.
    /// </summary>
    /// <returns>A list of available books.</returns>
    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<Book>>> CheckAllAvailableBooks()
    {
        var availableBooks = await _bookRepository.CheckAllAvailableBooksAsync();
        return Ok(availableBooks);
    }
    
    /// <summary>
    /// Gets a book by ID.
    /// </summary>
    /// <param name="id">The ID of the book to retrieve.</param>
    /// <returns>The book with the specified ID.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await _bookRepository.GetBookByIdAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return Ok(book);
    }
    
    /// <summary>
    /// Creates a new book.
    /// </summary>
    /// <param name="createBookDto">The details of the book to create.</param>
    /// <returns>The created book.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateBook([FromBody] CreateBookDto createBookDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        await _bookRepository.AddBookAsync(createBookDto);
        return Created();
    }
    
    /// <summary>
    /// Updates an existing book.
    /// </summary>
    /// <param name="id">The ID of the book to update.</param>
    /// <param name="bookDto">The updated book details.</param>
    /// <returns>No content.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] BookDto bookDto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var existingBook = await _bookRepository.GetBookByIdAsync(id);
        if (existingBook == null)
        {
            return NotFound();
        }
        await _bookRepository.UpdateBookAsync(id, bookDto);
        return NoContent();
    }
    
    /// <summary>
    /// Checks out a book.
    /// </summary>
    /// <param name="id">The ID of the book to check out.</param>
    /// <returns>No content.</returns>
    [HttpPut("{id:int}/checkout")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CheckoutBook(int id)
    {
        var book = await _bookRepository.CheckoutBookAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    
    /// <summary>
    /// Returns a book.
    /// </summary>
    /// <param name="id">The ID of the book to return.</param>
    /// <returns>No content.</returns>
    [HttpPut("{id:int}/return")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ReturnBook(int id)
    {
        var book = await _bookRepository.ReturnBookAsync(id);
        if (book == null)
        {
            return NotFound();
        }
        return NoContent();
    }
    
    /// <summary>
    /// Deletes a book.
    /// </summary>
    /// <param name="id">The ID of the book to delete.</param>
    /// <returns>No content.</returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
            
        var bookModel = await _bookRepository.DeleteBook(id);
            
        if (bookModel == null)
            return NotFound("Not Found");
            

        return NoContent();
    }
}