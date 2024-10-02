import { Select, Input, Checkbox, CheckboxGroup, Stack } from "@chakra-ui/react";
import { useEffect } from "react";

function HotelFilter({ filter, setFilter }) {
  const ratings = [1, 2, 3, 4, 5];

  const countries = [
    "United States", "Norway", "France", "Germany",
    "Japan", "Canada", "Ukraine", "Denmark",
    "Italy", "Sweden", "Spain", "Turkey",
    "Egypt", "Netherlands", "United Kingdom", "Poland"
  ];

  // Обновляем фильтры при монтировании компонента
  useEffect(() => {
    setFilter((prevFilter) => ({
      ...prevFilter,
      countries: prevFilter.countries || [],
      ratings: prevFilter.ratings || [],
    }));
  }, [setFilter]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFilter({ ...filter, [name]: value });
  };

  return (
    <div className="flex flex-col gap-5">
      <label>Search:</label>
      <Input
        placeholder="..."
        name="search"
        value={filter.search || ""}
        onChange={handleChange}
      />

      <label>Sort by:</label>
      <Select name="sortItem" value={filter.sortItem || ""} onChange={handleChange}>
        <option value="">Default</option>
        <option value="name">Name</option>
        <option value="rating">Rating</option>
      </Select>

      <label>Order by:</label>
      <Select name="sortOrder" value={filter.sortOrder || ""} onChange={handleChange}>
        <option value="desc">Descending</option>
        <option value="asc">Ascending</option>
      </Select>

      <label>Countries:</label>
      <CheckboxGroup
        value={filter.countries}
        onChange={(selectedCountries) => {
          setFilter({
            ...filter,
            countries: selectedCountries
          });
        }}
      >
        <Stack>
          {countries.map((country) => (
            <Checkbox key={country} value={country}>
              {country}
            </Checkbox>
          ))}
        </Stack>
      </CheckboxGroup>

      <label>Ratings:</label>
      <CheckboxGroup
        value={filter.ratings.map(String)}
        onChange={(selectedRatings) => {
          const numericRatings = selectedRatings.map(Number);
          setFilter({
            ...filter,
            ratings: numericRatings 
          });
        }}
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
  );
}

export default HotelFilter;
