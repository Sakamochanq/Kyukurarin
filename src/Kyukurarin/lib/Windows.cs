using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class Windows
{
    private readonly PerformanceCounter _cpuCounter;
    private readonly PerformanceCounter _ramCounter;

    public Windows()
    {
        // CPU 使用率
        _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        _cpuCounter.NextValue();

        // 空きメモリ
        _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
    }

    // 起動時の状態
    public void DisplayInfo(string TITLE_TEXT)
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"\n[{TITLE_TEXT}] Windows Logger\n");

        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine("＼(°_o)／ < aa kaseki ni nattimauyo ... ♪\n");

        foreach (Screen screen in Screen.AllScreens)
        {
            Console.WriteLine($"  [Display]     : {screen.DeviceName}");
            Console.WriteLine($"  [Bounds]      : {screen.Bounds.Width}x{screen.Bounds.Height}");
            Console.WriteLine($"  [WorkingArea] : {screen.WorkingArea.Width}x{screen.WorkingArea.Height}");
            Console.WriteLine($"  [Primary]     : {screen.Primary}\n");

            // FPS
            DEVMODE dm = new DEVMODE();
            dm.dmSize = (short)Marshal.SizeOf(typeof(DEVMODE));
            if (EnumDisplaySettings(screen.DeviceName, -1, ref dm))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"  [Frewuency]   : {dm.dmDisplayFrequency}");
            }
        }

        // CPU 使用率
        System.Threading.Thread.Sleep(500); // 補正
        float cpuUsage = _cpuCounter.NextValue();

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  [CpuUsage]    : {cpuUsage:0.0} %");

        // 空きメモリ
        float freeMem = _ramCounter.NextValue();

        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"  [Memory]      : {freeMem} MB");
        
        // 文字カラーのリセット
        Console.ForegroundColor = ConsoleColor.White;
    }


    // WinAPIからディスプレイ情報取得
    [StructLayout(LayoutKind.Sequential)]
    private struct DEVMODE
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string dmDeviceName;
        public short dmSpecVersion;
        public short dmDriverVersion;
        public short dmSize;
        public short dmDriverExtra;
        public int dmFields;
        public int dmPositionX;
        public int dmPositionY;
        public int dmDisplayOrientation;
        public int dmDisplayFixedOutput;
        public short dmColor;
        public short dmDuplex;
        public short dmYResolution;
        public short dmTTOption;
        public short dmCollate;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)] public string dmFormName;
        public short dmLogPixels;
        public int dmBitsPerPel;
        public int dmPelsWidth;
        public int dmPelsHeight;
        public int dmDisplayFlags;
        public int dmDisplayFrequency;
        public int dmICMMethod;
        public int dmICMIntent;
        public int dmMediaType;
        public int dmDitherType;
        public int dmReserved1;
        public int dmReserved2;
        public int dmPanningWidth;
        public int dmPanningHeight;
    }

    [DllImport("user32.dll")]
    private static extern bool EnumDisplaySettings(string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);
}