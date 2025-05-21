import Cookies from "js-cookie";
import {
  Box,
  Flex,
  Avatar,
  Text,
  Stack,
  Badge,
  Button,
  useColorModeValue,
} from "@chakra-ui/react";

const Profile = () => {
  const bg = useColorModeValue("white", "gray.800");
  const mainColor = "purple.600";
  const accentColor = "purple.300";
  const textColor = "gray.700";

  const userCookie = Cookies.get("user");
  const user = userCookie ? JSON.parse(userCookie) : null;

  if (!user) return <Text>User not found</Text>;

  const formatDate = (dateStr) => {
    if (!dateStr) return "-";
    const d = new Date(dateStr);
    return d.toLocaleDateString("en-US", {
      year: "numeric",
      month: "long",
      day: "numeric",
    });
  };

  return (
    <Flex
      justify="center"
      align="center"
      minH="100vh"
      bgGradient="linear(to-r, white, purple.50, purple.100)"
      p={6}
    >
      <Box
        maxW="400px"
        w="full"
        bg={bg}
        boxShadow="lg"
        rounded="xl"
        p={6}
        textAlign="center"
        border="2px solid"
        borderColor={accentColor}
      >
        <Avatar
          size="2xl"
          src={user.image}
          alt={`${user.firstName} ${user.lastName}`}
          mb={4}
          pos="relative"
          borderWidth="4px"
          borderColor={mainColor}
          mx="auto"
        />

        <Text fontSize="2xl" fontWeight="bold" color={mainColor}>
          {user.firstName} {user.lastName}
        </Text>

        <Badge
          mt={2}
          px={3}
          py={1}
          fontWeight="bold"
          fontSize="sm"
          colorScheme="purple"
          variant="subtle"
          rounded="full"
        >
          {user.role.toUpperCase()}
        </Badge>

        <Stack spacing={3} mt={6} textAlign="left" color={textColor}>
          <Box>
            <Text fontWeight="bold">Birth Date:</Text>
            <Text>{formatDate(user.birthDate)}</Text>
          </Box>
          <Box>
            <Text fontWeight="bold">Registered At:</Text>
            <Text>{formatDate(user.registeredAt)}</Text>
          </Box>
          <Box>
            <Text fontWeight="bold">Email:</Text>
            <Text>{user.email}</Text>
          </Box>
          <Box>
            <Text fontWeight="bold">Phone Number:</Text>
            <Text>{user.phoneNumber || "-"}</Text>
          </Box>
          <Box>
            <Text fontWeight="bold">Username:</Text>
            <Text>{user.userName}</Text>
          </Box>
        </Stack>

        <Button
          mt={6}
          colorScheme="purple"
          variant="solid"
          _hover={{ bg: "purple.700" }}
          w="full"
        >
          Edit Profile
        </Button>
      </Box>
    </Flex>
  );
};

export default Profile;