using SpaceWar;

internal class FastEnemy : Enemy
{
    public FastEnemy(Form gameForm, int spawnX, int spawnY, Spaceship targetSpaceship)
        : base(gameForm, "FastEnemyImage.png", spawnX, spawnY, targetSpaceship,20)
    {
        Health = 50;
        Speed = 5; 
        Damage = 10;
    }

}
