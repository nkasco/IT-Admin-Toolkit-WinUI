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
        public static void LaunchScript(string scriptPath, string args, string type)
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

        //string title, string description, string icon, string scriptPath, string args
        public static void ProcessScriptXML()
        {
            //TODO: Dynamically build the UI elements and click events on the fly
            XDocument guiConfig = XDocument.Load(@"XML\Gui.xml");

        }

        public static void LaunchScript(string scriptPath, string type)
        {
            //Overload condition if there are no args
            LaunchScript(scriptPath, "", type);
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
