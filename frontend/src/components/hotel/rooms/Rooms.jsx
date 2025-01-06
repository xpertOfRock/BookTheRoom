function Rooms({ rooms }) {
    return (
        <div className="grid grid-cols-1 gap-4 mt-6 mx-auto">
        {rooms.length > 0 ? (
          rooms.map((room) => (
            <div
              key={room.number}
              className="bg-white p-4 rounded-lg shadow-md hover:shadow-lg transition"
            >
              <h3 className="font-bold text-lg text-gray-800">{room.name}</h3>
              <p className="text-gray-600">Category: {room.category}</p>
              <p className="text-gray-600">Price: ${room.price}</p>
              <p className="text-gray-600">Room Number: {room.number}</p>
            </div>
          ))
        ) : (
          <p className="text-gray-500 col-span-full">
            No rooms found. Adjust your filters.
          </p>
        )}
      </div>
    );
  }
  
  export default Rooms;
  