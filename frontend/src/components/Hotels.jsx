import HotelCard from '../components/HotelCard';
import HotelFilter from '../components/HotelFilter';

function Hotels({hotels, filter, setFilter}){
    
    return (        
        <section className="p-8 flex flex-row justify-start itemx-staart gap-12">
   
            <div className="flex flex-col w-1/2 gap-10">
                <h1>Filters</h1>

                <HotelFilter filter={filter} setFilter={setFilter}/>

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