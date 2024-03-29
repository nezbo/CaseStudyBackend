﻿using Microservice.Common.Domain.Models;

namespace InvoiceAPI.Domain.Models;

public record Invoice : BaseEntity
{
    public DateOnly IssuingDate { get; set; }
    public ushort Year { get; set; }
    public ushort Month { get; set; }
    public decimal Total { get; set; }
}
