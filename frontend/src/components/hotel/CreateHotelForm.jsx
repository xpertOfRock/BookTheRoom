import React, { useState } from 'react';
import { postHotel } from '../../services/hotels';
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  Input,
  Textarea,
  NumberInput,
  NumberInputField,
  Switch,
  VStack,
  useToast,
} from '@chakra-ui/react';

function CreateHotelForm() {
  const [form, setForm] = useState({
    name: '',
    description: '',
    rating: 0,
    pool: false,
    country: '',
    state: '',
    city: '',
    street: '',
    postalCode: '',
    images: [],
  });

  const toast = useToast();

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setForm((prevForm) => ({
      ...prevForm,
      [name]: type === 'checkbox' ? checked : value,
    }));
  };

  const handleFileChange = (e) => {
    setForm((prevForm) => ({
      ...prevForm,
      images: Array.from(e.target.files),
    }));
  };

  const handleRatingChange = (value) => {
    setForm((prevForm) => ({
      ...prevForm,
      rating: value,
    }));
  };

  const onCreate = async (hotelForm) => {
    try {
      await postHotel(hotelForm);
      setForm({
        name: '',
        description: '',
        rating: 0,
        pool: false,
        country: '',
        state: '',
        city: '',
        street: '',
        postalCode: '',
        images: [],
      });
      
      toast({
        title: 'Hotel Created!',
        description: 'The hotel was successfully created.',
        status: 'success',
        duration: 5000,
        isClosable: true,
      });
    } catch (error) {
      toast({
        title: 'Error!',
        description: 'An error occurred while creating the hotel.',
        status: 'error',
        duration: 5000,
        isClosable: true,
      });
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const formData = new FormData();
    Object.keys(form).forEach((key) => {
      if (key === 'images') {
        form.images.forEach((file) => {
          formData.append('Images', file);
        });
      } else {
        formData.append(key.charAt(0).toUpperCase() + key.slice(1), form[key]);
      }
    });

    await onCreate(formData);
  };

  return (
    <Box p={8} maxWidth="600px" mx="auto">
      <form onSubmit={handleSubmit}>
        <VStack spacing={4}>
          <FormControl id="name" isRequired>
            <FormLabel>Name</FormLabel>
            <Input
              type="text"
              name="name"
              value={form.name}
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

          <FormControl id="rating" isRequired>
            <FormLabel>Rating</FormLabel>
            <NumberInput
              name="rating"
              value={form.rating}
              min={0}
              max={5}
              onChange={handleRatingChange}
            >
              <NumberInputField />
            </NumberInput>
          </FormControl>

          <FormControl id="pool">
            <FormLabel>Has Pool</FormLabel>
            <Switch
              name="pool"
              isChecked={form.pool}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="country" isRequired>
            <FormLabel>Country</FormLabel>
            <Input
              type="text"
              name="country"
              value={form.country}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="state" isRequired>
            <FormLabel>State</FormLabel>
            <Input
              type="text"
              name="state"
              value={form.state}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="city" isRequired>
            <FormLabel>City</FormLabel>
            <Input
              type="text"
              name="city"
              value={form.city}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="street" isRequired>
            <FormLabel>Street</FormLabel>
            <Input
              type="text"
              name="street"
              value={form.street}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="postalCode" isRequired>
            <FormLabel>Postal Code</FormLabel>
            <Input
              type="text"
              name="postalCode"
              value={form.postalCode}
              onChange={handleChange}
            />
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
            Create Hotel
          </Button>
        </VStack>
      </form>
    </Box>
  );
};

export default CreateHotelForm;
