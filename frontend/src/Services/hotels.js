import axios from "axios"

export const fetchHotels = async (filter) => {  
    try{
        var result = await axios.get("http://localhost:5286/api/Hotel", {
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
