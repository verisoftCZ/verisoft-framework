namespace Verisoft.Core.Codebooks.Constants;

public static class ValidationConstants
{
    public const string CollectionEmpty = "This collection cannot be empty";
    public const string CollectionEmptyOrOneItemGiven = "This collection cannot either be empty or contain only one element";
    public const string CollectionUniqueValues = "Values in collection must be unique";
    public const string DateBeforeEntityCreation = "Date must be after entity creation date";
    public const string DateToBeforeDateFrom = "Date To must be after Date From";
    public const string EntityNotFound = "Entity not found";
    public const string EntityStatusInvalid = "Entity status is invalid";
    public const string FieldNotAllowed = "This field is not allowed to be set for this entity";
    public const string FieldRequired = "This field is required";
    public const string InvalidCodebookRecord = "This field contains an invalid codebook value";
    public const string InvalidRecord = "This field contains an invalid value";
    public const string NumericValueTooLow = "Value must be greater than {0}";
    public const string NumericValueTooHigh = "Value must be less than {0}";
    public const string PropertyCannotBeChanged = "This property cannot be changed by update";
    public const string StringFieldTooLong = "Field exceeded maximum length of {0} characters";
    public const string StringFieldTooShort = "Field did not reach minimum length of {0} characters";
    public const string SubentityAlreadyAssigned = "This entity is already assigned";
    public const string TargetAndSourceMustNotMatch = "Source and target entity cannot be the same";
    public const string UnsupportedCountryForBankCodeType = "This country is not supported for filled bank code type";
    public const string UnsupportedCurrencyForBankAccountType = "This currency is not supported for filled bank account type";
    public const string UnsupportedCurrencyForBankCodeType = "This currency is not supported for filled bank code type";
    public const string UnsupportedCurrencyForCountry = "This currency is not supported for the selected country";
    public const string ValueMustBeGreaterThanOrEqualTo = "Value must be greater than or equal to {0}";
    public const string ValueMustBeLessThanOrEqualTo = "Value must be less than or equal to {0}";
    public const string ValueNotUnique = "Entity with this value already exists";
    public const string ValueMustBeBetweenInclusive = "Value must be between {0} and {1}, including both endpoints.";
    public const string TemplateTypeOrGroupMismatch = "Template type or group does not match the selected template";
    public const string InvalidCategoryAssignment = "Cannot assign this category. The meeting type doesn't match the requirements for this category.";
    public const string FileShouldNotBeEmpty = "Attached file should not be empty.";
    public const string FileTypeShouldBePdf = "Attached file type should be PDF.";
    public const string FileShouldBeValidPdf = "Attached file must be a valid PDF.";
    public const string FileTooLarge = "Attached file size exceeds the maximum allowed size of {0} MB.";
    public const string InvalidYearFormat = "Invalid year format. Value must be a 4-digit number.";
}