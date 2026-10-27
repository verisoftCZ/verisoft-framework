using Verisoft.Core.Common.Entities;

namespace Verisoft.Core.Data.EntityFramework.Test.Walkers.TestInfrastructure;

public class ParentEntity : BaseEntity<int>
{
    public string Name { get; set; } = string.Empty;

    public ICollection<CascadeChildEntity> CascadeChildren { get; set; } = [];

    public ICollection<GrandChildEntity> GrandChildren { get; set; } = [];

    public ICollection<RestrictChildEntity> RestrictChildren { get; set; } = [];

    public ICollection<SetNullChildEntity> SetNullChildren { get; set; } = [];
}

public class CascadeChildEntity : BaseEntity<int>
{
    public int ParentId { get; set; }

    public ParentEntity? Parent { get; set; }

    public ICollection<GrandChildEntity> GrandChildren { get; set; } = [];
}

public class GrandChildEntity : BaseEntity<int>
{
    public int CascadeChildId { get; set; }

    public CascadeChildEntity? CascadeChild { get; set; }
}

/// <summary>
/// Child with a nullable FK so the FK can be cleared in memory before the parent is deleted.
/// The nullable FK combined with Restrict behaviour reproduces the original bug.
/// </summary>
public class RestrictChildEntity : BaseEntity<int>
{
    public int? ParentId { get; set; }

    public ParentEntity? Parent { get; set; }
}

public class SetNullChildEntity : BaseEntity<int>
{
    public int? ParentId { get; set; }

    public ParentEntity? Parent { get; set; }
}
