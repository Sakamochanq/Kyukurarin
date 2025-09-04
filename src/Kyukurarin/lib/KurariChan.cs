using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

public class KurariChan : Form
{
    private readonly Image _image;

    public KurariChan(string imagePath)
    {
        if (!System.IO.File.Exists(imagePath))
            throw new ArgumentException("Not Found this Image: " + imagePath);

        _image = Image.FromFile(imagePath);

        FormBorderStyle = FormBorderStyle.None;
        ShowInTaskbar = false;

        //最前面配置 or 最背面配置のトリガー
        TopMost = true;

        Bounds = Screen.PrimaryScreen.Bounds;

        // 背景を透過させる
        BackColor = Color.Black;
        TransparencyKey = Color.Black;

        // デスクトップのアイコン背面に埋め込む
        EmbedBehindDesktopIcons();
    }

    // Form上に画像の描画
    protected override void OnPaint(PaintEventArgs e)
    {
        base.OnPaint(e);

        int x = (Width - _image.Width) / 2;
        int y = (Height - _image.Height) / 2;

        e.Graphics.DrawImage(_image, x, y);
    }

    // WorkerW を差し替える
    private void EmbedBehindDesktopIcons()
    {
        IntPtr progman = FindWindow("Progman", null);

        // WorkerWを作成
        SendMessageTimeout(progman, 0x052C, IntPtr.Zero, IntPtr.Zero,
                           SendMessageTimeoutFlags.SMTO_NORMAL, 1000, out _);

        // EnumWindows で WorkerW を探す
        EnumWindows((hWnd, lParam) =>
        {
            IntPtr shellView = FindWindowEx(hWnd, IntPtr.Zero, "SHELLDLL_DefView", null);
            if (shellView != IntPtr.Zero)
            {
                // WorkerW があった！
                IntPtr workerW = FindWindowEx(IntPtr.Zero, hWnd, "WorkerW", null);
                if (workerW != IntPtr.Zero)
                {
                    SetParent(this.Handle, workerW);
                    return false;
                }
            }
            return true;
        }, IntPtr.Zero);
    }

    // WinAPIの呼び出し
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
