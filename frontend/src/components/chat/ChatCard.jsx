import { Button } from "@chakra-ui/react";
import { getCurrentUserId, getUserById } from "../../services/auth";
import { useEffect, useState } from "react";
import { RiArrowRightLine } from "react-icons/ri";
import { useNavigate } from "react-router-dom";

function ChatCard({ chat }) {
  const navigate = useNavigate();
  const [otherUser, setOtherUser] = useState(null);

  const currentUserId = getCurrentUserId();
  const userId =
    Array.isArray(chat.usersId) && chat.usersId.length === 2
      ? chat.usersId.find((id) => id !== currentUserId)
      : undefined;

  useEffect(() => {
    if (!userId) return;

    const load = async () => {
      try {
        const result = await getUserById(userId);
        setOtherUser(result);
      } catch (error) {
        console.error(error);
      }
    };
    load();
  }, [userId]);

  return (
    <Button
      key={chat.id}
      colorScheme="purple"
      variant="outline"
      onClick={() => navigate(`/apartments/${chat.apartmentId}/chats/${chat.id}`)}
    >
      Chat with {otherUser ? otherUser.userName : "Loading..."}{" "}
      <RiArrowRightLine style={{ marginLeft: "8px" }} />
    </Button>
  );
}

export default ChatCard;
