import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';

import CreateHotelForm from './components/hotel/CreateHotelForm';
import UpdateHotelForm from './components/hotel/UpdateHotelForm';
import Hotels from './components/hotel/Hotels';
import Navbar from './components/Navbar'; 
import Home from './components/Home'; 
import Apartments from './components/apartment/Apartments';
import FAQ from './components/FAQ';
import Support from './components/Support';

function App() { 
  return (
    <Router>
      <>
        <header>
          <Navbar />
        </header>

        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/hotels" element={<Hotels />} />
          <Route path="/hotels/create" element={<CreateHotelForm/> } />
          <Route path="/hotels/update/:id" element={<UpdateHotelForm /> } />
          <Route path="/apartments" element={<Apartments />} />
          <Route path="/faq" element={<FAQ />} />
          <Route path="/support" element={<Support />} />
        </Routes>

        {/* <section className="p-8 flex flex-row justify-start itemx-staart gap-12">
          <div className="flex flex-col w-1/2 gap-10">
            <h1>Create hotel</h1>
                      
          </div>
          
          <div className="flex flex-col w-1/2 gap-10">
            <h1>Available hotels:</h1>
            <Hotels hotels={hotels} setFilter={setFilter} filter={filter}/>         
          </div>  
        </section> */}
      </>
    </Router>
  );
}

export default App;
