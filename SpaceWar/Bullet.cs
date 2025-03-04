    using System;
    using System.Drawing;
    using System.Windows.Forms;

    namespace SpaceWar

    {

        public class Bullet
        {
            public int Damage { get; private set; }
            public int Speed { get; private set; }
            public PictureBox BulletPicture { get; private set; }

            private Form gameForm;
            private float directionX; 
            private float directionY;
        public DateTime CreationTime { get; private set; }
        public Bullet(Form form, Point startPosition, float rotationAngle, int damage = 10, int speed = 10)
            {
            CreationTime = DateTime.Now; 


            Damage = damage;
                Speed = speed;
                gameForm = form;

                double angleInRadians = rotationAngle * Math.PI / 180.0; 
                directionX = (float)Math.Cos(angleInRadians) * Speed;
                directionY = (float)Math.Sin(angleInRadians) * Speed;

                
                BulletPicture = new PictureBox
                {
                    Size = new Size(5, 5),
                    BackColor = Color.Red, 
                    Location = startPosition
                };
           

            gameForm.Controls.Add(BulletPicture);
            




        }

        public void Move()
            {    
                BulletPicture.Left += (int)directionX;
                BulletPicture.Top -= (int)directionY;

            if (BulletPicture.Right < 0 || BulletPicture.Left > gameForm.ClientSize.Width ||
                    BulletPicture.Bottom < 0 || BulletPicture.Top > gameForm.ClientSize.Height)
                {
                    Destroy();
                }
            }

            public void Destroy()
            {
                gameForm.Controls.Remove(BulletPicture);
                BulletPicture.Dispose();
            }
        }
    }
