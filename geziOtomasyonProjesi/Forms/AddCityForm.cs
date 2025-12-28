using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using geziOtomasyonProjesi.Data;
using geziOtomasyonProjesi.Helpers;
using System.Data.SQLite;

namespace geziOtomasyonProjesi.Forms
{
    /// <summary>
    /// Şehir Ekleme/Düzenleme Formu
    /// </summary>
    public partial class AddCityForm : Form
    {
        private Panel pnlTitleBar;
        private Button btnClose;
        private Label lblTitle;
        private Panel pnlContent;
        
        private TextBox txtName;
        private TextBox txtCountry;
        private TextBox txtRegion;
        private TextBox txtCurrency;
        private TextBox txtLanguage;
        private TextBox txtDescription;
        private Button btnSave;

        public bool IsSaved { get; private set; } = false;

        public AddCityForm()
        {
            InitializeComponent();
            ApplyModernStyle();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(500, 550);
            this.Name = "AddCityForm";
            this.Text = "Yeni Şehir Ekle";
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
            this.lblTitle.Text = "🌍 Yeni Şehir Ekle";
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new Point(180, 12);
            this.pnlTitleBar.Controls.Add(this.lblTitle);

            this.Controls.Add(this.pnlTitleBar);

            // İçerik
            this.pnlContent = new Panel();
            this.pnlContent.BackColor = Color.Transparent;
            this.pnlContent.Dock = DockStyle.Fill;

            int yPos = 20;
            int spacing = 75;

            CreateInputField("Şehir Adı *", ref txtName, ref yPos, spacing);
            CreateInputField("Ülke *", ref txtCountry, ref yPos, spacing);
            CreateInputField("Bölge", ref txtRegion, ref yPos, spacing);
            CreateInputField("Para Birimi", ref txtCurrency, ref yPos, spacing);
            CreateInputField("Dil", ref txtLanguage, ref yPos, spacing);

            // Açıklama
            var lblDesc = new Label();
            lblDesc.Text = "Açıklama";
            lblDesc.Font = new Font("Segoe UI", 10F);
            lblDesc.ForeColor = Color.FromArgb(160, 174, 192);
            lblDesc.Location = new Point(30, yPos);
            lblDesc.AutoSize = true;
            this.pnlContent.Controls.Add(lblDesc);

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

        private void CreateInputField(string labelText, ref TextBox textBox, ref int yPos, int spacing)
        {
            var lbl = new Label();
            lbl.Text = labelText;
            lbl.Font = new Font("Segoe UI", 10F);
            lbl.ForeColor = Color.FromArgb(160, 174, 192);
            lbl.Location = new Point(30, yPos);
            lbl.AutoSize = true;
            this.pnlContent.Controls.Add(lbl);

            textBox = new TextBox();
            textBox.Font = new Font("Segoe UI", 11F);
            textBox.BackColor = Color.FromArgb(45, 55, 85);
            textBox.ForeColor = Color.White;
            textBox.BorderStyle = BorderStyle.FixedSingle;
            textBox.Size = new Size(440, 30);
            textBox.Location = new Point(30, yPos + 25);
            this.pnlContent.Controls.Add(textBox);

            yPos += spacing;
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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtCountry.Text))
            {
                MessageBox.Show("Şehir adı ve ülke zorunludur!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                using (var connection = DatabaseHelper.Instance.GetConnection())
                {
                    connection.Open();
                    var cmd = new SQLiteCommand(@"
                        INSERT INTO Cities (Name, Country, Region, Currency, Language, Description, CreatedAt, IsActive)
                        VALUES (@name, @country, @region, @currency, @language, @description, datetime('now'), 1)", connection);
                    
                    cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@country", txtCountry.Text.Trim());
                    cmd.Parameters.AddWithValue("@region", txtRegion.Text.Trim());
                    cmd.Parameters.AddWithValue("@currency", txtCurrency.Text.Trim());
                    cmd.Parameters.AddWithValue("@language", txtLanguage.Text.Trim());
                    cmd.Parameters.AddWithValue("@description", txtDescription.Text.Trim());
                    
                    cmd.ExecuteNonQuery();
                }

                IsSaved = true;
                MessageBox.Show("Şehir başarıyla eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
