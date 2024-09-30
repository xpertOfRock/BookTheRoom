import './App.css';
import { useEffect, useState } from 'react';
import { fetchHotels, postHotel } from './services/hotels';
import CreateHotelForm from './components/CreateHotelForm';
import Hotels from './components/Hotels';

function App() {
  const [hotels, setHotels] = useState([]);
  const [filter, setFilter] = useState({        
    search: "",
    sortItem: "id",
    sortOrder: "asc",
    countries: [],
    ratings: []
  });

  useEffect(() => {
    const fetchData = async () => {
      let hotels = await fetchHotels(filter);
      console.log("Sending filter to backend:", filter);
      console.log(hotels);

      setHotels(hotels);
    }

    fetchData();
  }, [filter]);

  const onCreate = async (hotelForm) => {
    await postHotel(hotelForm);
    let hotels = await fetchHotels(filter);
    setHotels(hotels);
  }

  return (
    <section className="p-8 flex flex-row justify-start itemx-staart gap-12">
      <div className="flex flex-col w-1/2 gap-10">
        <h1>Create hotel</h1>
        <CreateHotelForm onCreate={onCreate} />           
      </div>

      <div className="flex flex-col w-1/2 gap-10">
        <h1>Awailable hotels:</h1>
        <Hotels hotels={hotels} setFilter={setFilter} filter={filter}/>         
      </div>  
    </section>
  );
}

export default App;
