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

        try
        {
            using (var afr = new AudioFileReader(audioPath))
            using (var outputDevice = new WaveOutEvent())
            {
                outputDevice.Init(afr);
                outputDevice.Play();

                Console.WriteLine("\nPlaying audio...");

                // くらりちゃんの画像を読み込む
                KurariChan kChan = new KurariChan("./assets/image/Kyukurarin.jpg");

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
