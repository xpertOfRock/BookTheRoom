import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { register } from '../../services/auth';
import InputMask from 'react-input-mask';
import { Box, Button, Input, Stack, Heading, Text } from '@chakra-ui/react';

function Register() {
  const [userData, setUserData] = useState({
    firstName: '',
    lastName: '',
    email: '',
    username: '',
    phoneNumber: '',
    age: '',
    password: '',
  });
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUserData((prevData) => ({
      ...prevData,
      [name]: name === 'age' ? Number(value) : value,
    }));
  };

  const handleRegister = async () => {
    try {
      await register(userData);
      navigate('/');
    } catch (error) {
      setError(error.response?.data?.message || 'Registration failed. Please try again.');
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
          type="email"
          value={userData.email}
          onChange={handleChange}
          maxLength={254}
        />
        <Input
          placeholder="Username"
          name="username"
          value={userData.username}
          onChange={handleChange}
        />
        <InputMask
          mask="+38 (999) 999-99-99"
          value={userData.phoneNumber}
          onChange={handleChange}
        >
          {(inputProps) => (
            <Input
              {...inputProps}
              type="tel"
              placeholder="Phone Number"
              name="phoneNumber" // Добавлен атрибут name
            />
          )}
        </InputMask>
        <Input
          placeholder="Age"
          name="age"
          type="number"
          min={1}
          max={100}
          step={1}
          value={userData.age}
          onChange={handleChange}
        />
        <Input
          placeholder="Password"
          name="password"
          type="password"
          maxLength={20}
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
