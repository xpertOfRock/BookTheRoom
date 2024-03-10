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
                    new Hotel() {
                        Name = "Club Hotel Sera",
                        Description = "Club Hotel Sera offers a luxurious getaway on the beautiful shores of Antalya, Turkey. With its modern charm and stunning views of the Mediterranean, it's the perfect spot for a relaxing vacation.\r\n\r\nEnjoy spacious rooms, multiple pools, and a private beach. Indulge in delicious cuisine at the resort's restaurants, and unwind at the spa. With activities for all ages, from water sports to sightseeing, Club Hotel Sera has something for everyone. Come experience true relaxation and luxury at Club Hotel Sera.",
                        Rating = 5,
                        HasPool = true,
                        PreviewImage = new HotelImage
                        {
                            URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273053474.jpg?k=c67575a2a759349062915e9094d4f1ee9a7c8894fe7149090376610a93144173&o=&hp=1"
                        },
                        HotelImages = new List<HotelImage>(5)
                        {
                            new HotelImage
                            {
                                URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273053925.jpg?k=7e3611cfcd41b0b0997bf6ff3a9dd4aeab76da940e8be0847f7fd3f291518c7e&o=&hp=1"
                            },
                            new HotelImage
                            {
                                URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273053905.jpg?k=f6cc95e79f98acef62b56b2109289bef685cdc2f3f1526f1b52d90d93adea877&o=&hp=1"
                            },
                            new HotelImage
                            {
                                URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273053936.jpg?k=57312656b362760e086152f16c057273b891d666b446f9fb6e321eac44480f1e&o=&hp=1"
                            },
                            new HotelImage
                            {
                                URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273053939.jpg?k=c1e77549e5bb3fbfe7dcf840f8baff69257db1241fbc431065e6b5cb82936164&o=&hp=1"
                            },
                            new HotelImage
                            {
                                URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273053934.jpg?k=c003e54b0f92f8196e9b75c62a27f24c3fd43a4652709a0a99f9cb8afeebb1b7&o=&hp=1"
                            }
                        },
                        Address = new Address()
                        {
                            Country = "Turkey",
                            City = "Antalya",
                            Street = "Lara Cd.",
                            Building = 27
                        },
                        Rooms = new List<Room>
                        {
                            new Room
                            {
                                Number = 110,
                                PriceForRoom = 130,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.OneBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051855.jpg?k=8b673ef4a1ec79ff3280a10c1eb554bc2ddda67fc31de491bb0f66eff0b6271b&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051836.jpg?k=3d72748dcb6083f8e789e013035a38ed345002dc6c266d4e004a6adeac12d6f1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273047230.jpg?k=a3d7f7b526b2d30b14806cfe1ce75c0b0841efb916a86d312054ec6dde3dd774&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041425.jpg?k=c20a7a14502cb58e500ab1a23b998123187004c654b1ee83d02678aadbd3a965&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273047109.jpg?k=1687bb360e2749e825d68cb9a066d1aeb7fedd47a2b5d58fab4a8d39e320700e&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 112,
                                PriceForRoom = 130,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.OneBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051855.jpg?k=8b673ef4a1ec79ff3280a10c1eb554bc2ddda67fc31de491bb0f66eff0b6271b&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051836.jpg?k=3d72748dcb6083f8e789e013035a38ed345002dc6c266d4e004a6adeac12d6f1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273047230.jpg?k=a3d7f7b526b2d30b14806cfe1ce75c0b0841efb916a86d312054ec6dde3dd774&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041425.jpg?k=c20a7a14502cb58e500ab1a23b998123187004c654b1ee83d02678aadbd3a965&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273047109.jpg?k=1687bb360e2749e825d68cb9a066d1aeb7fedd47a2b5d58fab4a8d39e320700e&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 113,
                                PriceForRoom = 130,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.TwoBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051855.jpg?k=8b673ef4a1ec79ff3280a10c1eb554bc2ddda67fc31de491bb0f66eff0b6271b&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051836.jpg?k=3d72748dcb6083f8e789e013035a38ed345002dc6c266d4e004a6adeac12d6f1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273047230.jpg?k=a3d7f7b526b2d30b14806cfe1ce75c0b0841efb916a86d312054ec6dde3dd774&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041425.jpg?k=c20a7a14502cb58e500ab1a23b998123187004c654b1ee83d02678aadbd3a965&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273047109.jpg?k=1687bb360e2749e825d68cb9a066d1aeb7fedd47a2b5d58fab4a8d39e320700e&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 114,
                                PriceForRoom = 130,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.TwoBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051855.jpg?k=8b673ef4a1ec79ff3280a10c1eb554bc2ddda67fc31de491bb0f66eff0b6271b&o=&hp=1"
                                    },
                                    new RoomImage
                                    {  
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051836.jpg?k=3d72748dcb6083f8e789e013035a38ed345002dc6c266d4e004a6adeac12d6f1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273047230.jpg?k=a3d7f7b526b2d30b14806cfe1ce75c0b0841efb916a86d312054ec6dde3dd774&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041425.jpg?k=c20a7a14502cb58e500ab1a23b998123187004c654b1ee83d02678aadbd3a965&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273047109.jpg?k=1687bb360e2749e825d68cb9a066d1aeb7fedd47a2b5d58fab4a8d39e320700e&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 115,
                                PriceForRoom = 130,
                                IsFree = false,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.TwoBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051855.jpg?k=8b673ef4a1ec79ff3280a10c1eb554bc2ddda67fc31de491bb0f66eff0b6271b&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051836.jpg?k=3d72748dcb6083f8e789e013035a38ed345002dc6c266d4e004a6adeac12d6f1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273047230.jpg?k=a3d7f7b526b2d30b14806cfe1ce75c0b0841efb916a86d312054ec6dde3dd774&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041425.jpg?k=c20a7a14502cb58e500ab1a23b998123187004c654b1ee83d02678aadbd3a965&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273047109.jpg?k=1687bb360e2749e825d68cb9a066d1aeb7fedd47a2b5d58fab4a8d39e320700e&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 224,
                                PriceForRoom = 180,
                                IsFree = false,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041338.jpg?k=89803b102a20230adc5d8a9b7f6494f4e1616f0558ab798c84a04d1dce82cee4&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041334.jpg?k=88d49028106b971a0cfd6d81c244afdcb6cf091023d71a70601ccaced43f8ab8&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041308.jpg?k=96dbe78178aab577a342d45fcf307d50f4239b27ff62af23a760437b4c34f480&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273040602.jpg?k=af464a1490cbef41f028d4f476332f8b4fbf8ef3d74f09438015c193460bd82c&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273040276.jpg?k=914f293892900d94b6181bacd38a6f0e52535c00934a1c043ab89a70ad786a86&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 225,
                                PriceForRoom = 180,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041338.jpg?k=89803b102a20230adc5d8a9b7f6494f4e1616f0558ab798c84a04d1dce82cee4&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041334.jpg?k=88d49028106b971a0cfd6d81c244afdcb6cf091023d71a70601ccaced43f8ab8&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041308.jpg?k=96dbe78178aab577a342d45fcf307d50f4239b27ff62af23a760437b4c34f480&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273040602.jpg?k=af464a1490cbef41f028d4f476332f8b4fbf8ef3d74f09438015c193460bd82c&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273040276.jpg?k=914f293892900d94b6181bacd38a6f0e52535c00934a1c043ab89a70ad786a86&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 226,
                                PriceForRoom = 180,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041338.jpg?k=89803b102a20230adc5d8a9b7f6494f4e1616f0558ab798c84a04d1dce82cee4&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041334.jpg?k=88d49028106b971a0cfd6d81c244afdcb6cf091023d71a70601ccaced43f8ab8&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041308.jpg?k=96dbe78178aab577a342d45fcf307d50f4239b27ff62af23a760437b4c34f480&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273040602.jpg?k=af464a1490cbef41f028d4f476332f8b4fbf8ef3d74f09438015c193460bd82c&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273040276.jpg?k=914f293892900d94b6181bacd38a6f0e52535c00934a1c043ab89a70ad786a86&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 227,
                                PriceForRoom = 180,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041338.jpg?k=89803b102a20230adc5d8a9b7f6494f4e1616f0558ab798c84a04d1dce82cee4&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041334.jpg?k=88d49028106b971a0cfd6d81c244afdcb6cf091023d71a70601ccaced43f8ab8&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041308.jpg?k=96dbe78178aab577a342d45fcf307d50f4239b27ff62af23a760437b4c34f480&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273040602.jpg?k=af464a1490cbef41f028d4f476332f8b4fbf8ef3d74f09438015c193460bd82c&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273040276.jpg?k=914f293892900d94b6181bacd38a6f0e52535c00934a1c043ab89a70ad786a86&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 228,
                                PriceForRoom = 180,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041338.jpg?k=89803b102a20230adc5d8a9b7f6494f4e1616f0558ab798c84a04d1dce82cee4&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041334.jpg?k=88d49028106b971a0cfd6d81c244afdcb6cf091023d71a70601ccaced43f8ab8&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273041308.jpg?k=96dbe78178aab577a342d45fcf307d50f4239b27ff62af23a760437b4c34f480&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273040602.jpg?k=af464a1490cbef41f028d4f476332f8b4fbf8ef3d74f09438015c193460bd82c&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273040276.jpg?k=914f293892900d94b6181bacd38a6f0e52535c00934a1c043ab89a70ad786a86&o=&hp=1"
                                    }
                                }
                            }
                }
                    },
                    new Hotel {
                        Name = "The Plaza NY",
                        Description = "The Plaza, nestled on the corner of Fifth Avenue and Central Park South in New York City, is a renowned luxury hotel exuding timeless elegance. Since its establishment in 1907, it has been a haven for both travelers and celebrities, offering opulent accommodations, exceptional dining, and exquisite service. With its iconic Beaux-Arts architecture and prime location, The Plaza embodies the essence of New York City's sophistication and charm, promising an unforgettable experience for all who enter its doors.",
                        Rating = 5,
                        HasPool = true,
                        PreviewImage = new HotelImage
                        {
                            URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718768.jpg?k=f097e8f56230cd4aa38cdbaa52af08254b915e2014f4bcc98b04d308f54efbdb&o=&hp=1"
                        },
                        HotelImages = new List<HotelImage>(5)
                        {
                            new HotelImage
                            {
                                URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718760.jpg?k=608ceb5268219094ffb5f99c00dd1b869daf59485ca2ce071c49a9bd2feeba4f&o=&hp=1"
                            },
                            new HotelImage
                            {
                                URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718780.jpg?k=5bd33fc0a2eba7526b562202545c8f5620a531d582adfcb9606a62d5b5fbc853&o=&hp=1"
                            },
                            new HotelImage
                            {
                                URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718798.jpg?k=49069f23102966baa7e686393e997fb54113d486ee4e51a5327a668115d0892e&o=&hp=1"
                            },
                            new HotelImage
                            {
                                URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718788.jpg?k=e9551c2ed6fc2c1b29b7ae11cb4dda2dee789e21dd30aa24f59ad7c62d66b899&o=&hp=1"
                            },
                            new HotelImage
                            {
                                URL = "https://www.fairmont.com/assets/0/104/3225/3230/4116/4124/7bf52556-ee0f-4a55-83ce-9dc6417b622d.jpg"
                            }
                        },
                        Address = new Address()
                        {
                            Country = "U.S.A.",
                            City = "New York City",
                            Street = "5th Ave",
                            Building = 768
                        },
                        Rooms = new List<Room>
                        {
                            new Room
                            {
                                Number = 401,
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.OneBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718805.jpg?k=8014b74c2200b536138999d929b267528d34f3323cfa9fbe3f3d2f4ae9bd82d3&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718813.jpg?k=57c356977fb907698c2fc89c3f5d72cf73c93f2855bfcd7a79fa05f8807a8750&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718821.jpg?k=a1c2bceabab5466f7111bf5971cb26e06b81fdee3a0c4528fd19827204ac98a1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718828.jpg?k=8843809a06b9b068652577efa337b1ef4f3b1c294d0790a3ce2e8604d94d3218&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718837.jpg?k=44f8279b85e085abfe3698de3ec30fd9a3f6d4d6c430f00d7cdb7a560bb284c4&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 402,
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.OneBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718805.jpg?k=8014b74c2200b536138999d929b267528d34f3323cfa9fbe3f3d2f4ae9bd82d3&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718813.jpg?k=57c356977fb907698c2fc89c3f5d72cf73c93f2855bfcd7a79fa05f8807a8750&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718821.jpg?k=a1c2bceabab5466f7111bf5971cb26e06b81fdee3a0c4528fd19827204ac98a1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718828.jpg?k=8843809a06b9b068652577efa337b1ef4f3b1c294d0790a3ce2e8604d94d3218&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718837.jpg?k=44f8279b85e085abfe3698de3ec30fd9a3f6d4d6c430f00d7cdb7a560bb284c4&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 403,
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.OneBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718805.jpg?k=8014b74c2200b536138999d929b267528d34f3323cfa9fbe3f3d2f4ae9bd82d3&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718813.jpg?k=57c356977fb907698c2fc89c3f5d72cf73c93f2855bfcd7a79fa05f8807a8750&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718821.jpg?k=a1c2bceabab5466f7111bf5971cb26e06b81fdee3a0c4528fd19827204ac98a1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718828.jpg?k=8843809a06b9b068652577efa337b1ef4f3b1c294d0790a3ce2e8604d94d3218&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718837.jpg?k=44f8279b85e085abfe3698de3ec30fd9a3f6d4d6c430f00d7cdb7a560bb284c4&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 404,
                                PriceForRoom = 300,
                                IsFree = false,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.OneBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718805.jpg?k=8014b74c2200b536138999d929b267528d34f3323cfa9fbe3f3d2f4ae9bd82d3&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718813.jpg?k=57c356977fb907698c2fc89c3f5d72cf73c93f2855bfcd7a79fa05f8807a8750&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718821.jpg?k=a1c2bceabab5466f7111bf5971cb26e06b81fdee3a0c4528fd19827204ac98a1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718828.jpg?k=8843809a06b9b068652577efa337b1ef4f3b1c294d0790a3ce2e8604d94d3218&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718837.jpg?k=44f8279b85e085abfe3698de3ec30fd9a3f6d4d6c430f00d7cdb7a560bb284c4&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 405,
                                PriceForRoom = 300,
                                IsFree = false,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.OneBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718805.jpg?k=8014b74c2200b536138999d929b267528d34f3323cfa9fbe3f3d2f4ae9bd82d3&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718813.jpg?k=57c356977fb907698c2fc89c3f5d72cf73c93f2855bfcd7a79fa05f8807a8750&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718821.jpg?k=a1c2bceabab5466f7111bf5971cb26e06b81fdee3a0c4528fd19827204ac98a1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718828.jpg?k=8843809a06b9b068652577efa337b1ef4f3b1c294d0790a3ce2e8604d94d3218&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718837.jpg?k=44f8279b85e085abfe3698de3ec30fd9a3f6d4d6c430f00d7cdb7a560bb284c4&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 406,
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.OneBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718805.jpg?k=8014b74c2200b536138999d929b267528d34f3323cfa9fbe3f3d2f4ae9bd82d3&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718813.jpg?k=57c356977fb907698c2fc89c3f5d72cf73c93f2855bfcd7a79fa05f8807a8750&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718821.jpg?k=a1c2bceabab5466f7111bf5971cb26e06b81fdee3a0c4528fd19827204ac98a1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718828.jpg?k=8843809a06b9b068652577efa337b1ef4f3b1c294d0790a3ce2e8604d94d3218&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718837.jpg?k=44f8279b85e085abfe3698de3ec30fd9a3f6d4d6c430f00d7cdb7a560bb284c4&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 718,
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718877.jpg?k=4b35288685c689ba9d35420fd2411e70fe555991a9c4f6fc914125b79e58cb2a&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718906.jpg?k=8d236195ba86de4e1d4af08e646342cb467e5e3ac9ef6afe1e683f6c15513ec0&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718912.jpg?k=d86c36963196d6be7f12c39a2ea3756daf4f752f676f8727b24fc0297e2d7a1e&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718903.jpg?k=811a4eff5756fe02615aabaf70e436b5d64c0032a0ea98c15f9e674b404e18c1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718919.jpg?k=a8d822a2e983a07bbc23cb63ec45c65846f939fec12fc0bd8cb2c500149ddf20&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 719,
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718877.jpg?k=4b35288685c689ba9d35420fd2411e70fe555991a9c4f6fc914125b79e58cb2a&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718906.jpg?k=8d236195ba86de4e1d4af08e646342cb467e5e3ac9ef6afe1e683f6c15513ec0&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718912.jpg?k=d86c36963196d6be7f12c39a2ea3756daf4f752f676f8727b24fc0297e2d7a1e&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718903.jpg?k=811a4eff5756fe02615aabaf70e436b5d64c0032a0ea98c15f9e674b404e18c1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718919.jpg?k=a8d822a2e983a07bbc23cb63ec45c65846f939fec12fc0bd8cb2c500149ddf20&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 720,
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718877.jpg?k=4b35288685c689ba9d35420fd2411e70fe555991a9c4f6fc914125b79e58cb2a&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718906.jpg?k=8d236195ba86de4e1d4af08e646342cb467e5e3ac9ef6afe1e683f6c15513ec0&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718912.jpg?k=d86c36963196d6be7f12c39a2ea3756daf4f752f676f8727b24fc0297e2d7a1e&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718903.jpg?k=811a4eff5756fe02615aabaf70e436b5d64c0032a0ea98c15f9e674b404e18c1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718919.jpg?k=a8d822a2e983a07bbc23cb63ec45c65846f939fec12fc0bd8cb2c500149ddf20&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 721,
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718877.jpg?k=4b35288685c689ba9d35420fd2411e70fe555991a9c4f6fc914125b79e58cb2a&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718906.jpg?k=8d236195ba86de4e1d4af08e646342cb467e5e3ac9ef6afe1e683f6c15513ec0&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718912.jpg?k=d86c36963196d6be7f12c39a2ea3756daf4f752f676f8727b24fc0297e2d7a1e&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718903.jpg?k=811a4eff5756fe02615aabaf70e436b5d64c0032a0ea98c15f9e674b404e18c1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718919.jpg?k=a8d822a2e983a07bbc23cb63ec45c65846f939fec12fc0bd8cb2c500149ddf20&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                Number = 722,
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718877.jpg?k=4b35288685c689ba9d35420fd2411e70fe555991a9c4f6fc914125b79e58cb2a&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718906.jpg?k=8d236195ba86de4e1d4af08e646342cb467e5e3ac9ef6afe1e683f6c15513ec0&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718912.jpg?k=d86c36963196d6be7f12c39a2ea3756daf4f752f676f8727b24fc0297e2d7a1e&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718903.jpg?k=811a4eff5756fe02615aabaf70e436b5d64c0032a0ea98c15f9e674b404e18c1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {             
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718919.jpg?k=a8d822a2e983a07bbc23cb63ec45c65846f939fec12fc0bd8cb2c500149ddf20&o=&hp=1"
                                    }
                                }
                            },
                            new Room
                            {
                                PriceForRoom = 300,
                                IsFree = true,
                                PreviewImage = new RoomImage()
                                {
                                    URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/273051787.jpg?k=d47a41694c30dd5e179b77687e5a156abf863b1567dc051cadfa78f14e945064&o=&hp=1"
                                },
                                RoomCategory = RoomCategory.ThreeBedApartments,
                                RoomImages = new List<RoomImage>(5)
                                {
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718877.jpg?k=4b35288685c689ba9d35420fd2411e70fe555991a9c4f6fc914125b79e58cb2a&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718906.jpg?k=8d236195ba86de4e1d4af08e646342cb467e5e3ac9ef6afe1e683f6c15513ec0&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718912.jpg?k=d86c36963196d6be7f12c39a2ea3756daf4f752f676f8727b24fc0297e2d7a1e&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718903.jpg?k=811a4eff5756fe02615aabaf70e436b5d64c0032a0ea98c15f9e674b404e18c1&o=&hp=1"
                                    },
                                    new RoomImage
                                    {
                                        URL = "https://cf.bstatic.com/xdata/images/hotel/max1024x768/496718919.jpg?k=a8d822a2e983a07bbc23cb63ec45c65846f939fec12fc0bd8cb2c500149ddf20&o=&hp=1"
                                    }
                                }
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