using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace SpaceWar
{
    public abstract class Enemy
    {
        public int Health { get; set; }
        public int Speed { get; set; }
        public int Damage { get; set; }
        public PictureBox EnemyPicture { get; private set; }
        private Bitmap originalImage; 
        public Spaceship spaceship; 
        private System.Windows.Forms.Timer rotationTimer; 
        private System.Windows.Forms.Timer movementTimer; 
        private System.Windows.Forms.Timer shootTimer; 
        private double rotationAngle; 
        private Point targetPosition; 
        private double bulletRotationAngle = 90; 
        private ProgressBar healthBar;

        public Enemy(Form gameForm, string imageFilePath, int spawnX, int spawnY, Spaceship targetSpaceship,int health)
        {
            if (gameForm == null)
            {
                throw new ArgumentNullException(nameof(gameForm), "Game form cannot be null");
            }

            if (targetSpaceship == null)
            {
                throw new ArgumentNullException(nameof(targetSpaceship), "Spaceship cannot be null");
            }

            spaceship = targetSpaceship;
            Health = health; 

            originalImage = new Bitmap(imageFilePath);

            EnemyPicture = new PictureBox
            {
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(spawnX, spawnY),
                Size = new Size(50, 50),
                BackColor = Color.Transparent
            };


            EnemyPicture.Image = RotateImage(originalImage, 0); 

            gameForm.Controls.Add(EnemyPicture);

            healthBar = new ProgressBar
            {
                Size = new Size(200, 20), 
                Location = new Point(770, 30), 
                Maximum = Health,   
                Value = Health,      
                ForeColor = Color.Green
            };
            gameForm.Controls.Add(healthBar);
            healthBar.BringToFront();
            healthBar.Refresh(); 
            Label enemyLabel = new Label
            {
                Text = "ENEMY",           
                Font = new Font("Arial", 12, FontStyle.Bold), 
                ForeColor = Color.White,      
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(770, 5)   
            };

            gameForm.Controls.Add(enemyLabel); 
            enemyLabel.BringToFront();






            rotationTimer = new System.Windows.Forms.Timer { Interval = 50 }; 
            rotationTimer.Tick += (sender, e) => RotateTowardsSpaceship();
            rotationTimer.Start();

            movementTimer = new System.Windows.Forms.Timer { Interval = 5000 }; 
            movementTimer.Tick += (sender, e) => UpdateTargetPosition();
            movementTimer.Start();

            shootTimer = new System.Windows.Forms.Timer { Interval = 3000 }; 
            shootTimer.Tick += (sender, e) => Shoot();
            shootTimer.Start();

            UpdateTargetPosition();
        }

        private void RotateTowardsSpaceship()
        {
            if (spaceship?.GetPictureBox() == null || EnemyPicture?.Parent == null)
            {
                Console.WriteLine("RotateTowardsSpaceship skipped due to null spaceship or EnemyPicture.");
                return;
            }
            Point spaceshipCenter = new Point(
                spaceship.GetPictureBox().Left + spaceship.GetPictureBox().Width / 2,
                spaceship.GetPictureBox().Top + spaceship.GetPictureBox().Height / 2
            );
            Point enemyCenter = new Point(
                EnemyPicture.Left + EnemyPicture.Width / 2,
                EnemyPicture.Top + EnemyPicture.Height / 2
            );
            int deltaX = spaceshipCenter.X - enemyCenter.X;
            int deltaY = spaceshipCenter.Y - enemyCenter.Y;

            rotationAngle = Math.Atan2(deltaY, deltaX) * (180 / Math.PI); 
            rotationAngle += 90; 
            EnemyPicture.Image = RotateImage(originalImage, (float)rotationAngle);
        }

        private void UpdateTargetPosition()
        {
            if (spaceship?.GetPictureBox() == null)
            {
                Console.WriteLine("UpdateTargetPosition skipped due to null spaceship.");
                return;
            }

            targetPosition = new Point(
                spaceship.GetPictureBox().Left + spaceship.GetPictureBox().Width / 2,
                spaceship.GetPictureBox().Top + spaceship.GetPictureBox().Height / 2
            );
        }

        public virtual void Move()
        {
            if (targetPosition == Point.Empty || EnemyPicture == null)
            {
                Console.WriteLine("Move skipped due to null targetPosition or EnemyPicture.");
                return;
            }
            Point enemyCenter = new Point(
                EnemyPicture.Left + EnemyPicture.Width / 2,
                EnemyPicture.Top + EnemyPicture.Height / 2
            );
            int deltaX = targetPosition.X - enemyCenter.X;
            int deltaY = targetPosition.Y - enemyCenter.Y;
            double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);
            if (distance > 0)
            {
                EnemyPicture.Left += (int)(Speed * (deltaX / distance));
                EnemyPicture.Top += (int)(Speed * (deltaY / distance));
            }
        }

        public virtual void Shoot()
        {
            
           if (EnemyPicture?.Parent == null || spaceship?.GetPictureBox() == null)
                return;
            Point enemyCenter = new Point(
                EnemyPicture.Left + EnemyPicture.Width / 2,
                EnemyPicture.Top + EnemyPicture.Height / 2
            );
            Point spaceshipCenter = new Point(
                spaceship.GetPictureBox().Left + spaceship.GetPictureBox().Width / 2,
                spaceship.GetPictureBox().Top + spaceship.GetPictureBox().Height / 2
            );
            int deltaX = spaceshipCenter.X - enemyCenter.X;
            int deltaY = spaceshipCenter.Y - enemyCenter.Y;
            double angleToSpaceship = Math.Atan2(-deltaY, deltaX) * (180 / Math.PI);
            Bullet bullet = new Bullet(EnemyPicture.Parent as Form, enemyCenter, (float)angleToSpaceship, damage: Damage, speed: 8);
            ((GameScreen)EnemyPicture.Parent).AddBullet(bullet); 
        }   




        private Bitmap RotateImage(Bitmap image, float angle)
        {
            Bitmap rotatedImage = new Bitmap(image.Width, image.Height);
            using (Graphics g = Graphics.FromImage(rotatedImage))
            {
                g.Clear(Color.Transparent);
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.TranslateTransform(image.Width / 2, image.Height / 2);
                g.RotateTransform(angle);
                g.TranslateTransform(-image.Width / 2, -image.Height / 2);
                g.DrawImage(image, new Point(0, 0));
            }

            return rotatedImage;
        }

       
        public void TakeDamage(int amount)
        {
            Health -= amount; 
            UpdateHealthBar();

            if (Health <= 0) 
            {
                Destroy(); 
            }
        }
        public void Destroy()
        {
            rotationTimer?.Stop();
            movementTimer?.Stop();
            shootTimer?.Stop();
            rotationTimer?.Dispose();
            movementTimer?.Dispose();
            shootTimer?.Dispose();
            EnemyPicture?.Parent?.Controls.Remove(EnemyPicture);
            EnemyPicture?.Dispose();
            Console.WriteLine("Düşman yok edildi.");
        }
        public void DisableMovement()
        {
            Speed = 0; 
        }

        public void EnableMovement()
        {
            Speed = 3; 
        }
        public bool IsColliding { get; set; } = false; 
        public void StopShooting()
        {
            if (shootTimer != null)
            {
                shootTimer.Stop();
            }
        }

        public void StartShooting()
        {
            if (shootTimer != null)
            {
                shootTimer.Start();
            }
        }
        private void UpdateHealthBar()
        {
            healthBar.Value = Math.Max(healthBar.Minimum, Math.Min(healthBar.Maximum, Health));
        }



    }
}
