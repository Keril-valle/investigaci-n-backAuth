using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LibraryService.WebAPI.Data;
using LibraryService.WebAPI.Services;
using LibraryService.WebAPI.DTO;

namespace LibraryService.WebAPI.Controllers
{
    [ApiController]
    [Route("api/libraries/{libraryId}/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly ILibrariesService _librariesService;
        private readonly IBooksService _booksService;

        public BooksController(IBooksService booksService, ILibrariesService librariesService)
        {
            _librariesService = librariesService;
            _booksService = booksService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(int libraryId, BookForm bookForm)
        {
            var library = (await _librariesService.Get(new[] { libraryId })).FirstOrDefault();
            if (library == null)
                return NotFound();

            var book = new Book
            {
                Name = bookForm.Name,
                Category = bookForm.Category,
                LibraryId = libraryId
            };

            await _booksService.Add(book);
            return StatusCode(201, book);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(int libraryId)
        {
            var library = (await _librariesService.Get(new[] { libraryId })).FirstOrDefault();
            if (library == null)
                return NotFound();

            var books = await _booksService.Get(libraryId, System.Array.Empty<int>());
            return Ok(books);
        }
    }
}