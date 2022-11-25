using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace HomeBudget888WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// 
    public enum Skin { Default, Light, Dark, Other };

    public partial class App : Application
    {
        public static Skin Skin { get; set; } = Skin.Default;

        private void Application_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show("Exception not handled" + e.Exception.Message, "Error");

            e.Handled = true;
        }

        public void ChangeSkin(Skin newSkin)
        {
            Skin = newSkin;

            Resources.Clear();
            Resources.MergedDictionaries.Clear();

            switch (Skin)
            {
                case Skin.Light:
                    ApplyLightMode();
                    break;
                case Skin.Dark:
                    ApplyDarkMode();
                    break;
                case Skin.Other:
                    ApplyOtherMode();
                    break;
                default:
                    return;
            }

            ApplySharedResources();
        }

        private void ApplyLightMode()
        {
            AddResourceDictionary("light_color.xaml");
        }

        private void ApplyDarkMode()
        {
            AddResourceDictionary("blue_color.xaml");
        }

        private void ApplyOtherMode()
        {
            AddResourceDictionary("warm_color.xaml");
        }

        private void ApplySharedResources()
        {
            AddResourceDictionary("Layout.xaml");
        }

        private void AddResourceDictionary(string src)
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary() { Source = new Uri(src, UriKind.Relative) });
        }
    }

    public class SkinResourceDictionary : ResourceDictionary
    {
        private Uri _defaultSource;
        private Uri _lightSource;
        private Uri _darkSource;
        private Uri _otherSource;
        
        public Uri DefaultSource
        {
            get { return _defaultSource; }
            set
            {
                _defaultSource = value;
                UpdateSource();
            }
        }

        public Uri LightSource
        {
            get { return _lightSource; }
            set
            {
                _lightSource = value;
                UpdateSource();
            }
        }
        public Uri DarkSource
        {
            get { return _darkSource; }
            set
            {
                _darkSource = value;
                UpdateSource();
            }
        }

        public Uri OtherSource
        {
            get { return _otherSource; }
            set
            {
                _otherSource = value;
                UpdateSource();
            }
        }

        public void UpdateSource()
        {
            var val = App.Skin == Skin.Light ? LightSource : App.Skin == Skin.Dark ? DarkSource : App.Skin == Skin.Other ? OtherSource : DefaultSource;
            if (val != null && base.Source != val)
                base.Source = val;
        }
    }
}
