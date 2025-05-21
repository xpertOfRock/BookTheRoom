import { Box, Checkbox, CheckboxGroup, Heading, Stack, Input, VStack } from "@chakra-ui/react";

function ApartmentFilter({ filter, setFilter }) {
  const countries = [
    "United States", "Norway", "France", "Germany",
    "Japan", "Canada", "Ukraine", "Denmark",
    "Italy", "Sweden", "Spain", "Turkey",
    "Egypt", "Netherlands", "United Kingdom", "Poland"
  ];

  const handleCountriesChange = (selected) => {
    setFilter({ ...filter, countries: selected });
  };

  const handleMinPriceChange = (e) => {
    const v = e.target.value;
    setFilter({ ...filter, minPrice: v === "" ? undefined : parseFloat(v) });
  };

  const handleMaxPriceChange = (e) => {
    const v = e.target.value;
    setFilter({ ...filter, maxPrice: v === "" ? undefined : parseFloat(v) });
  };

  return (
    <VStack spacing={6} align="stretch">
      <Box bg="white" p={4} borderRadius="md" boxShadow="md">
        <Heading size="md" mb={3}>Countries</Heading>
        <CheckboxGroup value={filter.countries} onChange={handleCountriesChange}>
          <Stack spacing={3} maxH="240px" overflowY="auto">
            {countries.map((country) => (
              <Checkbox
                key={country}
                value={country}
                size="md"
                colorScheme="blue"
              >
                {country}
              </Checkbox>
            ))}
          </Stack>
        </CheckboxGroup>
      </Box>

      <Box bg="white" p={4} borderRadius="md" boxShadow="md">
        <Heading size="md" mb={3}>Price Range</Heading>
        <Stack direction={["column", "row"]} spacing={4}>
          <Input
            type="number"
            placeholder="Min Price"
            value={filter.minPrice ?? ""}
            onChange={handleMinPriceChange}
            bg="gray.50"
            size="md"
            borderRadius="md"
          />
          <Input
            type="number"
            placeholder="Max Price"
            value={filter.maxPrice ?? ""}
            onChange={handleMaxPriceChange}
            bg="gray.50"
            size="md"
            borderRadius="md"
          />
        </Stack>
      </Box>
    </VStack>
  );
}

export default ApartmentFilter;