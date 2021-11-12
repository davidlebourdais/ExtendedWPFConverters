using System.ComponentModel;
using Xunit;

namespace EMA.ExtendedWPFConverters.Tests
{
    public class EnumMembersToDescriptionsConverterTests
    {
        private enum TestEnum
        {
            [Description("With description")]
            WithDescription,
            WithoutDescription
        }
        
        [Fact]
        public void ConvertsEnumMembersToDescriptions()
        {
            var converter = new EnumMembersToDescriptionsConverter();
            var result = converter.Convert(typeof(TestEnum), typeof(object), null, null);
            
            Assert.Equal(new[] { "With description", nameof(TestEnum.WithoutDescription) }, result);
        }
        
        [Fact]
        public void ConvertsEnumMembersToDescriptionsBasedOnValue()
        {
            const TestEnum value = TestEnum.WithDescription;
            
            var converter = new EnumMembersToDescriptionsConverter();
            var result = converter.Convert(value, typeof(object), null, null);
            
            Assert.Equal(new[] { "With description", nameof(TestEnum.WithoutDescription) }, result);
        }
        
        [Fact]
        public void ConvertsEnumMembersToDescriptionsWithDeclaredDescriptionsOnly()
        {
            var converter = new EnumMembersToDescriptionsConverter { GetMembersWithNoDescription = false };
            var result = converter.Convert(typeof(TestEnum), typeof(object), null, null);
            
            Assert.Equal(new[] { "With description" }, result);
        }
        
        [Fact]
        public void ConvertsEnumMembersToDescriptionsWithTitleCase()
        {
            var converter = new EnumMembersToDescriptionsConverter { ToTitleCase = true };
            var result = converter.Convert(typeof(TestEnum), typeof(object), null, null);
            
            Assert.Equal(new[] { "With description", "Without Description" }, result);
        }
        
        [Fact]
        public void DoesNotConvertsEnumMembersToDescriptionsIfTypeIsNotEnum()
        {
            var converter = new EnumMembersToDescriptionsConverter();
            var result = converter.Convert(typeof(char), typeof(object), null, null);
            
            Assert.Null(result);
        }
    }
}
