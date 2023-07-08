using Volo.Abp.Application.Dtos;
using System;

namespace Acme.BookStore.Books
{
    public class BookExcelDownloadDto
    {
        public string DownloadToken { get; set; }

        public string? FilterText { get; set; }

        public string? BookName { get; set; }

        public BookExcelDownloadDto()
        {

        }
    }
}