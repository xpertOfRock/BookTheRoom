import {
  Accordion,
  AccordionItem,
  AccordionButton,
  AccordionPanel,
  AccordionIcon,
  Box,
  Flex,
  Text,
  Button,
  useColorModeValue,
} from "@chakra-ui/react";
import ApartmentFilter from "../../components/apartment/ApartmentFilter";
import ApartmentSortAndSearchFilter from "../../components/apartment/ApartmentSortAndSearchFilter";
import { useNavigate } from "react-router-dom";

const UserApartments = ({ aptFilter, setAptFilter, apartments, formatDate }) => {
  const bg = useColorModeValue("purple.100", "purple.100");
  const filterBg = useColorModeValue("purple.50", "purple.900");
  const accentColor = "purple.300";
  const mainColor = "purple.600";
  const navigate = useNavigate();

  return (
    <Accordion allowMultiple defaultIndex={[0]}>
      <AccordionItem>
        <AccordionButton borderRadius="md" color={mainColor} _expanded={{ bg: mainColor, color: "white" }}>
          <Box flex="1" textAlign="left" fontSize="lg" fontWeight="bold">
            Apartments
          </Box>
          <AccordionIcon />
        </AccordionButton>
        <AccordionPanel pb={4}>
          <Box mb={4} bg={filterBg} border="1px solid" borderColor={accentColor} rounded="md" p={4}>
            <ApartmentSortAndSearchFilter filter={aptFilter} setFilter={setAptFilter} />
            <br />
            <ApartmentFilter filter={aptFilter} setFilter={setAptFilter} />
          </Box>
          <Box maxH="300px" overflowY="auto" border="1px solid" borderColor={accentColor} rounded="md" p={3}>
            {apartments.map((apt) => (
              <Flex
                key={apt.id}
                mb={4}
                p={3}
                bg={bg}
                rounded="md"
                boxShadow="sm"
                align="center"
                justify="space-between"
              >
                <Box>
                  <Text fontWeight="bold">{apt.title}</Text>
                  <Text fontSize="sm">{apt.address}</Text>
                  <Text fontSize="sm">Price per night: {apt.price}$</Text>
                  <Text fontSize="sm">Created: {formatDate(apt.createdAt)}</Text>
                </Box>
                <Button size="sm" colorScheme="purple" onClick={() => navigate(`/apartments/${apt.id}`)}>
                  View
                </Button>
              </Flex>
            ))}
          </Box>
          <Flex justify="space-between" align="center" mt={2}>
            <Button
              size="sm"
              colorScheme="purple"          
              variant="outline"
              disabled={aptFilter.page <= 1}
              onClick={() => setAptFilter({ ...aptFilter, page: aptFilter.page - 1 })}
            >
              Prev
            </Button>
            <Text>Page {aptFilter.page}</Text>
            <Button 
              size="sm"
              colorScheme="purple"          
              variant="outline"
              onClick={() => setAptFilter({ ...aptFilter, page: aptFilter.page + 1 })}>
              Next
            </Button>
          </Flex>
        </AccordionPanel>
      </AccordionItem>
    </Accordion>
  );
};

export default UserApartments;
