using System;
using System.Collections.Generic;
using System.IO;

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
        private string[] musicFiles; // Масив для збереження списку музичних файлів
        private bool isPlaying; // Прапорець для відстеження поточного стану відтворення музики

        public MusicService()
        {
            playbackHistory = new List<string>();
            musicFiles = Directory.GetFiles("music", "*.mp3"); // Отримання списку музичних файлів з папки "music"
            isPlaying = false; // Початкове значення прапорця відтворення
        }

        public void PersonalProfile()
        {
            Console.WriteLine("=== Personal Profile ===");
            // TODO: Реалізуйте функціонал особистого профілю
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
                Console.WriteLine("List of available songs:");
                for (int i = 0; i < musicFiles.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {Path.GetFileNameWithoutExtension(musicFiles[i])}"); // Виведення списку доступних пісень
                }

                Console.Write("Select a song number to play: "); // Запит на вибір номеру пісні для відтворення
                string input = Console.ReadLine();
                if (int.TryParse(input, out int songNumber) && songNumber > 0 && songNumber <= musicFiles.Length)
                {
                    PlaySong(songNumber - 1); // Відтворення вибраної пісні
                }
                else
                {
                    Console.WriteLine("Wrong song selection."); // Повідомлення про неправильний вибір пісні
                }
            }
        }

        private void PlaySong(int songIndex)
        {
            string songName = Path.GetFileNameWithoutExtension(musicFiles[songIndex]);
            Console.WriteLine($"Playing song: {songName}"); // Виведення повідомлення про відтворення пісні

            playbackHistory.Add(songName); // Додавання пісні до історії прослуховування
            isPlaying = true; // Встановлення прапорця відтворення
        }

        private void StopMusic()
        {
            Console.WriteLine("The song is stopped."); // Виведення повідомлення про зупинку пісні

            isPlaying = false; // Зупинка відтворення пісні
        }
    }
}
