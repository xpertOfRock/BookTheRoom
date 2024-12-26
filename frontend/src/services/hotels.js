import axios from "axios";
import { getCurrentToken } from "./auth";
// "https://localhost:5286/api/hotel";
const API_URL = "https://localhost:6061/api/hotel";

export const fetchHotels = async (filter) => {
  try {
    const params = {
      search: filter?.search,
      sortItem: filter?.sortItem,
      sortOrder: filter?.sortOrder,
      countries: filter?.countries.length > 0 ? filter.countries.join(",") : undefined,
      ratings: filter?.ratings.length > 0 ? filter.ratings.join(",") : undefined,
      itemsCount: filter?.itemsCount
    };

    let result = await axios.get(API_URL, { params });
    return result.data.hotels;
  } catch (e) {
    console.error(e);
  }
};

export const fetchHotel = async (id) => {  
  try {
    let response = await axios.get(`${API_URL}/${id}`);
    return response.data;
  } catch (e) {
    console.error(e);
  }
};

export const postHotel = async (formData) => {
  try {
    const token = getCurrentToken();
    const response = await axios.post(API_URL, formData, {
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

export const putHotel = async (id, formData) => {
  try {
    const token = getCurrentToken();
    console.log(token);
    const response = await axios.put(`${API_URL}/${id}`, formData, {
      headers: {
        'Content-Type': 'multipart/form-data',
        'Authorization': `Bearer ${token}`,
      },
    });
    return response.status;
  } catch (e) {
    console.error(e);
  }
};

export const deleteHotel = async (id) => {
  try {
    const token = getCurrentToken();
    const response = await axios.delete(`${API_URL}/${id}`, {
      headers: {
        'Authorization': `Bearer ${token}`,
      },
    });
    return response.status;
  } catch (e) {
    console.error(e);
  }
};
