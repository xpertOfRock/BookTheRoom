import axios from 'axios';
import { isAuthorized, getCurrentToken } from './auth';

const API_URL = "https://localhost:6061/api/order";

export const getClientToken = async () => {
    try {  
      const response = await axios.get(`${API_URL}/client-token`);
      return response.data;
    } catch (error) {
      console.error(
        "Error fetching client token : ",
        error.response ? error.response.data : error.message
      );
    }
};

export const fetchUserOrders = async (filter) => {

  const token = getCurrentToken();

  try {    
      const params = {
        search: filter?.search || undefined,
        sortItem: filter?.sortItem || undefined,
        sortOrder: filter?.sortOrder || undefined,
        itemsCount: filter?.itemsCount || 9,
        page: filter?.page || 1
      };
  
      const result = await axios.get(`${API_URL}/user-orders`, { params,
        headers: {
            'Authorization': `Bearer ${token}`,
          },
       }           
      );
      return result.data.orders;
    } catch (e) {
      console.error(e);
    }
}

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

        return response;
      } catch (error) {
        
      }
}