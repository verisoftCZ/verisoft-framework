using Microsoft.AspNetCore.Mvc.ModelBinding;
using Verisoft.Core.Common.Entities.Codebooks;

namespace Verisoft.Core.Codebooks.Common.ModelBinders;

public class CodebookRecordModelBinder<TCodebook> : IModelBinder
    where TCodebook : CodebookRecordType, new()
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var modelName = bindingContext.ModelName;
        var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            return Task.CompletedTask;
        }

        var stringVal = valueProviderResult.FirstValue;
        if (stringVal == null)
        {
            return Task.CompletedTask;
        }

        try
        {
            var codebookValue = (CodebookRecord<TCodebook>)stringVal!;
            bindingContext.Result = ModelBindingResult.Success(codebookValue);
        }
        catch (Exception ex)
        {
            bindingContext.ModelState.TryAddModelError(modelName, ex.Message);
        }

        return Task.CompletedTask;
    }
}