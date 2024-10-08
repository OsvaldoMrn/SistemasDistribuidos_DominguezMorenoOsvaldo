using SoapApi.Dtos;

namespace SoapApi.Repositories;

 public interface IBookRepository{
        Task<BookModel> GetBookByIdAsync(Guid id, CancellationToken cancellationToken);
        public Task DeleteByIdAsync(BookModel book, CancellationToken cancellationToken);    
}