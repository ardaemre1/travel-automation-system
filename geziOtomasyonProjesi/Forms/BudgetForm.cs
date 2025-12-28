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
    /// Bütçe Yönetimi Formu
    /// </summary>
    public partial class BudgetForm : Form
    {
        private Panel pnlTitleBar;
        private Button btnClose;
        private Label lblTitle;
        private Panel pnlContent;
        private Label lblPageTitle;
        private Label lblTotalBudget;
        private DataGridView dgvBudget;
        private Button btnAdd;

        public BudgetForm()
        {
            InitializeComponent();
            ApplyModernStyle();
            LoadBudget();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(900, 600);
            this.Name = "BudgetForm";
            this.Text = "Bütçe Yönetimi";
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
            this.lblTitle.Text = "Bütçe Yönetimi";
            this.lblTitle.ForeColor = Color.FromArgb(160, 174, 192);
            this.lblTitle.Font = new Font("Segoe UI", 11F);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new Point(400, 12);
            this.pnlTitleBar.Controls.Add(this.lblTitle);

            this.Controls.Add(this.pnlTitleBar);

            // İçerik
            this.pnlContent = new Panel();
            this.pnlContent.BackColor = Color.Transparent;
            this.pnlContent.Dock = DockStyle.Fill;

            this.lblPageTitle = new Label();
            this.lblPageTitle.Text = "💰 Bütçe Yönetimi";
            this.lblPageTitle.Font = new Font("Segoe UI", 22F, FontStyle.Bold);
            this.lblPageTitle.ForeColor = Color.White;
            this.lblPageTitle.Location = new Point(30, 20);
            this.lblPageTitle.AutoSize = true;
            this.pnlContent.Controls.Add(this.lblPageTitle);

            this.lblTotalBudget = new Label();
            this.lblTotalBudget.Text = "Toplam: ₺0";
            this.lblTotalBudget.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTotalBudget.ForeColor = Color.FromArgb(237, 137, 54);
            this.lblTotalBudget.Location = new Point(30, 70);
            this.lblTotalBudget.AutoSize = true;
            this.pnlContent.Controls.Add(this.lblTotalBudget);

            this.btnAdd = new Button();
            this.btnAdd.Text = "➕ Harcama Ekle";
            this.btnAdd.Font = new Font("Segoe UI", 10F);
            this.btnAdd.BackColor = Color.FromArgb(72, 187, 120);
            this.btnAdd.ForeColor = Color.White;
            this.btnAdd.FlatStyle = FlatStyle.Flat;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            this.btnAdd.Size = new Size(150, 40);
            this.btnAdd.Location = new Point(700, 70);
            this.btnAdd.Cursor = Cursors.Hand;
            this.btnAdd.Click += (s, args) => {
                var addForm = new AddBudgetForm();
                addForm.ShowDialog();
                if (addForm.IsSaved) LoadBudget();
            };
            this.pnlContent.Controls.Add(this.btnAdd);

            this.dgvBudget = new DataGridView();
            this.dgvBudget.Location = new Point(30, 130);
            this.dgvBudget.Size = new Size(840, 400);
            this.dgvBudget.BackgroundColor = Color.FromArgb(30, 40, 60);
            this.dgvBudget.ForeColor = Color.White;
            this.dgvBudget.GridColor = Color.FromArgb(50, 60, 80);
            this.dgvBudget.BorderStyle = BorderStyle.None;
            this.dgvBudget.RowHeadersVisible = false;
            this.dgvBudget.AllowUserToAddRows = false;
            this.dgvBudget.ReadOnly = true;
            this.dgvBudget.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvBudget.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvBudget.EnableHeadersVisualStyles = false;
            this.dgvBudget.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(40, 50, 70);
            this.dgvBudget.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvBudget.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.dgvBudget.ColumnHeadersHeight = 45;
            this.dgvBudget.DefaultCellStyle.BackColor = Color.FromArgb(30, 40, 60);
            this.dgvBudget.DefaultCellStyle.ForeColor = Color.White;
            this.dgvBudget.DefaultCellStyle.SelectionBackColor = Color.FromArgb(237, 137, 54);
            this.dgvBudget.DefaultCellStyle.Font = new Font("Segoe UI", 10F);
            this.dgvBudget.RowTemplate.Height = 40;
            this.pnlContent.Controls.Add(this.dgvBudget);

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

        private void LoadBudget()
        {
            try
            {
                int userId = LoginForm.CurrentUser?.Id ?? 0;
                
                using (var connection = DatabaseHelper.Instance.GetConnection())
                {
                    connection.Open();
                    
                    // Toplam bütçe
                    var totalCmd = new SQLiteCommand("SELECT COALESCE(SUM(Amount), 0) FROM Budgets WHERE UserId = @userId", connection);
                    totalCmd.Parameters.AddWithValue("@userId", userId);
                    var total = Convert.ToDecimal(totalCmd.ExecuteScalar());
                    lblTotalBudget.Text = $"Toplam: ₺{total:N2}";

                    // Liste (eğer veri varsa)
                    var cmd = new SQLiteCommand(@"
                        SELECT Id, Category, Amount, Description, ExpenseDate 
                        FROM Budgets 
                        WHERE UserId = @userId AND IsActive = 1 
                        ORDER BY ExpenseDate DESC", connection);
                    cmd.Parameters.AddWithValue("@userId", userId);
                    
                    var adapter = new SQLiteDataAdapter(cmd);
                    var dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Columns.Contains("Id")) dt.Columns["Id"].ColumnName = "ID";
                    if (dt.Columns.Contains("Category")) dt.Columns["Category"].ColumnName = "Kategori";
                    if (dt.Columns.Contains("Amount")) dt.Columns["Amount"].ColumnName = "Tutar (₺)";
                    if (dt.Columns.Contains("Description")) dt.Columns["Description"].ColumnName = "Açıklama";
                    if (dt.Columns.Contains("ExpenseDate")) dt.Columns["ExpenseDate"].ColumnName = "Tarih";

                    dgvBudget.DataSource = dt;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bütçe yüklenirken hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
