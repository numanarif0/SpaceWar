using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceWar
{
    public class CollisionDetector
    {
        private static Random random = new Random(); 

       
        public static void CheckCollision(Spaceship player, Enemy enemy)
        {
            if (player == null || enemy == null || player.GetPictureBox() == null || enemy.EnemyPicture == null)
                return;

            if (player.GetPictureBox().Bounds.IntersectsWith(enemy.EnemyPicture.Bounds))
            {
                if (player.IsColliding || enemy.IsColliding)
                    return;
                player.IsColliding = true;
                enemy.IsColliding = true;
                StartCollisionEffect(player, enemy);
            }
        }

        
        private static bool isPlayerDestroyed = false; 

        public static void CheckBulletCollision(List<Bullet> bullets, List<Enemy> enemies, Spaceship player)
        {
            if (bullets == null || player == null || enemies == null)
                return;
            List<Bullet> bulletsToRemove = new List<Bullet>(); 
            List<Enemy> enemiesToRemove = new List<Enemy>();   
            for (int i = bullets.Count - 1; i >= 0; i--)
            {
                var bullet = bullets[i];
                if (bullet.BulletPicture == null || bullet.BulletPicture.Parent == null)
                    continue;
                if ((DateTime.Now - bullet.CreationTime).TotalSeconds < 0.1)
                    continue;
                if (player.GetPictureBox() != null && bullet.BulletPicture.Bounds.IntersectsWith(player.GetPictureBox().Bounds))
                {
                    Console.WriteLine("Mermi Spaceship'e çarptı.");
                    player.TakeDamage(bullet.Damage);
                    bulletsToRemove.Add(bullet); 
                    if (player.Health <= 0 && !isPlayerDestroyed) 
                    {
                        isPlayerDestroyed = true; 
                        Console.WriteLine("Spaceship yok edildi!");
                        var playerForm = player.GetPictureBox()?.Parent?.FindForm(); 
                        if (playerForm != null)
                        {
                            playerForm.Close();
                        }
                        else
                        {
                            Console.WriteLine("Form bulunamadı, oyun sonlandırılamıyor.");
                        }
                        return;
                    }
                }
                foreach (var enemy in enemies)
                {
                    if (enemy.EnemyPicture == null || enemy.EnemyPicture.Parent == null)
                        continue;
                    if (bullet.BulletPicture.Bounds.IntersectsWith(enemy.EnemyPicture.Bounds))
                    {
                        Console.WriteLine("Mermi düşmana çarptı.");
                        enemy.TakeDamage(bullet.Damage);
                        bulletsToRemove.Add(bullet);
                        if (enemy.Health <= 0)
                        {
                            Console.WriteLine("Düşman yok edildi.");
                            enemiesToRemove.Add(enemy);
                        }
                    }
                }
            }
            foreach (var bullet in bulletsToRemove)
            {
                bullets.Remove(bullet);
                bullet.Destroy();
            }
            foreach (var enemy in enemiesToRemove)
            {
                enemies.Remove(enemy);
                enemy.Destroy();
            }
        }


        








      
        private static void StartCollisionEffect(Spaceship player, Enemy enemy)
        {
            int freezeDuration = 2000; 
            player.DisableMovement();
            enemy.DisableMovement();
            player.DisableShooting();
            System.Windows.Forms.Timer freezeTimer = new System.Windows.Forms.Timer { Interval = freezeDuration }; 
            freezeTimer.Tick += (sender, e) =>
            {
                freezeTimer.Stop();
                freezeTimer.Dispose();
                player.EnableMovement();
                player.EnableShooting();
                RespawnEnemy(player, enemy);
            };
            freezeTimer.Start();
        }
         
       
        private static void RespawnEnemy(Spaceship player, Enemy enemy)
        {
            
            Point playerCenter = new Point(
                player.GetPictureBox().Left + player.GetPictureBox().Width / 2,
                player.GetPictureBox().Top + player.GetPictureBox().Height / 2
            );
            enemy.StopShooting();
            int collisionDamage = 20; 
            Console.WriteLine("Düşman oyuncuya çarptı ve hasar verdi!");
            player.TakeDamage(collisionDamage);
            int spawnX, spawnY;
            do
            {
                if (enemy.EnemyPicture?.Parent == null)
                {
                    Console.WriteLine("EnemyPicture veya Parent null, yeniden konumlandırılamıyor.");
                    return; 
                }

                spawnX = random.Next(50, enemy.EnemyPicture.Parent.ClientSize.Width - 50);
                spawnY = random.Next(50, enemy.EnemyPicture.Parent.ClientSize.Height - 50);                
            } while (Math.Sqrt(Math.Pow(spawnX - playerCenter.X, 2) + Math.Pow(spawnY - playerCenter.Y, 2)) < 100);
            enemy.EnemyPicture.Location = new Point(spawnX, spawnY);
            int damage = 10; 
            enemy.TakeDamage(damage);
            if (enemy.Health <= 0)
            {
                enemy.Destroy();
            }
            else
            {
                System.Windows.Forms.Timer resetTimer = new System.Windows.Forms.Timer();
                resetTimer.Interval = 1000; 
                resetTimer.Tick += (s, e) =>
                {
                    enemy.EnableMovement();

                    enemy.StartShooting();

                    resetTimer.Stop();
                    resetTimer.Dispose();
                };
                resetTimer.Start();
            }
            player.IsColliding = false;
            enemy.IsColliding = false;
        }






    }
}
