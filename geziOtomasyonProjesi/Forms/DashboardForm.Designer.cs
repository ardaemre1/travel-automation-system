namespace geziOtomasyonProjesi.Forms
{
    partial class DashboardForm
    {
        private System.ComponentModel.IContainer components = null;

        // Başlık çubuğu
        private System.Windows.Forms.Panel pnlTitleBar;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Button btnMaximize;
        private System.Windows.Forms.Label lblTitle;

        // Sidebar
        private System.Windows.Forms.Panel pnlSidebar;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblAppName;
        private System.Windows.Forms.Button btnDashboard;
        private System.Windows.Forms.Button btnCities;
        private System.Windows.Forms.Button btnPlaces;
        private System.Windows.Forms.Button btnHotels;
        private System.Windows.Forms.Button btnTrips;
        private System.Windows.Forms.Button btnBudget;
        private System.Windows.Forms.Button btnAI;
        private System.Windows.Forms.Button btnLogout;

        // İçerik
        private System.Windows.Forms.Panel pnlContent;

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
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1300, 800);
            this.Name = "DashboardForm";
            this.Text = "Dashboard";
            this.DoubleBuffered = true;

            // ==================== BAŞLIK ÇUBUĞU ====================
            this.pnlTitleBar = new System.Windows.Forms.Panel();
            this.pnlTitleBar.BackColor = System.Drawing.Color.FromArgb(22, 33, 62);
            this.pnlTitleBar.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTitleBar.Height = 45;
            this.pnlTitleBar.Cursor = System.Windows.Forms.Cursors.SizeAll;

            // macOS tarzı butonlar
            this.btnClose = CreateMacButton(System.Drawing.Color.FromArgb(255, 95, 87), 20);
            this.btnMinimize = CreateMacButton(System.Drawing.Color.FromArgb(255, 189, 46), 46);
            this.btnMaximize = CreateMacButton(System.Drawing.Color.FromArgb(40, 201, 64), 72);
            this.pnlTitleBar.Controls.Add(this.btnClose);
            this.pnlTitleBar.Controls.Add(this.btnMinimize);
            this.pnlTitleBar.Controls.Add(this.btnMaximize);

            this.lblTitle = new System.Windows.Forms.Label();
            this.lblTitle.Text = "Seyahat Planlayıcı - Dashboard";
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(160, 174, 192);
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lblTitle.AutoSize = true;
            this.lblTitle.BackColor = System.Drawing.Color.Transparent;
            this.lblTitle.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.pnlTitleBar.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlTitleBar);

            // ==================== SIDEBAR ====================
            this.pnlSidebar = new System.Windows.Forms.Panel();
            this.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(30, 255, 255, 255);
            this.pnlSidebar.Width = 280;
            this.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left;

            // Logo
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblLogo.Text = "✈";
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI Symbol", 44F);
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(102, 126, 234);
            this.lblLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblLogo.Location = new System.Drawing.Point(20, 15);
            this.lblLogo.AutoSize = true;
            this.pnlSidebar.Controls.Add(this.lblLogo);

            this.lblAppName = new System.Windows.Forms.Label();
            this.lblAppName.Text = "Seyahat\nPlanlayıcı";
            this.lblAppName.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Bold);
            this.lblAppName.ForeColor = System.Drawing.Color.White;
            this.lblAppName.BackColor = System.Drawing.Color.Transparent;
            this.lblAppName.Location = new System.Drawing.Point(100, 25);
            this.lblAppName.AutoSize = true;
            this.pnlSidebar.Controls.Add(this.lblAppName);

            // Sidebar menü butonları
            int yPos = 120;
            int btnHeight = 50;
            int spacing = 8;

            this.btnDashboard = CreateSidebarButton("🏠   Dashboard", yPos, true); yPos += btnHeight + spacing;
            this.btnCities = CreateSidebarButton("🌍   Şehirler", yPos); yPos += btnHeight + spacing;
            this.btnPlaces = CreateSidebarButton("📍   Gezilecek Yerler", yPos); yPos += btnHeight + spacing;
            this.btnHotels = CreateSidebarButton("🏨   Oteller", yPos); yPos += btnHeight + spacing;
            this.btnTrips = CreateSidebarButton("🗺️   Harita", yPos); yPos += btnHeight + spacing;
            this.btnBudget = CreateSidebarButton("💰   Bütçe Yönetimi", yPos); yPos += btnHeight + spacing;
            this.btnAI = CreateSidebarButton("🤖   AI Asistan", yPos);

            // Çıkış butonu
            this.btnLogout = new System.Windows.Forms.Button();
            this.btnLogout.Text = "🚪   Çıkış Yap";
            this.btnLogout.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.btnLogout.ForeColor = System.Drawing.Color.FromArgb(245, 101, 101);
            this.btnLogout.BackColor = System.Drawing.Color.Transparent;
            this.btnLogout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogout.FlatAppearance.BorderSize = 0;
            this.btnLogout.Size = new System.Drawing.Size(260, 50);
            this.btnLogout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogout.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnLogout.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            this.btnLogout.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            this.pnlSidebar.Controls.Add(this.btnLogout);
            this.Controls.Add(this.pnlSidebar);

            // ==================== İÇERİK PANELİ ====================
            this.pnlContent = new System.Windows.Forms.Panel();
            this.pnlContent.BackColor = System.Drawing.Color.Transparent;
            this.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill;

            this.Controls.Add(this.pnlContent);
            this.pnlContent.BringToFront();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button CreateMacButton(System.Drawing.Color color, int x)
        {
            var btn = new System.Windows.Forms.Button();
            btn.BackColor = color;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Size = new System.Drawing.Size(16, 16);
            btn.Location = new System.Drawing.Point(x, 14);
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
            btn.TabStop = false;
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddEllipse(0, 0, 16, 16);
            btn.Region = new System.Drawing.Region(path);
            return btn;
        }

        private System.Windows.Forms.Button CreateSidebarButton(string text, int yPos, bool isActive = false)
        {
            var btn = new System.Windows.Forms.Button();
            btn.Text = text;
            btn.Font = new System.Drawing.Font("Segoe UI", 12F);
            btn.ForeColor = System.Drawing.Color.White;
            btn.BackColor = isActive ? System.Drawing.Color.FromArgb(60, 102, 126, 234) : System.Drawing.Color.Transparent;
            btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Size = new System.Drawing.Size(260, 50);
            btn.Location = new System.Drawing.Point(10, yPos);
            btn.Cursor = System.Windows.Forms.Cursors.Hand;
            btn.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            btn.Padding = new System.Windows.Forms.Padding(25, 0, 0, 0);
            if (isActive) btn.Tag = "active";
            this.pnlSidebar.Controls.Add(btn);
            return btn;
        }
    }
}
