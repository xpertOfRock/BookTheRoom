import {
  Box,
  Heading,
  Text,
  VStack,
  Link,
  Container,
  Divider,
  useColorModeValue,
} from "@chakra-ui/react";

function Support() {
  const linkColor = useColorModeValue("purple.600", "purple.300");

  return (
    <Container maxW="container.md" py={10}>
      <Heading as="h2" size="xl" mb={6} color="purple.700" textAlign="center">
        Support & Contact
      </Heading>

      <VStack spacing={6} align="start" fontSize="md" color="gray.700">
        <Box>
          <Heading size="md" mb={2}>
            Refund Policy
          </Heading>
          <Text>
            Refunds for paid bookings are available only if the cancellation is made at least 3 days before the check-in date. Please review your booking details carefully and contact us within this timeframe to initiate a refund.
          </Text>
        </Box>

        <Divider />

        <Box>
          <Heading size="md" mb={2}>
            Assistance with Website Navigation
          </Heading>
          <Text>
            If you need help navigating the website or using its features, our support team is here to assist you. Whether it's searching for apartments, managing your bookings, or understanding payment options, feel free to reach out.
          </Text>
          <Text mt={2}>
            Email:{" "}
            <Link href="mailto:booktheroomhelp@gmail.com" color={linkColor} fontWeight="semibold">
              booktheroomhelp@gmail.com
            </Link>
          </Text>
        </Box>
      </VStack>
    </Container>
  );
}

export default Support;