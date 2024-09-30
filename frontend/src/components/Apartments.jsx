import ApartmentCard from '../components/ApartmentCard';
import ApartmentFilter from '../components/ApartmentFilter';

function Apartments({ apartments, filter, setFilter }) {
  
  return (        
    <section className="p-8 flex flex-row justify-start itemx-staart gap-12">
   
      <div className="flex flex-col w-1/2 gap-10">
        <h1>Filters</h1>

        <ApartmentFilter filter={filter} setFilter={setFilter} />

        <h1>Apartments</h1>      

        {apartments.map((a) => (
          <li key={a.id}>
            <ul>
              <ApartmentCard
                name={a.name}
                rating={a.rating}
                address={a.address}
                preview={a.preview}
                description={a.description}
              />
            </ul>
          </li>
        ))}

      </div>
    </section>
  );
}

export default Apartments;
