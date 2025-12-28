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
    /// Gezilecek Yerler Formu
    /// </summary>
    public partial class PlacesForm : Form
    {
        private Panel pnlTitleBar;
        private Button btnClose;
        private Label lblTitle;
        private Panel pnlContent;
        private Label lblPageTitle;
        private DataGridView dgvPlaces;
        private Button btnRefresh;

        public PlacesForm()
        {
            InitializeComponent();
            ApplyModernStyle();
            LoadPlaces();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(1000, 650);
            this.Name = "PlacesForm";
            this.Text = "Gezilecek Yerler";
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
            this.lblTitle.Text = "Gezilecek Yerler";
            this.lblTitle.ForeColor = Color.FromArgb(160, 174, 192);
            this.lblTitle.Font = new Font("Segoe UI", 11F);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new Point(450, 12);
            this.pnlTitleBar.Controls.Add(this.lblTitle);

            this.Controls.Add(this.pnlTitleBar);

            // İçerik
            this.pnlContent = new Panel();
            this.pnlContent.BackColor = Color.Transparent;
            this.pnlContent.Dock = DockStyle.Fill;

            this.lblPageTitle = new Label();
            this.lblPageTitle.Text = "📍 Gezilecek Yerler";
            this.lblPageTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            this.lblPageTitle.ForeColor = Color.White;
            this.lblPageTitle.Location = new Point(30, 20);
            this.lblPageTitle.AutoSize = true;
            this.pnlContent.Controls.Add(this.lblPageTitle);

            this.btnRefresh = new Button();
            this.btnRefresh.Text = "🔄 Yenile";
            this.btnRefresh.Font = new Font("Segoe UI", 10F);
            this.btnRefresh.BackColor = Color.FromArgb(118, 75, 162);
            this.btnRefresh.ForeColor = Color.White;
            this.btnRefresh.FlatStyle = FlatStyle.Flat;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.Size = new Size(120, 40);
            this.btnRefresh.Location = new Point(30, 75);
            this.btnRefresh.Cursor = Cursors.Hand;
            this.btnRefresh.Click += (s, e) => LoadPlaces();
            this.pnlContent.Controls.Add(this.btnRefresh);

            this.dgvPlaces = new DataGridView();
            this.dgvPlaces.Location = new Point(30, 130);
            this.dgvPlaces.Size = new Size(940, 450);
            this.dgvPlaces.BackgroundColor = Color.FromArgb(30, 40, 60);
            this.dgvPlaces.ForeColor = Color.White;
            this.dgvPlaces.GridColor = Color.FromArgb(50, 60, 80);
            this.dgvPlaces.BorderStyle = BorderStyle.None;
            this.dgvPlaces.RowHeadersVisible = false;
            this.dgvPlaces.AllowUserToAddRows = false;
            this.dgvPlaces.ReadOnly = true;
            this.dgvPlaces.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvPlaces.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPlaces.EnableHeadersVisualStyles = false;
            this.dgvPlaces.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 50, 70);
            this.dgvPlaces.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvPlaces.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvPlaces.ColumnHeadersHeight = 45;
            this.dgvPlaces.DefaultCellStyle.BackColor = Color.FromArgb(30, 40, 60);
            this.dgvPlaces.DefaultCellStyle.ForeColor = Color.White;
            this.dgvPlaces.DefaultCellStyle.SelectionBackColor = Color.FromArgb(118, 75, 162);
            this.dgvPlaces.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            this.dgvPlaces.RowTemplate.Height = 40;
            this.pnlContent.Controls.Add(this.dgvPlaces);

            this.Controls.Add(this.pnlContent);
            this.pnlContent.BringToFront();
            this.ResumeLayout(false);
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

        private void LoadPlaces()
        {
            try
            {
                using (var connection = DatabaseHelper.Instance.GetConnection())
                {
                    connection.Open();
                    var cmd = new SQLiteCommand(@"
                        SELECT p.Id, p.Name, c.Name as CityName, p.EntryFee, p.Rating 
                        FROM Places p 
                        INNER JOIN Cities c ON p.CityId = c.Id 
                        WHERE p.IsActive = 1", connection);
                    var adapter = new SQLiteDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);

                    dt.Columns["Id"].ColumnName = "ID";
                    dt.Columns["Name"].ColumnName = "Yer Adı";
                    dt.Columns["CityName"].ColumnName = "Şehir";
                    dt.Columns["EntryFee"].ColumnName = "Giriş Ücreti (₺)";
                    dt.Columns["Rating"].ColumnName = "Puan";

                    dgvPlaces.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Yerler yüklenirken hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
