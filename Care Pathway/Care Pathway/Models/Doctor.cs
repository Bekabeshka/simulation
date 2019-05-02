using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Care_Pathway.Models
{
    class Doctor : Person
    {
        public bool isBusy;
        
        public Doctor(Point location) : base(location)
        {
            this.isBusy = false;
            this.pictureBox.Image = Properties.Resources.nurse;
        }
        
    }
}
