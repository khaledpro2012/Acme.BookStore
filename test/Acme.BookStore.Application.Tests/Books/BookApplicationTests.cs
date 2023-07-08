using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Xunit;

namespace Acme.BookStore.Books
{
    public class BooksAppServiceTests : BookStoreApplicationTestBase
    {
        private readonly IBooksAppService _booksAppService;
        private readonly IRepository<Book, Guid> _bookRepository;

        public BooksAppServiceTests()
        {
            _booksAppService = GetRequiredService<IBooksAppService>();
            _bookRepository = GetRequiredService<IRepository<Book, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _booksAppService.GetListAsync(new GetBooksInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("31ef5ebf-48a7-45d9-899a-56821d313c00")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("0df97749-9d46-4717-a9b7-50b65fb0acf1")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _booksAppService.GetAsync(Guid.Parse("31ef5ebf-48a7-45d9-899a-56821d313c00"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("31ef5ebf-48a7-45d9-899a-56821d313c00"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new BookCreateDto
            {
                BookName = "5438180"
            };

            // Act
            var serviceResult = await _booksAppService.CreateAsync(input);

            // Assert
            var result = await _bookRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.BookName.ShouldBe("5438180");
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new BookUpdateDto()
            {
                BookName = "61e2f3f84"
            };

            // Act
            var serviceResult = await _booksAppService.UpdateAsync(Guid.Parse("31ef5ebf-48a7-45d9-899a-56821d313c00"), input);

            // Assert
            var result = await _bookRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.BookName.ShouldBe("61e2f3f84");
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _booksAppService.DeleteAsync(Guid.Parse("31ef5ebf-48a7-45d9-899a-56821d313c00"));

            // Assert
            var result = await _bookRepository.FindAsync(c => c.Id == Guid.Parse("31ef5ebf-48a7-45d9-899a-56821d313c00"));

            result.ShouldBeNull();
        }
    }
}