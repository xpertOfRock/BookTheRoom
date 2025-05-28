import React, { useState, useEffect } from "react";
import { Flex, Box, Text } from "@chakra-ui/react";
import { useNavigate } from "react-router-dom";
import { getCurrentUser, updateUser } from "../../services/auth";
import { fetchUserOrders } from "../../services/orders";
import { getUserComments } from "../../services/comments";
import { fetchUserApartments } from "../../services/apartments";
import ProfileCard from "../../components/profile/ProfileCard";
import ProfileEditForm from "../../components/profile/ProfileEditForm";
import UserOrders from "../../components/profile/UserOrders";
import UserComments from "../../components/profile/UserComments";
import UserApartments from "../../components/profile/UserApartments";

function Profile() {
  const [isEditing, setIsEditing] = useState(false);

  const [orderFilter, setOrderFilter] = useState({
    search: "",
    sortItem: "",
    sortOrder: "",
    page: 1,
    itemsCount: 3,
  });
  const [orders, setOrders] = useState([]);

  const [commentFilter, setCommentFilter] = useState({
    search: "",
    sortItem: "",
    sortOrder: "",
    page: 1,
    itemsCount: 3,
  });
  const [comments, setComments] = useState([]);

  const [aptFilter, setAptFilter] = useState({
    search: "",
    sortItem: "",
    sortOrder: "",
    countries: [],
    minPrice: undefined,
    maxPrice: undefined,
    page: 1,
    itemsCount: 3,
  });
  const [apartments, setApartments] = useState([]);

  const statuses = new Map([
    [0, "Active"],
    [1, "Awaiting"],
    [2, "Completed"],
    [3, "Closed"],
  ]);

  const statusColors = {
    Active: "blue.500",
    Awaiting: "yellow.400",
    Completed: "green.500",
    Closed: "red.500",
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

  const formatDate = (dateStr) =>
    dateStr
      ? new Date(dateStr).toLocaleDateString("en-US", {
          year: "numeric",
          month: "long",
          day: "numeric",
        })
      : "-";

  const user = getCurrentUser();
  const navigate = useNavigate();

  if (!user) return <Text>User not found</Text>;

  const handleEditClick = () => setIsEditing(true);
  const handleCancel = () => setIsEditing(false);

  const handleSave = async (formData) => {
    const data = new FormData();
    data.append("Email", formData.email);
    data.append("PhoneNumber", formData.phoneNumber);
    data.append("FirstName", formData.firstName);
    data.append("LastName", formData.lastName);
    if (formData.image) data.append("Image", formData.image);

    try {
      await updateUser(data);
      window.location.reload();
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <Flex
      minH="100vh"
      w={["100%", "100%", "75%"]}
      mx="auto"
      p={6}
      direction={["column", "column", "row"]}
      align="flex-start"
    >
      {isEditing ? (
        <ProfileEditForm user={user} onCancel={handleCancel} onSave={handleSave} formatDate={formatDate} />
      ) : (
        <ProfileCard user={user} onEdit={handleEditClick} formatDate={formatDate} />
      )}

      <Box w={["100%", "100%", "66%"]} ml={{ base: 0, md: 6 }}>
        <UserOrders
          orderFilter={orderFilter}
          setOrderFilter={setOrderFilter}
          orders={orders}
          statuses={statuses}
          statusColors={statusColors}
          formatDate={formatDate}
        />
        <UserComments
          commentFilter={commentFilter}
          setCommentFilter={setCommentFilter}
          comments={comments}
          formatDate={formatDate}
        />
        <UserApartments
          aptFilter={aptFilter}
          setAptFilter={setAptFilter}
          apartments={apartments}
          formatDate={formatDate}
        />
      </Box>
    </Flex>
  );
}

export default Profile;
