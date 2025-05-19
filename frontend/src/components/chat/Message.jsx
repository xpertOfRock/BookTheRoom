import { HubConnectionBuilder } from "@microsoft/signalr"
import { useNavigate } from "react-router-dom";

const joinChat = async (userName, chatId) => {
    
    const { id: id } = useParams();
    const navigate = useNavigate();

    var connection = new HubConnectionBuilder()
        .withUrl("https://localhost:6061/hubs/chat")
        .withAutomaticReconnect()
        .build();

    try{
        await connection.start();
        await connection.invoke("JoinChat", { userName, chatId });
        navigate(`/Apartments/${id}/Chats/${chatId}`);
    }catch(error){
        console.error(error);
    }
}