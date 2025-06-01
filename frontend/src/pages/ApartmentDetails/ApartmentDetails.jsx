import { useEffect, useState, useRef } from "react";
import { useParams, useNavigate } from "react-router-dom";
import Slider from "react-slick";
import Lightbox from "yet-another-react-lightbox";
import "yet-another-react-lightbox/styles.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faChevronLeft, faChevronRight } from "@fortawesome/free-solid-svg-icons";
import { fetchApartment } from "../../services/apartments";
import { postComment } from "../../services/comments";
import { fetchChatByApartmentId, postChat } from "../../services/chats";
import { getCurrentUserId } from "../../services/auth";
import ImagesSection from "../../components/shared/ImagesSection";
import CommentsSection from "../../components/shared/CommentsSection";
import ChatList from "../../components/chat/ChatList";
import {
  Box,
  Button,
  Grid,
  Heading,
  Text,
  VStack,
  Stack,
  useColorModeValue,
  useToast,
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

  const toast = useToast();

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
    };
    load();
  }, [id]);

  const handleImageClick = (idx) => {
    setPhotoIndex(idx);
    setIsLightboxOpen(true);
  };

  const startOrGetChat = async () => {
    const currentUserId = getCurrentUserId();

    if (!id || !apartment) {
      console.error("Apartment is null.");
      return;
    }

    const existingChat = await fetchChatByApartmentId(id);

    if (!existingChat) {
      const payload = {
        userIds: [currentUserId, apartment.ownerId],
        apartmentId: id,
      };

      try {
        const result = await postChat(payload);

        if (result && result.status === 200) {
          toast({
            title: "Success",
            description: "You've created new chat with the owner.",
            status: "success",
            duration: 5000,
            isClosable: true,
          });
          navigate(`/apartments/${id}/chats/${result.data.id}`);
        } else if (result && result.status === 401) {
          toast({
            title: "Unauthorized",
            description: "Required authorization.",
            status: "error",
            duration: 5000,
            isClosable: true,
          });
        } else {
          toast({
            title: "Error",
            description: `Unexpected status code: ${result ? result.status : "no response"}`,
            status: "error",
            duration: 5000,
            isClosable: true,
          });
        }
      } catch (error) {
        toast({
          title: "Error!",
          description: "An error occurred while creating the chat.",
          status: "error",
          duration: 5000,
          isClosable: true,
        });
        console.error(error);
      }
    } else {
      navigate(`/apartments/${id}/chats/${existingChat.id}`);
    }
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

  const slides = apartment.images.map((src) => ({ src }));
  const isOwner = apartment.ownerId === currentUserId;

  return (
    <Box maxW="7xl" mx="auto" px={{ base: 4, md: 8 }} py={8}>
      <VStack spacing={2} mb={8}>
        <Heading size="lg" color={textColor}>{apartment.title}</Heading>
        <Heading size="md" color={textColor}>{apartment.name}</Heading>
      </VStack>

      <Grid
        templateColumns={{ base: "1fr", lg: "1fr 1fr" }}
        gap={10}
        mb={10}
        alignItems="start"
      >
        <Box position="relative" rounded="lg" overflow="hidden" boxShadow={boxShadow}>
          <Slider {...sliderSettings} ref={sliderRef}>
            {apartment.images.map((image, index) => (
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
          <Heading size="md" color={textColor}>{apartment.title}</Heading>
          <Text fontWeight="semibold" color="purple.600">
            {apartment.userScore != null && apartment.userScore > 0 ? `Rating: ${apartment.userScore.toFixed(1)} ★` : "Not rated yet"}
          </Text>
          <Text color={textColor}>{apartment.address}</Text>
          <Text color="gray.700" fontSize="md" whiteSpace="pre-line">
            {apartment.description}
          </Text>
          <Text color="gray.700" fontSize="md" whiteSpace="pre-line">
            Price: {apartment.price}$
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
                <strong>Owner:</strong> {apartment.owner} {isOwner && (
                  <Box as="span" ml={2} px={2} py={0.5} bg={ownerBg} color={ownerTextColor} rounded="md" fontWeight="bold" fontSize="xs">
                    You
                  </Box>
                )}
              </Text>
              {apartment.email && <Text><strong>Email:</strong> {apartment.email}</Text>}
              {apartment.phoneNumber && <Text><strong>Phone:</strong> {apartment.phoneNumber}</Text>}
              {apartment.telegram && <Text><strong>Telegram:</strong> {apartment.telegram}</Text>}
              {apartment.instagram && <Text><strong>Instagram:</strong> {apartment.instagram}</Text>}
            </Stack>
          </Box>

          {!isOwner  ? (
            <Button
              colorScheme="purple"
              alignSelf="stretch"
              size="md"
              onClick={startOrGetChat}
            >
              Chat
            </Button>
          ) : null}
        </VStack>
      </Grid>

      <Grid templateColumns={{ base: "1fr", lg: "1fr 1fr" }} gap={8}>
        <Box>
          <ImagesSection images={apartment.images} onImageClick={handleImageClick} />
          {isOwner ? <ChatList chats={apartment.chats || []} /> : null}
          
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