using Shouldly;
using System;
using System.Linq;
using System.Threading.Tasks;
using Acme.BookStore.Books;
using Acme.BookStore.EntityFrameworkCore;
using Xunit;

namespace Acme.BookStore.Books
{
    public class BookRepositoryTests : BookStoreEntityFrameworkCoreTestBase
    {
        private readonly IBookRepository _bookRepository;

        public BookRepositoryTests()
        {
            _bookRepository = GetRequiredService<IBookRepository>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _bookRepository.GetListAsync(
                    bookName: "4f6c78f4d4ae4586ae464a0735d649d8ee74d09d1ced4ffbb584fef09ef163672eb095"
                );

                // Assert
                result.Count.ShouldBe(1);
                result.FirstOrDefault().ShouldNotBe(null);
                result.First().Id.ShouldBe(Guid.Parse("31ef5ebf-48a7-45d9-899a-56821d313c00"));
            });
        }

        [Fact]
        public async Task GetCountAsync()
        {
            // Arrange
            await WithUnitOfWorkAsync(async () =>
            {
                // Act
                var result = await _bookRepository.GetCountAsync(
                    bookName: "d939504ea0204f85aaff817a696b8dafe30eff04777d478b950547b0c1c93b913d59e75a7b6b4a7bbc8f1"
                );

                // Assert
                result.ShouldBe(1);
            });
        }
    }
}