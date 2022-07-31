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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace ITATKWinUI
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Reporting : Page
    {
        System.Collections.ObjectModel.ObservableCollection<ReportingData> ReportData = new System.Collections.ObjectModel.ObservableCollection<ReportingData>();

        public Reporting()
        {
            this.InitializeComponent();

            var records = MainWindow.GetReportingRecords();

            foreach(ReportingRecord record in records)
            {
                ReportData.Add(new ReportingData(record.ScriptName, record.ScriptPath, record.HostExecuting, record.FileExtension, record.Target, record.UserExecuting, record.DateTime));
            }
        }
    }

    public class ReportingData
    {
        public string ScriptName { get; set; }
        public string ScriptPath { get; set; }
        public string HostExecuting { get; set; }
        public string FileExtension { get; set; }
        public string Target { get; set; }
        public string UserExecuting { get; set; }
        public DateTime DateTime { get; set; }

        public ReportingData(string ScriptName, string ScriptPath, string HostExecuting, string FileExtension, string Target, string UserExecuting, DateTime DateTime)
        {
            this.ScriptName = ScriptName;
            this.ScriptPath = ScriptPath;
            this.HostExecuting = HostExecuting;
            this.FileExtension = FileExtension;
            this.Target = Target;
            this.UserExecuting = UserExecuting;
            this.DateTime = DateTime;
        }
    }
}
