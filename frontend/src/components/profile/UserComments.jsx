import {
  Accordion,
  AccordionItem,
  AccordionButton,
  AccordionPanel,
  AccordionIcon,
  Box,
  VStack,
  Input,
  Select,
  Flex,
  Text,
  Button,
  useColorModeValue,
} from "@chakra-ui/react";
import Comment from "../../components/comment/Comment";
import { useNavigate } from "react-router-dom";

const UserComments = ({ commentFilter, setCommentFilter, comments, formatDate }) => {
  const bg = useColorModeValue("white", "gray.800");
  const filterBg = useColorModeValue("purple.50", "purple.900");
  const accentColor = "purple.300";
  const mainColor = "purple.600";
  const navigate = useNavigate();

  return (
    <Accordion allowMultiple defaultIndex={[0]}>
      <AccordionItem>
        <AccordionButton borderRadius="md" color={mainColor} _expanded={{ bg: mainColor, color: "white" }}>
          <Box flex="1" textAlign="left" fontSize="lg" fontWeight="bold">
            Comments
          </Box>
          <AccordionIcon />
        </AccordionButton>
        <AccordionPanel pb={4}>
          <Box mb={4} bg={filterBg} border="1px solid" borderColor={accentColor} rounded="md" p={4}>
            <VStack align="start" spacing={3}>
              <Input
                placeholder="Search"
                size="sm"
                value={commentFilter.search}
                onChange={(e) => setCommentFilter({ ...commentFilter, search: e.target.value, page: 1 })}
              />
              <Select
                placeholder="Sort by"
                size="sm"
                value={commentFilter.sortItem}
                onChange={(e) => setCommentFilter({ ...commentFilter, sortItem: e.target.value, page: 1 })}
              >
                <option value="date">Date</option>
                <option value="rating">Rating</option>
              </Select>
              <Select
                placeholder="Order"
                size="sm"
                value={commentFilter.sortOrder}
                onChange={(e) => setCommentFilter({ ...commentFilter, sortOrder: e.target.value, page: 1 })}
              >
                <option value="asc">Ascending</option>
                <option value="desc">Descending</option>
              </Select>
              <Select
                size="sm"
                value={commentFilter.itemsCount}
                onChange={(e) => setCommentFilter({ ...commentFilter, itemsCount: Number(e.target.value), page: 1 })}
              >
                <option value={3}>3</option>
                <option value={5}>5</option>
                <option value={10}>10</option>
              </Select>
            </VStack>
          </Box>
          <Box maxH="300px" overflowY="auto" border="1px solid" borderColor={accentColor} rounded="md" p={3}>
            {comments.map((c, idx) => (
              <Flex
                key={c.id ?? idx}
                mb={4}
                p={3}
                bg={bg}
                rounded="md"
                boxShadow="sm"
                align="center"
                justify="space-between"
              >
                <Comment
                  username={c.username}
                  description={c.description}
                  createdAt={c.createdAt}
                  isCurrentUser
                  userScore={c.userScore}
                />
                <Button
                  size="sm"
                  colorScheme="purple"
                  onClick={() => navigate(c.hotelId ? `/hotels/${c.hotelId}` : `/apartments/${c.apartmentId}`)}
                >
                  View
                </Button>
              </Flex>
            ))}
          </Box>
          <Flex justify="space-between" align="center" mt={2}>
            <Button
              size="sm"
              colorScheme="purple"          
              variant="outline"
              disabled={commentFilter.page <= 1}
              onClick={() => setCommentFilter({ ...commentFilter, page: commentFilter.page - 1 })}
            >
              Prev
            </Button>
            <Text>Page {commentFilter.page}</Text>
            <Button
              size="sm"
              colorScheme="purple"          
              variant="outline"
              onClick={() => setCommentFilter({ ...commentFilter, page: commentFilter.page + 1 })}
            >
              Next
            </Button>
          </Flex>
        </AccordionPanel>
      </AccordionItem>
    </Accordion>
  );
};

export default UserComments;
