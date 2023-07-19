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
                Console.WriteLine("=== Main Menu ==="); // Головне меню
                Console.WriteLine("1. Personal Profile"); // Особистий профіль
                Console.WriteLine("2. Listening history"); // Історія прослуховування
                Console.WriteLine("3. Play music"); // Відтворити музику
                Console.WriteLine("4. Exit"); // Вийти
                Console.Write("Choose an option: "); // Виберіть опцію

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
                        Console.WriteLine("Invalid selection. Try again."); // Недійсний вибір. Спробуйте ще раз.
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
        private bool isPlaying; // Прапорець для відстеження поточного стану відтворення музики

        public MusicService()
        {
            playbackHistory = new List<string>();
            artists = Directory.GetDirectories("music"); // Отримання списку папок виконавців з папки "music"
            isPlaying = false; // Початкове значення прапорця відтворення
        }

        public void PersonalProfile()
        {
            Console.WriteLine("=== Personal Profile ==="); // Особистий профіль
            // TODO: Implement personal profile functionality
        }

        public void PlaybackHistory()
        {
            Console.WriteLine("=== Listening History ==="); // Історія прослуховування
            foreach (string song in playbackHistory)
            {
                Console.WriteLine(song); // Виведення кожної пісні з історії прослуховування
            }
        }

        public void PlayMusic()
        {
            if (isPlaying)
            {
                Console.WriteLine("Stop current song? (Yes/No)"); // Зупинити поточну пісню? (Так/Ні)
                string stop = Console.ReadLine();
                if (stop.ToUpper() == "YES") // ТАК
                {
                    StopMusic(); // Зупинити поточну пісню
                }
            }
            else
            {
                Console.WriteLine("=== Play music ==="); // Відтворити музику
                Console.WriteLine("List of available artists:"); // Список доступних виконавців:

                for (int i = 0; i < artists.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {Path.GetFileName(artists[i])}");
                }

                Console.Write("Select an artist number: "); // Виберіть номер виконавця:
                string artistInput = Console.ReadLine();
                if (int.TryParse(artistInput, out int artistNumber) && artistNumber > 0 && artistNumber <= artists.Length)
                {
                    string selectedArtist = Path.GetFileName(artists[artistNumber - 1]);
                    Console.WriteLine($"Selected artist: {selectedArtist}"); // Обраний виконавець:Console.WriteLine($"List of songs by {selectedArtist}:"); // Список пісень {selectedArtist}:
                    string[] songs = GetSongsForArtist(selectedArtist);

                    for (int i = 0; i < songs.Length; i++)
                    {
                        Console.WriteLine($"{i + 1}. {songs[i]}");
                    }

                    Console.Write("Select a song number: "); // Виберіть номер пісні:
                    string songInput = Console.ReadLine();
                    if (int.TryParse(songInput, out int songNumber) && songNumber > 0 && songNumber <= songs.Length)
                    {
                        string selectedSong = songs[songNumber - 1];
                        Console.WriteLine($"Playing song: {selectedSong}"); // Відтворення пісні:
                    }
                    else
                    {
                        Console.WriteLine("Invalid song number."); // Недійсний номер пісні.
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
                return Directory.GetFiles(artistFolderPath, "*.mp3");
            }
            else
            {
                Console.WriteLine($"Artist '{artist}' not found."); // Виконавець '{artist}' не знайдений.
                return new string[0];
            }
        }

        private void StopMusic()
        {
            Console.WriteLine("The song is stopped."); // Пісню зупинено.
            isPlaying = false; // Зупинка відтворення пісні
        }
    }
}