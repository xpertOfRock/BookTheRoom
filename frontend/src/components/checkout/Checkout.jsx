import React, { useEffect, useState, useRef } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import axios from "axios";
import dropin from "braintree-web-drop-in";

function Checkout() {
  const navigate = useNavigate();
  const location = useLocation();

  const {
    hotelId,
    roomNumber,
    checkIn,
    checkOut,
    basePrice,
    roomName
  } = location.state || {};

  const dropInContainer = useRef(null);
  const [dropInInstance, setDropInInstance] = useState(null);
  const [clientToken, setClientToken] = useState("");

  const [days, setDays] = useState(1);
  const [totalPrice, setTotalPrice] = useState(basePrice);

  useEffect(() => {
    if (checkIn && checkOut) {
      const inDate = new Date(checkIn);
      const outDate = new Date(checkOut);
      const diff = Math.ceil((outDate - inDate) / (1000 * 60 * 60 * 24));
      const validDays = diff > 0 ? diff : 1;
      setDays(validDays);
      setTotalPrice(basePrice * validDays);
    }
  }, [checkIn, checkOut, basePrice, navigate]);

  useEffect(() => {
    const fetchClientToken = async () => {
      try {
        const response = await axios.get("/api/order/client-token");
        setClientToken(response.data);
      } catch (error) {
        console.error("Error occured while fetching the client token:", error);
      }
    };
    fetchClientToken();
  }, []);

  useEffect(() => {
    if (!clientToken) return;

    dropin.create(
      {
        authorization: clientToken,
        container: dropInContainer.current,
        paypal: {
          flow: "vault",
        },
      },
      (err, instance) => {
        if (err) {
          console.error("Ошибка dropin.create:", err);
          return;
        }
        setDropInInstance(instance);
      }
    );
  }, [clientToken]);

  if (!hotelId || !roomNumber || !checkIn || !checkOut) {
    return (
      <div className="min-h-screen bg-gradient-to-r from-indigo-500 to-purple-600 flex items-center justify-center text-white">
        <div className="text-center">
          <h1 className="text-3xl font-bold mb-4">Нет данных для заказа</h1>
          <p>Пожалуйста, вернитесь и выберите комнату заново.</p>
        </div>
      </div>
    );
  }

  const handleBookSecurely = async () => {
    if (!dropInInstance) return;
    try {
      const { nonce } = await dropInInstance.requestPaymentMethod();

      const createOrderRequest = {
        NonceFromClient: nonce,
        Email: "user@example.com",
        Number: "1234567890",
        MinibarIncluded: false,
        MealsIncluded: false,
        CheckIn: checkIn,
        CheckOut: checkOut,
        CreatedAt: new Date().toISOString(),
        PaidImmediately: true,
        Status: 0,
      };

      const response = await axios.post(
        `/api/order/${hotelId}/${roomNumber}`,
        createOrderRequest
      );

      if (response.status === 201) {
        alert("Заказ успешно создан и оплачен!");
      } else {
        console.error("Ошибка при создании заказа:", response.data);
      }
    } catch (error) {
      console.error("Ошибка при оплате:", error);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-r from-indigo-500 to-purple-600 py-8 px-4 flex items-center justify-center">
      <div className="bg-white w-full max-w-4xl rounded-lg shadow-md grid grid-cols-1 md:grid-cols-2 overflow-hidden">
        
        <div
          className="relative bg-cover bg-center"
          style={{
            backgroundImage: `url("https://images.pexels.com/photos/2876787/pexels-photo-2876787.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=2")`,
            minHeight: "400px",
          }}
        >
          <div className="absolute inset-0 bg-indigo-900 bg-opacity-50 flex flex-col justify-end p-6">
            <h1 className="text-3xl text-white font-bold">
              {roomName || "Room"}
            </h1>
            <p className="text-lg text-gray-100 font-semibold">
              {basePrice} USD / ночь
            </p>
            <hr className="my-2 border-gray-300" />
            <p className="text-gray-200 text-sm">
              {days} {days === 1 ? "ночь" : "ночей"}: {checkIn} → {checkOut}
            </p>
          </div>
        </div>

        <div className="p-6 flex flex-col justify-between">
          <div>
            <h2 className="text-xl font-bold text-gray-800 mb-4">
              Receipt Summary
            </h2>
            <table className="w-full mb-6 text-gray-600">
              <tbody>
                <tr className="border-b">
                  <td className="py-2">
                    {basePrice} USD x {days} {days > 1 ? "ночи" : "ночь"}
                  </td>
                  <td className="text-right py-2">
                    {(basePrice * days).toFixed(2)} USD
                  </td>
                </tr>
                <tr className="border-b">
                  <td className="py-2">Discount</td>
                  <td className="text-right py-2">0.00 USD</td>
                </tr>
                <tr className="border-b">
                  <td className="py-2 font-semibold">Subtotal</td>
                  <td className="text-right py-2 font-semibold">
                    {(basePrice * days).toFixed(2)} USD
                  </td>
                </tr>
                <tr className="border-b">
                  <td className="py-2">Tax (пример ~10%)</td>
                  <td className="text-right py-2">
                    {((basePrice * days) * 0.1).toFixed(2)} USD
                  </td>
                </tr>
                <tr className="border-b">
                  <td className="py-2 font-bold text-lg">Total</td>
                  <td className="text-right py-2 font-bold text-lg">
                    {(totalPrice * 1.1).toFixed(2)} USD
                  </td>
                </tr>
              </tbody>
            </table>
          </div>

          <div>
            <h3 className="text-md font-semibold text-gray-800 mb-2">
              Payment Information
            </h3>
            <div
              className="bg-gray-100 p-4 rounded-md"
              ref={dropInContainer}
              style={{ minHeight: "200px" }}
            ></div>

            <button
              className="mt-4 w-full bg-indigo-600 hover:bg-indigo-700 text-white font-bold py-3 rounded-md transition-colors"
              onClick={handleBookSecurely}
            >
              Book Securely
            </button>
            <p className="text-xs text-gray-500 text-center mt-2">
              <i className="fa-solid fa-lock mr-1" />
              Your credit card information is encrypted
            </p>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Checkout;