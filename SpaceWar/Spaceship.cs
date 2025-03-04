    using System;
    using System.Drawing;
    using System.Windows.Forms;

    namespace SpaceWar
    {
        public class Spaceship
        {
            public int Health { get; private set; }
            private ProgressBar healthBar; 
            public int Damage { get; private set; }
            public int Speed { get; private set; }
            private PictureBox spaceshipPicture; 
            private Form gameForm; 
            private float rotationAngle;
            private float bulletAngle;
        private bool canShoot; 

        

            



            public Spaceship(Form form, int initialHealth = 100, int damage = 10, int speed = 5)
            {
                Health = initialHealth;
                Damage = damage;
                Speed = speed;
                gameForm = form;
                rotationAngle = 0; 
                bulletAngle = 42;
            canShoot = true; 

            spaceshipPicture = new PictureBox
                {
                    Size = new Size(50, 50),
                    Image = Image.FromFile("spaceship.png"),
                    Location = new Point(form.ClientSize.Width / 2, form.ClientSize.Height - 60) 
                };
           

            gameForm.Controls.Add(spaceshipPicture);

            healthBar = new ProgressBar
            {
                Size = new Size(200, 20), 
                Location = new Point(10, 30),
                Value = Health,
                Maximum = 100,
                ForeColor = Color.Green 

            }; 
            gameForm.Controls.Add(healthBar);
            healthBar.BringToFront();
            Label spaceshipLabel = new Label
            {
                Text = "SPACESHIP",           
                Font = new Font("Arial", 12, FontStyle.Bold), 
                ForeColor = Color.White,      
                BackColor = Color.Transparent,
                AutoSize = true,
                Location = new Point(10, 5)   
            };

            gameForm.Controls.Add(spaceshipLabel); 
            spaceshipLabel.BringToFront();    
                   

        }

        public void Move(string direction)
            {
                switch (direction.ToLower())
                {
                    case "a": 
                        if (spaceshipPicture.Left > 0)
                            spaceshipPicture.Left -= Speed;
                        else
                            spaceshipPicture.Left = gameForm.ClientSize.Width - spaceshipPicture.Width; 
                        break;

                    case "d": 
                        if (spaceshipPicture.Right < gameForm.ClientSize.Width)
                            spaceshipPicture.Left += Speed;
                        else
                            spaceshipPicture.Left = 0; 
                        break;  

                    case "w": 
                        if (spaceshipPicture.Top > 0)
                            spaceshipPicture.Top -= Speed;
                        else
                            spaceshipPicture.Top = gameForm.ClientSize.Height - spaceshipPicture.Height; 
                        break;

                    case "s": 
                        if (spaceshipPicture.Bottom < gameForm.ClientSize.Height)
                            spaceshipPicture.Top += Speed;
                        else
                            spaceshipPicture.Top = 0; 
                        break;
                }

                RotateSpaceship();
            }


         private void RotateSpaceship()
    {
        Image originalImage = Image.FromFile("spaceship.png");
    
        int newWidth = (int)(originalImage.Width * Math.Sqrt(2));  
        int newHeight = (int)(originalImage.Height * Math.Sqrt(2));

        Bitmap rotatedImage = new Bitmap(newWidth, newHeight);

        using (Graphics g = Graphics.FromImage(rotatedImage))
        {
            g.Clear(Color.Black);  
            g.TranslateTransform(rotatedImage.Width / 2, rotatedImage.Height / 2);
            g.RotateTransform(rotationAngle); 
            g.TranslateTransform(-originalImage.Width / 2, -originalImage.Height / 2);

            g.DrawImage(originalImage, new Point(0, 0));
        }

        spaceshipPicture.Image = rotatedImage;
        spaceshipPicture.Size = rotatedImage.Size;  
    }
            public void RotateOnKeyPress(string direction)
            {
                    if (direction == "a") 
                    {
                        rotationAngle -= 2; 
                    bulletAngle += 2;
                }
                    else if (direction == "d") 
                    {
                        rotationAngle += 2;
                    bulletAngle -= 2;
                }
                RotateSpaceship(); 
            }
            public Bullet Shoot()
            {

            if (!canShoot)
                return null; 
                            
            Point spaceshipCenter = new Point(
                    spaceshipPicture.Left + spaceshipPicture.Width / 2,
                    spaceshipPicture.Top + spaceshipPicture.Height / 2
                );

                double angleInRadians = bulletAngle * Math.PI / 180.0; 
                int bulletStartX = spaceshipCenter.X + (int)(Math.Cos(angleInRadians) * (spaceshipPicture.Width / 2));
                int bulletStartY = spaceshipCenter.Y - (int)(Math.Sin(angleInRadians) * (spaceshipPicture.Height / 2));

                Point bulletStartPosition = new Point(bulletStartX, bulletStartY);

                Bullet bullet = new Bullet(gameForm, bulletStartPosition, bulletAngle);
                return bullet;
            }

        public void DisableShooting()
        {
            canShoot = false;
        }

        public void EnableShooting()
        {
            canShoot = true;
        }









        public void TakeDamage(int amount)
            {
                Health -= amount;
            UpdateHealthBar();
            if (Health <= 0)
                {
                    Health = 0;
                    
                    gameForm.Close(); 
                }
                else
                {
                   
                }
            }
            public PictureBox GetPictureBox()
            {
                return spaceshipPicture;
            }
        public void DisableMovement()
        {
            Speed = 0; 
        }

        public void EnableMovement()
        {
            Speed = 5; 
        }
        public bool IsColliding { get; set; } = false; 

        private void UpdateHealthBar()
        {
            healthBar.Value = Math.Max(healthBar.Minimum, Math.Min(healthBar.Maximum, Health));
        }

        public void Heal(int amount)
        {
            Health = Math.Min(100, Health + amount); 
            UpdateHealthBar(); 
        }








    }
}
