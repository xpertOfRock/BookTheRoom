
import { VStack, Button, Heading, Box } from "@chakra-ui/react";
import ChatCard from "./ChatCard";

function ChatList({ chats }) {
 
  return (
    <Box>
      <Heading size="sm" mb={3}>
        Chats
      </Heading>
      <VStack align="start" spacing={2}>
        {chats.map((chat) => {
          return (
            <ChatCard key={chat.id} chat={chat} />
          );
        })}
      </VStack>
    </Box>
  );

}

export default ChatList;
