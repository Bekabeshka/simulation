using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Care_Pathway.Models
{
    abstract class Person
    {
        public int id;
        static int count = 0;
        public PictureBox pictureBox;

        public Person()
        {
            this.id = count;
            count += 1;
            this.pictureBox = new PictureBox();
            this.pictureBox.Size = new Size(64, 64);
            this.pictureBox.SizeMode = PictureBoxSizeMode.CenterImage;
        }
        public Person(Point location) : this()
        {
            this.pictureBox.Location = location;
        }

        public void MoveTo(Point location)
        {
            this.MoveTo(location.X, location.Y);
        }

        public void MoveTo(int x, int y)
        {
            int dx = (int)(x - this.pictureBox.Location.X) / 8;
            int dy = (int)(y - this.pictureBox.Location.Y) / 8;
            this.pictureBox.Location = new Point(this.pictureBox.Location.X + dx, this.pictureBox.Location.Y + dy);

            //Console.WriteLine(dx + " " + dy);
        }

        public void ToFront()
        {
            this.pictureBox.BringToFront();
        }
    }
}
