using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model
{
    public class TravelPlaceDetailModel
    {
        public int Id { get; set; }
        public int TravelId { get; set; }
        public string Type { get; set; }
        public int CityId { get; set; }
        public int StateId { get; set; }
    }
}
