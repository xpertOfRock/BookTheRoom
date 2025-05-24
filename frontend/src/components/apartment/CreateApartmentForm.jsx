import React, { useState } from 'react';
import {
  Box,
  Button,
  FormControl,
  FormLabel,
  Input,
  Textarea,
  NumberInput,
  NumberInputField,
  VStack,
  useToast
} from '@chakra-ui/react';
import { useNavigate } from 'react-router-dom';
import { postApartment } from '../../services/apartments';

function CreateApartmentForm() {
  const [form, setForm] = useState({
    Title: '',
    Description: '',
    PriceForNight: 0,
    Country: '',
    State: '',
    City: '',
    Street: '',
    PostalCode: '',
    Images: [],
    Telegram: '',
    Instagram: ''
  });
  
  const navigate = useNavigate();
  const toast = useToast();
  
  const handleChange = (e) => {
    const { name, value, type, files } = e.target;
    if (type === 'file') {
      setForm(prev => ({
        ...prev,
        Images: files ? Array.from(files) : []
      }));
    } else {
      setForm(prev => ({
        ...prev,
        [name]: value
      }));
    }
  };

  const handlePriceChange = (valueString) => {
    const value = parseFloat(valueString) || 0;
    setForm(prev => ({ ...prev, PriceForNight: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const formData = new FormData();

    ['Title', 'Description', 'Country', 'State', 'City', 'Street', 'PostalCode', 'Telegram', 'Instagram']
      .forEach(field => {
        if (form[field] !== undefined) {
          formData.append(field, form[field]);
        }
      });

    formData.append('PriceForNight', form.PriceForNight.toString());

    form.Images.forEach(file => {
      formData.append('Images', file);
    });

    try {
      const response = await postApartment(formData);
      if (response === 200) {
        toast({
          title: 'Success',
          description: 'Apartment was created successfully.',
          status: 'success',
          duration: 5000,
          isClosable: true,
        });
        } else if (response === 400) {
        toast({
          title: 'Error',
          description: 'An error has occured.',
          status: 'error',
          duration: 5000,
          isClosable: true,
        });
      } else if (response === 401) {
        toast({
          title: 'Unauthorized',
          description: 'Required authorization.',
          status: 'error',
          duration: 5000,
          isClosable: true,
        });
      } else if (response === 403) {
        toast({
          title: 'Forbidden',
          description: 'You cannot add a new record.',
          status: 'error',
          duration: 5000,
          isClosable: true,
      });
    } else {
      toast({
        title: 'Error',
        description: 'An error occurred while creating new apartment.',
        status: 'error',
        duration: 5000,
        isClosable: true,
      });
    }
      navigate('/apartments');
    }catch(error){
      console.log(error);
    }
  };

  return (
    <Box className="bg-white shadow-md rounded-lg p-6 mx-auto max-w-xl mt-8">
      <form onSubmit={handleSubmit}>
        <VStack spacing={4}>
          <FormControl id="Title" isRequired>
            <FormLabel>Title</FormLabel>
            <Input name="Title" value={form.Title} onChange={handleChange} />
          </FormControl>

          <FormControl id="Description" isRequired>
            <FormLabel>Description</FormLabel>
            <Textarea name="Description" value={form.Description} onChange={handleChange} />
          </FormControl>

          <FormControl id="PriceForNight" isRequired>
            <FormLabel>Price per Night</FormLabel>
            <NumberInput
              name="PriceForNight"
              value={form.PriceForNight}
              min={0}
              precision={2}
              onChange={handlePriceChange}
            >
              <NumberInputField />
            </NumberInput>
          </FormControl>

          <FormControl id="Country" isRequired>
            <FormLabel>Country</FormLabel>
            <Input name="Country" value={form.Country} onChange={handleChange} />
          </FormControl>

          <FormControl id="State">
            <FormLabel>State</FormLabel>
            <Input name="State" value={form.State} onChange={handleChange} />
          </FormControl>

          <FormControl id="City" isRequired>
            <FormLabel>City</FormLabel>
            <Input name="City" value={form.City} onChange={handleChange} />
          </FormControl>

          <FormControl id="Street" isRequired>
            <FormLabel>Street</FormLabel>
            <Input name="Street" value={form.Street} onChange={handleChange} />
          </FormControl>

          <FormControl id="PostalCode" isRequired>
            <FormLabel>Postal Code</FormLabel>
            <Input name="PostalCode" value={form.PostalCode} onChange={handleChange} />
          </FormControl>

          <FormControl id="Images">
            <FormLabel>Images</FormLabel>
            <Input
              type="file"
              name="Images"
              multiple
              accept="image/*"
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="Telegram">
            <FormLabel>Telegram</FormLabel>
            <Input name="Telegram" value={form.Telegram} onChange={handleChange} />
          </FormControl>

          <FormControl id="Instagram">
            <FormLabel>Instagram</FormLabel>
            <Input name="Instagram" value={form.Instagram} onChange={handleChange} />
          </FormControl>

          <Button
            type="submit"
            className="w-full bg-purple-600 hover:bg-purple-700 text-white"
            size="lg"
          >
            Create Apartment
          </Button>
        </VStack>
      </form>
    </Box>
  );
}

export default CreateApartmentForm;