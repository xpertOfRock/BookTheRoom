// import Slider from "react-slick";
// import React, { useEffect, useState, useRef } from "react";
// import { useParams, useNavigate } from "react-router-dom";
// import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
// import { faChevronLeft, faChevronRight } from "@fortawesome/free-solid-svg-icons";
// import { fetchHotel } from "../../services/hotels";
// import { fetchRooms } from "../../services/rooms";
// import { getCurrentUserId } from "../../services/auth";
// import RoomsFilter from "./rooms/RoomsFilter";
// import Rooms from "./rooms/Rooms";
// import Comment from "../comment/Comment";
// import CreateCommentForm from "../comment/CreateCommentForm";

// import "slick-carousel/slick/slick.css";
// import "slick-carousel/slick/slick-theme.css";

// function HotelDetails() {

//   const { id } = useParams();
//   const navigate = useNavigate();
//   const [hotel, setHotel] = useState(null);
//   const [loading, setLoading] = useState(true);
//   const [rooms, setRooms] = useState([]);
//   const [filter, setFilter] = useState({
//     search: "",
//     sortItem: "name",
//     sortOrder: "asc",
//     categories: [],
//     minPrice: null,
//     maxPrice: null,
//     checkIn: null,
//     checkOut: null,
//   });

//   const sliderRef = useRef(null);

//   const currentUserId = getCurrentUserId();

//   useEffect(() => {
//     const loadHotel = async () => {
//       try {
//         const data = await fetchHotel(id);
//         setHotel(data);
//       } catch (e) {
//         console.error("Error fetching hotel details:", e);
//       } finally {
//         setLoading(false);
//       }
//     };

//     loadHotel();
//   }, [id]);

//   const handleFilterApply = async () => {
//     try {
//       const roomsData = await fetchRooms(id, filter);
//       setRooms(roomsData);
//     } catch (e) {
//       console.error("Error fetching rooms:", e);
//     }
//   };

//   if (loading) {
//     return <div>Loading...</div>;
//   }

//   if (!hotel) {
//     return <div>Hotel not found.</div>;
//   }

//   const settings = {
//     dots: true,
//     infinite: true,
//     speed: 500,
//     slidesToShow: 1,
//     slidesToScroll: 1,
//     arrows: false,
//   };

//   const hasRatedComments =
//     Array.isArray(hotel.comments) &&
//     hotel.comments.some(
//       (c) =>
//         c.userId === currentUserId &&
//         c.userScore !== undefined &&
//         c.userScore !== null &&
//         c.userScore > 0
//     );

//   return (
//     <section>
//       <div className="container mx-auto px-4 w-5/6">
//         <h1 className="text-4xl font-bold my-6 text-gray-800">{hotel.name}</h1>
//         <button
//           className="bg-indigo-500 text-white px-4 py-2 rounded-md shadow-md hover:bg-indigo-700 transition-all"
//           onClick={() => navigate("/hotels")}
//         >
//           <FontAwesomeIcon icon={faChevronLeft} className="mr-2" /> Back
//         </button>

//         <div className="p-6 grid-cols-1 sm:grid-cols-2 flex flex-col lg:flex-row gap-4">
//           <div className="w-3/7 h-[432px] xl:w-2/5 lg:w-3/7 relative group sm:h-80 md:h-[432px]">
//             <Slider {...settings} ref={sliderRef}>
//               {hotel.images.map((image, index) => (
//                 <div key={index} className="w-full h-full">
//                   <img
//                     className="rounded-lg shadow-md w-auto object-center mx-auto md:h-[424px] sm:h-80"
//                     src={image}                  
//                     alt={`Hotel Image ${index + 1}`}
//                     loading="lazy"
//                   />
//                 </div>
//               ))}
//             </Slider>

//             <button
//               className="absolute left-2 bg-gray-800 bg-opacity-50 text-white p-2 rounded-full opacity-0 group-hover:opacity-100 transition-opacity z-10"
//               style={{ top: "50%", transform: "translateY(-50%)" }}
//               onClick={() => sliderRef.current.slickPrev()}
//             >
//               <FontAwesomeIcon icon={faChevronLeft} />
//             </button>

//             <button
//               className="absolute right-2 bg-gray-800 bg-opacity-50 text-white p-2 rounded-full opacity-0 group-hover:opacity-100 transition-opacity z-10"
//               style={{ top: "50%", transform: "translateY(-50%)" }}
//               onClick={() => sliderRef.current.slickNext()}
//             >
//               <FontAwesomeIcon icon={faChevronRight} />
//             </button>
//           </div>

//           <div className="w-4/7 lg:w-4/7 flex flex-col gap-4 bg-indigo-100 p-6 rounded-lg shadow-md">
//             <h3 className="text-2xl font-semibold text-gray-700">{hotel.name}</h3>
//             <h4>
//               {hotel.userScore !== undefined &&
//               hotel.userScore !== null &&
//               hotel.userScore >= 0
//                 ? `User score  ${hotel.userScore.toFixed(1)} ★`
//                 : "Has not been rated by users yet"}
//             </h4>
//             <h4>{hotel.address}</h4>
//             <p className="text-gray-600">{hotel.description}</p>
//           </div>
//         </div>

//         <h3 className="text-2xl font-semibold mt-6 text-gray-700">More Images</h3>
//         <div className="bg-white grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 lg:grid-cols-4 gap-4 my-4 p-2">
//           {hotel.images.map((image, index) => (
//             <img
//               key={index}
//               className="rounded-lg shadow-md w-full h-[200px] object-cover"
//               src={image}
//               alt={`Image ${index + 1}`}
//             />
//           ))}
//         </div>

//         <div className="grid sm:grid-cols-1 md:grid-cols-1 lg:grid-cols-2 xl:grid-cols-2 gap-4 my-6 p-2">
        
//           <div className="w-full bg-indigo-200 p-4 rounded-lg shadow-md">
//           <h3 className="text-2xl font-semibold text-gray-700 mb-4">Available Rooms</h3>           
//             <RoomsFilter filter={filter} setFilter={setFilter} onApplyFilters={handleFilterApply} />
//             <Rooms rooms={rooms} />
          
//           </div>
//           <div className="w-full p-4 rounded-lg shadow-md border-[3px] border-indigo-300">
//             <h3 className="text-2xl font-semibold text-gray-700 mb-2">Comments</h3>
//             <CreateCommentForm hotelId={hotel.id} hasRatedComments={hasRatedComments} />
//             {hotel.comments && hotel.comments.length > 0 ? (
//               <div className="border-2 border-indigo-300 mt-6 p-6 rounded-lg shadow-lg">
//                 <div className="space-y-4">
//                   {hotel.comments.map((comment) => (
//                   <Comment
//                     key={comment.id}
//                     username={comment.username}
//                     description={comment.description}
//                     createdAt={comment.createdAt}
//                     isCurrentUser={currentUserId === comment.userId}
//                     userScore={comment.userScore}
//                   />
//                   ))}
//                 </div>
//               </div>
//             ) : (
//               <p className="text-gray-500">No comments yet. Be the first to leave a comment!</p>
//             )}
//           </div>
//         </div>        
//       </div>
//     </section>
//   );
// }

// export default HotelDetails;














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
import ImagesSection from "./subcomponents/HotelDetails/ImagesSection";

import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";

function HotelDetails() {
  const { id } = useParams();
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
      <div className="container mx-auto px-4 w-5/6">
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
          <RoomsSection
            filter={filter}
            setFilter={setFilter}
            onApplyFilters={handleFilterApply}
            rooms={rooms}
          />

          <CommentSection
            hotel={hotel}
            hasRatedComments={hasRatedComments}
            currentUserId={currentUserId}
          />
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