import Room from "./subcomponents/Rooms/Room";

function Rooms({ rooms, checkIn, checkOut }) {
    return (
        <div className="grid sm:grid-cols-1 md:grid-cols-2 gap-4 mt-6 mx-auto">
        {rooms.length > 0 ? (
          rooms.map((room) => (
            <Room 
              hotelId={room.hotelId}
              name={room.name} 
              preview={room.preview} 
              number={room.number} 
              category={room.category} 
              checkIn={checkIn} 
              checkOut={checkOut}
            />
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
  