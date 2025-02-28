namespace Verisoft.CodebookApi.Application.Helpers;

public class CodebookApiErrorFactory : Verisoft.Core.Contracts.BusinessResult.ErrorFactory
{
    public const string LanguageDoesNotExistErrorCode = "LanguageDoesNotExist";
    public const string StringValueAlreadyExistErrorCode = "StringValueAlreadyExist";
    public const string StringValueTooLongErrorCode = "StringValueTooLong";
    public const string StringValueNotSetErrorCode = "StringValueNotSet";
    public const string DefaultLanguageTranslationNotSetErrorCode = "DefaultLanguageTranslationNotSet";
    public const string DescriptionTooLongErrorCode = "DescriptionTooLong";
    public const string TranslationValueNotSetErrorCode = "TranslationValueNotSet";
    public const string TranslationLanguagesNotUniqueErrorCode = "TranslationLanguagesNotUnique";
    public const string LocalTenantValuesNotSetErrorCode = "LocalTenantValuesNotSet";
    public const string LocalTenantValuesNotSingleSingleTenantValueErrorCode = "LocalTenantValuesNotSingleTenantValue";
    public const string DuplicitStringValueErrorCode = "DuplicitStringValue";
    public const string StringValueDoesNotStartWithLetterErrorCode = "StringValueDoesNotStartWithLetter";
    public const string CodebookAlreadyExistErrorCode = "CodebookAlreadyExist";
    public const string TenantValuesNotUniqueErrorCode = "TenantValuesNotUnique";
    public const string StringValueDoesNotExistErrorCode = "StringValueDoesNotExist";
    public const string TenantsNotExistErrorCode = "TenantsNotExist";
    public const string TranslationsNotSetErrorCode = "TranslationsNotSet";
    public const string CodebookNameNotSetErrorCode = "CodebookNameNotSet";
    public const string LanguageNameNotSetErrorCode = "LanguageNameNotSet";
    public const string LanguageAlreadyExistErrorCode = "LanguageAlreadyExist";
    public const string LanguageCodeNotSetErrorCode = "LanguageCodeNotSet";
    public const string LanguageCodeAlreadyExistErrorCode = "LanguageCodeAlreadyExist";

    public const string LanguageDoesNotExistErrorMessage = "Specified language does not exist.";
    public const string StringValueAlreadyExistErrorMessage = "Specified string value already exists.";
    public const string StringValueTooLongErrorMessage = "Specified string value is too long.";
    public const string StringValueNotSetErrorMessage = "String value is not set.";
    public const string DefaultLanguageTranslationNotSetErrorMessage = "Default language translation is not set.";
    public const string DescriptionTooLongErrorMessage = "Description is too long.";
    public const string TranslationValueNotSetErrorMessage = "Translation value is not set.";
    public const string TranslationLanguagesNotUniqueErrorMessage = "Translation languages is not unique.";
    public const string LocalTenantValuesNotSetErrorMessage = "Local tenant values are not set.";
    public const string LocalTenantValuesNotSingleSingleTenantValueErrorMessage = "Local tenant values do not have a single tenant value.";
    public const string DuplicitStringValueErrorMessage = "You are trying to add StringValue \"{stringValue}\" multiple times.";
    public const string StringValueDoesNotStartWithLetterErrorMessage = "String value does not start with a letter.";
    public const string CodebookAlreadyExistErrorMessage = "Codebook with name {codebookName} already exists.";
    public const string TenantValuesNotUniqueErrorMessage = "Tenant values is not unique.";
    public const string StringValueDoesNotExistErrorMessage = "Specified string value does not exist.";
    public const string TenantsNotExistErrorMessage = "Specified tenants do not exist.";
    public const string TranslationsNotSetErrorMessage = "Translations are not set.";
    public const string CodebookNameNotSetErrorMessage = "Codebook name is not set.";
    public const string LanguageNameNotSetErrorMessage = "Language name is not set.";
    public const string LanguageAlreadyExistErrorMessage = "Language with name {languageName} already exists.";
    public const string LanguageCodeNotSetErrorMessage = "Language code is not set.";
    public const string LanguageCodeAlreadyExistErrorMessage = "Language with code {languageCode} already exists.";

    public static Verisoft.Core.Contracts.BusinessResult.BusinessActionError DuplicitStringValue(string stringValue)
    {
        return Error(DuplicitStringValueErrorCode, DuplicitStringValueErrorMessage, new { stringValue });
    }

    public static Verisoft.Core.Contracts.BusinessResult.BusinessActionError EmptyStringValue()
    {
        return Error(StringValueNotSetErrorCode, StringValueNotSetErrorMessage);
    }
}
