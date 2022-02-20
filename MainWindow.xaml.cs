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
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI;
using System.Text;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Management.Automation;
using System.Xml.Linq;
using Microsoft.UI.Xaml.Media.Animation;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ITATKWinUI
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    //public class MyTextClass : INotifyPropertyChanged
    //{
    //    public event PropertyChangedEventHandler PropertyChanged;
    //    private string myText;
    //    public string MyText
    //    {
    //        get { return myText; }
    //        set
    //        {
    //            myText = value;
    //            OnPropertyChanged();
    //        }
    //    }

    //    protected void OnPropertyChanged([CallerMemberName] string name = null)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    //    }
    //}

    public partial class MainWindow : Window
    {
        public static void LaunchScript(object sender, EventArgs e, string scriptPath, string args, string type)
        {
            string EXEPath;

            //TODO: Add error handling for when someone inevitably tries to feed in something other than what is supported
            switch (type)
            {
                case "PS5":
                    EXEPath = @"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe";
                    break;

                case "PS7":
                    //TODO: Add PS7 path
                    EXEPath = @"pwsh.exe";
                    break;

                default:
                    EXEPath = "";
                    break;
            }

            var process = new Process
            {
                StartInfo = new ProcessStartInfo("\"" + EXEPath + "\"", "-ExecutionPolicy Bypass -NoProfile -File \"" + scriptPath + "\" " + args)
                {
                    CreateNoWindow = false
                }
            };
            //process.StartInfo.RedirectStandardOutput = true;
            //process.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
            //{
            //    // Prepend line numbers to each line of the output.
            //    if (!String.IsNullOrEmpty(e.Data))
            //    {
            //        lineCount++;
            //        output.Append("\n[" + lineCount + "]: " + e.Data);
            //    }
            //});
            process.Start();
            //process.BeginOutputReadLine();
            //process.WaitForExit();
            //Debug.WriteLine(output);
            //res = output.ToString();
            //process.WaitForExit();

            //process.Close();
        }

        public static void LaunchScript(string scriptPath, string args, string type)
        {
            //Overload condition for what we expect to use
            LaunchScript(null, null, scriptPath, args, type);
        }

        public static void LaunchScript(string scriptPath, string type)
        {
            //Overload condition if there are no args
            LaunchScript(null,null,scriptPath, "", type);
        }

        public static void LaunchExplorer(Object sender, EventArgs e, string path)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo("\"explorer.exe\"", "\"" + path + "\"") //Should this open the folder or just launch the file? (effectively "exploring" the script in Notepad)
                {
                    CreateNoWindow = false
                }
            };

            process.Start();
        }

        public static void LaunchExplorer(string path)
        {
            LaunchExplorer(null, null, path);
        }

        public static Page GenerateCategoryPageFromXML(string name)
        {
            Page page = new Page();
            
            StackPanel stackPanel = new StackPanel();
            stackPanel.Width = double.NaN;
            stackPanel.Name = name;

            TextBlock txtBlock = new TextBlock();
            txtBlock.Text = name;
            stackPanel.Children.Add(txtBlock);

            page.Content = stackPanel;

            foreach (XElement item in from y in MainWindow.guiConfig.Descendants("Item") select y)
            {
                if(item.Attribute("category").Value == name)
                {
                    stackPanel.Children.Add(MainWindow.GenerateExpanderFromXML(item.Attribute("name").Value, item.Attribute("description").Value, item.Attribute("path").Value, item.Attribute("psVersion").Value, item.Attribute("icon").Value, item.Attribute("category").Value));
                }
            }

                return page;
        }

        //TODO: Fix the navigation search
        public static NavigationViewItem GenerateCategoryNavigationViewItemFromXML(string category, string icon, string foreground)
        {
            NavigationViewItem navigationViewItem = new NavigationViewItem();
            navigationViewItem.Content = category;
            navigationViewItem.Name = "Nav" + category;

            //TODO: Add foreground color functionality
            SymbolIcon symbolIcon = new SymbolIcon();
            symbolIcon.Symbol = (Symbol)System.Enum.Parse(typeof(Symbol), icon);
            navigationViewItem.Icon = symbolIcon;

            return navigationViewItem;
        }

        public static NavigationViewItem GenerateCategoryNavigationViewItemFromXML(string category, string icon)
        {
            return GenerateCategoryNavigationViewItemFromXML(category, icon, null);
        }

        public static Expander GenerateExpanderFromXML(string name, string description, string path, string psVersion, string icon, string category)
        {
            //Build the base Expander
            Expander tmp = new Microsoft.UI.Xaml.Controls.Expander();
            tmp.Name = name;
            tmp.IsExpanded = false;
            tmp.ExpandDirection = Microsoft.UI.Xaml.Controls.ExpandDirection.Down;
            tmp.HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Stretch;
            tmp.Padding = new Microsoft.UI.Xaml.Thickness(20);
            tmp.Margin = new Microsoft.UI.Xaml.Thickness(10, 10, 10, 0);

            //Header Stack Panel
            StackPanel headerStack = new StackPanel();
            headerStack.Orientation = Microsoft.UI.Xaml.Controls.Orientation.Horizontal;
            headerStack.Padding = new Microsoft.UI.Xaml.Thickness(20);

            //Header Icon
            SymbolIcon headerIcon = new SymbolIcon();
            headerIcon.Symbol = (Symbol)System.Enum.Parse(typeof(Symbol), icon);
            headerIcon.Margin = new Microsoft.UI.Xaml.Thickness(0, 0, 10, 0);
            headerStack.Children.Add(headerIcon);

            //Header Title
            TextBlock headerTextBlock = new TextBlock();
            headerTextBlock.Text = name;
            headerStack.Children.Add(headerTextBlock);

            //Add Header Stack Panel to the Expander
            tmp.Header = headerStack;

            //Content Stack Panel
            StackPanel headerContentStackPanel = new StackPanel();

            //Description
            TextBlock headerContentTextBlock = new TextBlock();
            headerContentTextBlock.Text = "Description: " + description;
            headerContentTextBlock.HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Left;
            headerContentTextBlock.Margin = new Microsoft.UI.Xaml.Thickness(5);
            headerContentStackPanel.Children.Add(headerContentTextBlock);

            //Explore and Run Button Stack Panel
            StackPanel headerContentStackPanelStackPanel = new StackPanel();
            headerContentStackPanelStackPanel.Orientation = Microsoft.UI.Xaml.Controls.Orientation.Horizontal;
            headerContentStackPanelStackPanel.HorizontalAlignment = Microsoft.UI.Xaml.HorizontalAlignment.Right;
            
            //Explore Button
            Button headerContentExploreButton = new Button();
            headerContentExploreButton.Name = name + "Explore";
            headerContentExploreButton.Margin = new Microsoft.UI.Xaml.Thickness(0,0,5,0);
            StackPanel headerContentExploreButtonStackPanel = new StackPanel();
            headerContentExploreButtonStackPanel.Orientation = Microsoft.UI.Xaml.Controls.Orientation.Horizontal;
            SymbolIcon headerContentExploreButtonSymbolIcon = new SymbolIcon();
            headerContentExploreButtonSymbolIcon.Symbol = Symbol.Globe;
            headerContentExploreButtonSymbolIcon.Margin = new Microsoft.UI.Xaml.Thickness(0,0,5,0);
            TextBlock headerContentExploreButtonTextBlock = new TextBlock();
            headerContentExploreButtonTextBlock.Text = "Explore";
            headerContentExploreButtonStackPanel.Children.Add(headerContentExploreButtonSymbolIcon);
            headerContentExploreButtonStackPanel.Children.Add(headerContentExploreButtonTextBlock);
            headerContentExploreButton.Content = headerContentExploreButtonStackPanel;
            
            //Run Button
            Button headerContentRunButton = new Button();
            headerContentRunButton.Name = name + "Run";
            StackPanel headerContentRunButtonStackPanel = new StackPanel();
            headerContentRunButtonStackPanel.Orientation = Microsoft.UI.Xaml.Controls.Orientation.Horizontal;
            SymbolIcon headerContentRunButtonSymbolIcon = new SymbolIcon();
            headerContentRunButtonSymbolIcon.Symbol = Symbol.Play;
            headerContentRunButtonSymbolIcon.Margin = new Microsoft.UI.Xaml.Thickness(0, 0, 5, 0);
            TextBlock headerContentRunButtonTextBlock = new TextBlock();
            headerContentRunButtonTextBlock.Text = "Run";
            headerContentRunButtonStackPanel.Children.Add(headerContentRunButtonSymbolIcon);
            headerContentRunButtonStackPanel.Children.Add(headerContentRunButtonTextBlock);
            headerContentRunButton.Content = headerContentRunButtonStackPanel;

            //PS Version
            //TODO: Instead of a TextBlock this should be an icon
            TextBlock headerContentpsVersion = new TextBlock();
            headerContentpsVersion.Text = psVersion;

            //Add Buttons to Buttons Stack Panel
            headerContentStackPanelStackPanel.Children.Add(headerContentExploreButton);
            headerContentStackPanelStackPanel.Children.Add(headerContentRunButton);

            //Add Buttons Stack Panel to Parent Content Stack Panel
            headerContentStackPanel.Children.Add(headerContentStackPanelStackPanel);

            //Add PS Version to Parent Content Stack Panel
            headerContentStackPanel.Children.Add(headerContentpsVersion);

            //Finalize the Expander UI content
            tmp.Content = headerContentStackPanel;

            //Add Run and Explore click events
            headerContentRunButton.Click += (sender, e) => LaunchScript(path,psVersion);
            headerContentExploreButton.Click += (sender, e) => LaunchExplorer(path);

            return tmp;
        }

        public static XDocument guiConfig;

        public MainWindow()
        {
            this.InitializeComponent();
            //Load categories into the UI
            XDocument categoryConfig = XDocument.Load(@"XML\Categories.xml");
            guiConfig = XDocument.Load(@"XML\Scripts.xml");
            foreach (XElement item in from y in categoryConfig.Descendants("Item") select y)
            {
                MainNav.MenuItems.Add(GenerateCategoryNavigationViewItemFromXML(item.Attribute("category").Value, item.Attribute("icon").Value));
            }
        }

        //TODO: Terminal output WIP
        public static void myProgressEventHandler(object sender, DataAddedEventArgs e)
        {
            InformationRecord newRecord = ((PSDataCollection<InformationRecord>)sender)[e.Index];
            MyText = MyText + newRecord.ToString();
        }

        private static string myText;
        public static string MyText
        {
            get { return myText; }
            set
            {
                myText = value;
            }
        }

        private void UpdateMainWindowBindings()
        {
            Bindings.Update();
        }

        public void updateMainWindowBindings()
        {
            UpdateMainWindowBindings();
        }
        //End WIP

        private void MainNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            Page _page = GenerateCategoryPageFromXML(args.SelectedItemContainer.Content.ToString());
            //MainNav.Header = args.SelectedItemContainer.Content.ToString();
            contentFrame.Content = _page; //TODO: This probably doesn't need to generate each time you click
        }

        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        private void MachineDetailsToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if (ContentSplitView.IsPaneOpen == true)
            {
                MachineProgressRing.IsActive = false;
                ContentSplitView.IsPaneOpen = false;
            } else
            {
                MachineProgressRing.IsActive = true;
                ContentSplitView.IsPaneOpen= true;
            }
        }

        private void MachineComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(MachineComboBox.Text == "test")
            {
                PingSymbol.Visibility = Visibility.Visible;
                var greencolor = new Microsoft.UI.Xaml.Media.SolidColorBrush();
                greencolor.Color = Colors.Green;
                PingSymbol.Foreground = greencolor;
            } else if(MachineComboBox.Text == "offline")
            {
                PingSymbol.Visibility = Visibility.Visible;
                var redcolor = new Microsoft.UI.Xaml.Media.SolidColorBrush();
                redcolor.Color = Colors.Red;
                PingSymbol.Foreground = redcolor;
            } else
            {
                PingSymbol.Visibility= Visibility.Collapsed;
                MachineTeachingTip.IsOpen = true;
            }
        }

        private void MachineMultipleToggleButton_Click(object sender, RoutedEventArgs e)
        {
            if(MachineComboBox.Visibility == Visibility.Visible)
            {
                MachineComboBox.Visibility = Visibility.Collapsed;
                MultipleMachineInput.Visibility = Visibility.Visible;
                MachineMultipleClearButton.Visibility = Visibility.Visible;

                //Ensure the information pane is hidden
                ContentSplitView.IsPaneOpen = false;
                MachineDetailsToggleButton.IsChecked = false;
                MachineDetailsToggleButton.Visibility = Visibility.Collapsed;
            } else
            {
                MachineComboBox.Visibility = Visibility.Visible;
                MultipleMachineInput.Visibility = Visibility.Collapsed;
                MachineMultipleClearButton.Visibility = Visibility.Collapsed;

                //Show the information pane button
                ContentSplitView.Visibility = Visibility.Visible;
                MachineDetailsToggleButton.Visibility = Visibility.Visible;
            }
        }

        private void MachineMultipleClearButton_Click(object sender, RoutedEventArgs e)
        {
            MultipleMachineInput.Text = null;
        }

        private void HideScriptTerminal_Click(object sender, RoutedEventArgs e)
        {
            //This probably needs to be cleaned up a bit
            if (HideScriptTerminal.IsChecked == false && TerminalRowConfig.Height != GridLength.Auto)
            {
                ContentFrameRowConfig.Height = new GridLength(1, GridUnitType.Star);
                TerminalRowConfig.Height = GridLength.Auto;
                ContentFrameScrollViewer.Visibility = Visibility.Visible;
            }

            if(TerminalRow.Visibility == Visibility.Visible)
            {
                ContentFrameRowConfig.Height = new GridLength(1, GridUnitType.Star);
                TerminalRow.Visibility = Visibility.Collapsed;
            } else if (ContentFrameScrollViewer.Visibility == Visibility.Collapsed) {
                ContentFrameRowConfig.Height = GridLength.Auto;
                TerminalRowConfig.Height = new GridLength(1, GridUnitType.Star);
                TerminalRow.Visibility = Visibility.Visible;
            } 
            else
            {
                ContentFrameRowConfig.Height = new GridLength(1, GridUnitType.Star);
                TerminalRow.Visibility = Visibility.Visible;
            }
        }

        private void FullScreenTerminal_Click(object sender, RoutedEventArgs e)
        {
            if(ContentFrameScrollViewer.Visibility == Visibility.Visible)
            {
                //Full Screen
                ContentFrameScrollViewer.Visibility = Visibility.Collapsed;
                ContentFrameRowConfig.Height = GridLength.Auto;
                ScriptTerminal.Height = 540; //This needs to be more dynamic to fill the space
                TerminalRowConfig.Height = new GridLength(1, GridUnitType.Star);
            } else
            {
                //Docked
                ContentFrameRowConfig.Height = new GridLength(1, GridUnitType.Star);
                ContentFrameScrollViewer.Visibility = Visibility.Visible;
                ScriptTerminal.Height = 150;
                TerminalRowConfig.Height = GridLength.Auto;
            }
        }

        private void ClearTerminal_Click(object sender, RoutedEventArgs e)
        {
            //ScriptTerminal.Text = "";
            MyText = "";
            Bindings.Update();
        }

        private void CopyTerminal_Click(object sender, RoutedEventArgs e)
        {
            ScriptTerminal.SelectAll();
            ScriptTerminal.CopySelectionToClipboard();
        }
    } 
}
