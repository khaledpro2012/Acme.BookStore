using Volo.Abp.Application.Dtos;
using System;

namespace Acme.BookStore.Books
{
    public class GetBooksInput : PagedAndSortedResultRequestDto
    {
        public string? FilterText { get; set; }

        public string? BookName { get; set; }

        public GetBooksInput()
        {

        }
    }
}