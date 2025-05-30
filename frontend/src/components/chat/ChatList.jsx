import { VStack, Button, Heading, Box } from "@chakra-ui/react";
import { useNavigate, useParams } from "react-router-dom";

function ChatList({ chats }) {
  const { apartmentId } = useParams()
  const navigate = useNavigate();

  return (
    <Box>
      <Heading size="sm" mb={3}>Chats</Heading>
      <VStack align="start" spacing={2}>
        {chats.map((chat) => (
          <Button
            key={chat.id}
            variant="outline"
            onClick={() => navigate(`/apartments/${apartmentId}/chats/${chat.id}`)}
          >
            Chat with: {chat.users.filter((u) => !u.isCurrentUser).map((u) => u.name).join(", ")}
          </Button>
        ))}
      </VStack>
    </Box>
  );
}

export default ChatList;
