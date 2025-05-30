import React, { useState } from "react";
import {
  Box,
  Text,
} from "@chakra-ui/react";

function Message({ message, isOwn }) {
  const bg = isOwn ? "purple.300" : "purple.100";
  const alignSelf = isOwn ? "flex-end" : "flex-start";
  const textColor = isOwn ? "white" : "gray.800";
  return (
    <Box
      bg={bg}
      color={textColor}
      p={3}
      rounded="md"
      maxW="75%"
      alignSelf={alignSelf}
      boxShadow="md"
    >
      <Text fontWeight="bold" mb={1}>
        {message.UserName}
      </Text>
      <Text whiteSpace="pre-wrap">{message.Message}</Text>
      <Text fontSize="xs" mt={1} textAlign="right" opacity={0.6}>
        {new Date(message.CreatedAt).toLocaleString()}
      </Text>
    </Box>
  );
}

export default Message;



