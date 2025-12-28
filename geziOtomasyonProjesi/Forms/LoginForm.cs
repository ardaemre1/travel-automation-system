using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using geziOtomasyonProjesi.Data;
using geziOtomasyonProjesi.Helpers;
using geziOtomasyonProjesi.Models;

namespace geziOtomasyonProjesi.Forms
{
    /// <summary>
    /// Modern Giriş Formu - Form sınıfından KALITIM örneği
    /// iOS tarzı başlık çubuğu ve responsive tasarım
    /// </summary>
    public partial class LoginForm : Form
    {
        // Pencere sürükleme için değişkenler
        private bool isDragging = false;
        private Point dragStartPoint;

        // Current user bilgisi (static - tüm formlardan erişilebilir)
        public static User CurrentUser { get; private set; }

        public LoginForm()
        {
            InitializeComponent();
            ApplyModernStyle();
            SetupEventHandlers();
        }

        /// <summary>
        /// Form yüklendiğinde
        /// </summary>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            CenterLoginPanel();
        }

        /// <summary>
        /// Form boyutu değiştiğinde
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            CenterLoginPanel();
            this.Invalidate(); // Gradient'i yeniden çiz
        }

        /// <summary>
        /// Modern stil uygula
        /// </summary>
        private void ApplyModernStyle()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = ColorPalette.DarkBackground;
            this.DoubleBuffered = true;
            this.MinimumSize = new Size(800, 600);
        }

        /// <summary>
        /// Login panelini ve diğer elementleri ortala
        /// </summary>
        private void CenterLoginPanel()
        {
            if (pnlMain == null || pnlLogin == null) return;

            int centerX = pnlMain.Width / 2;
            int centerY = pnlMain.Height / 2;

            // Logo ortala - daha yukarıda
            if (lblLogo != null)
            {
                lblLogo.Location = new Point(centerX - lblLogo.Width / 2, 10);
            }

            // Uygulama adı ortala
            if (lblAppName != null)
            {
                lblAppName.Location = new Point(centerX - lblAppName.Width / 2, 140);
            }

            // Alt başlık ortala
            if (lblSubtitle != null)
            {
                lblSubtitle.Location = new Point(centerX - lblSubtitle.Width / 2, 195);
            }

            // Login panelini hem yatay hem dikey ortala
            int loginX = centerX - pnlLogin.Width / 2;
            int loginY = Math.Max(250, centerY - pnlLogin.Height / 2 + 50);
            pnlLogin.Location = new Point(loginX, loginY);

            // Başlık çubuğundaki başlığı ortala
            if (lblTitle != null)
            {
                lblTitle.Location = new Point((this.ClientSize.Width - lblTitle.Width) / 2, 12);
            }
        }

        /// <summary>
        /// Gradient arka plan çiz
        /// </summary>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (LinearGradientBrush brush = new LinearGradientBrush(
                this.ClientRectangle,
                ColorPalette.DarkBackground,
                ColorPalette.DarkSurface,
                LinearGradientMode.ForwardDiagonal))
            {
                e.Graphics.FillRectangle(brush, this.ClientRectangle);
            }
        }

        /// <summary>
        /// Event handler'ları ayarla
        /// </summary>
        private void SetupEventHandlers()
        {
            // iOS tarzı butonlar
            btnClose.Click += (s, e) => Application.Exit();
            btnMinimize.Click += (s, e) => this.WindowState = FormWindowState.Minimized;
            btnMaximize.Click += (s, e) => 
            {
                if (this.WindowState == FormWindowState.Maximized)
                    this.WindowState = FormWindowState.Normal;
                else
                    this.WindowState = FormWindowState.Maximized;
            };

            // Pencere sürükleme (başlık çubuğundan)
            pnlTitleBar.MouseDown += TitleBar_MouseDown;
            pnlTitleBar.MouseMove += TitleBar_MouseMove;
            pnlTitleBar.MouseUp += TitleBar_MouseUp;
            lblTitle.MouseDown += TitleBar_MouseDown;
            lblTitle.MouseMove += TitleBar_MouseMove;
            lblTitle.MouseUp += TitleBar_MouseUp;

            // Buton hover efektleri
            SetupButtonHoverEffects();

            // Enter tuşu ile giriş
            txtPassword.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    BtnLogin_Click(s, e);
                    e.Handled = true;
                }
            };

            txtUsername.KeyPress += (s, e) =>
            {
                if (e.KeyChar == (char)Keys.Enter)
                {
                    txtPassword.Focus();
                    e.Handled = true;
                }
            };
        }

        /// <summary>
        /// Buton hover efektleri
        /// </summary>
        private void SetupButtonHoverEffects()
        {
            // Giriş butonu hover
            btnLogin.MouseEnter += (s, e) => btnLogin.BackColor = ColorPalette.ButtonHover;
            btnLogin.MouseLeave += (s, e) => btnLogin.BackColor = ColorPalette.Primary;

            // Kayıt linki hover
            lnkRegister.MouseEnter += (s, e) => lnkRegister.LinkColor = ColorPalette.Primary;
            lnkRegister.MouseLeave += (s, e) => lnkRegister.LinkColor = ColorPalette.TextMuted;

            // iOS butonlar hover
            btnClose.MouseEnter += (s, e) => btnClose.BackColor = Color.FromArgb(255, 120, 110);
            btnClose.MouseLeave += (s, e) => btnClose.BackColor = Color.FromArgb(255, 95, 87);
            btnMinimize.MouseEnter += (s, e) => btnMinimize.BackColor = Color.FromArgb(255, 210, 80);
            btnMinimize.MouseLeave += (s, e) => btnMinimize.BackColor = Color.FromArgb(255, 189, 46);
            btnMaximize.MouseEnter += (s, e) => btnMaximize.BackColor = Color.FromArgb(60, 220, 90);
            btnMaximize.MouseLeave += (s, e) => btnMaximize.BackColor = Color.FromArgb(40, 201, 64);
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

        #region Giriş İşlemleri

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text;

            // Doğrulama
            if (string.IsNullOrEmpty(username))
            {
                ShowMessage("Lütfen kullanıcı adınızı girin.", false);
                txtUsername.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                ShowMessage("Lütfen şifrenizi girin.", false);
                txtPassword.Focus();
                return;
            }

            // Giriş işlemi
            try
            {
                var user = DatabaseHelper.Instance.LoginUser(username, password);

                if (user != null)
                {
                    CurrentUser = user;
                    
                    // Dashboard'a geç
                    var dashboard = new DashboardForm();
                    dashboard.FormClosed += (s, args) => this.Close();
                    dashboard.Show();
                    this.Hide();
                }
                else
                {
                    ShowMessage("Kullanıcı adı veya şifre hatalı!", false);
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowMessage("Giriş hatası: " + ex.Message, false);
            }
        }

        private void LnkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var registerForm = new RegisterForm();
            registerForm.ShowDialog(this);
        }

        #endregion

        #region Yardımcı Metodlar

        private void ShowMessage(string message, bool isSuccess)
        {
            lblMessage.Text = message;
            lblMessage.ForeColor = isSuccess ? ColorPalette.Success : ColorPalette.Error;
            lblMessage.Visible = true;
        }

        #endregion
    }
}
