using System;
using System.Collections.Generic;
using System.IO;
using NAudio.Wave;

namespace MusicPlayer
{
    class Program
    {
        static void Main(string[] args)
        {
            MusicService musicService = new MusicService();

            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("=== Main Menu ===");
                Console.WriteLine("1. Personal Profile"); // Опція: Особистий профіль
                Console.WriteLine("2. Listening history"); // Опція: Історія прослуховування
                Console.WriteLine("3. Play music"); // Опція: Відтворити музику
                Console.WriteLine("4. Exit"); // Опція: Вихід
                Console.Write("Choose an option: "); // Запит на вибір опції

                string option = Console.ReadLine();
                Console.WriteLine();

                switch (option)
                {
                    case "1":
                        musicService.PersonalProfile(); // Виклик методу особистого профілю
                        break;
                    case "2":
                        musicService.PlaybackHistory(); // Виклик методу історії прослуховування
                        break;
                    case "3":
                        musicService.PlayMusic(); // Виклик методу відтворення музики
                        break;
                    case "4":
                        exit = true; // Встановлення прапорця для виходу з циклу
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Try again."); // Вивід повідомлення про неправильний вибір
                        break;
                }

                Console.WriteLine();
            }
        }
    }

    class MusicService
    {
        private List<string> playbackHistory; // Список для збереження історії прослуховування
        private string[] artists; // Масив для збереження списку виконавців
        private string[] musicFiles; // Масив для збереження списку музичних файлів
        private bool isPlaying; // Прапорець для відстеження поточного стану відтворення музики

        private WaveOutEvent outputDevice; // Об'єкт для відтворення звуку
        private AudioFileReader audioFile; // Об'єкт для читання аудіофайлу

        private void StopMusic()
        {
            Console.WriteLine("The song is stopped."); // Виведення повідомлення про зупинку пісні

            outputDevice.Stop(); // Зупинка відтворення звуку
            outputDevice.Dispose(); // Звільнення ресурсів WaveOutEvent
            audioFile.Dispose(); // Звільнення ресурсів AudioFileReader

            isPlaying = false; // Зупинка відтворення пісні
        }

        private void PlaySongs(string[] songs)
        {
            Console.WriteLine($"List of songs by the selected artist:"); // Список пісень вибраного виконавця:
            for (int i = 0; i < songs.Length; i++)
            {
                Console.WriteLine($"{i + 1}. {Path.GetFileNameWithoutExtension(songs[i])}"); // Виведення списку пісень вибраного виконавця
            }

            Console.Write("Select a song number to play: "); // Виберіть номер пісні для відтворення
            string songInput = Console.ReadLine();
            if (int.TryParse(songInput, out int songNumber) && songNumber > 0 && songNumber <= songs.Length)
            {
                string selectedSong = songs[songNumber - 1];
                PlaySong(selectedSong); // Відтворення вибраної пісні
            }
            else
            {
                Console.WriteLine("Invalid song number."); // Недійсний номер пісні.
            }
        }

        private void PlaySong(string songPath)
        {
            string songName = Path.GetFileNameWithoutExtension(songPath);
            Console.WriteLine($"Playing song: {songName}"); // Виведення повідомлення про відтворення пісні

            playbackHistory.Add(songName); // Додавання пісні до історії прослуховування

            // Створення об'єкту WaveOutEvent для відтворення звуку
            outputDevice = new WaveOutEvent();
            // Створення об'єкту AudioFileReader для читання аудіофайлу
            audioFile = new AudioFileReader(songPath);

            // Підключення обробника події для визначення кінця відтворення пісні
            outputDevice.PlaybackStopped += (sender, e) =>
            {
                StopMusic();
            };

            outputDevice.Init(audioFile); // Ініціалізація WaveOutEvent і AudioFileReader
            outputDevice.Play(); // Відтворення звуку
            isPlaying = true; // Встановлення прапорця відтворення
        }
        public MusicService()
        {
            playbackHistory = new List<string>();
            artists = Directory.GetDirectories("music"); // Отримання списку папок виконавців з папки "music"
            musicFiles = Directory.GetFiles("music", "*.mp3"); // Отримання списку музичних файлів з папки "music"
            isPlaying = false; // Початкове значення прапорця відтворення
        }

        public void PersonalProfile()
        {
            Console.WriteLine("=== Personal Profile ===");
            // TODO: Implement personal profile functionality
        }

        public void PlaybackHistory()
        {
            Console.WriteLine("=== Listening History ===");
            foreach (string song in playbackHistory)
            {
                Console.WriteLine(song); // Виведення кожної пісні з історії прослуховування
            }
        }

        public void PlayMusic()
        {
            if (isPlaying)
            {
                Console.WriteLine("Stop current song? (Y/N)"); // Запит на зупинку поточної пісні
                string stop = Console.ReadLine();
                if (stop.ToUpper() == "Y")
                {
                    StopMusic(); // Зупинка поточної пісні
                }
            }
            else
            {
                Console.WriteLine("=== Play music ===");
                Console.WriteLine("List of available artists:");
                for (int i = 0; i < artists.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {Path.GetFileName(artists[i])}");
                }

                Console.Write("Select an artist number: "); // Виберіть номер виконавця:
                string artistInput = Console.ReadLine();
                if (int.TryParse(artistInput, out int artistNumber) && artistNumber > 0 && artistNumber <= artists.Length)
                {
                    string selectedArtist = Path.GetFileName(artists[artistNumber - 1]);
                    Console.WriteLine($"Selected artist: {selectedArtist}"); // Обраний виконавець:

                    // Get songs from the selected artist's folder
                    string[] artistSongs = GetSongsForArtist(selectedArtist);
                    if (artistSongs.Length > 0)
                    {
                        PlaySongs(artistSongs);
                    }
                    else
                    {
                        Console.WriteLine("No songs found for the selected artist.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid artist number."); // Недійсний номер виконавця.
                }
            }
        }

        private string[] GetSongsForArtist(string artist)
        {
            string artistFolderPath = Path.Combine("music", artist);
            if (Directory.Exists(artistFolderPath))
            {
                string[] songs = Directory.GetFiles(artistFolderPath, "*.mp3");
                if (songs.Length > 5)
                {
                    Array.Resize(ref songs, 5); // Limit the number of songs to 5
                }
                return songs;
            }
            else
            {
                Console.WriteLine($"Artist '{artist}' not found."); // Виконавець '{artist}' не знайдений.
                return new string[0];
            }
        }
    }
}
