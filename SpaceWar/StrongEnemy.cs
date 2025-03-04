using System;
using System.Windows.Forms;

namespace SpaceWar
{
    internal class StrongEnemy : Enemy
    {
        public StrongEnemy(Form gameForm, int spawnX, int spawnY, Spaceship targetSpaceship)
            : base(gameForm, "StrongEnemyImage.png", spawnX, spawnY, targetSpaceship,100)
        {
            Health = 100; 
            Speed = 2;    
            Damage = 30;  
        }


    }
}
