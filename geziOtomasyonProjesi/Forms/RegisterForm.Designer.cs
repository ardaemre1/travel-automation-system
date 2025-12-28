namespace geziOtomasyonProjesi.Forms
{
    partial class RegisterForm
    {
        private System.ComponentModel.IContainer components = null;

        // Kontroller
        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Label lblRegisterTitle;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label lblConfirmPassword;
        private System.Windows.Forms.TextBox txtConfirmPassword;
        private System.Windows.Forms.Button btnRegister;
        private System.Windows.Forms.LinkLabel lnkBack;
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
            this.ClientSize = new System.Drawing.Size(500, 600);
            this.Name = "RegisterForm";
            this.Text = "Kayıt Ol";

            // ==================== BAŞLIK ÇUBUĞU ====================
            this.pnlTitleBar = new System.Windows.Forms.Panel();
            this.pnlTitleBar.BackColor = System.Drawing.Color.FromArgb(26, 26, 46);
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.Height = 40;
            this.pnlTitleBar.Name = "pnlTitleBar";

            // Kapatma butonu (Kırmızı - iOS tarzı)
            this.btnClose = new System.Windows.Forms.Button();
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(255, 95, 87);
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Size = new System.Drawing.Size(14, 14);
            this.btnClose.Location = new System.Drawing.Point(15, 13);
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Name = "btnClose";
            this.btnClose.TabStop = false;
            this.pnlTitleBar.Controls.Add(this.btnClose);

            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTitle.Text = "Kayıt Ol";
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(220, 10);
            this.lblTitle.Name = "lblTitle";
            this.pnlTitleBar.Controls.Add(this.lblTitle);

            this.Controls.Add(this.pnlTitleBar);

            // ==================== ANA PANEL ====================
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlMain.BackColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            this.pnlMain.Size = new System.Drawing.Size(440, 500);
            this.pnlMain.Location = new System.Drawing.Point(30, 60);
            this.pnlMain.Name = "pnlMain";

            int yPos = 25;
            int spacing = 70;

            // Kayıt başlığı
            this.lblRegisterTitle = new System.Windows.Forms.Label();
            this.lblRegisterTitle.Text = "Yeni Hesap Oluştur";
            this.lblRegisterTitle.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold);
            this.lblRegisterTitle.ForeColor = System.Drawing.Color.White;
            this.lblRegisterTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblRegisterTitle.AutoSize = true;
            this.lblRegisterTitle.Location = new System.Drawing.Point(30, yPos);
            this.pnlMain.Controls.Add(this.lblRegisterTitle);
            yPos += 50;

            // Ad Soyad
            CreateInputField("Ad Soyad", ref this.lblFullName, ref this.txtFullName, ref yPos, spacing);
            
            // Kullanıcı Adı
            CreateInputField("Kullanıcı Adı", ref this.lblUsername, ref this.txtUsername, ref yPos, spacing);
            
            // E-posta
            CreateInputField("E-posta", ref this.lblEmail, ref this.txtEmail, ref yPos, spacing);
            
            // Şifre
            CreateInputField("Şifre", ref this.lblPassword, ref this.txtPassword, ref yPos, spacing, true);
            
            // Şifre Tekrar
            CreateInputField("Şifre Tekrar", ref this.lblConfirmPassword, ref this.txtConfirmPassword, ref yPos, spacing, true);

            // Mesaj etiketi
            this.lblMessage = new System.Windows.Forms.Label();
            this.lblMessage.Text = "";
            this.lblMessage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblMessage.ForeColor = System.Drawing.Color.FromArgb(245, 101, 101);
            this.lblMessage.BackColor = System.Drawing.Color.Transparent;
            this.lblMessage.AutoSize = true;
            this.lblMessage.Location = new System.Drawing.Point(30, yPos);
            this.lblMessage.Visible = false;
            this.pnlMain.Controls.Add(this.lblMessage);
            yPos += 20;

            // Kayıt butonu
            this.btnRegister = new System.Windows.Forms.Button();
            this.btnRegister.Text = "KAYIT OL";
            this.btnRegister.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.btnRegister.BackColor = System.Drawing.Color.FromArgb(102, 126, 234);
            this.btnRegister.ForeColor = System.Drawing.Color.White;
            this.btnRegister.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRegister.FlatAppearance.BorderSize = 0;
            this.btnRegister.Size = new System.Drawing.Size(380, 45);
            this.btnRegister.Location = new System.Drawing.Point(30, yPos);
            this.btnRegister.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRegister.Click += new System.EventHandler(this.BtnRegister_Click);
            this.pnlMain.Controls.Add(this.btnRegister);
            yPos += 55;

            // Geri linki
            this.lnkBack = new System.Windows.Forms.LinkLabel();
            this.lnkBack.Text = "← Giriş Sayfasına Dön";
            this.lnkBack.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lnkBack.LinkColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.lnkBack.ActiveLinkColor = System.Drawing.Color.FromArgb(102, 126, 234);
            this.lnkBack.BackColor = System.Drawing.Color.Transparent;
            this.lnkBack.AutoSize = true;
            this.lnkBack.Location = new System.Drawing.Point(140, yPos);
            this.lnkBack.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LnkBack_LinkClicked);
            this.pnlMain.Controls.Add(this.lnkBack);

            this.Controls.Add(this.pnlMain);
            this.ResumeLayout(false);
        }

        private void CreateInputField(string labelText, ref System.Windows.Forms.Label label, ref System.Windows.Forms.TextBox textBox, ref int yPos, int spacing, bool isPassword = false)
        {
            label = new System.Windows.Forms.Label();
            label.Text = labelText;
            label.Font = new System.Drawing.Font("Segoe UI", 10F);
            label.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            label.BackColor = System.Drawing.Color.Transparent;
            label.AutoSize = true;
            label.Location = new System.Drawing.Point(30, yPos);
            this.pnlMain.Controls.Add(label);

            textBox = new System.Windows.Forms.TextBox();
            textBox.Font = new System.Drawing.Font("Segoe UI", 11F);
            textBox.BackColor = System.Drawing.Color.FromArgb(40, 40, 60);
            textBox.ForeColor = System.Drawing.Color.White;
            textBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            textBox.Size = new System.Drawing.Size(380, 30);
            textBox.Location = new System.Drawing.Point(30, yPos + 22);
            if (isPassword) textBox.UseSystemPasswordChar = true;
            this.pnlMain.Controls.Add(textBox);

            yPos += spacing;
        }
    }
}
