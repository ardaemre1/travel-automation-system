using System;

namespace geziOtomasyonProjesi.Models
{
    /// <summary>
    /// Tüm entity sınıfları için abstract base class
    /// KALITIM (Inheritance) ve ABSTRACT CLASS örneği
    /// </summary>
    public abstract class BaseEntity
    {
        // Ortak özellikler
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; }

        // Constructor - base sınıf constructor'ı
        protected BaseEntity()
        {
            CreatedAt = DateTime.Now;
            IsActive = true;
        }

        // ABSTRACT METHOD - Alt sınıflar tarafından implement edilmeli
        public abstract string GetDisplayName();

        // ABSTRACT METHOD - Doğrulama için
        public abstract bool Validate();

        // VIRTUAL METHOD - Alt sınıflar override edebilir
        public virtual string GetSummary()
        {
            return $"Entity ID: {Id}, Created: {CreatedAt:dd.MM.yyyy}";
        }

        // VIRTUAL METHOD - Silme öncesi kontrol
        public virtual bool CanDelete()
        {
            return IsActive;
        }

        // Normal method
        public void MarkAsUpdated()
        {
            UpdatedAt = DateTime.Now;
        }

        // Normal method
        public void Deactivate()
        {
            IsActive = false;
            MarkAsUpdated();
        }

        // ToString override
        public override string ToString()
        {
            return GetDisplayName();
        }
    }
}
