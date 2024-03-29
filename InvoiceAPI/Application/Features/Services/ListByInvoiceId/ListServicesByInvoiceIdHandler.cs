﻿using AutoMapper;
using InvoiceAPI.Application.Repository;
using InvoiceAPI.Presentation.Models;
using MediatR;

namespace InvoiceAPI.Application.Features.Services.ListByInvoiceId;

public class ListServicesByInvoiceIdHandler : IRequestHandler<ListServicesByInvoiceIdQuery, IEnumerable<ServiceDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ListServicesByInvoiceIdHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ServiceDto>> Handle(ListServicesByInvoiceIdQuery request, CancellationToken cancellationToken)
    {
        var matches = await _unitOfWork.ServiceRepository.GetByInvoiceIdAsync(request.InvoiceId);
        return matches.Select(o => _mapper.Map<ServiceDto>(o));
    }
}
