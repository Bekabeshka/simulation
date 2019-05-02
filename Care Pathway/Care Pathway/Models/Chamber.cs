using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Care_Pathway.Models
{
    class Chamber
    {
        public int id;
        public bool isBusy;
        static int counter = 0;
        public PictureBox pictureBox;

        private Chamber() : base()
        {
            this.id = counter;
            counter += 1;
            this.isBusy = false;
        }

        public Chamber(PictureBox pictureBox) : this ()
        {
            this.pictureBox = pictureBox;
        }
        
    }
}
