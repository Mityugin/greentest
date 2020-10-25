using System;
using System.Windows;
using System.Windows.Media;
using System.Management;
using System.Diagnostics;
using System.Windows.Navigation;

namespace greentest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            TEST();

        }

        private void TEST()
        {
            TextBox1.Clear();
            SolidColorBrush mySolidColorBrush = new SolidColorBrush
            {
                Color = Color.FromRgb(0, 255, 0)
            };
            CPU_Ellipse.Fill = mySolidColorBrush;
            MEM_Ellipse.Fill = mySolidColorBrush;
            DISK_Ellipse.Fill = mySolidColorBrush;
            string NamespacePath = "\\\\.\\ROOT\\cimv2";
            
            string ClassName = "Win32_PerfFormattedData_PerfOS_Processor";
            ManagementClass oClass = new ManagementClass(NamespacePath + ":" + ClassName);
            foreach (ManagementObject oObject in oClass.GetInstances())
            {
                var usage = oObject["PercentProcessorTime"];
                var name = Convert.ToString( oObject["Name"]);
                if (Equals(name, "_Total"))
                {
                    CPU.Content = usage+" %";
                    TextBox1.AppendText("CPU Usage: " + usage + " %");
                    TextBox1.AppendText(Environment.NewLine);
                    if (Convert.ToInt32(usage) > 90)
                    {
                        SolidColorBrush CPUSolidColorBrush = new SolidColorBrush { Color = Color.FromRgb(255, 0, 0) };
                        CPU_Ellipse.Fill = CPUSolidColorBrush;

                    }
                }
                
            }

            ClassName = "Win32_OperatingSystem";
            oClass = new ManagementClass(NamespacePath + ":" + ClassName);
            foreach (ManagementObject oObject in oClass.GetInstances())
            {
                int FreeMem = Convert.ToInt32(oObject["FreePhysicalMemory"]);
                int TotalMem = Convert.ToInt32(oObject["TotalVisibleMemorySize"]);
                MEMORY .Content = FreeMem / (1024 * 1024)+" GB";

                TextBox1.AppendText("Total Visible Memory: " + TotalMem / (1024 * 1024) + "GB");
                TextBox1.AppendText(Environment.NewLine);
                TextBox1.AppendText("Free Physical Memory: "+ FreeMem / (1024 * 1024)+"GB");
                TextBox1.AppendText(Environment.NewLine);

                if (FreeMem< TotalMem/10)
                {
                    SolidColorBrush MEMSolidColorBrush = new SolidColorBrush { Color = Color.FromRgb(255, 0, 0) };
                    MEM_Ellipse .Fill = MEMSolidColorBrush;
                }
            }

            ClassName = "Win32_DiskDrive";
            oClass = new ManagementClass(NamespacePath + ":" + ClassName);
            foreach (ManagementObject oObject in oClass.GetInstances())
            {
                var sign = Convert.ToString(oObject["Signature"]);
                var model = Convert.ToString(oObject["Model"]);
                var status = Convert.ToString(oObject["Status"]);

                if (Equals(sign,""))
                {
                    TextBox1.AppendText("DISK model: " + model);
                    TextBox1.AppendText(Environment.NewLine);
                    TextBox1.AppendText("Status: " + status);
                    TextBox1.AppendText(Environment.NewLine);
                    DISK.Content = status;

                    if (!status.Equals("OK") || !model.Contains("SSD"))
                    {
                        SolidColorBrush DISKSolidColorBrush = new SolidColorBrush { Color = Color.FromRgb(255, 0, 0) };
                        DISK_Ellipse.Fill = DISKSolidColorBrush;
                    }
                }


            }

        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start(e.Uri.ToString());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            TEST();
        }


    }
}
