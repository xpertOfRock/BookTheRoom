import { useEffect, useState } from "react";
import CreateMessageForm from "./CreateMessageForm";
import Message from "./Message";
import {
  Flex,
  VStack,
  Text,
  useColorModeValue,
  useToast,
} from "@chakra-ui/react";
import { useParams } from "react-router-dom";
import { fetchChatById } from "../../services/chats";

function Chat({ currentUserId, onSendMessage }) {
  const [chat, setChat] = useState({});
  const [loading, setLoading] = useState(true);

  const { id, chatId } = useParams();
  const toast = useToast();

  const textColor = useColorModeValue("gray.800", "gray.200");
  const bgColor = useColorModeValue("purple.50", "purple.900");

  useEffect(() => {
    const load = async () => {
      try {
        const data = await fetchChatById(chatId);
        setChat(data);
      } catch (err) {
        toast({
          title: "Error",
          description: "Failed to load chat",
          status: "error",
          duration: 5000,
          isClosable: true,
        });
      } finally {
        setLoading(false);
      }
    };
    load();
  }, [id, toast]);

  if (loading)
    return (
      <Text textAlign="center" py={20} color={textColor}>
        Loading chat details...
      </Text>
    );

  return (
    <Flex
      direction="column"
      maxW="600px"
      h="600px"
      borderWidth={1}
      borderRadius="md"
      p={4}
      bg={bgColor}
    >
      <VStack
        spacing={3}
        overflowY="auto"
        flex={1}
        mb={4}
        css={{
          "&::-webkit-scrollbar": { width: "6px" },
          "&::-webkit-scrollbar-thumb": {
            backgroundColor: "#9f7aea",
            borderRadius: "24px",
          },
        }}
      >
        {(chat.Messages || []).map((msg) => (
          <Message
            key={msg.Id}
            message={msg}
            isOwn={msg.UserId === currentUserId}
          />
        ))}
      </VStack>

      <CreateMessageForm onSend={onSendMessage} />
    </Flex>
  );
}

export default Chat;
