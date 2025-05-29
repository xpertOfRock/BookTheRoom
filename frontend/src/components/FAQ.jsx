import {
  Box,
  Heading,
  Accordion,
  AccordionItem,
  AccordionButton,
  AccordionPanel,
  AccordionIcon,
  Container,
} from "@chakra-ui/react";

const faqs = [
  {
    question: "How do I book a room?",
    answer:
      "Simply browse available apartments or hotels, select dates, and confirm your booking with secure payment.",
  },
  {
    question: "Can I cancel my booking?",
    answer:
      "Yes, cancellations are possible depending on the property's cancellation policy. Visit support page for detail information.",
  },
  {
    question: "What payment methods are accepted?",
    answer: "We accept all major credit cards, PayPal, and other secure payment options.",
  },
  {
    question: "Is my personal data secure?",
    answer:
      "Absolutely. We use industry-standard encryption to protect your information.",
  },
];

function FAQ() {
  return (
    <Container maxW="container.md" py={10}>
      <Heading as="h2" size="xl" mb={6} color="purple.700" textAlign="center">
        Frequently Asked Questions
      </Heading>

      <Accordion allowMultiple>
        {faqs.map(({ question, answer }, idx) => (
          <AccordionItem key={idx} borderColor="purple.300" borderWidth="1px" mb={3} borderRadius="md">
            <AccordionButton _expanded={{ bg: "purple.600", color: "white" }} borderRadius="md" py={4}>
              <Box flex="1" textAlign="left" fontWeight="semibold" fontSize="lg">
                {question}
              </Box>
              <AccordionIcon />
            </AccordionButton>
            <AccordionPanel pb={4} fontSize="md" color="gray.700">
              {answer}
            </AccordionPanel>
          </AccordionItem>
        ))}
      </Accordion>
    </Container>
  );
}

export default FAQ;