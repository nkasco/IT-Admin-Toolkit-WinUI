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
using System.Collections.ObjectModel;
using System.Management.Automation.Runspaces;
using System.Diagnostics;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ITATKWinUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class General : Page
    {
        public General()
        {
            this.InitializeComponent();
        }

        private void Expander1LogonRun_Click(object sender, RoutedEventArgs e)
        {
            //Sample code for running PowerShell 7 via the Microsoft.PowerShell.SDK Library
            //This might work with Install-Package Microsoft.PowerShell.5.ReferenceAssemblies -Version 1.1.0
            /*
            InitialSessionState initial = InitialSessionState.CreateDefault();
            //initial.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.Unrestricted;
            Runspace runspace = RunspaceFactory.CreateRunspace(initial);
            runspace.Open();
            var ps = PowerShell.Create();
            ps.Runspace = runspace;
            ps.AddScript("C:\\scripts\\MultiLineTestScript.ps1").Invoke();
            */

            //Potential
            var script = "C:\\scripts space\\MultiLineTestScript.ps1";
            var process = new Process
            {
                StartInfo = new ProcessStartInfo(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe", "-ExecutionPolicy Bypass -NoProfile -File \"" + script + "\"")
                {
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
            process.Start();

            //This causes the main thread to hang, probably need to run this on another thread
            //TODO:
            /*
            string s = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            using (StreamWriter outfile = new StreamWriter("StandardOutput.txt", true))
            {
                outfile.Write(s);
            }*/

            //TODO: Capture this output and feed it into the UI to create a terminal like output experience

            //This works but runs on the same runsapce so the UI freezes
            /*
            //File.GetAttributes("C:\\scripts space\\MultiLineTestScript.ps1");
            string strCmdText = "C:\\scripts space\\MultiLineTestScript.ps1";
            var process = new Process();
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.FileName = @"C:\windows\system32\windowspowershell\v1.0\powershell.exe";
            process.StartInfo.Arguments = "\"&'" + strCmdText + "'\"";
            process.StartInfo.WorkingDirectory = "C:\\scripts space";
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.LoadUserProfile = false;
            process.StartInfo.UseShellExecute = false; //True for elevated?
            //process.StartInfo.Verb = RunAs; //Elevate Window

            process.Start();
            string s = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            using (StreamWriter outfile = new StreamWriter("StandardOutput.txt", true))
            {
                outfile.Write(s);
            }*/
        }
    }
}
