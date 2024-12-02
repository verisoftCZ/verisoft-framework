using System.Linq.Expressions;
using Verisoft.Core.Common.Extenders;
using Verisoft.Core.Common.Linq;
using Verisoft.Core.Common.Linq.Specifications;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Common.Specifications;

public sealed class UserSpecification : ISpecification<UserEntity>
{
    public string[] FirstName { get; set; }

    public string[] Surname { get; set; }

    public string[] Email { get; set; }

    public DocumentEntity[] Documents { get; set; }

    public string SearchField { get; set; }

    public Expression<Func<UserEntity, bool>> SatisfiedBy()
    {
        Specification<UserEntity> specification = new TrueSpecification<UserEntity>();
        if (SearchField.HasValue<string>())
        {
            var firstnameSpecification = new DirectSpecification<UserEntity>(c => c.FirstName.Contains(SearchField));
            var surnameSpecification = new DirectSpecification<UserEntity>(c => c.Surname.Contains(SearchField));
            var emailSpecification = new DirectSpecification<UserEntity>(c => c.Email.Contains(SearchField));
            specification = SpecificationExtender.OrAll(firstnameSpecification, surnameSpecification, emailSpecification);
        }

        if (FirstName.HasValue())
        {
            specification = specification.BuildOrSpecification(FirstName, searchValue => (entity) => entity.FirstName.Contains(searchValue));
        }

        if (Surname.HasValue())
        {
            specification = specification.BuildOrSpecification(Surname, searchValue => (entity) => entity.Surname.Contains(searchValue));
        }

        if (Email.HasValue())
        {
            specification = specification.BuildOrSpecification(Email, searchValue => (entity) => entity.Email.Contains(searchValue));
        }

        if (Documents.HasValue())
        {
            specification = specification.BuildOrSpecification(Documents, searchValue => (entity) => entity.Documents.Contains(searchValue));
        }

        return specification.SatisfiedBy();
    }
}
