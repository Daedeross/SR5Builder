namespace SR5Builder.Serialization.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Xunit;
    using Newtonsoft.Json;
    using Loaders;

    public class JsonTests
    {
        [Fact]
        public void TestCharacterSerialization()
        {
            var character = new CharacterLoader();
            character.Name = "Bob";
            character.Metatype = "Human";
            character.SpecialChoice = "Magician";
            character.Attributes.Add("Body", new AttributeLoader
            {
                Name = "Body",
                BaseRating = 2,
                ImprovedRating = 4
            });

            string output = JsonConvert.SerializeObject(character);
            System.Diagnostics.Debug.WriteLine(output);

            var character2 = JsonConvert.DeserializeObject<CharacterLoader>(output);

            Assert.Equal(character.Name, character2.Name);
            Assert.Equal(character.Metatype, character2.Metatype);
            Assert.Equal(character.SpecialChoice, character2.SpecialChoice);
            Assert.Equal(character.Attributes.Count, character2.Attributes.Count);

            foreach (var key in character.Attributes.Keys)
            {
                Assert.True(character2.Attributes.ContainsKey(key));
                Assert.Equal(character.Attributes[key].Name, character2.Attributes[key].Name);
                Assert.Equal(character.Attributes[key].BaseRating, character2.Attributes[key].BaseRating);
                Assert.Equal(character.Attributes[key].ImprovedRating, character2.Attributes[key].ImprovedRating);
            }
        }
    }
}
