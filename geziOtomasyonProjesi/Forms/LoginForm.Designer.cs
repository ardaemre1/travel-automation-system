namespace geziOtomasyonProjesi.Forms
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

        // Kontroller
        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnMaximize;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Panel pnlLogin;
        private System.Windows.Forms.Label lblLoginTitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.LinkLabel lnkRegister;
        private System.Windows.Forms.Label lblMessage;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 650);
            this.Name = "LoginForm";
            this.Text = "Seyahat Planlayıcı";
            this.DoubleBuffered = true;

            // ==================== BAŞLIK ÇUBUĞU (iOS Tarzı) ====================
            this.pnlTitleBar = new System.Windows.Forms.Panel();
            this.pnlTitleBar.BackColor = System.Drawing.Color.FromArgb(22, 33, 62);
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.Height = 45;
            this.pnlTitleBar.Name = "pnlTitleBar";
            this.pnlTitleBar.Cursor = System.Windows.Forms.Cursors.SizeAll;

            // Kapatma butonu (macOS kırmızı)
            this.btnClose = new System.Windows.Forms.Button();
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(255, 95, 87);
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Size = new System.Drawing.Size(16, 16);
            this.btnClose.Location = new System.Drawing.Point(20, 14);
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Name = "btnClose";
            this.btnClose.TabStop = false;
            RoundButton(this.btnClose);
            this.pnlTitleBar.Controls.Add(this.btnClose);

            // Küçültme butonu (macOS sarı)
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnMinimize.BackColor = System.Drawing.Color.FromArgb(255, 189, 46);
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.Size = new System.Drawing.Size(16, 16);
            this.btnMinimize.Location = new System.Drawing.Point(46, 14);
            this.btnMinimize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.TabStop = false;
            RoundButton(this.btnMinimize);
            this.pnlTitleBar.Controls.Add(this.btnMinimize);

            // Büyütme butonu (macOS yeşil)
            this.btnMaximize = new System.Windows.Forms.Button();
            this.btnMaximize.BackColor = System.Drawing.Color.FromArgb(40, 201, 64);
            this.btnMaximize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMaximize.FlatAppearance.BorderSize = 0;
            this.btnMaximize.Size = new System.Drawing.Size(16, 16);
            this.btnMaximize.Location = new System.Drawing.Point(72, 14);
            this.btnMaximize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMaximize.Name = "btnMaximize";
            this.btnMaximize.TabStop = false;
            RoundButton(this.btnMaximize);
            this.pnlTitleBar.Controls.Add(this.btnMaximize);

            // Başlık etiketi (ortada)
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTitle.Text = "Seyahat Planlayıcı";
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular);
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.pnlTitleBar.Controls.Add(this.lblTitle);

            this.Controls.Add(this.pnlTitleBar);

            // ==================== ANA İÇERİK PANELİ ====================
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlMain.BackColor = System.Drawing.Color.Transparent;
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Name = "pnlMain";

            // Logo - Uçak ikonu (metin olarak)
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblLogo.Text = "✈";
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI Symbol", 80F);
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(102, 126, 234);
            this.lblLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblLogo.AutoSize = true;
            this.lblLogo.Name = "lblLogo";
            this.pnlMain.Controls.Add(this.lblLogo);

            // Uygulama adı
            this.lblAppName = new System.Windows.Forms.Label();
            this.lblAppName.Text = "Seyahat Rotası Planlayıcı";
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI", 32F, System.Drawing.FontStyle.Bold);
            this.lblAppName.ForeColor = System.Drawing.Color.White;
            this.lblAppName.BackColor = System.Drawing.Color.Transparent;
            this.lblAppName.AutoSize = true;
            this.lblAppName.Name = "lblAppName";
            this.pnlMain.Controls.Add(this.lblAppName);

            // Alt başlık
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblSubtitle.Text = "Hayalinizdeki seyahati planlamaya başlayın";
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.lblSubtitle.BackColor = System.Drawing.Color.Transparent;
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Name = "lblSubtitle";
            this.pnlMain.Controls.Add(this.lblSubtitle);

            // ==================== GİRİŞ PANELİ (Glass Effect) ====================
            this.pnlLogin = new System.Windows.Forms.Panel();
            this.pnlLogin.BackColor = System.Drawing.Color.FromArgb(35, 255, 255, 255);
            this.pnlLogin.Size = new System.Drawing.Size(420, 340);
            this.pnlLogin.Name = "pnlLogin";

            // Giriş başlığı
            this.lblLoginTitle = new System.Windows.Forms.Label();
            this.lblLoginTitle.Text = "Giriş Yap";
            this.lblLoginTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblLoginTitle.ForeColor = System.Drawing.Color.White;
            this.lblLoginTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblLoginTitle.Location = new System.Drawing.Point(35, 25);
            this.lblLoginTitle.AutoSize = true;
            this.pnlLogin.Controls.Add(this.lblLoginTitle);

            // Kullanıcı adı label
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblUsername.Text = "Kullanıcı Adı";
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.lblUsername.BackColor = System.Drawing.Color.Transparent;
            this.lblUsername.Location = new System.Drawing.Point(35, 80);
            this.lblUsername.AutoSize = true;
            this.pnlLogin.Controls.Add(this.lblUsername);

            // Kullanıcı adı textbox
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtUsername.BackColor = System.Drawing.Color.FromArgb(45, 55, 85);
            this.txtUsername.ForeColor = System.Drawing.Color.White;
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtUsername.Size = new System.Drawing.Size(350, 34);
            this.txtUsername.Location = new System.Drawing.Point(35, 105);
            this.txtUsername.Name = "txtUsername";
            this.pnlLogin.Controls.Add(this.txtUsername);

            // Şifre label
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblPassword.Text = "Şifre";
            this.lblPassword.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblPassword.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.lblPassword.BackColor = System.Drawing.Color.Transparent;
            this.lblPassword.Location = new System.Drawing.Point(35, 155);
            this.lblPassword.AutoSize = true;
            this.pnlLogin.Controls.Add(this.lblPassword);

            // Şifre textbox
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(45, 55, 85);
            this.txtPassword.ForeColor = System.Drawing.Color.White;
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPassword.UseSystemPasswordChar = true;
            this.txtPassword.Size = new System.Drawing.Size(350, 34);
            this.txtPassword.Location = new System.Drawing.Point(35, 180);
            this.txtPassword.Name = "txtPassword";
            this.pnlLogin.Controls.Add(this.txtPassword);

            // Mesaj etiketi
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblMessage.Text = "";
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMessage.ForeColor = System.Drawing.Color.FromArgb(245, 101, 101);
            this.lblMessage.BackColor = System.Drawing.Color.Transparent;
            this.lblMessage.Location = new System.Drawing.Point(35, 220);
            this.lblMessage.Size = new System.Drawing.Size(350, 20);
            this.lblMessage.Visible = false;
            this.pnlLogin.Controls.Add(this.lblMessage);

            // Giriş butonu
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnLogin.Text = "GİRİŞ YAP";
            this.btnLogin.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnLogin.BackColor = System.Drawing.Color.FromArgb(102, 126, 234);
            this.btnLogin.ForeColor = System.Drawing.Color.White;
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.FlatAppearance.BorderSize = 0;
            this.btnLogin.Size = new System.Drawing.Size(350, 50);
            this.btnLogin.Location = new System.Drawing.Point(35, 245);
            this.btnLogin.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Click += new System.EventHandler(this.BtnLogin_Click);
            this.pnlLogin.Controls.Add(this.btnLogin);

            // Kayıt ol linki
            this.lnkRegister = new System.Windows.Forms.LinkLabel();
            this.lnkRegister.Text = "Hesabınız yok mu? Kayıt olun";
            this.lnkRegister.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lnkRegister.LinkColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.lnkRegister.ActiveLinkColor = System.Drawing.Color.FromArgb(102, 126, 234);
            this.lnkRegister.BackColor = System.Drawing.Color.Transparent;
            this.lnkRegister.AutoSize = true;
            this.lnkRegister.Location = new System.Drawing.Point(110, 305);
            this.lnkRegister.Name = "lnkRegister";
            this.lnkRegister.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkRegister_LinkClicked);
            this.pnlLogin.Controls.Add(this.lnkRegister);

            this.pnlMain.Controls.Add(this.pnlLogin);
            this.Controls.Add(this.pnlMain);
            this.pnlMain.BringToFront();

            this.ResumeLayout(false);
        }

        /// <summary>
        /// Butonu yuvarlak yap
        /// </summary>
        private void RoundButton(System.Windows.Forms.Button btn)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, btn.Width, btn.Height);
            btn.Region = new System.Drawing.Region(path);
        }
    }
}
