import './App.css';
import { useEffect, useState } from 'react';
import { fetchHotels } from './Services/hotels';
import HotelCard from './Components/HotelCard';

function App() {

  const [hotels, setHotels] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      var hotels = await fetchHotels();

      console.log(hotels);

      setHotels(hotels);
    }

    fetchData();
  }, [])

  return (
    // 
    <section className="p-8 flex flex-row justify-start itemx-staart gap-12">
      {/*  */}
      <div className="flex flex-col w-1/3 gap-10">
        <h1>Hotels</h1>
      </div>
      {/*  */}
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
    </section>
  );
}

export default App;
