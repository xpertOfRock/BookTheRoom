using BookTheRoom.Domain.Entities;
using BookTheRoom.Domain.Enums;

namespace BookTheRoom.Infrastructure.Data
{
    public class Seed
    {

        public static void SeedData(ApplicationDbContext context)
        {
            if (!context.Hotels.Any())
            {
                var hotels = new List<Hotel>
                {
                    new Hotel {
                        Name = "Club Hotel Sera",
                        Description = "Club Hotel Sera offers a luxurious getaway on the beautiful shores of Antalya, Turkey. With its modern charm and stunning views of the Mediterranean, it's the perfect spot for a relaxing vacation.\r\n\r\nEnjoy spacious rooms, multiple pools, and a private beach. Indulge in delicious cuisine at the resort's restaurants, and unwind at the spa. With activities for all ages, from water sports to sightseeing, Club Hotel Sera has something for everyone. Come experience true relaxation and luxury at Club Hotel Sera.",
                        Rating = 5,
                        HasPool = true,
                        NumberOfRooms = 516,
                        PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273053474.jpg?k=c67575a2a759349062915e9094d4f1ee9a7c8894fe7149090376610a93144173&o=&hp=1",
                        ImagesURL = new List<string>
                        {
                            "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273053925.jpg?k=7e3611cfcd41b0b0997bf6ff3a9dd4aeab76da940e8be0847f7fd3f291518c7e&o=&hp=1",
                            "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273053939.jpg?k=c1e77549e5bb3fbfe7dcf840f8baff69257db1241fbc431065e6b5cb82936164&o=&hp=1",
                            "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273053930.jpg?k=a110edbd3c8bf250cad75792c81ca44a295a4bcc19e94fbc1f28e3cd1b3213c3&o=&hp=1",
                            "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273053948.jpg?k=39588ce530d0030ee1bdc85c8e37b19c4b64a39c643d88cbde313d8a43ff5675&o=&hp=1"
                        },
                        Address = new Address()
                        {
                            Country = "Turkey",
                            City = "Antalya",
                            StreetOrDistrict = "Lara Cd.",
                            Index = 27
                        },
                        Rooms = new List<Room>
                        {
                            new Room
                            {
                                Number = 110,
                                Description = "Comfortable apartments for one person. There is a mini bar which you can use in case you are ready to pay for it. Good view of the sea.",
                                PriceForRoom = 130,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273037548.webp?k=f3d7263b1f1231c175337a43f16b1f52a445e66b2c6216e42e02e82f84029d40&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273037819.webp?k=feab169c99f813af4451df91e3db60ecc82590706694c3c5cb6b8952eff37e22&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273037548.webp?k=f3d7263b1f1231c175337a43f16b1f52a445e66b2c6216e42e02e82f84029d40&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273037589.webp?k=7fb460144f7342875fcee961dba13e586d7124daf4c1717b82f22938e98e9f8b&o=",
                                },
                                RoomCategory = RoomCategory.OneBedApartments,
                                
                            },
                            new Room
                            {
                                Number = 112,
                                Description = "Comfortable apartments for one person. There is a mini bar which you can use in case you are ready to pay for it. Good view of the sea.",
                                PriceForRoom = 130,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273037548.webp?k=f3d7263b1f1231c175337a43f16b1f52a445e66b2c6216e42e02e82f84029d40&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273037819.webp?k=feab169c99f813af4451df91e3db60ecc82590706694c3c5cb6b8952eff37e22&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273037548.webp?k=f3d7263b1f1231c175337a43f16b1f52a445e66b2c6216e42e02e82f84029d40&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273037589.webp?k=7fb460144f7342875fcee961dba13e586d7124daf4c1717b82f22938e98e9f8b&o=",
                                },
                                RoomCategory = RoomCategory.OneBedApartments,
                                
                            },
                            new Room
                            {
                                Number = 113,
                                Description = "Perfect room for 2 person and child (optional). There is a mini bar which you can use in case you are ready to pay for it. Good view of the sea.",
                                PriceForRoom = 130,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/153350247.webp?k=cef802c42c2b5c845fa0b7097fea42672f2b8e908de94fa46607e281b897f07b&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/57780222.webp?k=233916ec5e6b40dc0a6c29172d80bf4665cbc1063f28074601a49e1c02bd1fd8&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/153350247.webp?k=cef802c42c2b5c845fa0b7097fea42672f2b8e908de94fa46607e281b897f07b&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273036265.webp?k=e9a27fbf1d7c06a63f0415abefc2bad86de0408a0d2a8ca0ebe54435b5940fe4&o="
                                },
                                RoomCategory = RoomCategory.TwoBedApartments,
                                
                            },
                            new Room
                            {
                                Number = 114,
                                Description = "Perfect room for 2 person and child (optional). There is a mini bar which you can use in case you are ready to pay for it. Good view of the sea.",
                                PriceForRoom = 130,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/153350247.webp?k=cef802c42c2b5c845fa0b7097fea42672f2b8e908de94fa46607e281b897f07b&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/57780222.webp?k=233916ec5e6b40dc0a6c29172d80bf4665cbc1063f28074601a49e1c02bd1fd8&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/153350247.webp?k=cef802c42c2b5c845fa0b7097fea42672f2b8e908de94fa46607e281b897f07b&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273036265.webp?k=e9a27fbf1d7c06a63f0415abefc2bad86de0408a0d2a8ca0ebe54435b5940fe4&o="
                                },
                                RoomCategory = RoomCategory.TwoBedApartments,
                                
                            },
                            new Room
                            {
                                Number = 115,
                                Description = "Perfect room for 2 person and child (optional). There is a mini bar which you can use in case you are ready to pay for it. Good view of the sea.",
                                PriceForRoom = 130,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/153350247.webp?k=cef802c42c2b5c845fa0b7097fea42672f2b8e908de94fa46607e281b897f07b&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/57780222.webp?k=233916ec5e6b40dc0a6c29172d80bf4665cbc1063f28074601a49e1c02bd1fd8&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/153350247.webp?k=cef802c42c2b5c845fa0b7097fea42672f2b8e908de94fa46607e281b897f07b&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273036265.webp?k=e9a27fbf1d7c06a63f0415abefc2bad86de0408a0d2a8ca0ebe54435b5940fe4&o="
                                },
                                RoomCategory = RoomCategory.TwoBedApartments,
                                
                            },
                            new Room
                            {
                                Number = 224,
                                Description = "Comfortable apartments for small company or family. Plenty of space will guarantee you comfortable living here. There is a mini bar which you can use in case you are ready to pay for it. Good view of the sea.",
                                PriceForRoom = 180,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273036265.webp?k=e9a27fbf1d7c06a63f0415abefc2bad86de0408a0d2a8ca0ebe54435b5940fe4&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/153351194.webp?k=32059d56b16bf662789949484d7db8e2e33ba0df1219f9ff17d15d5558b275b1&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max200/57780222.webp?k=233916ec5e6b40dc0a6c29172d80bf4665cbc1063f28074601a49e1c02bd1fd8&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273036265.webp?k=e9a27fbf1d7c06a63f0415abefc2bad86de0408a0d2a8ca0ebe54435b5940fe4&o=",
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                
                            },
                            new Room
                            {
                                Number = 225,
                                Description = "Comfortable apartments for small company or family. Plenty of space will guarantee you comfortable living here. There is a mini bar which you can use in case you are ready to pay for it. Good view of the sea.",
                                PriceForRoom = 180,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273036265.webp?k=e9a27fbf1d7c06a63f0415abefc2bad86de0408a0d2a8ca0ebe54435b5940fe4&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/153351194.webp?k=32059d56b16bf662789949484d7db8e2e33ba0df1219f9ff17d15d5558b275b1&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max200/57780222.webp?k=233916ec5e6b40dc0a6c29172d80bf4665cbc1063f28074601a49e1c02bd1fd8&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273036265.webp?k=e9a27fbf1d7c06a63f0415abefc2bad86de0408a0d2a8ca0ebe54435b5940fe4&o=",
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                
                            },
                            new Room
                            {
                                Number = 226,
                                Description = "Comfortable apartments for small company or family. Plenty of space will guarantee you comfortable living here. There is a mini bar which you can use in case you are ready to pay for it. Good view of the sea.",
                                PriceForRoom = 180,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273036265.webp?k=e9a27fbf1d7c06a63f0415abefc2bad86de0408a0d2a8ca0ebe54435b5940fe4&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/153351194.webp?k=32059d56b16bf662789949484d7db8e2e33ba0df1219f9ff17d15d5558b275b1&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max200/57780222.webp?k=233916ec5e6b40dc0a6c29172d80bf4665cbc1063f28074601a49e1c02bd1fd8&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273036265.webp?k=e9a27fbf1d7c06a63f0415abefc2bad86de0408a0d2a8ca0ebe54435b5940fe4&o=",
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                
                            },
                            new Room
                            {
                                Number = 227,
                                 Description = "Comfortable apartments for small company or family. Plenty of space will guarantee you comfortable living here. There is a mini bar which you can use in case you are ready to pay for it. Good view of the sea.",
                                PriceForRoom = 180,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273036265.webp?k=e9a27fbf1d7c06a63f0415abefc2bad86de0408a0d2a8ca0ebe54435b5940fe4&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/153351194.webp?k=32059d56b16bf662789949484d7db8e2e33ba0df1219f9ff17d15d5558b275b1&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max200/57780222.webp?k=233916ec5e6b40dc0a6c29172d80bf4665cbc1063f28074601a49e1c02bd1fd8&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273036265.webp?k=e9a27fbf1d7c06a63f0415abefc2bad86de0408a0d2a8ca0ebe54435b5940fe4&o=",
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                
                            },
                            new Room
                            {
                                Number = 228,
                                 Description = "Comfortable apartments for small company or family. Plenty of space will guarantee you comfortable living here. There is a mini bar which you can use in case you are ready to pay for it. Good view of the sea.",
                                PriceForRoom = 180,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273036265.webp?k=e9a27fbf1d7c06a63f0415abefc2bad86de0408a0d2a8ca0ebe54435b5940fe4&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/153351194.webp?k=32059d56b16bf662789949484d7db8e2e33ba0df1219f9ff17d15d5558b275b1&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max200/57780222.webp?k=233916ec5e6b40dc0a6c29172d80bf4665cbc1063f28074601a49e1c02bd1fd8&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273036265.webp?k=e9a27fbf1d7c06a63f0415abefc2bad86de0408a0d2a8ca0ebe54435b5940fe4&o=",
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,                            
                            }
                }
                    },
                    new Hotel {
                        Name = "The Plaza NY",
                        Description = "The Plaza, nestled on the corner of Fifth Avenue and Central Park South in New York City, is a renowned luxury hotel exuding timeless elegance. Since its establishment in 1907, it has been a haven for both travelers and celebrities, offering opulent accommodations, exceptional dining, and exquisite service. With its iconic Beaux-Arts architecture and prime location, The Plaza embodies the essence of New York City's sophistication and charm, promising an unforgettable experience for all who enter its doors.",
                        Rating = 5,
                        HasPool = true,
                        NumberOfRooms = 648,
                        PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718768.jpg?k=f097e8f56230cd4aa38cdbaa52af08254b915e2014f4bcc98b04d308f54efbdb&o=&hp=1",
                        ImagesURL = new List<string>
                        {
                            "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718760.jpg?k=608ceb5268219094ffb5f99c00dd1b869daf59485ca2ce071c49a9bd2feeba4f&o=&hp=1",
                            "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718798.jpg?k=49069f23102966baa7e686393e997fb54113d486ee4e51a5327a668115d0892e&o=&hp=1",
                            "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718798.jpg?k=49069f23102966baa7e686393e997fb54113d486ee4e51a5327a668115d0892e&o=&hp=1",
                            "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718788.jpg?k=e9551c2ed6fc2c1b29b7ae11cb4dda2dee789e21dd30aa24f59ad7c62d66b899&o=&hp=1"
                        },
                        Address = new Address()
                        {
                            Country = "U.S.A.",
                            City = "New York City",
                            StreetOrDistrict = "5th Ave",
                            Index = 768
                        },
                        Rooms = new List<Room>
                        {
                            new Room
                            {
                                Number = 401,
                                Description = "Room 'Plaza' is good variant with great price-quality ratio. There is a mini bar which you can use in case you are ready to pay for it. Good view of NY city and Central Park.",
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296573.webp?k=85c96849561c86c59990b2fbfddf67237f22ee7f1c5d66a04b0878eaa147a3c0&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296676.webp?k=2c2289664a72735be983ff98d8c0013a9c22f010e90e57fb1dc7187486eb84c2&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718845.webp?k=c8d5b056d699bf3245793d7777a638b5bbc3d82ea1e710d9f3543bef74e9b679&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296270.webp?k=09216340a23473d3a64d4b597dd7f9225c5018f19914aeced2eb91e2f26bfd37&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296462.webp?k=204818458c605d24ab74b219bb3fae9a65ec326782ff00b43635bc011eda0ae0&o="
                                },
                                RoomCategory = RoomCategory.OneBedApartments
                                
                            },
                            new Room
                            {
                                Number = 402,
                                Description = "Room 'Plaza' is good variant with great price-quality ratio. There is a mini bar which you can use in case you are ready to pay for it. Good view of NY city and Central Park.",
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296573.webp?k=85c96849561c86c59990b2fbfddf67237f22ee7f1c5d66a04b0878eaa147a3c0&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296676.webp?k=2c2289664a72735be983ff98d8c0013a9c22f010e90e57fb1dc7187486eb84c2&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718845.webp?k=c8d5b056d699bf3245793d7777a638b5bbc3d82ea1e710d9f3543bef74e9b679&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296270.webp?k=09216340a23473d3a64d4b597dd7f9225c5018f19914aeced2eb91e2f26bfd37&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296462.webp?k=204818458c605d24ab74b219bb3fae9a65ec326782ff00b43635bc011eda0ae0&o="
                                },
                                RoomCategory = RoomCategory.OneBedApartments
                                
                            },
                            new Room
                            {
                                Number = 403,
                                Description = "Room 'Plaza' is good variant with great price-quality ratio. There is a mini bar which you can use in case you are ready to pay for it. Good view of NY city and Central Park.",
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296573.webp?k=85c96849561c86c59990b2fbfddf67237f22ee7f1c5d66a04b0878eaa147a3c0&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296676.webp?k=2c2289664a72735be983ff98d8c0013a9c22f010e90e57fb1dc7187486eb84c2&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718845.webp?k=c8d5b056d699bf3245793d7777a638b5bbc3d82ea1e710d9f3543bef74e9b679&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296270.webp?k=09216340a23473d3a64d4b597dd7f9225c5018f19914aeced2eb91e2f26bfd37&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296462.webp?k=204818458c605d24ab74b219bb3fae9a65ec326782ff00b43635bc011eda0ae0&o="
                                },
                                RoomCategory = RoomCategory.OneBedApartments,                               
                            },
                            new Room
                            {
                                Number = 404,
                                Description = "Room 'Plaza' is good variant with great price-quality ratio. There is a mini bar which you can use in case you are ready to pay for it. Good view of NY city and Central Park.",
                                PriceForRoom = 300,
                                IsFree = false,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296573.webp?k=85c96849561c86c59990b2fbfddf67237f22ee7f1c5d66a04b0878eaa147a3c0&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296676.webp?k=2c2289664a72735be983ff98d8c0013a9c22f010e90e57fb1dc7187486eb84c2&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718845.webp?k=c8d5b056d699bf3245793d7777a638b5bbc3d82ea1e710d9f3543bef74e9b679&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296270.webp?k=09216340a23473d3a64d4b597dd7f9225c5018f19914aeced2eb91e2f26bfd37&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296462.webp?k=204818458c605d24ab74b219bb3fae9a65ec326782ff00b43635bc011eda0ae0&o="
                                },
                                RoomCategory = RoomCategory.OneBedApartments,                                
                            },
                            new Room
                            {
                                Number = 405,
                                Description = "Room 'Plaza' is good variant with great price-quality ratio. There is a mini bar which you can use in case you are ready to pay for it. Good view of NY city and Central Park.",
                                PriceForRoom = 300,
                                IsFree = false,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296573.webp?k=85c96849561c86c59990b2fbfddf67237f22ee7f1c5d66a04b0878eaa147a3c0&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296676.webp?k=2c2289664a72735be983ff98d8c0013a9c22f010e90e57fb1dc7187486eb84c2&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718845.webp?k=c8d5b056d699bf3245793d7777a638b5bbc3d82ea1e710d9f3543bef74e9b679&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296270.webp?k=09216340a23473d3a64d4b597dd7f9225c5018f19914aeced2eb91e2f26bfd37&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296462.webp?k=204818458c605d24ab74b219bb3fae9a65ec326782ff00b43635bc011eda0ae0&o="
                                },
                                RoomCategory = RoomCategory.OneBedApartments,
                                
                            },
                            new Room
                            {
                                Number = 406,
                                Description = "Room 'Plaza' is good variant with great price-quality ratio. There is a mini bar which you can use in case you are ready to pay for it. Good view of NY city and Central Park.",
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296573.webp?k=85c96849561c86c59990b2fbfddf67237f22ee7f1c5d66a04b0878eaa147a3c0&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296676.webp?k=2c2289664a72735be983ff98d8c0013a9c22f010e90e57fb1dc7187486eb84c2&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718845.webp?k=c8d5b056d699bf3245793d7777a638b5bbc3d82ea1e710d9f3543bef74e9b679&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296270.webp?k=09216340a23473d3a64d4b597dd7f9225c5018f19914aeced2eb91e2f26bfd37&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/32296462.webp?k=204818458c605d24ab74b219bb3fae9a65ec326782ff00b43635bc011eda0ae0&o="
                                },
                                RoomCategory = RoomCategory.OneBedApartments,
                            },
                            new Room
                            {
                                Number = 718,
                                Description = "Room 'Fitzgerald King Suite' is a greate example of comfort & style. There is a mini bar which you can use in case you are ready to pay for it. Good view of NY city and Central Park.",
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718973.webp?k=7f4fe1d20f1088a0ba9f051d2c122b0e2341f0ff87fcbabf52202ba12dcc5695&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718890.webp?k=3c11d898884c37cbdf975309dbe139d5183607a5dc2e89280fe82f46ea100648&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719001.webp?k=fcc68f1964806050277cadb91aade628d7af645af767a8c6ece35a1bc23434a3&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719019.webp?k=0f5c2dd130f9edce3f1983a0bd724a211b42f09f3f5cf401b73d557a3cdea1b3&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719012.webp?k=8a496390cc6d3b72f41de6bca2343a6ea0ef641cab42e9f1da0a6a306d550a8d&o="
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                
                            },
                            new Room
                            {
                                Number = 719,
                                Description = "Room 'Fitzgerald King Suite' is a greate example of comfort & style. There is a mini bar which you can use in case you are ready to pay for it. Good view of NY city and Central Park.",
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718973.webp?k=7f4fe1d20f1088a0ba9f051d2c122b0e2341f0ff87fcbabf52202ba12dcc5695&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718890.webp?k=3c11d898884c37cbdf975309dbe139d5183607a5dc2e89280fe82f46ea100648&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719001.webp?k=fcc68f1964806050277cadb91aade628d7af645af767a8c6ece35a1bc23434a3&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719019.webp?k=0f5c2dd130f9edce3f1983a0bd724a211b42f09f3f5cf401b73d557a3cdea1b3&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719012.webp?k=8a496390cc6d3b72f41de6bca2343a6ea0ef641cab42e9f1da0a6a306d550a8d&o="
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                
                            },
                            new Room
                            {
                                Number = 720,
                                Description = "Room 'Fitzgerald King Suite' is a greate example of comfort & style. There is a mini bar which you can use in case you are ready to pay for it. Good view of NY city and Central Park.",
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718973.webp?k=7f4fe1d20f1088a0ba9f051d2c122b0e2341f0ff87fcbabf52202ba12dcc5695&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718890.webp?k=3c11d898884c37cbdf975309dbe139d5183607a5dc2e89280fe82f46ea100648&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719001.webp?k=fcc68f1964806050277cadb91aade628d7af645af767a8c6ece35a1bc23434a3&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719019.webp?k=0f5c2dd130f9edce3f1983a0bd724a211b42f09f3f5cf401b73d557a3cdea1b3&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719012.webp?k=8a496390cc6d3b72f41de6bca2343a6ea0ef641cab42e9f1da0a6a306d550a8d&o="
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                               
                            },
                            new Room
                            {
                                Number = 721,
                                Description = "Room 'Fitzgerald King Suite' is a greate example of comfort & style. There is a mini bar which you can use in case you are ready to pay for it. Good view of NY city and Central Park.",
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718973.webp?k=7f4fe1d20f1088a0ba9f051d2c122b0e2341f0ff87fcbabf52202ba12dcc5695&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718890.webp?k=3c11d898884c37cbdf975309dbe139d5183607a5dc2e89280fe82f46ea100648&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719001.webp?k=fcc68f1964806050277cadb91aade628d7af645af767a8c6ece35a1bc23434a3&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719019.webp?k=0f5c2dd130f9edce3f1983a0bd724a211b42f09f3f5cf401b73d557a3cdea1b3&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719012.webp?k=8a496390cc6d3b72f41de6bca2343a6ea0ef641cab42e9f1da0a6a306d550a8d&o="
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                
                            },
                            new Room
                            {
                                Number = 722,
                                Description = "Room 'Fitzgerald King Suite' is a greate example of comfort & style. There is a mini bar which you can use in case you are ready to pay for it. Good view of NY city and Central Park.",
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718973.webp?k=7f4fe1d20f1088a0ba9f051d2c122b0e2341f0ff87fcbabf52202ba12dcc5695&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718890.webp?k=3c11d898884c37cbdf975309dbe139d5183607a5dc2e89280fe82f46ea100648&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719001.webp?k=fcc68f1964806050277cadb91aade628d7af645af767a8c6ece35a1bc23434a3&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719019.webp?k=0f5c2dd130f9edce3f1983a0bd724a211b42f09f3f5cf401b73d557a3cdea1b3&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719012.webp?k=8a496390cc6d3b72f41de6bca2343a6ea0ef641cab42e9f1da0a6a306d550a8d&o="
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                               
                            },
                            new Room
                            {
                                Number = 723,
                                PriceForRoom = 300,
                                Description = "Room 'Fitzgerald King Suite' is a greate example of comfort & style. There is a mini bar which you can use in case you are ready to pay for it. Good view of NY city and Central Park.",
                                IsFree = true,
                                PreviewURL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718973.webp?k=7f4fe1d20f1088a0ba9f051d2c122b0e2341f0ff87fcbabf52202ba12dcc5695&o=",
                                ImagesURL = new List<string>
                                {
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718890.webp?k=3c11d898884c37cbdf975309dbe139d5183607a5dc2e89280fe82f46ea100648&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719001.webp?k=fcc68f1964806050277cadb91aade628d7af645af767a8c6ece35a1bc23434a3&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719019.webp?k=0f5c2dd130f9edce3f1983a0bd724a211b42f09f3f5cf401b73d557a3cdea1b3&o=",
                                    "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496719012.webp?k=8a496390cc6d3b72f41de6bca2343a6ea0ef641cab42e9f1da0a6a306d550a8d&o="
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                
                            }
                        }
                        
                    }
                };             
                context.Hotels.AddRange(hotels);
                context.SaveChanges();
            }           
        }        
    }
    
}