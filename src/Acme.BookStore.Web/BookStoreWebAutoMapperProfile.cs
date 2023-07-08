using Acme.BookStore.Web.Pages.Books;
using Volo.Abp.AutoMapper;
using Acme.BookStore.Books;
using AutoMapper;

namespace Acme.BookStore.Web;

public class BookStoreWebAutoMapperProfile : Profile
{
    public BookStoreWebAutoMapperProfile()
    {
        //Define your object mappings here, for the Web project

        CreateMap<BookDto, BookUpdateViewModel>();
        CreateMap<BookUpdateViewModel, BookUpdateDto>();
        CreateMap<BookCreateViewModel, BookCreateDto>();
    }
}