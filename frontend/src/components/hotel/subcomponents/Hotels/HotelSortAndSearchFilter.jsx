function HotelSortAndSearchFilter({ filter, setFilter }) {
  const handleChange = (e) => {
    const { name, value } = e.target;
    const parsedValue = name === "itemsCount" ? parseInt(value, 10) : value;
    setFilter({ ...filter, [name]: parsedValue });
  };

  return (
    <div className="flex flex-col lg:flex-row lg:items-center lg:gap-6 gap-4 w-full justify-center">
      <div className="flex flex-col w-full lg:w-auto">
        <label className="block text-sm font-medium text-gray-700 mb-2">Search:</label>
        <input
          type="text"
          name="search"
          value={filter.search}
          onChange={handleChange}
          placeholder="Search..."
          className="border border-gray-300 rounded p-2 w-full"
        />
      </div>

      <div className="flex flex-col w-full lg:w-auto">
        <label className="block text-sm font-medium text-gray-700 mb-2">Sort by:</label>
        <select
          name="sortItem"
          value={filter.sortItem}
          onChange={handleChange}
          className="border border-gray-300 rounded p-2 w-full"
        >
          <option value="id">Default</option>
          <option value="name">Name</option>
          <option value="rating">Rating</option>
        </select>
      </div>

      <div className="flex flex-col w-full lg:w-auto">
        <label className="block text-sm font-medium text-gray-700 mb-2">Order by:</label>
        <select
          name="sortOrder"
          value={filter.sortOrder}
          onChange={handleChange}
          className="border border-gray-300 rounded p-2 w-full"
        >
          <option value="desc">Descending</option>
          <option value="asc">Ascending</option>
        </select>
      </div>

      <div className="flex flex-col w-full lg:w-auto">
        <label className="block text-sm font-medium text-gray-700 mb-2">Items per page:</label>
        <select
          name="itemsCount"
          value={filter.itemsCount || 6}
          onChange={handleChange}
          className="border border-gray-300 rounded p-2 w-full"
        >
          <option value={6}>6</option>
          <option value={9}>9</option>
          <option value={12}>12</option>
          <option value={15}>15</option>
        </select>
      </div>
    </div>
  );
}

export default HotelSortAndSearchFilter;