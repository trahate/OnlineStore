using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelDataRecorder.Model
{
   public class TravelStateModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        // public virtual ICollection<TravelCityModel> TravelCities { get; set; }
        // public virtual TravelCountryModel TravelCountry { get; set; }
    }
}
