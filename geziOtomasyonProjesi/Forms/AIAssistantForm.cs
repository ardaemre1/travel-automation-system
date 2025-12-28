using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using geziOtomasyonProjesi.Helpers;

namespace geziOtomasyonProjesi.Forms
{
    /// <summary>
    /// AI Asistan Formu - Gemini API ile seyahat önerileri
    /// </summary>
    public partial class AIAssistantForm : Form
    {
        private Panel pnlTitleBar;
        private Button btnClose;
        private Label lblTitle;
        private Panel pnlContent;
        private Label lblPageTitle;
        private TextBox txtQuestion;
        private Button btnAsk;
        private RichTextBox txtResponse;
        private Label lblInfo;

        // Gemini API - Ücretsiz API anahtarı kullanıcıdan alınacak
        private const string GEMINI_API_URL = "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent";
        private string apiKey = "";

        public AIAssistantForm()
        {
            InitializeComponent();
            ApplyModernStyle();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new Size(900, 700);
            this.Name = "AIAssistantForm";
            this.Text = "AI Seyahat Asistanı";
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
            this.lblTitle.Text = "AI Seyahat Asistanı";
            this.lblTitle.ForeColor = Color.FromArgb(160, 174, 192);
            this.lblTitle.Font = new Font("Segoe UI", 11F);
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new Point(380, 12);
            this.pnlTitleBar.Controls.Add(this.lblTitle);

            this.Controls.Add(this.pnlTitleBar);

            // İçerik
            this.pnlContent = new Panel();
            this.pnlContent.BackColor = Color.Transparent;
            this.pnlContent.Dock = DockStyle.Fill;

            this.lblPageTitle = new Label();
            this.lblPageTitle.Text = "🤖 AI Seyahat Asistanı";
            this.lblPageTitle.Font = new Font("Segoe UI", 24F, FontStyle.Bold);
            this.lblPageTitle.ForeColor = Color.White;
            this.lblPageTitle.Location = new Point(30, 20);
            this.lblPageTitle.AutoSize = true;
            this.pnlContent.Controls.Add(this.lblPageTitle);

            this.lblInfo = new Label();
            this.lblInfo.Text = "Seyahat planınız hakkında sorular sorun, rota önerileri alın!";
            this.lblInfo.Font = new Font("Segoe UI", 11F);
            this.lblInfo.ForeColor = Color.FromArgb(160, 174, 192);
            this.lblInfo.Location = new Point(30, 70);
            this.lblInfo.AutoSize = true;
            this.pnlContent.Controls.Add(this.lblInfo);

            // Soru girişi
            this.txtQuestion = new TextBox();
            this.txtQuestion.Font = new Font("Segoe UI", 12F);
            this.txtQuestion.BackColor = Color.FromArgb(45, 55, 85);
            this.txtQuestion.ForeColor = Color.White;
            this.txtQuestion.BorderStyle = BorderStyle.FixedSingle;
            this.txtQuestion.Multiline = true;
            this.txtQuestion.Size = new Size(700, 80);
            this.txtQuestion.Location = new Point(30, 110);
            this.txtQuestion.Text = "İstanbul'da 3 günlük bir gezi planı önerir misin? Tarihi yerler ve yemek mekanları dahil olsun.";
            this.pnlContent.Controls.Add(this.txtQuestion);

            // Sor butonu
            this.btnAsk = new Button();
            this.btnAsk.Text = "🚀 Sor";
            this.btnAsk.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            this.btnAsk.BackColor = Color.FromArgb(102, 126, 234);
            this.btnAsk.ForeColor = Color.White;
            this.btnAsk.FlatStyle = FlatStyle.Flat;
            this.btnAsk.FlatAppearance.BorderSize = 0;
            this.btnAsk.Size = new Size(120, 80);
            this.btnAsk.Location = new Point(750, 110);
            this.btnAsk.Cursor = Cursors.Hand;
            this.btnAsk.Click += BtnAsk_Click;
            this.pnlContent.Controls.Add(this.btnAsk);

            // Yanıt alanı
            this.txtResponse = new RichTextBox();
            this.txtResponse.Font = new Font("Segoe UI", 11F);
            this.txtResponse.BackColor = Color.FromArgb(30, 40, 60);
            this.txtResponse.ForeColor = Color.White;
            this.txtResponse.BorderStyle = BorderStyle.None;
            this.txtResponse.Size = new Size(840, 430);
            this.txtResponse.Location = new Point(30, 210);
            this.txtResponse.ReadOnly = true;
            this.txtResponse.Text = "Merhaba! Ben AI seyahat asistanınızım. 🌍\n\nSize şu konularda yardımcı olabilirim:\n• Şehir ve rota önerileri\n• Gezilecek yerler\n• Bütçe planlaması\n• Konaklama önerileri\n• Yerel yemek ve kültür tavsiyeleri\n\nYukarıdaki alana sorunuzu yazın ve 'Sor' butonuna tıklayın!\n\n💡 İpucu: Ne kadar detaylı soru sorarsanız, o kadar iyi öneriler alırsınız.";
            this.pnlContent.Controls.Add(this.txtResponse);

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

        private async void BtnAsk_Click(object sender, EventArgs e)
        {
            string question = txtQuestion.Text.Trim();
            if (string.IsNullOrEmpty(question))
            {
                MessageBox.Show("Lütfen bir soru yazın.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            btnAsk.Enabled = false;
            btnAsk.Text = "⏳...";
            txtResponse.Text = "Düşünüyorum...\n\n🔄 Lütfen bekleyin...";

            try
            {
                string response = await GetAIResponse(question);
                txtResponse.Text = response;
            }
            catch (Exception ex)
            {
                txtResponse.Text = $"❌ Bir hata oluştu:\n{ex.Message}\n\n" +
                    "💡 Çevrimdışı mod aktif. İşte bazı genel öneriler:\n\n" +
                    GenerateOfflineResponse(question);
            }
            finally
            {
                btnAsk.Enabled = true;
                btnAsk.Text = "🚀 Sor";
            }
        }

        private async Task<string> GetAIResponse(string question)
        {
            // API anahtarı yoksa çevrimdışı mod
            if (string.IsNullOrEmpty(apiKey))
            {
                return GenerateOfflineResponse(question);
            }

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromSeconds(30);
                
                string requestBody = $@"{{
                    ""contents"": [{{
                        ""parts"": [{{
                            ""text"": ""Sen bir seyahat danışmanısın. Türkçe yanıt ver. Kullanıcının sorusu: {question.Replace("\"", "\\\"")}""
                        }}]
                    }}]
                }}";

                var content = new StringContent(requestBody, Encoding.UTF8, "application/json");
                var response = await client.PostAsync($"{GEMINI_API_URL}?key={apiKey}", content);
                
                if (response.IsSuccessStatusCode)
                {
                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    // Basit JSON parse (gerçek uygulamada Newtonsoft.Json kullanılmalı)
                    int textStart = jsonResponse.IndexOf("\"text\":") + 9;
                    int textEnd = jsonResponse.IndexOf("\"", textStart);
                    if (textStart > 8 && textEnd > textStart)
                    {
                        return jsonResponse.Substring(textStart, textEnd - textStart)
                            .Replace("\\n", "\n")
                            .Replace("\\\"", "\"");
                    }
                }
                
                throw new Exception("API yanıt vermedi");
            }
        }

        private string GenerateOfflineResponse(string question)
        {
            string lowerQuestion = question.ToLower();
            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine("🤖 AI Asistan Yanıtı (Çevrimdışı Mod)\n");
            sb.AppendLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━\n");

            if (lowerQuestion.Contains("istanbul"))
            {
                sb.AppendLine("📍 İSTANBUL SEYAHATİ ÖNERİLERİ\n");
                sb.AppendLine("🏛️ Tarihi Yerler:");
                sb.AppendLine("  • Ayasofya - Bizans/Osmanlı mirası");
                sb.AppendLine("  • Topkapı Sarayı - Osmanlı padişahlarının evi");
                sb.AppendLine("  • Yerebatan Sarnıcı - Yeraltı su deposu");
                sb.AppendLine("  • Sultanahmet Camii (Mavi Cami)\n");
                sb.AppendLine("🍽️ Yemek Mekanları:");
                sb.AppendLine("  • Pandeli (Mısır Çarşısı)");
                sb.AppendLine("  • Tarihi Sultanahmet Köftecisi");
                sb.AppendLine("  • Karaköy Güllüoğlu (Baklava)\n");
                sb.AppendLine("💰 Tahmini Günlük Bütçe: ₺500-1000");
            }
            else if (lowerQuestion.Contains("paris"))
            {
                sb.AppendLine("📍 PARİS SEYAHATİ ÖNERİLERİ\n");
                sb.AppendLine("🗼 Mutlaka Görülmesi Gerekenler:");
                sb.AppendLine("  • Eyfel Kulesi");
                sb.AppendLine("  • Louvre Müzesi (Mona Lisa)");
                sb.AppendLine("  • Notre-Dame Katedrali");
                sb.AppendLine("  • Champs-Élysées Bulvarı\n");
                sb.AppendLine("💰 Tahmini Günlük Bütçe: €100-200");
            }
            else if (lowerQuestion.Contains("bütçe") || lowerQuestion.Contains("para"))
            {
                sb.AppendLine("💰 BÜTÇE PLANLAMA İPUÇLARI\n");
                sb.AppendLine("1. Uçuş: Erken rezervasyon %30 tasarruf sağlar");
                sb.AppendLine("2. Konaklama: Hafta içi fiyatlar daha uygun");
                sb.AppendLine("3. Yemek: Yerel pazarlar turistik yerlere göre ucuz");
                sb.AppendLine("4. Ulaşım: Günlük kart alın, tek bilet yerine");
                sb.AppendLine("5. Müzeler: Ücretsiz gün/saatleri araştırın");
            }
            else
            {
                sb.AppendLine("🌍 GENEL SEYAHATİ İPUÇLARI\n");
                sb.AppendLine("✈️ Planlama:");
                sb.AppendLine("  • Gitmek istediğiniz şehri belirleyin");
                sb.AppendLine("  • Gün sayısına göre rota oluşturun");
                sb.AppendLine("  • Konaklama ve ulaşımı önceden ayırtın\n");
                sb.AppendLine("📋 Kontrol Listesi:");
                sb.AppendLine("  • Pasaport/kimlik geçerliliği");
                sb.AppendLine("  • Seyahat sigortası");
                sb.AppendLine("  • Yerel para birimi");
                sb.AppendLine("  • Şarj cihazları ve adaptör");
            }

            sb.AppendLine("\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
            sb.AppendLine("💡 Daha detaylı öneriler için Gemini API anahtarı ekleyin.");
            
            return sb.ToString();
        }
    }
}
