import axios from "axios"
import { getCurrentToken } from "./auth";
const API_URL = "https://localhost:6061/api/apartment";

export const fetchApartments = async (filter) => {
  try {
    const params = {
      search:     filter?.search    ?? undefined,
      sortItem:   filter?.sortItem  ?? undefined,
      sortOrder:  filter?.sortOrder ?? undefined,
      countries:  filter?.countries?.length > 0 ? filter.countries.join(",") : undefined,
      minPrice:   filter?.minPrice  ?? undefined,
      maxPrice:   filter?.maxPrice  ?? undefined,
      itemsCount: filter?.itemsCount ?? 9,
      page:       filter?.page       ?? 1,
    };

    const response = await axios.get(`${API_URL}`, { params });
    return response.data.apartments;
  } catch (e) {
    console.error(e);
  }
};

export const fetchUserApartments = async (filter) => {
    try {
      const token = getCurrentToken();
      
      const params = {
        search: filter?.search ?? undefined,
        sortItem: filter?.sortItem ?? undefined,
        sortOrder: filter?.sortOrder ?? undefined,
        countries: filter?.countries.length > 0 ? filter.countries.join(",") : undefined,
        minPrice: filter?.minPrice ?? undefined,
        maxPrice: filter?.maxPrice ?? undefined,
        itemsCount: filter?.itemsCount ?? 9,
        page: filter?.page ?? 1
      };
  
      const result = await axios.get(`${API_URL}/user-apartments`, { params,       
          headers: {
            'Authorization': `Bearer ${token}`,
          },
        }
      );
      return result.data.apartments;
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