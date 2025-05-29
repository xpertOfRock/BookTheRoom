import { Box, Flex, Text, Link, Stack, IconButton, useColorModeValue } from "@chakra-ui/react";
import { FaFacebook, FaTwitter, FaInstagram } from "react-icons/fa";

function Footer() {
  return (
    <Box
      bg={useColorModeValue("purple.700", "purple.900")}
      color="white"
      py={8}
      px={6}
      mt={20}
    >
      <Flex
        direction={["column", "row"]}
        maxW="container.lg"
        mx="auto"
        justify="space-between"
        align="center"
        gap={4}
      >
        <Text fontWeight="bold" fontSize="lg">
          © {new Date().getFullYear()} BookTheRoom. All rights reserved.
        </Text>

        <Stack direction="row" spacing={6} align="center">
          <Link href="/about" _hover={{ textDecoration: "underline" }}>
            About Us
          </Link>
          <Link href="/contact" _hover={{ textDecoration: "underline" }}>
            Contact
          </Link>
          <Link href="/privacy" _hover={{ textDecoration: "underline" }}>
            Privacy Policy
          </Link>
        </Stack>

        {/* <Stack direction="row" spacing={4}>
          <IconButton
            as="a"
            href="https://facebook.com"
            aria-label="Facebook"
            icon={<FaFacebook />}
            colorScheme="facebook"
            size="md"
            isRound
          />
          <IconButton
            as="a"
            href="https://twitter.com"
            aria-label="Twitter"
            icon={<FaTwitter />}
            colorScheme="twitter"
            size="md"
            isRound
          />
          <IconButton
            as="a"
            href="https://instagram.com"
            aria-label="Instagram"
            icon={<FaInstagram />}
            colorScheme="pink"
            size="md"
            isRound
          />
        </Stack> */}
      </Flex>
    </Box>
  );
}

export default Footer;