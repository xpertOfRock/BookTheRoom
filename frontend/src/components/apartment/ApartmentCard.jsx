import { useNavigate } from "react-router-dom";

function ApartmentCard({ id, name, preview, address, userScore, createdAt }) {
  const navigate = useNavigate();

  const handleViewClick = () => {
    navigate(`/apartments/${id}`);
  };

  const formattedDate = createdAt
    ? new Date(createdAt).toLocaleString(undefined, {
        year: 'numeric',
        month: '2-digit',
        day: '2-digit',
        hour: '2-digit',
        minute: '2-digit',
      })
    : '';

  return (
    <div className="relative max-w-sm bg-white border border-gray-200 rounded-lg shadow-md m-4 flex flex-col">
      {userScore !== undefined && userScore !== null && userScore >= 0 && (
        <div className="absolute -top-3 -right-3 bg-yellow-300 text-gray-800 font-bold px-2 py-1 text-sm rounded-lg shadow-lg z-10">
          {`User Score: ${userScore.toFixed(1)} ★`}
        </div>
      )}

      <img
        className="rounded-t-lg w-full h-48 object-cover"
        src={preview}
        alt={`${name} preview`}
        loading="lazy"
      />
      <div className="p-4 flex flex-col flex-grow">
        <h5 className="text-lg font-semibold text-gray-800">{name}</h5>
        <p className="text-gray-500 mt-3 text-sm">Location: {address}</p>
        <div className="flex items-center justify-end mt-auto pt-4 border-t border-gray-200">
          <button
            onClick={handleViewClick}
            className="px-3 py-1 text-sm font-medium text-white bg-blue-500 rounded hover:bg-blue-600"
          >
            View
          </button>
          <small className="text-gray-400 ml-4">{formattedDate}</small>
        </div>
      </div>
    </div>
  );
}

export default ApartmentCard;