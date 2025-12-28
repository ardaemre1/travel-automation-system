using System;
using System.Collections.Generic;

namespace geziOtomasyonProjesi.Models
{
    /// <summary>
    /// Şehir sınıfı - LocationBase'den kalıtım
    /// POLİMORFİZM örneği
    /// </summary>
    public class City : LocationBase
    {
        public string Country { get; set; }
        public string Region { get; set; }
        public int? Population { get; set; }
        public string Currency { get; set; }
        public string Language { get; set; }
        public string TimeZone { get; set; }

        // İlişkili veriler
        public List<Place> Places { get; set; }
        public List<Hotel> Hotels { get; set; }

        // Constructor
        public City() : base()
        {
            Places = new List<Place>();
            Hotels = new List<Hotel>();
        }

        // OVERRIDE - Abstract method implementasyonu
        public override string GetDisplayName()
        {
            return $"{Name}, {Country}";
        }

        // OVERRIDE - Abstract method implementasyonu
        public override string GetLocationType()
        {
            return "Şehir";
        }

        // OVERRIDE - Doğrulama
        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return false;
            if (string.IsNullOrWhiteSpace(Country))
                return false;
            return true;
        }

        // OVERRIDE - Şehir silme kontrolü
        public override bool CanDelete()
        {
            // Eğer şehre bağlı yerler varsa silinemez
            if (Places != null && Places.Count > 0)
                return false;
            if (Hotels != null && Hotels.Count > 0)
                return false;
            return base.CanDelete();
        }

        // Şehre özel method
        public void AddPlace(Place place)
        {
            if (Places == null)
                Places = new List<Place>();
            
            place.CityId = this.Id;
            Places.Add(place);
        }

        public void AddHotel(Hotel hotel)
        {
            if (Hotels == null)
                Hotels = new List<Hotel>();
            
            hotel.CityId = this.Id;
            Hotels.Add(hotel);
        }
    }
}
