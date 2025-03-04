using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace SpaceWar
{
    public partial class ScoreboardForm : Form
    {
        private DataGridView dataGridViewScores; 
        private Button closeButton; 
        private Label titleLabel; 
        private Label playerNameLabel; 

        public ScoreboardForm(string playerName)
        {
            InitializeComponent();
            SetupForm(playerName); 
            LoadScores();
        }

        private void SetupForm(string playerName)
        {
            this.Text = "Skor Tablosu";
            this.Width = 600;
            this.Height = 400;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(30, 30, 30);

            titleLabel = new Label
            {
                Text = "Skor Tablosu",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, 10),
                Width = this.ClientSize.Width,
                Height = 40
            };
            this.Controls.Add(titleLabel);

            playerNameLabel = new Label
            {
                Text = $"Player: {playerName}",
                Font = new Font("Arial", 12, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(0, titleLabel.Bottom + 5), 
                Width = this.ClientSize.Width,
                Height = 30
            };
            this.Controls.Add(playerNameLabel);

            int gridTop = playerNameLabel.Bottom + 10; 
            dataGridViewScores = new DataGridView
            {
                Location = new Point(10, gridTop), 
                Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height - gridTop - 60), 
                BackgroundColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Arial", 12, FontStyle.Bold),
                    BackColor = Color.FromArgb(60, 60, 60),
                    ForeColor = Color.White,
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                },
                DefaultCellStyle = new DataGridViewCellStyle
                {
                    Font = new Font("Arial", 12),
                    BackColor = Color.FromArgb(50, 50, 50),
                    ForeColor = Color.White,
                    SelectionBackColor = Color.FromArgb(80, 80, 80),
                    SelectionForeColor = Color.White
                },
                EnableHeadersVisualStyles = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            this.Controls.Add(dataGridViewScores);

            closeButton = new Button
            {
                Text = "CLOSE",
                Location = new Point((this.ClientSize.Width - 100) / 2, this.ClientSize.Height - 50),
                Size = new Size(100, 30),
                Font = new Font("Arial", 12, FontStyle.Bold),
                BackColor = Color.FromArgb(100, 100, 100),
                ForeColor = Color.White
            };
            closeButton.Click += CloseButton_Click;
            this.Controls.Add(closeButton);

            this.Resize += (s, e) =>
            {
                titleLabel.Width = this.ClientSize.Width;
                playerNameLabel.Width = this.ClientSize.Width;

                dataGridViewScores.Size = new Size(this.ClientSize.Width - 20, this.ClientSize.Height - gridTop - 60);
                closeButton.Location = new Point((this.ClientSize.Width - closeButton.Width) / 2, this.ClientSize.Height - 50);
            };
        }


        private void LoadScores()
        {
            string filePath = "scoreboard.txt";
            if (!File.Exists(filePath) || new FileInfo(filePath).Length == 0)
            {
                MessageBox.Show("Henüz kayıtlı skor bulunmuyor.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            List<string[]> scores = new List<string[]>();
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(',');
                        if (parts.Length == 3) 
                        {
                            scores.Add(parts);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Skor dosyası okunurken bir hata oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (scores.Count == 0)
            {
                MessageBox.Show("Henüz kayıtlı skor bulunmuyor.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            scores = scores.OrderByDescending(s => int.Parse(s[1])).ToList();
            if (dataGridViewScores.Columns.Count == 0)
            {
                dataGridViewScores.Columns.Add("Player", "Oyuncu Adı");
                dataGridViewScores.Columns.Add("Score", "Skor");
                dataGridViewScores.Columns.Add("Date", "Tarih");
            }
            dataGridViewScores.Rows.Clear();
            foreach (var score in scores)
            {
                dataGridViewScores.Rows.Add(score[0], score[1], score[2]);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close(); 
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }
    }
}
