import React, { useState } from 'react';
import { Box, Button, Input, FormControl, FormLabel, useToast } from '@chakra-ui/react';
import { login } from '../../services/authentication';

const Login = () => {
  const [emailOrUsername, setEmailOrUsername] = useState('');
  const [password, setPassword] = useState('');
  const toast = useToast();

  const handleLogin = async (e) => {
    e.preventDefault();

    try {
      const response = await login(emailOrUsername, password);

      toast({
        title: 'Successful authorization!',
        status: 'success',
        duration: 5000,
        isClosable: true,
      });

      window.location.href = '/home';
    } catch (error) {
      toast({
        title: 'Error logging in.',
        description: error.response?.data?.message || 'An unhandled error occurred.',
        status: 'error',
        duration: 5000,
        isClosable: true,
      });
    }
  };

  return (
    <Box width="400px" mx="auto" mt="100px">
      <form onSubmit={handleLogin}>
        <FormControl id="emailOrUsername" isRequired>
          <FormLabel>Email or Username</FormLabel>
          <Input
            type="text"
            value={emailOrUsername}
            onChange={(e) => setEmailOrUsername(e.target.value)}
            placeholder="Enter your email or username"
          />
        </FormControl>
        <FormControl id="password" isRequired mt="4">
          <FormLabel>Password</FormLabel>
          <Input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="Enter your password"
          />
        </FormControl>
        <Button type="submit" colorScheme="blue" mt="6" width="full">
          Login
        </Button>
      </form>
    </Box>
  );
};

export default Login;
