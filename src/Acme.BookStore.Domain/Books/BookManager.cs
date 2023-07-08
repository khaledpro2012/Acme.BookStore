using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Volo.Abp;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Domain.Services;
using Volo.Abp.Data;

namespace Acme.BookStore.Books
{
    public class BookManager : DomainService
    {
        private readonly IBookRepository _bookRepository;

        public BookManager(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<Book> CreateAsync(
        string bookName)
        {
            Check.NotNullOrWhiteSpace(bookName, nameof(bookName));

            var book = new Book(
             GuidGenerator.Create(),
             bookName
             );

            return await _bookRepository.InsertAsync(book);
        }

        public async Task<Book> UpdateAsync(
            Guid id,
            string bookName, [CanBeNull] string concurrencyStamp = null
        )
        {
            Check.NotNullOrWhiteSpace(bookName, nameof(bookName));

            var book = await _bookRepository.GetAsync(id);

            book.BookName = bookName;

            book.SetConcurrencyStampIfNotNull(concurrencyStamp);
            return await _bookRepository.UpdateAsync(book);
        }

    }
}