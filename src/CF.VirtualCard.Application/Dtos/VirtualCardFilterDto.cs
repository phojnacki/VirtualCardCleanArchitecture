﻿namespace CF.VirtualCard.Application.Dtos;

public record VirtualCardFilterDto
{
    public long Id { get; set; }
    public string CardNumber { get; set; }
    public string FirstName { get; set; }
    public string Surname { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string OrderBy { get; set; } = "firstName";
    public string SortBy { get; set; } = "asc";
}