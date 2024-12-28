import Slider from "react-slick";
import React, { useEffect, useState, useRef } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft, faChevronRight } from "@fortawesome/free-solid-svg-icons";
import { fetchHotel } from "../../services/hotels";

import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";

function HotelDetails() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [hotel, setHotel] = useState(null);
  const [loading, setLoading] = useState(true);

  const sliderRef = useRef(null);

  useEffect(() => {
    const loadHotel = async () => {
      try {
        const data = await fetchHotel(id);
        setHotel(data);
      } catch (e) {
        console.error("Error fetching hotel details:", e);
      } finally {
        setLoading(false);
      }
    };

    loadHotel();
  }, [id]);

  if (loading) {
    return <div>Loading...</div>;
  }

  if (!hotel) {
    return <div>Hotel not found.</div>;
  }

  const address = JSON.parse(hotel.jsonAddress || "{}");

  const settings = {
    dots: true,
    infinite: true,
    speed: 500,
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: false,
  };

  return (
    <div className="container mx-auto px-4">
      <h1 className="text-4xl font-bold my-6 text-gray-800">{hotel.name}</h1>
      <button
        className="bg-blue-500 text-white px-4 py-2 rounded-md shadow-md hover:bg-blue-600 transition-all"
        onClick={() => navigate("/hotels")}
      >
        <FontAwesomeIcon icon={faChevronLeft} className="mr-2" /> Back
      </button>

      <div className="flex flex-col lg:flex-row gap-8 mt-6">
        <div className="lg:w-2/3 relative group">
          <Slider {...settings} ref={sliderRef}>
            {hotel.images.map((image, index) => (
              <div key={index} className="relative">
                <img
                  className="rounded-lg shadow-md w-full h-[400px] object-cover"
                  src={image}
                  alt={`Image ${index + 1}`}
                />
              </div>
            ))}
          </Slider>

          <button
            className="absolute left-2 bg-gray-800 bg-opacity-50 text-white p-2 rounded-full opacity-0 group-hover:opacity-100 transition-opacity z-10"
            style={{ top: "50%", transform: "translateY(-50%)" }} // Центрируем стрелку
            onClick={() => sliderRef.current.slickPrev()}
          >
            <FontAwesomeIcon icon={faChevronLeft} />
          </button>

          <button
            className="absolute right-2 bg-gray-800 bg-opacity-50 text-white p-2 rounded-full opacity-0 group-hover:opacity-100 transition-opacity z-10"
            style={{ top: "50%", transform: "translateY(-50%)" }} // Центрируем стрелку
            onClick={() => sliderRef.current.slickNext()}
          >
            <FontAwesomeIcon icon={faChevronRight} />
          </button>
        </div>

        <div className="lg:w-1/3 flex flex-col gap-4">
          <h3 className="text-2xl font-semibold text-gray-700">{hotel.name}</h3>
          <p className="text-gray-600">{hotel.description}</p>
          <div>
            <h3 className="text-lg font-medium text-gray-800">Location:</h3>
            <ul className="list-disc ml-6 text-gray-600">
              <li>Country: {address.Country}</li>
              <li>State: {address.State}</li>
              <li>City: {address.City}</li>
              <li>Street: {address.Street}</li>
              <li>Postal Code: {address.PostalCode}</li>
            </ul>
          </div>
          <button
            className="bg-green-500 text-white px-4 py-2 rounded-md shadow-md hover:bg-green-600 transition-all"
            onClick={() => navigate(`/rooms/${hotel.id}`)}
          >
            View Rooms
          </button>
        </div>
      </div>

      <h3 className="text-2xl font-semibold mt-8 text-gray-700">More Images</h3>
      <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 mt-4">
        {hotel.images.map((image, index) => (
          <img
            key={index}
            className="rounded-lg shadow-md w-full h-[200px] object-cover"
            src={image}
            alt={`Image ${index + 1}`}
          />
        ))}
      </div>
    </div>
  );
}

export default HotelDetails;