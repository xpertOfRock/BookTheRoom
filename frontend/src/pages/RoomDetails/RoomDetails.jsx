import React, { useEffect, useState, useRef } from "react";
import { useParams, useNavigate, useLocation } from "react-router-dom";
import Slider from "react-slick";

import Lightbox from "yet-another-react-lightbox";
import "yet-another-react-lightbox/styles.css";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft, faChevronRight } from "@fortawesome/free-solid-svg-icons";

import { fetchRoom } from "../../services/rooms"; 
import ImagesSection from "../../components/shared/ImagesSection";

import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";

function RoomDetails() {
  const { id, number } = useParams();
  const navigate = useNavigate();
  const location = useLocation();
  const sliderRef = useRef(null);

  const { checkIn, checkOut, hotel } = location.state || {};
 
  const [room, setRoom] = useState(null);
  const [orderData, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isLightboxOpen, setIsLightboxOpen] = useState(false);
  const [photoIndex, setPhotoIndex] = useState(0);

  const categories = new Map([
    [1, 'One bed apartments'],
    [2, 'Two bed apartments'],
    [3, 'Three bed apartments'],
    [4, 'Luxury'],
  ]);

  useEffect(() => {
    const loadRoom = async () => {
      try {
        if(checkOut === undefined || checkIn === undefined){
            navigate(`/hotels/${id}`);
        }

        const data = await fetchRoom(id, number);

        setData({
          hotelId: id,
          roomNumber: number,
          checkIn: checkIn,
          checkOut: checkOut,
          basePrice: data.price,
          roomName: data.name,
          hotelName: hotel
        });

        setRoom(data);
      } catch (e) {
        console.error("Error fetching room details:", e);
      } finally {
        setLoading(false);
      }
    };
    console.log(`CheckIn: ${checkIn}, CheckOut: ${checkOut}`);
    loadRoom();
  }, [id, number]);

  const sliderSettings = {
    dots: true,
    infinite: true,
    speed: 500,
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: false,
  }; 

  const handleCheckoutRedirection = () => {
    navigate(`/checkout`, {state: orderData});
  }

  const handleImageClick = (index) => {
    setPhotoIndex(index);
    setIsLightboxOpen(true);
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (!room) {
    return <div>Room not found.</div>;
  }

  const { 
    images = [], 
    name, 
    description, 
    price, 
    category 
  } = room;

  const slides = images.map((src) => ({ src }));

  return (
    <section>
      <div className="container mx-auto px-4 w-full">
        <h1 className="text-4xl font-bold my-6 text-gray-800">{name}</h1>

        <button
          className="bg-indigo-500 text-white px-4 py-2 rounded-md shadow-md hover:bg-indigo-700 transition-all"
          onClick={() => navigate(`/hotels/${id}`)}
        >
          <FontAwesomeIcon icon={faChevronLeft} className="mr-2" /> Back
        </button>

        <div className="flex flex-col xl:flex-row items-center gap-6 p-6">
          <div className="xl:w-1/2 w-full lg:h-[432px] relative group indent-auto">
            <Slider {...sliderSettings} ref={sliderRef}>
              {images.map((image, index) => (
                <div key={index} className="w-full h-full">
                  <img
                    className="rounded-lg shadow-md w-auto h-[432px] object-cover mx-auto cursor-pointer"
                    src={image}
                    alt={`Room Image ${index + 1}`}
                    loading="lazy"
                    onClick={() => handleImageClick(index)}
                  />
                </div>
              ))}
            </Slider>

            <button
              className="absolute left-2 bg-gray-800 bg-opacity-50 text-white p-2 rounded-full opacity-0 group-hover:opacity-100 transition-opacity z-10"
              style={{ top: "50%", transform: "translateY(-50%)" }}
              onClick={() => sliderRef.current.slickPrev()}
            >
              <FontAwesomeIcon icon={faChevronLeft} />
            </button>

            <button
              className="absolute right-2 bg-gray-800 bg-opacity-50 text-white p-2 rounded-full opacity-0 group-hover:opacity-100 transition-opacity z-10"
              style={{ top: "50%", transform: "translateY(-50%)" }}
              onClick={() => sliderRef.current.slickNext()}
            >
              <FontAwesomeIcon icon={faChevronRight} />
            </button>
          </div>

          <div className="xl:w-1/2 w-full bg-indigo-200 border-[3px] border-indigo-400 flex flex-col rounded-lg shadow-md my-auto">
            <div className="bg-indigo-100 p-6 rounded-lg shadow-md align-middle my-3 mx-3">
              <h3 className="text-2xl font-semibold text-gray-700">{name}</h3>
              <h4 className="text-gray-600 mb-2">{categories.get(category) ?? 'Standard'}</h4>
              <p className="text-gray-600">{description}</p>
              <p className="mt-4 text-lg font-medium text-gray-900">
                Price: ${price}
              </p>
            </div>
          </div>
        </div>

        <ImagesSection images={images} onImageClick={handleImageClick} />

        <Lightbox
          open={isLightboxOpen}
          close={() => setIsLightboxOpen(false)}
          slides={slides}
          index={photoIndex}
        />
        <button
          onClick={handleCheckoutRedirection}
          className="px-3 py-1 text-sm font-medium text-white bg-blue-500 rounded hover:bg-blue-600 w-full"
        >
          Book this room
        </button>
      </div>
    </section>
  );
}

export default RoomDetails;