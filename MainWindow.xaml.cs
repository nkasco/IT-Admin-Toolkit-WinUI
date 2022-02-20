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

        //TODO: Generate Category pages from XML to be dynamic
        public static Page GenerateCategoryPageFromXML()
        {
            Page page = new Page();

            return page;
        }

        //TODO: Generate Navigation View Items from the XML Categories
        public static NavigationViewItem GenerateCategoryNavigationViewItemFromXML()
        {
            NavigationViewItem navigationViewItem = new NavigationViewItem();

            return navigationViewItem;
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

        public MainWindow()
        {
            this.InitializeComponent();
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
            Type _page = null;

            if(args.SelectedItemContainer.Content.ToString() == "User")
            {
                _page = typeof(User);
                //MainWindow.MyText = "TestUser";
                //Bindings.Update();
            }
            else if(args.SelectedItemContainer.Content.ToString() == "Manage")
            {
                _page = typeof(Manage);
                Bindings.Update();
            }
            else if (args.SelectedItemContainer.Content.ToString() == "Logs and Stats")
            {
                _page = typeof(LogsandStats);
            }
            else if (args.SelectedItemContainer.Content.ToString() == "Troubleshoot")
            {
                _page = typeof(Troubleshoot);
            }
            else if (args.SelectedItemContainer.Content.ToString() == "Remediate")
            {
                _page = typeof(Remediate);
            }
            else if (args.SelectedItemContainer.Content.ToString() == "Security")
            {
                _page = typeof(Security);
            }
            else if (args.SelectedItemContainer.Content.ToString() == "Inventory")
            {
                _page = typeof(Inventory);
            }
            else if (args.SelectedItemContainer.Content.ToString() == "Elevate")
            {
                _page = typeof(Elevate);
            }
            else if (args.SelectedItemContainer.Content.ToString() == "Virtual Machine")
            {
                _page = typeof(VirtualMachine);
            } else if (args.SelectedItemContainer.Content.ToString() == "Settings")
            {
                _page = typeof(Settings);
            }
            else
            {
                _page = typeof(General);
            }

            //MainNav.Header = args.SelectedItemContainer.Content.ToString();
            contentFrame.Navigate(_page);
        }

        /*private void MainNav_Navigate(string navItemTag, Windows.UI.Xaml.Media.Animation.NavigationTransitionInfo transitionInfo)
        {

        }*/

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
