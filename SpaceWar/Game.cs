using System;

namespace SpaceWar
{
    public class Game
    {
        private GameScreen gameScreen;
        private Spaceship spaceship;

        public Game(GameScreen screen)
        {
            gameScreen = screen;
            spaceship = new Spaceship(gameScreen); 
        }

        public void RotateOnKeyPress(string direction)
        {
            spaceship.RotateOnKeyPress(direction);
        }

       
        public void Move(string direction)
        {
            switch (direction.ToLower())
            {
                case "a": 
                    spaceship.Move("a");
                    break;
                case "d": 
                    spaceship.Move("d");
                    break;
                case "w": 
                    spaceship.Move("w");
                    break;
                case "s": 
                    spaceship.Move("s");
                    break;
            }
        }

        public void Shoot()
        {
            spaceship.Shoot();
        }
        public Spaceship GetSpaceship()
        {
            return spaceship;
        }

    }
}
