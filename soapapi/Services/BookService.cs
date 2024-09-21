using System.ServiceModel;
using SoapApi.Contracts;
using SoapApi.Dtos;
using SoapApi.Mappers;
using SoapApi.Repositories;

namespace SoapApi.Services;

public class BookService : IBookContract{
    private readonly IBookRepository _bookRepository;
    public BookService(IBookRepository bookRepository){
        _bookRepository = bookRepository;
    }


    public async Task<bool> DeleteBookById(Guid bookId, CancellationToken cancellationToken)
    {
        var book = await _bookRepository.GetBookByIdAsync(bookId, cancellationToken);
        if (book is null){
            throw new FaultException("Book not found");
        }

        await _bookRepository.DeleteByIdAsync(book, cancellationToken);
        return true;
    }


    public async Task<BookResponseDto> GetBookById(Guid bookId, CancellationToken cancellationToken){
        var book = await _bookRepository.GetBookByIdAsync(bookId, cancellationToken);
        if(book is not null){
            return book.ToDto();
        }

        throw new FaultException("Book not found");
    }
}