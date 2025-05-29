import { Box, Flex, Text, Heading, Spacer, useColorModeValue } from "@chakra-ui/react";
import { StarIcon } from "@chakra-ui/icons";

function Comment({ username, description, createdAt, isCurrentUser, userScore }) {
  const meBg = useColorModeValue("purple.100", "purple.700");
  const notMeBg = useColorModeValue("white", "gray.700");

  const bg = isCurrentUser ? meBg : notMeBg;

  const borderColor = useColorModeValue("indigo.300", "indigo.600");
  const textColor = useColorModeValue("gray.800", "gray.200");
  const dateColor = useColorModeValue("gray.500", "gray.400");
  const scoreColor = "yellow.400";

  const formatDate = (dateStr) => {
    const d = new Date(dateStr);
    const day = String(d.getDate()).padStart(2, "0");
    const month = String(d.getMonth() + 1).padStart(2, "0");
    const year = d.getFullYear();
    const hours = String(d.getHours()).padStart(2, "0");
    const minutes = String(d.getMinutes()).padStart(2, "0");
    return `${day}.${month}.${year} ${hours}:${minutes}`;
  };

  return (
    <Box
      p={4}
      bg={bg}
      borderRadius="md"
      borderWidth={isCurrentUser ? 0 : 2}
      borderColor={borderColor}
      boxShadow="md"
    >
      <Flex align="center" mb={2}>
        <Heading size="md" color={textColor} fontWeight="bold">
          {username}
        </Heading>

        {userScore !== undefined && userScore !== null && userScore > 0 && (
          <Flex align="center" ml={3} color={scoreColor} fontWeight="semibold" fontSize="md" userSelect="none">
            <StarIcon mr={1} />
            {userScore.toFixed(1)}
          </Flex>
        )}

        <Spacer />

        <Text fontSize="sm" color={dateColor} whiteSpace="nowrap" userSelect="none">
          {formatDate(createdAt)}
        </Text>
      </Flex>

      <Text color={textColor} whiteSpace="pre-wrap">
        {description}
      </Text>
    </Box>
  );
}

export default Comment;
