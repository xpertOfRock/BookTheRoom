import axios from "axios"
import { getCurrentToken } from "./auth";
const API_URL = "https://localhost:6061/api/apartment";
const HUB_URL = "https://localhost:6061/hubs/apartment/chat";

export const fetchApartments = async (filter) => {
  try {
    const params = {
      search: filter?.search || undefined,
      sortItem: filter?.sortItem || undefined,
      sortOrder: filter?.sortOrder || undefined,
      countries: filter?.countries.length > 0 ? filter.countries.join(",") : undefined,
      minPrice: filter?.minPrice || undefined,
      maxPrice: filter?.maxPrice || undefined,
      itemsCount: filter?.itemsCount || 9,
      page: filter?.page || 1
    };

    const result = await axios.get(`${API_URL}`, { params });
    console.log(result.data);
    if (result.status !== 200) {
        if (result.status === 401) {
          alert("Unauthorized: required authorization.");
        } else if (result.status === 403) {
          alert("You cannot add a new record.");
        } else {
          alert("An error occurred while creating new apartment.");
        }
      }
    return result.data;
  } catch (e) {
    console.error(e);
  }
};

export const fetchUserApartments = async (filter) => {
    try {
      const params = {
        search: filter?.search || undefined,
        sortItem: filter?.sortItem || undefined,
        sortOrder: filter?.sortOrder || undefined,
        countries: filter?.countries.length > 0 ? filter.countries.join(",") : undefined,
        minPrice: filter?.minPrice || undefined,
        maxPrice: filter?.maxPrice || undefined,
        itemsCount: filter?.itemsCount || 9,
        page: filter?.page || 1
      };
  
      const result = await axios.get(`${API_URL}/user`, { params });
      return result.data.hotels;
    } catch (e) {
      console.error(e);
    }
};

export const fetchApartment = async (id) => {  
  try{
      const response = await axios.get(`${API_URL}/${id}`);
      return response.data;
  }catch(e){
      console.error(e);
  }
};

export const postApartment = async (formData) => {
    try {
      const token = getCurrentToken();

      const response = await axios.post(`${API_URL}`, formData, {
        headers: {
        'Content-Type': 'multipart/form-data',
        'Authorization': `Bearer ${token}`,
      },
      });      
      return response.status;
  } catch (error) {   
    console.error('Error occured while creating apartment:', error.response ? error.response.data : error.message);
  }
};


export const putApartment = async (id, apartment) => {
  try{
    const response = await axios.put(`${API_URL}/${id}`, apartment);
    return response.status;
  }catch(e){
    console.error(e);
  }
};

export const deleteApartment = async(id) => {
  try{
    const response = await axios.delete(`${API_URL}/${id}`);
    return response.status;
  }catch(e){
    console.error(e);
  }
};

// const joinChat = async (userName, chatId) => {
    
//     const { id: id } = useParams();
//     const navigate = useNavigate();

//     var connection = new HubConnectionBuilder()
//         .withUrl(`${HUB_URL}`)
//         .withAutomaticReconnect()
//         .build();

//     try{
//         await connection.start();
//         await connection.invoke("JoinChat", { userName, chatId });
//         navigate(`/Apartments/${id}/Chats/${chatId}`);
//     }catch(error){
//         console.error(error);
//     }
// }