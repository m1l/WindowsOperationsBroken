using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace WindowsOperationsBroken
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string baseDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

            string dummyFileName = "DummyFile.bin";
            string dummyFilePath = Path.Combine(baseDirectory, dummyFileName);

            string destinationDirectory = Path.Combine(baseDirectory, "DestinationDir");
            string destinationFilePath = Path.Combine(destinationDirectory, dummyFileName);            

            // Ensure destination directory exists
            Directory.CreateDirectory(destinationDirectory);

            // Create dummy file if it doesn't exist
            if (!File.Exists(dummyFilePath))
                CreateDummyFile(dummyFilePath);

            // Delete destination file if it already exists
            if (File.Exists(destinationFilePath))
                File.Delete(destinationFilePath);

            // Perform the copy operation
            DoCopy(dummyFilePath, destinationDirectory, this, (bool)UseWin7Dialogs.IsChecked);
        }

        

        internal static void DoCopy(string file, string destDir, Window owner, bool useWindows7Dialogs)
        {
            var wih = new System.Windows.Interop.WindowInteropHelper(owner);
            var hWnd = wih.Handle;

            var thread = new System.Threading.Thread(() =>
            {   
                try
                {
                    using (FileOperation fileOp = new FileOperation(null, hWnd))
                    {
                        FileOperationFlags flags = FileOperationFlags.FOFX_ADDUNDORECORD | FileOperationFlags.FOF_ALLOWUNDO;
                        if (useWindows7Dialogs)
                            flags |= FileOperationFlags.FOFX_NOMINIMIZEBOX;
                        fileOp.SetOperationFlags(flags);
                        fileOp.CopyItem(file, destDir, string.Empty);
                        fileOp.PerformOperations();
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Error during file operation: " + ex.Message);
                }       
            });
            thread.IsBackground = false;
            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
        }

        //version with message pump, suggested by ChatGPT, makes no difference
        internal static void DoCopyWithPump(string file, string destDir, Window owner, bool useWindows7Dialogs)
        {
            var wih = new System.Windows.Interop.WindowInteropHelper(owner);
            var hWnd = wih.Handle;

            var thread = new System.Threading.Thread(() =>
            {
                var disp = System.Windows.Threading.Dispatcher.CurrentDispatcher;

                disp.BeginInvoke((Action)(() =>
                {
                    try
                    {
                        using (FileOperation fileOp = new FileOperation(null, hWnd))
                        {
                            FileOperationFlags flags = FileOperationFlags.FOFX_ADDUNDORECORD | FileOperationFlags.FOF_ALLOWUNDO;
                            if (useWindows7Dialogs)
                                flags |= FileOperationFlags.FOFX_NOMINIMIZEBOX;
                            fileOp.SetOperationFlags(flags);
                            fileOp.CopyItem(file, destDir, string.Empty);
                            fileOp.PerformOperations();
                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error during file operation: " + ex.Message);
                    }
                    finally
                    {
                        disp.BeginInvokeShutdown(
                            System.Windows.Threading.DispatcherPriority.Background);
                    }

                }));


                System.Windows.Threading.Dispatcher.Run();
            });
            thread.IsBackground = false;
            thread.SetApartmentState(System.Threading.ApartmentState.STA);
            thread.Start();
        }

        public static void CreateDummyFile(string filePath)
        {
            const long sizeInBytes = 3L * 1000 * 1000 * 1000; // 1 GB

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    fs.SetLength(sizeInBytes);
                }

                Debug.WriteLine("Dummy file created: " + filePath);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error creating dummy file: " + ex.Message);
            }
        }
    }
}