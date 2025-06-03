import {
  Box,
  VStack,
  FormControl,
  FormLabel,
  Input,
  Textarea,
  NumberInput,
  NumberInputField,
  Button,
  Heading,
  Select,
  useToast
} from "@chakra-ui/react";
import { useEffect, useState, useRef } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { putRoom, fetchRoom } from "../../services/rooms";
import { isAdmin } from "../../services/auth";

function UpdateRoomForm() {
  const { id: hotelId, number: number } = useParams();
  const navigate = useNavigate();
  const toast = useToast();

  const initialState = {
    name: "",
    description: "",
    price: 0,
    category: "",
    images: []
  };

  const [roomData, setRoomData] = useState(initialState);
  const [selectedFiles, setSelectedFiles] = useState([]);
  const fileInputRef = useRef(null);

  useEffect(() => {
    if (isAdmin() === false) {
      navigate("/");
    }
    const getRoom = async () => {
      try {
        const data = await fetchRoom(hotelId, number);
        setRoomData({
          name: data.name || "",
          description: data.description || "",
          price: data.price ?? 0,
          category: data.roomCategory || "",
          images: data.images || []
        });
      } catch (error) {
        console.error("Failed to fetch room data", error);
      }
    };

    getRoom();
  }, [hotelId, number, navigate]);

  const handleChange = (e) => {
    const { name, value, type } = e.target;
    setRoomData((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? e.target.checked : value
    }));
  };

  const handleNumberChange = (valueAsString, valueAsNumber, fieldName) => {
    setRoomData((prev) => ({
      ...prev,
      [fieldName]: valueAsNumber
    }));
  };

  const handleFileChange = (e) => {
    setSelectedFiles(e.target.files);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append("Name", roomData.name);
    formData.append("Description", roomData.description);
    formData.append("Price", roomData.price);
    formData.append("RoomCategory", roomData.category);

    for (let i = 0; i < selectedFiles.length; i++) {
      formData.append("Images", selectedFiles[i]);
    }

    try {
      const response = await putRoom(hotelId, number, formData);

      if (response === 200) {
        toast({
          title: "Success",
          description: "Room was updated successfully.",
          status: "success",
          duration: 5000,
          isClosable: true
        });
        setRoomData(initialState);
        setSelectedFiles([]);
        if (fileInputRef.current) {
          fileInputRef.current.value = null;
        }
      } else if (response === 400) {
        toast({
          title: "Error",
          description: "An error has occurred.",
          status: "error",
          duration: 5000,
          isClosable: true
        });
      } else if (response === 401) {
        toast({
          title: "Unauthorized",
          description: "Required authorization.",
          status: "error",
          duration: 5000,
          isClosable: true
        });
      } else if (response === 403) {
        toast({
          title: "Forbidden",
          description: "You don't have rights to perform this action.",
          status: "error",
          duration: 5000,
          isClosable: true
        });
      }
    } catch (error) {
      console.error("Error updating room", error);
    }
  };

  return (
    <Box p={8} maxWidth="600px" mx="auto">
      <Heading mb={6}>
        Update Room: {roomData.name || "(no name)"}
      </Heading>
      <form onSubmit={handleSubmit}>
        <VStack spacing={4}>
          <FormControl id="name" isRequired>
            <FormLabel>Name</FormLabel>
            <Input
              type="text"
              name="name"
              value={roomData.name}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="description" isRequired>
            <FormLabel>Description</FormLabel>
            <Textarea
              name="description"
              value={roomData.description}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="price" isRequired>
            <FormLabel>Price</FormLabel>
            <NumberInput
              name="price"
              value={roomData.price}
              min={0}
              precision={2}
              onChange={(valueStr, valueNum) =>
                handleNumberChange(valueStr, valueNum, "price")
              }
            >
              <NumberInputField />
            </NumberInput>
          </FormControl>

          <FormControl id="category" isRequired>
            <FormLabel>Room Category</FormLabel>
            <Select
              name="category"
              value={roomData.category}
              onChange={handleChange}
            >
              <option value="">Select a category</option>
              <option value="1">One Bed Apartments</option>
              <option value="2">Two Bed Apartments</option>
              <option value="3">Three Bed Apartments</option>
              <option value="4">Luxury</option>
            </Select>
          </FormControl>

          <FormControl id="images">
            <FormLabel>Images</FormLabel>
            <Input
              ref={fileInputRef}
              type="file"
              multiple
              onChange={handleFileChange}
              className="border border-gray-300 rounded p-2 w-full"
            />
          </FormControl>

          <Button type="submit" colorScheme="purple" size="lg" width="full">
            Update
          </Button>
        </VStack>
      </form>
    </Box>
  );
}

export default UpdateRoomForm;
