using System;

namespace Acme.BookStore.Books;

[Serializable]
public class BookExcelDownloadTokenCacheItem
{
    public string Token { get; set; }
}