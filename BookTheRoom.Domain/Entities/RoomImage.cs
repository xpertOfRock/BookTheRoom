using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTheRoom.Domain.Entities
{
    public class RoomImage
    {
        public int Id { get; set; }
        public string URL { get; set; }
        public int RoomId { get; set; }

        public Room Room { get; set; }
    }
}
