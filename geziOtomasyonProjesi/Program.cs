using System;
using System.Windows.Forms;
using Microsoft.Win32;
using geziOtomasyonProjesi.Forms;

namespace geziOtomasyonProjesi
{
    static class Program
    {
        /// <summary>
        /// Uygulamanın ana giriş noktası.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // WebBrowser kontrolü için IE11 Edge modunu etkinleştir
            SetBrowserEmulationMode();
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // LoginForm ile uygulama başlat
            Application.Run(new LoginForm());
        }

        /// <summary>
        /// WebBrowser kontrolü için IE11 Edge modunu registry'de ayarla
        /// Bu sayede Leaflet.js gibi modern JavaScript kütüphaneleri çalışır
        /// </summary>
        private static void SetBrowserEmulationMode()
        {
            try
            {
                string appName = System.IO.Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                
                using (var key = Registry.CurrentUser.CreateSubKey(
                    @"Software\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION",
                    RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    // 11001 = IE11 Edge mode
                    key?.SetValue(appName, 11001, RegistryValueKind.DWord);
                }
            }
            catch
            {
                // Registry yazma hatası olursa sessizce devam et
            }
        }
    }
}
