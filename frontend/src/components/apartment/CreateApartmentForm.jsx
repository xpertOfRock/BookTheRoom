import React, { useState, useEffect } from 'react';
import {
  Box,
  Flex,
  Button,
  FormControl,
  FormLabel,
  Input,
  Textarea,
  NumberInput,
  NumberInputField,
  VStack,
  Heading,
  Text,
  useToast,
  useColorModeValue,
  Stack,
} from '@chakra-ui/react';
import { useNavigate } from 'react-router-dom';
import { postApartment } from '../../services/apartments';
import ImagesSection from '../shared/ImagesSection';

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
    Instagram: '',
  });
  const [imagePreviews, setImagePreviews] = useState([]);
  const navigate = useNavigate();
  const toast = useToast();

  const handleChange = (e) => {
    const { name, value, type, files } = e.target;
    if (type === 'file') {
      const fileList = files ? Array.from(files) : [];
      setForm((prev) => ({ ...prev, Images: fileList }));

      const previews = fileList.map((file) => URL.createObjectURL(file));
      setImagePreviews(previews);
    } else {
      setForm((prev) => ({ ...prev, [name]: value }));
    }
  };

  const handlePriceChange = (valueString) => {
    const value = parseFloat(valueString) || 0;
    setForm((prev) => ({ ...prev, PriceForNight: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const formData = new FormData();

    [
      'Title',
      'Description',
      'Country',
      'State',
      'City',
      'Street',
      'PostalCode',
      'Telegram',
      'Instagram',
    ].forEach((field) => {
      if (form[field] !== undefined) {
        formData.append(field, form[field]);
      }
    });

    formData.append('PriceForNight', form.PriceForNight.toString());
    form.Images.forEach((file) => formData.append('Images', file));

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
          description: 'An error has occurred.',
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
    } catch (error) {
      console.log(error);
    }
  };

  const previewBg = useColorModeValue('gray.50', 'gray.700');
  const formBg = useColorModeValue('white', 'gray.800');
  const borderColor = useColorModeValue('gray.200', 'gray.600');

  return (
    <Flex
      direction={{ base: 'column', lg: 'row' }}
      maxW="7xl"
      mx="auto"
      p={{ base: 4, md: 8 }}
      gap={8}
    >
      <Box
        as="form"
        onSubmit={handleSubmit}
        w={{ base: '100%', lg: '50%' }}
        bg={formBg}
        boxShadow="lg"
        rounded="lg"
        p={6}
        border="1px solid"
        borderColor={borderColor}
      >
        <VStack spacing={4} align="stretch">
          <FormControl id="Title" isRequired>
            <FormLabel>Title</FormLabel>
            <Input name="Title" value={form.Title} onChange={handleChange} />
          </FormControl>

          <FormControl id="Description" isRequired>
            <FormLabel>Description</FormLabel>
            <Textarea
              name="Description"
              value={form.Description}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="PriceForNight" isRequired>
            <FormLabel>Price per Night (in USD)</FormLabel>
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
            <Input
              name="PostalCode"
              value={form.PostalCode}
              onChange={handleChange}
            />
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
            <Input
              name="Telegram"
              value={form.Telegram}
              onChange={handleChange}
            />
          </FormControl>

          <FormControl id="Instagram">
            <FormLabel>Instagram</FormLabel>
            <Input
              name="Instagram"
              value={form.Instagram}
              onChange={handleChange}
            />
          </FormControl>

          <Button colorScheme="purple" type="submit" size="lg" w="full">
            Create Apartment
          </Button>
        </VStack>
      </Box>

      <Box
        w={{ base: '100%', lg: '50%' }}
        bg={previewBg}
        boxShadow="lg"
        rounded="lg"
        p={6}
        border="1px solid"
        borderColor={borderColor}
      >
        <Stack spacing={4}>
          <Heading size="lg">{form.Title || 'Apartment Title'}</Heading>
          <Text color="gray.600">
            {form.City && form.Country
              ? `${form.Country}, ${form.City}`
              : 'Country, City'}
          </Text>
          <Text>{form.Description || 'Apartment description will appear here.'}</Text>
          <Text fontWeight="bold" fontSize="xl">
            {form.PriceForNight > 0
              ? `$${form.PriceForNight.toFixed(2)} / night`
              : 'Price per night'}
          </Text>

          <Box>
            <Text fontWeight="semibold" mb={1}>
              Address:
            </Text>
            <Text>
              {[
                form.Country,               
                form.State,              
                form.City,
                form.Street,
                form.PostalCode,               
              ]
                .filter((part) => part)
                .join(', ')}
            </Text>
          </Box>

          <ImagesSection images={imagePreviews} onImageClick={() => {}} />
        </Stack>
      </Box>
    </Flex>
  );
}

export default CreateApartmentForm;
