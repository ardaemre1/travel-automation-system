using System;

namespace geziOtomasyonProjesi.Models
{
    /// <summary>
    /// Bütçe kategorileri enum
    /// </summary>
    public enum BudgetCategory
    {
        Ulasim,
        Konaklama,
        YemeIcme,
        Aktivite,
        Alisveris,
        Eglence,
        Saglik,
        Diger
    }

    /// <summary>
    /// Bütçe sınıfı - BaseEntity'den kalıtım
    /// ÇOK BİÇİMLİLİK (Polymorphism) örneği
    /// </summary>
    public class Budget : BaseEntity
    {
        public int UserId { get; set; }
        public int? TripId { get; set; }
        public BudgetCategory Category { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }
        public DateTime ExpenseDate { get; set; }
        public string Currency { get; set; }
        public bool IsPaid { get; set; }

        // Navigation properties
        public User User { get; set; }
        public Trip Trip { get; set; }

        // Constructor
        public Budget() : base()
        {
            ExpenseDate = DateTime.Now;
            Currency = "TRY";
            IsPaid = false;
        }

        // Parametreli constructor
        public Budget(BudgetCategory category, decimal amount, string description) : base()
        {
            Category = category;
            Amount = amount;
            Description = description;
            ExpenseDate = DateTime.Now;
            Currency = "TRY";
        }

        // OVERRIDE - Abstract method implementasyonu
        public override string GetDisplayName()
        {
            return $"{GetCategoryName()} - {Amount:C}";
        }

        // OVERRIDE - Doğrulama
        public override bool Validate()
        {
            if (UserId <= 0)
                return false;
            if (Amount <= 0)
                return false;
            if (string.IsNullOrWhiteSpace(Description))
                return false;
            return true;
        }

        // OVERRIDE - Özet bilgi
        public override string GetSummary()
        {
            string baseSummary = base.GetSummary();
            string paidStatus = IsPaid ? "Ödendi" : "Bekliyor";
            return $"{baseSummary}, {GetCategoryName()}: {Amount:C} ({paidStatus})";
        }

        // OVERRIDE - Silme kontrolü
        public override bool CanDelete()
        {
            // Ödenmiş harcamalar silinememeli (sadece uyarı amaçlı)
            if (IsPaid)
            {
                // Yine de base kontrolüne bırakıyoruz
                return base.CanDelete();
            }
            return base.CanDelete();
        }

        // Kategori adını Türkçe döndür
        public string GetCategoryName()
        {
            switch (Category)
            {
                case BudgetCategory.Ulasim:
                    return "Ulaşım";
                case BudgetCategory.Konaklama:
                    return "Konaklama";
                case BudgetCategory.YemeIcme:
                    return "Yeme-İçme";
                case BudgetCategory.Aktivite:
                    return "Aktivite";
                case BudgetCategory.Alisveris:
                    return "Alışveriş";
                case BudgetCategory.Eglence:
                    return "Eğlence";
                case BudgetCategory.Saglik:
                    return "Sağlık";
                default:
                    return "Diğer";
            }
        }

        // Ödeme durumunu güncelle
        public void MarkAsPaid()
        {
            IsPaid = true;
            MarkAsUpdated();
        }
    }
}
