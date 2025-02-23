﻿namespace PetInsurancePlatform.Insurance.Application.Dtos;

public sealed class AddressDto
{
    public static readonly AddressDto None = new();

    public int District { get; set; }

    public string Street { get; set; } = string.Empty;

    public string Alley { get; set; } = string.Empty;

    public int PlateNumber { get; set; }

    public long PostalCode { get; set; }
}
