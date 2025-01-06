import React, { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { postRoom } from "../../../services/rooms";
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  Input,
  Textarea,
  NumberInput,
  NumberInputField,
  Select,
  VStack,
  useToast,
} from "@chakra-ui/react";

function CreateRoomForm() {
  const { id: hotelId } = useParams(); 
  const navigate = useNavigate();
  const [form, setForm] = useState({
    number: 0,
    title: "",
    description: "",
    pricePerNight: 500,
    category: 1,
    images: [],
  });

  const toast = useToast();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prevForm) => ({
      ...prevForm,
      [name]: value,
    }));
  };

  const handleFileChange = (e) => {
    setForm((prevForm) => ({
      ...prevForm,
      images: Array.from(e.target.files),
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const formData = new FormData();
    Object.keys(form).forEach((key) => {
      if (key === "images") {
        form.images.forEach((file) => {
          formData.append("Images", file);
        });
      } else {
        formData.append(key.charAt(0).toUpperCase() + key.slice(1), form[key]);
      }
    });
    console.log(hotelId);
    console.log(formData);
    try {
      const response = await postRoom(hotelId, formData);
      console.log(response);
      setForm({
        number: 0,
        name: "",
        description: "",
        pricePerNight: 500,
        category: 1,
        images: [],
      });

      if(response !== 200){
        toast({
            title: "Error!",
            description: "An error occurred while creating the room.",
            status: "error",
            duration: 5000,
            isClosable: true,
          });          
      }else{
        toast({
            title: "Room Created!",
            description: "The room was successfully created.",
            status: "success",
            duration: 5000,
            isClosable: true,
          });
      }
      

      navigate(`/hotels/${hotelId}`);
    } catch (error) {
      
    }
  };

  return (
    <Box p={8} maxWidth="600px" mx="auto">
      <form onSubmit={handleSubmit}>
        <VStack spacing={4}>
          <FormControl id="number" isRequired>
            <FormLabel>Room Number</FormLabel>
            <NumberInput
              name="number"
              value={form.number}
              min={1}
              onChange={(valueString) =>
                setForm((prevForm) => ({
                  ...prevForm,
                  number: parseInt(valueString, 10),
                }))
              }
            >
              <NumberInputField />
            </NumberInput>
          </FormControl>

          <FormControl id="title" isRequired>
            <FormLabel>Room Name</FormLabel>
            <Input
              type="text"
              name="title"
              value={form.title}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="description" isRequired>
            <FormLabel>Description</FormLabel>
            <Textarea
              name="description"
              value={form.description}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="pricePerNight" isRequired>
            <FormLabel>Price per Night</FormLabel>
            <NumberInput
              name="pricePerNight"
              value={form.pricePerNight}
              min={1}
              precision={2}
              onChange={(valueString) =>
                setForm((prevForm) => ({
                  ...prevForm,
                  pricePerNight: parseFloat(valueString),
                }))
              }
            >
              <NumberInputField />
            </NumberInput>
          </FormControl>

          <FormControl id="category" isRequired>
            <FormLabel>Category</FormLabel>
            <Select
              name="category"
              value={form.category}
              onChange={handleChange}
              className="w-full border border-gray-300 rounded p-2"
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
              name="images"
              multiple
              accept="image/*"
              onChange={handleFileChange}
            />
          </FormControl>

          <Button type="submit" colorScheme="purple" size="lg" width="full">
            Create Room
          </Button>
        </VStack>
      </form>
    </Box>
  );
}

export default CreateRoomForm;