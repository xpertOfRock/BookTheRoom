import React, { useEffect } from "react";
import {
  Box,
  SimpleGrid,
  FormControl,
  FormLabel,
  Input,
  Select,
  Button,
  Checkbox,
  Stack,
  Popover,
  PopoverTrigger,
  PopoverContent,
  PopoverArrow,
  PopoverCloseButton,
  PopoverBody,
  PopoverHeader,
  useColorModeValue,
  Flex,
  useToast,
} from "@chakra-ui/react";

const defaultCheckIn = new Date(Date.now() + 86400000).toISOString().split("T")[0];
const defaultCheckOut = new Date(Date.now() + 172800000).toISOString().split("T")[0];

function RoomsFilter({ filter, setFilter, onApplyFilters }) {
  const borderColor = useColorModeValue("purple.300", "purple.600");
  const bgColor = useColorModeValue("purple.50", "purple.900");
  const textColor = useColorModeValue("gray.800", "gray.200");
  const toast = useToast();

  useEffect(() => {
    if (!filter.checkIn) setFilter((prev) => ({ ...prev, checkIn: defaultCheckIn }));
    if (!filter.checkOut) setFilter((prev) => ({ ...prev, checkOut: defaultCheckOut }));
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;

    if (name === "minPrice") {
      if (value === "") return setFilter({ ...filter, minPrice: "" });
      let newMin = parseFloat(value);
      if (isNaN(newMin) || newMin < 0) newMin = 0;
      let currentMax = parseFloat(filter.maxPrice || 0);
      if (currentMax < newMin && currentMax !== 0) currentMax = newMin + 0.01;
      setFilter({ ...filter, minPrice: newMin, maxPrice: currentMax });
      return;
    }

    if (name === "maxPrice") {
      if (value === "") return setFilter({ ...filter, maxPrice: "" });
      let newMax = parseFloat(value);
      if (isNaN(newMax) || newMax < 0) newMax = 0;
      let currentMin = parseFloat(filter.minPrice || 0);
      if (newMax !== 0 && newMax < currentMin) {
        toast({
          title: "Invalid price range",
          description: "Maximum price cannot be less than minimum price.",
          status: "error",
          duration: 4000,
          isClosable: true,
          position: "top-right",
        });
        return;
      }
      setFilter({ ...filter, maxPrice: newMax });
      return;
    }

    if (name === "checkIn") {
      const newCheckInDate = new Date(value);
      const currentCheckOutDate = new Date(filter.checkOut);
      if (newCheckInDate >= currentCheckOutDate) {
        const nextDay = new Date(newCheckInDate);
        nextDay.setDate(nextDay.getDate() + 1);
        setFilter({
          ...filter,
          checkIn: value,
          checkOut: nextDay.toISOString().split("T")[0],
        });
        return;
      }
    }

    setFilter({ ...filter, [name]: value });
  };

  const handleCategoryChange = (categoryId, isChecked) => {
    const updatedCategories = isChecked
      ? [...filter.categories, categoryId]
      : filter.categories.filter((id) => id !== categoryId);
    setFilter({ ...filter, categories: updatedCategories });
  };

  const getMinCheckInDate = () => defaultCheckIn;
  const getMinCheckOutDate = () => {
    if (filter.checkIn) {
      const date = new Date(filter.checkIn);
      date.setDate(date.getDate() + 1);
      return date.toISOString().split("T")[0];
    }
    return defaultCheckOut;
  };

  return (
    <Box bg={bgColor} p={6} rounded="lg" shadow="md" color={textColor} borderWidth={2} borderColor={borderColor}>
      <Flex mb={6} justify="center" fontWeight="semibold" fontSize="xl" color={useColorModeValue("purple.700", "purple.300")}>
        Available Rooms
      </Flex>

      <SimpleGrid columns={{ base: 1, md: 3 }} spacing={6} mb={6}>
        <FormControl>
          <FormLabel>Search by Name</FormLabel>
          <Input
            name="search"
            value={filter.search || ""}
            onChange={handleChange}
            placeholder="Enter room name"
            borderColor={borderColor}
            focusBorderColor="purple.400"
          />
        </FormControl>

        <FormControl>
          <FormLabel>Sort By</FormLabel>
          <Select
            name="sortItem"
            value={filter.sortItem || "name"}
            onChange={handleChange}
            borderColor={borderColor}
            focusBorderColor="purple.400"
          >
            <option value="name">Name</option>
            <option value="price">Price</option>
            <option value="number">Room Number</option>
          </Select>
        </FormControl>

        <FormControl>
          <FormLabel>Sort Order</FormLabel>
          <Select
            name="sortOrder"
            value={filter.sortOrder || "asc"}
            onChange={handleChange}
            borderColor={borderColor}
            focusBorderColor="purple.400"
          >
            <option value="asc">Ascending</option>
            <option value="desc">Descending</option>
          </Select>
        </FormControl>
      </SimpleGrid>

      <Popover placement="bottom-start">
        <PopoverTrigger>
          <Button colorScheme="purple" variant="outline" borderColor={borderColor} mb={6} w="fit-content">
            Categories
          </Button>
        </PopoverTrigger>
        <PopoverContent borderColor={borderColor}>
          <PopoverArrow />
          <PopoverCloseButton />
          <PopoverHeader fontWeight="semibold" borderBottomWidth={1}>
            Choose categories
          </PopoverHeader>
          <PopoverBody>
            <Stack spacing={3}>
              {[
                { id: "1", label: "One Bed Apartments" },
                { id: "2", label: "Two Bed Apartments" },
                { id: "3", label: "Three Bed Apartments" },
                { id: "4", label: "Luxury" },
              ].map((category) => (
                <Checkbox
                  key={category.id}
                  value={category.id}
                  isChecked={filter.categories.includes(category.id)}
                  onChange={(e) => handleCategoryChange(category.id, e.target.checked)}
                  color={textColor}
                >
                  {category.label}
                </Checkbox>
              ))}
            </Stack>
          </PopoverBody>
        </PopoverContent>
      </Popover>

      <SimpleGrid columns={{ base: 1, md: 2 }} spacing={6} mb={6}>
        <FormControl>
          <FormLabel>Min Price</FormLabel>
          <Input
            type="text"
            name="minPrice"
            value={filter.minPrice ?? ""}
            onChange={handleChange}
            placeholder="Minimum price"
            borderColor={borderColor}
            focusBorderColor="purple.400"
          />
        </FormControl>

        <FormControl>
          <FormLabel>Max Price</FormLabel>
          <Input
            type="text"
            name="maxPrice"
            value={filter.maxPrice ?? ""}
            onChange={handleChange}
            placeholder="Maximum price"
            borderColor={borderColor}
            focusBorderColor="purple.400"
          />
        </FormControl>
      </SimpleGrid>

      <SimpleGrid columns={{ base: 1, md: 2 }} spacing={6} mb={6}>
        <FormControl>
          <FormLabel>Check-In Date</FormLabel>
          <Input
            type="date"
            name="checkIn"
            value={filter.checkIn || ""}
            onChange={handleChange}
            min={getMinCheckInDate()}
            borderColor={borderColor}
            focusBorderColor="purple.400"
          />
        </FormControl>

        <FormControl>
          <FormLabel>Check-Out Date</FormLabel>
          <Input
            type="date"
            name="checkOut"
            value={filter.checkOut || ""}
            onChange={handleChange}
            min={getMinCheckOutDate()}
            borderColor={borderColor}
            focusBorderColor="purple.400"
          />
        </FormControl>
      </SimpleGrid>

      <Flex justify="center">
        <Button colorScheme="purple" size="lg" px={12} onClick={onApplyFilters} _hover={{ bg: "purple.600" }}>
          Apply Filters
        </Button>
      </Flex>
    </Box>
  );
}

export default RoomsFilter;