function Room({ name,  preview, number, category }) {
  
  const categories = new Map([
    [1, 'One bed apartments'],
    [2, 'Two bed apartments'],
    [3, 'Three bed apartments'],
    [4, 'Luxury'],
  ]);

  return (    
    <div className="relative max-w-sm bg-white border border-gray-200 rounded-lg shadow-md m-4 flex flex-col">
      <img
        className="rounded-t-lg w-full h-60 object-cover"
        src={preview}
        alt="Hotel preview"
        loading="lazy"
      />
      <div className="p-3 flex flex-col flex-grow">
        <h5 className="text-lg font-semibold text-gray-800">{name}</h5>
        <p className="text-gray-500 mt-2 text-sm">Room №{number}</p>
        <div className="flex-grow"></div>
        <div className="flex items-center justify-between mt-4">
          <button
            // onClick={handleViewClick}
            className="px-3 py-1 text-sm font-medium text-white bg-blue-500 rounded hover:bg-blue-600"
          >
            View
          </button>
          <small className="text-gray-400">{categories.get(category) ?? 'Standard'}</small>
        </div>
      </div>
    </div>
  );
}

export default Room;