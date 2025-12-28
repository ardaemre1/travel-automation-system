using System;

namespace geziOtomasyonProjesi.Models
{
    /// <summary>
    /// Otel yıldız dereceleri enum
    /// </summary>
    public enum HotelStars
    {
        OneStar = 1,
        TwoStar = 2,
        ThreeStar = 3,
        FourStar = 4,
        FiveStar = 5
    }

    /// <summary>
    /// Otel sınıfı - LocationBase'den kalıtım
    /// POLİMORFİZM örneği
    /// </summary>
    public class Hotel : LocationBase
    {
        public int CityId { get; set; }
        public HotelStars Stars { get; set; }
        public decimal PricePerNight { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Website { get; set; }
        public bool HasWifi { get; set; }
        public bool HasParking { get; set; }
        public bool HasPool { get; set; }
        public bool HasBreakfast { get; set; }
        public double Rating { get; set; }

        // Navigation property
        public City City { get; set; }

        // Constructor
        public Hotel() : base()
        {
            Stars = HotelStars.ThreeStar;
            HasWifi = true;
        }

        // OVERRIDE - Abstract method implementasyonu
        public override string GetDisplayName()
        {
            return $"{Name} ({GetStarsDisplay()})";
        }

        // OVERRIDE - Abstract method implementasyonu
        public override string GetLocationType()
        {
            return "Otel";
        }

        // OVERRIDE - Doğrulama
        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return false;
            if (CityId <= 0)
                return false;
            if (PricePerNight <= 0)
                return false;
            return true;
        }

        // OVERRIDE - Özet bilgi
        public override string GetSummary()
        {
            string baseSummary = base.GetSummary();
            return $"{baseSummary}, Stars: {GetStarsDisplay()}, Price: {PricePerNight:C}/night";
        }

        // Yıldız gösterimi
        public string GetStarsDisplay()
        {
            return new string('★', (int)Stars) + new string('☆', 5 - (int)Stars);
        }

        // Özellikler listesi
        public string GetAmenities()
        {
            var amenities = new System.Collections.Generic.List<string>();
            
            if (HasWifi) amenities.Add("WiFi");
            if (HasParking) amenities.Add("Otopark");
            if (HasPool) amenities.Add("Havuz");
            if (HasBreakfast) amenities.Add("Kahvaltı");
            
            return string.Join(", ", amenities);
        }

        // Toplam konaklama ücreti hesapla
        public decimal CalculateTotalPrice(int nights)
        {
            return PricePerNight * nights;
        }
    }
}
