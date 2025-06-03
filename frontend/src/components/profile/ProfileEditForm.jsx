import React, { useState, useEffect } from "react";
import {
  Box,
  Avatar,
  Stack,
  Button,
  Input,
  useColorModeValue,
  Text,
  Icon,
  Center,
} from "@chakra-ui/react";
import { FiCamera } from "react-icons/fi";

const ProfileEditForm = ({ user, onCancel, onSave, formatDate }) => {
  const bg = useColorModeValue("white", "gray.800");
  const mainColor = "purple.600";
  const accentColor = "purple.300";
  const textColor = "gray.700";

  const [formData, setFormData] = useState({
    email: user.email || "",
    phoneNumber: user.phoneNumber || "",
    firstName: user.firstName || "",
    lastName: user.lastName || "",
    image: null,
  });
  const [preview, setPreview] = useState(user.image || "");
  const [isHover, setIsHover] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);

  useEffect(() => {
    return () => {
      if (preview && preview !== user.image) {
        URL.revokeObjectURL(preview);
      }
    };
  }, [preview, user.image]);

  const handleChange = (e) => {
    const { name, files, value } = e.target;
    if (name === "image" && files && files[0]) {
      const file = files[0];
      setFormData((prev) => ({ ...prev, image: file }));
      const url = URL.createObjectURL(file);
      setPreview(url);
    } else {
      setFormData((prev) => ({ ...prev, [name]: value }));
    }
  };

  const openFileDialog = () => {
    document.getElementById("photo-upload").click();
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setIsSubmitting(true);
    await onSave(formData);
    setIsSubmitting(false);
  };

  return (
    <Box
      as="form"
      w={["100%", "100%", "33%"]}
      bg={bg}
      boxShadow="lg"
      rounded="xl"
      p={6}
      textAlign="center"
      border="2px solid"
      borderColor={accentColor}
      mb={{ base: 6, md: 0 }}
      flexShrink={0}
      onSubmit={handleSubmit}
    >
      <Box
        position="relative"
        width="fit-content"
        mx="auto"
        mb={4}
        onMouseEnter={() => setIsHover(true)}
        onMouseLeave={() => setIsHover(false)}
        cursor="pointer"
        onClick={openFileDialog}
      >
        <Avatar
          size="2xl"
          src={preview}
          alt={`${formData.firstName || user.firstName} ${formData.lastName || user.lastName}`}
          borderWidth="4px"
          borderColor={mainColor}
        />
        {isHover && (
          <Center
            position="absolute"
            top="0"
            left="0"
            right="0"
            bottom="0"
            bg="rgba(0, 0, 0, 0.5)"
            borderRadius="full"
            color="white"
          >
            <Icon as={FiCamera} boxSize={10} />
          </Center>
        )}
      </Box>

      <Input
        id="photo-upload"
        type="file"
        name="image"
        accept="image/*"
        onChange={handleChange}
        display="none"
      />

      <Stack spacing={3} mt={6} textAlign="left" color={textColor}>
        <Text>
          <strong>Birth Date:</strong> {formatDate(user.birthDate)}
        </Text>
        <Text>
          <strong>Registered At:</strong> {formatDate(user.registeredAt)}
        </Text>
        <Box>
          <Text mb={1} fontWeight="bold">
            Email
          </Text>
          <Input
            name="email"
            value={formData.email}
            onChange={handleChange}
            type="email"
            required
          />
        </Box>
        <Box>
          <Text mb={1} fontWeight="bold">
            Phone
          </Text>
          <Input
            name="phoneNumber"
            value={formData.phoneNumber}
            onChange={handleChange}
            type="tel"
          />
        </Box>
        <Box>
          <Text mb={1} fontWeight="bold">
            First Name
          </Text>
          <Input
            name="firstName"
            value={formData.firstName}
            onChange={handleChange}
            required
          />
        </Box>
        <Box>
          <Text mb={1} fontWeight="bold">
            Last Name
          </Text>
          <Input
            name="lastName"
            value={formData.lastName}
            onChange={handleChange}
            required
          />
        </Box>
      </Stack>
      <Stack direction="row" spacing={4} mt={6} justify="center">
        <Button onClick={onCancel} colorScheme="purple" variant="outline" isDisabled={isSubmitting}>
          Cancel
        </Button>
        <Button colorScheme="purple" type="submit" isLoading={isSubmitting} loadingText="Saving">
          Save
        </Button>
      </Stack>
    </Box>
  );
};

export default ProfileEditForm;
