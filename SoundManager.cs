using NAudio.Wave;
using System;
using System.IO;

internal static class SoundManager
{
    private static WaveOutEvent moveOutput;
    private static AudioFileReader moveSound;

    private static WaveOutEvent musicOutput;
    private static AudioFileReader musicReader;

    public static event Action<string>? OnError;

    public static void PlayMoveSound()
    {
        try
        {
            StopMoveSound();

            moveSound = new AudioFileReader("step.mp3");
            moveOutput = new WaveOutEvent();
            moveOutput.Init(moveSound);
            moveOutput.Play();
        }
        catch (Exception ex)
        {
            RaiseError(ex.Message);
        }
    }

    public static void PlayBattleMusic()
    {
        try
        {
            StopBattleMusic();

            string path = "battle.mp3";

            if (!File.Exists(path))
            {
                RaiseError($"Файл {path} не знайдено.");
                return;
            }

            musicReader = new AudioFileReader(path);
            musicOutput = new WaveOutEvent();
            musicOutput.Init(musicReader);
            musicOutput.Play();

            musicOutput.PlaybackStopped += (s, e) =>
            {
                try
                {
                    if (musicReader != null && musicOutput != null &&
                        musicOutput.PlaybackState != PlaybackState.Stopped)
                    {
                        musicReader.Position = 0;
                        musicOutput.Play();
                    }
                }
                catch (Exception ex)
                {
                    RaiseError(ex.Message);
                }
            };
        }
        catch (Exception ex)
        {
            RaiseError(ex.Message);
        }
    }

    public static void StopBattleMusic()
    {
        try
        {
            musicOutput?.Stop();
            musicReader?.Dispose();
            musicOutput?.Dispose();
        }
        catch (Exception ex)
        {
            RaiseError(ex.Message);
        }
        finally
        {
            musicReader = null;
            musicOutput = null;
        }
    }

    public static void StopMoveSound()
    {
        moveOutput?.Stop();
        moveSound?.Dispose();
        moveOutput?.Dispose();
        moveSound = null;
        moveOutput = null;
    }
    private static void RaiseError(string message)
    {
        OnError?.Invoke($"[Sound error]: {message}");
    }

}
