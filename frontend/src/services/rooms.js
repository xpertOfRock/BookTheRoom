import axios from "axios"

// "https://localhost:5286/api/room";
const API_URL = "https://localhost:6061/api/room";

export const fetchRooms = async (filter) => {
  try {
    const params = {
      search: filter?.search || undefined,
      sortItem: filter?.sortItem || undefined,
      sortOrder: filter?.sortOrder || undefined,
      countries: filter?.countries.length > 0 ? filter.countries.join(",") : undefined,
      prices: filter?.ratings.length > 0 ? filter.ratings.join(",") : undefined,
    };

    let result = await axios.get(API_URL, { params });
    return result.data.hotels;
  } catch (e) {
    console.error(e);
  }
};


export const fetchRoom = async (id) => {  
  try{
      let response = await axios.get(`${API_URL}/${id}`);
      return response.data;
  }catch(e){
      console.error(e);
  }
};

export const postRoom = async (hotelId, formData) => {
  try {
    const token = getToken();
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


export const putRoom = async (id, number, formData) => {
  try {
    const token = getToken();
    const response = await axios.put(`${API_URL}/${id}/${number}`, formData, {
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

export const deleteRoom = async(id) => {
  try {
    const token = getToken();
    const response = await axios.delete(`${API_URL}/${id}/${number}`, {
      headers: {
        'Authorization': `Bearer ${token}`,
      },
    });
    return response.status;
  } catch (e) {
    console.error(e);
  }
};