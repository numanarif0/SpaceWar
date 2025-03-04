    using System;
    using System.Drawing;
    using System.Windows.Forms;
    using Timer = System.Windows.Forms.Timer;

    namespace SpaceWar
    {
        internal class BossEnemy : Enemy
        {
            public BossEnemy(Form gameForm, int spawnX, int spawnY, Spaceship targetSpaceship)
                : base(gameForm, "BossEnemyImage.png", spawnX, spawnY, targetSpaceship,300)
            {
                Health = 80;  
                Speed = 4;     
                Damage = 50;  
            }

            public override void Shoot()
            {
                if (spaceship?.GetPictureBox() == null || EnemyPicture?.Parent == null)
                {
                    Console.WriteLine("Spaceship veya EnemyPicture null!");
                    return;
                }
                Point enemyCenter = new Point(
                    EnemyPicture?.Left + EnemyPicture?.Width / 2 ?? 0,
                    EnemyPicture?.Top + EnemyPicture?.Height / 2 ?? 0
                );
                if (enemyCenter == Point.Empty)
                {
                    Console.WriteLine("Enemy center could not be determined.");
                    return;
                }
                PictureBox bulletPicture = new PictureBox
                {
                    Size = new Size(10, 10),
                    BackColor = Color.Red, 
                    Location = enemyCenter
                };
                if (EnemyPicture?.Parent == null)
                {
                    Console.WriteLine("EnemyPicture parent form not found.");
                    return;
                }
                EnemyPicture.Parent.Controls.Add(bulletPicture);
                Timer bulletMovementTimer = new Timer { Interval = 20 }; 
                bulletMovementTimer.Tick += (sender, e) =>
                {
                    MoveBullet(bulletPicture, bulletMovementTimer);
                };
                bulletMovementTimer.Start();
                Timer destroyTimer = new Timer { Interval = 4000 }; 
                destroyTimer.Tick += (sender, e) =>
                {
                    DestroyBullet(bulletPicture, bulletMovementTimer, destroyTimer);
                };
                destroyTimer.Start();
            }

        private void MoveBullet(PictureBox bulletPicture, Timer movementTimer)
        {
            if (bulletPicture == null || spaceship?.GetPictureBox() == null)
            {
                Console.WriteLine("BulletPicture veya Spaceship null!");
                DestroyBullet(bulletPicture, movementTimer, null);
                return;
            }
            int speed = 5; 
            Point bulletCenter = new Point(
                bulletPicture.Left + bulletPicture.Width / 2,
                bulletPicture.Top + bulletPicture.Height / 2
            );
            Point spaceshipCenter = new Point(
                spaceship.GetPictureBox().Left + spaceship.GetPictureBox().Width / 2,
                spaceship.GetPictureBox().Top + spaceship.GetPictureBox().Height / 2
            );
            int deltaX = spaceshipCenter.X - bulletCenter.X;
            int deltaY = spaceshipCenter.Y - bulletCenter.Y;
            double angleToSpaceship = Math.Atan2(deltaY, deltaX);
            bulletPicture.Left += (int)(speed * Math.Cos(angleToSpaceship));
            bulletPicture.Top += (int)(speed * Math.Sin(angleToSpaceship));
            if (bulletPicture.Bounds.IntersectsWith(spaceship.GetPictureBox().Bounds))
            {
                spaceship.TakeDamage(Damage); 
                Console.WriteLine("Mermi Spaceship'e çarptı ve hasar verdi!");
                DestroyBullet(bulletPicture, movementTimer, null);
                return;
            }
            if (bulletPicture.Parent != null &&
             (bulletPicture.Left < 0 || bulletPicture.Top < 0 ||
              bulletPicture.Right > bulletPicture.Parent.ClientSize.Width ||
              bulletPicture.Bottom > bulletPicture.Parent.ClientSize.Height))
            {
                DestroyBullet(bulletPicture, movementTimer, null);
            }
        }


        // Mermiyi yok eder
        private void DestroyBullet(PictureBox bulletPicture, Timer movementTimer, Timer destroyTimer)
            {
                try
                {
                    if (movementTimer != null)
                    {
                        movementTimer.Stop();
                        movementTimer.Dispose();
                    }
                    if (destroyTimer != null)
                    {
                        destroyTimer.Stop();
                        destroyTimer.Dispose();
                    }
                    if (bulletPicture?.Parent != null)
                    {
                        bulletPicture.Parent.Controls.Remove(bulletPicture);
                        bulletPicture.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("DestroyBullet Error: " + ex.Message);
                }
            }
        }
    }
