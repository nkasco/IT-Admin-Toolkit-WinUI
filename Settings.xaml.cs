using Microsoft.Management.Infrastructure;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Xml.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;

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
            void AppendChangeLogItem(XElement changelog)
            {
                TextBlock tmp =  new TextBlock();
                tmp.Text = "v" + changelog.Attribute("version").Value + " - " + changelog.Attribute("detail").Value;
                tmp.Margin = new Thickness(5);

                ChangelogXMLUI.Children.Add(tmp);
            }

            SettingVersion.Content = "IT Admin Toolkit v" + changelogDetail.Descendants("Item").Last().Attribute("version").Value;

            foreach (XElement item in from y in changelogDetail.Descendants("Item") select y)
            {
                AppendChangeLogItem(item);
            }

                //Load settings and set UI accordingly
                XDocument settingsXML = XDocument.Load(@"Settings.xml");
            foreach (XElement item in from y in settingsXML.Descendants("Item") select y)
            {
                switch (item.Attribute("Name").Value)
                {
                    case "SettingStartupBehavior":
                        SettingStartupBehavior.IsOn = Convert.ToBoolean(item.Attribute("Setting").Value);
                        break;

                    case "SettingAutomaticUpdates":
                        SettingAutomaticUpdates.IsOn = Convert.ToBoolean(item.Attribute("Setting").Value);
                        break;

                    case "SettingShowExplorer":
                        SettingShowExplorer.IsOn = Convert.ToBoolean(item.Attribute("Setting").Value);
                        break;

                    case "SettingTheme":
                        SettingTheme.SelectedValue = item.Attribute("Setting").Value.ToString();
                        break;

                    case "SettingApplicationTitle":
                        SettingApplicationTitle.Text = item.Attribute("Setting").Value;
                        break;

                    case "SettingApplicationIconImage":
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.UriSource = new Uri(SettingApplicationIconImage.BaseUri, item.Attribute("Setting").Value);
                        SettingApplicationIconImage.Source = bitmapImage;
                        break;
                }
            }
        }

        private void SaveSettings()
        {
            XDocument settingsXML = XDocument.Load(@"Settings.xml");
            foreach (XElement item in from y in settingsXML.Descendants("Item") select y)
            {
                switch (item.Attribute("Name").Value)
                {
                    //General
                    case "SettingStartupBehavior":
                        item.Attribute("Setting").Value = SettingStartupBehavior.IsOn.ToString();
                        if(SettingStartupBehavior.IsOn.ToString() == "true")
                        {
                            //TODO: Make app startup automatically after a reboot
                        }
                        break;

                    case "SettingAutomaticUpdates":
                        item.Attribute("Setting").Value = SettingAutomaticUpdates.IsOn.ToString();
                        break;

                    case "SettingShowExplorer":
                        item.Attribute("Setting").Value = SettingShowExplorer.IsOn.ToString();
                        break;

                    //Appearance
                    case "SettingTheme":
                        item.Attribute("Setting").Value = SettingTheme.SelectedValue.ToString();
                        break;

                    //Branding
                    case "SettingApplicationTitle":
                        item.Attribute("Setting").Value = SettingApplicationTitle.Text;
                        break;

                    case "SettingApplicationIconImage":
                        var bitmapImage = (BitmapImage)SettingApplicationIconImage.Source;
                        item.Attribute("Setting").Value = bitmapImage.UriSource.ToString();
                        break;
                }
            }

            settingsXML.Save(@"Settings.xml");
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
            if (setting == "Branding")
            {
                SaveSettings();
            }
            SetSettingVisibility(setting);
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        private void SettingApplicationIconOFD_Click(object sender, RoutedEventArgs e)
        {
            //TODO: Add an Open File Dialog
        }


    }
}
