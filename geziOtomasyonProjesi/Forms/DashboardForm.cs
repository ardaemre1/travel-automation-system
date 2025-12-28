using System;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using geziOtomasyonProjesi.Data;
using geziOtomasyonProjesi.Helpers;
using System.Data.SQLite;

namespace geziOtomasyonProjesi.Forms
{
    /// <summary>
    /// Ana Dashboard Formu - Tüm sayfalar gömülü paneller olarak gösterilir
    /// </summary>
    public partial class DashboardForm : Form
    {
        private bool isDragging = false;
        private Point dragStartPoint;
        private string currentPage = "dashboard";

        public DashboardForm()
        {
            InitializeComponent();
            ApplyModernStyle();
            SetupEventHandlers();
            ShowDashboard();
        }

        private void ApplyModernStyle()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorPalette.DarkBackground;
            this.DoubleBuffered = true;
            this.MinimumSize = new Size(1200, 700);
            this.Resize += (s, e) => UpdateLayout();
        }

        private void UpdateLayout()
        {
            if (pnlContent == null) return;
            if (btnLogout != null && pnlSidebar != null)
                btnLogout.Location = new Point(10, pnlSidebar.Height - 60);
            if (lblTitle != null)
                lblTitle.Location = new Point((this.ClientSize.Width - lblTitle.Width) / 2, 12);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (var brush = new LinearGradientBrush(ClientRectangle, 
                ColorPalette.DarkBackground, ColorPalette.DarkSurface, LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }

        private void SetupEventHandlers()
        {
            // iOS tarzı butonlar
            btnClose.Click += (s, e) => Application.Exit();
            btnMinimize.Click += (s, e) => this.WindowState = FormWindowState.Minimized;
            btnMaximize.Click += (s, e) =>
            {
                this.WindowState = this.WindowState == FormWindowState.Maximized 
                    ? FormWindowState.Normal : FormWindowState.Maximized;
            };

            // Pencere sürükleme
            pnlTitleBar.MouseDown += TitleBar_MouseDown;
            pnlTitleBar.MouseMove += TitleBar_MouseMove;
            pnlTitleBar.MouseUp += TitleBar_MouseUp;

            // iOS buton hover
            btnClose.MouseEnter += (s, e) => btnClose.BackColor = Color.FromArgb(255, 120, 110);
            btnClose.MouseLeave += (s, e) => btnClose.BackColor = Color.FromArgb(255, 95, 87);
            btnMinimize.MouseEnter += (s, e) => btnMinimize.BackColor = Color.FromArgb(255, 210, 80);
            btnMinimize.MouseLeave += (s, e) => btnMinimize.BackColor = Color.FromArgb(255, 189, 46);
            btnMaximize.MouseEnter += (s, e) => btnMaximize.BackColor = Color.FromArgb(60, 220, 90);
            btnMaximize.MouseLeave += (s, e) => btnMaximize.BackColor = Color.FromArgb(40, 201, 64);

            // Sidebar hover
            SetupSidebarButtonHover(btnDashboard);
            SetupSidebarButtonHover(btnCities);
            SetupSidebarButtonHover(btnPlaces);
            SetupSidebarButtonHover(btnHotels);
            SetupSidebarButtonHover(btnTrips);
            SetupSidebarButtonHover(btnBudget);
            SetupSidebarButtonHover(btnAI);

            // Sidebar tıklamaları - Gömülü sayfa göster
            btnDashboard.Click += (s, e) => ShowDashboard();
            btnCities.Click += (s, e) => ShowCitiesPage();
            btnPlaces.Click += (s, e) => ShowPlacesPage();
            btnHotels.Click += (s, e) => ShowHotelsPage();
            btnTrips.Click += (s, e) => ShowMapPage(); // Harita sayfası
            btnBudget.Click += (s, e) => ShowBudgetPage();
            btnAI.Click += (s, e) => ShowAIPage();

            // Çıkış
            btnLogout.Click += BtnLogout_Click;
            btnLogout.MouseEnter += (s, e) => btnLogout.BackColor = Color.FromArgb(30, 245, 101, 101);
            btnLogout.MouseLeave += (s, e) => btnLogout.BackColor = Color.Transparent;
        }

        private void SetupSidebarButtonHover(Button btn)
        {
            if (btn == null) return;
            btn.MouseEnter += (s, e) => { if (btn.Tag?.ToString() != "active") btn.BackColor = Color.FromArgb(40, 255, 255, 255); };
            btn.MouseLeave += (s, e) => { if (btn.Tag?.ToString() != "active") btn.BackColor = Color.Transparent; };
        }

        private void SetActiveButton(Button activeBtn)
        {
            // Tüm butonları resetle
            foreach (var btn in new[] { btnDashboard, btnCities, btnPlaces, btnHotels, btnTrips, btnBudget, btnAI })
            {
                if (btn != null)
                {
                    btn.Tag = null;
                    btn.BackColor = Color.Transparent;
                }
            }
            // Aktif butonu işaretle
            if (activeBtn != null)
            {
                activeBtn.Tag = "active";
                activeBtn.BackColor = Color.FromArgb(60, 102, 126, 234);
            }
        }

        private void ClearContent()
        {
            pnlContent.Controls.Clear();
        }

        #region Dashboard Sayfası

        private void ShowDashboard()
        {
            SetActiveButton(btnDashboard);
            ClearContent();
            currentPage = "dashboard";

            // Hoş geldin
            var lblWelcome = new Label
            {
                Text = $"Merhaba, {LoginForm.CurrentUser?.GetDisplayName() ?? "Kullanıcı"}! 👋",
                Font = new Font("Segoe UI", 28F, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Location = new Point(40, 30),
                AutoSize = true
            };
            pnlContent.Controls.Add(lblWelcome);

            var lblSubtitle = new Label
            {
                Text = "Bir sonraki maceranızı planlamaya hazır mısınız?",
                Font = new Font("Segoe UI", 13F),
                ForeColor = Color.FromArgb(160, 174, 192),
                BackColor = Color.Transparent,
                Location = new Point(40, 80),
                AutoSize = true
            };
            pnlContent.Controls.Add(lblSubtitle);

            // İstatistik kartları
            int cardX = 40, cardY = 140, cardW = 220, cardH = 140, spacing = 30;
            CreateStatCard("🌍", GetCount("Cities"), "Şehirler", Color.FromArgb(102, 126, 234), cardX, cardY, cardW, cardH);
            CreateStatCard("📍", GetCount("Places"), "Gezilecek Yerler", Color.FromArgb(118, 75, 162), cardX + cardW + spacing, cardY, cardW, cardH);
            CreateStatCard("🗺️", GetTripCount().ToString(), "Seyahatlerim", Color.FromArgb(72, 187, 120), cardX + (cardW + spacing) * 2, cardY, cardW, cardH);
            CreateStatCard("💰", $"₺{GetBudgetTotal():N0}", "Toplam Bütçe", Color.FromArgb(237, 137, 54), cardX + (cardW + spacing) * 3, cardY, cardW, cardH);
        }

        private void CreateStatCard(string icon, string value, string title, Color accent, int x, int y, int w, int h)
        {
            var panel = new Panel { BackColor = Color.FromArgb(40, 255, 255, 255), Size = new Size(w, h), Location = new Point(x, y) };
            panel.Controls.Add(new Label { Text = icon, Font = new Font("Segoe UI Emoji", 28F), Location = new Point(20, 15), AutoSize = true, BackColor = Color.Transparent });
            panel.Controls.Add(new Label { Text = value, Font = new Font("Segoe UI", 26F, FontStyle.Bold), ForeColor = accent, Location = new Point(20, 60), AutoSize = true, BackColor = Color.Transparent });
            panel.Controls.Add(new Label { Text = title, Font = new Font("Segoe UI", 10F), ForeColor = Color.FromArgb(160, 174, 192), Location = new Point(20, 110), AutoSize = true, BackColor = Color.Transparent });
            pnlContent.Controls.Add(panel);
        }

        private string GetCount(string table) { try { using (var c = DatabaseHelper.Instance.GetConnection()) { c.Open(); return new SQLiteCommand($"SELECT COUNT(*) FROM {table} WHERE IsActive=1", c).ExecuteScalar().ToString(); } } catch { return "0"; } }
        private long GetTripCount() { try { using (var c = DatabaseHelper.Instance.GetConnection()) { c.Open(); var cmd = new SQLiteCommand("SELECT COUNT(*) FROM Trips WHERE UserId=@u", c); cmd.Parameters.AddWithValue("@u", LoginForm.CurrentUser?.Id ?? 0); return (long)cmd.ExecuteScalar(); } } catch { return 0; } }
        private decimal GetBudgetTotal() { try { using (var c = DatabaseHelper.Instance.GetConnection()) { c.Open(); var cmd = new SQLiteCommand("SELECT COALESCE(SUM(Amount),0) FROM Budgets WHERE UserId=@u", c); cmd.Parameters.AddWithValue("@u", LoginForm.CurrentUser?.Id ?? 0); return Convert.ToDecimal(cmd.ExecuteScalar()); } } catch { return 0; } }

        #endregion

        #region Şehirler Sayfası

        private DataGridView dgvCities;

        private void ShowCitiesPage()
        {
            SetActiveButton(btnCities);
            ClearContent();
            currentPage = "cities";

            pnlContent.Controls.Add(new Label { Text = "🌍 Şehirler", Font = new Font("Segoe UI", 24F, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.Transparent, Location = new Point(40, 20), AutoSize = true });

            // Butonlar
            var btnAdd = CreateActionButton("➕ Ekle", Color.FromArgb(72, 187, 120), 40, 80);
            btnAdd.Click += (s, e) => { var f = new AddCityForm(); f.ShowDialog(); LoadCitiesData(); };
            pnlContent.Controls.Add(btnAdd);

            var btnDelete = CreateActionButton("🗑️ Sil", Color.FromArgb(245, 101, 101), 150, 80);
            btnDelete.Click += DeleteCity;
            pnlContent.Controls.Add(btnDelete);

            // Grid
            dgvCities = CreateDataGrid(40, 140, pnlContent.Width - 80, pnlContent.Height - 180);
            pnlContent.Controls.Add(dgvCities);
            LoadCitiesData();
        }

        private void LoadCitiesData()
        {
            try { using (var c = DatabaseHelper.Instance.GetConnection()) { c.Open(); var dt = new DataTable(); new SQLiteDataAdapter(new SQLiteCommand("SELECT Id as ID, Name as Şehir, Country as Ülke, Region as Bölge, Currency as 'Para Birimi' FROM Cities WHERE IsActive=1", c)).Fill(dt); dgvCities.DataSource = dt; } } catch { }
        }

        private void DeleteCity(object s, EventArgs e)
        {
            if (dgvCities.SelectedRows.Count == 0) { MessageBox.Show("Silmek için bir satır seçin."); return; }
            int id = Convert.ToInt32(dgvCities.SelectedRows[0].Cells["ID"].Value);
            if (MessageBox.Show("Silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
            { try { using (var c = DatabaseHelper.Instance.GetConnection()) { c.Open(); new SQLiteCommand($"UPDATE Cities SET IsActive=0 WHERE Id={id}", c).ExecuteNonQuery(); } LoadCitiesData(); } catch { } }
        }

        #endregion

        #region Gezilecek Yerler Sayfası

        private DataGridView dgvPlaces;

        private void ShowPlacesPage()
        {
            SetActiveButton(btnPlaces);
            ClearContent();
            currentPage = "places";

            pnlContent.Controls.Add(new Label { Text = "📍 Gezilecek Yerler", Font = new Font("Segoe UI", 24F, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.Transparent, Location = new Point(40, 20), AutoSize = true });

            var btnAdd = CreateActionButton("➕ Ekle", Color.FromArgb(72, 187, 120), 40, 80);
            btnAdd.Click += (s, e) => { var f = new AddPlaceForm(); f.ShowDialog(); if (f.IsSaved) LoadPlacesData(); };
            pnlContent.Controls.Add(btnAdd);

            var btnDelete = CreateActionButton("🗑️ Sil", Color.FromArgb(245, 101, 101), 150, 80);
            btnDelete.Click += DeletePlace;
            pnlContent.Controls.Add(btnDelete);

            var btnRefresh = CreateActionButton("🔄 Yenile", Color.FromArgb(118, 75, 162), 260, 80);
            btnRefresh.Click += (s, e) => LoadPlacesData();
            pnlContent.Controls.Add(btnRefresh);

            dgvPlaces = CreateDataGrid(40, 140, pnlContent.Width - 80, pnlContent.Height - 180);
            pnlContent.Controls.Add(dgvPlaces);
            LoadPlacesData();
        }

        private void LoadPlacesData()
        {
            try { 
                using (var c = DatabaseHelper.Instance.GetConnection()) { 
                    c.Open(); 
                    var dt = new DataTable(); 
                    new SQLiteDataAdapter(new SQLiteCommand("SELECT p.Id as ID, p.Name as 'Yer Adi', c.Name as Sehir, p.Latitude as Enlem, p.Longitude as Boylam, p.EntryFee as 'Giris Ucreti' FROM Places p INNER JOIN Cities c ON p.CityId=c.Id WHERE p.IsActive=1", c)).Fill(dt); 
                    dgvPlaces.DataSource = dt; 
                } 
            } catch { }
        }

        private void DeletePlace(object s, EventArgs e)
        {
            if (dgvPlaces.SelectedRows.Count == 0) { MessageBox.Show("Silmek için bir satır seçin."); return; }
            int id = Convert.ToInt32(dgvPlaces.SelectedRows[0].Cells["ID"].Value);
            if (MessageBox.Show("Bu yeri silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo) == DialogResult.Yes)
            { 
                try { 
                    using (var c = DatabaseHelper.Instance.GetConnection()) { 
                        c.Open(); 
                        new SQLiteCommand($"UPDATE Places SET IsActive=0 WHERE Id={id}", c).ExecuteNonQuery(); 
                    } 
                    LoadPlacesData(); 
                } catch { } 
            }
        }

        #endregion

        #region Oteller Sayfası

        private DataGridView dgvHotels;

        private void ShowHotelsPage()
        {
            SetActiveButton(btnHotels);
            ClearContent();
            currentPage = "hotels";

            pnlContent.Controls.Add(new Label { Text = "🏨 Oteller", Font = new Font("Segoe UI", 24F, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.Transparent, Location = new Point(40, 20), AutoSize = true });

            var btnRefresh = CreateActionButton("🔄 Yenile", Color.FromArgb(72, 187, 120), 40, 80);
            btnRefresh.Click += (s, e) => LoadHotelsData();
            pnlContent.Controls.Add(btnRefresh);

            dgvHotels = CreateDataGrid(40, 140, pnlContent.Width - 80, pnlContent.Height - 180);
            pnlContent.Controls.Add(dgvHotels);
            LoadHotelsData();
        }

        private void LoadHotelsData()
        {
            try { using (var c = DatabaseHelper.Instance.GetConnection()) { c.Open(); var dt = new DataTable(); new SQLiteDataAdapter(new SQLiteCommand("SELECT h.Id as ID, h.Name as 'Otel Adı', c.Name as Şehir, h.Stars as Yıldız, h.PricePerNight as 'Gecelik (₺)' FROM Hotels h INNER JOIN Cities c ON h.CityId=c.Id WHERE h.IsActive=1", c)).Fill(dt); dgvHotels.DataSource = dt; } } catch { }
        }

        #endregion

        #region Bütçe Sayfası

        private DataGridView dgvBudget;
        private Label lblBudgetTotal;

        private void ShowBudgetPage()
        {
            SetActiveButton(btnBudget);
            ClearContent();
            currentPage = "budget";

            pnlContent.Controls.Add(new Label { Text = "💰 Bütçe Yönetimi", Font = new Font("Segoe UI", 24F, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.Transparent, Location = new Point(40, 20), AutoSize = true });

            lblBudgetTotal = new Label { Text = "Toplam: ₺0", Font = new Font("Segoe UI", 18F, FontStyle.Bold), ForeColor = Color.FromArgb(237, 137, 54), BackColor = Color.Transparent, Location = new Point(40, 70), AutoSize = true };
            pnlContent.Controls.Add(lblBudgetTotal);

            var btnAdd = CreateActionButton("➕ Ekle", Color.FromArgb(72, 187, 120), 300, 70);
            btnAdd.Click += (s, e) => { var f = new AddBudgetForm(); f.ShowDialog(); LoadBudgetData(); };
            pnlContent.Controls.Add(btnAdd);

            dgvBudget = CreateDataGrid(40, 130, pnlContent.Width - 80, pnlContent.Height - 170);
            pnlContent.Controls.Add(dgvBudget);
            LoadBudgetData();
        }

        private void LoadBudgetData()
        {
            int userId = LoginForm.CurrentUser?.Id ?? 0;
            try
            {
                using (var c = DatabaseHelper.Instance.GetConnection())
                {
                    c.Open();
                    var totalCmd = new SQLiteCommand("SELECT COALESCE(SUM(Amount),0) FROM Budgets WHERE UserId=@u", c);
                    totalCmd.Parameters.AddWithValue("@u", userId);
                    lblBudgetTotal.Text = $"Toplam: ₺{Convert.ToDecimal(totalCmd.ExecuteScalar()):N2}";

                    var dt = new DataTable();
                    var cmd = new SQLiteCommand("SELECT Id as ID, Category as Kategori, Amount as 'Tutar (₺)', Description as Açıklama, ExpenseDate as Tarih FROM Budgets WHERE UserId=@u AND IsActive=1 ORDER BY ExpenseDate DESC", c);
                    cmd.Parameters.AddWithValue("@u", userId);
                    new SQLiteDataAdapter(cmd).Fill(dt);
                    dgvBudget.DataSource = dt;
                }
            }
            catch { }
        }

        #endregion

        #region AI Asistan Sayfası

        private RichTextBox txtAIResponse;
        private TextBox txtAIQuestion;

        private void ShowAIPage()
        {
            SetActiveButton(btnAI);
            ClearContent();
            currentPage = "ai";

            pnlContent.Controls.Add(new Label { Text = "🤖 AI Seyahat Asistanı", Font = new Font("Segoe UI", 24F, FontStyle.Bold), ForeColor = Color.White, BackColor = Color.Transparent, Location = new Point(40, 20), AutoSize = true });
            pnlContent.Controls.Add(new Label { Text = "Seyahat planınız hakkında sorular sorun!", Font = new Font("Segoe UI", 11F), ForeColor = Color.FromArgb(160, 174, 192), BackColor = Color.Transparent, Location = new Point(40, 65), AutoSize = true });

            txtAIQuestion = new TextBox { Font = new Font("Segoe UI", 12F), BackColor = Color.FromArgb(45, 55, 85), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle, Multiline = true, Size = new Size(pnlContent.Width - 200, 70), Location = new Point(40, 100), Text = "İstanbul'da 3 günlük bir gezi planı önerir misin?" };
            pnlContent.Controls.Add(txtAIQuestion);

            var btnAsk = CreateActionButton("🚀 Sor", Color.FromArgb(102, 126, 234), pnlContent.Width - 140, 100);
            btnAsk.Size = new Size(80, 70);
            btnAsk.Click += AskAI;
            pnlContent.Controls.Add(btnAsk);

            txtAIResponse = new RichTextBox { Font = new Font("Segoe UI", 11F), BackColor = Color.FromArgb(30, 40, 60), ForeColor = Color.White, BorderStyle = BorderStyle.None, Size = new Size(pnlContent.Width - 80, pnlContent.Height - 220), Location = new Point(40, 190), ReadOnly = true };
            txtAIResponse.Text = "Merhaba! Ben AI seyahat asistanınızım. 🌍\n\nSize şu konularda yardımcı olabilirim:\n• Şehir ve rota önerileri\n• Gezilecek yerler\n• Bütçe planlaması\n• Konaklama önerileri\n\nYukarıya sorunuzu yazın ve 'Sor' butonuna tıklayın!";
            pnlContent.Controls.Add(txtAIResponse);
        }

        private void AskAI(object s, EventArgs e)
        {
            string q = txtAIQuestion.Text.ToLower();
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("🤖 AI Yanıtı\n━━━━━━━━━━━━━━━━━━━━━━━━━━\n");

            if (q.Contains("istanbul"))
            {
                sb.AppendLine("📍 İSTANBUL SEYAHATİ\n");
                sb.AppendLine("🏛️ Tarihi Yerler: Ayasofya, Topkapı Sarayı, Sultanahmet Camii");
                sb.AppendLine("🍽️ Yemekler: Kokoreç, Balık Ekmek, Künefe");
                sb.AppendLine("💰 Günlük Bütçe: ₺500-1000");
            }
            else if (q.Contains("paris"))
            {
                sb.AppendLine("📍 PARİS SEYAHATİ\n");
                sb.AppendLine("🗼 Görülecekler: Eyfel Kulesi, Louvre, Notre-Dame");
                sb.AppendLine("💰 Günlük Bütçe: €100-200");
            }
            else
            {
                sb.AppendLine("🌍 SEYAHATİ İPUÇLARI\n");
                sb.AppendLine("• Erken rezervasyon yapın");
                sb.AppendLine("• Yerel yemekleri deneyin");
                sb.AppendLine("• Müze kartı alın");
            }
            txtAIResponse.Text = sb.ToString();
        }

        #endregion

        #region Harita Sayfası (OpenStreetMap)

        private WebBrowser webMap;

        private void ShowMapPage()
        {
            SetActiveButton(btnTrips);
            ClearContent();
            currentPage = "map";

            pnlContent.Controls.Add(new Label { 
                Text = "🗺️ Gezilecek Yerler Haritası", 
                Font = new Font("Segoe UI", 24F, FontStyle.Bold), 
                ForeColor = Color.White, 
                BackColor = Color.Transparent, 
                Location = new Point(40, 20), 
                AutoSize = true 
            });

            var btnRefresh = CreateActionButton("🔄 Yenile", Color.FromArgb(102, 126, 234), 40, 70);
            btnRefresh.Click += (s, e) => RefreshMap();
            pnlContent.Controls.Add(btnRefresh);

            webMap = new WebBrowser
            {
                Location = new Point(40, 120),
                Size = new Size(pnlContent.Width - 80, pnlContent.Height - 160),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                ScriptErrorsSuppressed = true
            };
            pnlContent.Controls.Add(webMap);

            RefreshMap();
        }

        private void RefreshMap()
        {
            string placesJs = GetPlacesFromDatabase();
            string mapHtml = GetMapHtml(placesJs);
            webMap.DocumentText = mapHtml;
        }

        private string GetPlacesFromDatabase()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine("var places = [");
            int count = 0;
            
            try
            {
                using (var c = DatabaseHelper.Instance.GetConnection())
                {
                    c.Open();
                    // Daha basit sorgu - Latitude ve Longitude 0 olmayan kayıtları al
                    var cmd = new SQLiteCommand(@"
                        SELECT p.Name, COALESCE(p.Description, ''), p.Latitude, p.Longitude, c.Name 
                        FROM Places p 
                        INNER JOIN Cities c ON p.CityId = c.Id 
                        WHERE p.IsActive = 1 
                        AND p.Latitude != 0 
                        AND p.Longitude != 0", c);
                    
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string name = reader.GetString(0).Replace("'", "").Replace("\"", "");
                            string desc = reader.GetString(1).Replace("'", "").Replace("\"", "");
                            double lat = reader.GetDouble(2);
                            double lng = reader.GetDouble(3);
                            string city = reader.GetString(4).Replace("'", "").Replace("\"", "");
                            
                            sb.AppendLine($"  [{lat.ToString(System.Globalization.CultureInfo.InvariantCulture)}, {lng.ToString(System.Globalization.CultureInfo.InvariantCulture)}, '{name}', '{city} - {desc}'],");
                            count++;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Hata durumunda console'a yaz
                Console.WriteLine("Harita veri hatası: " + ex.Message);
            }

            // Veritabanında hiç yer yoksa varsayılan şehirleri göster
            if (count == 0)
            {
                sb.AppendLine("  [48.8566, 2.3522, 'Paris', 'Fransa - Eyfel Kulesi, Louvre Muzesi'],");
                sb.AppendLine("  [41.0082, 28.9784, 'Istanbul', 'Turkiye - Ayasofya, Topkapi Sarayi'],");
                sb.AppendLine("  [41.9028, 12.4964, 'Roma', 'Italya - Kolezyum, Vatikan'],");
            }
            
            // Son virgülü kaldır ve dizgiyi kapat
            sb.AppendLine("];");
            
            return sb.ToString();
        }

        private string GetMapHtml(string placesJs)
        {
            return $@"<!DOCTYPE html>
<html>
<head>
    <meta http-equiv='X-UA-Compatible' content='IE=edge'>
    <meta charset='utf-8'>
    <title>Harita</title>
    <link rel='stylesheet' href='https://unpkg.com/leaflet@1.9.4/dist/leaflet.css' />
    <style>
        * {{ margin: 0; padding: 0; }}
        html, body {{ width: 100%; height: 100%; }}
        #map {{ width: 100%; height: 100%; background: #1a1a2e; }}
        .leaflet-popup-content-wrapper {{ background: #2d3748; color: white; border-radius: 8px; }}
        .leaflet-popup-tip {{ background: #2d3748; }}
    </style>
</head>
<body>
    <div id='map'></div>
    <script src='https://unpkg.com/leaflet@1.9.4/dist/leaflet.js'></script>
    <script>
        try {{
            var map = L.map('map').setView([41.0082, 28.9784], 4);
            L.tileLayer('https://{{s}}.tile.openstreetmap.org/{{z}}/{{x}}/{{y}}.png', {{
                attribution: 'OSM',
                maxZoom: 18
            }}).addTo(map);

            {placesJs}

            for (var i = 0; i < places.length; i++) {{
                var p = places[i];
                if (p[0] && p[1]) {{
                    var marker = L.marker([p[0], p[1]]).addTo(map);
                    marker.bindPopup('<b>' + p[2] + '</b><br>' + p[3]);
                }}
            }}
        }} catch(e) {{
            document.body.innerHTML = '<h2 style=""color:white;text-align:center;padding:50px;background:#1a1a2e"">Harita Yuklenemedi</h2>';
        }}
    </script>
</body>
</html>";
        }

        #endregion

        #region Yardımcılar

        private Button CreateActionButton(string text, Color color, int x, int y)
        {
            return new Button { Text = text, Font = new Font("Segoe UI", 10F), BackColor = color, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Size = new Size(100, 40), Location = new Point(x, y), Cursor = Cursors.Hand, FlatAppearance = { BorderSize = 0 } };
        }

        private DataGridView CreateDataGrid(int x, int y, int w, int h)
        {
            var dgv = new DataGridView
            {
                Location = new Point(x, y), Size = new Size(w, h), BackgroundColor = Color.FromArgb(30, 40, 60),
                ForeColor = Color.White, GridColor = Color.FromArgb(50, 60, 80), BorderStyle = BorderStyle.None,
                RowHeadersVisible = false, AllowUserToAddRows = false, ReadOnly = true, SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, EnableHeadersVisualStyles = false
            };
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 50, 70);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            dgv.ColumnHeadersHeight = 45;
            dgv.DefaultCellStyle.BackColor = Color.FromArgb(30, 40, 60);
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(102, 126, 234);
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            dgv.RowTemplate.Height = 40;
            dgv.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            return dgv;
        }

        #endregion

        #region Pencere Sürükleme

        private void TitleBar_MouseDown(object sender, MouseEventArgs e) { if (e.Button == MouseButtons.Left) { isDragging = true; dragStartPoint = e.Location; } }
        private void TitleBar_MouseMove(object sender, MouseEventArgs e) { if (isDragging) { var p = PointToScreen(e.Location); this.Location = new Point(p.X - dragStartPoint.X, p.Y - dragStartPoint.Y); } }
        private void TitleBar_MouseUp(object sender, MouseEventArgs e) { isDragging = false; }

        #endregion

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Çıkış yapmak istediğinize emin misiniz?", "Çıkış", MessageBoxButtons.YesNo) == DialogResult.Yes)
                this.Close();
        }
    }
}
