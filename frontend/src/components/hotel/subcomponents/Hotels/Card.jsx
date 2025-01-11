import { useNavigate } from 'react-router-dom';

function Card({ id, name,  preview, rating, address, userScore }) {
  const navigate = useNavigate(); 

  const handleViewClick = () => {
    navigate(`/hotels/${id}`);
  };

  return (    
    <div className="relative max-w-sm bg-white border border-gray-200 rounded-lg shadow-md m-4 flex flex-col">
      {userScore !== undefined && userScore !== null && userScore >= 0 && (
        <div className="absolute -top-3 -right-3 bg-yellow-300 text-gray-800 -mt- font-bold px-2 py-1 text-sm rounded-lg shadow-lg z-10">
          {`User Score: ${userScore.toFixed(1)} ★`}
        </div>
      )}

      <img
        className="rounded-t-lg w-full h-48 object-cover"
        src={preview}
        alt="Hotel preview"
        loading="lazy"
      />
      <div className="p-3 flex flex-col flex-grow">
        <h5 className="text-lg font-semibold text-gray-800">{name}</h5>
        <p className="text-gray-500 mt-2 text-sm">Location: {address}</p>
        <div className="flex-grow"></div>
        <div className="flex items-center justify-between mt-4">
          <button
            onClick={handleViewClick}
            className="px-3 py-1 text-sm font-medium text-white bg-blue-500 rounded hover:bg-blue-600"
          >
            View
          </button>
          <small className="text-gray-400">Rating: {rating} ★</small>
        </div>
      </div>
    </div>
  );
}

export default Card;