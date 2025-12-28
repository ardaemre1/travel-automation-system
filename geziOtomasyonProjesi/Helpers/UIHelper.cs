using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace geziOtomasyonProjesi.Helpers
{
    /// <summary>
    /// Modern UI bileşenleri için helper sınıf
    /// </summary>
    public static class UIHelper
    {
        #region Panel ve Form Stilleri

        /// <summary>
        /// Gradient arka plan çiz
        /// </summary>
        public static void DrawGradientBackground(Graphics g, Rectangle rect, Color startColor, Color endColor)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                rect, startColor, endColor, LinearGradientMode.ForwardDiagonal))
            {
                g.FillRectangle(brush, rect);
            }
        }

        /// <summary>
        /// Yuvarlak köşeli dikdörtgen oluştur
        /// </summary>
        public static GraphicsPath CreateRoundedRectangle(Rectangle bounds, int radius)
        {
            int diameter = radius * 2;
            Size size = new Size(diameter, diameter);
            Rectangle arc = new Rectangle(bounds.Location, size);
            GraphicsPath path = new GraphicsPath();

            if (radius == 0)
            {
                path.AddRectangle(bounds);
                return path;
            }

            // Sol üst köşe
            path.AddArc(arc, 180, 90);

            // Sağ üst köşe
            arc.X = bounds.Right - diameter;
            path.AddArc(arc, 270, 90);

            // Sağ alt köşe
            arc.Y = bounds.Bottom - diameter;
            path.AddArc(arc, 0, 90);

            // Sol alt köşe
            arc.X = bounds.Left;
            path.AddArc(arc, 90, 90);

            path.CloseFigure();
            return path;
        }

        #endregion

        #region Buton Stilleri

        /// <summary>
        /// Butona modern stil uygula
        /// </summary>
        public static void ApplyModernButtonStyle(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = ColorPalette.Primary;
            button.ForeColor = ColorPalette.TextLight;
            button.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            button.Cursor = Cursors.Hand;

            // Hover efekti
            button.MouseEnter += (s, e) =>
            {
                button.BackColor = ColorPalette.ButtonHover;
            };

            button.MouseLeave += (s, e) =>
            {
                button.BackColor = ColorPalette.Primary;
            };

            button.MouseDown += (s, e) =>
            {
                button.BackColor = ColorPalette.ButtonPressed;
            };

            button.MouseUp += (s, e) =>
            {
                button.BackColor = ColorPalette.ButtonHover;
            };
        }

        /// <summary>
        /// İkincil buton stili
        /// </summary>
        public static void ApplySecondaryButtonStyle(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 1;
            button.FlatAppearance.BorderColor = ColorPalette.Primary;
            button.BackColor = Color.Transparent;
            button.ForeColor = ColorPalette.Primary;
            button.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            button.Cursor = Cursors.Hand;

            button.MouseEnter += (s, e) =>
            {
                button.BackColor = Color.FromArgb(20, ColorPalette.Primary);
            };

            button.MouseLeave += (s, e) =>
            {
                button.BackColor = Color.Transparent;
            };
        }

        #endregion

        #region TextBox Stilleri

        /// <summary>
        /// TextBox'a modern stil uygula
        /// </summary>
        public static void ApplyModernTextBoxStyle(TextBox textBox)
        {
            textBox.BorderStyle = BorderStyle.None;
            textBox.Font = new Font("Segoe UI", 11);
            textBox.BackColor = Color.FromArgb(245, 245, 250);
            textBox.ForeColor = ColorPalette.TextDark;
        }

        #endregion

        #region Label Stilleri

        /// <summary>
        /// Başlık label stili
        /// </summary>
        public static void ApplyTitleLabelStyle(Label label)
        {
            label.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            label.ForeColor = ColorPalette.TextLight;
            label.BackColor = Color.Transparent;
        }

        /// <summary>
        /// Alt başlık label stili
        /// </summary>
        public static void ApplySubtitleLabelStyle(Label label)
        {
            label.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            label.ForeColor = ColorPalette.TextMuted;
            label.BackColor = Color.Transparent;
        }

        #endregion

        #region Mesaj Kutuları

        /// <summary>
        /// Başarı mesajı göster
        /// </summary>
        public static void ShowSuccess(string message, string title = "Başarılı")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// Hata mesajı göster
        /// </summary>
        public static void ShowError(string message, string title = "Hata")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// Uyarı mesajı göster
        /// </summary>
        public static void ShowWarning(string message, string title = "Uyarı")
        {
            MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        /// <summary>
        /// Onay sorusu göster
        /// </summary>
        public static bool ShowConfirm(string message, string title = "Onay")
        {
            return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        #endregion

        #region Form Ayarları

        /// <summary>
        /// Form'a temel stilleri uygula
        /// </summary>
        public static void ApplyFormStyle(Form form)
        {
            form.BackColor = ColorPalette.DarkBackground;
            form.ForeColor = ColorPalette.TextLight;
            form.Font = new Font("Segoe UI", 10);
            form.FormBorderStyle = FormBorderStyle.FixedSingle;
            form.MaximizeBox = false;
            form.StartPosition = FormStartPosition.CenterScreen;
        }

        #endregion
    }

    #region Custom Controls

    /// <summary>
    /// Yuvarlak köşeli panel - KALITIM örneği
    /// Panel sınıfından türetilmiş custom control
    /// </summary>
    public class RoundedPanel : Panel
    {
        private int _borderRadius = 20;
        private Color _backgroundColor = Color.FromArgb(40, 255, 255, 255);

        public int BorderRadius
        {
            get => _borderRadius;
            set { _borderRadius = value; Invalidate(); }
        }

        public Color PanelBackgroundColor
        {
            get => _backgroundColor;
            set { _backgroundColor = value; Invalidate(); }
        }

        public RoundedPanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }

        // OVERRIDE - OnPaint metodunu override ediyoruz
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
            using (GraphicsPath path = UIHelper.CreateRoundedRectangle(rect, BorderRadius))
            {
                using (SolidBrush brush = new SolidBrush(_backgroundColor))
                {
                    e.Graphics.FillPath(brush, path);
                }
            }
        }
    }

    /// <summary>
    /// Placeholder destekli TextBox - KALITIM örneği
    /// </summary>
    public class PlaceholderTextBox : TextBox
    {
        private string _placeholder = "";
        private Color _placeholderColor = Color.Gray;
        private bool _isPlaceholder = true;
        private bool _isPasswordMode = false;
        private Color _normalForeColor = Color.Black;

        public string Placeholder
        {
            get => _placeholder;
            set
            {
                _placeholder = value;
                if (_isPlaceholder)
                {
                    base.Text = _placeholder;
                }
            }
        }

        public Color PlaceholderColor
        {
            get => _placeholderColor;
            set => _placeholderColor = value;
        }

        public bool IsPasswordMode
        {
            get => _isPasswordMode;
            set => _isPasswordMode = value;
        }

        public new string Text
        {
            get => _isPlaceholder ? "" : base.Text;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    ShowPlaceholder();
                }
                else
                {
                    HidePlaceholder();
                    base.Text = value;
                }
            }
        }

        public PlaceholderTextBox()
        {
            GotFocus += OnGotFocus;
            LostFocus += OnLostFocus;
        }

        private void OnGotFocus(object sender, EventArgs e)
        {
            if (_isPlaceholder)
            {
                HidePlaceholder();
            }
        }

        private void OnLostFocus(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(base.Text))
            {
                ShowPlaceholder();
            }
        }

        private void ShowPlaceholder()
        {
            _isPlaceholder = true;
            base.Text = _placeholder;
            ForeColor = _placeholderColor;
            if (_isPasswordMode)
            {
                UseSystemPasswordChar = false;
            }
        }

        private void HidePlaceholder()
        {
            _isPlaceholder = false;
            base.Text = "";
            ForeColor = _normalForeColor;
            if (_isPasswordMode)
            {
                UseSystemPasswordChar = true;
            }
        }

        public void SetNormalForeColor(Color color)
        {
            _normalForeColor = color;
        }
    }

    /// <summary>
    /// Gradient arka planlı panel - KALITIM örneği
    /// </summary>
    public class GradientPanel : Panel
    {
        private Color _startColor = ColorPalette.GradientStart;
        private Color _endColor = ColorPalette.GradientEnd;
        private LinearGradientMode _gradientMode = LinearGradientMode.ForwardDiagonal;

        public Color StartColor
        {
            get => _startColor;
            set { _startColor = value; Invalidate(); }
        }

        public Color EndColor
        {
            get => _endColor;
            set { _endColor = value; Invalidate(); }
        }

        public LinearGradientMode GradientMode
        {
            get => _gradientMode;
            set { _gradientMode = value; Invalidate(); }
        }

        public GradientPanel()
        {
            DoubleBuffered = true;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.ResizeRedraw, true);
        }

        // OVERRIDE - OnPaint metodunu override ediyoruz
        protected override void OnPaint(PaintEventArgs e)
        {
            using (LinearGradientBrush brush = new LinearGradientBrush(
                ClientRectangle, _startColor, _endColor, _gradientMode))
            {
                e.Graphics.FillRectangle(brush, ClientRectangle);
            }
        }
    }

    #endregion
}
