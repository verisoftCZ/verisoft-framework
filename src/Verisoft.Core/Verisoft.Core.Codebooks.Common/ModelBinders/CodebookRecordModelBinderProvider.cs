using Microsoft.AspNetCore.Mvc.ModelBinding;
using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Codebooks.Common.ModelBinders;

public class CodebookRecordModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        var modelType = context.Metadata.ModelType;

        if (modelType.IsGenericType
            && modelType.GetGenericTypeDefinition() == typeof(CodebookRecord<>))
        {
            var tCodebook = modelType.GetGenericArguments()[0];
            var binderType = typeof(CodebookRecordModelBinder<>).MakeGenericType(tCodebook);
            return (IModelBinder?)Activator.CreateInstance(binderType);
        }

        return null;
    }
}