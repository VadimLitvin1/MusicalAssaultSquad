using NAudio.Wave;

namespace MusicPlayerApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles(); // Включення візуальних стилів для програми
            Application.SetCompatibleTextRenderingDefault(false); // Встановлення сумісного рендерингу тексту для програми
            Application.Run(new MainForm()); // Запуск головної форми програми
        }
    }

    public partial class MainForm : Form
    {
        private MusicService musicService; // Сервіс для роботи з музикою
        private ListBox artistsListBox; // Список для відображення виконавців
        private ListBox songsListBox; // Список для відображення пісень
        private Button personalProfileButton; // Кнопка "Особистий профіль"
        private Button playbackHistoryButton; // Кнопка "Історія програвання"
        private Button playMusicButton; // Кнопка "Відтворити музику"
        private UserProfile userProfile; // Об'єкт для збереження користувальницького профілю

        public MainForm()
        {
            InitializeComponent();
            musicService = new MusicService(); // Ініціалізація об'єкту музичного сервісу
            userProfile = new UserProfile(); // Ініціалізація об'єкту користувальницького профілю
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            musicService.LoadMusicFiles(); // Завантаження музичних файлів
            artistsListBox.DataSource = musicService.GetArtists(); // Заповнення списку виконавців даними
        }

        private void personalProfileButton_Click(object sender, EventArgs e)
        {
            UserProfileForm profileForm = new UserProfileForm(userProfile); // Створення форми для редагування особистого профілю
            profileForm.ShowDialog(); // Відображення форми модально

            if (profileForm.DialogResult == DialogResult.OK)
            {
                // Оновлення імені користувача на головному екрані
                this.Text = $"TuneFlow - {userProfile.Username}";
            }
        }

        private void playbackHistoryButton_Click(object sender, EventArgs e)
        {
            PlaybackHistoryForm historyForm = new PlaybackHistoryForm(musicService.GetPlaybackHistory()); // Створення форми для відображення історії програвання
            historyForm.ShowDialog(); // Відображення форми модально
        }

        private void playMusicButton_Click(object sender, EventArgs e)
        {
            if (musicService.IsPlaying) // Якщо музика вже програєвається
            {
                var result = MessageBox.Show("Stop the current song?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question); // Підтвердження зупинки поточної пісні
                if (result == DialogResult.Yes)
                {
                    musicService.StopMusic(); // Зупинка музики
                }
            }
            else // Якщо музика не програєвається
            {
                if (songsListBox.SelectedIndex >= 0) // Якщо вибрано пісню зі списку
                {
                    int songNumber = songsListBox.SelectedIndex; // Отримання номеру вибраної пісні
                    musicService.PlaySong(songNumber); // Відтворення вибраної пісні
                    MessageBox.Show($"Playing song: {musicService.GetCurrentSong()}"); // Відображення повідомлення про те, що відтворюється пісня
                }
                else
                {
                    MessageBox.Show("No song selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Виведення повідомлення про помилку, якщо пісня не вибрана
                }
            }
        }

        private void artistsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedArtist = artistsListBox.SelectedItem.ToString(); // Отримання вибраного виконавця зі списку
            musicService.SetSelectedArtist(selectedArtist); // Встановлення вибраного виконавця в музичному сервісі
            songsListBox.DataSource = musicService.GetSongsBySelectedArtist(); // Заповнення списку пісень вибраного виконавця даними
        }

        private void InitializeComponent()
        {
            int buttonWidth = 100; // Початкова ширина кнопки
            int buttonHeight = 30; // Початкова висота кнопки

            this.artistsListBox = new System.Windows.Forms.ListBox(); // Ініціалізація списку для відображення виконавців
            this.songsListBox = new System.Windows.Forms.ListBox(); // Ініціалізація списку для відображення пісень
            this.personalProfileButton = new System.Windows.Forms.Button(); // Ініціалізація кнопки "Особистий профіль"
            this.playbackHistoryButton = new System.Windows.Forms.Button(); // Ініціалізація кнопки "Історія програвання"
            this.playMusicButton = new System.Windows.Forms.Button(); // Ініціалізація кнопки "Відтворити музику"
            this.SuspendLayout();

            // 
            // artistsListBox
            // 
            this.artistsListBox.BackColor = System.Drawing.Color.FromArgb(30, 30, 30); // Встановлення колір фону списку виконавців
            this.artistsListBox.ForeColor = System.Drawing.Color.White; // Встановлення колір тексту списку виконавців
            this.artistsListBox.FormattingEnabled = true; // Встановлення властивості для можливості вибору елементів списку
            this.artistsListBox.Location = new System.Drawing.Point(12, 12); // Встановлення положення списку виконавців на формі
            this.artistsListBox.Name = "artistsListBox"; // Встановлення назви списку виконавців
            this.artistsListBox.Size = new System.Drawing.Size(150, 225); // Встановлення розміру списку виконавців
            this.artistsListBox.TabIndex = 0; // Встановлення порядку переходу до списку виконавців при натисканні клавіші Tab
            this.artistsListBox.SelectedIndexChanged += new System.EventHandler(this.artistsListBox_SelectedIndexChanged); // Підписка на подію зміни вибраного виконавця

            // 
            // songsListBox
            // 
            this.songsListBox.BackColor = System.Drawing.Color.FromArgb(30, 30, 30); // Встановлення колір фону списку пісень
            this.songsListBox.ForeColor = System.Drawing.Color.White; // Встановлення колір тексту списку пісень
            this.songsListBox.FormattingEnabled = true; // Встановлення властивості для можливості вибору елементів списку
            this.songsListBox.Location = new System.Drawing.Point(168, 12); // Встановлення положення списку пісень на формі
            this.songsListBox.Name = "songsListBox"; // Встановлення назви списку пісень
            this.songsListBox.Size = new System.Drawing.Size(150, 225); // Встановлення розміру списку пісень
            this.songsListBox.TabIndex = 1; // Встановлення порядку переходу до списку пісень при натисканні клавіші Tab

            // ...
            // personalProfileButton
            // Кнопка для переходу до особистого профілю користувача
            this.personalProfileButton.BackColor = System.Drawing.Color.FromArgb(255, 128, 0); // Встановлення коліру фону кнопки
            this.personalProfileButton.FlatAppearance.BorderSize = 0; // Встановлення товщини рамки кнопки
            this.personalProfileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat; // Встановлення стилю кнопки без рамки
            this.personalProfileButton.ForeColor = System.Drawing.Color.White; // Встановлення коліру тексту кнопки
            this.personalProfileButton.Location = new System.Drawing.Point(12, 250); // Встановлення положення кнопки на формі
            this.personalProfileButton.Name = "personalProfileButton"; // Встановлення назви кнопки
            this.personalProfileButton.Size = new System.Drawing.Size(100, 30); // Встановлення розміру кнопки
            this.personalProfileButton.TabIndex = 2; // Встановлення порядку переходу до кнопки при натисканні клавіші Tab
            this.personalProfileButton.Text = "Profile"; // Встановлення тексту на кнопці
            this.personalProfileButton.UseVisualStyleBackColor = false; // Встановлення властивості для відображення кольору фону
            this.personalProfileButton.Click += new System.EventHandler(this.personalProfileButton_Click); // Підписка на подію кліку на кнопку "Profile"

            // playbackHistoryButton
            // Кнопка для переходу до історії програвання пісень
            this.playbackHistoryButton.BackColor = System.Drawing.Color.FromArgb(255, 0, 128); // Встановлення коліру фону кнопки
            this.playbackHistoryButton.FlatAppearance.BorderSize = 0; // Встановлення товщини рамки кнопки
            this.playbackHistoryButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat; // Встановлення стилю кнопки без рамки
            this.playbackHistoryButton.ForeColor = System.Drawing.Color.White; // Встановлення коліру тексту кнопки
            this.playbackHistoryButton.Location = new System.Drawing.Point(118, 250); // Встановлення положення кнопки на формі
            this.playbackHistoryButton.Name = "playbackHistoryButton"; // Встановлення назви кнопки
            this.playbackHistoryButton.Size = new System.Drawing.Size(100, 30); // Встановлення розміру кнопки
            this.playbackHistoryButton.TabIndex = 3; // Встановлення порядку переходу до кнопки при натисканні клавіші Tab
            this.playbackHistoryButton.Text = "History"; // Встановлення тексту на кнопці
            this.playbackHistoryButton.UseVisualStyleBackColor = false; // Встановлення властивості для відображення кольору фону
            this.playbackHistoryButton.Click += new System.EventHandler(this.playbackHistoryButton_Click); // Підписка на подію кліку на кнопку "History"

            // playMusicButton
            // Кнопка для відтворення музики
            this.playMusicButton.BackColor = System.Drawing.Color.FromArgb(0, 192, 0); // Встановлення коліру фону кнопки
            this.playMusicButton.FlatAppearance.BorderSize = 0; // Встановлення товщини рамки кнопки
            this.playMusicButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat; // Встановлення стилю кнопки без рамки
            this.playMusicButton.ForeColor = System.Drawing.Color.White; // Встановлення коліру тексту кнопки
            this.playMusicButton.Location = new System.Drawing.Point(224, 250); // Встановлення положення кнопки на формі
            this.playMusicButton.Name = "playMusicButton"; // Встановлення назви кнопки
            this.playMusicButton.Size = new System.Drawing.Size(100, 30); // Встановлення розміру кнопки
            this.playMusicButton.TabIndex = 4; // Встановлення порядку переходу до кнопки при натисканні клавіші Tab
            this.playMusicButton.Text = "Play"; // Встановлення тексту на кнопці
            this.playMusicButton.UseVisualStyleBackColor = false; // Встановлення властивості для відображення кольору фону
            this.playMusicButton.Click += new System.EventHandler(this.playMusicButton_Click); // Підписка на подію кліку на кнопку "Play"

            // MainForm
            // Форма головного вікна програми
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30); // Встановлення коліру фону форми
            this.ClientSize = new System.Drawing.Size(336, 292); // Встановлення розміру форми
            this.Controls.Add(this.playMusicButton); // Додавання кнопки "Play" на форму
            this.Controls.Add(this.playbackHistoryButton); // Додавання кнопки "History" на форму
            this.Controls.Add(this.personalProfileButton); // Додавання кнопки "Profile" на форму
            this.Controls.Add(this.songsListBox); // Додавання списку пісень на форму
            this.Controls.Add(this.artistsListBox); // Додавання списку виконавців на форму
            this.Name = "MainForm"; // Встановлення назви форми
            this.Text = "TuneFlow"; // Встановлення заголовка форми
            this.Load += new System.EventHandler(this.MainForm_Load); // Підписка на подію завантаження форми "Load"
            this.ResumeLayout(false); // Завершення ініціалізації компонентів форми
        }
    }

    public class MusicService
    {
        private List<string> playbackHistory; // Список для збереження історії програвання пісень
        private string[] artists; // Масив для збереження списку виконавців
        private string[] musicFiles; // Масив для збереження списку музичних файлів
        private string selectedArtist; // Зберігає вибраного виконавця
        private bool isPlaying; // Зберігає стан програвання пісень
        private WaveOutEvent outputDevice; // Об'єкт для програвання звуку
        private AudioFileReader audioFile; // Об'єкт для зчитування аудіофайлів

        public MusicService()
        {
            playbackHistory = new List<string>(); // Ініціалізуємо список історії програвання пісень
            artists = Directory.GetDirectories("music").Select(Path.GetFileName).ToArray(); // Отримуємо список виконавців з папки "music"
            isPlaying = false; // Встановлюємо початковий стан програвання як "не програє"
        }

        public void LoadMusicFiles()
        {
            // Завантажуємо музичні файли з папки "music"
            musicFiles = Directory.GetFiles("music", "*.mp3", SearchOption.AllDirectories);
        }

        public string[] GetArtists()
        {
            return artists; // Повертаємо список виконавців
        }

        public void SetSelectedArtist(string artist)
        {
            selectedArtist = artist; // Встановлюємо обраний виконавця
        }

        public string[] GetSongsBySelectedArtist()
        {
            string artistPath = Path.Combine("music", selectedArtist); // Формуємо шлях до папки з вибраним виконавцем
            musicFiles = Directory.GetFiles(artistPath, "*.mp3"); // Отримуємо список музичних файлів для обраного виконавця
            return musicFiles.Select(Path.GetFileNameWithoutExtension).ToArray(); // Повертаємо тільки назви пісень без розширення
        }

        public void PlaySong(int songNumber)
        {
            if (isPlaying)
            {
                outputDevice.Stop(); // Зупиняємо програвання, якщо вже програє
                outputDevice.Dispose(); // Звільняємо ресурси пристрою відтворення
                audioFile.Dispose(); // Звільняємо ресурси аудіофайла
                isPlaying = false; // Встановлюємо стан програвання як "не програє"
            }

            if (songNumber >= 0 && songNumber < musicFiles.Length)
            {
                string selectedSong = musicFiles[songNumber]; // Отримуємо шлях до обраної пісні
                PlaySong(selectedSong); // Програємо пісню за шляхом
                playbackHistory.Add(selectedSong); // Додаємо пісню до історії програвання
            }
        }

        private void PlaySong(string songPath)
        {
            if (File.Exists(songPath))
            {
                audioFile = new AudioFileReader(songPath); // Створюємо новий об'єкт для зчитування аудіофайлу
                outputDevice = new WaveOutEvent(); // Створюємо новий об'єкт для програвання звуку
                outputDevice.PlaybackStopped += (sender, e) => StopMusic(); // Додаємо обробник події, який виконається при завершенні програвання
                outputDevice.Init(audioFile); // Ініціалізуємо пристрій відтворення з аудіофайлом
                outputDevice.Play(); // Починаємо програвати аудіофайл
                isPlaying = true; // Встановлюємо стан програвання як "програє"
            }
            else
            {
                MessageBox.Show("The selected song is not available.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void StopMusic()
        {
            if (isPlaying)
            {
                outputDevice.Stop(); // Зупиняємо програвання
                outputDevice.Dispose(); // Звільняємо ресурси пристрою відтворення
                audioFile.Dispose(); // Звільняємо ресурси аудіофайла
                isPlaying = false; // Встановлюємо стан програвання як "не програє"
            }
        }

        public bool IsPlaying
        {
            get { return isPlaying; } // Повертаємо поточний стан програвання
        }

        public string GetCurrentSong()
        {
            // Додайте ваш код для отримання назви поточної пісні, що програється.
            // Зараз я просто повертаю фразу-заповнювач.
            return "No song is playing.";
        }

        public List<string> GetPlaybackHistory()
        {
            return playbackHistory; // Повертаємо список історії програвання
        }
    }
}


public class UserProfile
{
    public string Username { get; set; } // Властивість для збереження імені користувача

    // Можна додати інші властивості профілю користувача, такі як ім'я, прізвище, електронна пошта тощо.
}

public class UserProfileForm : Form
{
    private UserProfile userProfile; // Об'єкт для збереження профілю користувача
    private TextBox usernameTextBox; // Текстове поле для введення імені користувача
    private Button saveButton; // Кнопка для збереження змін профілю користувача

    public UserProfileForm(UserProfile userProfile)
    {
        this.userProfile = userProfile; // Приймаємо профіль користувача, переданий у конструктор
        InitializeComponent();
        usernameTextBox.Text = userProfile.Username; // Встановлюємо текст в текстовому полі з іменем користувача
    }

    private void InitializeComponent()
    {
        int buttonWidth = 100; // Початкова ширина кнопки
        int buttonHeight = 30; // Початкова висота кнопки

        this.usernameTextBox = new System.Windows.Forms.TextBox(); // Створення текстового поля для імені користувача
        this.saveButton = new System.Windows.Forms.Button(); // Створення кнопки для збереження профілю користувача
        this.SuspendLayout();

        // 
        // usernameTextBox
        // 
        this.usernameTextBox.Location = new System.Drawing.Point(12, 12); // Встановлюємо положення текстового поля на формі
        this.usernameTextBox.Name = "usernameTextBox"; // Встановлюємо назву текстового поля
        this.usernameTextBox.Size = new System.Drawing.Size(200, 20); // Встановлюємо розмір текстового поля
        this.usernameTextBox.TabIndex = 0; // Встановлюємо порядок переходу до текстового поля при натисканні клавіші Tab

        // 
        // saveButton
        // 
        this.saveButton.Location = new System.Drawing.Point(12, 38); // Встановлюємо положення кнопки на формі
        this.saveButton.Name = "saveButton"; // Встановлюємо назву кнопки
        this.saveButton.Size = new System.Drawing.Size(75, 23); // Встановлюємо розмір кнопки
        this.saveButton.TabIndex = 1; // Встановлюємо порядок переходу до кнопки при натисканні клавіші Tab
        this.saveButton.Text = "Save"; // Встановлюємо текст на кнопці
        this.saveButton.UseVisualStyleBackColor = true; // Встановлюємо, що кнопка має стандартний вигляд
        this.saveButton.Click += new System.EventHandler(this.saveButton_Click); // Підписка на подію кліку на кнопку "Save"

        // 
        // UserProfileForm
        // 
        this.ClientSize = new System.Drawing.Size(224, 75); // Встановлюємо розмір форми
        this.Controls.Add(this.saveButton); // Додаємо кнопку "Save" на форму
        this.Controls.Add(this.usernameTextBox); // Додаємо текстове поле для імені на форму
        this.Name = "UserProfileForm"; // Встановлюємо назву форми
        this.Text = "User Profile"; // Встановлюємо заголовок форми
        this.ResumeLayout(false);
        this.PerformLayout(); // Завершення ініціалізації компонентів форми

        this.ResumeLayout(false);
    }

    private void saveButton_Click(object sender, EventArgs e)
    {
        userProfile.Username = usernameTextBox.Text;
        MessageBox.Show("Profile updated successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        this.DialogResult = DialogResult.OK;
        this.Close();
    }
}

public class PlaybackHistoryForm : Form
{
    private ListBox playbackHistoryListBox;

    public PlaybackHistoryForm(List<string> playbackHistory)
    {
        InitializeComponent();
        playbackHistoryListBox.Items.AddRange(playbackHistory.ToArray()); // Додавання елементів історії програвання до списку
    }

    private void InitializeComponent()
    {
        this.playbackHistoryListBox = new ListBox();
        this.SuspendLayout();

        // 
        // playbackHistoryListBox
        // 
        this.playbackHistoryListBox.Location = new System.Drawing.Point(12, 12);
        this.playbackHistoryListBox.Name = "playbackHistoryListBox";
        this.playbackHistoryListBox.Size = new System.Drawing.Size(200, 200);
        this.playbackHistoryListBox.TabIndex = 0;

        // PlaybackHistoryForm
        // 
        this.ClientSize = new System.Drawing.Size(224, 240);
        this.Controls.Add(this.playbackHistoryListBox);
        this.Name = "PlaybackHistoryForm";
        this.Text = "Playback History";
        this.ResumeLayout(false);
    }
}