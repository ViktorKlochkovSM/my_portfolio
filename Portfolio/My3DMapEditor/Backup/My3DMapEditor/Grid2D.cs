using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace My3DMapEditor
{
    public class Grid2D
    {
        public Image[] img = new Image[7];

        public int sizeGrid = 2;
        public int mainSizeGrid = 16;
        private Pen penOs = new Pen(Color.Blue,1);
        private Pen gridPen = new Pen(Color.FromArgb(0, 0, 120));

        public Grid2D()
        {
        }
        /// <summary>
        /// привязка точки к сетке
        /// </summary>
        /// <param name="pt">точка</param>
        /// <returns>привязанный новый Point</returns>
        public Point SnapingPoint(Point pt)
        {
            int newXCoord = -Form1.panelWidth;
            int newYCoord = -Form1.panelHeight;

            if (pt.X > Form1.panelWidth / 2)
            {
                int t = pt.X - Form1.panelWidth / 2;
                t /= mainSizeGrid;
                newXCoord = Form1.panelWidth / 2 + (t * mainSizeGrid);
            }
            else
            {
                int t = Form1.panelWidth / 2 - pt.X;
                t /= mainSizeGrid;
                newXCoord = Form1.panelWidth / 2 - (t * mainSizeGrid);
            }

            if (pt.Y > Form1.panelHeight / 2)
            {
                int t = pt.Y - Form1.panelHeight / 2;
                t /= mainSizeGrid;

                newYCoord = (Form1.panelHeight / 2 + (t * mainSizeGrid));
            }
            else
            {
                int t = Form1.panelHeight / 2 - pt.Y;
                t /= mainSizeGrid;
                newYCoord = (Form1.panelHeight / 2 - (t * mainSizeGrid));
            }

            return new Point(newXCoord, newYCoord);
        }
        public Point ConvertGlobalCoordsToLocal(Point pt)
        {
            Point pZerro = new Point(Form1.panelWidth/2, Form1.panelHeight/2);
            int newXCoord = -Form1.panelWidth;
            int newYCoord = -Form1.panelHeight;

            if (pt.X > pZerro.X || pt.X < pZerro.X)
            {
                newXCoord = pt.X - pZerro.X;
            }
            else
            {
                newXCoord = 0;
            }

            if (pt.Y > pZerro.Y || pt.Y < pZerro.Y)
            {
                newYCoord = pZerro.Y - pt.Y;
            }
            else
            {
                newYCoord = 0;
            }

            return new Point(newXCoord, newYCoord);
        }
        public float ConvertLocalCoordsToGlobal(float pt,int f)
        {
            Point pZerro = new Point(Form1.panelWidth / 2, Form1.panelHeight / 2);
            int newXCoord = -Form1.panelWidth;
            int newYCoord = -Form1.panelHeight;

            switch (f)
            {
                case 0:
                    if (pt > 0 || pt < 0)
                    {
                        newXCoord = (int)pt + Form1.panelWidth / 2;
                    }
                    else
                    {
                        newXCoord = Form1.panelWidth / 2;
                    }
                    break;

                case 1:
                    if (pt > 0 || pt < 0)
                    {
                        newXCoord = (-1 * (int)pt) + Form1.panelWidth / 2;
                    }
                    else
                    {
                        newXCoord = Form1.panelWidth / 2;
                    }
                    break;

                case 2:
                    if (pt > 0 || pt < 0)
                    {
                        newXCoord = (-1*(int)pt) + Form1.panelWidth / 2;
                    }
                    else
                    {
                        newXCoord = Form1.panelWidth / 2;
                    }
                    break;
            }

            return newXCoord;
        }
        public void DrawGrid(Graphics g,int h,int v,Rectangle r)
        {
            int hv = h;
            int vv = v;
            int lx = 0;
            int ly = 0;
            int rx = 0;
            int ry = 0;


            lx = Form1.panelWidth / 2 - ((((Form1.panelWidth / 2) - (hv - 20)) / mainSizeGrid) * mainSizeGrid);
            rx = lx + 20 + r.Width;

            ly = Form1.panelHeight / 2 - ((((Form1.panelHeight / 2) - (vv - 20)) / mainSizeGrid) * mainSizeGrid);
            ry = ly + 20 + r.Height;

            for (int i = lx; i < rx; i += mainSizeGrid)
            {
                g.DrawLine(gridPen, new Point(i, vv - 50), new Point(i, vv + 50 + r.Height));
            }
            for (int i = ly; i < ry; i += mainSizeGrid)
            {
                g.DrawLine(gridPen, new Point(hv - 50, i), new Point( hv + 50 + r.Width,i));
            }

            g.DrawLine(penOs, new Point(Form1.panelWidth / 2, 0), new Point(Form1.panelWidth / 2, Form1.panelHeight));
            g.DrawLine(penOs, new Point(0, Form1.panelHeight / 2), new Point(Form1.panelWidth, Form1.panelHeight / 2));
        }
    }
}
