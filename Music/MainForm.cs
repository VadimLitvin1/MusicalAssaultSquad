using NAudio.Wave;

namespace MusicPlayerApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles(); // ��������� ��������� ����� ��� ��������
            Application.SetCompatibleTextRenderingDefault(false); // ������������ �������� ���������� ������ ��� ��������
            Application.Run(new MainForm()); // ������ ������� ����� ��������
        }
    }

    public partial class MainForm : Form
    {
        private MusicService musicService; // ����� ��� ������ � �������
        private ListBox artistsListBox; // ������ ��� ����������� ����������
        private ListBox songsListBox; // ������ ��� ����������� �����
        private Button personalProfileButton; // ������ "��������� �������"
        private Button playbackHistoryButton; // ������ "������ �����������"
        private Button playMusicButton; // ������ "³�������� ������"
        private UserProfile userProfile; // ��'��� ��� ���������� ������������������� �������

        public MainForm()
        {
            InitializeComponent();
            musicService = new MusicService(); // ����������� ��'���� ��������� ������
            userProfile = new UserProfile(); // ����������� ��'���� ������������������� �������
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            musicService.LoadMusicFiles(); // ������������ �������� �����
            artistsListBox.DataSource = musicService.GetArtists(); // ���������� ������ ���������� ������
        }

        private void personalProfileButton_Click(object sender, EventArgs e)
        {
            UserProfileForm profileForm = new UserProfileForm(userProfile); // ��������� ����� ��� ����������� ���������� �������
            profileForm.ShowDialog(); // ³���������� ����� ��������

            if (profileForm.DialogResult == DialogResult.OK)
            {
                // ��������� ���� ����������� �� ��������� �����
                this.Text = $"TuneFlow - {userProfile.Username}";
            }
        }

        private void playbackHistoryButton_Click(object sender, EventArgs e)
        {
            PlaybackHistoryForm historyForm = new PlaybackHistoryForm(musicService.GetPlaybackHistory()); // ��������� ����� ��� ����������� ����� �����������
            historyForm.ShowDialog(); // ³���������� ����� ��������
        }

        private void playMusicButton_Click(object sender, EventArgs e)
        {
            if (musicService.IsPlaying) // ���� ������ ��� ������������
            {
                var result = MessageBox.Show("Stop the current song?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question); // ϳ����������� ������� ������� ���
                if (result == DialogResult.Yes)
                {
                    musicService.StopMusic(); // ������� ������
                }
            }
            else // ���� ������ �� ������������
            {
                if (songsListBox.SelectedIndex >= 0) // ���� ������� ���� � ������
                {
                    int songNumber = songsListBox.SelectedIndex; // ��������� ������ ������� ���
                    musicService.PlaySong(songNumber); // ³��������� ������� ���
                    MessageBox.Show($"Playing song: {musicService.GetCurrentSong()}"); // ³���������� ����������� ��� ��, �� ������������ ����
                }
                else
                {
                    MessageBox.Show("No song selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // ��������� ����������� ��� �������, ���� ���� �� �������
                }
            }
        }

        private void artistsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedArtist = artistsListBox.SelectedItem.ToString(); // ��������� ��������� ��������� � ������
            musicService.SetSelectedArtist(selectedArtist); // ������������ ��������� ��������� � ��������� �����
            songsListBox.DataSource = musicService.GetSongsBySelectedArtist(); // ���������� ������ ����� ��������� ��������� ������
        }

        private void InitializeComponent()
        {
            int buttonWidth = 100; // ��������� ������ ������
            int buttonHeight = 30; // ��������� ������ ������

            this.artistsListBox = new System.Windows.Forms.ListBox(); // ����������� ������ ��� ����������� ����������
            this.songsListBox = new System.Windows.Forms.ListBox(); // ����������� ������ ��� ����������� �����
            this.personalProfileButton = new System.Windows.Forms.Button(); // ����������� ������ "��������� �������"
            this.playbackHistoryButton = new System.Windows.Forms.Button(); // ����������� ������ "������ �����������"
            this.playMusicButton = new System.Windows.Forms.Button(); // ����������� ������ "³�������� ������"
            this.SuspendLayout();

            // 
            // artistsListBox
            // 
            this.artistsListBox.BackColor = System.Drawing.Color.FromArgb(30, 30, 30); // ������������ ���� ���� ������ ����������
            this.artistsListBox.ForeColor = System.Drawing.Color.White; // ������������ ���� ������ ������ ����������
            this.artistsListBox.FormattingEnabled = true; // ������������ ���������� ��� ��������� ������ �������� ������
            this.artistsListBox.Location = new System.Drawing.Point(12, 12); // ������������ ��������� ������ ���������� �� ����
            this.artistsListBox.Name = "artistsListBox"; // ������������ ����� ������ ����������
            this.artistsListBox.Size = new System.Drawing.Size(150, 225); // ������������ ������ ������ ����������
            this.artistsListBox.TabIndex = 0; // ������������ ������� �������� �� ������ ���������� ��� ��������� ������ Tab
            this.artistsListBox.SelectedIndexChanged += new System.EventHandler(this.artistsListBox_SelectedIndexChanged); // ϳ������ �� ���� ���� ��������� ���������

            // 
            // songsListBox
            // 
            this.songsListBox.BackColor = System.Drawing.Color.FromArgb(30, 30, 30); // ������������ ���� ���� ������ �����
            this.songsListBox.ForeColor = System.Drawing.Color.White; // ������������ ���� ������ ������ �����
            this.songsListBox.FormattingEnabled = true; // ������������ ���������� ��� ��������� ������ �������� ������
            this.songsListBox.Location = new System.Drawing.Point(168, 12); // ������������ ��������� ������ ����� �� ����
            this.songsListBox.Name = "songsListBox"; // ������������ ����� ������ �����
            this.songsListBox.Size = new System.Drawing.Size(150, 225); // ������������ ������ ������ �����
            this.songsListBox.TabIndex = 1; // ������������ ������� �������� �� ������ ����� ��� ��������� ������ Tab

            // ...
            // personalProfileButton
            // ������ ��� �������� �� ���������� ������� �����������
            this.personalProfileButton.BackColor = System.Drawing.Color.FromArgb(255, 128, 0); // ������������ ����� ���� ������
            this.personalProfileButton.FlatAppearance.BorderSize = 0; // ������������ ������� ����� ������
            this.personalProfileButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat; // ������������ ����� ������ ��� �����
            this.personalProfileButton.ForeColor = System.Drawing.Color.White; // ������������ ����� ������ ������
            this.personalProfileButton.Location = new System.Drawing.Point(12, 250); // ������������ ��������� ������ �� ����
            this.personalProfileButton.Name = "personalProfileButton"; // ������������ ����� ������
            this.personalProfileButton.Size = new System.Drawing.Size(100, 30); // ������������ ������ ������
            this.personalProfileButton.TabIndex = 2; // ������������ ������� �������� �� ������ ��� ��������� ������ Tab
            this.personalProfileButton.Text = "Profile"; // ������������ ������ �� ������
            this.personalProfileButton.UseVisualStyleBackColor = false; // ������������ ���������� ��� ����������� ������� ����
            this.personalProfileButton.Click += new System.EventHandler(this.personalProfileButton_Click); // ϳ������ �� ���� ���� �� ������ "Profile"

            // playbackHistoryButton
            // ������ ��� �������� �� ����� ����������� �����
            this.playbackHistoryButton.BackColor = System.Drawing.Color.FromArgb(255, 0, 128); // ������������ ����� ���� ������
            this.playbackHistoryButton.FlatAppearance.BorderSize = 0; // ������������ ������� ����� ������
            this.playbackHistoryButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat; // ������������ ����� ������ ��� �����
            this.playbackHistoryButton.ForeColor = System.Drawing.Color.White; // ������������ ����� ������ ������
            this.playbackHistoryButton.Location = new System.Drawing.Point(118, 250); // ������������ ��������� ������ �� ����
            this.playbackHistoryButton.Name = "playbackHistoryButton"; // ������������ ����� ������
            this.playbackHistoryButton.Size = new System.Drawing.Size(100, 30); // ������������ ������ ������
            this.playbackHistoryButton.TabIndex = 3; // ������������ ������� �������� �� ������ ��� ��������� ������ Tab
            this.playbackHistoryButton.Text = "History"; // ������������ ������ �� ������
            this.playbackHistoryButton.UseVisualStyleBackColor = false; // ������������ ���������� ��� ����������� ������� ����
            this.playbackHistoryButton.Click += new System.EventHandler(this.playbackHistoryButton_Click); // ϳ������ �� ���� ���� �� ������ "History"

            // playMusicButton
            // ������ ��� ���������� ������
            this.playMusicButton.BackColor = System.Drawing.Color.FromArgb(0, 192, 0); // ������������ ����� ���� ������
            this.playMusicButton.FlatAppearance.BorderSize = 0; // ������������ ������� ����� ������
            this.playMusicButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat; // ������������ ����� ������ ��� �����
            this.playMusicButton.ForeColor = System.Drawing.Color.White; // ������������ ����� ������ ������
            this.playMusicButton.Location = new System.Drawing.Point(224, 250); // ������������ ��������� ������ �� ����
            this.playMusicButton.Name = "playMusicButton"; // ������������ ����� ������
            this.playMusicButton.Size = new System.Drawing.Size(100, 30); // ������������ ������ ������
            this.playMusicButton.TabIndex = 4; // ������������ ������� �������� �� ������ ��� ��������� ������ Tab
            this.playMusicButton.Text = "Play"; // ������������ ������ �� ������
            this.playMusicButton.UseVisualStyleBackColor = false; // ������������ ���������� ��� ����������� ������� ����
            this.playMusicButton.Click += new System.EventHandler(this.playMusicButton_Click); // ϳ������ �� ���� ���� �� ������ "Play"

            // MainForm
            // ����� ��������� ���� ��������
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 30); // ������������ ����� ���� �����
            this.ClientSize = new System.Drawing.Size(336, 292); // ������������ ������ �����
            this.Controls.Add(this.playMusicButton); // ��������� ������ "Play" �� �����
            this.Controls.Add(this.playbackHistoryButton); // ��������� ������ "History" �� �����
            this.Controls.Add(this.personalProfileButton); // ��������� ������ "Profile" �� �����
            this.Controls.Add(this.songsListBox); // ��������� ������ ����� �� �����
            this.Controls.Add(this.artistsListBox); // ��������� ������ ���������� �� �����
            this.Name = "MainForm"; // ������������ ����� �����
            this.Text = "TuneFlow"; // ������������ ��������� �����
            this.Load += new System.EventHandler(this.MainForm_Load); // ϳ������ �� ���� ������������ ����� "Load"
            this.ResumeLayout(false); // ���������� ����������� ���������� �����
        }
    }

    public class MusicService
    {
        private List<string> playbackHistory; // ������ ��� ���������� ����� ����������� �����
        private string[] artists; // ����� ��� ���������� ������ ����������
        private string[] musicFiles; // ����� ��� ���������� ������ �������� �����
        private string selectedArtist; // ������ ��������� ���������
        private bool isPlaying; // ������ ���� ����������� �����
        private WaveOutEvent outputDevice; // ��'��� ��� ����������� �����
        private AudioFileReader audioFile; // ��'��� ��� ���������� ���������

        public MusicService()
        {
            playbackHistory = new List<string>(); // ���������� ������ ����� ����������� �����
            artists = Directory.GetDirectories("music").Select(Path.GetFileName).ToArray(); // �������� ������ ���������� � ����� "music"
            isPlaying = false; // ������������ ���������� ���� ����������� �� "�� ������"
        }

        public void LoadMusicFiles()
        {
            // ����������� ������ ����� � ����� "music"
            musicFiles = Directory.GetFiles("music", "*.mp3", SearchOption.AllDirectories);
        }

        public string[] GetArtists()
        {
            return artists; // ��������� ������ ����������
        }

        public void SetSelectedArtist(string artist)
        {
            selectedArtist = artist; // ������������ ������� ���������
        }

        public string[] GetSongsBySelectedArtist()
        {
            string artistPath = Path.Combine("music", selectedArtist); // ������� ���� �� ����� � �������� ����������
            musicFiles = Directory.GetFiles(artistPath, "*.mp3"); // �������� ������ �������� ����� ��� �������� ���������
            return musicFiles.Select(Path.GetFileNameWithoutExtension).ToArray(); // ��������� ����� ����� ����� ��� ����������
        }

        public void PlaySong(int songNumber)
        {
            if (isPlaying)
            {
                outputDevice.Stop(); // ��������� �����������, ���� ��� ������
                outputDevice.Dispose(); // ��������� ������� �������� ����������
                audioFile.Dispose(); // ��������� ������� ���������
                isPlaying = false; // ������������ ���� ����������� �� "�� ������"
            }

            if (songNumber >= 0 && songNumber < musicFiles.Length)
            {
                string selectedSong = musicFiles[songNumber]; // �������� ���� �� ������ ���
                PlaySong(selectedSong); // �������� ���� �� ������
                playbackHistory.Add(selectedSong); // ������ ���� �� ����� �����������
            }
        }

        private void PlaySong(string songPath)
        {
            if (File.Exists(songPath))
            {
                audioFile = new AudioFileReader(songPath); // ��������� ����� ��'��� ��� ���������� ���������
                outputDevice = new WaveOutEvent(); // ��������� ����� ��'��� ��� ����������� �����
                outputDevice.PlaybackStopped += (sender, e) => StopMusic(); // ������ �������� ��䳿, ���� ���������� ��� ��������� �����������
                outputDevice.Init(audioFile); // ���������� ������� ���������� � ����������
                outputDevice.Play(); // �������� ���������� ��������
                isPlaying = true; // ������������ ���� ����������� �� "������"
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
                outputDevice.Stop(); // ��������� �����������
                outputDevice.Dispose(); // ��������� ������� �������� ����������
                audioFile.Dispose(); // ��������� ������� ���������
                isPlaying = false; // ������������ ���� ����������� �� "�� ������"
            }
        }

        public bool IsPlaying
        {
            get { return isPlaying; } // ��������� �������� ���� �����������
        }

        public string GetCurrentSong()
        {
            // ������� ��� ��� ��� ��������� ����� ������� ���, �� ����������.
            // ����� � ������ �������� �����-����������.
            return "No song is playing.";
        }

        public List<string> GetPlaybackHistory()
        {
            return playbackHistory; // ��������� ������ ����� �����������
        }
    }
}


public class UserProfile
{
    public string Username { get; set; } // ���������� ��� ���������� ���� �����������

    // ����� ������ ���� ���������� ������� �����������, ��� �� ��'�, �������, ���������� ����� ����.
}

public class UserProfileForm : Form
{
    private UserProfile userProfile; // ��'��� ��� ���������� ������� �����������
    private TextBox usernameTextBox; // �������� ���� ��� �������� ���� �����������
    private Button saveButton; // ������ ��� ���������� ��� ������� �����������

    public UserProfileForm(UserProfile userProfile)
    {
        this.userProfile = userProfile; // �������� ������� �����������, ��������� � �����������
        InitializeComponent();
        usernameTextBox.Text = userProfile.Username; // ������������ ����� � ���������� ��� � ������ �����������
    }

    private void InitializeComponent()
    {
        int buttonWidth = 100; // ��������� ������ ������
        int buttonHeight = 30; // ��������� ������ ������

        this.usernameTextBox = new System.Windows.Forms.TextBox(); // ��������� ���������� ���� ��� ���� �����������
        this.saveButton = new System.Windows.Forms.Button(); // ��������� ������ ��� ���������� ������� �����������
        this.SuspendLayout();

        // 
        // usernameTextBox
        // 
        this.usernameTextBox.Location = new System.Drawing.Point(12, 12); // ������������ ��������� ���������� ���� �� ����
        this.usernameTextBox.Name = "usernameTextBox"; // ������������ ����� ���������� ����
        this.usernameTextBox.Size = new System.Drawing.Size(200, 20); // ������������ ����� ���������� ����
        this.usernameTextBox.TabIndex = 0; // ������������ ������� �������� �� ���������� ���� ��� ��������� ������ Tab

        // 
        // saveButton
        // 
        this.saveButton.Location = new System.Drawing.Point(12, 38); // ������������ ��������� ������ �� ����
        this.saveButton.Name = "saveButton"; // ������������ ����� ������
        this.saveButton.Size = new System.Drawing.Size(75, 23); // ������������ ����� ������
        this.saveButton.TabIndex = 1; // ������������ ������� �������� �� ������ ��� ��������� ������ Tab
        this.saveButton.Text = "Save"; // ������������ ����� �� ������
        this.saveButton.UseVisualStyleBackColor = true; // ������������, �� ������ �� ����������� ������
        this.saveButton.Click += new System.EventHandler(this.saveButton_Click); // ϳ������ �� ���� ���� �� ������ "Save"

        // 
        // UserProfileForm
        // 
        this.ClientSize = new System.Drawing.Size(224, 75); // ������������ ����� �����
        this.Controls.Add(this.saveButton); // ������ ������ "Save" �� �����
        this.Controls.Add(this.usernameTextBox); // ������ �������� ���� ��� ���� �� �����
        this.Name = "UserProfileForm"; // ������������ ����� �����
        this.Text = "User Profile"; // ������������ ��������� �����
        this.ResumeLayout(false);
        this.PerformLayout(); // ���������� ����������� ���������� �����

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
        playbackHistoryListBox.Items.AddRange(playbackHistory.ToArray()); // ��������� �������� ����� ����������� �� ������
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