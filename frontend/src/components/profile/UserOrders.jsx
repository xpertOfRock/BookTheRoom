import React from "react";
import {
  Accordion,
  AccordionItem,
  AccordionButton,
  AccordionPanel,
  AccordionIcon,
  Box,
  VStack,
  Input,
  Select,
  Flex,
  Text,
  Badge,
  Button,
  useColorModeValue,
} from "@chakra-ui/react";

const UserOrders = ({ orderFilter, setOrderFilter, orders, statuses, statusColors, formatDate }) => {
  const bg = useColorModeValue("purple.100", "purple.100");
  const filterBg = useColorModeValue("purple.50", "purple.900");
  const accentColor = "purple.300";
  const mainColor = "purple.600";

  return (
    <Accordion allowMultiple defaultIndex={[0]}>
      <AccordionItem>
        <AccordionButton borderRadius="md" color={mainColor} _expanded={{ bg: mainColor, color: "white" }}>
          <Box flex="1" textAlign="left" fontSize="lg" fontWeight="bold">
            Orders
          </Box>
          <AccordionIcon />
        </AccordionButton>
        <AccordionPanel pb={4}>
          <Box mb={4} bg={filterBg} border="1px solid" borderColor={accentColor} rounded="md" p={4}>
            <VStack align="start" spacing={3}>
              <Input
                placeholder="Search"
                size="sm"
                value={orderFilter.search}
                onChange={(e) => setOrderFilter({ ...orderFilter, search: e.target.value, page: 1 })}
              />
              <Select
                placeholder="Sort by"
                size="sm"
                value={orderFilter.sortItem}
                onChange={(e) => setOrderFilter({ ...orderFilter, sortItem: e.target.value, page: 1 })}
              >
                <option value="date">Date</option>
                <option value="status">Status</option>
                <option value="price">Price</option>
              </Select>
              <Select
                placeholder="Order"
                size="sm"
                value={orderFilter.sortOrder}
                onChange={(e) => setOrderFilter({ ...orderFilter, sortOrder: e.target.value, page: 1 })}
              >
                <option value="asc">Ascending</option>
                <option value="desc">Descending</option>
              </Select>
              <Select
                size="sm"
                value={orderFilter.itemsCount}
                onChange={(e) => setOrderFilter({ ...orderFilter, itemsCount: Number(e.target.value), page: 1 })}
              >
                <option value={3}>3</option>
                <option value={5}>5</option>
                <option value={10}>10</option>
              </Select>
            </VStack>
          </Box>
          <Box maxH="300px" overflowY="auto" border="1px solid" borderColor={accentColor} rounded="md" p={3}>
            {orders.map((o) => {
              const label = statuses.get(o.status);
              return (
                <Box key={o.orderId} mb={4} p={3} bg={bg} rounded="md" boxShadow="sm">
                  <Flex justify="space-between" align="center">
                    <Text fontWeight="bold">Order #{o.orderId}</Text>
                    <Badge px={2} py={1} color="white" bg={statusColors[label]} rounded="md">
                      {label}
                    </Badge>
                  </Flex>
                  <Text fontSize="sm">Hotel: {o.hotelName}</Text>
                  <Text fontSize="sm">Room: {o.roomNumber}</Text>
                  <Text fontSize="sm">Price: ${o.overallPrice}</Text>
                  <Text fontSize="sm">Check-In: {formatDate(o.checkIn)}</Text>
                  <Text fontSize="sm">Check-Out: {formatDate(o.checkOut)}</Text>
                </Box>
              );
            })}
          </Box>
          <Flex justify="space-between" align="center" mt={2}>
            <Button
              size="sm"
              colorScheme="purple"          
              variant="outline"
              disabled={orderFilter.page <= 1}
              onClick={() => setOrderFilter({ ...orderFilter, page: orderFilter.page - 1 })}
            >
              Prev
            </Button>
            <Text>Page {orderFilter.page}</Text>
            <Button
              size="sm"
              colorScheme="purple"          
              variant="outline"
              onClick={() => setOrderFilter({ ...orderFilter, page: orderFilter.page + 1 })}
            >
              Next
            </Button>
          </Flex>
        </AccordionPanel>
      </AccordionItem>
    </Accordion>
  );
};

export default UserOrders;
