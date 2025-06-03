import {
  Box,
  VStack,
  FormControl,
  FormLabel,
  Input,
  Textarea,
  NumberInput,
  NumberInputField,
  Switch,
  Button,
  Heading,
} from "@chakra-ui/react";
import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { fetchHotel, putHotel } from "../../services/hotels";
import { useToast } from "@chakra-ui/react";

function UpdateHotelForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const toast = useToast();

  const [hotelData, setHotelData] = useState({
    name: "",
    description: "",
    rating: 1,
    pool: false,
    country: "",
    state: "",
    city: "",
    street: "",
    postalCode: "",
    images: [],
  });
  const [selectedFiles, setSelectedFiles] = useState([]);
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    const getHotel = async () => {
      try {
        const data = await fetchHotel(id);
        let parsedAddress = {};
        if (data.jsonAddress) {
          try {
            parsedAddress = JSON.parse(data.jsonAddress.trim());
          } catch {}
        }
        setHotelData({
          name: data.name || "",
          description: data.description || "",
          rating: data.rating || 1,
          pool: data.hasPool || false,
          country: parsedAddress?.Country || "",
          state: parsedAddress?.State || "",
          city: parsedAddress?.City || "",
          street: parsedAddress?.Street || "",
          postalCode: parsedAddress?.PostalCode || "",
          images: data.images || [],
        });
      } catch (e) {
        console.error("Failed to fetch hotel data", e);
      }
    };
    getHotel();
  }, [id]);

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setHotelData({
      ...hotelData,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const handleFileChange = (e) => {
    setSelectedFiles(e.target.files);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);
    const formData = new FormData();
    formData.append("Name", hotelData.name);
    formData.append("Description", hotelData.description);
    formData.append("Rating", hotelData.rating);
    formData.append("Pool", hotelData.pool);
    formData.append("Country", hotelData.country);
    formData.append("State", hotelData.state);
    formData.append("City", hotelData.city);
    formData.append("Street", hotelData.street);
    formData.append("PostalCode", hotelData.postalCode);
    for (let i = 0; i < selectedFiles.length; i++) {
      formData.append("Images", selectedFiles[i]);
    }
    try {
      const response = await putHotel(id, formData);
      if (response === 200) {
        toast({
          title: "Success",
          description: "Hotel was updated successfully.",
          status: "success",
          duration: 5000,
          isClosable: true,
        });
        navigate(`/hotels/${id}`);
      } else if (response === 401) {
        toast({
          title: "Unauthorized",
          description: "Required authorization.",
          status: "error",
          duration: 5000,
          isClosable: true,
        });
      } else if (response === 403) {
        toast({
          title: "Forbidden",
          description: "You don't have rights to perform this action.",
          status: "error",
          duration: 5000,
          isClosable: true,
        });
      }
    } catch (e) {
      console.error("Error updating hotel", e);
    }
    setIsSubmitting(false);
  };

  return (
    <Box p={8} maxWidth="600px" mx="auto">
      <Heading mb={6}>Update data for {hotelData.name}</Heading>
      <form onSubmit={handleSubmit}>
        <VStack spacing={4}>
          <FormControl id="name" isRequired>
            <FormLabel>Name</FormLabel>
            <Input
              type="text"
              name="name"
              value={hotelData.name}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="description" isRequired>
            <FormLabel>Description</FormLabel>
            <Textarea
              name="description"
              value={hotelData.description}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="rating" isRequired>
            <FormLabel>Rating</FormLabel>
            <NumberInput
              name="rating"
              value={hotelData.rating}
              min={1}
              max={5}
              onChange={(value) =>
                setHotelData({ ...hotelData, rating: parseInt(value, 10) })
              }
            >
              <NumberInputField />
            </NumberInput>
          </FormControl>

          <FormControl id="pool">
            <FormLabel>Has Pool</FormLabel>
            <Switch
              name="pool"
              isChecked={hotelData.pool}
              onChange={(e) =>
                setHotelData({ ...hotelData, pool: e.target.checked })
              }
            />
          </FormControl>

          <FormControl id="country" isRequired>
            <FormLabel>Country</FormLabel>
            <Input
              type="text"
              name="country"
              value={hotelData.country}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="state" isRequired>
            <FormLabel>State</FormLabel>
            <Input
              type="text"
              name="state"
              value={hotelData.state}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="city" isRequired>
            <FormLabel>City</FormLabel>
            <Input
              type="text"
              name="city"
              value={hotelData.city}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="street" isRequired>
            <FormLabel>Street</FormLabel>
            <Input
              type="text"
              name="street"
              value={hotelData.street}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="postalCode" isRequired>
            <FormLabel>Postal Code</FormLabel>
            <Input
              type="text"
              name="postalCode"
              value={hotelData.postalCode}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="images">
            <FormLabel>Upload Images</FormLabel>
            <Input
              type="file"
              multiple
              onChange={handleFileChange}
            />
          </FormControl>

          <Button
            type="submit"
            colorScheme="purple"
            size="lg"
            width="full"
            isLoading={isSubmitting}
            loadingText="Updating"
          >
            Update Hotel
          </Button>
        </VStack>
      </form>
    </Box>
  );
}

export default UpdateHotelForm;