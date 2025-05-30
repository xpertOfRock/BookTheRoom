import React, { useEffect, useState, useRef } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Slider from "react-slick";
import Lightbox from "yet-another-react-lightbox";
import "yet-another-react-lightbox/styles.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft, faChevronRight } from "@fortawesome/free-solid-svg-icons";
import { fetchHotel } from "../../services/hotels";
import { fetchRooms } from "../../services/rooms";
import { postComment } from "../../services/comments";
import { getCurrentUserId } from "../../services/auth";
import RoomsSection from "../../components/hotel/subcomponents/HotelDetails/RoomsSection";
import CommentSection from "../../components/shared/CommentsSection";
import ImagesSection from "../../components/shared/ImagesSection";
import {
  Box,
  Button,
  Grid,
  Heading,
  Text,
  VStack,
  Flex,
  useColorModeValue,
} from "@chakra-ui/react";
import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";

function HotelDetails() {
  const { id } = useParams();
  const navigate = useNavigate();
  const sliderRef = useRef(null);
  const [hotel, setHotel] = useState(null);
  const [loading, setLoading] = useState(true);
  const [rooms, setRooms] = useState([]);
  const [filter, setFilter] = useState({ search: "", sortItem: "name", sortOrder: "asc", categories: [], minPrice: null, maxPrice: null, checkIn: null, checkOut: null });
  const defaultCheckIn = new Date(Date.now() + 86400000).toISOString().split("T")[0];
  const defaultCheckOut = new Date(Date.now() + 172800000).toISOString().split("T")[0];
  const [isLightboxOpen, setIsLightboxOpen] = useState(false);
  const [photoIndex, setPhotoIndex] = useState(0);
  const [redirectCheckIn, setCheckIn] = useState(defaultCheckIn);
  const [redirectCheckOut, setCheckOut] = useState(defaultCheckOut);
  const currentUserId = getCurrentUserId();
  const bg = useColorModeValue("indigo.50", "indigo.900");
  const borderColor = useColorModeValue("indigo.400", "indigo.600");
  const textColor = useColorModeValue("gray.800", "gray.200");

  useEffect(() => {
    (async () => {
      try { setHotel(await fetchHotel(id)); } catch { } finally { setLoading(false); }
    })();
  }, [id]);

  const handleFilterApply = async () => {
    setCheckIn(filter.checkIn);
    setCheckOut(filter.checkOut);
    setRooms(await fetchRooms(id, filter));
  };

  const hasRatedComments = Array.isArray(hotel?.comments) && hotel.comments.some(c => c.userId === currentUserId && c.userScore > 0);

  const sliderSettings = { dots: true, infinite: true, speed: 500, slidesToShow: 1, slidesToScroll: 1, arrows: false };

  const handleImageClick = index => { setPhotoIndex(index); setIsLightboxOpen(true); };
  const addComment = async payload => { await postComment(payload); setHotel(await fetchHotel(id)); };

  if (loading) return <Box textAlign="center" py={20} color={textColor}>Loading...</Box>;
  if (!hotel) return <Box textAlign="center" py={20} color={textColor}>Hotel not found.</Box>;

  const { images = [], name, userScore, address, description } = hotel;
  const slides = images.map(src => ({ src }));

  return (
    <Box maxW="7xl" mx="auto" px={{ base: 4, md: 8 }} py={8}>
      <Flex mb={8} align="center" position="relative" minH="48px">
        <Button position="absolute" left={0} colorScheme="purple" leftIcon={<FontAwesomeIcon icon={faChevronLeft} />} onClick={() => navigate("/hotels")} size="md" _hover={{ bg: "purple.600" }}>Back</Button>
        <Box mx="auto">
          <Heading size="2xl" color={textColor} textAlign="center" noOfLines={1}>{name}</Heading>
        </Box>
      </Flex>

      <Grid templateColumns={{ base: "1fr", xl: "1fr 1fr" }} gap={8} mb={10}>
        <Box position="relative" role="group" h="432px" rounded="lg" overflow="hidden" boxShadow="lg">
          <Slider {...sliderSettings} ref={sliderRef}>
            {images.map((img, idx) => (
              <Box key={idx} h="432px" overflow="hidden">
                <img src={img} alt={`Slide ${idx+1}`} className="object-cover w-full h-full cursor-pointer" onClick={() => handleImageClick(idx)} />
              </Box>
            ))}
          </Slider>
          <Button position="absolute" left={2} top="50%" transform="translateY(-50%)" size="sm" bg="blackAlpha.600" color="white" _hover={{ bg: "blackAlpha.800" }} onClick={() => sliderRef.current.slickPrev()} aria-label="Prev" rounded="full"><FontAwesomeIcon icon={faChevronLeft} /></Button>
          <Button position="absolute" right={2} top="50%" transform="translateY(-50%)" size="sm" bg="blackAlpha.600" color="white" _hover={{ bg: "blackAlpha.800" }} onClick={() => sliderRef.current.slickNext()} aria-label="Next" rounded="full"><FontAwesomeIcon icon={faChevronRight} /></Button>
        </Box>
        <Box bg={bg} border={`3px solid ${borderColor}`} rounded="lg" p={6} shadow="md">
          <VStack align="start" spacing={4}>
            <Heading size="xl" color={textColor}>{name}</Heading>
            <Text fontSize="lg" color={textColor}>{userScore != null && userScore > 0 ? `User score ${userScore.toFixed(1)} ★` : "Has not been rated"}</Text>
            <Text fontSize="md" color={textColor}>{address}</Text>
            <Text fontSize="md" color="gray.600" whiteSpace="pre-line">{description}</Text>
          </VStack>
        </Box>
      </Grid>

      <ImagesSection images={images} onImageClick={handleImageClick} />
      <Grid templateColumns={{ base: "1fr", lg: "2fr 1fr" }} gap={8} mt={6}>
        <Box><RoomsSection hotelId={hotel.id} filter={filter} setFilter={setFilter} onApplyFilters={handleFilterApply} rooms={rooms} hotelName={name} checkIn={redirectCheckIn} checkOut={redirectCheckOut} /></Box>
        <Box><CommentSection property={hotel} hasRatedComments={hasRatedComments} currentUserId={currentUserId} onAddComment={addComment} propertyType={1} /></Box>
      </Grid>
      <Lightbox open={isLightboxOpen} close={() => setIsLightboxOpen(false)} slides={slides} index={photoIndex} />
    </Box>
  );
}

export default HotelDetails;