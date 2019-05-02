using Care_Pathway.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Care_Pathway
{
    public partial class Form : System.Windows.Forms.Form
    {
        List<PictureBox> chambersPB;
        List<Doctor> doctors;
        List<Client> clients;
        List<Chamber> chambers;
        Dictionary<Chamber, Client> chamberClient;
        Dictionary<Doctor, Chamber> doctorChamber;

        Queue<Client> queue;

        Random rand;
        
        Point spawn = new Point(20, 480);
        Point docSpawn = new Point(113, 278);

        public Form()
        {
            InitializeComponent();
            
            this.rand = new Random();

            this.queue = new Queue<Client>();
            this.clients = new List<Client>();
            this.doctors = new List<Doctor>();
            this.chambers = new List<Chamber>();
            
            this.chamberClient = new Dictionary<Chamber, Client>();
            this.doctorChamber = new Dictionary<Doctor, Chamber>();

            this.createChamberClient();
            this.createrDoctors();
        }

        private void createrDoctors()
        {
            for (int i = 0; i < 4; i++)
            {
                Doctor temp = new Doctor(docSpawn);
                this.doctors.Add(temp);
                this.Controls.Add(temp.pictureBox);
                temp.ToFront();

                this.doctorChamber.Add(temp, null);
            }
        }

        private void createChamberClient()
        {
            List<PictureBox> pictureBoxes = this.Controls.OfType<PictureBox>().ToList<PictureBox>();

            this.chambersPB = pictureBoxes.Where(pb => pb.Name.Contains("chamber")).ToList<PictureBox>();
            foreach (PictureBox chbx in this.chambersPB)
                this.chambers.Add(new Chamber(chbx));
            
            foreach (Chamber ch in this.chambers)
                this.chamberClient.Add(ch, null);
            
        }

        private void start_Click(object sender, EventArgs e)
        {
            this.timerCreate.Enabled = true;
            this.mainTimer.Enabled = true;
        }

        private bool IsAllBusy()
        {
            foreach (Chamber ch in this.chambers)
            {
                if (!ch.isBusy)
                {
                    return false;
                }
            }
            return true;
        }

        private void AllocateClients()
        {
            while (this.queue.Count > 0 && !this.IsAllBusy())
            {
                Client temp = this.queue.Peek();

                if (temp.state == State.DEAD)
                {
                    this.queue.Dequeue();
                    continue;
                }

                foreach (Chamber ch in this.chambers)
                {
                    if (!ch.isBusy)
                    {
                        temp.state = State.MOVING;
                        this.queue.Dequeue();
                        ch.isBusy = true;
                        this.chamberClient[ch] = temp;
                        break;
                    }
                }
            }
        }

        private void mainTimer_Tick(object sender, EventArgs e)
        {
            this.AllocateClients();

            foreach (Client cl in this.clients)
            {
                if (cl.state == State.HEALING)
                {
                    cl.HealingTimeTick();
                    cl.CheckRecovered();
                }
                else if (cl.state == State.INLINE || cl.state == State.MOVING || cl.state == State.WAITING)
                {
                    cl.WatingTimeTick();
                    cl.CheckDead();
                }
            }

            foreach (KeyValuePair<Chamber, Client> chcl in this.chamberClient)
            {
                if (chcl.Value != null && chcl.Value.state == State.MOVING)
                {
                    if (this.IsIn(chcl.Value.pictureBox.Location, new Point(chcl.Key.pictureBox.Location.X + 36, chcl.Key.pictureBox.Location.Y + 2)))
                        chcl.Value.state = State.WAITING;
                }
            }

            foreach (KeyValuePair<Chamber, Client> chcl in this.chamberClient)
            {
                if (chcl.Value != null)
                {
                    if (chcl.Value.state == State.MOVING)
                    {
                        chcl.Value.MoveTo(chcl.Key.pictureBox.Location.X + 36, chcl.Key.pictureBox.Location.Y + 2);
                    }
                    else if (chcl.Value.state == State.RECOVERED)
                    {
                        chcl.Key.isBusy = false;
                    }
                }
            }

            foreach (Client cl in this.clients)
            {
                if (cl.state == State.RECOVERED)
                {
                    cl.MoveTo(exit.Location);
                }
            }

            foreach (KeyValuePair<Doctor, Chamber> doch in this.doctorChamber)
            {
                if (doch.Value != null)
                {
                    doch.Key.MoveTo(doch.Value.pictureBox.Location);
                }
            }

            foreach (Client cl in this.clients)
            {
                if (cl.state == State.DEAD || (cl.state == State.RECOVERED && this.IsIn(cl.pictureBox.Location, exit.Location)))
                {
                    this.Controls.Remove(cl.pictureBox);
                }
            }
           
            foreach (KeyValuePair<Chamber, Client> chcl in this.chamberClient)
            {
                if (chcl.Value != null)
                {
                    foreach (Doctor doc in this.doctors)
                    {
                        if (this.doctorChamber[doc] != null && !this.doctorChamber[doc].isBusy)
                        {
                            doc.isBusy = false;
                        }
                        else if (!doc.isBusy && chcl.Value.state == State.WAITING)
                        {
                            doc.isBusy = true;
                            chcl.Value.state = State.HEALING;
                            this.doctorChamber[doc] = chcl.Key;
                        }
                    }
                }
            }


        }

        private Chamber GetAlmostDead()
        {
            KeyValuePair<Chamber, Client> maxi = new KeyValuePair<Chamber, Client>(new Chamber(new PictureBox()), new Client(0, new Point()));
            foreach (KeyValuePair<Chamber, Client> chcl in this.chamberClient)
            {
                if (chcl.Value.watingTime >= maxi.Value.watingTime && chcl.Value.state == State.WAITING)
                    maxi = chcl;
            }
            return maxi.Key;
        }

        private bool IsIn(Point location, Point locationCh)
        {
            if (location.X >= locationCh.X - 15 && location.X <= locationCh.X + 15 && location.Y >= locationCh.Y - 15 && location.Y <= locationCh.Y + 15)
                return true;
            return false;
        }

        private void Form_MouseClick(object sender, MouseEventArgs e)
        {
            Console.WriteLine(e.Location);
        }

        private void timerCreate_Tick(object sender, EventArgs e)
        {
            int n = this.rand.Next(1, 10);
            if (n > 7)
            {
                int num = this.rand.Next(2, 5);
                Client temp = new Client(num, this.spawn);
                this.queue.Enqueue(temp);
                this.clients.Add(temp);
                this.Controls.Add(temp.pictureBox);
                temp.ToFront();
                //Console.WriteLine(temp);
            }
        }

        private void pause_Click(object sender, EventArgs e)
        {
            this.mainTimer.Enabled = false;
            this.timerCreate.Enabled = false;
        }

        private void pausec_Click(object sender, EventArgs e)
        {
            this.timerCreate.Enabled = false;
        }

        private void resumec_Click(object sender, EventArgs e)
        {
            this.timerCreate.Enabled = true;
        }
    }
}

/*
            foreach (Client cl in this.clients)
            {
                if (cl.state == State.HEALING)
                {
                    cl.HealingTimeTick();
                    cl.CheckRecovered();
                }
                else if (cl.state == State.INLINE || cl.state == State.MOVING || cl.state == State.WAITING)
                {
                    cl.WatingTimeTick();
                    cl.CheckDead();
                }
                else if (cl.state == State.DEAD)
                {
                    this.Controls.Remove(cl.pictureBox);
                }
                else if (cl.state == State.RECOVERED)
                {
                    cl.MoveTo(exit.Location);
                }

                //Console.WriteLine(cl.state.ToString());
            }

            foreach (KeyValuePair<Chamber, Client> chcl in this.chamberClient)
            {
                if (chcl.Value != null)
                {
                    if (chcl.Value.state == State.MOVING)
                    {
                        chcl.Value.MoveTo(chcl.Key.pictureBox.Location.X + 36, chcl.Key.pictureBox.Location.Y + 2);
                        if (this.IsIn(chcl.Value.pictureBox.Location, new Point(chcl.Key.pictureBox.Location.X + 36, chcl.Key.pictureBox.Location.Y + 2)))
                            chcl.Value.state = State.WAITING;
                    }
                    else if (chcl.Value.state == State.DEAD)
                    {
                        chcl.Key.isBusy = false;
                    }
                    else if (chcl.Value.state == State.RECOVERED)
                    {
                        chcl.Key.isBusy = false;
                        this.doctorChamber.First(x => x.Value == chcl.Key).Key.isBusy = false;
                    }
                }
            }
            */
