import {
  Box,
  Checkbox,
  CheckboxGroup,
  Heading,
  Stack,
  Text,
  Divider,
  useColorModeValue,
} from "@chakra-ui/react";

function HotelFilters({ filter, setFilter }) {
  const ratings = [1, 2, 3, 4, 5];
  const countries = [
    "United States", "Norway", "France", "Germany",
    "Japan", "Canada", "Ukraine", "Denmark",
    "Italy", "Sweden", "Spain", "Turkey",
    "Egypt", "Netherlands", "United Kingdom", "Poland",
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

  const textColor = useColorModeValue("gray.800", "gray.200");
  const boxBg = useColorModeValue("white", "gray.700");
  const boxShadow = useColorModeValue("md", "dark-lg");
  const borderColor = useColorModeValue("purple.300", "purple.600");

  return (
    <Box
      bg={boxBg}
      p={4}
      rounded="md"
      shadow={boxShadow}
      borderWidth="3px"
      borderColor={borderColor}
      maxW="280px"
    >
      <Heading size="md" mb={4} color={textColor}>
        Filters
      </Heading>

      <Box mb={6}>
        <Heading size="sm" mb={3} color={textColor}>
          Countries:
        </Heading>
        <CheckboxGroup value={filter.countries} onChange={handleCountriesChange}>
          <Stack spacing={2} maxH="240px" overflowY="auto">
            {countries.map((country) => (
              <Checkbox
                key={country}
                value={country}
                colorScheme="purple"
                size="md"
                borderColor="gray.400"
              >
                <Text color={textColor} fontSize="sm">
                  {country}
                </Text>
              </Checkbox>
            ))}
          </Stack>
        </CheckboxGroup>
      </Box>

      <Divider borderColor={borderColor} mb={6} />

      <Box>
        <Heading size="sm" mb={3} color={textColor}>
          Ratings:
        </Heading>
        <CheckboxGroup
          value={filter.ratings.map(String)}
          onChange={handleRatingsChange}
        >
          <Stack spacing={2}>
            {ratings.map((rating) => (
              <Checkbox
                key={rating}
                value={rating.toString()}
                colorScheme="purple"
                size="md"
                borderColor="gray.400"
              >
                <Text color={textColor} fontSize="sm">
                  {rating} ★
                </Text>
              </Checkbox>
            ))}
          </Stack>
        </CheckboxGroup>
      </Box>
    </Box>
  );
}

export default HotelFilters;
