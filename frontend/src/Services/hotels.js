import axios from "axios"

export const fetchHotels = async () => {  
    try{
        var result = await axios.get("http://localhost:5286/api/Hotel");
        console.log(result);

        return result.data.hotels;
    }catch(e){
        console.error(e);
    }
};
