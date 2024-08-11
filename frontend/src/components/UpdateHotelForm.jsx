import { Button, Checkbox, Input, Radio, Select, TagLabel, Textarea } from "@chakra-ui/react";
import { useState } from "react";
import { postHotels } from "../services/hotels";

function UpdateHotelForm ({onCreate}) {
    const [hotel, setHotel] = useState(null);

    const onSubmit = (e) => {
        e.preventDefault();
        setHotel(null);
        postHotels(hotel);
    }

    return (
        <form onSubmit={onSubmit} className="w-full flex flex-col gap-3">
            <h3 className="font-bold text-x1">Create new hotel</h3>
            <div className="w-1/2 flex flex-col gap-3">
                <Text>Name</Text>
                <Input 
                    placeholder="Write name here"
                    value={hotel?.name ?? ""}
                    onChange={(e) => setHotel({...hotel, name: e.target.value})}      
                />
                <Text>Description</Text>
                <Textarea
                    placeholder="Write description here"
                    value={hotel?.description ?? ""}
                    onChange={(e) => setHotel({...hotel, description: e.target.value})}            
                />
                <Text>Has pool</Text>
                <Select onChange={(e) => setFilter({...hotel, HasPool: e.target.value})}>
                <option>---</option>
                </Select>
                <Input 
                    placeholder="Name"
                    value={hotel?.name ?? ""}
                    onChange={(e) => setHotel({...hotel, name: e.target.value})}      
                />
            </div>
            <div className="w-1/2 flex flex-col gap-3">
                <Input 
                    placeholder="Name"
                    value={hotel?.name ?? ""}
                    onChange={(e) => setHotel({...hotel, name: e.target.value})}      
                />
                <Input 
                    placeholder="Name"
                    value={hotel?.name ?? ""}
                    onChange={(e) => setHotel({...hotel, name: e.target.value})}      
                />
            </div>
            
            <Button type="submit" variant={"filled"}>Create!</Button>
        </form>
    );
}

export default UpdateHotelForm;