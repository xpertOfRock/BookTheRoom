// CreateCommentForm.jsx
import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  Textarea,
  Text,
  HStack,
  IconButton,
  useColorModeValue,
} from "@chakra-ui/react";
import { isAuthorized } from "../../services/auth";

function CreateCommentForm({ propertyId, hasRatedComments, onAddComment, propertyType }) {
  const [description, setDescription] = useState("");
  const [rating, setRating] = useState(0);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const navigate = useNavigate();
  const textColor = useColorModeValue("gray.800", "gray.200");

  const handleRatingClick = (value) => {
    if (!hasRatedComments) {
      setRating(value);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    if (!description.trim()) {
      setError("Comment cannot be empty.");
      return;
    }

    setError("");
    setLoading(true);

    try {
      await onAddComment({
        description,
        propertyId,
        propertyType,
        userScore: rating,
      });
      setDescription("");
      setRating(0);
    } catch (err) {
      console.error("Error posting comment:", err);
      setError("Failed to post comment. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box
      as="form"
      onSubmit={handleSubmit}
      mt={2}
      bg={useColorModeValue("indigo.100", "indigo.900")}
      p={4}
      borderRadius="lg"
      boxShadow="md"
      borderWidth={2}
      borderColor={useColorModeValue("indigo.300", "indigo.600")}
      color={textColor}
    >
      <FormControl mb={4}>
        <FormLabel>Your Comment:</FormLabel>
        <Textarea
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          placeholder="Write your comment here..."
          rows={4}
          focusBorderColor="purple.400"
          resize="vertical"
        />
      </FormControl>

      {!hasRatedComments ? (
        <FormControl mb={4}>
          <FormLabel>Your Rating:</FormLabel>
          <HStack spacing={1}>
            {[...Array(10)].map((_, i) => {
              const value = i + 1;
              return (
                <IconButton
                  key={value}
                  aria-label={`Rate ${value} star${value > 1 ? "s" : ""}`}
                  icon={
                    <Text
                      fontSize="2xl"
                      color={value <= rating ? "yellow.400" : "gray.300"}
                      _hover={{ color: "yellow.500" }}
                      cursor="pointer"
                    >
                      ★
                    </Text>
                  }
                  onClick={() => handleRatingClick(value)}
                  variant="ghost"
                  size="sm"
                />
              );
            })}
          </HStack>
        </FormControl>
      ) : (
        <Text color="gray.500" fontSize="sm" mb={4}>
          You have already rated this hotel
        </Text>
      )}

      {error && (
        <Text color="red.500" mb={4}>
          {error}
        </Text>
      )}

      <Box textAlign="right">
        {isAuthorized() ? (
          <Button
            type="submit"
            colorScheme="purple"
            isLoading={loading}
            loadingText="Posting..."
          >
            Comment
          </Button>
        ) : (
          <Text color={textColor}>
            You need to be logged in to leave a comment.{" "}
            <Text
              as="span"
              color="purple.500"
              cursor="pointer"
              textDecoration="underline"
              onClick={() => navigate("/login")}
            >
              Log in here
            </Text>
          </Text>
        )}
      </Box>
    </Box>
  );
}

export default CreateCommentForm;
