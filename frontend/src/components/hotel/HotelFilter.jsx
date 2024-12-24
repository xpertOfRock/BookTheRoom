import { Checkbox, CheckboxGroup, Stack } from "@chakra-ui/react";

function HotelFilter({ filter, setFilter }) {
  const ratings = [1, 2, 3, 4, 5];
  const countries = [
    "United States", "Norway", "France", "Germany",
    "Japan", "Canada", "Ukraine", "Denmark",
    "Italy", "Sweden", "Spain", "Turkey",
    "Egypt", "Netherlands", "United Kingdom", "Poland"
  ];

  const handleCountryChange = (selectedCountries) => {
    setFilter({ ...filter, countries: selectedCountries });
  };

  const handleRatingChange = (selectedRatings) => {
    const numericRatings = selectedRatings.map(Number);
    setFilter({ ...filter, ratings: numericRatings });
  };

  return (
    <div className="flex flex-col gap-6">
      <div>
        <label className="block text-sm font-medium text-gray-700 mb-2">Countries:</label>
        <CheckboxGroup
          value={filter.countries}
          onChange={handleCountryChange}
        >
          <Stack>
            {countries.map((country) => (
              <Checkbox key={country} value={country}>
                {country}
              </Checkbox>
            ))}
          </Stack>
        </CheckboxGroup>
      </div>
      <div>
        <label className="block text-sm font-medium text-gray-700 mb-2">Ratings:</label>
        <CheckboxGroup
          value={filter.ratings.map(String)}
          onChange={handleRatingChange}
        >
          <Stack>
            {ratings.map((rating) => (
              <Checkbox key={rating} value={rating.toString()}>
                {rating} Stars
              </Checkbox>
            ))}
          </Stack>
        </CheckboxGroup>
      </div>
    </div>
  );
}

export default HotelFilter;
