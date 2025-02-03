import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { register } from '../../services/auth';
import InputMask from 'react-input-mask';
import { Box, Button, Input, Stack, Heading, Text, Center } from '@chakra-ui/react';

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

  const handleChange = (e) => {
    const { name, value } = e.target;
    setUserData((prevData) => ({
      ...prevData,
      [name]: value,
    }));
  };

  const handleRegister = async () => {
    try {
      await register(userData);
      navigate('/');
      window.location.reload();
    } catch (error) {
      setError(error.response?.data?.message || 'Registration failed. Please try again.');
    }
  };

  return (
    <Center axis="both">
    <Box p={4} width={"25%"}>
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
              name="phoneNumber" // Обязательно добавляем name
            />
          )}
        </InputMask>
        <Input
          placeholder="Birthdate"
          name="birthdate"
          type="date" // Ввод даты
          value={userData.birthdate}
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
    </Center>
  );
}

export default Register;