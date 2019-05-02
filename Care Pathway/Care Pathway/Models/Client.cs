using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Care_Pathway.Models
{
    enum State {
        INLINE,
        MOVING,
        WAITING,
        HEALING,
        RECOVERED,
        DEAD
    }

    class Client : Person
    {
        public int illnessDegree;
        public int watingTime;
        int healingTime;
        public State state;
        
        private Client(Point location) : base(location)
        {
            this.pictureBox.Image = Properties.Resources.patient;
        }
        public Client(int illnessDegree, Point location) : this(location)
        {
            this.watingTime = 0;
            this.healingTime = 0;
            this.illnessDegree = illnessDegree;
            this.state = State.INLINE;
        }

        public void CheckDead()
        {
            if (this.watingTime > this.illnessDegree * 25 && this.state != State.HEALING)
                this.state = State.DEAD;
        }

        public void CheckRecovered()
        {
            if (this.healingTime > this.illnessDegree * 5)
                this.state = State.RECOVERED;
        }

        public void WatingTimeTick()
        {
            this.watingTime += 1;
        }

        public void HealingTimeTick()
        {
            this.healingTime += 1;
        }

        public string toString()
        {
            return "illness degree: " + this.illnessDegree + "\nhealing time: " + this.healingTime + "\nlocation: " + this.pictureBox.Location.ToString() + "\nstate: " + this.state.ToString() + '\n';
        } 
    }
}
