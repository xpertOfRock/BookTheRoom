// src/components/Navbar.js
import { Box, Flex, HStack, Link, IconButton, useDisclosure, Stack, Button, Text } from '@chakra-ui/react';
import { HamburgerIcon, CloseIcon } from '@chakra-ui/icons'; 
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { getCurrentToken, getCurrentUser, logout, refreshToken } from '../services/auth';

function Navbar() {
  const { isOpen, onOpen, onClose } = useDisclosure();
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [user, setUser] = useState(null);
  const navigate = useNavigate();

  const checkAuthStatus = () => {
    const token = getCurrentToken();
    const currentUser = getCurrentUser();
    if (token && currentUser) {
      setIsAuthenticated(true);
      setUser(currentUser);
    } else {
      setIsAuthenticated(false);
      setUser(null);
    }
  };

  useEffect(() => {
    checkAuthStatus();
  }, []);

  useEffect(() => {
    const interval = setInterval(() => {
      if (isAuthenticated) {
        refreshToken();
      }
    }, 10 * 60 * 1000);

    return () => clearInterval(interval);
  }, [isAuthenticated]);

  const handleLogout = async () => {
    try {
      await logout();
      setIsAuthenticated(false);
      setUser(null);
      navigate('/');
    } catch (error) {
      console.error('Logout failed', error);
    }
  };

  return (
    <Box bg="gray.800" px={4}>
      <Flex h={16} alignItems="center" justifyContent="space-between">
        <IconButton
          size="md"
          icon={isOpen ? <CloseIcon /> : <HamburgerIcon />}
          aria-label="Open Menu"
          display={{ md: 'none' }}
          onClick={isOpen ? onClose : onOpen}
        />

        <HStack spacing={8} alignItems="center">
          <Box color="white" fontSize="xl" fontWeight="bold">Book The Room</Box>

          <HStack as="nav" spacing={4} display={{ base: 'none', md: 'flex' }}>
            <Link as={RouterLink} to="/" px={2} py={1} rounded="md" _hover={{ bg: 'gray.700' }} color="white">
              Home
            </Link>
            <Link as={RouterLink} to="/hotels" px={2} py={1} rounded="md" _hover={{ bg: 'gray.700' }} color="white">
              Hotels
            </Link>
            <Link as={RouterLink} to="/apartments" px={2} py={1} rounded="md" _hover={{ bg: 'gray.700' }} color="white">
              Apartments
            </Link>
            <Link as={RouterLink} to="/faq" px={2} py={1} rounded="md" _hover={{ bg: 'gray.700' }} color="white">
              FAQ
            </Link>
            <Link as={RouterLink} to="/support" px={2} py={1} rounded="md" _hover={{ bg: 'gray.700' }} color="white">
              Support
            </Link>
          </HStack>
        </HStack>

        <Flex alignItems="center">
          {isAuthenticated ? (
            <>
              <Text color="white" mr={4}>Welcome, {user?.userName || user?.email}</Text>
              <Button colorScheme="red" size="sm" onClick={handleLogout}>
                Logout
              </Button>
            </>
          ) : (
            <>
              <Button as={RouterLink} to="/login" colorScheme="blue" size="sm" mr={2}>
                Login
              </Button>
              <Button as={RouterLink} to="/register" colorScheme="green" size="sm">
                Register
              </Button>
            </>
          )}
        </Flex>
      </Flex>

      {isOpen ? (
        <Box pb={4} display={{ md: 'none' }}>
          <Stack as="nav" spacing={4}>
            <Link as={RouterLink} to="/">Home</Link>
            <Link as={RouterLink} to="/hotels">Hotels</Link>
            <Link as={RouterLink} to="/apartments">Apartments</Link>
            <Link as={RouterLink} to="/faq">FAQ</Link>
            <Link as={RouterLink} to="/support">Support</Link>
          </Stack>
        </Box>
      ) : null}
    </Box>
  );
}

export default Navbar;