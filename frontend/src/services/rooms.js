import axios from "axios"
import { getCurrentToken } from "./auth";

// "https://localhost:5286/api/room";
const API_URL = "https://localhost:6061/api/room";

export const fetchRooms = async (hotelId, filter) => {
  try {
    const defaultCheckIn = new Date(Date.now() + 86400000).toISOString();
    const defaultCheckOut = new Date(Date.now() + 172800000).toISOString();

    const params = {
      search: filter?.search || null,
      sortItem: filter?.sortItem || null,
      sortOrder: filter?.sortOrder || null,
      categories: filter?.categories?.length > 0 ? filter.categories.join(",") : null,
      minPrice: filter?.minPrice > 0 ? filter.minPrice : 1,
      maxPrice: filter?.maxPrice > 0 ? filter.maxPrice : null,
      checkIn: filter?.checkIn || defaultCheckIn,
      checkOut: filter?.checkOut || defaultCheckOut,
    };

    const response = await axios.get(`${API_URL}/${hotelId}`, { params });
    return response.data.rooms;
  } catch (e) {
    console.error("Error fetching rooms:", e);
    throw e;
  }
};


// export const fetchRoom = async (id) => {  
//   try{
//       let response = await axios.get(`${API_URL}/${id}`);
//       return response.data;
//   }catch(e){
//       console.error(e);
//   }
// };

export const postRoom = async (hotelId, formData) => {
  try {
    const token = getCurrentToken();
    const response = await axios.post(`${API_URL}/${hotelId}`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        'Authorization': `Bearer ${token}`,
      },
    });
    
    return response.status;
  } catch (error) {
    console.error('Error creating hotel:', error.response ? error.response.data : error.message);
  }
};


// export const putRoom = async (id, number, formData) => {
//   try {
//     const token = getToken();
//     const response = await axios.put(`${API_URL}/${id}/${number}`, formData, {
//       headers: {
//         'Content-Type': 'multipart/form-data',
//         'Authorization': `Bearer ${token}`,
//       },
//     });
//     return response.status;
//   } catch (e) {
//     console.error(e);
//   }
// };

// export const deleteRoom = async(id) => {
//   try {
//     const token = getToken();
//     const response = await axios.delete(`${API_URL}/${id}/${number}`, {
//       headers: {
//         'Authorization': `Bearer ${token}`,
//       },
//     });
//     return response.status;
//   } catch (e) {
//     console.error(e);
//   }
// };