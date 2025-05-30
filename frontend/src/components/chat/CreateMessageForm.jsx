import { useState } from "react";
import {
  Button,
  Input,
  HStack,
  useColorModeValue,
} from "@chakra-ui/react";

function CreateMessageForm({ onSend, isSending }) {
  const [text, setText] = useState("");

  const handleSend = () => {
    if (text.trim()) {
      onSend(text.trim());
      setText("");
    }
  };

  const handleKeyDown = (e) => {
    if (e.key === "Enter" && !e.shiftKey) {
      e.preventDefault();
      handleSend();
    }
  };

  return (
    <HStack mt={4} spacing={2}>
      <Input
        placeholder="Введите сообщение..."
        value={text}
        onChange={(e) => setText(e.target.value)}
        onKeyDown={handleKeyDown}
        bg={useColorModeValue("white", "gray.700")}
        _focus={{ borderColor: "purple.500" }}
        isDisabled={isSending}
      />
      <Button
        colorScheme="purple"
        onClick={handleSend}
        isLoading={isSending}
        loadingText="Отправка"
      >
        Send
      </Button>
    </HStack>
  );
}

export default CreateMessageForm;