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
    Select
  } from "@chakra-ui/react";
  import { useEffect, useState } from "react";
  import { useParams, useNavigate } from "react-router-dom";
  import { putRoom, fetchRoom } from "../../services/rooms";
  
  function UpdateRoomForm() {
    const { id: hotelId, number: number } = useParams();
    const navigate = useNavigate();
    const [roomData, setRoomData] = useState({
      name: "",
      description: "",
      price: 0,
      category: "",
      images: []
    });
  
    const [selectedFiles, setSelectedFiles] = useState([]);
  
    useEffect(() => {
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
    }, [hotelId, number]);
  
    const handleChange = (e) => {
      const { name, value, type, checked } = e.target;
      setRoomData({
        ...roomData,
        [name]: type === "checkbox" ? checked : value
      });
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
        const status = await putRoom(hotelId, number, formData);
        if (status === 200) {
          alert("Room updated successfully!");
          navigate(`/hotels/${hotelId}`);
        } else {
          alert("Failed to update room");
        }
      } catch (error) {
        console.error("Error updating room", error);
      }
    };
  
    return (
      <Box p={8} maxWidth="600px" mx="auto">
        <Heading mb={6}>Update Room: {roomData.name}</Heading>
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
   
            <FormControl id="roomCategory" isRequired>
              <FormLabel>Room Category</FormLabel>
              <Select
                name="roomCategory"
                value={roomData.category}
                onChange={handleChange}
              >
                <option value="1">One Bed Apartments</option>
                <option value="2">Two Bed Apartments</option>
                <option value="3">Three Bed Apartments</option>
                <option value="4">Luxury</option>
              </Select>
            </FormControl>
  
            <FormControl id="images">
              <FormLabel>Images</FormLabel>
              <Input
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