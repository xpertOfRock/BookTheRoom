import React from "react";
import { Box, Flex, Text, Heading, Spacer, useColorModeValue } from "@chakra-ui/react";

function Comment({ username, description, createdAt, isCurrentUser, userScore }) {
  const me = useColorModeValue("purple.100", "purple.700");
  const notMe = useColorModeValue("white", "gray.700");
  
  const bg = isCurrentUser
    ? me
    : notMe;

  const borderColor = useColorModeValue("indigo.300", "indigo.600");
  const textColor = useColorModeValue("gray.800", "gray.200");
  const dateColor = useColorModeValue("gray.500", "gray.400");

  return (
    <Box
      p={4}
      bg={bg}
      borderRadius="md"
      borderWidth={isCurrentUser ? 0 : 2}
      borderColor={borderColor}
      boxShadow="md"
    >
      <Flex mb={2} align="center">
        <Heading size="md" color={textColor}>
          {username}
          {userScore !== undefined && userScore !== null && userScore > 0 && (
            <Text as="span" color="yellow.400" ml={2} fontWeight="semibold">
              {userScore.toFixed(1)} ★
            </Text>
          )}
        </Heading>
        <Spacer />
        <Text fontSize="sm" color={dateColor} whiteSpace="nowrap">
          {new Date(createdAt).toLocaleString("en-US", {
            year: "numeric",
            month: "long",
            day: "numeric",
            hour: "2-digit",
            minute: "2-digit",
          })}
        </Text>
      </Flex>
      <Text color={textColor}>{description}</Text>
    </Box>
  );
}
export default Comment;