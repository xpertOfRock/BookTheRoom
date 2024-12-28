import { useNavigate } from 'react-router-dom';

function Card({ id, name, description, preview, rating, address }) {
  const navigate = useNavigate(); 

  const handleViewClick = () => {
    navigate(`/hotels/${id}`);
  };

  return (
    <div className="max-w-sm bg-white border border-gray-200 rounded-lg shadow-md m-4">
      <img
        className="rounded-t-lg w-full h-48 object-cover"
        src={preview}
        alt="Hotel preview"
      />
      <div className="p-4">
        <h5 className="text-lg font-semibold text-gray-800">{name}</h5>
        <p className="text-gray-600 mt-2">{description}</p>
        <p className="text-gray-500 mt-2 text-sm">Location: {address}</p>
        <div className="flex items-center justify-between mt-4">
          <div className="flex gap-2">
            <button 
              onClick={handleViewClick}
              className="px-3 py-1 text-sm font-medium text-white bg-blue-500 rounded hover:bg-blue-600"
            >
              View
            </button>
          </div>
          <small className="text-gray-400">Rating: {rating} ★</small>
        </div>
      </div>
    </div>
  );
}

export default Card;