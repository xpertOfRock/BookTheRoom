namespace BookTheRoom.WebUI.Endpoints
{
    public static class EndpointsConfiguration
    {
        public static void ConfigureEndpoints(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapControllerRoute(
                name: "Hotels",
                pattern: "Hotels",
                defaults: new { controller = "Hotel", action = "Hotels" }
                );
            endpoints.MapControllerRoute(
                name: "Hotel",
                pattern: "Hotels/{id:int}",
                defaults: new { controller = "Hotel", action = "Hotel" }
                );
            endpoints.MapControllerRoute(
                name: "AddHotel",
                pattern: "/Hotels/Add",
                defaults: new { controller = "Hotel", action = "AddHotel" }
                );
            endpoints.MapControllerRoute(
                name: "EditHotel",
                pattern: "/Hotels/{id:int}/Edit",
                defaults: new { controller = "Hotel", action = "EditHotel" }
                );
            endpoints.MapControllerRoute(
                name: "DeleteHotel",
                pattern: "/Hotels/{id:int}/Delete",
                defaults: new { controller = "Hotel", action = "DeleteHotel" }
                );


            endpoints.MapControllerRoute(
                name: "Rooms",
                pattern: "/Hotels/{hotelId:int}/Rooms",
                defaults: new { controller = "Room", action = "Rooms" }
                );
            endpoints.MapControllerRoute(
                name: "Room",
                pattern: "/Hotels/{hotelId:int}/Rooms{number:int}",
                defaults: new { controller = "Room", action = "Room" }
                );
            endpoints.MapControllerRoute(
                name: "AddRoom",
                pattern: "/Hotels/{hotelId:int}/Rooms/Add",
                defaults: new { controller = "Room", action = "AddRoom" }
                );
            endpoints.MapControllerRoute(
                name: "EditRoom",
                pattern: "/Hotels/{hotelId:int}/Rooms/{number:int}/Edit",
                defaults: new { controller = "Room", action = "EditRoom" }
                );
            endpoints.MapControllerRoute(
                name: "DeleteRoom",
                pattern: "/Hotels/{hotelId:int}/Rooms/{number:int}/Delete",
                defaults: new { controller = "Room", action = "DeleteRoom" }
                );

            endpoints.MapControllerRoute(
                name: "Orders",
                pattern: "/Orders",
                defaults: new { controller = "Order", action = "Orders" }
                );
            endpoints.MapControllerRoute(
                name: "/UserOrders",
                pattern: "/Profile/{userName:string}/Orders",
                defaults: new { controller = "Order", action = "UserOrders" }
                );
            endpoints.MapControllerRoute(
                name: "UserOrder",
                pattern: "Profile/{userId:string}/Orders/{id:int}",
                defaults: new { controller = "Order", action = "Order" }
                );
            endpoints.MapControllerRoute(
                name: "Checkout",
                pattern: "/Checkout",
                defaults: new { controller = "Order", action = "Checkout" }
                );
            endpoints.MapControllerRoute(
                name: "EditOrder",
                pattern: "/Orders/{id:int}/Edit",
                defaults: new { controller = "Order", action = "EditHotel" }
                );
            endpoints.MapControllerRoute(
                name: "DeleteOrder",
                pattern: "/Orders/{id:int}/Delete",
                defaults: new { controller = "Order", action = "DeleteRoom" }
                );

            endpoints.MapControllerRoute(
                name: "Profile",
                pattern: "/Profile/{userName:string}",
                defaults: new { controller = "Account", action = "Profile" }
                );
            endpoints.MapControllerRoute(
                name: "EditProfile",
                pattern: "/Profile/{userName:string}/Edit",
                defaults: new { controller = "Account", action = "EditProfile" }
                );
            endpoints.MapControllerRoute(
                name: "Login",
                pattern: "/Login",
                defaults: new { controller = "Account", action = "Login" }
                );
            endpoints.MapControllerRoute(
                name: "Register",
                pattern: "/Register",
                defaults: new { controller = "Account", action = "Register" }
                );

        }
    }
}
