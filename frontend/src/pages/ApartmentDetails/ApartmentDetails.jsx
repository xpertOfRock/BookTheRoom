import { useEffect, useState, useRef } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Slider from "react-slick";

import Lightbox from "yet-another-react-lightbox";
import "yet-another-react-lightbox/styles.css";

import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft, faChevronRight } from "@fortawesome/free-solid-svg-icons";

import { fetchApartment } from "../../services/apartments";
import { postComment } from "../../services/comments";
import { getCurrentUserId } from "../../services/auth";
import ImagesSection from "../../components/shared/ImagesSection";
import CommentsSection from "../../components/shared/CommentsSection";

import {
  Box,
  Button,
  Grid,
  Heading,
  Text,
  VStack,
  Stack,
  useColorModeValue,
} from "@chakra-ui/react";

import "slick-carousel/slick/slick.css";
import "slick-carousel/slick/slick-theme.css";

function ApartmentDetails() {
  const { id } = useParams();
  const navigate = useNavigate();
  const sliderRef = useRef(null);

  const [apartment, setApartment] = useState(null);
  const [loading, setLoading] = useState(true);
  const [isLightboxOpen, setIsLightboxOpen] = useState(false);
  const [photoIndex, setPhotoIndex] = useState(0);
  const currentUserId = getCurrentUserId();

  const bg = useColorModeValue("white", "gray.700");
  const boxShadow = useColorModeValue("lg", "dark-lg");
  const textColor = useColorModeValue("gray.800", "gray.200");
  const borderColor = useColorModeValue("purple.400", "purple.600");
  const ownerBg = useColorModeValue("purple.100", "purple.900");
  const ownerTextColor = useColorModeValue("purple.800", "purple.200");

  useEffect(() => {
    const load = async () => {
      try {
        const data = await fetchApartment(id);
        setApartment(data);
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    }
    load();
  }, [id]);

  const handleImageClick = (idx) => {
    setPhotoIndex(idx);
    setIsLightboxOpen(true);
  };

  const addComment = async ({ description, propertyId, propertyType, userScore }) => {
    try {
      await postComment({ description, propertyId, propertyType, userScore });
      const updated = await fetchApartment(id);
      setApartment(updated);
    } catch (err) {
      console.error(err);
    }
  };
  
  const sliderSettings = {
    dots: true,
    infinite: true,
    speed: 500,
    slidesToShow: 1,
    slidesToScroll: 1,
    arrows: false,
  };

  if (loading) return <Text textAlign="center" py={20} color={textColor}>Loading apartment details...</Text>;
  if (!apartment) return <Text textAlign="center" py={20} color={textColor}>Apartment not found.</Text>;

  const {
    images = [],
    title,
    name,
    userScore,
    address,
    description,
    owner,
    email,
    phoneNumber,
    telegram,
  } = apartment;

  const slides = images.map((src) => ({ src }));
  const isOwner = owner === currentUserId;

  return (
    <Box maxW="7xl" mx="auto" px={{ base: 4, md: 8 }} py={8}>
      <VStack spacing={2} mb={8}>
        <Heading size="lg" color={textColor}>{title}</Heading>
        <Heading size="md" color={textColor}>{name}</Heading>
      </VStack>

      <Grid
        templateColumns={{ base: "1fr", lg: "1fr 1fr" }}
        gap={10}
        mb={10}
        alignItems="start"
      >
        <Box position="relative" rounded="lg" overflow="hidden" boxShadow={boxShadow}>
          <Slider {...sliderSettings} ref={sliderRef}>
            {images.map((image, index) => (
              <Box key={index} h="432px" overflow="hidden">
                <img
                  className="object-cover w-full h-full cursor-pointer transition-transform duration-300 hover:scale-105"
                  src={image}
                  alt={`Hotel Image ${index + 1}`}
                  loading="lazy"
                  onClick={() => handleImageClick(index)}
                />
              </Box>
            ))}
          </Slider>

          <Button
            position="absolute"
            top="50%"
            left="2"
            transform="translateY(-50%)"
            size="sm"
            bg="blackAlpha.600"
            color="white"
            _hover={{ bg: "blackAlpha.800" }}
            onClick={() => sliderRef.current.slickPrev()}
            aria-label="Previous slide"
            rounded="full"
            zIndex="10"
          >
            <FontAwesomeIcon icon={faChevronLeft} />
          </Button>
          <Button
            position="absolute"
            top="50%"
            right="2"
            transform="translateY(-50%)"
            size="sm"
            bg="blackAlpha.600"
            color="white"
            _hover={{ bg: "blackAlpha.800" }}
            onClick={() => sliderRef.current.slickNext()}
            aria-label="Next slide"
            rounded="full"
            zIndex="10"
          >
            <FontAwesomeIcon icon={faChevronRight} />
          </Button>
        </Box>

        <VStack
          bg={bg}
          p={6}
          rounded="lg"
          boxShadow={boxShadow}
          align="start"
          spacing={6}
        >
          <Heading size="md" color={textColor}>{title}</Heading>
          <Text fontWeight="semibold" color="purple.600">
            {userScore != null ? `Rating: ${userScore.toFixed(1)} ★` : "Not rated yet"}
          </Text>
          <Text color={textColor}>{address}</Text>
          <Text color="gray.500" fontSize="md" whiteSpace="pre-line">
            {description}
          </Text>

          <Box
            w="100%"
            p={4}
            bg={bg}
            border={`2px solid ${borderColor}`}
            rounded="md"
            boxShadow="sm"
          >
            <Heading size="sm" mb={2} color={textColor}>
              Contacts
            </Heading>
            <Stack spacing={1} fontSize="sm" color={textColor}>
              <Text>
                <strong>Owner:</strong> {owner} {isOwner && (
                  <Box as="span" ml={2} px={2} py={0.5} bg={ownerBg} color={ownerTextColor} rounded="md" fontWeight="bold" fontSize="xs">
                    (you)
                  </Box>
                )}
              </Text>
              {email && <Text><strong>Email:</strong> {email}</Text>}
              {phoneNumber && <Text><strong>Phone:</strong> {phoneNumber}</Text>}
              {telegram && <Text><strong>Telegram:</strong> {telegram}</Text>}
            </Stack>
          </Box>

          <Button
            colorScheme="purple"
            variant="outline"
            alignSelf="stretch"
            size="md"
            onClick={() => alert("Start chatting clicked")}
          >
            Start chatting
          </Button>
        </VStack>
      </Grid>

      <Grid
        templateColumns={{ base: "1fr", lg: "1fr 1fr" }}
        gap={8}
      >
        <Box>
          <ImagesSection images={images} onImageClick={handleImageClick} />
        </Box>

        <Box>
          <CommentsSection
            property={apartment}
            hasRatedComments={false}
            currentUserId={currentUserId}
            onAddComment={addComment}
            propertyType={2}
          />
        </Box>
      </Grid>

      <Lightbox
        open={isLightboxOpen}
        close={() => setIsLightboxOpen(false)}
        slides={slides}
        index={photoIndex}
      />
    </Box>
  );
}

export default ApartmentDetails;
