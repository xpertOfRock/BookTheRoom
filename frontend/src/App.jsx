import './App.css';
import { useEffect, useState } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { fetchHotels, postHotel } from './services/hotels';
import CreateHotelForm from './components/CreateHotelForm';
import Hotels from './components/Hotels';
import Navbar from './components/Navbar'; 
import Home from './components/Home'; 
import Apartments from './components/Apartments';
import FAQ from './components/FAQ';
import Support from './components/Support';

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
    <Router>
      <>
        <header>
          <Navbar />
        </header>

        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/hotels" element={<Hotels hotels={hotels} setFilter={setFilter} filter={filter} />} />
          <Route path="/apartments" element={<Apartments />} />
          <Route path="/faq" element={<FAQ />} />
          <Route path="/support" element={<Support />} />
        </Routes>

        <section className="p-8 flex flex-row justify-start itemx-staart gap-12">
          <div className="flex flex-col w-1/2 gap-10">
            <h1>Create hotel</h1>
            <CreateHotelForm onCreate={onCreate} />           
          </div>
          
          <div className="flex flex-col w-1/2 gap-10">
            <h1>Available hotels:</h1>
            <Hotels hotels={hotels} setFilter={setFilter} filter={filter}/>         
          </div>  
        </section>
      </>
    </Router>
  );
}

export default App;
