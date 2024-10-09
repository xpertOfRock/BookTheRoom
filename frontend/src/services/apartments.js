import axios from "axios"

export const fetchApartments = async (filter) => {
  try {
    const params = {
      search: filter?.search || undefined,
      sortItem: filter?.sortItem || undefined,
      sortOrder: filter?.sortOrder || undefined,
      countries: filter?.countries.length > 0 ? filter.countries.join(",") : undefined,
      prices: filter?.ratings.length > 0 ? filter.ratings.join(",") : undefined,
    };

    let result = await axios.get("https://localhost:5286/api/apartment", { params });
    return result.data.hotels;
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
        prices: filter?.ratings.length > 0 ? filter.ratings.join(",") : undefined,
      };
  
      let result = await axios.get("https://localhost:5286/api/apartment/user", { params });
      return result.data.hotels;
    } catch (e) {
      console.error(e);
    }
};

export const fetchApartment = async (id) => {  
  try{
      let response = await axios.get("https://localhost:5286/api/apartment/" + id);
      return response.data;
  }catch(e){
      console.error(e);
  }
};

export const postApartment = async (formData) => {
    try {
      const response = await axios.post("https://localhost:5286/api/apartment", formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });      
      return response.status;
  } catch (error) {   
    console.error('Error creating hotel:', error.response ? error.response.data : error.message);
  }
};


export const putApartment = async (id, apartment) => {
  try{
    const response = await axios.put("https://localhost:5286/api/apartment/"+ id, apartment);
    return response.status;
  }catch(e){
    console.error(e);
  }
};

export const deleteApartment = async(id) => {
  try{
    const response = await axios.delete("https://localhost:5286/api/hotel/" + id);
    return response.status;
  }catch(e){
    console.error(e);
  }
};