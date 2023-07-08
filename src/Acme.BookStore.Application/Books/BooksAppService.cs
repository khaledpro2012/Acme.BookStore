using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Acme.BookStore.Permissions;
using Acme.BookStore.Books;
using MiniExcelLibs;
using Volo.Abp.Content;
using Volo.Abp.Authorization;
using Volo.Abp.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Acme.BookStore.Shared;

namespace Acme.BookStore.Books
{

    [Authorize(BookStorePermissions.Books.Default)]
    public class BooksAppService : ApplicationService, IBooksAppService
    {
        private readonly IDistributedCache<BookExcelDownloadTokenCacheItem, string> _excelDownloadTokenCache;
        private readonly IBookRepository _bookRepository;
        private readonly BookManager _bookManager;

        public BooksAppService(IBookRepository bookRepository, BookManager bookManager, IDistributedCache<BookExcelDownloadTokenCacheItem, string> excelDownloadTokenCache)
        {
            _excelDownloadTokenCache = excelDownloadTokenCache;
            _bookRepository = bookRepository;
            _bookManager = bookManager;
        }

        public virtual async Task<PagedResultDto<BookDto>> GetListAsync(GetBooksInput input)
        {
            var totalCount = await _bookRepository.GetCountAsync(input.FilterText, input.BookName);
            var items = await _bookRepository.GetListAsync(input.FilterText, input.BookName, input.Sorting, input.MaxResultCount, input.SkipCount);

            return new PagedResultDto<BookDto>
            {
                TotalCount = totalCount,
                Items = ObjectMapper.Map<List<Book>, List<BookDto>>(items)
            };
        }

        public virtual async Task<BookDto> GetAsync(Guid id)
        {
            return ObjectMapper.Map<Book, BookDto>(await _bookRepository.GetAsync(id));
        }

        [Authorize(BookStorePermissions.Books.Delete)]
        public virtual async Task DeleteAsync(Guid id)
        {
            await _bookRepository.DeleteAsync(id);
        }

        [Authorize(BookStorePermissions.Books.Create)]
        public virtual async Task<BookDto> CreateAsync(BookCreateDto input)
        {

            var book = await _bookManager.CreateAsync(
            input.BookName
            );

            return ObjectMapper.Map<Book, BookDto>(book);
        }

        [Authorize(BookStorePermissions.Books.Edit)]
        public virtual async Task<BookDto> UpdateAsync(Guid id, BookUpdateDto input)
        {

            var book = await _bookManager.UpdateAsync(
            id,
            input.BookName, input.ConcurrencyStamp
            );

            return ObjectMapper.Map<Book, BookDto>(book);
        }

        [AllowAnonymous]
        public virtual async Task<IRemoteStreamContent> GetListAsExcelFileAsync(BookExcelDownloadDto input)
        {
            var downloadToken = await _excelDownloadTokenCache.GetAsync(input.DownloadToken);
            if (downloadToken == null || input.DownloadToken != downloadToken.Token)
            {
                throw new AbpAuthorizationException("Invalid download token: " + input.DownloadToken);
            }

            var items = await _bookRepository.GetListAsync(input.FilterText, input.BookName);

            var memoryStream = new MemoryStream();
            await memoryStream.SaveAsAsync(ObjectMapper.Map<List<Book>, List<BookExcelDto>>(items));
            memoryStream.Seek(0, SeekOrigin.Begin);

            return new RemoteStreamContent(memoryStream, "Books.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
        }

        public async Task<DownloadTokenResultDto> GetDownloadTokenAsync()
        {
            var token = Guid.NewGuid().ToString("N");

            await _excelDownloadTokenCache.SetAsync(
                token,
                new BookExcelDownloadTokenCacheItem { Token = token },
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30)
                });

            return new DownloadTokenResultDto
            {
                Token = token
            };
        }
    }
}