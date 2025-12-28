using System;

namespace geziOtomasyonProjesi.Models
{
    /// <summary>
    /// Lokasyon için abstract base class
    /// ÇOKLU KALITIM HİYERARŞİSİ örneği
    /// BaseEntity -> LocationBase -> City/Place/Hotel
    /// </summary>
    public abstract class LocationBase : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }

        // Constructor - base çağrısı
        protected LocationBase() : base()
        {
        }

        // ABSTRACT METHOD - Alt sınıflar implement etmeli
        public abstract string GetLocationType();

        // VIRTUAL METHOD - Koordinat kontrolü
        public virtual bool HasCoordinates()
        {
            return Latitude.HasValue && Longitude.HasValue;
        }

        // OVERRIDE - BaseEntity'den gelen virtual method
        public override string GetSummary()
        {
            string baseSummary = base.GetSummary();
            string coords = HasCoordinates() ? $"({Latitude:F4}, {Longitude:F4})" : "No coordinates";
            return $"{baseSummary}, Location: {Name}, {coords}";
        }

        // Mesafe hesaplama (Haversine formülü)
        public double CalculateDistanceTo(LocationBase other)
        {
            if (!HasCoordinates() || !other.HasCoordinates())
                return -1;

            const double R = 6371; // Dünya'nın yarıçapı (km)
            
            double lat1 = Latitude.Value * Math.PI / 180;
            double lat2 = other.Latitude.Value * Math.PI / 180;
            double dLat = (other.Latitude.Value - Latitude.Value) * Math.PI / 180;
            double dLon = (other.Longitude.Value - Longitude.Value) * Math.PI / 180;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            
            return R * c;
        }
    }
}
