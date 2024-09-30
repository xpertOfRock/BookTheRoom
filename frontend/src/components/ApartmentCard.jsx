import { Card, CardHeader, CardBody, CardFooter, Heading, Divider, Image, Text } from '@chakra-ui/react';

function ApartmentCard({ name, description, preview, rating, address }) {
  return (
    <Card variant={"filled"}>
      <CardHeader>
        <Heading size={"md"}>{name}</Heading>
      </CardHeader>
      <Divider borderColor={"gray"} />
      <CardBody>
        <Image 
          src={preview} 
          alt={name}
          placeholder={"https://media.istockphoto.com/id/1222357475/vector/image-preview-icon-picture-placeholder-for-website-or-ui-ux-design-vector-illustration.jpg?s=612x612&w=0&k=20&c=KuCo-dRBYV7nz2gbk4J9w1WtTAgpTdznHu55W9FjimE="}
        />
        <Text>{description}</Text>
        <Divider borderColor={"gray"} />
        <Text>Location: {address}</Text>
      </CardBody>
      <CardFooter>Rating {rating} ★</CardFooter>
    </Card>
  );
}

export default ApartmentCard;
