import { Box, Text } from "@chakra-ui/react";

function Message({ message, isOwn }) {
  const alignSelf = isOwn ? "flex-end" : "flex-start";

  const bg = isOwn ? "purple.600" : "transparent";
  const textColor = isOwn ? "white" : "purple.600";
  const border = isOwn ? "none" : "1px solid";
  const borderColor = isOwn ? "none" : "purple.600";

  const formattedDate = message.createdAt
    ? new Date(message.createdAt).toLocaleString(undefined, {
        year: "numeric",
        month: "2-digit",
        day: "2-digit",
        hour: "2-digit",
        minute: "2-digit",
      })
    : "";

  return (
    <Box
      bg={bg}
      color={textColor}
      border={border}
      borderColor={borderColor}
      p={3}
      rounded="md"
      maxW="75%"
      alignSelf={alignSelf}
      boxShadow="md"
    >
      <Text fontWeight="bold" mb={1}>
        {message.userName}
      </Text>
      <Text whiteSpace="pre-wrap">{message.message}</Text>
      <Text fontSize="xs" mt={1} textAlign="right" opacity={0.6}>
        {formattedDate}
      </Text>
    </Box>
  );
}

export default Message;