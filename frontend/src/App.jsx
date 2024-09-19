import './App.css';
import { useEffect, useState } from 'react';
import { fetchHotels, postHotel } from './services/hotels';
import HotelCard from './components/HotelCard';
import Filter from './components/Filter';
import CreateHotelForm from './components/CreateHotelForm';

function App() {

  const [hotels, setHotels] = useState([]);
  const [filter, setFilter] = useState({
    search: "",
    sortItem: "id",
    sortOrder: "asc"
  });


  useEffect(() => {
    const fetchData = async () => {
      var hotels = await fetchHotels(filter);

      console.log(hotels);

      setHotels(hotels);
    }

    fetchData();
  }, [filter]);
  
  const onCreate = async (hotelForm) => {
      await postHotel(hotelForm);
      var hotels = await fetchHotels(filter);
      setHotels(hotels);
  }

  return (
    <section className="p-8 flex flex-row justify-start itemx-staart gap-12">
      <div className="flex flex-col w-1/2 gap-10">
        <h1>Create hotel</h1>
        <CreateHotelForm onCreate={onCreate}/>
        <h1>Filters</h1>
        <Filter filter={filter} setFilter={setFilter}/>
      </div>

      <div className="flex flex-col w-1/2 gap-10">
        <h1>Hotels</h1>      
          <ul className="flex flex-col gap-5 w-1/2">
          {hotels.map((h) => (
            <li key = {h.id}>
              <ul>
                <HotelCard
                  name={h.name}
                  rating={h.rating}
                  address={h.address}
                  preview={h.preview}
                />
              </ul>
            </li>
          ))}
        </ul>        
      </div>
      </section>
  );
}

export default App;
