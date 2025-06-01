import axios from "axios";
import { getCurrentToken } from "./auth";

const API_URL = "https://localhost:6061/api/Chat";

export const postChat = async (data) => {
  try {
    const token = getCurrentToken();
    const response = await axios.post(API_URL, data, {
      headers: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      },
    });
    return response;
  } catch (error) {
    console.error('Error creating hotel:', error.response ? error.response.data : error.message);
  }
};

export const fetchChatByApartmentId = async (apartmentId) => {  
  try {
    const token = getCurrentToken();
    const response = await axios.get(`${API_URL}/apartment-chats/${apartmentId}`, {
      headers: {
        'Authorization': `Bearer ${token}`,
      },
    });  
    console.log(response);
    return response.data;
  } catch (e) {
    console.error(e);
  }
};

export const fetchChatById = async (id) => {  
  try {
    const token = getCurrentToken();
    const response = await axios.get(`${API_URL}/${id}`, {
      headers: {
        'Authorization': `Bearer ${token}`,
      },
    });  
    return response.data;
  } catch (e) {
    console.error(e);
  }
};