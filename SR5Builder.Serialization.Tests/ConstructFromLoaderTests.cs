namespace SR5Builder.Serialization.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;

    public class ConstructFromLoaderTests
    {
        [Fact]
        public void TestAttributeFromLoader()
        {
            SR5Builder.DataModels.Attribute attrib;

            SR5Builder.Loaders.AttributeLoader loader = new Loaders.AttributeLoader();
            loader.BaseRating = 3;
            loader.Book = "Bob";
            loader.Page = 6;
            loader.ImprovedRating = 5;
            loader.Category = "Thingy";
            loader.Name = "Body";
            
        }
    }
}
