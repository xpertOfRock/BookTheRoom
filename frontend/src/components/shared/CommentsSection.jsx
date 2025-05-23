import { useState, useMemo } from "react";
import {
  Box,
  Heading,
  VStack,
  Text,
  Select,
  HStack,
  Button,
  useColorModeValue,
} from "@chakra-ui/react";
import CreateCommentForm from "../comment/CreateCommentForm";
import Comment from "../comment/Comment";

function CommentsSection({
  property,
  hasRatedComments,
  currentUserId,
  onAddComment,
  propertyType,
  onBackClick,
}) {
  const [sortOption, setSortOption] = useState("dateDesc");
  const [filterOption, setFilterOption] = useState("all");

  const borderColor = useColorModeValue("indigo.300", "indigo.600");
  const bg = useColorModeValue("indigo.50", "indigo.900");
  const textColor = useColorModeValue("gray.800", "gray.200");
  const titleColor = useColorModeValue("purple.700", "purple.300");

  const filteredComments = useMemo(() => {
    if (!property.comments) return [];

    let comments = [...property.comments];

    if (filterOption === "myComments") {
      comments = comments.filter((c) => c.userId === currentUserId);
    } else if (filterOption === "rated") {
      comments = comments.filter(
        (c) => c.userScore !== undefined && c.userScore !== null && c.userScore > 0
      );
    }

    switch (sortOption) {
      case "dateAsc":
        comments.sort((a, b) => new Date(a.createdAt) - new Date(b.createdAt));
        break;
      case "dateDesc":
        comments.sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt));
        break;
      case "ratedFirst":
        comments.sort((a, b) => (b.userScore ?? 0) - (a.userScore ?? 0));
        break;
      case "ratedLast":
        comments.sort((a, b) => (a.userScore ?? 0) - (b.userScore ?? 0));
        break;
      default:
        break;
    }

    return comments;
  }, [property.comments, sortOption, filterOption, currentUserId]);

  return (
    <Box
      w="full"
      p={6}
      borderWidth={3}
      borderColor={borderColor}
      borderRadius="lg"
      boxShadow="md"
      bg={bg}
      color={textColor}
      fontFamily="'Inter', sans-serif"
    >
      <HStack justify="space-between" mb={4}>       
        <Heading size="lg" mb={0} color={titleColor}>
          Comments
        </Heading>
        <Box w="60px" />
      </HStack>

      <CreateCommentForm
        propertyId={property.id}
        hasRatedComments={hasRatedComments}
        onAddComment={onAddComment}
        propertyType={propertyType}
      />

      <HStack mt={6} mb={4} spacing={6} flexWrap="wrap" justify={{ base: "center", md: "flex-start" }}>
        <Select
          maxW="220px"
          value={sortOption}
          onChange={(e) => setSortOption(e.target.value)}
          bg="white"
          color="black"
          borderColor="gray.300"
          size="md"
          borderRadius="md"
          aria-label="Sort comments"
        >
          <option value="dateDesc">Newest First</option>
          <option value="dateAsc">Oldest First</option>
          <option value="ratedFirst">Highest Rated First</option>
          <option value="ratedLast">Lowest Rated First</option>
        </Select>

        <Select
          maxW="220px"
          value={filterOption}
          onChange={(e) => setFilterOption(e.target.value)}
          bg="white"
          color="black"
          borderColor="gray.300"
          size="md"
          borderRadius="md"
          aria-label="Filter comments"
        >
          <option value="all">All Comments</option>
          <option value="myComments">My Comments</option>
          <option value="rated">Rated Only</option>
        </Select>
      </HStack>

      {filteredComments.length > 0 ? (
        <VStack spacing={6} align="stretch">
          {filteredComments.map((comment) => (
            <Comment
              key={comment.id}
              username={comment.username}
              description={comment.description}
              createdAt={comment.createdAt}
              isCurrentUser={currentUserId === comment.userId}
              userScore={comment.userScore}
            />
          ))}
        </VStack>
      ) : (
        <Text fontStyle="italic" color="gray.500" textAlign="center" mt={10}>
          No comments to display.
        </Text>
      )}
    </Box>
  );
}

export default CommentsSection;