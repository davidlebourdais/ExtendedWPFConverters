# Changelog

Added new converters:
- EqualityToBooleanConverter
- EqualityToVisibilityConverter

## v2.0.3
Added new converters:
- CamelCaseStringToTitleStringConverter
- CornerRadiusToDoubleConverter
- EnumMembersToDescriptionsConverter
- EnumValueToDescriptionConverter
- ThicknessToDoubleConverter
- TypePropertiesToDescriptionsConverter

## v2.0.2
Updated to .NET 5
Minor API breaking changes: 
- BooleanOperation.Xnor -> BooleanOperation.XNor
- MathOperation.Substract -> MathOperation.Subtract
- MathOperation.SubstractPositiveOnly -> MathOperation.SubtractPositiveOnly

## v2.0.1
Updated minimum .Net Core version and package info for better visibility on Nuget feeds.

## v2.0.0
Initial version for public release, comprising the following list of unit tested converters:
- BooleanToBooleanConverter
- BooleanToBooleanConverterForMultibinding
- BooleanToFontWeightConverter
- BooleanToFontWeightConverterForMultibinding
- BooleanToObjectConverter
- BooleanToNumberConverter
- BooleanToOpacityConverter
- BooleanToThicknessConverter
- BooleanToVisibilityConverter
- BooleanToVisibilityConverterForMultibinding
- CollectionCountConverter
- CollectionFirstItemConverter
- CollectionFirstItemConverterForMultibinding
- CollectionIndexOfConverterForMultibinding
- ColorToSolidColorBrushConverter
- ImageToBitmapImageConverter
- MathConverter
- MathConverterForMultibinding
- InstanceToTypeConverter
- ObjectToObjectConverterWithActivators
- NotNullToVisibilityConverter
- NotNullOrEmptyStringToBooleanConverter
- NotNullOrEmptyStringToVisibilityConverter
- NotNullOrEmptyStringToVisibilityConverterForMultibinding

Documentation for this can be found on the archived [README](https://github.com/davidlebourdais/ExtendedWPFConverters/blob/v2.0.0/README.md).
