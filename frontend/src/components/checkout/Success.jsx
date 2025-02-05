import { useToast } from "@chakra-ui/react";
import React from "react";
import { useLocation, useNavigate } from "react-router-dom";
import { useEffect } from "react";

function Success() {
  const navigate = useNavigate();
  const location = useLocation();
  const redirected = location.state || undefined;

  useEffect(() => {
    console.log(redirected);
    if(redirected === null || redirected === undefined || !redirected){      
      navigate("/");     
    }
  },[]);

  return (
    <div className="bg-[#EBF0F5] py-10 text-center min-h-screen flex items-center justify-center">
      <div className="bg-white p-16 rounded shadow inline-block mx-auto">
        <div className="w-[200px] h-[200px] bg-[#F8FAF5] rounded-full mx-auto flex items-center justify-center">
          <span className="text-[#9ABC66] text-[100px] leading-[200px]">
            ✓
          </span>
        </div>
        <h1 className="text-[#88B04B] font-black text-[40px] mb-[10px] mt-4">
          Success
        </h1>
        <p className="text-[#404F5E] text-[20px]">
          We received your purchase request
          <br />
          The information has been sent to your email.
        </p>
      </div>
    </div>
  );
};

export default Success;