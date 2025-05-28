import {
  Box,
  Avatar,
  Text,
  Stack,
  Badge,
  Button,
  useColorModeValue,
} from "@chakra-ui/react";

const ProfileCard = ({ user, onEdit, formatDate }) => {
  const bg = useColorModeValue("white", "gray.800");
  const mainColor = "purple.600";
  const accentColor = "purple.300";
  const textColor = "gray.700";

  return (
    <Box
      w={["100%", "100%", "33%"]}
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
      <Avatar
        size="2xl"
        src={user.image}
        alt={`${user.firstName} ${user.lastName}`}
        mb={4}
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
        <Text>
          <strong>Birth Date:</strong> {formatDate(user.birthDate)}
        </Text>
        <Text>
          <strong>Registered At:</strong> {formatDate(user.registeredAt)}
        </Text>
        <Text>
          <strong>Email:</strong> {user.email}
        </Text>
        <Text>
          <strong>Phone:</strong> {user.phoneNumber || "-"}
        </Text>
        <Text>
          <strong>Username:</strong> {user.userName}
        </Text>
      </Stack>
      <Button mt={6} colorScheme="purple" variant="solid" w="full" onClick={onEdit}>
        Edit Profile
      </Button>
    </Box>
  );
};

export default ProfileCard;