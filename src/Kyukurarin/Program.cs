using System.Threading;
using NAudio.Wave;
using System;


public class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        //Console.WriteLine("Hello Kyukurarin");

        string audioPath = "./assets/audio/Kyukurarin.wav";
        string Title = "Kyukurarin for Windows by Sakamochanq";
        string Kyukurain = "./assets/image/Kyukurarin.jpg";

        try
        {
            using (var afr = new AudioFileReader(audioPath))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(afr);
                outputDevice.Play();

                //Console.WriteLine("\nPlaying audio...\n");

                // 画面情報の取得と表示
                Windows windows = new Windows();
                windows.DisplayInfo(Title);

                // くらりちゃんの画像を読み込む（False:任意の画像　True:スクリーンショット）
                KurariChan kChan = new KurariChan(true, 0.5f);

                //System.Windows.Forms.Application.EnableVisualStyles();
                //System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

                System.Windows.Forms.Application.Run(kChan);

                //再生終了まで待機
                while (outputDevice.PlaybackState == PlaybackState.Playing)
                {
                    Thread.Sleep(100);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
