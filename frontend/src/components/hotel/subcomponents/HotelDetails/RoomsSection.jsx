import React from "react";
import Rooms from "../../../rooms/Rooms";
import RoomsFilter from "../../../rooms/subcomponents/Rooms/RoomsFilter";

function RoomsSection({ filter, setFilter, onApplyFilters, rooms, hotelName }) {
  return (
    <div className="w-full grid sm:grid-cols-1 md:grid-cols-1 lg:grid-cols-1 xl:grid-cols-1 gap-4 rounded-lg shadow-md border-[3px] border-indigo-300">
      <div className="bg-indigo-200 p-4 rounded-md shadow-md">
        <h3 className="text-2xl font-semibold text-gray-700 mb-4">
          Available Rooms
        </h3>
        <RoomsFilter
          filter={filter}
          setFilter={setFilter}
          onApplyFilters={onApplyFilters}
        />
        <Rooms 
          rooms={rooms}
          checkIn={filter.checkIn}
          checkOut={filter.checkOut}
          hotel={hotelName} 
          className="items-center"/>
      </div>
    </div>
  );
}

export default RoomsSection;