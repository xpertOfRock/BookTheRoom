import { useLocation, useNavigate } from "react-router-dom";
import { useEffect } from "react";
import { FaCheckCircle } from 'react-icons/fa';

function Success() {
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
          border-b-[4px] border-b-[#28a745]
          rounded-lg
          bg-white
        "
      >
        <FaCheckCircle className="mx-auto text-[55px] text-[#28a745]" />
        <h2
          className="
            mt-[10px] mb-[12px]
            text-[40px]
            font-medium
            leading-[1.2]
            text-gray-900
          "
        >
          Your payment was successful
        </h2>
        <p className="mb-0 text-[18px] font-medium text-[#495057]">
          Thank you for your payment. We will<br />
          be in contact with more details shortly.
        </p>
      </div>
    </div>
  );
};

export default Success;