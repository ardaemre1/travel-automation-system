using System;
using System.Drawing;

namespace geziOtomasyonProjesi.Helpers
{
    /// <summary>
    /// Uygulama renk paleti
    /// </summary>
    public static class ColorPalette
    {
        // Ana renkler
        public static readonly Color Primary = ColorTranslator.FromHtml("#667eea");
        public static readonly Color PrimaryDark = ColorTranslator.FromHtml("#5a67d8");
        public static readonly Color Secondary = ColorTranslator.FromHtml("#764ba2");
        public static readonly Color Accent = ColorTranslator.FromHtml("#f093fb");
        
        // Arka plan renkleri
        public static readonly Color DarkBackground = ColorTranslator.FromHtml("#1a1a2e");
        public static readonly Color DarkSurface = ColorTranslator.FromHtml("#16213e");
        public static readonly Color DarkCard = ColorTranslator.FromHtml("#0f3460");
        
        // Metin renkleri
        public static readonly Color TextLight = ColorTranslator.FromHtml("#ffffff");
        public static readonly Color TextMuted = ColorTranslator.FromHtml("#a0aec0");
        public static readonly Color TextDark = ColorTranslator.FromHtml("#2d3748");
        
        // Durum renkleri
        public static readonly Color Success = ColorTranslator.FromHtml("#48bb78");
        public static readonly Color Warning = ColorTranslator.FromHtml("#ed8936");
        public static readonly Color Error = ColorTranslator.FromHtml("#f56565");
        public static readonly Color Info = ColorTranslator.FromHtml("#4299e1");
        
        // Gradient başlangıç ve bitiş renkleri
        public static readonly Color GradientStart = ColorTranslator.FromHtml("#667eea");
        public static readonly Color GradientEnd = ColorTranslator.FromHtml("#764ba2");
        
        // Buton renkleri
        public static readonly Color ButtonHover = ColorTranslator.FromHtml("#7c3aed");
        public static readonly Color ButtonPressed = ColorTranslator.FromHtml("#6d28d9");
        
        // Input renkleri
        public static readonly Color InputBackground = Color.FromArgb(40, 255, 255, 255);
        public static readonly Color InputBorder = Color.FromArgb(60, 255, 255, 255);
        public static readonly Color InputFocusBorder = ColorTranslator.FromHtml("#a78bfa");
    }
}
