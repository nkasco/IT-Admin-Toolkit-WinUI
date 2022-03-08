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
using Windows.UI;
using System.Reflection;
using Microsoft.UI.Xaml.Markup;
using Microsoft.UI.Xaml.Media.Imaging;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ITATKWinUI
{
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
        //TODO: Need a way to reset these as well as resetting the UI
        public static string CurrentInput;

        public static string CurrentMultipleInput;

        private static string SingleOrMulti = "Single";

        private static void LaunchScript(object sender, EventArgs e, string scriptPath, string args, string type, string inputType)
        {
            string EXEPath;

            if(inputType == "Machine")
            {
                //TODO: This may need to be something like "-Machine CurrentInput" but for now this is fine
                if (SingleOrMulti == "Single")
                {
                    if (CurrentInput == "")
                    {
                        //TODO: Handle when there is no input but the config defines it as required
                    }

                    if (args != "")
                    {
                        args = args + " " + CurrentInput;
                    }
                    else
                    {
                        args = CurrentInput;
                    }
                }
                else
                {
                    if (CurrentMultipleInput == "")
                    {
                        //TODO: Handle when there is no input but the config defines it as required
                    }

                    if (args != "")
                    {
                        args = args + " " + CurrentMultipleInput;
                    }
                    else
                    {
                        args = CurrentMultipleInput;
                    }
                }
            }

            //TODO: Add VBS, WSF, BAT, CMD, and PY support
            switch (type)
            {
                case "PS5":
                    EXEPath = @"powershell.exe";
                    break;

                case "PS7":
                    EXEPath = @"pwsh.exe";
                    break;

                default:
                    EXEPath = "";
                    break;
            }

            if (EXEPath != "")
            {
                if (!File.Exists(scriptPath))
                {
                    scriptPath = Environment.CurrentDirectory + "\\" + scriptPath;
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
        }

        private static void LaunchScript(string scriptPath, string args, string type, string inputType)
        {
            //Overload condition for what we expect to use
            LaunchScript(null, null, scriptPath, args, type, inputType);
        }

        private static void LaunchScript(string scriptPath, string type, string inputType)
        {
            //Overload condition if there are no args
            LaunchScript(null,null,scriptPath, "", type, inputType);
        }

        public static void LaunchExplorer(Object sender, EventArgs e, string path)
        {
            if (!File.Exists(path))
            {
                path = Environment.CurrentDirectory + "\\" + path;
            }

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
            //ResourceDictionary myResourceDictionary = new ResourceDictionary();
            //myResourceDictionary.Source = new Uri("ResourceDictionary.xaml", UriKind.RelativeOrAbsolute);
            ////page.Resources.MergedDictionaries.Add(myResourceDictionary);

            StackPanel stackPanel = new StackPanel();
            stackPanel.Width = double.NaN;
            stackPanel.Name = name;

            TextBlock txtBlock = new TextBlock();
            txtBlock.Text = name;
            txtBlock.Padding = new Thickness(5);
            stackPanel.Children.Add(txtBlock);

            page.Content = stackPanel;

            foreach (XElement item in from y in MainWindow.guiConfig.Descendants("Item") select y)
            {
                if(item.Attribute("category").Value == name)
                {
                    stackPanel.Children.Add(MainWindow.GenerateExpanderFromXML(item.Attribute("name").Value, item.Attribute("description").Value, item.Attribute("path").Value, item.Attribute("psVersion").Value, item.Attribute("icon").Value, item.Attribute("category").Value, item.Attribute("inputType").Value));
                }
            }

                return page;
        }

        //TODO: Fix the navigation search
        private void AutoSuggestBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            // Only get results when it was a user typing,
            // otherwise assume the value got filled in by TextMemberPath
            // or the handler for SuggestionChosen.
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                //Set the ItemsSource to be your filtered dataset
                //sender.ItemsSource = ;
            }
        }


        private void AutoSuggestBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        {
            // Set sender.Text. You can use args.SelectedItem to build your text string.
        }


        private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        {
            if (args.ChosenSuggestion != null)
            {
                // User selected an item from the suggestion list, take an action on it here.
            }
            else
            {
                // Use args.QueryText to determine what to do.
            }
        }


        public static NavigationViewItem GenerateCategoryNavigationViewItemFromXML(string category, string icon, string foreground)
        {
            NavigationViewItem navigationViewItem = new NavigationViewItem();
            navigationViewItem.Content = category;
            navigationViewItem.Name = "Nav" + category;
            //navigationViewItem.Margin = new Thickness(10,0,10,0); //TODO: This looks great in expanded mode but we need to adjust it in collapsed mode

            SymbolIcon symbolIcon = new SymbolIcon();
            symbolIcon.Symbol = (Symbol)Enum.Parse(typeof(Symbol), icon);
            var color = (Color)XamlBindingHelper.ConvertValue(typeof(Color), foreground);
            var brush = new SolidColorBrush(color);
            symbolIcon.Foreground = brush;
            navigationViewItem.Icon = symbolIcon;

            return navigationViewItem;
        }

        public static NavigationViewItem GenerateCategoryNavigationViewItemFromXML(string category, string icon)
        {
            return GenerateCategoryNavigationViewItemFromXML(category, icon, null);
        }
        public static Button GenerateExpanderButton(string text, string name, Orientation orientation, Symbol icon, Thickness iconMargin, Thickness btnMargin)
        {
            Button btn = new Button();

            btn.Name = name + text;
            btn.Margin = btnMargin;
            StackPanel ButtonStackPanel = new StackPanel();
            ButtonStackPanel.Orientation = orientation;
            SymbolIcon ButtonSymbolIcon = new SymbolIcon();
            ButtonSymbolIcon.Symbol = icon;
            ButtonSymbolIcon.Margin = iconMargin;
            TextBlock ButtonTextBlock = new TextBlock();
            ButtonTextBlock.Text = text;
            ButtonStackPanel.Children.Add(ButtonSymbolIcon);
            ButtonStackPanel.Children.Add(ButtonTextBlock);
            btn.Content = ButtonStackPanel;

            return btn;
        }

        public static Expander GenerateExpanderFromXML(string name, string description, string path, string psVersion, string icon, string category, string inputType)
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
            Button headerContentExploreButton = GenerateExpanderButton("Explore", name, Microsoft.UI.Xaml.Controls.Orientation.Horizontal, Symbol.Globe, new Microsoft.UI.Xaml.Thickness(0, 0, 5, 0), new Microsoft.UI.Xaml.Thickness(0, 0, 5, 0));

            //Run Button
            Button headerContentRunButton = GenerateExpanderButton("Run", name, Microsoft.UI.Xaml.Controls.Orientation.Horizontal, Symbol.Play, new Microsoft.UI.Xaml.Thickness(0, 0, 5, 0), new Microsoft.UI.Xaml.Thickness(0));

            //PS Version and Input Type
            StackPanel MetadataIcons = new StackPanel {Orientation = Microsoft.UI.Xaml.Controls.Orientation.Horizontal};
            Image PSImage = new Image();
            PSImage.Margin = new Thickness(5,0,5,0);
            if(psVersion == "PS5")
            {
                void Image_Loaded(object sender, RoutedEventArgs e)
                {
                    Image img = sender as Image;
                    BitmapImage bitmapImage = new BitmapImage();
                    img.Width = bitmapImage.DecodePixelWidth = 20;
                    bitmapImage.UriSource = new Uri(img.BaseUri, "Assets/Powershell5.png");
                    img.Source = bitmapImage;
                }
                PSImage.Loaded += Image_Loaded;
                ToolTip toolTip = new ToolTip();
                toolTip.Content = "Runs with Windows PowerShell 5";
                ToolTipService.SetToolTip(PSImage, toolTip);
                MetadataIcons.Children.Add(PSImage);
            }
            else if(psVersion == "PS7")
            {
                void Image_Loaded(object sender, RoutedEventArgs e)
                {
                    Image img = sender as Image;
                    BitmapImage bitmapImage = new BitmapImage();
                    img.Width = bitmapImage.DecodePixelWidth = 20;
                    bitmapImage.UriSource = new Uri(img.BaseUri, "Assets/Powershell7.ico");
                    img.Source = bitmapImage;
                }
                PSImage.Loaded += Image_Loaded;
                ToolTip toolTip = new ToolTip();
                toolTip.Content = "Runs with PowerShell 7";
                ToolTipService.SetToolTip(PSImage, toolTip);
                MetadataIcons.Children.Add(PSImage);
            }

            if (inputType == "Machine")
            {
                SymbolIcon InputTypeIcon = new SymbolIcon();
                InputTypeIcon.Symbol = Symbol.GoToStart;
                InputTypeIcon.Width = 20;
                InputTypeIcon.Height = 20;
                ToolTip toolTip = new ToolTip();
                toolTip.Content = "Requires machine input to run";
                ToolTipService.SetToolTip(InputTypeIcon, toolTip);
                MetadataIcons.Children.Add(InputTypeIcon);
            }

            //Add Buttons to Buttons Stack Panel
            headerContentStackPanelStackPanel.Children.Add(headerContentExploreButton);
            headerContentStackPanelStackPanel.Children.Add(headerContentRunButton);

            //Add Buttons Stack Panel to Parent Content Stack Panel
            headerContentStackPanel.Children.Add(headerContentStackPanelStackPanel);

            //Add PS Version to Parent Content Stack Panel
            headerContentStackPanel.Children.Add(MetadataIcons);

            //Finalize the Expander UI content
            tmp.Content = headerContentStackPanel;

            //Add Run and Explore click events
            headerContentRunButton.Click += (sender, e) => LaunchScript(path, psVersion, inputType);
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
                MainNav.MenuItems.Add(GenerateCategoryNavigationViewItemFromXML(item.Attribute("category").Value, item.Attribute("icon").Value, item.Attribute("foreground").Value));
            }

            //Select the first navigation item
            MainNav.SelectedItem = MainNav.MenuItems[1]; //Index 1 because 0 is the "Categories" text header
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
            //Bindings.Update();
        }

        public void updateMainWindowBindings()
        {
            UpdateMainWindowBindings();
        }
        //End WIP

        private void MainNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer.Content.ToString() == "Settings")
            {
                Type _page = null;
                _page = typeof(Settings);
                contentFrame.Navigate(_page);
            }
            else
            {
                Page _page = GenerateCategoryPageFromXML(args.SelectedItemContainer.Content.ToString());
                //MainNav.Header = args.SelectedItemContainer.Content.ToString();
                contentFrame.Content = _page; //TODO: This probably doesn't need to generate each time you click
            }
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
            //Add machine to the history, prevent duplicates
            if (!MachineComboBox.Items.Contains(MachineComboBox.Text))
            {
                //MachineComboBox.Items.Add(MachineComboBox.Text); //TODO: This is buggy and needs some work, disabling for now
            }

            //Set public variable with the current input
            CurrentInput = MachineComboBox.Text;

            //TODO: Provide realtime ping checks
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
                SingleOrMulti = "Multi";
                PingSymbol.Visibility = Visibility.Collapsed;
                MachineComboBox.Visibility = Visibility.Collapsed;
                MultipleMachineInput.Visibility = Visibility.Visible;
                MachineMultipleClearButton.Visibility = Visibility.Visible;

                //Ensure the information pane is hidden
                ContentSplitView.IsPaneOpen = false;
                MachineDetailsToggleButton.IsChecked = false;
                MachineDetailsToggleButton.Visibility = Visibility.Collapsed;
            } else
            {
                SingleOrMulti = "Single";
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
            //Bindings.Update();
        }

        private void CopyTerminal_Click(object sender, RoutedEventArgs e)
        {
            ScriptTerminal.SelectAll();
            ScriptTerminal.CopySelectionToClipboard();
        }

        private void MultipleMachineInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Replace new lines with commas
            CurrentMultipleInput = MultipleMachineInput.Text.Trim().Replace("\r", ",");
        }
    } 
}
