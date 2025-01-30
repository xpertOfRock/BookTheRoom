import { Checkbox, CheckboxGroup } from "@chakra-ui/react";

function Filters({ filter, setFilter }) {
  const ratings = [1, 2, 3, 4, 5];

  const countries = [
    "United States", "Norway", "France", "Germany",
    "Japan", "Canada", "Ukraine", "Denmark",
    "Italy", "Sweden", "Spain", "Turkey",
    "Egypt", "Netherlands", "United Kingdom", "Poland"
  ];

  const handleCountriesChange = (selectedCountries) => {
    setFilter({
      ...filter,
      countries: selectedCountries,
    });
  };

  const handleRatingsChange = (selectedRatings) => {
    const numericRatings = selectedRatings.map(Number);
    setFilter({
      ...filter,
      ratings: numericRatings,
    });
  };

  return (
    <div className="grid-cols-2 lg:flex-row gap-6 sm:grid-cols-2 flex flex-col ">
      <div>
        <h3 className="text-lg font-semibold mb-2">Countries:</h3>
        <CheckboxGroup
          value={filter.countries}
          onChange={handleCountriesChange}
        >
          <div className="flex flex-col gap-2">
            {countries.map((country) => (
              <label key={country} className="flex items-center gap-2 text-sm">
                <Checkbox
                  value={country}
                  className="border border-gray-400 rounded-md accent-blue-600 hover:accent-blue-700"
                />
                <span className="text-gray-800">{country}</span>
              </label>
            ))}
          </div>
        </CheckboxGroup>
      </div>

      <div>
        <h3 className="text-lg font-semibold mb-2">Ratings:</h3>
        <CheckboxGroup
          value={filter.ratings.map(String)}
          onChange={handleRatingsChange}
        >
          <div className="flex flex-col gap-2">
            {ratings.map((rating) => (
              <label key={rating} className="flex items-center gap-2 text-sm">
                <Checkbox
                  value={rating.toString()}
                  className="border border-gray-400 rounded-md accent-blue-600 hover:accent-blue-700"
                />
                <span className="text-gray-800">{rating} ★</span>
              </label>
            ))}
          </div>
        </CheckboxGroup>
      </div>
    </div>
  );
}

export default Filters;
