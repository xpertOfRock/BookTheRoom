import axios from "axios"

export const fetchHotels = async (filter) => {  
    try{
        var result = await axios.get("https://localhost:5286/api/hotel", {
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
      var result = await axios.get("https://localhost:5286/api/hotel/" + id);
      return result.data;
  }catch(e){
      console.error(e);
  }
};



export const postHotel = async (hotelData, files) => {
  try {
    const formData = new FormData();
    
    formData.append("request", new Blob([JSON.stringify(hotelData)], {
      type: "application/json"
    }));
    
    if (files && files.length > 0) {
      files.forEach(file => {
        formData.append("Files", file);
      });
    }

    const response = await axios.post("https://localhost:5286/api/hotel", formData, {
      headers: {
        'Content-Type': 'multipart/form-data'
      }
    });

    return response.status;
  } catch (e) {
    console.error(e);
  }
};

export const putHotel = async (id, hotel) => {
  try{
    var response = await axios.put("http://localhost:5286/api/hotel/"+ id, hotel);
    return response.status;
  }catch(e){
    console.error(e);
  }
};

export const deleteHotel = async(id) => {
  try{
    var response = await axios.delete("http://localhost:5286/api/hotel/" + id);
    return response.status;
  }catch(e){
    console.error(e);
  }
};