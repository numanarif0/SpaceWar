using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace SpaceWar
{
    public partial class GameScreen : Form
    {
        private Game game;
        private HashSet<Keys> pressedKeys; 
        private List<Enemy> enemies; 
        private List<Bullet> bullets; 
        private int totalScore = 0; 
        private Dictionary<int, int> levelScores = new Dictionary<int, int>(); 
        private string playerName;
        private List<PictureBox> hearts; 
        private System.Windows.Forms.Timer heartSpawnTimer; 
        private List<Meteor> meteors;
        private System.Windows.Forms.Timer meteorSpawnTimer; 







        private System.Windows.Forms.Timer gameTimer; 
       

        public GameScreen(string playerName)
        {
            InitializeComponent();

            this.playerName = playerName;

            this.Width = 1000;
            this.Height = 700;
            this.DoubleBuffered = true;

            
            this.BackgroundImage = Image.FromFile("SpaceWarGameplayArea.png");

           
            this.KeyPreview = true;

            
            pressedKeys = new HashSet<Keys>();

           
            game = new Game(this);

            hearts = new List<PictureBox>();

           
            heartSpawnTimer = new System.Windows.Forms.Timer
            {
                Interval = 10000
            };
            heartSpawnTimer.Tick += SpawnHeart;
            heartSpawnTimer.Start();


            enemies = new List<Enemy>();
            bullets = new List<Bullet>();

            gameTimer = new System.Windows.Forms.Timer
            {
                Interval = 16 
            };
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            this.KeyDown += new KeyEventHandler(GameScreen_KeyDown);
            this.KeyUp += new KeyEventHandler(GameScreen_KeyUp);

            SpawnEnemy();
            InitializeMeteorSpawner();



        }

        private void GameScreen_KeyDown(object sender, KeyEventArgs e)
        {
            if (!pressedKeys.Contains(e.KeyCode))
            {
                pressedKeys.Add(e.KeyCode);
            }

            HandleKeyPress(); 
        }
        private void SpawnHeart(object sender, EventArgs e)
        {
            Random random = new Random();
            int spawnX = random.Next(50, this.ClientSize.Width - 50);
            int spawnY = random.Next(50, this.ClientSize.Height - 50);

            PictureBox heart = new PictureBox
            {
                Size = new Size(50, 50),
                Image = Image.FromFile("hearthImage.png"), 
                Location = new Point(spawnX, spawnY),
                BackColor = Color.Transparent,
                Tag = "heart" 
            };

            this.Controls.Add(heart);
            hearts.Add(heart);
            heart.SendToBack();

            var timer = new System.Windows.Forms.Timer
            {
                Interval = 5000 
            };
            timer.Tick += (s, ev) =>
            {
                timer.Stop();
                timer.Dispose();
                RemoveHeart(heart); 
            };
            timer.Start();
        }

        private void CheckHeartCollision()
        {
            foreach (var heart in hearts.ToList()) 
            {
                if (game.GetSpaceship().GetPictureBox().Bounds.IntersectsWith(heart.Bounds)) 
                {
                    game.GetSpaceship().Heal(20); 
                    RemoveHeart(heart); 
                }
            }
        }




        private void GameScreen_KeyUp(object sender, KeyEventArgs e)
        {
            pressedKeys.Remove(e.KeyCode);
        }

        private void HandleKeyPress()
        {
            if (pressedKeys.Contains(Keys.A))
            {
                game.RotateOnKeyPress("a");
            }

            if (pressedKeys.Contains(Keys.D))
            {
                game.RotateOnKeyPress("d");
            }

            if (pressedKeys.Contains(Keys.A) && pressedKeys.Contains(Keys.W)) 
            {
                game.Move("a");
                game.Move("w");
            }
            else if (pressedKeys.Contains(Keys.A) && pressedKeys.Contains(Keys.S)) 
            {
                game.Move("a");
                game.Move("s");
            }
            else if (pressedKeys.Contains(Keys.D) && pressedKeys.Contains(Keys.W)) 
            {
                game.Move("d");
                game.Move("w");
            }
            else if (pressedKeys.Contains(Keys.D) && pressedKeys.Contains(Keys.S)) 
            {
                game.Move("d");
                game.Move("s");
            }
            else
            {
                if (pressedKeys.Contains(Keys.W))
                    game.Move("w");
                if (pressedKeys.Contains(Keys.S))
                    game.Move("s");
                if (pressedKeys.Contains(Keys.A))
                    game.Move("a");
                if (pressedKeys.Contains(Keys.D))
                    game.Move("d");
            }

            if (pressedKeys.Contains(Keys.Space))
            {
                Bullet bullet = game.GetSpaceship().Shoot(); 
                if (bullet != null)
                {
                    bullets.Add(bullet);
                }
                pressedKeys.Remove(Keys.Space);
            }
        }

        private int currentLevel = 1; 

        private void SpawnEnemy()
        {
            try
            {
                switch (currentLevel)
                {
                    case 1:
                        
                        SpawnBasicEnemy();
                        break;
                    case 2:
                        SpawnFastEnemy();
                        break;
                    case 3:
                        SpawnStrongEnemy();
                        break;
                    case 4:
                        SpawnBossEnemy();
                        break;
                    default:
                        MessageBox.Show("Maksimum seviyeye ulaşıldı!");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Düşman oluşturulurken bir hata oluştu: " + ex.Message);
            }
        }

        private void SpawnBasicEnemy()
        {
            int spawnX = new Random().Next(0, this.ClientSize.Width - 50);
            int spawnY = new Random().Next(50, this.ClientSize.Height / 2);
            var enemy = new BasicEnemy(this, spawnX, spawnY, game.GetSpaceship());
            enemies.Add(enemy);
        }

        private void SpawnFastEnemy()
        {
            int spawnX = new Random().Next(0, this.ClientSize.Width - 50);
            int spawnY = new Random().Next(50, this.ClientSize.Height / 2);
            var enemy = new FastEnemy(this, spawnX, spawnY, game.GetSpaceship());
            enemies.Add(enemy);
        }

        private void SpawnStrongEnemy()
        {
            int spawnX = new Random().Next(0, this.ClientSize.Width - 50);
            int spawnY = new Random().Next(50, this.ClientSize.Height / 2);
            var enemy = new StrongEnemy(this, spawnX, spawnY, game.GetSpaceship());
            enemies.Add(enemy);
        }

        private void SpawnBossEnemy()
        {
            int spawnX = new Random().Next(0, this.ClientSize.Width - 50);
            int spawnY = new Random().Next(50, this.ClientSize.Height / 2);
            var enemy = new BossEnemy(this, spawnX, spawnY, game.GetSpaceship());
            enemies.Add(enemy);
        }


        private void GameLoop(object sender, EventArgs e)
        {
            if (gameOver || isPlayerDead) return; 
            UpdateBullets(); 
            UpdateEnemies(); 
            CheckCollisions(); 
            CheckShipEnemyCollision(); 
            CheckPlayerDeath(); 
            CheckLevelProgress(); 
            CheckHeartCollision();
            UpdateMeteors();
            CheckMeteorCollision();


            if (gameOver)
            {
                EndGame(); 
            }
        }




        private bool gameOver = false;
        private bool isPlayerDead = false; 

        private void CheckLevelProgress()
        {
            if (isPlayerDead) return; 

            if (enemies.Count == 0 && !gameOver) 
            {
                int levelScore = CalculateScore(currentLevel, game.GetSpaceship().Health);
                levelScores[currentLevel] = levelScore; 
                totalScore += levelScore; 

                currentLevel++; 

                if (currentLevel <= 4) 
                {
                    SpawnEnemy(); 
                }
                else
                {
                    gameOver = true; 
                }
            }
        }


        private void CheckPlayerDeath()
        {
            if (game.GetSpaceship().Health <= 0 && !isPlayerDead) 
            {
                isPlayerDead = true; 
                

                EndGame();
            }
        }







        private void UpdateBullets()
        {
            foreach (var bullet in bullets.ToList())
            {
                bullet.Move(); 
                

                if (bullet.BulletPicture.Parent == null)
                {
                    bullets.Remove(bullet);
                }
            }
        }


        private void UpdateEnemies()
        {
            foreach (var enemy in enemies.ToList())
            {
                 enemy.Move();

                if (enemy.EnemyPicture.Parent == null)
                {
                    enemies.Remove(enemy);
                    continue;
                }

                if (enemy.Health <= 0)
                {
                    enemies.Remove(enemy);
                    SpawnEnemy();
                }
            }
        }

        private void CheckCollisions()
        {
            CollisionDetector.CheckBulletCollision(bullets, enemies, game.GetSpaceship());
           
        }
        public void AddBullet(Bullet bullet)
        {
            bullets.Add(bullet); 


            
        }
        private void CheckShipEnemyCollision()
        {
            foreach (var enemy in enemies.ToList())
            {
                CollisionDetector.CheckCollision(game.GetSpaceship(), enemy);

                if (enemy.Health <= 0)
                {
                    enemies.Remove(enemy);
                    SpawnEnemy();
                }
            }
        }


        private int CalculateScore(int currentLevel, int remainingHealth)
        {
            int score = 0;

           
            switch (currentLevel)
            {
                case 1:
                    score = 20;
                    break;
                case 2:
                    score = 30; 
                    break;
                case 3:
                    score = 50; 
                    break;
                case 4:
                    score = 100; 
                    break;
            }

            if (currentLevel == 4)
            {
                score += remainingHealth; 
            }

            return score;
        }



        private void SaveScoreToFile(string playerName, int score)
        {
            string filePath = "scoreboard.txt";
            string currentDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            using (StreamWriter writer = new StreamWriter(filePath, true)) 
            {
                writer.WriteLine($"{playerName},{score},{currentDate}");
            }
        }


        private ScoreboardForm scoreboardForm; 

        private void EndGame()
        {
            if (gameOver || isPlayerDead) 
            {
               

                SaveScoreToFile(playerName, totalScore); 

                MessageBox.Show($"Oyun sona erdi! Toplam Skorunuz: {totalScore}", "Oyun Bitti", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (scoreboardForm == null || scoreboardForm.IsDisposed)
                {
                    scoreboardForm = new ScoreboardForm(playerName);
                }

                scoreboardForm.ShowDialog();

                this.Close(); 
            }
        }
        private void InitializeMeteorSpawner()
        {
            meteors = new List<Meteor>();

            meteorSpawnTimer = new System.Windows.Forms.Timer
            {
                Interval = 2000 
            };

            meteorSpawnTimer.Tick += (sender, e) =>
            {
                Random random = new Random();
                int spawnX = random.Next(50, this.ClientSize.Width - 50); 
                int spawnY = -50; 

                Meteor meteor = new Meteor(this, spawnX, spawnY, speed: 8);
                meteors.Add(meteor);
            };

            meteorSpawnTimer.Start();
        }
        private void UpdateMeteors()
        {
            foreach (var meteor in meteors.ToList())
            {
                meteor.Move();

                if (game.GetSpaceship().GetPictureBox().Bounds.IntersectsWith(meteor.MeteorPicture.Bounds))
                {
                    game.GetSpaceship().TakeDamage(15);
                    meteor.Destroy();
                    meteors.Remove(meteor);
                }
            }
        }
        private void CheckMeteorCollision()
        {
            foreach (var meteor in meteors.ToList())
            {
                if (game.GetSpaceship().GetPictureBox().Bounds.IntersectsWith(meteor.MeteorPicture.Bounds))
                {
                    game.GetSpaceship().TakeDamage(15);
                    meteor.Destroy();
                    meteors.Remove(meteor);
                }
            }
        }
        private void RemoveHeart(PictureBox heart)
        {
            if (heart != null && hearts.Contains(heart))
            {
                this.Controls.Remove(heart); 
                hearts.Remove(heart);
                heart.Dispose();
            }
        }














    }
}
