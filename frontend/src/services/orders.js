import axios from 'axios';
import { isAuthorized, getCurrentToken } from './auth';

const API_URL = "https://localhost:6061/api/order";

export const getClientToken = async () => {
    try {  
      const response = await axios.get(`${API_URL}/client-token`);
      console.log(response.data);
      return response.data;
    } catch (error) {
      console.error(
        "Error fetching client token : ",
        error.response ? error.response.data : error.message
      );
    }
};

export const postOrder = async (hotelId, number, orderData) => {
    try {
        const token = isAuthorized ? getCurrentToken() : "";
    
        const response = await axios.post(`${API_URL}/${hotelId}/${number}`,
          orderData, 
          {
            headers: {
              Authorization: `Bearer ${token}`
            },
          }
        );
        return response.data;
      } catch (error) {
        console.error(
          "Error occured while creating new order: ",
          error.response ? error.response.data : error.message
        );
      }
}