import HotelCard from './HotelCard';
import HotelFilter from './HotelFilter';
import { fetchHotels } from '../../services/hotels';
import { useEffect, useState } from 'react';

function Hotels(){
    const [hotels, setHotels] = useState([]);
    const [filter, setFilter] = useState({        
        search: "",
        sortItem: "id",
        sortOrder: "asc",
        countries: [],
        ratings: []
    });

    useEffect(() => {

        const fetchData = async () => {
            let hotels = await fetchHotels(filter);
            setHotels(hotels);
        }

        fetchData();
    }, [filter]);

    return (        
        <section className="p-8 flex flex-row justify-start itemx-staart gap-12">
   
            <div className="flex flex-col w-1/2 gap-10">
                <h1>Filters</h1>
                <HotelFilter filter={filter} setFilter={setFilter}/>
                    
            </div>
            <div className="flex flex-col w-1/2 gap-10">
                <h1>Hotels</h1>      

                {hotels.map((h) => (
                    <li key = {h.id}>
                    <ul>
                        <HotelCard
                        name={h.name}
                        rating={h.rating}
                        address={h.address}
                        preview={h.preview}
                        />
                    </ul>
                    </li>
                ))}
                      
            </div>
        </section>
    );
}

export default Hotels;