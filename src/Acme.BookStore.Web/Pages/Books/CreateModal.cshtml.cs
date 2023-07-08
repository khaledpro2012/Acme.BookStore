using Acme.BookStore.Shared;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Acme.BookStore.Books;

namespace Acme.BookStore.Web.Pages.Books
{
    public class CreateModalModel : BookStorePageModel
    {
        [BindProperty]
        public BookCreateViewModel Book { get; set; }

        private readonly IBooksAppService _booksAppService;

        public CreateModalModel(IBooksAppService booksAppService)
        {
            _booksAppService = booksAppService;

            Book = new();
        }

        public async Task OnGetAsync()
        {
            Book = new BookCreateViewModel();

            await Task.CompletedTask;
        }

        public async Task<IActionResult> OnPostAsync()
        {

            await _booksAppService.CreateAsync(ObjectMapper.Map<BookCreateViewModel, BookCreateDto>(Book));
            return NoContent();
        }
    }

    public class BookCreateViewModel : BookCreateDto
    {
    }
}