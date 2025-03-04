using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceWar
{
    public partial class MainForm : Form
    {
        private Button mStart;
        private Button mExit;
        private TextBox playerNameTextBox; 
        private Label nameLabel; 
        private Button scoreBoardOpen;
       

        public MainForm()
        {
           
            this.Text = "SpaceWar - Ana Menü";
            this.Width = 1000;
            this.Height = 700;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackgroundImage = Image.FromFile("SpaceWarGiris.png");

            nameLabel = new Label();
            nameLabel.Text = "Enter Your Name:";
            nameLabel.Font = new Font("Verdana", 10, FontStyle.Bold);
            nameLabel.ForeColor = Color.White;
            nameLabel.BackColor = Color.Transparent; 
            nameLabel.Location = new Point(400, 400);
            nameLabel.AutoSize = true;
            this.Controls.Add(nameLabel);

            
            playerNameTextBox = new TextBox();
            playerNameTextBox.Font = new Font("Verdana", 10, FontStyle.Regular);
            playerNameTextBox.Size = new Size(200, 25);
            playerNameTextBox.Location = new Point(400, 430);
            this.Controls.Add(playerNameTextBox);

            
            mStart = new Button();
            mStart.Text = "PLAY";
            mStart.Font = new Font("Verdana", 8, FontStyle.Bold);
            mStart.ForeColor = Color.White;
            mStart.BackColor = Color.Black;
            mStart.FlatStyle = FlatStyle.Flat;
            mStart.FlatAppearance.BorderColor = Color.White;
            mStart.Size = new Size(127, 27);
            mStart.Location = new Point(293, 470);
            mStart.Click += MStart_Click;
            this.Controls.Add(mStart);

           
            mExit = new Button();
            mExit.Text = "EXIT";
            mExit.Font = new Font("Verdana", 8, FontStyle.Bold);
            mExit.ForeColor = Color.White;
            mExit.BackColor = Color.Black;
            mExit.FlatStyle = FlatStyle.Flat;
            mExit.FlatAppearance.BorderColor = Color.White;
            mExit.Size = new Size(127, 27);
            mExit.Location = new Point(550, 470);
            mExit.Click += MExit_Click;
            this.Controls.Add(mExit);

            scoreBoardOpen = new Button();
            scoreBoardOpen.Text = "SCOREBOARD";
            scoreBoardOpen.Font = new Font("Verdana", 8, FontStyle.Bold);
            scoreBoardOpen.ForeColor = Color.White;
            scoreBoardOpen.BackColor = Color.Black;
            scoreBoardOpen.FlatStyle = FlatStyle.Flat;
            scoreBoardOpen.FlatAppearance.BorderColor = Color.White;
            scoreBoardOpen.Size = new Size(127, 27);
            scoreBoardOpen.Location = new Point(421, 550);
            this.Controls.Add(scoreBoardOpen);
            scoreBoardOpen.Click += ScoreBoardOpen_Click;




            InitializeComponent();
        }

        private void ScoreBoardOpen_Click(object? sender, EventArgs e)
        {
            
            string scoreboardPlayer = string.IsNullOrEmpty(playerNameTextBox.Text.Trim()) ? "Misafir" : playerNameTextBox.Text.Trim();

            
            ScoreboardForm scoreboard = new ScoreboardForm(scoreboardPlayer);
            scoreboard.Show();
            this.Hide();
        }


        private void MExit_Click(object? sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MStart_Click(object? sender, EventArgs e)
        {
            string playerName = playerNameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(playerName))
            {
                MessageBox.Show("Please enter your name before starting the game!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
            GameScreen gameScreen = new GameScreen(playerName);
            gameScreen.Show();
            this.Hide();
        }
    }
}
