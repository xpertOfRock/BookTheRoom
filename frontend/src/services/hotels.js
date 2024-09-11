import axios from "axios"

export const fetchHotels = async (filter) => {  
    try{
      const result = await axios.get("https://localhost:5286/api/hotel", {
            params: {
              search: filter?.search,
              sortItem: filter?.sortItem,
              sortOrder: filter?.sortOrder
            }
          });

      return result.data.hotels;
    }catch(e){
        console.error(e);
    }
};

export const fetchHotel = async (id) => {  
  try{
      const response = await axios.get("https://localhost:5286/api/hotel/" + id);
      return response.data;
  }catch(e){
      console.error(e);
  }
};

export const postHotel = async (formData) => {
    try {
      const response = await axios.post("https://localhost:5286/api/hotel", formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });      
      return response.status;
  } catch (error) {   
    console.error('Error creating hotel:', error);
  }
};


export const putHotel = async (id, hotel) => {
  try{
    const response = await axios.put("http://localhost:5286/api/hotel/"+ id, hotel);
    return response.status;
  }catch(e){
    console.error(e);
  }
};

export const deleteHotel = async(id) => {
  try{
    const response = await axios.delete("http://localhost:5286/api/hotel/" + id);
    return response.status;
  }catch(e){
    console.error(e);
  }
};