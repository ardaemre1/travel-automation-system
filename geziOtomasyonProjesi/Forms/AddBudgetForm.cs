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
    /// Harcama Ekleme Formu
    /// </summary>
    public partial class AddBudgetForm : Form
    {
        private Panel pnlTitleBar;
        private Button btnClose;
        private Label lblTitle;
        private Panel pnlContent;
        
        private ComboBox cmbCategory;
        private TextBox txtAmount;
        private TextBox txtDescription;
        private DateTimePicker dtpDate;
        private CheckBox chkPaid;
        private Button btnSave;

        public bool IsSaved { get; private set; } = false;

        public AddBudgetForm()
        {
            InitializeComponent();
            ApplyModernStyle();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(450, 450);
            this.Name = "AddBudgetForm";
            this.Text = "Harcama Ekle";
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
            this.lblTitle.Text = "💰 Harcama/Bütçe Ekle";
            this.lblTitle.ForeColor = Color.White;
            this.lblTitle.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new Point(140, 12);
            this.pnlTitleBar.Controls.Add(this.lblTitle);

            this.Controls.Add(this.pnlTitleBar);

            // İçerik
            this.pnlContent = new Panel();
            this.pnlContent.BackColor = Color.Transparent;
            this.pnlContent.Dock = DockStyle.Fill;

            int yPos = 20;

            // Kategori
            var lblCategory = new Label();
            lblCategory.Text = "Kategori";
            lblCategory.Font = new Font("Segoe UI", 10F);
            lblCategory.ForeColor = Color.FromArgb(160, 174, 192);
            lblCategory.Location = new Point(30, yPos);
            lblCategory.AutoSize = true;
            this.pnlContent.Controls.Add(lblCategory);

            cmbCategory = new ComboBox();
            cmbCategory.Font = new Font("Segoe UI", 11F);
            cmbCategory.BackColor = Color.FromArgb(45, 55, 85);
            cmbCategory.ForeColor = Color.White;
            cmbCategory.FlatStyle = FlatStyle.Flat;
            cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbCategory.Size = new Size(390, 30);
            cmbCategory.Location = new Point(30, yPos + 25);
            cmbCategory.Items.AddRange(new string[] { "🍽️ Yemek", "🏨 Konaklama", "🚗 Ulaşım", "🎫 Giriş Ücreti", "🛍️ Alışveriş", "📱 Diğer" });
            cmbCategory.SelectedIndex = 0;
            this.pnlContent.Controls.Add(cmbCategory);
            yPos += 75;

            // Tutar
            var lblAmount = new Label();
            lblAmount.Text = "Tutar (₺)";
            lblAmount.Font = new Font("Segoe UI", 10F);
            lblAmount.ForeColor = Color.FromArgb(160, 174, 192);
            lblAmount.Location = new Point(30, yPos);
            lblAmount.AutoSize = true;
            this.pnlContent.Controls.Add(lblAmount);

            txtAmount = new TextBox();
            txtAmount.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            txtAmount.BackColor = Color.FromArgb(45, 55, 85);
            txtAmount.ForeColor = Color.FromArgb(72, 187, 120);
            txtAmount.BorderStyle = BorderStyle.FixedSingle;
            txtAmount.Size = new Size(390, 35);
            txtAmount.Location = new Point(30, yPos + 25);
            txtAmount.Text = "0";
            this.pnlContent.Controls.Add(txtAmount);
            yPos += 80;

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
            txtDescription.Size = new Size(390, 30);
            txtDescription.Location = new Point(30, yPos + 25);
            this.pnlContent.Controls.Add(txtDescription);
            yPos += 75;

            // Tarih
            var lblDate = new Label();
            lblDate.Text = "Tarih";
            lblDate.Font = new Font("Segoe UI", 10F);
            lblDate.ForeColor = Color.FromArgb(160, 174, 192);
            lblDate.Location = new Point(30, yPos);
            lblDate.AutoSize = true;
            this.pnlContent.Controls.Add(lblDate);

            dtpDate = new DateTimePicker();
            dtpDate.Font = new Font("Segoe UI", 11F);
            dtpDate.Format = DateTimePickerFormat.Short;
            dtpDate.Size = new Size(200, 30);
            dtpDate.Location = new Point(30, yPos + 25);
            this.pnlContent.Controls.Add(dtpDate);

            chkPaid = new CheckBox();
            chkPaid.Text = "Ödendi";
            chkPaid.Font = new Font("Segoe UI", 11F);
            chkPaid.ForeColor = Color.White;
            chkPaid.Location = new Point(260, yPos + 25);
            chkPaid.AutoSize = true;
            this.pnlContent.Controls.Add(chkPaid);
            yPos += 80;

            // Kaydet butonu
            this.btnSave = new Button();
            this.btnSave.Text = "💾 KAYDET";
            this.btnSave.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.btnSave.BackColor = Color.FromArgb(72, 187, 120);
            this.btnSave.ForeColor = Color.White;
            this.btnSave.FlatStyle = FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Size = new Size(390, 50);
            this.btnSave.Location = new Point(30, yPos);
            this.btnSave.Cursor = Cursors.Hand;
            this.btnSave.Click += BtnSave_Click;
            this.pnlContent.Controls.Add(this.btnSave);

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

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!decimal.TryParse(txtAmount.Text, out decimal amount) || amount <= 0)
            {
                MessageBox.Show("Geçerli bir tutar girin!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int userId = LoginForm.CurrentUser?.Id ?? 1;
                int categoryIndex = cmbCategory.SelectedIndex;

                using (var connection = DatabaseHelper.Instance.GetConnection())
                {
                    connection.Open();
                    var cmd = new SQLiteCommand(@"
                        INSERT INTO Budgets (UserId, Category, Amount, Description, ExpenseDate, IsPaid, Currency, CreatedAt, IsActive)
                        VALUES (@userId, @category, @amount, @description, @expenseDate, @isPaid, 'TRY', datetime('now'), 1)", connection);
                    
                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@category", categoryIndex);
                    cmd.Parameters.AddWithValue("@amount", amount);
                    cmd.Parameters.AddWithValue("@description", txtDescription.Text.Trim());
                    cmd.Parameters.AddWithValue("@expenseDate", dtpDate.Value.ToString("yyyy-MM-dd"));
                    cmd.Parameters.AddWithValue("@isPaid", chkPaid.Checked ? 1 : 0);
                    
                    cmd.ExecuteNonQuery();
                }

                IsSaved = true;
                MessageBox.Show("Harcama başarıyla eklendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kayıt hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
