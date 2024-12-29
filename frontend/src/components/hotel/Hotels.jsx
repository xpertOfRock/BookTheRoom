import SortAndSearch from './SortAndSearch';
import Filters from './Filters';
import Card from './Card';
import { fetchHotels } from '../../services/hotels';
import { useEffect, useState } from 'react';

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
  
    useEffect(() => {
      const fetchData = async () => {
        try {
          const combinedFilters = { ...sortAndSearch, ...filterBy };
          console.log('Request Payload:', combinedFilters);
          let response = await fetchHotels(combinedFilters);
          console.log(response);
          setHotels(response || []);
        } catch (error) {
          console.error('Error fetching hotels:', error);
          setHotels([]);
        }
      };
  
      fetchData();
    }, [sortAndSearch, filterBy]);
  
    return (
      <section className="p-8 flex flex-col gap-8">
          
        <div className="flex flex-row gap-12">

          <div className="w-1/3 md:w-full sd:w-full lg:w-1/3 bg-indigo-100 p-6 rounded-lg shadow-md">
            <h2 className="text-xl font-semibold mb-4">Filters</h2>
            <Filters filter={filterBy} setFilter={setFilterBy} />
          </div>

          <div className="w-full lg:w-2/3">
            <div className="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-8 bg-indigo-100 p-6 rounded-lg shadow-md">
              <SortAndSearch filter={sortAndSearch} setFilter={setSortAndSearch} />
            </div>

            <h2 className="text-xl font-semibold mx-4 my-4">Hotels</h2>

            <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6 bg-indigo-100 p-6 rounded-lg shadow-md">
              {hotels && hotels.length > 0 ? (
                hotels.slice(0, sortAndSearch.itemsCount).map((hotel) => (
                  <Card
                    key={hotel.id}
                    id={hotel.id}
                    name={hotel.name}
                    preview={hotel.preview}
                    rating={hotel.rating}
                    address={hotel.address}
                  />
                ))
              ) : (
                <p className="text-gray-500">No hotels found.</p>
              )}
            </div>

          </div>

        </div>

      </section>
    );
  }
  
  export default Hotels;
  