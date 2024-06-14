function roundUp(number) {
    return Math.ceil(number);
}

function calculateDaysDifference(checkInDate, checkOutDate) {
    const oneDay = 24 * 60 * 60 * 1000; 
    const startDate = new Date(checkInDate);
    const endDate = new Date(checkOutDate);

    const differenceInDays = Math.abs((endDate - startDate) / oneDay);
    return roundUp(differenceInDays); 
}
function updateTotalPrice() {
    const checkInDate = document.getElementById('CheckIn').value;
    const checkOutDate = document.getElementById('CheckOut').value;
    const pricePerNight = parseFloat(document.getElementById('pricePerNight').innerText);

    const daysDifference = calculateDaysDifference(checkInDate, checkOutDate);
    const totalPrice = daysDifference * pricePerNight;

    document.getElementById('overallValue').innerText = totalPrice.toFixed(2);
}

document.getElementById('CheckIn').addEventListener('change', updateTotalPrice);
document.getElementById('CheckOut').addEventListener('change', updateTotalPrice);

updateTotalPrice();