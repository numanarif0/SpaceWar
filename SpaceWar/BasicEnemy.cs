using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceWar
{
    internal class BasicEnemy : Enemy
    {
        private int moveDirection = 1; // Hareket yönü: 1 sağa, -1 sola

        public BasicEnemy(Form gameForm, int spawnX, int spawnY, Spaceship targetSpaceship)
     : base(gameForm, "BasicEnemyImage.png", spawnX, spawnY, targetSpaceship,50)
        {
            Health = 50;
            Speed = 3;
            Damage = 10;
        }


        public override void Move()
        {
            EnemyPicture.Left += Speed * moveDirection;

             if (EnemyPicture.Left > EnemyPicture.Parent.ClientSize.Width)
            {
                EnemyPicture.Left = -EnemyPicture.Width; 
            }

            if (EnemyPicture.Right < 0)
            {
                EnemyPicture.Left = EnemyPicture.Parent.ClientSize.Width; 
            }
        }


    }
}
