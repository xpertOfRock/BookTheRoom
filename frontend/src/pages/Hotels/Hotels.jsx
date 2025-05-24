import React, { useEffect, useState } from "react";
import HotelSortAndSearchFilter from "../../components/hotel/subcomponents/Hotels/HotelSortAndSearchFilter";
import HotelFilters from "../../components/hotel/subcomponents/Hotels/HotelFilters";
import Card from "../../components/hotel/subcomponents/Hotels/Card";
import { fetchHotels } from "../../services/hotels";
import {
  Box,
  Heading,
  SimpleGrid,
  VStack,
  Stack,
  useColorModeValue,
} from "@chakra-ui/react";

function Hotels() {
  const [hotels, setHotels] = useState([]);
  const [sortAndSearch, setSortAndSearch] = useState({
    search: "",
    sortItem: "id",
    sortOrder: "desc",
    itemsCount: 15,
  });

  const [filterBy, setFilterBy] = useState({
    countries: [],
    ratings: [],
  });

  const bg = useColorModeValue("purple.50", "purple.900");
  const borderColor = useColorModeValue("purple.300", "purple.600");
  const textColor = useColorModeValue("gray.800", "gray.200");

  useEffect(() => {
    const fetchData = async () => {
      try {
        const combinedFilters = { ...sortAndSearch, ...filterBy };
        const response = await fetchHotels(combinedFilters);
        setHotels(response || []);
      } catch (error) {
        console.error("Error fetching hotels:", error);
        setHotels([]);
      }
    };
    fetchData();
  }, [sortAndSearch, filterBy]);

  return (
    <Box bg="white" minH="100vh" p={{ base: 4, md: 8 }}>
      <Stack
        maxW="7xl"
        mx="auto"
        spacing={8}
        direction={{ base: "column", md: "row" }}
        align={{ md: "flex-start" }}
      >
        <Stack
          spacing={6}
          w={{ base: "full", md: "320px" }}
          order={{ base: 0, md: 0 }}
        >
          <Box
            bg={bg}
            p={6}
            rounded="lg"
            shadow="md"
            borderWidth="3px"
            borderColor={borderColor}
          >
            <Heading size="lg" mb={6} color={textColor}>
              Filters
            </Heading>
            <HotelFilters filter={filterBy} setFilter={setFilterBy} />
          </Box>
        </Stack>

        <Stack
          spacing={6}
          w={{ base: "full", md: "calc(100% - 320px)" }}
          order={{ base: 1, md: 1 }}
        >
          <Box
            bg={bg}
            p={6}
            rounded="lg"
            shadow="md"
            borderWidth="3px"
            borderColor={borderColor}
          >
            <HotelSortAndSearchFilter
              filter={sortAndSearch}
              setFilter={setSortAndSearch}
            />
          </Box>

          <Box
            bg={bg}
            p={6}
            rounded="lg"
            shadow="md"
            borderWidth="3px"
            borderColor={borderColor}
          >
            <Heading size="lg" mb={4} color={textColor}>
              Hotels
            </Heading>
            {hotels.length > 0 ? (
              <SimpleGrid columns={{ base: 1, sm: 2, lg: 3 }} spacing={6}>
                {hotels
                  .slice(0, sortAndSearch.itemsCount)
                  .map((hotel) => (
                    <Card
                      key={hotel.id}
                      id={hotel.id}
                      name={hotel.name}
                      preview={hotel.preview}
                      rating={hotel.rating}
                      address={hotel.address}
                      userScore={hotel.userScore}
                    />
                  ))}
              </SimpleGrid>
            ) : (
              <Box color="gray.500" textAlign="center" py={10}>
                No hotels found.
              </Box>
            )}
          </Box>
        </Stack>
      </Stack>
    </Box>
  );
}

export default Hotels;
