import React, { useEffect, useState, useRef } from "react";
import { useParams, useNavigate, useLocation } from "react-router-dom";
import Slider from "react-slick";
import Lightbox from "yet-another-react-lightbox";
import "yet-another-react-lightbox/styles.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft, faChevronRight } from "@fortawesome/free-solid-svg-icons";
import { fetchRoom } from "../../services/rooms";
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

function RoomDetails() {
  const { id, number } = useParams();
  const navigate = useNavigate();
  const location = useLocation();
  const sliderRef = useRef(null);
  const { checkIn, checkOut, hotel } = location.state || {};

  const [room, setRoom] = useState(null);
  const [orderData, setOrderData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isLightboxOpen, setIsLightboxOpen] = useState(false);
  const [photoIndex, setPhotoIndex] = useState(0);

  const categories = new Map([
    [1, "One bed apartments"],
    [2, "Two bed apartments"],
    [3, "Three bed apartments"],
    [4, "Luxury"]
  ]);

  const bg = useColorModeValue("white", "gray.800");
  const boxShadow = useColorModeValue("lg", "dark-lg");
  const textColor = useColorModeValue("gray.800", "gray.200");
  const borderColor = useColorModeValue("purple.400", "purple.600");

  useEffect(() => {
    (async () => {
      if (!checkIn || !checkOut) {
        navigate(`/hotels/${id}`);
        return;
      }
      const data = await fetchRoom(id, number);
      setRoom(data);
      setOrderData({
        hotelId: id,
        roomNumber: number,
        checkIn,
        checkOut,
        basePrice: data.price,
        roomName: data.name,
        hotelName: hotel
      });
      setLoading(false);
    })();
  }, [id, number, checkIn, checkOut]);

  const sliderSettings = {
    dots: true,
    infinite: true,
    speed: 500,
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: false
  };

  const handleImageClick = idx => {
    setPhotoIndex(idx);
    setIsLightboxOpen(true);
  };

  const handleCheckout = () => {
    navigate(`/checkout`, { state: orderData });
  };

  if (loading) return <Box textAlign="center" py={20} color={textColor}>Loading...</Box>;
  if (!room) return <Box textAlign="center" py={20} color={textColor}>Room not found.</Box>;

  const { images = [], name, description, price, category } = room;
  const slides = images.map(src => ({ src }));

  return (
    <Box maxW="7xl" mx="auto" px={{ base: 4, md: 8 }} py={8}>
      <Flex mb={6} align="center">
        <Button
          leftIcon={<FontAwesomeIcon icon={faChevronLeft} />}
          onClick={() => navigate(`/hotels/${id}`)}
          colorScheme="purple"
          size="md"
        >
          Back
        </Button>
        <Heading flex="1" textAlign="center" size="xl" color={textColor}>
          {name}
        </Heading>
      </Flex>

      <Grid templateColumns={{ base: "1fr", lg: "1fr 1fr" }} gap={8} mb={6}>
        <Box position="relative" role="group" rounded="lg" overflow="hidden" boxShadow={boxShadow}>
          <Slider {...sliderSettings} ref={sliderRef}>
            {images.map((img, idx) => (
              <Box key={idx} h="432px" overflow="hidden">
                <img
                  src={img}
                  alt={`Slide ${idx + 1}`}
                  className="object-cover w-full h-full cursor-pointer"
                  onClick={() => handleImageClick(idx)}
                />
              </Box>
            ))}
          </Slider>
          <Button
            position="absolute"
            left={2}
            top="50%"
            transform="translateY(-50%)"
            size="sm"
            bg="blackAlpha.600"
            color="white"
            _hover={{ bg: "blackAlpha.800" }}
            onClick={() => sliderRef.current.slickPrev()}
            rounded="full"
            aria-label="Prev"
          >
            <FontAwesomeIcon icon={faChevronLeft} />
          </Button>
          <Button
            position="absolute"
            right={2}
            top="50%"
            transform="translateY(-50%)"
            size="sm"
            bg="blackAlpha.600"
            color="white"
            _hover={{ bg: "blackAlpha.800" }}
            onClick={() => sliderRef.current.slickNext()}
            rounded="full"
            aria-label="Next"
          >
            <FontAwesomeIcon icon={faChevronRight} />
          </Button>
        </Box>

        <Box bg={bg} p={6} rounded="lg" boxShadow={boxShadow} borderWidth={3} borderColor={borderColor}>
          <VStack align="start" spacing={4}>
            <Heading size="lg" color={textColor}>{name}</Heading>
            <Text color="gray.600">{categories.get(category) ?? 'Standard'}</Text>
            <Text color={textColor} whiteSpace="pre-line">{description}</Text>
            <Text fontSize="lg" fontWeight="semibold" color={textColor}>Price: ${price}</Text>
          </VStack>
        </Box>
      </Grid>

      <ImagesSection images={images} onImageClick={handleImageClick} />

      <Box mt={6}>
        <Button colorScheme="purple" size="lg" w="full" onClick={handleCheckout}>
          Book this room
        </Button>
      </Box>

      <Lightbox open={isLightboxOpen} close={() => setIsLightboxOpen(false)} slides={slides} index={photoIndex} />
    </Box>
  );
}

export default RoomDetails;