import React, { useEffect } from "react";
import {
  Box,
  Flex,
  Text,
  Input,
  Select,
  Button,
  Stack,
  FormControl,
  FormLabel,
  Checkbox,
  Popover,
  PopoverTrigger,
  PopoverContent,
  PopoverArrow,
  PopoverCloseButton,
  PopoverBody,
  PopoverHeader,
} from "@chakra-ui/react";

const defaultCheckIn = new Date(Date.now() + 86400000).toISOString().split("T")[0];
const defaultCheckOut = new Date(Date.now() + 172800000).toISOString().split("T")[0];

function RoomsFilter({ filter, setFilter, onApplyFilters }) {
  useEffect(() => {
    if (!filter.checkIn) {
      setFilter((prev) => ({ ...prev, checkIn: defaultCheckIn }));
    }
    if (!filter.checkOut) {
      setFilter((prev) => ({ ...prev, checkOut: defaultCheckOut }));
    }
  }, []);

  const handleChange = (e) => {
    let { name, value } = e.target;

    if (name === "minPrice") {
      let newMin = parseInt(value, 10);
      if (isNaN(newMin) || newMin < 0) {
        newMin = 0;
      }
      let currentMax = parseInt(filter.maxPrice || 0, 10);
      if (currentMax < newMin) {
        currentMax = newMin + 1;
      }
      setFilter({
        ...filter,
        minPrice: newMin,
        maxPrice: currentMax,
      });
      return;
    }

    if (name === "maxPrice") {
      let newMax = parseInt(value, 10);
      if (isNaN(newMax) || newMax < 0) {
        newMax = 0;
      }
      let currentMin = parseInt(filter.minPrice || 0, 10);
      if (newMax < currentMin) {
        newMax = currentMin + 1;
      }
      setFilter({
        ...filter,
        maxPrice: newMax,
      });
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
      const checkInDate = new Date(filter.checkIn);
      checkInDate.setDate(checkInDate.getDate() + 1);
      return checkInDate.toISOString().split("T")[0];
    }
    return defaultCheckOut;
  };

  return (
    <Box className="bg-indigo-100 p-6 text-gray-700 rounded-lg shadow-md">
      <Text
        fontSize="xl"
        mb={4}
        fontWeight="semibold"
        className="text-gray-700 text-center"
      >
        Available Rooms
      </Text>

      <Box className="rounded-lg shadow-md border border-indigo-300 p-4 mb-6">
        <Flex
          wrap="wrap"
          gap={4}
          justify="center"
          align="center"
        >
          <FormControl maxW="200px">
            <FormLabel className="text-gray-700" fontSize="sm">
              Search:
            </FormLabel>
            <Input
              type="text"
              name="search"
              value={filter.search || ""}
              onChange={handleChange}
              placeholder="Search by name"
              border="1px"
              borderColor="indigo.300"
              className="text-gray-700 border border-indigo-300 rounded"
            />
          </FormControl>

          <FormControl maxW="200px">
            <FormLabel className="text-gray-700" fontSize="sm">
              Sort By:
            </FormLabel>
            <Select
              name="sortItem"
              value={filter.sortItem || "name"}
              onChange={handleChange}
              border="1px"
              borderColor="indigo.300"
              className="text-gray-700 border border-indigo-300 rounded"
            >
              <option value="name">Name</option>
              <option value="price">Price</option>
              <option value="number">Room Number</option>
            </Select>
          </FormControl>

          <FormControl maxW="200px">
            <FormLabel className="text-gray-700" fontSize="sm">
              Sort Order:
            </FormLabel>
            <Select
              name="sortOrder"
              value={filter.sortOrder || "asc"}
              onChange={handleChange}
              border="1px"
              borderColor="indigo.300"
              className="text-gray-700 border border-indigo-300 rounded"
            >
              <option value="asc">Ascending</option>
              <option value="desc">Descending</option>
            </Select>
          </FormControl>

          <Box>
            <Popover>
              <PopoverTrigger>
                <Button
                  colorScheme="purple"
                  size="sm"
                  variant="outline"
                  className="border border-indigo-300"
                >
                  Categories
                </Button>
              </PopoverTrigger>
              <PopoverContent
                borderColor="indigo.300"
                className="border border-indigo-300"
              >
                <PopoverArrow />
                <PopoverCloseButton />
                <PopoverHeader className="font-semibold text-gray-700 border-b-indigo-300">
                  Choose categories
                </PopoverHeader>
                <PopoverBody>
                  <Stack direction="column" spacing={2}>
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
                        onChange={(e) =>
                          handleCategoryChange(category.id, e.target.checked)
                        }
                        className="text-gray-700"
                      >
                        {category.label}
                      </Checkbox>
                    ))}
                  </Stack>
                </PopoverBody>
              </PopoverContent>
            </Popover>
          </Box>
        </Flex>
      </Box>

      <Box className="rounded-lg shadow-md border border-indigo-300 p-4 mb-6">
        <Flex
          wrap="wrap"
          gap={4}
          justify="center"
          align="center"
        >
          <FormControl maxW="200px">
            <FormLabel className="text-gray-700" fontSize="sm">
              Min Price:
            </FormLabel>
            <Input
              type="number"
              name="minPrice"
              value={filter.minPrice ?? ""}
              onChange={handleChange}
              placeholder="Min Price"
              border="1px"
              borderColor="indigo.300"
              className="text-gray-700 border border-indigo-300 rounded"
            />
          </FormControl>

          <FormControl maxW="200px">
            <FormLabel className="text-gray-700" fontSize="sm">
              Max Price:
            </FormLabel>
            <Input
              type="number"
              name="maxPrice"
              value={filter.maxPrice ?? ""}
              onChange={handleChange}
              placeholder="Max Price"
              border="1px"
              borderColor="indigo.300"
              className="text-gray-700 border border-indigo-300 rounded"
            />
          </FormControl>
        </Flex>
      </Box>

      <Box className="rounded-lg shadow-md border border-indigo-300 p-4 mb-6">
        <Flex
          wrap="wrap"
          gap={4}
          justify="center"
          align="center"
        >
          <FormControl maxW="200px">
            <FormLabel className="text-gray-700" fontSize="sm">
              Check-In:
            </FormLabel>
            <Input
              type="date"
              name="checkIn"
              value={filter.checkIn || ""}
              onChange={handleChange}
              min={getMinCheckInDate()}
              border="1px"
              borderColor="indigo.300"
              className="text-gray-700 border border-indigo-300 rounded"
            />
          </FormControl>

          <FormControl maxW="200px">
            <FormLabel className="text-gray-700" fontSize="sm">
              Check-Out:
            </FormLabel>
            <Input
              type="date"
              name="checkOut"
              value={filter.checkOut || ""}
              onChange={handleChange}
              min={getMinCheckOutDate()}
              border="1px"
              borderColor="indigo.300"
              className="text-gray-700 border border-indigo-300 rounded"
            />
          </FormControl>
        </Flex>
      </Box>

      <Box textAlign="center">
        <Button
          onClick={onApplyFilters}
          colorScheme="purple"
          className="text-white"
        >
          Apply Filters
        </Button>
      </Box>
    </Box>
  );
}

export default RoomsFilter;