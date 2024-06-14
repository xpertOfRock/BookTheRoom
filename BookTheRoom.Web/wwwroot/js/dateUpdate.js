var checkinInput = document.getElementById('CheckIn');
var checkoutInput = document.getElementById('CheckOut');

var checkin = new Date(checkinInput.value);
var checkout = new Date(checkoutInput.value);
var nextDay = new Date(checkin);

checkinInput.min = nextDay.toISOString().slice(0, 16);
nextDay.setDate(nextDay.getDate() + 1);
checkoutInput.min = nextDay.toISOString().slice(0, 16);

function updateMinCheckOut() {
    var checkin = new Date(checkinInput.value);
    var checkout = new Date(checkoutInput.value);
    var nextDay = new Date(checkin);

    if (checkin > checkout) {
        nextDay.setDate(nextDay.getDate() + 1);
        checkoutInput.value = nextDay.toISOString().slice(0, 16);
        checkout = nextDay;
    }
    nextDay.setDate(nextDay.getDate() + 1);
    checkoutInput.min = nextDay.toISOString().slice(0, 16);
}

checkinInput.addEventListener('input', function () {
    updateMinCheckOut();
});
checkoutInput.addEventListener('input', updateMinCheckOut);