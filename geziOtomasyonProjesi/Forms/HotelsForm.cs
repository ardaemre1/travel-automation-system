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
    /// Oteller Formu
    /// </summary>
    public partial class HotelsForm : Form
    {
        private Panel pnlTitleBar;
        private Button btnClose;
        private Label lblTitle;
        private Panel pnlContent;
        private Label lblPageTitle;
        private DataGridView dgvHotels;
        private Button btnRefresh;

        public HotelsForm()
        {
            InitializeComponent();
            ApplyModernStyle();
            LoadHotels();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(1000, 650);
            this.Name = "HotelsForm";
            this.Text = "Oteller";
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
            this.lblTitle.Text = "Oteller";
            this.lblTitle.ForeColor = Color.FromArgb(160, 174, 192);
            this.lblTitle.Font = new Font("Segoe UI", 11F);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new Point(470, 12);
            this.pnlTitleBar.Controls.Add(this.lblTitle);

            this.Controls.Add(this.pnlTitleBar);

            // İçerik
            this.pnlContent = new Panel();
            this.pnlContent.BackColor = Color.Transparent;
            this.pnlContent.Dock = DockStyle.Fill;

            this.lblPageTitle = new Label();
            this.lblPageTitle.Text = "🏨 Oteller";
            this.lblPageTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            this.lblPageTitle.ForeColor = Color.White;
            this.lblPageTitle.Location = new Point(30, 20);
            this.lblPageTitle.AutoSize = true;
            this.pnlContent.Controls.Add(this.lblPageTitle);

            this.btnRefresh = new Button();
            this.btnRefresh.Text = "🔄 Yenile";
            this.btnRefresh.Font = new Font("Segoe UI", 10F);
            this.btnRefresh.BackColor = Color.FromArgb(72, 187, 120);
            this.btnRefresh.ForeColor = Color.White;
            this.btnRefresh.FlatStyle = FlatStyle.Flat;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.Size = new Size(120, 40);
            this.btnRefresh.Location = new Point(30, 75);
            this.btnRefresh.Cursor = Cursors.Hand;
            this.btnRefresh.Click += (s, e) => LoadHotels();
            this.pnlContent.Controls.Add(this.btnRefresh);

            this.dgvHotels = new DataGridView();
            this.dgvHotels.Location = new Point(30, 130);
            this.dgvHotels.Size = new Size(940, 450);
            this.dgvHotels.BackgroundColor = Color.FromArgb(30, 40, 60);
            this.dgvHotels.ForeColor = Color.White;
            this.dgvHotels.GridColor = Color.FromArgb(50, 60, 80);
            this.dgvHotels.BorderStyle = BorderStyle.None;
            this.dgvHotels.RowHeadersVisible = false;
            this.dgvHotels.AllowUserToAddRows = false;
            this.dgvHotels.ReadOnly = true;
            this.dgvHotels.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvHotels.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHotels.EnableHeadersVisualStyles = false;
            this.dgvHotels.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 50, 70);
            this.dgvHotels.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvHotels.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvHotels.ColumnHeadersHeight = 45;
            this.dgvHotels.DefaultCellStyle.BackColor = Color.FromArgb(30, 40, 60);
            this.dgvHotels.DefaultCellStyle.ForeColor = Color.White;
            this.dgvHotels.DefaultCellStyle.SelectionBackColor = Color.FromArgb(72, 187, 120);
            this.dgvHotels.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            this.dgvHotels.RowTemplate.Height = 40;
            this.pnlContent.Controls.Add(this.dgvHotels);

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

        private void LoadHotels()
        {
            try
            {
                using (var connection = DatabaseHelper.Instance.GetConnection())
                {
                    connection.Open();
                    var cmd = new SQLiteCommand(@"
                        SELECT h.Id, h.Name, c.Name as CityName, h.Stars, h.PricePerNight, h.Rating 
                        FROM Hotels h 
                        INNER JOIN Cities c ON h.CityId = c.Id 
                        WHERE h.IsActive = 1", connection);
                    var adapter = new SQLiteDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);

                    dt.Columns["Id"].ColumnName = "ID";
                    dt.Columns["Name"].ColumnName = "Otel Adı";
                    dt.Columns["CityName"].ColumnName = "Şehir";
                    dt.Columns["Stars"].ColumnName = "Yıldız";
                    dt.Columns["PricePerNight"].ColumnName = "Gecelik (₺)";
                    dt.Columns["Rating"].ColumnName = "Puan";

                    dgvHotels.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Oteller yüklenirken hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
