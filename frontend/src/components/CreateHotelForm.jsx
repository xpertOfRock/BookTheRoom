import { 
    Button, 
    NumberInput, 
    NumberInputField,
    NumberInputStepper,
    NumberIncrementStepper,
    NumberDecrementStepper, 
    Input,  
    Select, 
    Text, 
    Textarea 
} from "@chakra-ui/react";
import { useState } from "react";


function CreateHotelForm ({onCreate}) {
    const [images, setImages] = useState([]);
    const [hotel, setHotel] = useState({
        Name: "",
        Description: "",
        HasPool: false,
        RoomsAmount: 40,
        Rating: 5,
        Address: {
            Country: "",
            State: "",
            City: "",
            Street: "",
            PostalCode: ""
        }
    });

    const handleImage = async (e) => {
        const filesArray = Array.from(e.target.files);
        setImages(filesArray);
    }

    const onSubmit = async (e) => {
        e.preventDefault();
        setHotel({
            Name: "",
            Description: "",
            HasPool: false,
            RoomsAmount: 40,
            Rating: 5,
            Address: {
                Country: "",
                State: "",
                City: "",
                Street: "",
                PostalCode: ""
            }
        });
        onCreate(hotel, images);
    }

    return (
        <form onSubmit={onSubmit} className="w-full flex flex-col gap-3" encType="multipart/form-data">
            <h3 className="font-bold text-x1">Create new hotel</h3>
            <div className="w-1/2 flex flex-col gap-3">
                <Text>Name</Text>
                <Input 
                    placeholder="Write name here"
                    value={hotel.Name}
                    onChange={(e) => setHotel({...hotel, Name: e.target.value})}      
                />
                <Text>Description</Text>
                <Textarea
                    placeholder="Write description here"
                    value={hotel.Description}
                    onChange={(e) => setHotel({...hotel, Description: e.target.value})}            
                />
                <Text>Has pool</Text>
                <Select value={hotel.HasPool.toString()} onChange={(e) => setHotel({...hotel, HasPool: e.target.value === "true"})}>
                    <option value="true">Yes</option>
                    <option value="false">No</option>
                </Select>
                <Text>Amount of rooms</Text>
                <NumberInput 
                    step={1} 
                    value={hotel.RoomsAmount}
                    min={10} 
                    max={8000}
                    onChange={(value) => setHotel({...hotel, RoomsAmount: parseInt(value, 10)})}>
                    <NumberInputField />
                    <NumberInputStepper>
                        <NumberIncrementStepper />
                        <NumberDecrementStepper />
                    </NumberInputStepper>
                </NumberInput>
                <Text>Rating in ★</Text>
                <NumberInput 
                    step={1} 
                    value={hotel.Rating}
                    min={1} 
                    max={5}
                    onChange={(value) => setHotel({...hotel, Rating: parseInt(value, 10)})}>
                    <NumberInputField />
                    <NumberInputStepper>
                        <NumberIncrementStepper />
                        <NumberDecrementStepper />
                    </NumberInputStepper>
                </NumberInput>
            </div>
            <div className="w-1/2 flex flex-col gap-3">   
                <Text>Address</Text>            
                <Input 
                    placeholder="Country"
                    value={hotel.Address.Country}
                    onChange={(e) => setHotel({
                        ...hotel, 
                        Address: {
                            ...hotel.Address,
                            Country: e.target.value
                        }
                    })}            
                />
                <Input 
                    placeholder="State"
                    value={hotel.Address.State}
                    onChange={(e) => setHotel({
                        ...hotel, 
                        Address: {
                            ...hotel.Address,
                            State: e.target.value
                        }
                    })}           
                />
                <Input 
                    placeholder="City"
                    value={hotel.Address.City}
                    onChange={(e) => setHotel({
                        ...hotel, 
                        Address: {
                            ...hotel.Address,
                            City: e.target.value
                        }
                    })}           
                />
                <Input 
                    placeholder="Street"
                    value={hotel.Address.Street}
                    onChange={(e) => setHotel({
                        ...hotel, 
                        Address: {
                            ...hotel.Address,
                            Street: e.target.value
                        }
                    })}           
                />
                <Input 
                    placeholder="Postal Code"
                    value={hotel.Address.PostalCode}
                    onChange={(e) => setHotel({
                        ...hotel, 
                        Address: {
                            ...hotel.Address,
                            PostalCode: e.target.value
                        }
                    })}           
                />
                <Input
                    type="file"
                    multiple
                    onChange={handleImage}      
                />
            </div>
            
            <Button type="submit" variant={"filled"}>Create!</Button>
        </form>
    );
}

export default CreateHotelForm;
