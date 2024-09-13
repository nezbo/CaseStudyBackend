using AutoMapper;
using InvoiceAPI.Application.Repository;
using InvoiceAPI.Presentation.Models;
using MediatR;

namespace InvoiceAPI.Application.Features.Services.ListByInvoiceId;

public class ListServicesByInvoiceIdHandler : IRequestHandler<ListServicesByInvoiceIdQuery, IEnumerable<ServiceDto>>
{
    private readonly IServiceRepository _repository;
    private readonly IMapper _mapper;

    public ListServicesByInvoiceIdHandler(IServiceRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServiceDto>> Handle(ListServicesByInvoiceIdQuery request, CancellationToken cancellationToken)
    {
        var matches = await _repository.GetByInvoiceIdAsync(request.InvoiceId);
        return matches.Select(o => _mapper.Map<ServiceDto>(o));
    }
}
