import React, { useState, useEffect } from "react";
import {
  Box,
  Flex,
  Avatar,
  Text,
  Stack,
  Badge,
  Button,
  useColorModeValue,
  Input,
  Select,
  Accordion,
  AccordionItem,
  AccordionButton,
  AccordionPanel,
  AccordionIcon,
  VStack
} from "@chakra-ui/react";
import { useNavigate } from "react-router-dom";
import { getCurrentUser } from "../../services/auth";
import { fetchUserOrders } from "../../services/orders";
import { getUserComments } from "../../services/comments";
import { fetchUserApartments } from "../../services/apartments";
import ApartmentFilter from "../../components/apartment/ApartmentFilter";
import ApartmentSortAndSearchFilter from "../../components/apartment/ApartmentSortAndSearchFilter";
import Comment from "../../components/comment/Comment";

function Profile() {
  const bg = useColorModeValue("white", "gray.800");
  const filterBg = useColorModeValue("purple.50", "purple.900");
  const mainColor = "purple.600";
  const accentColor = "purple.300";
  const textColor = "gray.700";
  const navigate = useNavigate();

  const [orderFilter, setOrderFilter] = useState(
    { 
      search: "",
      sortItem: "",
      sortOrder: "",
      page: 1,
      itemsCount: 3
    });
  const [orders, setOrders] = useState([]);

  const [commentFilter, setCommentFilter] = useState(
    {
      search: "",
      sortItem: "",
      ortOrder: "",
      page: 1,
      itemsCount: 3
    }); 
  const [comments, setComments] = useState([]);

  const [aptFilter, setAptFilter] = useState(
    { 
      search: "",
      sortItem: "",
      sortOrder: "",
      countries: [],
      minPrice: undefined,
      maxPrice: undefined,
      page: 1,
      itemsCount: 3
    }
  );
  const [apartments, setApartments] = useState([]);

  const statuses = new Map([
    [0, "Active"],
    [1, "Awaiting"],
    [2, "Completed"],
    [3, "Closed"]
  ]);

  const statusColors = {
    Active: "blue.500",
    Awaiting: "yellow.400",
    Completed: "green.500",
    Closed: "red.500"
  };

  useEffect(() => {
    const fetchOrders = async () => {
      const data = await fetchUserOrders(orderFilter);
      setOrders(data || []);
    };
    fetchOrders();
  }, [orderFilter]);

  useEffect(() => {
    const fetchComments = async () => {
      const data = await getUserComments(commentFilter);
      setComments(data || []);
    };
    fetchComments();
  }, [commentFilter]);

  useEffect(() => {
    const fetchApartments = async () => {
      const data = await fetchUserApartments(aptFilter);
      setApartments(data || []);
    };
    fetchApartments();
  }, [aptFilter]);

  const formatDate = dateStr =>
    dateStr
      ? new Date(dateStr).toLocaleDateString("en-US", { year: "numeric", month: "long", day: "numeric" })
      : "-";

  const user = getCurrentUser();
  if (!user) return <Text>User not found</Text>;

  return (
    <Flex
      minH="100vh"
      w={["100%", "100%", "75%"]}
      mx="auto"
      bg={bg}
      p={6}
      direction={["column", "column", "row"]}
      align="flex-start"
    >
      <Box
        w={["100%", "100%", "25%"]}
        bg={bg}       
        boxShadow="lg"
        rounded="xl"
        p={6}
        textAlign="center"
        border="2px solid"
        borderColor={accentColor}
        mb={{ base: 6, md: 0 }}
        flexShrink={0}
      >
        <Avatar size="2xl" src={user.image} alt={`${user.firstName} ${user.lastName}`} mb={4} borderWidth="4px" borderColor={mainColor} mx="auto" />
        <Text fontSize="2xl" fontWeight="bold" color={mainColor}>
          {user.firstName} {user.lastName}
        </Text>
        <Badge mt={2} px={3} py={1} fontWeight="bold" fontSize="sm" colorScheme="purple" variant="subtle" rounded="full">
          {user.role.toUpperCase()}
        </Badge>
        <Stack spacing={3} mt={6} textAlign="left" color={textColor}>
          <Text><strong>Birth Date:</strong> {formatDate(user.birthDate)}</Text>
          <Text><strong>Registered At:</strong> {formatDate(user.registeredAt)}</Text>
          <Text><strong>Email:</strong> {user.email}</Text>
          <Text><strong>Phone:</strong> {user.phoneNumber || "-"}</Text>
          <Text><strong>Username:</strong> {user.userName}</Text>
        </Stack>
        <Button mt={6} colorScheme="purple" variant="solid" w="full">Edit Profile</Button>
      </Box>

      <Box w={["100%", "100%", "75%"]} ml={{ base: 0, md: 6 }}>
        <Accordion allowMultiple defaultIndex={[0, 1, 2]}>

          <AccordionItem>
            <AccordionButton borderRadius="md" color={mainColor} _expanded={{ bg: mainColor, color: "white" }}>
              <Box flex="1" textAlign="left" fontSize="lg" fontWeight="bold">Orders</Box>
              <AccordionIcon />
            </AccordionButton>
            <AccordionPanel pb={4}>
              <Box mb={4} bg={filterBg} border="1px solid" borderColor={accentColor} rounded="md" p={4}>
                <VStack align="start" spacing={3}>
                  <Input placeholder="Search" size="sm" value={orderFilter.search} onChange={e => setOrderFilter({ ...orderFilter, search: e.target.value, page: 1 })} />
                  <Select placeholder="Sort by" size="sm" value={orderFilter.sortItem} onChange={e => setOrderFilter({ ...orderFilter, sortItem: e.target.value, page: 1 })}>
                    <option value="date">Date</option>
                    <option value="status">Status</option>
                    <option value="price">Price</option>
                  </Select>
                  <Select placeholder="Order" size="sm" value={orderFilter.sortOrder} onChange={e => setOrderFilter({ ...orderFilter, sortOrder: e.target.value, page: 1 })}>
                    <option value="asc">Ascending</option>
                    <option value="desc">Descending</option>
                  </Select>
                  <Select size="sm" value={orderFilter.itemsCount} onChange={e => setOrderFilter({ ...orderFilter, itemsCount: e.target.value, page: 1 })}>
                    <option value={3}>3</option>
                    <option value={5}>5</option>
                    <option value={10}>10</option>
                  </Select>
                </VStack>
              </Box>
              <Box maxH="300px" overflowY="auto" border="1px solid" borderColor={accentColor} rounded="md" p={3}>
                {orders.map(o => {
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
                <Button size="sm" disabled={orderFilter.page <= 1} onClick={() => setOrderFilter({ ...orderFilter, page: orderFilter.page - 1 })}>Prev</Button>
                <Text>Page {orderFilter.page}</Text>
                <Button size="sm" onClick={() => setOrderFilter({ ...orderFilter, page: orderFilter.page + 1 })}>Next</Button>
              </Flex>
            </AccordionPanel>
          </AccordionItem>

          <AccordionItem>
            <AccordionButton borderRadius="md" color={mainColor} _expanded={{ bg: mainColor, color: "white" }}>
              <Box flex="1" textAlign="left" fontSize="lg" fontWeight="bold">Comments</Box>
              <AccordionIcon />
            </AccordionButton>
            <AccordionPanel pb={4}>
              <Box mb={4} bg={filterBg} border="1px solid" borderColor={accentColor} rounded="md" p={4}>
                <VStack align="start" spacing={3}>
                  <Input placeholder="Search" size="sm" value={commentFilter.search} onChange={e => setCommentFilter({ ...commentFilter, search: e.target.value, page: 1 })} />
                  <Select placeholder="Sort by" size="sm" value={commentFilter.sortItem} onChange={e => setCommentFilter({ ...commentFilter, sortItem: e.target.value, page: 1 })}>
                    <option value="date">Date</option>
                    <option vakue="rating">Rating</option>
                  </Select>
                  <Select placeholder="Order" size="sm" value={commentFilter.sortOrder} onChange={e => setCommentFilter({ ...commentFilter, sortOrder: e.target.value, page: 1 })}>
                    <option value="asc">Ascending</option>
                    <option value="desc">Descending</option>
                  </Select>
                  <Select size="sm" value={commentFilter.itemsCount} onChange={e => setCommentFilter({ ...commentFilter, itemsCount: e.target.value, page: 1 })}>
                    <option value={3}>3</option>
                    <option value={5}>5</option>
                    <option value={10}>10</option>
                  </Select>
                </VStack>
              </Box>
              <Box maxH="300px" overflowY="auto" border="1px solid" borderColor={accentColor} rounded="md" p={3}>
                {comments.map((c, idx) => (
                  <Flex key={c.id ?? idx} mb={4} p={3} bg={bg} rounded="md" boxShadow="sm" align="center" justify="space-between">
                    <Comment username={c.username} description={c.description} createdAt={c.createdAt} isCurrentUser userScore={c.userScore} />
                    <Button size="sm" colorScheme="purple" onClick={() => navigate(c.hotelId ? `/hotels/${c.hotelId}` : `/apartments/${c.apartmentId}`)}>View</Button>
                  </Flex>
                ))}
              </Box>
              <Flex justify="space-between" align="center" mt={2}>
                <Button size="sm" disabled={commentFilter.page <= 1} onClick={() => setCommentFilter({ ...commentFilter, page: commentFilter.page - 1 })}>Prev</Button>
                <Text>Page {commentFilter.page}</Text>
                <Button size="sm" onClick={() => setCommentFilter({ ...commentFilter, page: commentFilter.page + 1 })}>Next</Button>
              </Flex>
            </AccordionPanel>
          </AccordionItem>

          <AccordionItem>
            <AccordionButton borderRadius="md" color={mainColor} _expanded={{ bg: mainColor, color: "white" }}>
              <Box flex="1" textAlign="left" fontSize="lg" fontWeight="bold">Apartments</Box>
              <AccordionIcon />
            </AccordionButton>
            <AccordionPanel pb={4}>
              <Box mb={4} bg={filterBg} border="1px solid" borderColor={accentColor} rounded="md" p={4}>
                <ApartmentSortAndSearchFilter filter={aptFilter} setFilter={setAptFilter} />
                <br />
                <ApartmentFilter filter={aptFilter} setFilter={setAptFilter} />
              </Box>
              <Box maxH="300px" overflowY="auto" border="1px solid" borderColor={accentColor} rounded="md" p={3}>
                {apartments.map(apt => (
                  <Flex key={apt.id} mb={4} p={3} bg={bg} rounded="md" boxShadow="sm" align="center" justify="space-between">
                    <Box>
                      <Text fontWeight="bold">{apt.title}</Text>
                      <Text fontSize="sm">{apt.address}</Text>
                      <Text fontSize="sm">Price per night: ${apt.priceForNight}</Text>
                      <Text fontSize="sm">Created: {formatDate(apt.createdAt)}</Text>
                    </Box>
                    <Button size="sm" colorScheme="purple" onClick={() => navigate(`/apartments/${apt.id}`)}>View</Button>
                  </Flex>
                ))}
              </Box>
              <Flex justify="space-between" align="center" mt={2}>
                <Button size="sm" disabled={aptFilter.page <= 1} onClick={() => setAptFilter({ ...aptFilter, page: aptFilter.page - 1 })}>Prev</Button>
                <Text>Page {aptFilter.page}</Text>
                <Button size="sm" onClick={() => setAptFilter({ ...aptFilter, page: aptFilter.page + 1 })}>Next</Button>
              </Flex>
            </AccordionPanel>
          </AccordionItem>
        </Accordion>
      </Box>
    </Flex>
  );
}

export default Profile;
