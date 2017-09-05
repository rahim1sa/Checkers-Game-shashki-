using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        int turn = 0;
        bool movExtra = false;
        PictureBox selected = null;

        List<PictureBox> cats = new List<PictureBox>();
        List<PictureBox> dogs = new List<PictureBox>();
        public Form1()
        {
            InitializeComponent();
            loadLists();
        }

        private void loadLists()
        {
            cats.Add(cat1);
            cats.Add(cat2);
            cats.Add(cat3);
            cats.Add(cat4);
            cats.Add(cat5);
            cats.Add(cat6);
            cats.Add(cat7);
            cats.Add(cat8);
            cats.Add(cat9);
            cats.Add(cat10);
            cats.Add(cat11);
            cats.Add(cat12);

            dogs.Add(dog1);
            dogs.Add(dog2);
            dogs.Add(dog3);
            dogs.Add(dog4);
            dogs.Add(dog5);
            dogs.Add(dog6);
            dogs.Add(dog7);
            dogs.Add(dog8);
            dogs.Add(dog9);
            dogs.Add(dog10);
            dogs.Add(dog11);
            dogs.Add(dog12);
        }

        public void selection (object obj)
        {

            if (!movExtra)
            {
                try { selected.BackColor = Color.Black; }
                catch { }
                PictureBox file = (PictureBox)obj;
                selected = file;
                selected.BackColor = Color.IndianRed;
            }
            
        }

        private void pictureClick(object sender, MouseEventArgs e)
        {
            movement((PictureBox)sender);
        }

        private void movement(PictureBox picture)
        {
            if (selected != null)
            {

                string color = selected.Name.ToString().Substring(0, 3);
                if (/*confirm(selected, picture, color)*/true)//validacion-conrimation(onaylama)
                {
                    
                    Point prev = selected.Location;
                    selected.Location = picture.Location;
                    int progress = prev.Y - picture.Location.Y;

                    if (!ExtraMove(color) | Math.Abs(progress) == 50)//Check extra moves
                    {
                        IfQuenn(color);
                        turn++;
                        selected.BackColor = Color.Black;
                        selected = null;
                        movExtra = false;
                    }
                    else
                    {
                        movExtra = true;
                    }
                }
            }
        }

        private bool ExtraMove(string color) 
        {
            List<PictureBox> oppositeSide = color == "cat" ? dogs : cats;
            List<Point> positions = new List<Point>();
            int followPos = color == "cat" ? -100 : 100;

            positions.Add(new Point(selected.Location.X + 100, selected.Location.Y + followPos));
            positions.Add(new Point(selected.Location.X - 100, selected.Location.Y + followPos));
            if (selected.Tag == "queen")
            {
                positions.Add(new Point(selected.Location.X + 100, selected.Location.Y - followPos));
                positions.Add(new Point(selected.Location.X - 100, selected.Location.Y - followPos));
            }

            bool result = false;
            for (int i = 0; i < positions.Count; i++)
            {
                if (positions[i].X >= 50 && positions[i].X <= 400 && positions[i].Y >= 50 && positions[i].Y <= 400)
                {
                    if (!occupied(positions[i], cats) && !occupied(positions[i], dogs))
                    {
                        Point pointHalf = new Point(average(positions[i].X, selected.Location.X), average(positions[i].Y, selected.Location.Y));
                        if (occupied(pointHalf, oppositeSide))
                        {
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        private bool occupied(Point point, List<PictureBox> bundle)
        {
            for (int i = 0; i < bundle.Count; i++)
            {
                if (point == bundle[i].Location)
                {
                    return true;
                }
            }
            return false;
        }

        private int average(int n1, int n2)
        {
            int result = n1 + n2;
            result = result / 2;
            return Math.Abs(result);
        }

        private bool confirm(PictureBox origin, PictureBox destination, string color)
        {
            Point pointOrigin = origin.Location;
            Point pointDestination = destination.Location;
            int progress = pointOrigin.Y - pointDestination.Y;
            progress = color == "cat" ? progress : (progress * -1);
            progress = selected.Tag == "queen" ? Math.Abs(progress) : progress;

            if (progress == 50)
            {
                return true;
            }
            else if (progress == 100)
            {
                Point pointHalf = new Point(average(pointDestination.X, pointOrigin.X), average(pointDestination.Y, pointOrigin.Y));
                List<PictureBox> oppositeSide = color == "cat" ? dogs : cats;
                for (int i = 0; i < oppositeSide.Count; i++)
                {
                    if (oppositeSide[i].Location == pointHalf)
                    {
                        oppositeSide[i].Location = new Point(0, 0);
                        oppositeSide[i].Visible = false;
                        return true;
                    }
                }
            }
            return false;
        }

        private void IfQuenn(string color)
        {
            if (color == "dog" && selected.Location.Y == 411)
            {
                selected.BackgroundImage = Properties.Resources.queen;
                selected.Tag = "queen";
            }
            else if (color == "cat" && selected.Location.Y == 12)
            {
                selected.BackgroundImage = Properties.Resources.queen;
                selected.Tag = "queen";
            }
        }

        private void selectedCat(object sender, MouseEventArgs e)
        {
            if (turn % 2 == 1)
            {
                selection(sender);
            }
            else
            {
                MessageBox.Show("The next move of black checker");
            }
            
            
        }

        private void selectedDog(object sender, MouseEventArgs e)
        {
            if (turn % 2 == 0)
            {
                selection(sender);
            }
            else
            {
                MessageBox.Show("The next move of black checker");
            }
        }
    }
}
