import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import CreateForm from './components/hotel/CreateForm';
import UpdateForm from './components/hotel/UpdateForm';
import Hotels from './components/hotel/Hotels';
import HotelDetails from './components/hotel/HotelDetails';
import CreateRoomForm from './components/rooms/CreateRoomForm';
import UpdateRoomForm from './components/rooms/UpdateRoomForm';
import Navbar from './components/Navbar'; 
import Home from './components/Home'; 
import Apartments from './components/apartment/Apartments';
import FAQ from './components/FAQ';
import Support from './components/Support';
import Login from './components/authorization/Login';
import Register from './components/authorization/Register';
import RoomDetails from './components/rooms/RoomDetails';
import Checkout from './components/checkout/Checkout';
import Success from './components/checkout/Success';

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
          <Route path="/hotels/:id" element={<HotelDetails />} />
          <Route path="/hotels/:id/room/:number" element={<RoomDetails />}/>
          <Route path="/checkout" element={<Checkout />} />
          <Route path="/checkout/success" element={<Success/>} />
          <Route path="/admin/hotels/create" element={<CreateForm/> } />
          <Route path="/admin/hotels/update/:id" element={<UpdateForm /> } />
          <Route path="/admin/hotels/:id/rooms/create" element={<CreateRoomForm /> } />
          <Route path="/admin/hotels/:id/rooms/update/:number" element={<UpdateRoomForm /> } />
          <Route path="/apartments" element={<Apartments />} />
          <Route path="/faq" element={<FAQ />} />
          <Route path="/support" element={<Support />} />
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
        </Routes>        
      </>
    </Router>
  );
}

export default App;
