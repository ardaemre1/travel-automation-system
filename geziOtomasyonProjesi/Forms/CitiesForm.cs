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
    /// Şehirler Formu - Şehir listesi ve yönetimi
    /// </summary>
    public partial class CitiesForm : Form
    {
        private Panel pnlTitleBar;
        private Button btnClose;
        private Label lblTitle;
        private Panel pnlContent;
        private Label lblPageTitle;
        private DataGridView dgvCities;
        private Panel pnlActions;
        private Button btnRefresh;

        public CitiesForm()
        {
            InitializeComponent();
            ApplyModernStyle();
            LoadCities();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(900, 600);
            this.Name = "CitiesForm";
            this.Text = "Şehirler";
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
            this.lblTitle.Text = "Şehirler";
            this.lblTitle.ForeColor = Color.FromArgb(160, 174, 192);
            this.lblTitle.Font = new Font("Segoe UI", 11F);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new Point(420, 12);
            this.pnlTitleBar.Controls.Add(this.lblTitle);

            this.Controls.Add(this.pnlTitleBar);

            // İçerik
            this.pnlContent = new Panel();
            this.pnlContent.BackColor = Color.Transparent;
            this.pnlContent.Dock = DockStyle.Fill;
            this.pnlContent.Padding = new Padding(30);

            // Sayfa başlığı
            this.lblPageTitle = new Label();
            this.lblPageTitle.Text = "🌍 Şehirler";
            this.lblPageTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            this.lblPageTitle.ForeColor = Color.White;
            this.lblPageTitle.Location = new Point(30, 20);
            this.lblPageTitle.AutoSize = true;
            this.pnlContent.Controls.Add(this.lblPageTitle);

            // Aksiyonlar paneli
            this.pnlActions = new Panel();
            this.pnlActions.BackColor = Color.Transparent;
            this.pnlActions.Location = new Point(30, 70);
            this.pnlActions.Size = new Size(840, 50);

            this.btnRefresh = new Button();
            this.btnRefresh.Text = "🔄 Yenile";
            this.btnRefresh.Font = new Font("Segoe UI", 10F);
            this.btnRefresh.BackColor = Color.FromArgb(102, 126, 234);
            this.btnRefresh.ForeColor = Color.White;
            this.btnRefresh.FlatStyle = FlatStyle.Flat;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.Size = new Size(120, 40);
            this.btnRefresh.Location = new Point(0, 5);
            this.btnRefresh.Cursor = Cursors.Hand;
            this.btnRefresh.Click += (s, e) => LoadCities();
            this.pnlActions.Controls.Add(this.btnRefresh);

            // Ekle butonu
            var btnAdd = new Button();
            btnAdd.Text = "➕ Ekle";
            btnAdd.Font = new Font("Segoe UI", 10F);
            btnAdd.BackColor = Color.FromArgb(72, 187, 120);
            btnAdd.ForeColor = Color.White;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Size = new Size(100, 40);
            btnAdd.Location = new Point(130, 5);
            btnAdd.Cursor = Cursors.Hand;
            btnAdd.Click += (s, e) => { 
                var addForm = new AddCityForm();
                addForm.ShowDialog();
                if (addForm.IsSaved) LoadCities();
            };
            this.pnlActions.Controls.Add(btnAdd);

            // Sil butonu
            var btnDelete = new Button();
            btnDelete.Text = "🗑️ Sil";
            btnDelete.Font = new Font("Segoe UI", 10F);
            btnDelete.BackColor = Color.FromArgb(245, 101, 101);
            btnDelete.ForeColor = Color.White;
            btnDelete.FlatStyle = FlatStyle.Flat;
            btnDelete.FlatAppearance.BorderSize = 0;
            btnDelete.Size = new Size(100, 40);
            btnDelete.Location = new Point(240, 5);
            btnDelete.Cursor = Cursors.Hand;
            btnDelete.Click += BtnDelete_Click;
            this.pnlActions.Controls.Add(btnDelete);

            this.pnlContent.Controls.Add(this.pnlActions);

            // DataGridView
            this.dgvCities = new DataGridView();
            this.dgvCities.Location = new Point(30, 130);
            this.dgvCities.Size = new Size(840, 400);
            this.dgvCities.BackgroundColor = Color.FromArgb(30, 40, 60);
            this.dgvCities.ForeColor = Color.White;
            this.dgvCities.GridColor = Color.FromArgb(50, 60, 80);
            this.dgvCities.BorderStyle = BorderStyle.None;
            this.dgvCities.RowHeadersVisible = false;
            this.dgvCities.AllowUserToAddRows = false;
            this.dgvCities.AllowUserToDeleteRows = false;
            this.dgvCities.ReadOnly = true;
            this.dgvCities.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCities.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCities.EnableHeadersVisualStyles = false;
            this.dgvCities.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 50, 70);
            this.dgvCities.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvCities.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvCities.ColumnHeadersHeight = 45;
            this.dgvCities.DefaultCellStyle.BackColor = Color.FromArgb(30, 40, 60);
            this.dgvCities.DefaultCellStyle.ForeColor = Color.White;
            this.dgvCities.DefaultCellStyle.SelectionBackColor = Color.FromArgb(102, 126, 234);
            this.dgvCities.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            this.dgvCities.RowTemplate.Height = 40;
            this.pnlContent.Controls.Add(this.dgvCities);

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

        private void LoadCities()
        {
            try
            {
                using (var connection = DatabaseHelper.Instance.GetConnection())
                {
                    connection.Open();
                    var cmd = new SQLiteCommand("SELECT Id, Name, Country, Region, Currency, Language FROM Cities WHERE IsActive = 1", connection);
                    var adapter = new SQLiteDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);

                    // Kolon isimlerini Türkçeleştir
                    dt.Columns["Id"].ColumnName = "ID";
                    dt.Columns["Name"].ColumnName = "Şehir";
                    dt.Columns["Country"].ColumnName = "Ülke";
                    dt.Columns["Region"].ColumnName = "Bölge";
                    dt.Columns["Currency"].ColumnName = "Para Birimi";
                    dt.Columns["Language"].ColumnName = "Dil";

                    dgvCities.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Şehirler yüklenirken hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dgvCities.SelectedRows.Count == 0)
            {
                MessageBox.Show("Lütfen silmek istediğiniz şehri seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var row = dgvCities.SelectedRows[0];
            int cityId = Convert.ToInt32(row.Cells["ID"].Value);
            string cityName = row.Cells["Şehir"].Value.ToString();

            if (MessageBox.Show($"'{cityName}' şehrini silmek istediğinize emin misiniz?", "Silme Onayı", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    using (var connection = DatabaseHelper.Instance.GetConnection())
                    {
                        connection.Open();
                        var cmd = new SQLiteCommand("UPDATE Cities SET IsActive = 0 WHERE Id = @id", connection);
                        cmd.Parameters.AddWithValue("@id", cityId);
                        cmd.ExecuteNonQuery();
                    }
                    LoadCities();
                    MessageBox.Show("Şehir başarıyla silindi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
