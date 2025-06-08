import { useEffect, useState, useRef } from "react";
import {
  Flex,
  Box,
  VStack,
  Text,
  Heading,
  Stack,
  Spinner,
  useColorModeValue,
  useToast,
  Divider,
  Button,
} from "@chakra-ui/react";
import { useNavigate, useParams } from "react-router-dom";
import { HubConnectionBuilder } from "@microsoft/signalr";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft } from "@fortawesome/free-solid-svg-icons";
import { fetchChatByApartmentId, fetchChatById } from "../../services/chats";
import { fetchApartment } from "../../services/apartments";
import {
  getCurrentToken,
  getCurrentUserId,
  getCurrentUsername,
} from "../../services/auth";
import Message from "../../components/chat/Message";
import CreateMessageForm from "../../components/chat/CreateMessageForm";

function Chat() {
  const { id: apartmentId, chatId: paramChatId } = useParams();
  const [apartment, setApartment] = useState(null);
  const [chatId, setChatId] = useState(paramChatId || null);
  const [messages, setMessages] = useState([]);
  const [connection, setConnection] = useState(null);
  const [loading, setLoading] = useState(true);
  const [loadingApartment, setLoadingApartment] = useState(false);
  const [sending, setSending] = useState(false);

  const navigate = useNavigate();
  const messagesEndRef = useRef(null);
  const toast = useToast();

  const bgColor = useColorModeValue("gray.100", "gray.900");
  const infoBg = useColorModeValue("white", "gray.700");
  const chatBg = useColorModeValue("white", "gray.800");
  const borderColor = useColorModeValue("gray.300", "gray.600");

  const HUB_URL = "https://localhost:6061/chat";

  useEffect(() => {
    if (!apartmentId) return;

    const loadApartment = async () => {
      setLoadingApartment(true);
      try {
        const data = await fetchApartment(apartmentId);
        setApartment(data);
      } catch {
        toast({
          title: "Error",
          description: "Failed to load apartment info",
          status: "error",
          duration: 5000,
          isClosable: true,
        });
      } finally {
        setLoadingApartment(false);
      }
    };

    loadApartment();
  }, [apartmentId]);

  useEffect(() => {
    const loadChat = async () => {
      setLoading(true);
      try {
        let idToUse = chatId;
        if (!idToUse && apartmentId) {
          const chatData = await fetchChatByApartmentId(apartmentId);
          if (!chatData) throw new Error("No chat found for this apartment");
          idToUse = chatData.id || chatData.Id;
          setChatId(idToUse);
        }
        if (!idToUse) throw new Error("No chatId or apartment chat found");
        const chatDetail = await fetchChatById(idToUse);
        setMessages(chatDetail.messages || []);
      } catch (err) {
        toast({
          title: "Error",
          description: err.message || "Failed to load chat",
          status: "error",
          duration: 5000,
          isClosable: true,
        });
      } finally {
        setLoading(false);
      }
    };

    loadChat();
  }, [chatId, apartmentId, toast]);

  useEffect(() => {
    if (!chatId) return;
    if (connection) return;

    const conn = new HubConnectionBuilder()
      .withUrl(HUB_URL, { accessTokenFactory: () => getCurrentToken() })
      .withAutomaticReconnect()
      .build();

    conn.on(
      "ReceiveMessage",
      (id, userId, username, messageText, createdAt, connectionId) => {
        const newMsg = {
          id: id,
          userId: userId,
          userName: username,
          message: messageText,
          createdAt: createdAt,
          connectionId: connectionId,
        };
        setMessages((prev) => [...prev, newMsg]);
      }
    );

    const startConnection = async () => {
      try {
        await conn.start();

        await conn.invoke("JoinChat", {
          username: getCurrentUsername(),
          chatId,
        });

        setConnection(conn);
      } catch (error) {
        console.log(error);
      }
    };

    startConnection();

    return () => {
      conn.stop();
      setConnection(null);
    };
  }, [chatId]);

  useEffect(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: "smooth" });
  }, [messages]);

  const onSendMessage = async (text) => {
    if (!connection) return;
    setSending(true);
    try {
      await connection.invoke("SendMessage", {
        chatId,
        username: getCurrentUsername(),
        message: text.trim(),
      });
    } catch {
      toast({
        title: "Error",
        description: "Failed to send message",
        status: "error",
        duration: 3000,
        isClosable: true,
      });
    } finally {
      setSending(false);
    }
  };

  if (loading || loadingApartment) {
    return (
      <Flex align="center" justify="center" h="100vh" bg={bgColor}>
        <Spinner size="xl" color="purple.500" />
      </Flex>
    );
  }

  return (
    <Flex
      direction="column"
      h="100vh"
      w="100%"
      bg={bgColor}
      p={{ base: 2, md: 6 }}
      align="center"
      justify="center"
    >
      <Flex
        direction={{ base: "column", md: "row" }}
        maxW="960px"
        w="100%"
        h="100%"
        bg={bgColor}
        borderRadius="md"
        boxShadow="lg"
        overflow="hidden"
      >
        {apartment && (
          <Box
            position="relative"
            w={{ base: "100%", md: "40%" }}
            h={{ base: "auto", md: "100%" }}
            overflowY="auto"
            bg={infoBg}
            borderRight="1px"
            borderColor={borderColor}
            p={6}
            css={{
              "&::-webkit-scrollbar": { width: "6px" },
              "&::-webkit-scrollbar-thumb": {
                backgroundColor: "#9f7aea",
                borderRadius: "24px",
              },
            }}
          >
            <Button
              position="absolute"
              top="16px"
              left="16px"
              colorScheme="purple"
              leftIcon={<FontAwesomeIcon icon={faChevronLeft} />}
              size="md"
              onClick={() => navigate(`/apartments/${apartmentId}`)}
              _hover={{ bg: "purple.600" }}
            >
              Back
            </Button>

            <Stack spacing={4} mt="40px">
              <Heading size="lg" textAlign="center">
                {apartment.title}
              </Heading>

              <Text fontSize="sm" color="gray.500">
                {apartment.description}
              </Text>
              <Divider />
              <Box>
                <Text fontWeight="bold">Owner</Text>
                <Text>{apartment.owner}</Text>
                <Text fontSize="sm" color="gray.600">
                  Email: {apartment.email}
                </Text>
                <Text fontSize="sm" color="gray.600">
                  Phone: {apartment.phoneNumber}
                </Text>
                {apartment.telegram && (
                  <Text fontSize="sm" color="gray.600">
                    Telegram: {apartment.telegram}
                  </Text>
                )}
                {apartment.instagram && (
                  <Text fontSize="sm" color="gray.600">
                    Instagram: {apartment.instagram}
                  </Text>
                )}
              </Box>
              <Divider />
              <Box>
                <Text fontWeight="bold">Address</Text>
                <Text fontSize="sm" color="gray.600">
                  {apartment.address}
                </Text>
              </Box>
              <Divider />
              <Text fontWeight="bold">Price per night</Text>
              <Text>${apartment.price}</Text>
            </Stack>
          </Box>
        )}
        <Box
          w={{ base: "100%", md: "60%" }}
          h={{ base: "300px", md: "100%" }}
          display="flex"
          flexDirection="column"
          p={6}
          bg={chatBg}
          borderLeft="1px"
          borderColor={borderColor}
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
            {messages.length === 0 && (
              <Text
                color="gray.500"
                fontStyle="italic"
                mt={4}
                textAlign="center"
              >
                No messages yet. Start the conversation!
              </Text>
            )}
            {messages.map((msg) => (
              <Message
                key={msg.id}
                message={msg}
                isOwn={msg.userId === getCurrentUserId()}
              />
            ))}
            <div ref={messagesEndRef} />
          </VStack>
          <Divider />
          <CreateMessageForm onSend={onSendMessage} isSending={sending} />
        </Box>
      </Flex>
    </Flex>
  );
}

export default Chat;
