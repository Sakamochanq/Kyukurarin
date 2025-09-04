using System.Threading;
using NAudio.Wave;
using System;

public class Program
{
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

                Console.WriteLine("Playing audio...");

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
