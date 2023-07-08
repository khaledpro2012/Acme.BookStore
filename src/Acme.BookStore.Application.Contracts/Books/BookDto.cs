using System;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace Acme.BookStore.Books
{
    public class BookDto : FullAuditedEntityDto<Guid>, IHasConcurrencyStamp
    {
        public string BookName { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}