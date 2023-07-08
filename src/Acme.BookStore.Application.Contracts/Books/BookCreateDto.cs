using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Acme.BookStore.Books
{
    public class BookCreateDto
    {
        [Required]
        public string BookName { get; set; } = "100";
    }
}