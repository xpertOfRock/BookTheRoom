import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { register } from '../../services/auth';
import InputMask from 'react-input-mask';
import {
  Box,
  Button,
  Input,
  Grid,
  VStack,
  Heading,
  Text,
  Center,
  Image,
  useColorModeValue,
} from '@chakra-ui/react';

function Register() {
  const [userData, setUserData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    username: '',
    phoneNumber: '',
    birthdate: '',
    password: '',
  });
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleChange = e => {
    const { name, value } = e.target;
    setUserData(prev => ({ ...prev, [name]: value }));
  };

  const handleRegister = async () => {
    try {
      await register(userData);
      navigate('/');
      window.location.reload();
    } catch (err) {
      setError(err.response?.data?.message || 'Registration failed. Please try again.');
    }
  };

  const pageBg = useColorModeValue('gray.50', 'gray.800');
  const formBg = 'white';
  const inputBg = useColorModeValue('gray.100', 'gray.700');
  const inputHoverBg = useColorModeValue('purple.50', 'purple.600');
  const inputColor = useColorModeValue('gray.900', 'white');

  return (
    <Center minH="100vh" bg={pageBg} p={4}>
      <Box
        maxW={{ base: '90%', md: '800px' }}
        w="full"
        bg={formBg}
        borderRadius="25px"
        boxShadow="xl"
        overflow="hidden"
      >
        <Grid templateColumns={{ base: '1fr', md: '1fr 1fr' }}>
          <Box p={{ base: 6, md: 10 }}>
            <Heading mb={4} fontSize="2xl" color="purple.600">
              Create Your Account
            </Heading>
            {error && (
              <Text mb={4} color="red.400" fontWeight="semibold">
                {error}
              </Text>
            )}
            <VStack spacing={4} align="stretch">
              <Input
                placeholder="First Name"
                name="firstName"
                value={userData.firstName}
                onChange={handleChange}
                bg={inputBg}
                color={inputColor}
                borderRadius="md"
                _hover={{ bg: inputHoverBg }}
              />
              <Input
                placeholder="Last Name"
                name="lastName"
                value={userData.lastName}
                onChange={handleChange}
                bg={inputBg}
                color={inputColor}
                borderRadius="md"
                _hover={{ bg: inputHoverBg }}
              />
              <Input
                placeholder="Email"
                name="email"
                type="email"
                value={userData.email}
                onChange={handleChange}
                bg={inputBg}
                color={inputColor}
                borderRadius="md"
                _hover={{ bg: inputHoverBg }}
              />
              <Input
                placeholder="Username"
                name="username"
                value={userData.username}
                onChange={handleChange}
                bg={inputBg}
                color={inputColor}
                borderRadius="md"
                _hover={{ bg: inputHoverBg }}
              />
              <InputMask mask="+38 (999) 999-99-99" value={userData.phoneNumber} onChange={handleChange}>
                {inputProps => (
                  <Input
                    {...inputProps}
                    placeholder="Phone Number"
                    name="phoneNumber"
                    bg={inputBg}
                    color={inputColor}
                    borderRadius="md"
                    _hover={{ bg: inputHoverBg }}
                  />
                )}
              </InputMask>
              <Input
                placeholder="Birthdate"
                name="birthdate"
                type="date"
                value={userData.birthdate}
                onChange={handleChange}
                bg={inputBg}
                color={inputColor}
                borderRadius="md"
                _hover={{ bg: inputHoverBg }}
              />
              <Input
                placeholder="Password"
                name="password"
                type="password"
                maxLength={20}
                value={userData.password}
                onChange={handleChange}
                bg={inputBg}
                color={inputColor}
                borderRadius="md"
                _hover={{ bg: inputHoverBg }}
              />
              <Button
                w="full"
                mt={2}
                bgGradient="linear(to-r, purple.400, purple.500)"
                color="white"
                _hover={{ bgGradient: 'linear(to-r, purple.500, purple.400)' }}
                borderRadius="md"
                onClick={handleRegister}
              >
                Register
              </Button>
            </VStack>
          </Box>
          <Box display={{ base: 'none', md: 'block' }}>
            <Image
              src="https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-registration/draw1.webp"
              alt="Registration Illustration"
              objectFit="cover"
              h="100%"
              w="100%"
            />
          </Box>
        </Grid>
      </Box>
    </Center>
  );
}

export default Register;
