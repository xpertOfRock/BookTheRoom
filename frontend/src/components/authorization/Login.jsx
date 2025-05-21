import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { login } from '../../services/auth';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faHotel, faEnvelope, faLock } from '@fortawesome/free-solid-svg-icons';
import {
  Box,
  Button,
  Input,
  InputGroup,
  InputLeftElement,
  Grid,
  VStack,
  Heading,
  Text,
  Center,
  useColorModeValue,
} from '@chakra-ui/react';

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
    } catch {
      setError('Login failed. Please check your credentials.');
    }
  };

  const pageBg = useColorModeValue('gray.50', 'gray.800');
  const formBg = 'white';
  const inputBg = useColorModeValue('gray.100', 'gray.700');
  const inputHoverBg = useColorModeValue('purple.50', 'purple.600');
  const inputColor = useColorModeValue('gray.900', 'white');
  const buttonBg = 'purple.500';
  const buttonHoverBg = 'purple.600';
  const textColor = 'white';

  return (
    <Center minH="100vh" bg={pageBg} p={4}>
      <Box
        maxW={{ base: '90%', md: '800px' }}
        w="full"
        bg={formBg}
        borderRadius="lg"
        boxShadow="xl"
        overflow="hidden"
      >
        <Grid templateColumns={{ base: '1fr', md: '1fr 1fr' }}>
          <Box p={{ base: 6, md: 10 }}>
            <Center mb={4}>
              <FontAwesomeIcon icon={faHotel} style={{ color: 'purple', fontSize: '1.5rem' }} />
              <Heading ml={2} size="lg" color="purple.600">
                BookTheRoom
              </Heading>
            </Center>
            {error && (
              <Text mb={4} color="red.400" fontWeight="semibold" textAlign="center">
                {error}
              </Text>
            )}
            <VStack spacing={4} align="stretch">
              <Box>
                <Text mb={1} fontSize="sm" color="gray.600">
                  Email or Username
                </Text>
                <InputGroup>
                  <InputLeftElement pointerEvents="none">
                    <FontAwesomeIcon icon={faEnvelope} style={{ color: 'gray' }} />
                  </InputLeftElement>
                  <Input
                    placeholder="Enter email or username"
                    value={emailOrUsername}
                    onChange={e => setEmailOrUsername(e.target.value)}
                    bg={inputBg}
                    color={inputColor}
                    borderRadius="md"
                    _hover={{ bg: inputHoverBg }}
                  />
                </InputGroup>
              </Box>
              <Box>
                <Text mb={1} fontSize="sm" color="gray.600">
                  Password
                </Text>
                <InputGroup>
                  <InputLeftElement pointerEvents="none">
                    <FontAwesomeIcon icon={faLock} style={{ color: 'gray' }} />
                  </InputLeftElement>
                  <Input
                    placeholder="Enter password"
                    type="password"
                    value={password}
                    onChange={e => setPassword(e.target.value)}
                    bg={inputBg}
                    color={inputColor}
                    borderRadius="md"
                    _hover={{ bg: inputHoverBg }}
                  />
                </InputGroup>
              </Box>
              <Button
                w="full"
                mt={2}
                bg={buttonBg}
                color={textColor}
                _hover={{ bg: buttonHoverBg }}
                borderRadius="md"
                onClick={handleLogin}
              >
                Sign in
              </Button>
            </VStack>
          </Box>
          <Box display={{ base: 'none', md: 'block' }} bgGradient="linear(to-br, purple.500, purple.600)" p={10}>
            <Text fontSize="xl" fontWeight="bold" color={textColor} mb={4}>
              Welcome to BookTheRoom
            </Text>
            <Text color={textColor} fontSize="sm">
              Reserve rooms in hotels and private apartments with ease. Manage bookings, view availability, and enjoy personalized deals all in one place.
            </Text>
          </Box>
        </Grid>
      </Box>
    </Center>
  );
}

export default Login;
