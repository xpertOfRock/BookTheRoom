import { Box, Flex, HStack, Link, IconButton, useDisclosure, Stack, Button } from '@chakra-ui/react';
import { HamburgerIcon, CloseIcon } from '@chakra-ui/icons'; 
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import { getCurrentToken, getCurrentUser, logout, refreshToken } from '../services/auth';

function Navbar() {
  const { isOpen, onOpen, onClose } = useDisclosure();
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [user, setUser] = useState(null);
  const navigate = useNavigate();

  const bgColor = 'purple.900';
  const linkColor = 'white';
  const linkHoverBg = 'purple.700';
  const buttonColorScheme = 'purple';

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
      window.location.reload();
    } catch (error) {
      console.error('Logout failed', error);
    }
  };

  return (
    <Box bg={bgColor} px={4}>
      <Flex h={16} alignItems="center" justifyContent="space-between">
        <IconButton
          size="md"
          icon={isOpen ? <CloseIcon /> : <HamburgerIcon />}
          aria-label="Open Menu"
          display={{ md: 'none' }}
          onClick={isOpen ? onClose : onOpen}
          color={linkColor}
          bg="transparent"
          _hover={{ bg: linkHoverBg }}
        />

        <HStack spacing={8} alignItems="center">
          <Box color={linkColor} fontSize="xl" fontWeight="bold">Book The Room</Box>

          <HStack as="nav" spacing={4} display={{ base: 'none', md: 'flex' }}>
            {['/', '/hotels', '/apartments', '/faq', '/support'].map((path, idx) => {
              const name = ['Home', 'Hotels', 'Apartments', 'FAQ', 'Support'][idx];
              return (
                <Link
                  key={path}
                  as={RouterLink}
                  to={path}
                  px={2}
                  py={1}
                  rounded="md"
                  color={linkColor}
                  _hover={{ bg: linkHoverBg }}
                >
                  {name}
                </Link>
              );
            })}
          </HStack>
        </HStack>

        <Flex alignItems="center">
          {isAuthenticated ? (
            <>
              <Button
                as={RouterLink}
                to="/profile"
                colorScheme={buttonColorScheme}
                size="sm"
                mr={4}
              >
                Profile
              </Button>
              <Button colorScheme="red" size="sm" onClick={handleLogout}>
                Logout
              </Button>
            </>
          ) : (
            <>
              <Button as={RouterLink} to="/login" colorScheme={buttonColorScheme} size="sm" mr={2}>
                Login
              </Button>
              <Button as={RouterLink} to="/register" colorScheme={buttonColorScheme} size="sm">
                Register
              </Button>
            </>
          )}
        </Flex>
      </Flex>

      {isOpen ? (
        <Box pb={4} display={{ md: 'none' }}>
          <Stack as="nav" spacing={4}>
            {['/', '/hotels', '/apartments', '/faq', '/support'].map((path, idx) => {
              const name = ['Home', 'Hotels', 'Apartments', 'FAQ', 'Support'][idx];
              return (
                <Link
                  key={path}
                  as={RouterLink}
                  to={path}
                  px={2}
                  py={1}
                  rounded="md"
                  color={linkColor}
                  _hover={{ bg: linkHoverBg }}
                >
                  {name}
                </Link>
              );
            })}
          </Stack>
        </Box>
      ) : null}
    </Box>
  );
}

export default Navbar;
