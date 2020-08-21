# WPF Extended converters
Provides a set of commonly used and tested converters to build WPF applications.

## Notions

> Note: this is important to grasp the notions in order to quickly find a
> suitable converter.

In this package, converters can have multiple usage:
-	**Type conversion**: input is converted from one type to another, optionally using an operation or parameters in the process
	-	Name pattern: *Type1ToType2Converter*
	-	Ex: *BooleanToVisibilityConverter*
-	**Operation application**: **input is operated using a function** that emits the output. Function operation can be parameterized for some converters (ex: MathConverter)
	-	Name pattern: *FunctionNameConverter* or *FunctionDomainNameConverter*
	-	Ex: *CollectionCountConverter*, *MathConverter*
-	**Hybrid usage**: an operation that clearly indicates the output type. 
	-	Name pattern: *FunctionNameToTypeConverter*
	-	Ex: *NonNullOrEmptyStringToVisibilityConverter*

Aside from usage, converters can be classified into 3 families:

- **Single value converters**: to be used with a Binding  **one input gives one output**. They have no suffix in their name.

- **Multi-value converters** to be used with a MultiBinding: **multiple inputs of interest give a single output**. They often display an *'Operation'* parameter that dictates how entries are tied together (ex: *'Or'* operation). Their name pattern is *XXXXConverterForMultibinding*.

- **Multi-value converters with activators** to be used with a MultiBinding but: **one input of interest + activation signals giving a single output**. They indeed all accept booleans as secondary entries and all provide an **ActivationOperation** property that ties these boolean values together. Allowed operation values are common boolean operations: *None*, *Not*, *And*, *Or*, *Xor*, *Nand*, *Nor*, *Xnor* and defaults to *And*. When the operation over the entries outputs the 'true', the input is transferred as output, otherwise **ValueForInvalid** is returned. They have a *XXXXConverterWithActivators* name pattern.

All converters are supplied for one, two or three of the above families. Availability mainly depends on usage relevance (ex: *CollectionIndexOfConverter* cannot be used with a single variable entry since a collection and a value must be provided, so it cannot be a single value converter).

## Quick reference


|Input type|Name|Function|Output type|Single value?|Multi-binding?|With activators?|
|---|---|---|---|---|---|---|
|Boolean|BooleanToBooleanConverter||Boolean|Yes|Yes|No
|Boolean|BooleanToFontWeightConverter||FontWeight|Yes|Yes|No
|Boolean|BooleanToObjectConverter|   |Object|Yes|No|No
|Boolean|BooleanToNumberConverter|   |IComparable|Yes|No|No
|Boolean|BooleanToOpacityConverter|   |Double|Yes|No|No
|Boolean|BooleanToThicknessConverter|   |Thickness|Yes|No|No
|Boolean|BooleanToVisibilityConverter|   |Visibility|Yes|Yes|No
|IEnumerable|CollectionCountConverter|Count()|Int or String|Yes|No|No
|IEnumerable|CollectionFirstItemConverter|First()|Object|Yes|Yes|No
|IEnumerable|CollectionIndexOfConverter|First()|Int or String|No|Yes|No
|Color|ColorToSolidColorBrushConverter|   |SolidColorBrush|Yes|No|No
|Image|ImageToBitmapImageConverter|   |Bitmap|Yes|No|No
|Double|MathConverter|+, -, *, /, %, ^, abs()|Double or String|Yes|Yes|No
|Object|InstanceToTypeConverter||Type|Yes|No|No
|Object|ObjectToObjectConverter||Object|No|No|Yes
|Object|NotNullToVisibilityConverter|!=null|Visibility|Yes|No|No
|String|NotNullOrEmptyStringToBooleanConverter|!=null and !=empty|Bool|Yes|No|No
|String|NotNullOrEmptyStringToVisibilityConverter|!=null and !=empty|Bool|Yes|No|Yes


## How to use
Alway invoke the library in your xaml header definition:

    xmlns:extconv="clr-namespace:EMA.ExtendedWPFConverters;assembly=ExtendedWPFConverters"

Then you can either directly invoke the converter along with its parameters:

    <CheckBox IsChecked="{Binding SomeProperty, Converter={extconv:BooleanToBooleanConverter Operation=Not}}"/>

or (most machine efficient way) declare your converter and its parameters as a static resource:

    <UserControl.Resources>
        <extconv:BooleanToBooleanConverter x:Key="NotBooleanToBooleanConverter" Operation="Not" />
    </UserControl.Resources>
    
    <CheckBox IsChecked="{Binding SomeProperty, Converter={StaticResource NotBooleanToBooleanConverter}}"/>

Find the most suitable converter name using the quick reference and add a suffix when needed (ForMultiBinding or WithActivators).

## Reference

> Note: Suffixes are most of the time not mentioned in the reference. Remind that description and parameters apply to multi-value converters for a given converter description.

> Note: Categories in this reference follow the project source code hierarchy.

### Boolean converters ("BooleanToXXXX")
Converts any boolean entry into a new object type.

**Parameters**
 -  ***ValueForTrue***: to be returned when conversion result is true (after operation if any). Default is set to common 'true' value for the target type (ex: Visible for Visibility)
 -  ***ValueForFalse***: value to be returned when conversion result is false (after operation if any). Default value is often set to the target type's default value (ex: 0.0 for double).
 -  ***ValueForInvalid***: value to be returned when input is not boolean or is null. Default often worth target type's default value too.
 -  ***Operation***: an optional boolean operation to be applied on all inputs and that will give the final converted result. Allowed values depends if the converter is used for single bindings (None, Not) or multi-bindings (None, Not, And, Or, Xor, Nand, etc.). Default is None.
 - 
**Example**

    <TextBlock Text="{Binding SomeBoolProperty, Converter={extconv:BooleanToNumberConverter ValueForTrue=1.0}}"/>
   
**Notes**

Note 1: the conversion parameter overrides **ValueForTrue** when provided. This way, you can quickly personalize converters at the xaml node level while using a static resource. Ex: 

    <Border Visibility="{Binding SomeBoolProperty, Converter={extconv:BooleanToVisibilityConverter}, ConverterParameter={StaticResource MyVisibilityResource}"/>

Note 2: backward conversion is referenced on *ValueForTrue* (or parameter) value. In doubt ( i.e. if *ValueForTrue* = *ValueForFalse*), then 'true' is returned. 

Note 3: Although settable, **ValueForTrue** and **ValueForFalse** cannot be changed on **BooleanToBooleanConverter** for developer experience considerations.

### Collection converters ("CollectionXXXX")

#### CollectionCountConverter
Returns the number of items the input collection contains.

**Parameters**
 -  ***OutputAsString***: sets type of returned value (string or int). Default is false.
 -  ***DefaultCountValue***: to be returned in case the input is not iterable. Default is 0.
 - ***DefaultCountValueString***: to be returned in case the input is not iterable and ***OutputAsString*** is set. Default is "0".

#### CollectionFirstItemConverter
Return the first item of a collection is any (default value otherwise).

#### CollectionFirstItemConverterForMultibinding
Returns the first item of a collection passed as first item or the first item of the multibinding as a whole. Useful to get notified by other binding within the multibinding without impacting the output value.

**Parameters**
 -  ***AsIEnumerable***: indicates if first item of the first input collection must be returned, or if the first input must be returned as a whole. Default is true.

#### CollectionIndexOfConverterForMultiBinding
In a collection given as first input, returns the index of the element passed as a second input.

**Parameters**
 -  ***OutputAsString***: sets type of returned value (string or int). Default is false.
 -  ***ValueForInvalid***: to be returned in case the collection is not iterable. Default is -1.
 -  ***ValueStringForInvalid***: to be returned in case the collection is not iterable. Default is -1.
 -  ***ValueForNotFound***: to be returned in case the collection does not contains the value and ***OutputAsString*** is set. Default is empty string.
 - ***ValueStringForNotFound***: to be returned in case the collection does not contains the value and ***OutputAsString*** is set. Default is empty string.
 
### Color converters ("ColorToXXXXConverter")

#### ColorToSolidColorBrushConverter
Transforms a System.Windows.Media.Color into a System.Windows.Media.SolidColorBrush.

**Parameters:**
 -  ***Default***: Default color to be returned in case color input is not valid. Default is Brushes.Black.

**Example**

    <Border BorderBrush="{Binding SomeColorProperty, Converter={extconv:ColorToSolidColorBrushConverter Default=Brushes.Grey}"/>


### Image converters ("ImageToXXXXConverter")

#### ImageToBitmapImageConverter
Transforms a System.Drawing.Image into a System.Windows.Media.Imaging.BitmapImage.

**Example**

    <Image Source="{Binding SomeImage, Converter={extconv:ImageToBitmapImageConverter}}"/>

### Math converters ("MathConverter")
Performs a mathematical operation over a single input and the converter parameter (*MathConverter*) or over several inputs (*MathConverterForMultibinding*) and returns the result.

**Parameters**
 -  ***Operation***: mathematical operation to be applied on the inputs
	 - Add: sums input values up
	 - Substract: substracts input values
	 - SubstractPositiveOnly: subtracts input values but cannot go negative (bottoms at 0)
	 - Multiply: product of the input values
	 - Divide: division of the input values, returns PositiveInfinity or NegativeInfinity if encounters a division by zero
	 - Modulo: applies sequentially the modulo operation on all inputs
	 - Power: sequentially raises result to the power of next input
	 - Absolute: returns the absolute value of the first input
 -  ***ValueForInvalid***: to be returned in case inputs are not numbers. Default is *Binding.DoNothing*.
 -  ***OutputAsString***: sets type of returned value (string or int). Default is false.

**Example**

    <RotateTransform Angle="{Binding SomeRadValue, Converter={extconv:MathConverter, Operation=Multiply}, ConverterParameter=57.2958}"/>

**About**
Originally inspired by [MaterialDesignInXamlToolkit](https://github.com/MaterialDesignInXAML/MaterialDesignInXamlToolkit)'s MathConverter

### Misc. converters 

#### InstanceToTypeConverter
Returns the type of the input or null if null.

#### ObjectToObjectConverterWithActivators
Returns the input only if activators allow for it.

**Parameters**
See **'Multi-value converters with activators'** description on top of this documentation.

### 'Not null' converters (NotNullToXXXXConverter)

#### NotNullToVisibilityConverter
Converts the input null state into a Visibility value.

**Parameters**
 -  ***ValueForNull***: value to be returned when the input is null. Default is Visibility.Collapsed.
 -  ***ValueForNotNull***: value to be returned when the input is not null. Default is Visibility.Visible.

**Example**

    <ContentControl Visibility="{Binding SomeViewModel, Converter={extconv:NotNullToVisibilityConverter ValueForNull=Visibility.Hidden}}" />

### String converters (NotNullOrEmptyStringToXXXXConverter)
Returns a result depending on the state of a passed string (if null or empty).

**Parameters**
 -  ***ValueForNullOrEmpty***: value to be returned when the input string is null or empty. Default is Visibility.Collapsed.
 -  ***ValueForNotNullOrEmpty***: value to be returned when the input is not null nor empty. Default is Visibility.Visible.

**Example**

    <TextBox Text="{Binding SomeText}" Visibility="{Binding SomeText, Converter={extconv:NotNullOrEmptyStringToVisibilityConverter}}" />

## That's it!
Hope you will enjoy these converters!