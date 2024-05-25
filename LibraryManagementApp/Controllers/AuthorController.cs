using LibraryManagementApp.Models;
using LibraryManagementApp.Models.Dtos;
using LibraryManagementApp.Models.Dtos.AuthorDtos;
using LibraryManagementApp.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementApp.Controllers;

[Route("/api/[Controller]")]
[ApiController]
public class AuthorController : ControllerBase
{
    private readonly IAuthorRepository _authorRepository;

    public AuthorController(IAuthorRepository authorRepository)
    {
        _authorRepository = authorRepository;
    }

    /// <summary>
    /// Gets all existing authors.
    /// </summary>
    /// <returns>A list of authors and their books.</returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAuthors()
    {
        var authors = await _authorRepository.GetAllAuthorsAsync();
        return Ok(authors);
    }
    
    /// <summary>
    /// Gets an author by ID.
    /// </summary>
    /// <param name="id">The ID of the author to retrieve.</param>
    /// <returns>The author with the specified ID.</returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Author>> GetAuthor(int id)
    {
        var author = await _authorRepository.GetAuthorByIdAsync(id);

        if (author == null)
        {
            return NotFound();
        }

        return Ok(author);
    }
    
    /// <summary>
    /// Creates a new author.
    /// </summary>
    /// <param name="createAuthorDto">The details of the author to create.</param>
    /// <returns>The created author.</returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Author>> CreateAuthor(CreateAuthorDto createAuthorDto)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        await _authorRepository.AddAuthorAsync(createAuthorDto);
        return Created();
    }

    /// <summary>
    /// Updates an existing author.
    /// </summary>
    /// <param name="id">The ID of the author to update.</param>
    /// <param name="updateAuthorDto">The updated author details.</param>
    /// <returns>The updated author.</returns>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAuthor(int id, UpdateAuthorDto updateAuthorDto)
    {
         await _authorRepository.UpdateAuthorAsync(id, updateAuthorDto);

        return NoContent();
    }
    
    /// <summary>
    /// Deletes an author.
    /// </summary>
    /// <param name="id">The ID of the author to delete.</param>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var author = await _authorRepository.GetAuthorByIdAsync(id);

        if (author == null)
        {
            return NotFound();
        }

        await _authorRepository.DeleteAuthor(id);
        
        return NoContent();
    }
}