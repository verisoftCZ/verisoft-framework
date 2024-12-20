﻿namespace Verisoft.DemoApi.Contracts.Models.Document;

public class Document : DocumentCreateModel
{
    public int Id { get; set; }

    public string ContentType { get; set; }

    public string Name { get; set; }

    public int UserId { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public string UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
