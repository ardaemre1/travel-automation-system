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
    /// Gezilecek Yer Ekleme Formu - Koordinat desteği ile
    /// </summary>
    public partial class AddPlaceForm : Form
    {
        private Panel pnlTitleBar;
        private Button btnClose;
        private Label lblTitle;
        private Panel pnlContent;
        
        private ComboBox cmbCity;
        private TextBox txtName;
        private TextBox txtDescription;
        private TextBox txtLatitude;
        private TextBox txtLongitude;
        private TextBox txtEntryFee;
        private ComboBox cmbCategory;
        private Button btnSave;

        public bool IsSaved { get; private set; } = false;

        public AddPlaceForm()
        {
            InitializeComponent();
            ApplyModernStyle();
            LoadCities();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(500, 620);
            this.Name = "AddPlaceForm";
            this.Text = "Yeni Yer Ekle";
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;

            // Başlık çubuğu
            this.pnlTitleBar = new Panel();
            this.pnlTitleBar.BackColor = Color.FromArgb(22, 33, 62);
            this.pnlTitleBar.Dock = DockStyle.Top;
            this.pnlTitleBar.Height = 45;

            this.btnClose = new Button();
            this.btnClose.BackColor = Color.FromArgb(255, 95, 87);
            this.btnClose.FlatStyle = FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Size = new Size(16, 16);
            this.btnClose.Location = new Point(20, 14);
            this.btnClose.Cursor = Cursors.Hand;
            var path = new GraphicsPath();
            path.AddEllipse(0, 0, 16, 16);
            this.btnClose.Region = new Region(path);
            this.btnClose.Click += (s, e) => this.Close();
            this.pnlTitleBar.Controls.Add(this.btnClose);

            this.lblTitle = new Label();
            this.lblTitle.Text = "📍 Yeni Gezilecek Yer Ekle";
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new Point(150, 12);
            this.pnlTitleBar.Controls.Add(this.lblTitle);

            this.Controls.Add(this.pnlTitleBar);

            // İçerik
            this.pnlContent = new Panel();
            this.pnlContent.BackColor = Color.Transparent;
            this.pnlContent.Dock = DockStyle.Fill;

            int yPos = 20;

            // Şehir seçimi
            AddLabel("Şehir *", yPos);
            cmbCity = new ComboBox();
            cmbCity.Font = new Font("Segoe UI", 11F);
            cmbCity.BackColor = Color.FromArgb(45, 55, 85);
            cmbCity.ForeColor = Color.White;
            cmbCity.FlatStyle = FlatStyle.Flat;
            cmbCity.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCity.Size = new Size(440, 30);
            cmbCity.Location = new Point(30, yPos + 25);
            this.pnlContent.Controls.Add(cmbCity);
            yPos += 70;

            // Yer adı
            AddLabel("Yer Adı *", yPos);
            txtName = CreateTextBox(yPos);
            this.pnlContent.Controls.Add(txtName);
            yPos += 70;

            // Kategori
            AddLabel("Kategori", yPos);
            cmbCategory = new ComboBox();
            cmbCategory.Font = new Font("Segoe UI", 11F);
            cmbCategory.BackColor = Color.FromArgb(45, 55, 85);
            cmbCategory.ForeColor = Color.White;
            cmbCategory.FlatStyle = FlatStyle.Flat;
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.Size = new Size(440, 30);
            cmbCategory.Location = new Point(30, yPos + 25);
            cmbCategory.Items.AddRange(new string[] { "🏛️ Tarihi", "🏞️ Doğal", "🎨 Müze", "🛕 Dini", "🎢 Eğlence", "🍽️ Yeme-İçme", "🛍️ Alışveriş" });
            cmbCategory.SelectedIndex = 0;
            this.pnlContent.Controls.Add(cmbCategory);
            yPos += 70;

            // Koordinatlar (yan yana)
            AddLabel("Enlem (Latitude) *", yPos);
            txtLatitude = new TextBox();
            txtLatitude.Font = new Font("Segoe UI", 11F);
            txtLatitude.BackColor = Color.FromArgb(45, 55, 85);
            txtLatitude.ForeColor = Color.FromArgb(72, 187, 120);
            txtLatitude.BorderStyle = BorderStyle.FixedSingle;
            txtLatitude.Size = new Size(200, 30);
            txtLatitude.Location = new Point(30, yPos + 25);
            txtLatitude.Text = "41.0082";
            this.pnlContent.Controls.Add(txtLatitude);

            var lblLng = new Label();
            lblLng.Text = "Boylam (Longitude) *";
            lblLng.Font = new Font("Segoe UI", 10F);
            lblLng.ForeColor = Color.FromArgb(160, 174, 192);
            lblLng.Location = new Point(250, yPos);
            lblLng.AutoSize = true;
            this.pnlContent.Controls.Add(lblLng);

            txtLongitude = new TextBox();
            txtLongitude.Font = new Font("Segoe UI", 11F);
            txtLongitude.BackColor = Color.FromArgb(45, 55, 85);
            txtLongitude.ForeColor = Color.FromArgb(72, 187, 120);
            txtLongitude.BorderStyle = BorderStyle.FixedSingle;
            txtLongitude.Size = new Size(200, 30);
            txtLongitude.Location = new Point(250, yPos + 25);
            txtLongitude.Text = "28.9784";
            this.pnlContent.Controls.Add(txtLongitude);
            yPos += 70;

            // Giriş ücreti
            AddLabel("Giriş Ücreti (₺)", yPos);
            txtEntryFee = CreateTextBox(yPos);
            txtEntryFee.Text = "0";
            yPos += 70;

            // Açıklama
            AddLabel("Açıklama", yPos);
            txtDescription = new TextBox();
            txtDescription.Font = new Font("Segoe UI", 11F);
            txtDescription.BackColor = Color.FromArgb(45, 55, 85);
            txtDescription.ForeColor = Color.White;
            txtDescription.BorderStyle = BorderStyle.FixedSingle;
            txtDescription.Multiline = true;
            txtDescription.Size = new Size(440, 60);
            txtDescription.Location = new Point(30, yPos + 25);
            this.pnlContent.Controls.Add(txtDescription);
            yPos += 100;

            // Kaydet butonu
            this.btnSave = new Button();
            this.btnSave.Text = "💾 KAYDET";
            this.btnSave.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.btnSave.BackColor = Color.FromArgb(72, 187, 120);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Size = new Size(440, 50);
            this.btnSave.Location = new Point(30, yPos);
            this.btnSave.Cursor = Cursors.Hand;
            this.btnSave.Click += BtnSave_Click;
            this.pnlContent.Controls.Add(this.btnSave);

            this.Controls.Add(this.pnlContent);
            this.pnlContent.BringToFront();
            this.ResumeLayout(false);
        }

        private void AddLabel(string text, int yPos)
        {
            var lbl = new Label();
            lbl.Text = text;
            lbl.Font = new Font("Segoe UI", 10F);
            lbl.ForeColor = Color.FromArgb(160, 174, 192);
            lbl.Location = new Point(30, yPos);
            lbl.AutoSize = true;
            this.pnlContent.Controls.Add(lbl);
        }

        private TextBox CreateTextBox(int yPos)
        {
            var txt = new TextBox();
            txt.Font = new Font("Segoe UI", 11F);
            txt.BackColor = Color.FromArgb(45, 55, 85);
            txt.ForeColor = Color.White;
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.Size = new Size(440, 30);
            txt.Location = new Point(30, yPos + 25);
            this.pnlContent.Controls.Add(txt);
            return txt;
        }

        private void ApplyModernStyle()
        {
            this.BackColor = ColorPalette.DarkBackground;
            this.Paint += (s, e) =>
            {
                using (var brush = new LinearGradientBrush(ClientRectangle,
                    ColorPalette.DarkBackground, ColorPalette.DarkSurface, LinearGradientMode.ForwardDiagonal))
                {
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
            };
        }

        private void LoadCities()
        {
            try
            {
                using (var connection = DatabaseHelper.Instance.GetConnection())
                {
                    connection.Open();
                    var cmd = new SQLiteCommand("SELECT Id, Name FROM Cities WHERE IsActive = 1 ORDER BY Name", connection);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cmbCity.Items.Add(new ComboBoxItem(reader.GetInt32(0), reader.GetString(1)));
                        }
                    }
                }
                if (cmbCity.Items.Count > 0) cmbCity.SelectedIndex = 0;
            }
            catch { }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (cmbCity.SelectedItem == null || string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Şehir ve yer adı zorunludur!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!double.TryParse(txtLatitude.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double lat) ||
                !double.TryParse(txtLongitude.Text, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out double lng))
            {
                MessageBox.Show("Geçerli koordinatlar girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            decimal.TryParse(txtEntryFee.Text, out decimal entryFee);

            try
            {
                var cityItem = (ComboBoxItem)cmbCity.SelectedItem;

                using (var connection = DatabaseHelper.Instance.GetConnection())
                {
                    connection.Open();
                    var cmd = new SQLiteCommand(@"
                        INSERT INTO Places (CityId, Name, Description, Category, Latitude, Longitude, EntryFee, Rating, CreatedAt, IsActive)
                        VALUES (@cityId, @name, @description, @category, @lat, @lng, @fee, 0, datetime('now'), 1)", connection);
                    
                    cmd.Parameters.AddWithValue("@cityId", cityItem.Id);
                    cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@description", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@category", cmbCategory.SelectedIndex);
                    cmd.Parameters.AddWithValue("@lat", lat);
                    cmd.Parameters.AddWithValue("@lng", lng);
                    cmd.Parameters.AddWithValue("@fee", entryFee);
                    
                    cmd.ExecuteNonQuery();
                }

                IsSaved = true;
                MessageBox.Show("Yer başarıyla eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ComboBox için özel sınıf
        private class ComboBoxItem
        {
            public int Id { get; }
            public string Name { get; }
            public ComboBoxItem(int id, string name) { Id = id; Name = name; }
            public override string ToString() => Name;
        }
    }
}
