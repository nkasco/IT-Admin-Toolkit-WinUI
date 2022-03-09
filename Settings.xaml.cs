using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ITATKWinUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
            //Load changelog from XML
            XDocument changelogDetail = XDocument.Load(@"Changelog.xml");

            //Load settings and set UI accordingly
            XDocument settingsXML = XDocument.Load(@"Settings.xml");
        }

        private void SetSettingVisibility(string settingTab)
        {
            GeneralTab.Visibility = Visibility.Collapsed;
            AppearanceTab.Visibility = Visibility.Collapsed;
            BrandingTab.Visibility = Visibility.Collapsed;
            KeybindsTab.Visibility = Visibility.Collapsed;
            HelpTab.Visibility = Visibility.Collapsed;
            Changelog.Visibility = Visibility.Collapsed;

            switch (settingTab)
            {
                case "General":
                    GeneralTab.Visibility = Visibility.Visible;
                    break;

                case "Appearance":
                    AppearanceTab.Visibility = Visibility.Visible;
                    break;

                case "Branding":
                    BrandingTab.Visibility = Visibility.Visible;
                    break;

                case "Keybinds":
                    KeybindsTab.Visibility = Visibility.Visible;
                    break;

                case "Help":
                    HelpTab.Visibility = Visibility.Visible;
                    break;

                case "Changelog":
                    Changelog.Visibility = Visibility.Visible;
                    break;

                default:
                    break;
            }
        }

        private void SettingsNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            string setting = args.SelectedItemContainer.Content.ToString();
            SetSettingVisibility(setting);
        }
    }
}
