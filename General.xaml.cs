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
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Text;
using System.Xml.Linq;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ITATKWinUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 
    public sealed partial class General : Page
    {
        public General()
        {
            this.InitializeComponent();

            //TODO: Dynamically build the UI elements and click events on the fly
            XDocument guiConfig = XDocument.Load(@"XML\Scripts.xml");
            foreach (XElement item in from y in guiConfig.Descendants("Item") select y)
            {
                TestStack.Children.Add(MainWindow.GenerateExpanderFromXML(item.Attribute("name").Value, item.Attribute("description").Value, item.Attribute("path").Value, item.Attribute("psVersion").Value, item.Attribute("icon").Value , item.Attribute("category").Value));
            }
        }

        private void Expander1LogonRun_Click(object sender, RoutedEventArgs e)
        {

            //Sample code for running PowerShell 7 via the Microsoft.PowerShell.SDK Library
            var script = "C:\\scripts space\\MultiLineTestScript.ps1";
            InitialSessionState initial = InitialSessionState.CreateDefault();
            initial.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;
            Runspace runspace = RunspaceFactory.CreateRunspace(initial);
            runspace.Open();
            var ps = PowerShell.Create();
            ps.Runspace = runspace;
            ps.AddScript(File.ReadAllText(script)); //Must use ReadAllText to allow paths with spaces, AddScript() doesn't seem to support it natively
            ps.Streams.Information.DataAdded += MainWindow.myProgressEventHandler;
            ps.Invoke();

            /*
            try
            {
                Collection<PSObject> results = ps.Invoke();

                StringBuilder sb = new StringBuilder();
                foreach (PSObject result in results)
                {
                    Debug.WriteLine("Results:");
                    Debug.WriteLine(result.ToString());
                    sb.Append(result.ToString());
                }

                MainWindow.MyText = sb.ToString();
            }
            catch (RuntimeException runtimeException)
            {
                Debug.WriteLine(
                        "Runtime exception: {0}: {1}\n{2}",
                        runtimeException.ErrorRecord.InvocationInfo.InvocationName,
                        runtimeException.Message,
                        runtimeException.ErrorRecord.InvocationInfo.PositionMessage);
            }*/

            runspace.Close();
        }

        private void Expander2LogonRun_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.LaunchScript("C:\\scripts space\\MultiLineTestScript.ps1", "", "PS5");
        }

        private void Expander3LogonRun_Click(object sender, RoutedEventArgs e)
        {
            MainWindow.LaunchScript("C:\\scripts space\\MultiLineTestScript.ps1", "", "PS7");
        }
    }
}
