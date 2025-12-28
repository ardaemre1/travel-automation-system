using System;

namespace geziOtomasyonProjesi.Models
{
    /// <summary>
    /// Kullanıcı sınıfı - BaseEntity'den kalıtım alır
    /// OVERRIDE ve BASE kullanım örneği
    /// </summary>
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public DateTime? LastLoginAt { get; set; }

        // Constructor - base constructor çağrısı
        public User() : base()
        {
            // Base constructor otomatik çağrılır
        }

        // Parametreli constructor
        public User(string username, string email) : base()
        {
            Username = username;
            Email = email;
        }

        // OVERRIDE - Abstract method implementasyonu
        public override string GetDisplayName()
        {
            return !string.IsNullOrEmpty(FullName) ? FullName : Username;
        }

        // OVERRIDE - Abstract method implementasyonu
        public override bool Validate()
        {
            if (string.IsNullOrWhiteSpace(Username))
                return false;
            if (string.IsNullOrWhiteSpace(Email))
                return false;
            if (string.IsNullOrWhiteSpace(PasswordHash))
                return false;
            if (Username.Length < 3)
                return false;
            return true;
        }

        // OVERRIDE - Virtual method override (base çağrısı ile)
        public override string GetSummary()
        {
            // BASE kullanımı - üst sınıf metodunu çağırma
            string baseSummary = base.GetSummary();
            return $"{baseSummary}, User: {Username}, Email: {Email}";
        }

        // OVERRIDE - Virtual method override
        public override bool CanDelete()
        {
            // Kullanıcı son giriş yapmışsa silinemesin
            if (LastLoginAt.HasValue && (DateTime.Now - LastLoginAt.Value).TotalDays < 30)
            {
                return false;
            }
            return base.CanDelete();
        }

        // Kullanıcıya özel method
        public void UpdateLastLogin()
        {
            LastLoginAt = DateTime.Now;
            MarkAsUpdated();
        }
    }
}
