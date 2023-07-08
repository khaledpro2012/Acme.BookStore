using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Volo.Abp.Domain.Entities;

namespace Acme.BookStore.Books
{
    public class BookUpdateDto : IHasConcurrencyStamp
    {
        [Required]
        public string BookName { get; set; }

        public string ConcurrencyStamp { get; set; }
    }
}