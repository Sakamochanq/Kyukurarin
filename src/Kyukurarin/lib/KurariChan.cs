using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class KurariChan : Form
{
    private readonly Image _image;

    // 任意の画像用コンストラクタ
    public KurariChan(bool captureDesktop, float scale = 0.8f)
    {
        if (!captureDesktop)
            throw new ArgumentException("captureDesktop must be true to capture the desktop");

        // デスクトップ取得
        Image desktopImage = CaptureDesktop();

        // リサイズ
        int newWidth = (int)(desktopImage.Width * scale);
        int newHeight = (int)(desktopImage.Height * scale);
        _image = new Bitmap(desktopImage, newWidth, newHeight);

        InitializeForm();
    }

    // スクリーンショット用コンストラクタ
    public KurariChan(bool captureDesktop)
    {
        if (captureDesktop)
        {
            _image = CaptureDesktop();
        }
        else
        {
            throw new ArgumentException("captureDesktop must be true to capture the desktop");
        }
        InitializeForm();
    }

    // 共通初期化
    private void InitializeForm()
    {
        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;
        TopMost = false;
        Bounds = Screen.PrimaryScreen.Bounds;

        // 背景透過
        BackColor = Color.Black;
        TransparencyKey = Color.Black;

        // デスクトップのアイコン背面に埋め込む
        EmbedBehindDesktopIcons();
    }

    // Form上に画像を描画
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);
        int x = (Width - _image.Width) / 2;
        int y = (Height - _image.Height) / 2;
        e.Graphics.DrawImage(_image, x, y);
    }

    // デスクトップスクリーンショット取得
    private Image CaptureDesktop()
    {
        int width = Screen.PrimaryScreen.Bounds.Width;
        int height = Screen.PrimaryScreen.Bounds.Height;
        Bitmap bmp = new Bitmap(width, height);

        using (Graphics g = Graphics.FromImage(bmp))
        {
            g.CopyFromScreen(0, 0, 0, 0, bmp.Size);
        }

        return bmp;
    }

    // デスクトップ背面に埋め込む
    private void EmbedBehindDesktopIcons()
    {
        IntPtr progman = FindWindow("Progman", null);

        // WorkerW 作成
        SendMessageTimeout(progman, 0x052C, IntPtr.Zero, IntPtr.Zero,
                           SendMessageTimeoutFlags.SMTO_NORMAL, 1000, out _);

        EnumWindows((hWnd, lParam) =>
        {
            IntPtr shellView = FindWindowEx(hWnd, IntPtr.Zero, "SHELLDLL_DefView", null);
            if (shellView != IntPtr.Zero)
            {
                IntPtr workerW = FindWindowEx(IntPtr.Zero, hWnd, "WorkerW", null);
                if (workerW != IntPtr.Zero)
                {
                    SetParent(this.Handle, workerW);
                    return false; // 見つけたら終了
                }
            }
            return true; // 続行
        }, IntPtr.Zero);
    }

    // WinAPI
    [DllImport("user32.dll")] private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
    [DllImport("user32.dll")] private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
    [DllImport("user32.dll")] private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
    [DllImport("user32.dll")] private static extern IntPtr SendMessageTimeout(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam, SendMessageTimeoutFlags fuFlags, uint uTimeout, out IntPtr lpdwResult);
    [DllImport("user32.dll")] private static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);

    private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

    [Flags]
    private enum SendMessageTimeoutFlags : uint
    {
        SMTO_NORMAL = 0x0000
    }
}
