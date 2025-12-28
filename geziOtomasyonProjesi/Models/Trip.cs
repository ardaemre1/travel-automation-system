using System;
using System.Collections.Generic;
using System.Linq;

namespace geziOtomasyonProjesi.Models
{
    /// <summary>
    /// Seyahat planı sınıfı - BaseEntity'den kalıtım
    /// POLİMORFİZM ve karmaşık iş mantığı örneği
    /// </summary>
    public class Trip : BaseEntity
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalBudget { get; set; }
        public string Notes { get; set; }

        // Navigation properties
        public User User { get; set; }
        public List<TripPlace> TripPlaces { get; set; }
        public List<Budget> Budgets { get; set; }

        // Constructor
        public Trip() : base()
        {
            TripPlaces = new List<TripPlace>();
            Budgets = new List<Budget>();
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddDays(7);
        }

        // OVERRIDE - Abstract method implementasyonu
        public override string GetDisplayName()
        {
            return $"{Name} ({StartDate:dd.MM.yyyy} - {EndDate:dd.MM.yyyy})";
        }

        // OVERRIDE - Doğrulama
        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Name))
                return false;
            if (UserId <= 0)
                return false;
            if (EndDate < StartDate)
                return false;
            if (TotalBudget < 0)
                return false;
            return true;
        }

        // OVERRIDE - Özet bilgi
        public override string GetSummary()
        {
            string baseSummary = base.GetSummary();
            return $"{baseSummary}, Trip: {Name}, Duration: {GetDurationDays()} days, Budget: {TotalBudget:C}";
        }

        // OVERRIDE - Silme kontrolü
        public override bool CanDelete()
        {
            // Başlamış bir seyahat silinemez
            if (DateTime.Now >= StartDate && DateTime.Now <= EndDate)
                return false;
            return base.CanDelete();
        }

        // Seyahat süresi (gün)
        public int GetDurationDays()
        {
            return (EndDate - StartDate).Days + 1;
        }

        // Toplam harcama
        public decimal GetTotalExpenses()
        {
            if (Budgets == null || Budgets.Count == 0)
                return 0;
            return Budgets.Sum(b => b.Amount);
        }

        // Kalan bütçe
        public decimal GetRemainingBudget()
        {
            return TotalBudget - GetTotalExpenses();
        }

        // Bütçe kullanım yüzdesi
        public double GetBudgetUsagePercentage()
        {
            if (TotalBudget <= 0)
                return 0;
            return (double)(GetTotalExpenses() / TotalBudget) * 100;
        }

        // Belirli bir gün için yerler
        public List<TripPlace> GetPlacesForDay(int dayNumber)
        {
            if (TripPlaces == null)
                return new List<TripPlace>();
            return TripPlaces.Where(tp => tp.DayNumber == dayNumber)
                             .OrderBy(tp => tp.OrderInDay)
                             .ToList();
        }

        // Yer ekle
        public void AddPlace(Place place, int dayNumber, int order)
        {
            if (TripPlaces == null)
                TripPlaces = new List<TripPlace>();

            var tripPlace = new TripPlace
            {
                TripId = this.Id,
                PlaceId = place.Id,
                Place = place,
                DayNumber = dayNumber,
                OrderInDay = order
            };

            TripPlaces.Add(tripPlace);
        }

        // Harcama ekle
        public void AddExpense(Budget budget)
        {
            if (Budgets == null)
                Budgets = new List<Budget>();

            budget.TripId = this.Id;
            budget.UserId = this.UserId;
            Budgets.Add(budget);
        }
    }

    /// <summary>
    /// Seyahat-Yer ilişki sınıfı
    /// </summary>
    public class TripPlace
    {
        public int TripId { get; set; }
        public int PlaceId { get; set; }
        public int DayNumber { get; set; }
        public int OrderInDay { get; set; }
        public string Notes { get; set; }
        public TimeSpan? PlannedTime { get; set; }

        // Navigation properties
        public Trip Trip { get; set; }
        public Place Place { get; set; }
    }
}
