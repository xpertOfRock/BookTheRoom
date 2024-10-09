// import { Box, Button, Heading, Input, VStack, useToast } from "@chakra-ui/react";
// import { useState } from "react";

// const Login = () => {
//     const [email, setEmail] = useState('');
//     const [password, setPassword] = useState('');
//     const toast = useToast();

//     const handleSubmit = async (e) => {
//         e.preventDefault();
//         try {
//             await login();
//         } catch (error) {
//             toast({
//                 title: "Error occurred while authorization process",
//                 description: error.message,
//                 status: "error",
//                 duration: 5000,
//                 isClosable: true,
//             });
//         }
//     };

//     return (
//         <Box
//             bg="blue.50"
//             display="flex"
//             justifyContent="center"
//             alignItems="center"
//             height="100vh"
//         >
//             <VStack
//                 as="form"
//                 onSubmit={handleSubmit}
//                 bg="white"
//                 p={8}
//                 spacing={4}
//                 boxShadow="lg"
//                 rounded="md"
//                 width="400px"
//             >
//                 <Heading as="h1" size="lg" color="blue.500">
//                     Login Page
//                 </Heading>
//                 <Input 
//                     placeholder="Email" 
//                     value={email}
//                     onChange={(e) => setEmail(e.target.value)}
//                     type="email"
//                     required
//                     bg="blue.100"
//                 />
//                 <Input 
//                     placeholder="Password" 
//                     value={password}
//                     onChange={(e) => setPassword(e.target.value)}
//                     type="password"
//                     required
//                     bg="blue.100"
//                 />
//                 <Button type="submit" colorScheme="blue" width="full">
//                     Login
//                 </Button>
//             </VStack>
//         </Box>
//     );
// };

// export default Login;