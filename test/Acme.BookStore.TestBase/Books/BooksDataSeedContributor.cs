using System;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Uow;
using Acme.BookStore.Books;

namespace Acme.BookStore.Books
{
    public class BooksDataSeedContributor : IDataSeedContributor, ISingletonDependency
    {
        private bool IsSeeded = false;
        private readonly IBookRepository _bookRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public BooksDataSeedContributor(IBookRepository bookRepository, IUnitOfWorkManager unitOfWorkManager)
        {
            _bookRepository = bookRepository;
            _unitOfWorkManager = unitOfWorkManager;

        }

        public async Task SeedAsync(DataSeedContext context)
        {
            if (IsSeeded)
            {
                return;
            }

            await _bookRepository.InsertAsync(new Book
            (
                id: Guid.Parse("31ef5ebf-48a7-45d9-899a-56821d313c00"),
                bookName: "4f6c78f4d4ae4586ae464a0735d649d8ee74d09d1ced4ffbb584fef09ef163672eb095"
            ));

            await _bookRepository.InsertAsync(new Book
            (
                id: Guid.Parse("0df97749-9d46-4717-a9b7-50b65fb0acf1"),
                bookName: "d939504ea0204f85aaff817a696b8dafe30eff04777d478b950547b0c1c93b913d59e75a7b6b4a7bbc8f1"
            ));

            await _unitOfWorkManager.Current.SaveChangesAsync();

            IsSeeded = true;
        }
    }
}