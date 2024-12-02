using System.Linq.Expressions;
using Verisoft.Core.Common.Extenders;
using Verisoft.Core.Common.Linq;
using Verisoft.Core.Common.Linq.Specifications;
using Verisoft.DemoApi.Common.Entities;

namespace Verisoft.DemoApi.Common.Specifications
{
    public class DocumentSpecification : ISpecification<DocumentEntity>
    {
        public string[] Name { get; set; }

        public string[] ContentType { get; set; }

        public string SearchField { get; set; }

        public Expression<Func<DocumentEntity, bool>> SatisfiedBy()
        {
            Specification<DocumentEntity> specification = new TrueSpecification<DocumentEntity>();
            if (SearchField.HasValue<string>())
            {
                var nameSpecification = new DirectSpecification<DocumentEntity>(c => c.Name.Contains(SearchField));
                var contentTypeSpecification = new DirectSpecification<DocumentEntity>(c => c.ContentType.Contains(SearchField));
                var descriptionSpecification = new DirectSpecification<DocumentEntity>(c => c.Description.Contains(SearchField));
                specification = SpecificationExtender.OrAll(nameSpecification, contentTypeSpecification, descriptionSpecification);
            }

            if (Name.HasValue())
            {
                specification = specification.BuildOrSpecification(Name, searchValue => (entity) => entity.Name.Contains(searchValue));
            }

            if (ContentType.HasValue())
            {
                specification = specification.BuildOrSpecification(ContentType, searchValue => (entity) => entity.ContentType.Contains(searchValue));
            }

            return specification.SatisfiedBy();
        }
    }
}
