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
  useToast,
  useColorModeValue,
  Stack,
  Text,
} from '@chakra-ui/react';
import { useParams, useNavigate } from 'react-router-dom';
import { fetchApartment, putApartment } from '../../services/apartments';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faChevronLeft } from '@fortawesome/free-solid-svg-icons';
import { getCurrentUserId } from '../../services/auth';
import ImagesSection from '../shared/ImagesSection';

function UpdateApartmentForm() {
  const { id } = useParams();
  const navigate = useNavigate();
  const toast = useToast();

  const [form, setForm] = useState({
    Title: '',
    Description: '',
    PriceForNight: 0,
    Country: '',
    State: '',
    City: '',
    Street: '',
    PostalCode: '',
    Telegram: '',
    Instagram: '',
  });
  const [selectedFiles, setSelectedFiles] = useState([]);
  const [imagePreviews, setImagePreviews] = useState([]);
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    const loadApartment = async () => {
      try {
        const data = await fetchApartment(id);
        if (getCurrentUserId() !== data.ownerId) {
          navigate(`/apartments/${id}`);
          return;
        }
        let parsedAddress = {};
        if (data.jsonAddress) {
          try {
            parsedAddress = JSON.parse(data.jsonAddress.trim());
          } catch {}
        }
        setForm({
          Title: data.title || '',
          Description: data.description || '',
          PriceForNight: data.price || 0,
          Country: parsedAddress.Country || '',
          State: parsedAddress.State || '',
          City: parsedAddress.City || '',
          Street: parsedAddress.Street || '',
          PostalCode: parsedAddress.PostalCode || '',
          Telegram: data.telegram || '',
          Instagram: data.instagram || '',
        });
        setImagePreviews(Array.isArray(data.images) ? data.images : []);
      } catch {
        toast({
          title: 'Error',
          description: 'Failed to load apartment data.',
          status: 'error',
          duration: 5000,
          isClosable: true,
        });
      }
    };
    loadApartment();
  }, [id, navigate, toast]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handlePriceChange = (valueString) => {
    const value = parseFloat(valueString) || 0;
    setForm((prev) => ({ ...prev, PriceForNight: value }));
  };

  const handleFileChange = (e) => {
    const files = e.target.files ? Array.from(e.target.files) : [];
    setSelectedFiles(files);
    const previews = files.map((file) => URL.createObjectURL(file));
    setImagePreviews(previews);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);
    const formData = new FormData();
    formData.append('Title', form.Title);
    formData.append('Description', form.Description);
    formData.append('Price', form.PriceForNight.toString());
    formData.append('Country', form.Country);
    formData.append('State', form.State);
    formData.append('City', form.City);
    formData.append('Street', form.Street);
    formData.append('PostalCode', form.PostalCode);
    formData.append('Telegram', form.Telegram);
    formData.append('Instagram', form.Instagram);
    selectedFiles.forEach((file) => {
      formData.append('Images', file);
    });
    try {
      const status = await putApartment(id, formData);
      if (status === 200) {
        toast({
          title: 'Success',
          description: 'Apartment was successfully updated.',
          status: 'success',
          duration: 5000,
          isClosable: true,
        });
        navigate(`/apartments/${id}`);
      } else if (status === 400) {
        toast({
          title: 'Error',
          description: 'Invalid data provided. Check the input data.',
          status: 'error',
          duration: 5000,
          isClosable: true,
        });
        setIsSubmitting(false);
      } else if (status === 401) {
        toast({
          title: 'Unauthorized',
          description: 'Please sign in to continue.',
          status: 'error',
          duration: 5000,
          isClosable: true,
        });
        setIsSubmitting(false);
      } else if (status === 403) {
        toast({
          title: 'Forbidden',
          description: 'You do not have permission for this action.',
          status: 'error',
          duration: 5000,
          isClosable: true,
        });
        setIsSubmitting(false);
      } else {
        toast({
          title: 'Error',
          description: 'An unexpected error occurred. Check the input data.',
          status: 'error',
          duration: 5000,
          isClosable: true,
        });
        setIsSubmitting(false);
      }
    } catch {
      toast({
        title: 'Error',
        description: 'Failed to update apartment.',
        status: 'error',
        duration: 5000,
        isClosable: true,
      });
      setIsSubmitting(false);
    }
  };

  const formBg = useColorModeValue('white', 'gray.800');
  const borderColor = useColorModeValue('gray.200', 'gray.600');
  const previewBg = useColorModeValue('gray.50', 'gray.700');

  return (
    <Flex
      direction={{ base: 'column', lg: 'row' }}
      maxW="7xl"
      mx="auto"
      p={{ base: 4, md: 8 }}
      gap={8}
      position="relative"
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
        <Flex align="center" justify="center" position="relative" mb={4}>
          <Button
            position="absolute"
            left={0}
            colorScheme="purple"
            leftIcon={<FontAwesomeIcon icon={faChevronLeft} />}
            onClick={() => navigate(`/apartments/${id}`)}
            size="md"
            _hover={{ bg: 'purple.600' }}
          >
            Back
          </Button>
          <Heading>Edit</Heading>
        </Flex>
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
            <FormLabel>Price per Night (USD)</FormLabel>
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
            <Input
              name="Country"
              value={form.Country}
              onChange={handleChange}
            />
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
            <Input
              name="Street"
              value={form.Street}
              onChange={handleChange}
            />
          </FormControl>
          <FormControl id="PostalCode" isRequired>
            <FormLabel>Postal Code</FormLabel>
            <Input
              name="PostalCode"
              value={form.PostalCode}
              onChange={handleChange}
            />
          </FormControl>
          <FormControl id="Telegram">
            <FormLabel>Telegram</FormLabel>
            <Input
              name="Telegram"
              value={form.Telegram}
              onChange={handleChange}
              placeholder="@..."
            />
          </FormControl>
          <FormControl id="Instagram">
            <FormLabel>Instagram</FormLabel>
            <Input
              name="Instagram"
              value={form.Instagram}
              onChange={handleChange}
              placeholder="@..."
            />
          </FormControl>
          <FormControl id="Images">
            <FormLabel>Upload Images</FormLabel>
            <Input
              type="file"
              name="Images"
              multiple
              accept="image/*"
              onChange={handleFileChange}
            />
          </FormControl>
          <Button
            colorScheme="purple"
            type="submit"
            size="lg"
            w="full"
            isLoading={isSubmitting}
            loadingText="Updating"
          >
            Edit
          </Button>
          <Text>You cannot upload more than 20 images</Text>
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
            {form.Country && form.City
              ? `${form.Country}, ${form.City}`
              : 'Country, City'}
          </Text>
          <Text>
            {form.Description || 'Apartment description will appear here.'}
          </Text>
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
              {[form.Country, form.State, form.City, form.Street, form.PostalCode]
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

export default UpdateApartmentForm;
