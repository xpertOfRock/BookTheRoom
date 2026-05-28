using Core.ValueObjects;

namespace Tests.CoreTests
{
    public class AddressTests
    {
        private static Address CreateSampleAddress() =>
            new Address(
                country: "Ukraine",
                state: "Lviv Oblast",
                city: "Lviv",
                street: "Svobody Ave 1",
                postalCode: "79000");

        [Fact]
        public void Constructor_AssignsAllProperties()
        {
            var address = CreateSampleAddress();

            Assert.Equal("Ukraine", address.Country);
            Assert.Equal("Lviv Oblast", address.State);
            Assert.Equal("Lviv", address.City);
            Assert.Equal("Svobody Ave 1", address.Street);
            Assert.Equal("79000", address.PostalCode);
        }

        [Fact]
        public void ToString_FullForm_ContainsAllComponents()
        {
            var address = CreateSampleAddress();

            var result = address.ToString(false);

            Assert.Equal("Ukraine, Lviv Oblast, Lviv, Svobody Ave 1, 79000", result);
        }

        [Fact]
        public void ToString_ShortForm_ContainsOnlyCountryAndCity()
        {
            var address = CreateSampleAddress();

            var result = address.ToString(true);

            Assert.Equal("Ukraine, Lviv", result);
        }

        [Fact]
        public void ToString_ShortForm_DiffersFromFullForm()
        {
            var address = CreateSampleAddress();

            Assert.NotEqual(address.ToString(true), address.ToString(false));
        }

        [Fact]
        public void AsJson_ProducesJsonContainingAllFields()
        {
            var address = CreateSampleAddress();

            var json = Address.AsJson(address);

            Assert.Contains("\"Country\":\"Ukraine\"", json);
            Assert.Contains("\"State\":\"Lviv Oblast\"", json);
            Assert.Contains("\"City\":\"Lviv\"", json);
            Assert.Contains("\"Street\":\"Svobody Ave 1\"", json);
            Assert.Contains("\"PostalCode\":\"79000\"", json);
        }

        [Theory]
        [InlineData("", "Lviv")]
        [InlineData("Ukraine", "")]
        public void ToString_ShortForm_HandlesEmptyComponentsWithoutThrowing(string country, string city)
        {
            var address = new Address(country, "State", city, "Street", "00000");

            var result = address.ToString(true);

            Assert.Equal($"{country}, {city}", result);
        }
    }
}
