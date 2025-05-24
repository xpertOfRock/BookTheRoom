import { Box, Heading, VStack } from "@chakra-ui/react";
import RoomsFilter from "../../../rooms/subcomponents/Rooms/RoomsFilter";
import Rooms from "../../../rooms/Rooms";

function RoomsSection({ filter, setFilter, onApplyFilters, rooms, hotelName, checkIn, checkOut }) {
  return (
    <Box
      w="full"
      borderWidth={3}
      borderColor="purple.400"
      borderRadius="lg"
      boxShadow="md"
      bg="purple.50"
      p={6}
    >
      <VStack spacing={6} align="stretch">
        <Heading size="lg" color="purple.700">
          Available Rooms
        </Heading>

        <Box>
          <RoomsFilter
            filter={filter}
            setFilter={setFilter}
            onApplyFilters={onApplyFilters}
          />
        </Box>

        <Box>
          <Rooms
            rooms={rooms}
            checkIn={checkIn}
            checkOut={checkOut}
            hotel={hotelName}
            className="items-center"
          />
        </Box>
      </VStack>
    </Box>
  );
}

export default RoomsSection;
