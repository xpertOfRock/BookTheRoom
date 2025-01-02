import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { login } from '../../services/auth';
import { Box, Button, Input, Stack, Heading, Text } from '@chakra-ui/react';

function Login() {
  const [emailOrUsername, setEmailOrUsername] = useState('');
  const [password, setPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleLogin = async () => {
    try {
      await login(emailOrUsername, password);
      navigate('/');
      window.location.reload();
    } catch (error) {
      setError('Login failed. Please check your credentials.');
    }
  };

  return (
    <Box p={4} className="w-1/3 items-center">
      <Heading mb={6}>Login</Heading>
      {error && <Text color="red.500">{error}</Text>}
      <Stack spacing={4}>
        <Input
          placeholder="Email or Username"
          value={emailOrUsername}
          onChange={(e) => setEmailOrUsername(e.target.value)}
        />
        <Input
          placeholder="Password"
          type="password"
          value={password}
          onChange={(e) => setPassword(e.target.value)}
        />
        <Button onClick={handleLogin} colorScheme="blue">
          Login
        </Button>
      </Stack>
    </Box>
  );
}

export default Login;
