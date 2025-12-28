using System;
using System.Data;
using System.Data.SQLite;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using geziOtomasyonProjesi.Models;

namespace geziOtomasyonProjesi.Data
{
    /// <summary>
    /// Veritabanı işlemleri için helper sınıf
    /// </summary>
    public class DatabaseHelper
    {
        private static DatabaseHelper _instance;
        private static readonly object _lock = new object();
        
        private readonly string _connectionString;
        private readonly string _databasePath;

        // Singleton pattern
        public static DatabaseHelper Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseHelper();
                        }
                    }
                }
                return _instance;
            }
        }

        // Constructor
        private DatabaseHelper()
        {
            string appDataPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "GeziOtomasyonProjesi");
            
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }

            _databasePath = Path.Combine(appDataPath, "travel_planner.db");
            _connectionString = $"Data Source={_databasePath};Version=3;";

            InitializeDatabase();
        }

        // Veritabanını başlat
        private void InitializeDatabase()
        {
            if (!File.Exists(_databasePath))
            {
                SQLiteConnection.CreateFile(_databasePath);
            }

            CreateTables();
            SeedData();
        }

        // Tabloları oluştur
        private void CreateTables()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                // Users tablosu
                ExecuteNonQuery(connection, @"
                    CREATE TABLE IF NOT EXISTS Users (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Username TEXT NOT NULL UNIQUE,
                        PasswordHash TEXT NOT NULL,
                        Email TEXT NOT NULL,
                        FullName TEXT,
                        LastLoginAt TEXT,
                        CreatedAt TEXT NOT NULL,
                        UpdatedAt TEXT,
                        IsActive INTEGER DEFAULT 1
                    )");

                // Cities tablosu
                ExecuteNonQuery(connection, @"
                    CREATE TABLE IF NOT EXISTS Cities (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT NOT NULL,
                        Country TEXT NOT NULL,
                        Region TEXT,
                        Description TEXT,
                        ImageUrl TEXT,
                        Latitude REAL,
                        Longitude REAL,
                        Population INTEGER,
                        Currency TEXT,
                        Language TEXT,
                        TimeZone TEXT,
                        CreatedAt TEXT NOT NULL,
                        UpdatedAt TEXT,
                        IsActive INTEGER DEFAULT 1
                    )");

                // Places tablosu
                ExecuteNonQuery(connection, @"
                    CREATE TABLE IF NOT EXISTS Places (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CityId INTEGER NOT NULL,
                        Name TEXT NOT NULL,
                        Description TEXT,
                        ImageUrl TEXT,
                        Latitude REAL,
                        Longitude REAL,
                        Category INTEGER NOT NULL,
                        AverageVisitMinutes INTEGER DEFAULT 60,
                        EntryFee REAL DEFAULT 0,
                        OpeningHours TEXT,
                        ClosingHours TEXT,
                        IsOpenOnWeekends INTEGER DEFAULT 1,
                        Rating REAL DEFAULT 0,
                        Address TEXT,
                        CreatedAt TEXT NOT NULL,
                        UpdatedAt TEXT,
                        IsActive INTEGER DEFAULT 1,
                        FOREIGN KEY (CityId) REFERENCES Cities(Id)
                    )");

                // Hotels tablosu
                ExecuteNonQuery(connection, @"
                    CREATE TABLE IF NOT EXISTS Hotels (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        CityId INTEGER NOT NULL,
                        Name TEXT NOT NULL,
                        Description TEXT,
                        ImageUrl TEXT,
                        Latitude REAL,
                        Longitude REAL,
                        Stars INTEGER DEFAULT 3,
                        PricePerNight REAL NOT NULL,
                        Address TEXT,
                        Phone TEXT,
                        Email TEXT,
                        Website TEXT,
                        HasWifi INTEGER DEFAULT 1,
                        HasParking INTEGER DEFAULT 0,
                        HasPool INTEGER DEFAULT 0,
                        HasBreakfast INTEGER DEFAULT 0,
                        Rating REAL DEFAULT 0,
                        CreatedAt TEXT NOT NULL,
                        UpdatedAt TEXT,
                        IsActive INTEGER DEFAULT 1,
                        FOREIGN KEY (CityId) REFERENCES Cities(Id)
                    )");

                // Trips tablosu
                ExecuteNonQuery(connection, @"
                    CREATE TABLE IF NOT EXISTS Trips (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER NOT NULL,
                        Name TEXT NOT NULL,
                        StartDate TEXT NOT NULL,
                        EndDate TEXT NOT NULL,
                        TotalBudget REAL DEFAULT 0,
                        Notes TEXT,
                        CreatedAt TEXT NOT NULL,
                        UpdatedAt TEXT,
                        IsActive INTEGER DEFAULT 1,
                        FOREIGN KEY (UserId) REFERENCES Users(Id)
                    )");

                // TripPlaces tablosu
                ExecuteNonQuery(connection, @"
                    CREATE TABLE IF NOT EXISTS TripPlaces (
                        TripId INTEGER NOT NULL,
                        PlaceId INTEGER NOT NULL,
                        DayNumber INTEGER NOT NULL,
                        OrderInDay INTEGER NOT NULL,
                        Notes TEXT,
                        PlannedTime TEXT,
                        PRIMARY KEY (TripId, PlaceId, DayNumber),
                        FOREIGN KEY (TripId) REFERENCES Trips(Id),
                        FOREIGN KEY (PlaceId) REFERENCES Places(Id)
                    )");

                // Budgets tablosu
                ExecuteNonQuery(connection, @"
                    CREATE TABLE IF NOT EXISTS Budgets (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        UserId INTEGER NOT NULL,
                        TripId INTEGER,
                        Category INTEGER NOT NULL,
                        Amount REAL NOT NULL,
                        Description TEXT,
                        ExpenseDate TEXT NOT NULL,
                        Currency TEXT DEFAULT 'TRY',
                        IsPaid INTEGER DEFAULT 0,
                        CreatedAt TEXT NOT NULL,
                        UpdatedAt TEXT,
                        IsActive INTEGER DEFAULT 1,
                        FOREIGN KEY (UserId) REFERENCES Users(Id),
                        FOREIGN KEY (TripId) REFERENCES Trips(Id)
                    )");

                connection.Close();
            }
        }

        // Örnek veri ekle
        private void SeedData()
        {
            using (var connection = GetConnection())
            {
                connection.Open();

                // Örnek şehir var mı kontrol et
                var checkCmd = new SQLiteCommand("SELECT COUNT(*) FROM Cities", connection);
                long cityCount = (long)checkCmd.ExecuteScalar();

                if (cityCount == 0)
                {
                    // Örnek şehirler ekle
                    ExecuteNonQuery(connection, @"
                        INSERT INTO Cities (Name, Country, Region, Description, Latitude, Longitude, Currency, Language, CreatedAt)
                        VALUES 
                        ('Paris', 'Fransa', 'Île-de-France', 'Işıklar şehri, sanat ve romantizm merkezi', 48.8566, 2.3522, 'EUR', 'Fransızca', datetime('now')),
                        ('İstanbul', 'Türkiye', 'Marmara', 'İki kıtayı birleştiren tarihi şehir', 41.0082, 28.9784, 'TRY', 'Türkçe', datetime('now')),
                        ('Roma', 'İtalya', 'Lazio', 'Antik Roma İmparatorluğunun başkenti', 41.9028, 12.4964, 'EUR', 'İtalyanca', datetime('now')),
                        ('Tokyo', 'Japonya', 'Kanto', 'Modern teknoloji ve geleneksel kültürün buluştuğu şehir', 35.6762, 139.6503, 'JPY', 'Japonca', datetime('now')),
                        ('Barcelona', 'İspanya', 'Katalonya', 'Gaudi mimarisi ve Akdeniz güzellikleri', 41.3851, 2.1734, 'EUR', 'İspanyolca', datetime('now'))
                    ");

                    // Örnek yerler ekle
                    ExecuteNonQuery(connection, @"
                        INSERT INTO Places (CityId, Name, Description, Category, AverageVisitMinutes, EntryFee, Rating, CreatedAt)
                        VALUES 
                        (1, 'Eyfel Kulesi', 'Paris''in simgesi, 324 metre yüksekliğinde demir kule', 1, 120, 26.80, 4.7, datetime('now')),
                        (1, 'Louvre Müzesi', 'Dünyanın en büyük sanat müzesi', 0, 240, 17.00, 4.8, datetime('now')),
                        (1, 'Notre-Dame Katedrali', 'Gotik mimari şaheseri', 1, 90, 0, 4.6, datetime('now')),
                        (2, 'Ayasofya', 'Bizans döneminden kalma tarihi yapı', 1, 120, 25.00, 4.8, datetime('now')),
                        (2, 'Topkapı Sarayı', 'Osmanlı padişahlarının yaşadığı saray', 1, 180, 30.00, 4.7, datetime('now')),
                        (2, 'Kapalıçarşı', 'Dünyanın en eski kapalı çarşılarından biri', 4, 120, 0, 4.5, datetime('now')),
                        (3, 'Kolezyum', 'Antik Roma amfitiyatrosu', 1, 120, 18.00, 4.8, datetime('now')),
                        (3, 'Vatikan Müzesi', 'Papalık sanat koleksiyonu', 0, 240, 17.00, 4.7, datetime('now'))
                    ");

                    // Örnek oteller ekle
                    ExecuteNonQuery(connection, @"
                        INSERT INTO Hotels (CityId, Name, Stars, PricePerNight, HasWifi, HasBreakfast, Rating, CreatedAt)
                        VALUES 
                        (1, 'Hotel Le Marais', 4, 180.00, 1, 1, 4.5, datetime('now')),
                        (1, 'Pullman Paris Tour Eiffel', 5, 350.00, 1, 1, 4.7, datetime('now')),
                        (2, 'Sultanahmet Palace Hotel', 4, 120.00, 1, 1, 4.4, datetime('now')),
                        (2, 'Four Seasons Istanbul', 5, 450.00, 1, 1, 4.9, datetime('now')),
                        (3, 'Hotel Artemide', 4, 150.00, 1, 1, 4.6, datetime('now'))
                    ");
                }

                // Varsayılan kullanıcı oluştur (arda / 1234a)
                var userCheck = new SQLiteCommand("SELECT COUNT(*) FROM Users WHERE Username = 'arda'", connection);
                long userCount = (long)userCheck.ExecuteScalar();
                
                if (userCount == 0)
                {
                    string passwordHash = HashPassword("1234a");
                    var insertUser = new SQLiteCommand(
                        @"INSERT INTO Users (Username, PasswordHash, Email, FullName, CreatedAt, IsActive) 
                          VALUES ('arda', @hash, 'arda@example.com', 'Arda', datetime('now'), 1)", connection);
                    insertUser.Parameters.AddWithValue("@hash", passwordHash);
                    insertUser.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        // Bağlantı al
        public SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(_connectionString);
        }

        // Sorgu çalıştır (NonQuery)
        private void ExecuteNonQuery(SQLiteConnection connection, string sql)
        {
            using (var command = new SQLiteCommand(sql, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        // Şifre hashleme
        public string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        #region User Operations

        // Kullanıcı kayıt
        public bool RegisterUser(string username, string password, string email, string fullName)
        {
            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    
                    string sql = @"INSERT INTO Users (Username, PasswordHash, Email, FullName, CreatedAt) 
                                   VALUES (@username, @passwordHash, @email, @fullName, @createdAt)";
                    
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@passwordHash", HashPassword(password));
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@fullName", fullName);
                        command.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString("o"));
                        
                        command.ExecuteNonQuery();
                    }
                    
                    connection.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Kullanıcı giriş
        public User LoginUser(string username, string password)
        {
            // Test kullanıcısı - her zaman çalışır
            if (username == "arda" && password == "1234a")
            {
                return new User
                {
                    Id = 1,
                    Username = "arda",
                    Email = "arda@example.com",
                    FullName = "Arda",
                    CreatedAt = DateTime.Now,
                    IsActive = true
                };
            }

            try
            {
                using (var connection = GetConnection())
                {
                    connection.Open();
                    
                    string passwordHash = HashPassword(password);
                    string sql = "SELECT * FROM Users WHERE Username = @username AND PasswordHash = @hash AND IsActive = 1";
                    
                    using (var command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@hash", passwordHash);
                        
                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                var user = new User
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    Username = reader["Username"].ToString(),
                                    Email = reader["Email"].ToString(),
                                    FullName = reader["FullName"]?.ToString(),
                                    IsActive = true
                                };
                                return user;
                            }
                        }
                    }
                }
            }
            catch { }
            
            return null;
        }

        // Son giriş zamanını güncelle
        private void UpdateLastLogin(int userId)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                
                string sql = "UPDATE Users SET LastLoginAt = @lastLogin WHERE Id = @id";
                
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@lastLogin", DateTime.Now.ToString("o"));
                    command.Parameters.AddWithValue("@id", userId);
                    command.ExecuteNonQuery();
                }
                
                connection.Close();
            }
        }

        // Kullanıcı adı var mı kontrol et
        public bool UsernameExists(string username)
        {
            using (var connection = GetConnection())
            {
                connection.Open();
                
                string sql = "SELECT COUNT(*) FROM Users WHERE Username = @username";
                
                using (var command = new SQLiteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@username", username);
                    long count = (long)command.ExecuteScalar();
                    
                    connection.Close();
                    return count > 0;
                }
            }
        }

        #endregion
    }
}
