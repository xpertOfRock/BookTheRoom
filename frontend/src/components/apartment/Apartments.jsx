import { useEffect, useState } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBars, faTimes } from '@fortawesome/free-solid-svg-icons';
import ApartmentSortAndSearchFilter from './ApartmentSortAndSearchFilter';
import ApartmentFilter from './ApartmentFilter';
import ApartmentCard from './ApartmentCard';
import { fetchApartments } from '../../services/apartments';

function Apartments() {
  const [apartments, setApartments] = useState([]);
  const [isSidebarOpen, setIsSidebarOpen] = useState(false);

  const [sortAndSearch, setSortAndSearch] = useState({
    search: '',
    sortItem: 'id',
    sortOrder: 'desc',
    itemsCount: 9,
  });

  const [filterBy, setFilterBy] = useState({
    countries: [],
    minPrice: undefined,
    maxPrice: undefined,
  });

  useEffect(() => {
    const fetchData = async () => {
      try {
        const combinedFilters = { ...sortAndSearch, ...filterBy };
        const response = await fetchApartments(combinedFilters);
        setApartments(response || []);
      } catch (error) {
        console.error('Error fetching apartments:', error);
        setApartments([]);
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
        className={`
        absolute top-0 left-0 bottom-0
        bg-indigo-100 p-4 sm:p-6
        w-full sm:w-64
        h-full overflow-y-auto
        border border-indigo-300
        transform transition-transform duration-300 z-50
        ${isSidebarOpen ? 'translate-x-0' : '-translate-x-full'}
        sm:rounded-r-lg
      `}
      >
        <h2 className="text-xl font-semibold mb-4">Filters</h2>
        <ApartmentFilter filter={filterBy} setFilter={setFilterBy} />
      </aside>

      <main
        className={`transition-all duration-300 ${
          isSidebarOpen ? 'opacity-90' : 'opacity-100'
        } p-8`}
      >
        <div className="mb-6">
          <div className="bg-indigo-100 p-6 rounded-lg shadow-md border border-indigo-300">
            <ApartmentSortAndSearchFilter
              filter={sortAndSearch}
              setFilter={setSortAndSearch}
            />
          </div>
        </div>

        <h2 className="text-xl font-semibold mb-4">Apartments</h2>
        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-6">
          {apartments && apartments.length > 0 ? (
            apartments.slice(0, sortAndSearch.itemsCount).map((apt) => (
              <ApartmentCard
                key={apt.id}
                id={apt.id}
                title={apt.title}
                priceForNight={apt.priceForNight}
                address={apt.address}
                preview={apt.preview}
              />
            ))
          ) : (
            <p className="text-gray-500">No apartments found.</p>
          )}
        </div>
      </main>
    </div>
  );
}

export default Apartments;