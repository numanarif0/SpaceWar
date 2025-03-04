using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceWar
{
    public class Meteor
    {
        public PictureBox MeteorPicture { get; private set; }
        private Form gameForm;
        private int speed;

        public Meteor(Form form, int spawnX, int spawnY, int speed = 5)
        {
            this.gameForm = form;
            this.speed = speed;

            MeteorPicture = new PictureBox
            {
                Size = new Size(50, 50), 
                Image = Image.FromFile("meteorImage.png"), 
                Location = new Point(spawnX, spawnY),
                BackColor = Color.Transparent,
                Tag = "meteor"
            };

            gameForm.Controls.Add(MeteorPicture);
            MeteorPicture.SendToBack();
        }

        public void Move()
        {
            if (MeteorPicture != null)
            {
                MeteorPicture.Top += speed; 
                if (MeteorPicture.Top > gameForm.ClientSize.Height)
                {
                    Destroy(); 
                }
            }
        }

        public void Destroy()
        {
            if (MeteorPicture != null && MeteorPicture.Parent != null)
            {
                gameForm.Controls.Remove(MeteorPicture);
                MeteorPicture.Dispose();
            }
        }
    }
}
