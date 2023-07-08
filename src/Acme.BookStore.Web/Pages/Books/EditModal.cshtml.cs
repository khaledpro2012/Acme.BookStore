using Acme.BookStore.Shared;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Volo.Abp.Application.Dtos;
using Acme.BookStore.Books;

namespace Acme.BookStore.Web.Pages.Books
{
    public class EditModalModel : BookStorePageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public BookUpdateViewModel Book { get; set; }

        private readonly IBooksAppService _booksAppService;

        public EditModalModel(IBooksAppService booksAppService)
        {
            _booksAppService = booksAppService;

            Book = new();
        }

        public async Task OnGetAsync()
        {
            var book = await _booksAppService.GetAsync(Id);
            Book = ObjectMapper.Map<BookDto, BookUpdateViewModel>(book);

        }

        public async Task<NoContentResult> OnPostAsync()
        {

            await _booksAppService.UpdateAsync(Id, ObjectMapper.Map<BookUpdateViewModel, BookUpdateDto>(Book));
            return NoContent();
        }
    }

    public class BookUpdateViewModel : BookUpdateDto
    {
    }
}