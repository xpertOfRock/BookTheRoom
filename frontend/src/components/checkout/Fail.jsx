import { FaTimesCircle } from 'react-icons/fa';
import { useLocation, useNavigate } from "react-router-dom";
import { useEffect } from "react";

function Fail() {
const navigate = useNavigate();
  const location = useLocation();
  const redirected = location.state || undefined;

  useEffect(() => {
    if(redirected === null || redirected === undefined || !redirected){      
      navigate("/");     
    }
  },[]);

  return (
    <div className="container mx-auto px-4 py-6">
      <div
        className="
          w-full max-w-md
          shadow-[0px_15px_25px_rgba(0,0,0,0.1)]
          p-[45px]
          mx-auto my-[40px]
          text-center
          border-b-[4px] border-b-red-500
          rounded-lg
          bg-white
        "
      >
        <FaTimesCircle className="mx-auto text-[55px] text-red-500" />
        <h2
          className="
            mt-[10px] mb-[12px]
            text-[40px]
            font-medium
            leading-[1.2]
            text-gray-900
          "
        >
          Your payment failed
        </h2>
        <p className="mb-0 text-[18px] font-medium text-gray-600">
          Try again later.
        </p>
      </div>
    </div>
  );
}

export default Fail;