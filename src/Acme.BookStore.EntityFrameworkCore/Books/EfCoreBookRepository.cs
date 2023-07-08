using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;
using Acme.BookStore.EntityFrameworkCore;

namespace Acme.BookStore.Books
{
    public class EfCoreBookRepository : EfCoreRepository<BookStoreDbContext, Book, Guid>, IBookRepository
    {
        public EfCoreBookRepository(IDbContextProvider<BookStoreDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        public async Task<List<Book>> GetListAsync(
            string filterText = null,
            string bookName = null,
            string sorting = null,
            int maxResultCount = int.MaxValue,
            int skipCount = 0,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetQueryableAsync()), filterText, bookName);
            query = query.OrderBy(string.IsNullOrWhiteSpace(sorting) ? BookConsts.GetDefaultSorting(false) : sorting);
            return await query.PageBy(skipCount, maxResultCount).ToListAsync(cancellationToken);
        }

        public async Task<long> GetCountAsync(
            string filterText = null,
            string bookName = null,
            CancellationToken cancellationToken = default)
        {
            var query = ApplyFilter((await GetDbSetAsync()), filterText, bookName);
            return await query.LongCountAsync(GetCancellationToken(cancellationToken));
        }

        protected virtual IQueryable<Book> ApplyFilter(
            IQueryable<Book> query,
            string filterText,
            string bookName = null)
        {
            return query
                    .WhereIf(!string.IsNullOrWhiteSpace(filterText), e => e.BookName.Contains(filterText))
                    .WhereIf(!string.IsNullOrWhiteSpace(bookName), e => e.BookName.Contains(bookName));
        }
    }
}