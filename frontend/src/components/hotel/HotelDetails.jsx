import React, { useEffect, useState, useRef } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Slider from "react-slick";

import Lightbox from "yet-another-react-lightbox";
import "yet-another-react-lightbox/styles.css";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft, faChevronRight } from "@fortawesome/free-solid-svg-icons";

import { fetchHotel } from "../../services/hotels";
import { fetchRooms } from "../../services/rooms";
import { getCurrentUserId } from "../../services/auth";

import RoomsSection from "./subcomponents/HotelDetails/RoomsSection";
import CommentSection from "./subcomponents/HotelDetails/CommentsSection";
import ImagesSection from "../shared/ImagesSection";

import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";

function HotelDetails() {
  const { id: id } = useParams();
  const navigate = useNavigate();
  const sliderRef = useRef(null);

  const [hotel, setHotel] = useState(null);
  const [loading, setLoading] = useState(true);
  const [rooms, setRooms] = useState([]);
  const [filter, setFilter] = useState({
    search: "",
    sortItem: "name",
    sortOrder: "asc",
    categories: [],
    minPrice: null,
    maxPrice: null,
    checkIn: null,
    checkOut: null,
  });

  const [isLightboxOpen, setIsLightboxOpen] = useState(false);
  const [photoIndex, setPhotoIndex] = useState(0);

  const currentUserId = getCurrentUserId();

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

  const handleFilterApply = async () => {
    try {
      const roomsData = await fetchRooms(id, filter);
      setRooms(roomsData);
    } catch (e) {
      console.error("Error fetching rooms:", e);
    }
  };

  const hasRatedComments =
    Array.isArray(hotel?.comments) &&
    hotel.comments.some(
      (c) =>
        c.userId === currentUserId &&
        c.userScore !== undefined &&
        c.userScore !== null &&
        c.userScore > 0
    );

  const sliderSettings = {
    dots: true,
    infinite: true,
    speed: 500,
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: false,
  };

  const handleImageClick = (index) => {
    setPhotoIndex(index);
    setIsLightboxOpen(true);
  };

  if (loading) {
    return <div>Loading...</div>;
  }

  if (!hotel) {
    return <div>Hotel not found.</div>;
  }

  const { images = [], name, userScore, address, description } = hotel;

  const slides = images.map((src) => ({ src }));

  return (
    <section>
      <div className="container mx-auto px-4 w-full">
        <h1 className="text-4xl font-bold my-6 text-gray-800">{name}</h1>

        <button
          className="bg-indigo-500 text-white px-4 py-2 rounded-md shadow-md hover:bg-indigo-700 transition-all"
          onClick={() => navigate("/hotels")}
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
                    alt={`Hotel Image ${index + 1}`}
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
              <h4>
                {userScore !== undefined && userScore !== null && userScore >= 0
                  ? `User score  ${userScore.toFixed(1)} ★`
                  : "Has not been rated by users yet"}
              </h4>
              <h4>{address}</h4>
              <p className="text-gray-600">{description}</p>
            </div>
          </div>
          
        </div>        

        <ImagesSection images={images} onImageClick={handleImageClick} />

        <div className="p-6 grid-cols-2 lg:flex-row gap-4 sm:grid-cols-2 flex flex-col">
          <div className="lg:w-2/3">
            <RoomsSection
              hotelId={hotel.id}            
              filter={filter}
              setFilter={setFilter}
              onApplyFilters={handleFilterApply}
              rooms={rooms}
              hotelName={name}
            />
          </div>

          <div classname="lg:w-1/3">
            <CommentSection
              hotel={hotel}
              hasRatedComments={hasRatedComments}
              currentUserId={currentUserId}
            />
          </div>
          

          
        </div>

        <Lightbox
          open={isLightboxOpen}
          close={() => setIsLightboxOpen(false)}
          slides={slides}
          index={photoIndex}
        />
      </div>
    </section>
  );
}

export default HotelDetails;