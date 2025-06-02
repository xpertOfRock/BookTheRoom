import React, { useEffect, useState } from "react";
import ApartmentSortAndSearchFilter from "../../components/apartment/ApartmentSortAndSearchFilter";
import ApartmentFilter from "../../components/apartment/ApartmentFilter";
import ApartmentCard from "../../components/apartment/ApartmentCard";
import { fetchApartments } from "../../services/apartments";
import {
  Box,
  Heading,
  SimpleGrid,
  Stack,
  Flex,
  Button,
  useColorModeValue,
} from "@chakra-ui/react";
import { useNavigate } from "react-router-dom";

function Apartments() {
  const [apartments, setApartments] = useState([]);
  const [sortAndSearch, setSortAndSearch] = useState({
    search: "",
    sortItem: "id",
    sortOrder: "desc",
    itemsCount: 15,
  });
  const [filterBy, setFilterBy] = useState({
    countries: [],
    minPrice: undefined,
    maxPrice: undefined,
  });

  const navigate = useNavigate();

  const bg = useColorModeValue("purple.50", "purple.900");
  const borderColor = useColorModeValue("purple.300", "purple.600");
  const textColor = useColorModeValue("gray.800", "gray.200");

  useEffect(() => {
    const fetchData = async () => {
      try {
        const combinedFilters = { ...sortAndSearch, ...filterBy };
        const response = await fetchApartments(combinedFilters);
        setApartments(response || []);
      } catch (error) {
        console.error("Error fetching apartments:", error);
        setApartments([]);
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
            <ApartmentFilter filter={filterBy} setFilter={setFilterBy} />
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
            <ApartmentSortAndSearchFilter
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
            <Flex justify="space-between" align="center" mb={4}>
              <Heading size="lg" color={textColor}>
                Apartments
              </Heading>
              <Button
                colorScheme="purple"
                onClick={() => navigate("/apartments/create")}
              >
                Create
              </Button>
            </Flex>

            {apartments.length > 0 ? (
              <SimpleGrid columns={{ base: 1, sm: 2, lg: 3 }} spacing={6}>
                {apartments
                  .slice(0, sortAndSearch.itemsCount)
                  .map((apt) => (
                    <ApartmentCard
                      key={apt.id}
                      id={apt.id}
                      name={apt.title}
                      address={apt.address}
                      preview={apt.preview}
                      userScore={apt.userScore}
                      createdAt={apt.createdAt}
                      price={apt.price}
                    />
                  ))}
              </SimpleGrid>
            ) : (
              <Box color="gray.500" textAlign="center" py={10}>
                No apartments found.
              </Box>
            )}
          </Box>
        </Stack>
      </Stack>
    </Box>
  );
}

export default Apartments;
