import { Box, Heading, Text, Button, VStack, Image, Flex, SimpleGrid } from "@chakra-ui/react";
import { Link as RouterLink } from "react-router-dom";

function Home() {
  return (
    <Box maxW="container.lg" mx="auto" p={6}>
      <VStack spacing={6} textAlign="center">
        <Heading as="h1" size="2xl" color="purple.700">
          Welcome to BookTheRoom
        </Heading>
        <Text fontSize="lg" maxW="600px" color="gray.600">
          Find and book your perfect room in hotels and apartments quickly and easily.
        </Text>

        <Flex gap={6} justifyContent="center" mt={4} w="100%">
          <Button
            colorScheme="purple"
            size="lg"
            as={RouterLink}
            to="/hotels"
            px={10}
          >
            Browse Hotels
          </Button>
          <Button
            colorScheme="purple"
            size="lg"
            as={RouterLink}
            to="/apartments"
            px={10}
          >
            Browse Apartments
          </Button>
        </Flex>
      </VStack>

      <Flex mt={12} direction={["column", "row"]} justify="space-around" gap={6}>
        <Box maxW="300px" textAlign="center" p={4} borderWidth="1px" borderRadius="md" boxShadow="md">
          <Image
            src="/images/easy-booking.webp"
            alt="Easy Booking"
            mb={4}
            borderRadius="md"
            objectFit="cover"
            maxH="180px"
            mx="auto"
          />
          <Heading size="md" mb={2}>Easy Booking</Heading>
          <Text fontSize="sm" color="gray.600">
            Quickly browse hotels and book rooms with just a few clicks, or try browsing users' apartments.
          </Text>
        </Box>

        <Box maxW="300px" textAlign="center" p={4} borderWidth="1px" borderRadius="md" boxShadow="md">
          <Image
            src="/images/secure-payment.webp"
            alt="Secure Payment"
            mb={4}
            borderRadius="md"
            objectFit="cover"
            maxH="180px"
            mx="auto"
          />
          <Heading size="md" mb={2}>Secure Payment</Heading>
          <Text fontSize="sm" color="gray.600">
            All transactions are safe and encrypted.
          </Text>
        </Box>

        <Box maxW="300px" textAlign="center" p={4} borderWidth="1px" borderRadius="md" boxShadow="md">
          <Image
            src="/images/customer-support.webp"
            alt="Customer Support"
            mb={4}
            borderRadius="md"
            objectFit="cover"
            maxH="180px"
            mx="auto"
          />
          <Heading size="md" mb={2}>24/7 Support</Heading>
          <Text fontSize="sm" color="gray.600">
            Our support team is here to help anytime.
          </Text>
        </Box>
      </Flex>

      <Box mt={20} textAlign="center" px={4}>
        <Heading size="xl" mb={6} color="purple.700">
          How It Works
        </Heading>
        <SimpleGrid columns={[1, null, 3]} spacing={8} maxW="container.md" mx="auto">
          <Box p={4} borderWidth="1px" borderRadius="md" boxShadow="sm">
            <Heading size="md" mb={2}>Search</Heading>
            <Text fontSize="sm" color="gray.600">
              Use filters to find the perfect hotel or apartment that fits your needs.
            </Text>
          </Box>
          <Box p={4} borderWidth="1px" borderRadius="md" boxShadow="sm">
            <Heading size="md" mb={2}>Book</Heading>
            <Text fontSize="sm" color="gray.600">
              Securely book your chosen room online within minutes.
            </Text>
          </Box>
          <Box p={4} borderWidth="1px" borderRadius="md" boxShadow="sm">
            <Heading size="md" mb={2}>Enjoy</Heading>
            <Text fontSize="sm" color="gray.600">
              Relax and enjoy your stay, with 24/7 support if you need assistance.
            </Text>
          </Box>
        </SimpleGrid>
      </Box>
    </Box>
  );
}

export default Home;