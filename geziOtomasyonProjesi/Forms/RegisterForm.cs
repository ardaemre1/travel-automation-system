using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using geziOtomasyonProjesi.Data;
using geziOtomasyonProjesi.Helpers;

namespace geziOtomasyonProjesi.Forms
{
    /// <summary>
    /// Kayıt Formu - Form sınıfından KALITIM örneği
    /// iOS tarzı başlık çubuğu
    /// </summary>
    public partial class RegisterForm : Form
    {
        // Pencere sürükleme için değişkenler
        private bool isDragging = false;
        private Point dragStartPoint;

        public RegisterForm()
        {
            InitializeComponent();
            ApplyModernStyle();
            SetupEventHandlers();
        }

        /// <summary>
        /// Modern stil uygula
        /// </summary>
        private void ApplyModernStyle()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.BackColor = ColorPalette.DarkBackground;
            this.DoubleBuffered = true;
            this.Paint += RegisterForm_Paint;
        }

        private void RegisterForm_Paint(object sender, PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                ColorPalette.DarkBackground,
                ColorPalette.DarkSurface,
                LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        private void SetupEventHandlers()
        {
            // iOS tarzı butonlar
            btnClose.Click += (s, e) => this.Close();

            // Pencere sürükleme
            pnlTitleBar.MouseDown += TitleBar_MouseDown;
            pnlTitleBar.MouseMove += TitleBar_MouseMove;
            pnlTitleBar.MouseUp += TitleBar_MouseUp;

            // Buton hover
            btnRegister.MouseEnter += (s, e) => btnRegister.BackColor = ColorPalette.ButtonHover;
            btnRegister.MouseLeave += (s, e) => btnRegister.BackColor = ColorPalette.Primary;

            lnkBack.MouseEnter += (s, e) => lnkBack.ForeColor = ColorPalette.Primary;
            lnkBack.MouseLeave += (s, e) => lnkBack.ForeColor = ColorPalette.TextMuted;
        }

        #region Pencere Sürükleme

        private void TitleBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragStartPoint = new Point(e.X, e.Y);
            }
        }

        private void TitleBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point p = PointToScreen(e.Location);
                this.Location = new Point(p.X - dragStartPoint.X, p.Y - dragStartPoint.Y);
            }
        }

        private void TitleBar_MouseUp(object sender, MouseEventArgs e)
        {
            isDragging = false;
        }

        #endregion

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            // Doğrulama
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                ShowError("Lütfen adınızı girin.");
                txtFullName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtUsername.Text) || txtUsername.Text.Length < 3)
            {
                ShowError("Kullanıcı adı en az 3 karakter olmalı.");
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtEmail.Text) || !txtEmail.Text.Contains("@"))
            {
                ShowError("Geçerli bir e-posta girin.");
                txtEmail.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text) || txtPassword.Text.Length < 4)
            {
                ShowError("Şifre en az 4 karakter olmalı.");
                txtPassword.Focus();
                return;
            }

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                ShowError("Şifreler eşleşmiyor!");
                txtConfirmPassword.Focus();
                return;
            }

            if (DatabaseHelper.Instance.UsernameExists(txtUsername.Text))
            {
                ShowError("Bu kullanıcı adı zaten kullanılıyor.");
                txtUsername.Focus();
                return;
            }

            try
            {
                bool success = DatabaseHelper.Instance.RegisterUser(
                    txtUsername.Text.Trim(),
                    txtPassword.Text,
                    txtEmail.Text.Trim(),
                    txtFullName.Text.Trim()
                );

                if (success)
                {
                    MessageBox.Show("Kayıt başarılı! Giriş yapabilirsiniz.", "Başarılı", 
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close();
                }
                else
                {
                    ShowError("Kayıt sırasında bir hata oluştu.");
                }
            }
            catch (Exception ex)
            {
                ShowError("Hata: " + ex.Message);
            }
        }

        private void LnkBack_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void ShowError(string message)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = ColorPalette.Error;
            lblMessage.Visible = true;
        }
    }
}
