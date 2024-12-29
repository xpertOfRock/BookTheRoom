
function SortAndSearch({ filter, setFilter }) {

  const handleChange = (e) => {
    const { name, value } = e.target;
    const parsedValue = name === 'itemsCount' ? parseInt(value, 10) : value;
    setFilter({ ...filter, [name]: parsedValue });
    console.log(`Updated ${name}:`, parsedValue);
  };
  
    return (
      <div className="flex flex-col lg:flex-row lg:items-center lg:gap-4">
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-4">Search:</label>
          <input
            type="text"
            name="search"
            value={filter.search}
            onChange={handleChange}
            placeholder="Search..."
            className="border border-gray-300 rounded p-2 w-full lg:w-60"
          />
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-4">Sort by:</label>
          <select
            name="sortItem"
            value={filter.sortItem}
            onChange={handleChange}
            className="border border-gray-300 rounded p-2 w-full lg:w-40"
          >
            <option value="id">Default</option>
            <option value="name">Name</option>
            <option value="rating">Rating</option>
          </select>
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-4">Order by:</label>
          <select
            name="sortOrder"
            value={filter.sortOrder}
            onChange={handleChange}
            className="border border-gray-300 rounded p-2 w-full lg:w-40"
          >
            <option value="desc">Descending</option>
            <option value="asc">Ascending</option>
          </select>
        </div>
        <div>
          <label className="block text-sm font-medium text-gray-700 mb-4">Items per page:</label>
          <select
          name="itemsCount"
          value={filter.itemsCount || 15}
          onChange={handleChange}
          className="border border-gray-300 rounded p-2 w-full lg:w-40"
        >
          <option value={15}>15</option>
          <option value={30}>30</option>
          <option value={45}>45</option>
          <option value={60}>60</option>
        </select>
        </div>
      </div>
    );
  }
  
export default SortAndSearch;