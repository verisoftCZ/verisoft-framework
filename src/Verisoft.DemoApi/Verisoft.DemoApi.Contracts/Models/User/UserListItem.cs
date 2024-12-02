using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Contracts.Models.User;

public sealed class UserListItem
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string Surname { get; set; }

    public string Email { get; set; }

    public ICollection<DocumentEntity> Documents { get; set; } = new List<DocumentEntity>();
}
