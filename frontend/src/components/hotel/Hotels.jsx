// import SortAndSearch from './subcomponents/Hotels/SortAndSearch';
// import Filters from './subcomponents/Hotels/Filters';
// import Card from './subcomponents/Hotels/Card';
// import { fetchHotels } from '../../services/hotels';
// import { useEffect, useState } from 'react';

// function Hotels() {
//     const [hotels, setHotels] = useState([]);
//     const [sortAndSearch, setSortAndSearch] = useState({
//       search: '',
//       sortItem: 'id',
//       sortOrder: 'desc',
//       itemsCount: 15,
//     });
  
//     const [filterBy, setFilterBy] = useState({
//       countries: [],
//       ratings: [],
//     });
  
//     useEffect(() => {
//       const fetchData = async () => {
//         try {
//           const combinedFilters = { ...sortAndSearch, ...filterBy };
//           let response = await fetchHotels(combinedFilters);
//           setHotels(response || []);
//         } catch (error) {
//           console.error('Error fetching hotels:', error);
//           setHotels([]);
//         }
//       };
  
//       fetchData();
//     }, [sortAndSearch, filterBy]);
  
//     return (
//       <section className="flex flex-col items-center justify-center p-8 gap-8">
//         <div className="flex flex-col lg:flex-row gap-8 w-full max-w-7xl">

//           <div className="w-full lg:w-1/4 bg-indigo-100 p-6 rounded-lg shadow-md border-[3px] border-indigo-300">
//             <h2 className="text-xl font-semibold mb-4">Filters</h2>
//             <Filters filter={filterBy} setFilter={setFilterBy} />
//           </div>

//           <div className="w-full lg:w-3/4 flex flex-col gap-6">
//             <div className="bg-indigo-100 p-6 rounded-lg shadow-md border-[3px] border-indigo-300">
//               <SortAndSearch filter={sortAndSearch} setFilter={setSortAndSearch} />
//             </div>

//             <h2 className="text-xl font-semibold">Hotels</h2>
//             <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 bg-indigo-100 p-6 rounded-lg shadow-md border-[3px] border-indigo-300">
//               {hotels && hotels.length > 0 ? (
//                 hotels.slice(0, sortAndSearch.itemsCount).map((hotel) => (
//                   <Card
//                     key={hotel.id}
//                     id={hotel.id}
//                     name={hotel.name}
//                     preview={hotel.preview}
//                     rating={hotel.rating}
//                     address={hotel.address}
//                     userScore={hotel.userScore}
//                   />
//                 ))
//               ) : (
//                 <p className="text-gray-500">No hotels found.</p>
//               )}
//             </div>
//           </div>
//         </div>
//       </section>
//     );
//   }
  
//   export default Hotels;

import { useEffect, useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBars, faTimes } from '@fortawesome/free-solid-svg-icons';
import SortAndSearch from './subcomponents/Hotels/SortAndSearch';
import Filters from './subcomponents/Hotels/Filters';
import Card from './subcomponents/Hotels/Card';
import { fetchHotels } from '../../services/hotels';

function Hotels() {
  const [hotels, setHotels] = useState([]);
  const [sortAndSearch, setSortAndSearch] = useState({
    search: '',
    sortItem: 'id',
    sortOrder: 'desc',
    itemsCount: 15,
  });
  const [filterBy, setFilterBy] = useState({
    countries: [],
    ratings: [],
  });
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const combinedFilters = { ...sortAndSearch, ...filterBy };
        const response = await fetchHotels(combinedFilters);
        setHotels(response || []);
      } catch (error) {
        console.error('Error fetching hotels:', error);
        setHotels([]);
      }
    };

    fetchData();
  }, [sortAndSearch, filterBy]);

  return (
    <div className="relative min-h-screen bg-gray-50 pt-16">
      <button
        onClick={() => setIsSidebarOpen(!isSidebarOpen)}
        className="absolute top-4 left-4 z-50 flex items-center px-3 py-2 text-indigo-700 bg-white rounded-md shadow-md"
      >
        <FontAwesomeIcon icon={isSidebarOpen ? faTimes : faBars} size="lg" />
        <span className="ml-2">Filters</span>
      </button>

      {isSidebarOpen && (
        <div
          className="absolute inset-0 bg-black bg-opacity-50 z-40"
          onClick={() => setIsSidebarOpen(false)}
        />
      )}

      <aside
        className={`absolute top-0 left-0 bottom-0 w-64 bg-indigo-100 p-6 rounded-r-lg shadow-md border border-indigo-300
          transform transition-transform duration-300 z-50
          ${isSidebarOpen ? 'translate-x-0' : '-translate-x-full'}
        `}
      >
        <h2 className="text-xl font-semibold mb-4">Filters</h2>
        <Filters filter={filterBy} setFilter={setFilterBy} />
      </aside>

      <main
        className={`transition-all duration-300 ${
          isSidebarOpen ? 'opacity-90' : 'opacity-100'
        } p-8`}
      >
        <div className="mb-6">
          <div className="bg-indigo-100 p-6 rounded-lg shadow-md border border-indigo-300">
            <SortAndSearch filter={sortAndSearch} setFilter={setSortAndSearch} />
          </div>
        </div>
        <h2 className="text-xl font-semibold mb-4">Hotels</h2>
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
          {hotels && hotels.length > 0 ? (
            hotels.slice(0, sortAndSearch.itemsCount).map((hotel) => (
              <Card
                key={hotel.id}
                id={hotel.id}
                name={hotel.name}
                preview={hotel.preview}
                rating={hotel.rating}
                address={hotel.address}
                userScore={hotel.userScore}
              />
            ))
          ) : (
            <p className="text-gray-500">No hotels found.</p>
          )}
        </div>
      </main>
    </div>
  );
}

export default Hotels;
