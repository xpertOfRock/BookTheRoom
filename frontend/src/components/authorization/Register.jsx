import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { register } from '../services/auth';
import { Box, Button, Input, Stack, Heading, Text } from '@chakra-ui/react';

function Register() {
  const [userData, setUserData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    username: '',
    phoneNumber: '',
    age: 0,
    password: ''
  });
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleChange = (e) => {
    setUserData({ ...userData, [e.target.name]: e.target.value });
  };

  const handleRegister = async () => {
    try {
      await register(userData);

      navigate('/');
    } catch (error) {
      setError('Registration failed. Please try again.');
    }
  };

  return (
    <Box p={4}>
      <Heading mb={6}>Register</Heading>
      {error && <Text color="red.500">{error}</Text>}
      <Stack spacing={4}>
        <Input
          placeholder="First Name"
          name="firstName"
          value={userData.firstName}
          onChange={handleChange}
        />
        <Input
          placeholder="Last Name"
          name="lastName"
          value={userData.lastName}
          onChange={handleChange}
        />
        <Input
          placeholder="Email"
          name="email"
          value={userData.email}
          onChange={handleChange}
        />
        <Input
          placeholder="Username"
          name="username"
          value={userData.username}
          onChange={handleChange}
        />
        <Input
          placeholder="Phone Number"
          name="phoneNumber"
          value={userData.phoneNumber}
          onChange={handleChange}
        />
        <Input
          placeholder="Age"
          name="age"
          type="number"
          value={userData.age}
          onChange={handleChange}
        />
        <Input
          placeholder="Password"
          name="password"
          type="password"
          value={userData.password}
          onChange={handleChange}
        />
        <Button onClick={handleRegister} colorScheme="green">
          Register
        </Button>
      </Stack>
    </Box>
  );
}

export default Register;