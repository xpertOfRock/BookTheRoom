using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookTheRoom.Application.DTO
{
    public class UploadedFile
    {
        public string FileName { get; set; }
        public Stream Stream { get; set; }
    }

}
