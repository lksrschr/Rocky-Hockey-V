using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using System.Reflection;
using RockyHockey.Common;

using AForge;
using AForge.Imaging;
using AForge.Imaging.Filters;
using AForge.Math.Geometry;

using AForge.Vision;
using System.Drawing.Drawing2D;
using System.Linq;

namespace RockyHockeyGUI
{
    public partial class GameFieldDetectionView : Form
    {
        public GameFieldDetectionView(Bitmap imgInput)
        {
            
            InitializeComponent();
            this.RectanglePicBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.imgInput = imgInput;
            RectanglePicBox.Image = this.imgInput;
        }
        Bitmap imgInput;
        public ArrayList clickcoordinatelist = new ArrayList();
        public ArrayList sortedcoordinatelist = new ArrayList();
        public ArrayList fulllist = new ArrayList();
        public ArrayList axiscoordinatelist = new ArrayList();


        private void RectanglePicBox_Click(object sender, EventArgs e)
        {
            if (clickcoordinatelist.Count <= 3)
            {
                MouseEventArgs me = (MouseEventArgs)e;
                System.Drawing.Point coordinates = me.Location;
                clickcoordinatelist.Add(coordinates);
                textBox1.Text += "" + coordinates;

                var X = me.Location.X;
                var Y = me.Location.Y;


                drawPoint(X, Y);
            }
        }

        public void drawPoint(int x, int y)
        {
            Graphics g = Graphics.FromHwnd(RectanglePicBox.Handle);
            SolidBrush brush = new SolidBrush(Color.LimeGreen);
            System.Drawing.Point dPoint = new System.Drawing.Point(x, y);
            Rectangle rect = new Rectangle(dPoint, new Size(4, 4));
            g.FillRectangle(brush, rect);
            g.Dispose();
        }

        private void DrawRectangleButton_Click(object sender, EventArgs e)
        {

            FillSortedArray();
            fulllist = sortedcoordinatelist;
            if (sortedcoordinatelist.Count > 0)
            {
                Graphics g = Graphics.FromHwnd(RectanglePicBox.Handle);
                Pen pen = new Pen(Color.Green, 3);

                for (int i = 0; i < sortedcoordinatelist.Count - 1; i++)
                {
                    System.Drawing.Point p1 = (System.Drawing.Point)sortedcoordinatelist[i];
                    System.Drawing.Point p2 = (System.Drawing.Point)sortedcoordinatelist[i + 1];
                    g.DrawLine(pen, p1.X, p1.Y, p2.X, p2.Y);

                }

                System.Drawing.Point k1 = (System.Drawing.Point)sortedcoordinatelist[0];
                System.Drawing.Point k2 = (System.Drawing.Point)sortedcoordinatelist[3];
                g.DrawLine(pen, k1.X, k1.Y, k2.X, k2.Y);
                DetermineandSaveAxes();
            }
        }

        private void ResetCoordinatesButton_Click(object sender, EventArgs e)
        {
            clickcoordinatelist = new ArrayList();
            sortedcoordinatelist = new ArrayList();
            textBox1.Text = "";
            RectanglePicBox.Image = imgInput;
        }

        private void StartGameButton_Click(object sender, EventArgs e)
        {
            //
            DetermineGameField();
            this.Close();
        }

        private void DetermineGameField()
        {
            FillSortedArray();
            Config.Instance.GameField.UpperLeft = (System.Drawing.Point)sortedcoordinatelist[0];
            Config.Instance.GameField.UpperRight = (System.Drawing.Point)sortedcoordinatelist[1];
            Config.Instance.GameField.LowerRight = (System.Drawing.Point)sortedcoordinatelist[2];
            Config.Instance.GameField.LowerLeft = (System.Drawing.Point)sortedcoordinatelist[3];
            DetermineandSaveAxes();
        }

        private void DetermineandSaveAxes()
        {
            int smallestx = getMinxy(clickcoordinatelist, "X");
            int smallesty = getMinxy(clickcoordinatelist, "Y");
            int highestx = getMaxxy(clickcoordinatelist, "X");
            int highesty = getMaxxy(clickcoordinatelist, "Y");

            //Informatiker-Koordinatensystem
            System.Drawing.Point smallestxsmallesty = new System.Drawing.Point(smallestx, smallesty); // Hier ist der 0 Punkt im Koordinatensystem(Oben links)
            Config.Instance.GameField.Offset = smallestxsmallesty;
            Config.Instance.GameField.XYOrigin = smallestxsmallesty;
            
            System.Drawing.Point highestxsmallesty = new System.Drawing.Point(highestx, smallesty); // Hier ist der Punkt ganz oben rechts
            Config.Instance.GameField.ExtremeY = highestxsmallesty;

            System.Drawing.Point smallestxhighesty = new System.Drawing.Point(smallestx, highesty); // Hier ist der Punkt ganz unten links
            Config.Instance.GameField.ExtremeX = smallestxhighesty;

            axiscoordinatelist.Add(smallestxhighesty);
            axiscoordinatelist.Add(smallestxsmallesty);
            axiscoordinatelist.Add(highestxsmallesty);



            DrawAxis();

        }


        private void DrawAxis()
        {
            Graphics graphics = Graphics.FromHwnd(RectanglePicBox.Handle);
            Pen pen = new Pen(Color.Red, 3);
            System.Drawing.Point p1 = (System.Drawing.Point)axiscoordinatelist[0];
            System.Drawing.Point p2 = (System.Drawing.Point)axiscoordinatelist[1];
            System.Drawing.Point p3 = (System.Drawing.Point)axiscoordinatelist[2];
            graphics.DrawLine(pen, p1.X, p1.Y, p2.X, p2.Y);
            graphics.DrawLine(pen, p2.X, p2.Y, p3.X, p3.Y);
        }

        private void FillSortedArray()
        {
            System.Drawing.Point upperleft, upperright, lowerright, lowerleft;
            System.Drawing.Point temp = (System.Drawing.Point)clickcoordinatelist[0];
            //upperleft

            for (int i = 0; i < clickcoordinatelist.Count; i++)
            {
                if (temp.X + temp.Y > ((System.Drawing.Point)clickcoordinatelist[i]).X + ((System.Drawing.Point)clickcoordinatelist[i]).Y)
                {
                    temp = (System.Drawing.Point)clickcoordinatelist[i];
                }
            }
            upperleft = temp;

            //lowerright
            for (int i = 0; i < clickcoordinatelist.Count; i++)
            {
                if (temp.X + temp.Y <= ((System.Drawing.Point)clickcoordinatelist[i]).X + ((System.Drawing.Point)clickcoordinatelist[i]).Y)
                {
                    temp = (System.Drawing.Point)clickcoordinatelist[i];
                }
            }
            lowerright = temp;

            //Delete upperleft and lowerright
            for (int i = 0; i < clickcoordinatelist.Count; i++)
            {
                int x = 0;
                if ((upperleft.X + upperleft.Y) == ((System.Drawing.Point)clickcoordinatelist[i]).X + ((System.Drawing.Point)clickcoordinatelist[i]).Y)
                {
                    clickcoordinatelist.RemoveAt(i);
                    x = 1;
                }

                if ((lowerright.X + lowerright.Y) == ((System.Drawing.Point)clickcoordinatelist[i]).X + ((System.Drawing.Point)clickcoordinatelist[i]).Y)
                {
                    clickcoordinatelist.RemoveAt(i);
                    x = 1;
                }
                if (x == 1) i = -1;

            }

            //Filter highestxsamllesty
            temp = (System.Drawing.Point)clickcoordinatelist[0];
            if (temp.X < ((System.Drawing.Point)clickcoordinatelist[1]).X)
            {
                upperright = (System.Drawing.Point)clickcoordinatelist[1];
                lowerleft = temp;
            }
            else
            {
                lowerleft = (System.Drawing.Point)clickcoordinatelist[1];
                upperright = temp;
            }



            sortedcoordinatelist.Add(upperleft);
            sortedcoordinatelist.Add(upperright);
            sortedcoordinatelist.Add(lowerright);
            sortedcoordinatelist.Add(lowerleft);


        }

        public int getMaxxy(ArrayList fulllist, String xy)
        {
            int highesty = 0;
            int highestx = 0;
            int max = int.MinValue;

            if (xy == "Y")
            {
                max = int.MinValue;
                for (int i = 0; i < fulllist.Count; i++)
                {
                    System.Drawing.Point p = (System.Drawing.Point)fulllist[i];

                    if (p.Y > max)
                    {
                        max = p.Y;
                        highesty = p.Y;
                    }

                }

                return highesty;

            }
            else if (xy == "X")
            {
                max = int.MinValue;
                for (int i = 0; i < fulllist.Count; i++)
                {
                    System.Drawing.Point p = (System.Drawing.Point)fulllist[i];

                    if (p.X > max)
                    {
                        max = p.X;
                        highestx = p.X;
                    }

                }
                return highestx;
            }
            return 0;
        }

        public int getMinxy(ArrayList fulllist, String xy)
        {
            int smallestx = 0;
            int smallesty = 0;
            int min = int.MaxValue;

            if (xy == "Y")
            {
                min = int.MaxValue;
                for (int i = 0; i < fulllist.Count; i++)
                {
                    System.Drawing.Point p = (System.Drawing.Point)fulllist[i];

                    if (p.Y < min)
                    {
                        min = p.Y;
                        smallesty = p.Y;
                    }

                }
                return smallesty;
            }
            else if (xy == "X")
            {
                min = int.MaxValue;
                for (int i = 0; i < fulllist.Count; i++)
                {
                    System.Drawing.Point p = (System.Drawing.Point)fulllist[i];

                    if (p.X < min)
                    {
                        min = p.X;
                        smallestx = p.X;
                    }

                }
                return smallestx;
            }
            return 0;
        }
    }
}
