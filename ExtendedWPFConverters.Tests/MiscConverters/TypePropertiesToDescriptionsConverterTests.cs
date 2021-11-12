using System.ComponentModel;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class TypePropertiesToDescriptionsConverterTests
    {
        private class TestClass
        {
            [Description("With description")]
            public string WithDescription { get; }
            public string WithoutDescription { get; }
        }
        
        [Fact]
        public void ConvertsTypePropertiesToDescriptions()
        {
            var converter = new TypePropertiesToDescriptionsConverter();
            var result = converter.Convert(typeof(TestClass), typeof(object), null, null);
            
            Assert.Equal(new[] { "With description", nameof(TestClass.WithoutDescription) }, result);
        }
        
        [Fact]
        public void ConvertsTypePropertiesToDescriptionsWithDeclaredDescriptionsOnly()
        {
            var converter = new TypePropertiesToDescriptionsConverter() { GetMembersWithNoDescription = false };
            var result = converter.Convert(typeof(TestClass), typeof(object), null, null);
            
            Assert.Equal(new[] { "With description" }, result);
        }
        
        [Fact]
        public void ConvertsTypePropertiesToDescriptionsWithTitleCase()
        {
            var converter = new TypePropertiesToDescriptionsConverter() { ToTitleCase = true };
            var result = converter.Convert(typeof(TestClass), typeof(object), null, null);
            
            Assert.Equal(new[] { "With description", "Without Description" }, result);
        }
    }
}
