using System;

namespace geziOtomasyonProjesi.Models
{
    /// <summary>
    /// Yer kategorileri enum
    /// </summary>
    public enum PlaceCategory
    {
        Muzeler,
        TarihiYerler,
        Parklar,
        Restoranlar,
        AlisVerisMerkezleri,
        Plajlar,
        DogalGuzellikler,
        Eglence,
        Diger
    }

    /// <summary>
    /// Gezilecek yer sınıfı - LocationBase'den kalıtım
    /// POLİMORFİZM örneği
    /// </summary>
    public class Place : LocationBase
    {
        public int CityId { get; set; }
        public PlaceCategory Category { get; set; }
        public int AverageVisitMinutes { get; set; }
        public decimal EntryFee { get; set; }
        public string OpeningHours { get; set; }
        public string ClosingHours { get; set; }
        public bool IsOpenOnWeekends { get; set; }
        public double Rating { get; set; }
        public string Address { get; set; }

        // Navigation property
        public City City { get; set; }

        // Constructor
        public Place() : base()
        {
            IsOpenOnWeekends = true;
            Rating = 0;
        }

        // OVERRIDE - Abstract method implementasyonu
        public override string GetDisplayName()
        {
            return $"{Name} ({GetCategoryName()})";
        }

        // OVERRIDE - Abstract method implementasyonu
        public override string GetLocationType()
        {
            return "Gezilecek Yer";
        }

        // OVERRIDE - Doğrulama
        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return false;
            if (CityId <= 0)
                return false;
            if (AverageVisitMinutes < 0)
                return false;
            if (EntryFee < 0)
                return false;
            return true;
        }

        // OVERRIDE - Özet bilgi (base kullanımı)
        public override string GetSummary()
        {
            string baseSummary = base.GetSummary();
            return $"{baseSummary}, Category: {GetCategoryName()}, Fee: {EntryFee:C}, Visit Time: {AverageVisitMinutes} min";
        }

        // Kategori adını Türkçe döndür
        public string GetCategoryName()
        {
            switch (Category)
            {
                case PlaceCategory.Muzeler:
                    return "Müzeler";
                case PlaceCategory.TarihiYerler:
                    return "Tarihi Yerler";
                case PlaceCategory.Parklar:
                    return "Parklar";
                case PlaceCategory.Restoranlar:
                    return "Restoranlar";
                case PlaceCategory.AlisVerisMerkezleri:
                    return "Alışveriş Merkezleri";
                case PlaceCategory.Plajlar:
                    return "Plajlar";
                case PlaceCategory.DogalGuzellikler:
                    return "Doğal Güzellikler";
                case PlaceCategory.Eglence:
                    return "Eğlence";
                default:
                    return "Diğer";
            }
        }

        // Ziyaret formatı
        public string GetVisitTimeFormatted()
        {
            if (AverageVisitMinutes < 60)
                return $"{AverageVisitMinutes} dakika";
            
            int hours = AverageVisitMinutes / 60;
            int mins = AverageVisitMinutes % 60;
            
            if (mins == 0)
                return $"{hours} saat";
            
            return $"{hours} saat {mins} dakika";
        }
    }
}
