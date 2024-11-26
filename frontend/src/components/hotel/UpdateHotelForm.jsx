import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom'; 
import { fetchHotel, putHotel } from '../../services/hotels';
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
import HotelCard from './HotelCard';

const UpdateHotelForm = () => {
  const { id } = useParams(); 
  const [hotel, setHotel] = useState(null);
  const [form, setForm] = useState({
    name: '',
    description: '',
    rating: 0,
    pool: false,
    address: '',
    images: [],
  });

  const toast = useToast();

  const getHotel = async () => {
    try {
      const hotelData = await fetchHotel(id); 
      setHotel(hotelData);
      setForm({
        name: hotelData.name || '',
        description: hotelData.description || '',
        rating: hotelData.rating || 0,
        pool: hotelData.pool || false,
        address: hotelData.address || '',
        images: hotelData.images || [],
      });
    } catch (error) {
      toast({
        title: 'Error!',
        description: 'Failed to load hotel data.',
        status: 'error',
        duration: 5000,
        isClosable: true,
      });
    }
  };

  useEffect(() => {
    getHotel();
  }, [id]);

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

  const onUpdate = async (hotelForm) => {
    try {
      await putHotel(id, hotelForm);
      toast({
        title: 'Hotel Updated!',
        description: 'The hotel was successfully updated.',
        status: 'success',
        duration: 5000,
        isClosable: true,
      });
    } catch (error) {
      toast({
        title: 'Error!',
        description: 'An error occurred while updating the hotel.',
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

    await onUpdate(formData);
  };

  if (!hotel) {
    return <p>Loading...</p>;
  }

  return (
    <section className="p-8 flex flex-row justify-start gap-12">
      <div className="flex flex-col w-1/2 gap-10">
      <HotelCard
        name={hotel.name}
        description={hotel.description}
        preview={hotel.images[0]}
        rating={hotel.rating}
        address={hotel.address}
      />
      </div>
      <div className="flex flex-col w-1/2 gap-10">
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

            <Button type="submit" colorScheme="teal" size="lg" width="full">
              Update Hotel
            </Button>
          </VStack>
        </form>
      </Box>
      </div>
      
    </section>
  );
};

export default UpdateHotelForm;