import React, { useEffect, useRef, useState } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import dropin from "braintree-web-drop-in";
import { getCurrentUser, isAuthorized } from "../../services/auth";
import { getClientToken, postOrder } from "../../services/orders";
import { useToast, Button } from "@chakra-ui/react";

function Checkout() {
  const navigate = useNavigate();
  const location = useLocation();
  const toast = useToast();

  const {
    hotelId,
    roomNumber,
    checkIn,
    checkOut,
    basePrice = 0,
    roomName = "Unknown Room",
  } = location.state || {};

  const currentUser = isAuthorized() ? getCurrentUser() : null;
  const comission = 0.05;

  const [orderData, setOrderData] = useState({
    firstName: currentUser?.firstName || "",
    lastName: currentUser?.lastName || "",
    email: currentUser?.email || "",
    phone: currentUser?.phoneNumber || "",
    minibarIncluded: false,
    mealsIncluded: false,
  });

  const [days, setDays] = useState(1);
  const [subTotal, setSubTotal] = useState(basePrice);
  const [totalPrice, setTotalPrice] = useState(basePrice);
  const [clientToken, setClientToken] = useState("");
  const [dropInInstance, setDropInInstance] = useState(null);
  const dropInContainer = useRef(null);
  const [isProcessing, setIsProcessing] = useState(false);

  useEffect(() => {
    if (!hotelId || !roomNumber || !checkIn || !checkOut) navigate("/");
  }, [hotelId, roomNumber, checkIn, checkOut, navigate]);

  useEffect(() => {
    if (checkIn && checkOut) {
      const inDate = new Date(checkIn);
      const outDate = new Date(checkOut);
      const diff = Math.ceil((outDate - inDate) / (1000 * 60 * 60 * 24));
      const validDays = diff > 0 ? diff : 1;

      let tempSub = basePrice * validDays;
      if (orderData.minibarIncluded) tempSub *= 1.03;
      if (orderData.mealsIncluded) tempSub *= 1.03;

      const final = tempSub * (1 + comission);

      setDays(validDays);
      setSubTotal(Number(tempSub.toFixed(2)));
      setTotalPrice(Number(final.toFixed(2)));
    }
  }, [checkIn, checkOut, basePrice, orderData.minibarIncluded, orderData.mealsIncluded]);

  useEffect(() => {
    const fetchToken = async () => {
      try {
        const token = await getClientToken();
        setClientToken(token);
      } catch (error) {
        console.error("Error fetching Braintree client token:", error);
      }
    };
    fetchToken();
  }, []);

  useEffect(() => {
    if (!clientToken) return;
    dropin.create(
      {
        authorization: clientToken,
        container: dropInContainer.current,
        card: { cardholderName: { required: true } },
      },
      (err, instance) => {
        if (err) {
          console.error("Error creating Drop-In:", err);
          return;
        }
        setDropInInstance(instance);
      }
    );
  }, [clientToken]);

  const handleInputChange = (e) => {
    const { name, value, type, checked } = e.target;
    setOrderData((prev) => ({
      ...prev,
      [name]: type === "checkbox" ? checked : value,
    }));
  };

  const handleBookSecurely = async () => {
    const { firstName, lastName, email, phone } = orderData;
    if (!firstName || !lastName || !email || !phone) {
      toast({
        title: "Missing info",
        description: "Заповніть усі обовʼязкові поля.",
        status: "warning",
        duration: 4000,
        isClosable: true,
      });
      return;
    }
    if (!dropInInstance) {
      toast({
        title: "Payment not ready",
        description: "Спробуйте пізніше.",
        status: "error",
        duration: 4000,
        isClosable: true,
      });
      return;
    }

    try {
      setIsProcessing(true);
      const { nonce } = await dropInInstance.requestPaymentMethod();

      const createOrderRequest = {
        NonceFromClient: nonce,
        Email: orderData.email,
        FirstName: orderData.firstName,
        LastName: orderData.lastName,
        Number: orderData.phone,
        MinibarIncluded: orderData.minibarIncluded,
        MealsIncluded: orderData.mealsIncluded,
        CheckIn: checkIn,
        CheckOut: checkOut,
        CreatedAt: new Date().toISOString(),
      };

      const result = await postOrder(hotelId, roomNumber, createOrderRequest);
      if (result.isSuccess) {
        toast({
          title: "Success!",
          description: "Your order was successfully created!",
          status: "success",
          duration: 7000,
          isClosable: true,
        });
        navigate("/checkout/success", { state: createOrderRequest });
      }
    } catch (error) {
      console.error("Payment error:", error);
      toast({
        title: "Payment failed",
        description: "Щось пішло не так :(",
        status: "error",
        duration: 6000,
        isClosable: true,
      });
    } finally {
      setIsProcessing(false);
    }
  };

  return (
    <section>
      <div className="min-h-screen flex justify-center bg-white p-6">
        <div className="bg-white w-full max-w-4xl grid grid-cols-1 md:grid-cols-2 rounded-lg shadow-md overflow-hidden">
          <div
            className="relative bg-cover bg-center"
            style={{
              backgroundImage:
                'url("https://images.pexels.com/photos/2876787/pexels-photo-2876787.jpeg")',
              minHeight: "400px",
            }}
          >
            <div className="absolute inset-0 bg-indigo-900 bg-opacity-50 flex flex-col justify-end p-6">
              <h1 className="text-3xl text-white font-bold">{roomName}</h1>
              <p className="text-lg text-gray-200 font-semibold">
                {basePrice} USD / night
              </p>
              <hr className="my-2 border-gray-300" />
              <p className="text-gray-100 text-sm">
                {days} {days === 1 ? "night" : "nights"}: {checkIn} → {checkOut}
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
                      {basePrice} USD × {days} {days > 1 ? "nights" : "night"}
                    </td>
                    <td className="text-right py-2">
                      {(basePrice * days).toFixed(2)} USD
                    </td>
                  </tr>
                  <tr className="border-b">
                    <td className="py-2">
                      Extras (+3% each):
                      <br />
                      {orderData.minibarIncluded && <span>Minibar </span>}
                      {orderData.mealsIncluded && <span>Meals </span>}
                      {!orderData.minibarIncluded &&
                        !orderData.mealsIncluded && <span>None</span>}
                    </td>
                    <td className="text-right py-2" />
                  </tr>
                  <tr className="border-b">
                    <td className="py-2 font-semibold">Subtotal</td>
                    <td className="text-right py-2 font-semibold">
                      {subTotal.toFixed(2)} USD
                    </td>
                  </tr>
                  <tr className="border-b">
                    <td className="py-2">Tax (~5%)</td>
                    <td className="text-right py-2">
                      {(subTotal * comission).toFixed(2)} USD
                    </td>
                  </tr>
                  <tr className="border-b">
                    <td className="py-2 font-bold text-lg">Total</td>
                    <td className="text-right py-2 font-bold text-lg">
                      {totalPrice.toFixed(2)} USD
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>

            <div className="mb-6">
              <h3 className="text-md font-semibold text-gray-800 mb-2">
                Contact Info
              </h3>
              <div className="grid grid-cols-1 gap-4 mb-4">
                <div>
                  <label className="block font-semibold mb-1">First Name</label>
                  <input
                    type="text"
                    name="firstName"
                    className="w-full border border-gray-300 p-2 rounded"
                    value={orderData.firstName}
                    onChange={handleInputChange}
                  />
                </div>
                <div>
                  <label className="block font-semibold mb-1">Last Name</label>
                  <input
                    type="text"
                    name="lastName"
                    className="w-full border border-gray-300 p-2 rounded"
                    value={orderData.lastName}
                    onChange={handleInputChange}
                  />
                </div>
                <div>
                  <label className="block font-semibold mb-1">Email</label>
                  <input
                    type="email"
                    name="email"
                    className="w-full border border-gray-300 p-2 rounded"
                    value={orderData.email}
                    onChange={handleInputChange}
                  />
                </div>
                <div>
                  <label className="block font-semibold mb-1">
                    Phone Number
                  </label>
                  <input
                    type="tel"
                    name="phone"
                    className="w-full border border-gray-300 p-2 rounded"
                    value={orderData.phone}
                    onChange={handleInputChange}
                  />
                </div>
              </div>

              <div className="flex items-center mb-2">
                <input
                  type="checkbox"
                  id="minibar"
                  name="minibarIncluded"
                  className="mr-2"
                  checked={orderData.minibarIncluded}
                  onChange={handleInputChange}
                />
                <label htmlFor="minibar" className="font-semibold">
                  Minibar Included (+3%)
                </label>
              </div>
              <div className="flex items-center mb-4">
                <input
                  type="checkbox"
                  id="meals"
                  name="mealsIncluded"
                  className="mr-2"
                  checked={orderData.mealsIncluded}
                  onChange={handleInputChange}
                />
                <label htmlFor="meals" className="font-semibold">
                  Meals Included (+3%)
                </label>
              </div>

              <h3 className="text-md font-semibold text-gray-800 mb-2">
                Payment Information
              </h3>
              <div
                ref={dropInContainer}
                className="bg-gray-100 p-4 rounded-md"
                style={{ minHeight: "200px" }}
              />
            </div>

            <Button
              colorScheme="purple"
              isFullWidth
              size="lg"
              isLoading={isProcessing}
              loadingText="Processing"
              isDisabled={isProcessing}
              onClick={handleBookSecurely}
            >
              Confirm
            </Button>

            <p className="text-xs text-gray-500 text-center mt-2">
              Your credit card information is encrypted
            </p>
          </div>
        </div>
      </div>
    </section>
  );
}

export default Checkout;
