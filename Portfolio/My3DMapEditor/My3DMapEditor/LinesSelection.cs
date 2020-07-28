using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace My3DMapEditor
{
    class LinesSelection:IDisposable
    {
        private Point[] masPointFront = new Point[10];
        private Point[] masPointRight = new Point[10];
        private Point[] masPointTop = new Point[10];

        public bool isFrontSide = true;

        private Rectangle[] masRectsFront = new Rectangle[8];
        private Rectangle[] masRectsRight = new Rectangle[8];
        private Rectangle[] masRectsTop = new Rectangle[8];


        private int indexOfSelectRect = 0;

        public LinesSelection(RectPrimitive r_pr)
        {
            masPointFront = r_pr.MasLinesFront;

            masPointRight = r_pr.MasLinesRight;

            masPointTop = r_pr.MasLinesTop;

            #region CreatingMasRectS

            masRectsFront[0] = new Rectangle(masPointFront[0].X - 2, masPointFront[0].Y - 2, 4, 4);
            masRectsFront[1] = new Rectangle(masPointFront[1].X - 2, masPointFront[1].Y - 2, 4, 4);
            masRectsFront[2] = new Rectangle(masPointFront[2].X - 2, masPointFront[2].Y - 2, 4, 4);
            masRectsFront[3] = new Rectangle(masPointFront[3].X - 2, masPointFront[3].Y - 2, 4, 4);

            masRectsFront[4] = new Rectangle(masPointFront[5].X - 2, masPointFront[5].Y - 2, 4, 4);
            masRectsFront[5] = new Rectangle(masPointFront[6].X - 2, masPointFront[6].Y - 2, 4, 4);
            masRectsFront[6] = new Rectangle(masPointFront[7].X - 2, masPointFront[7].Y - 2, 4, 4);
            masRectsFront[7] = new Rectangle(masPointFront[8].X - 2, masPointFront[8].Y - 2, 4, 4);



            masRectsRight[0] = new Rectangle(masPointRight[0].X - 2, masPointRight[0].Y - 2, 4, 4);
            masRectsRight[1] = new Rectangle(masPointRight[1].X - 2, masPointRight[1].Y - 2, 4, 4);
            masRectsRight[2] = new Rectangle(masPointRight[2].X - 2, masPointRight[2].Y - 2, 4, 4);
            masRectsRight[3] = new Rectangle(masPointRight[3].X - 2, masPointRight[3].Y - 2, 4, 4);

            masRectsRight[4] = new Rectangle(masPointRight[5].X - 2, masPointRight[5].Y - 2, 4, 4);
            masRectsRight[5] = new Rectangle(masPointRight[6].X - 2, masPointRight[6].Y - 2, 4, 4);
            masRectsRight[6] = new Rectangle(masPointRight[7].X - 2, masPointRight[7].Y - 2, 4, 4);
            masRectsRight[7] = new Rectangle(masPointRight[8].X - 2, masPointRight[8].Y - 2, 4, 4);



            masRectsTop[0] = new Rectangle(masPointTop[0].X - 2, masPointTop[0].Y - 2, 4, 4);
            masRectsTop[1] = new Rectangle(masPointTop[1].X - 2, masPointTop[1].Y - 2, 4, 4);
            masRectsTop[2] = new Rectangle(masPointTop[2].X - 2, masPointTop[2].Y - 2, 4, 4);
            masRectsTop[3] = new Rectangle(masPointTop[3].X - 2, masPointTop[3].Y - 2, 4, 4);

            masRectsTop[4] = new Rectangle(masPointTop[5].X - 2, masPointTop[5].Y - 2, 4, 4);
            masRectsTop[5] = new Rectangle(masPointTop[6].X - 2, masPointTop[6].Y - 2, 4, 4);
            masRectsTop[6] = new Rectangle(masPointTop[7].X - 2, masPointTop[7].Y - 2, 4, 4);
            masRectsTop[7] = new Rectangle(masPointTop[8].X - 2, masPointTop[8].Y - 2, 4, 4);

            #endregion

        }
		~LinesSelection()
		{
			Dispose();
		}
		public void Dispose()
		{
			masPointFront = null;
			masPointRight = null;
			masPointTop = null;

			masRectsFront = null;
			masRectsRight = null;
			masRectsTop = null;

			GC.SuppressFinalize(this);
		}

        #region Properties

        public int IndexOfSelectRect
        {
            get { return indexOfSelectRect; }
        }
        public Point[] MasPointFront
        {
            get { return masPointFront; }
        }
        public Point[] MasPointRight
        {
            get { return masPointRight; }
        }
        public Point[] MasPointTop
        {
            get { return masPointTop; }
        }
        
        public Rectangle[] MasRectsFront
        {
            get { return masRectsFront; }
        }
        public Rectangle[] MasRectsRight
        {
            get { return masRectsRight; }
        }
        public Rectangle[] MasRectsTop
        {
            get { return masRectsTop; }
        }

        #endregion

        #region Functions

        public System.Windows.Forms.Cursor isMouseUnderRegionSel(Point mouse, out string s, out Point p, string nameWindow)
        {
            s = "None";
            p = new Point(0, 0);

            #region CheckRectRegions

            for (int i = 0; i < 8; i++)
            {
                if (isMouseUnderIndexesRegion(i, mouse, nameWindow))
                {
                    indexOfSelectRect = i;

                    if (i >= 4)
                        isFrontSide = false;
                    else
                        isFrontSide = true;

                    if (nameWindow == "Front")
                        p = masPointFront[i];

                    if (nameWindow == "Right")
                        p = masPointRight[i];

                    if (nameWindow == "Top")
                        p = masPointTop[i];

                    s = "Transform";

                    return System.Windows.Forms.Cursors.NoMove2D;
                }
            }

            #endregion

            indexOfSelectRect = -1;
            return System.Windows.Forms.Cursors.Cross;
        }
        private bool isMouseUnderIndexesRegion(int index, Point m, string nameWindow)
        {
            if (nameWindow == "Front")
            {
                if (masRectsFront[index].X <= m.X && masRectsFront[index].Y <= m.Y && masRectsFront[index].X + masRectsFront[index].Width >= m.X && masRectsFront[index].Y + masRectsFront[index].Height >= m.Y)
                    return true;
            }
            if (nameWindow == "Right")
            {
                if (masRectsRight[index].X <= m.X && masRectsRight[index].Y <= m.Y && masRectsRight[index].X + masRectsRight[index].Width >= m.X && masRectsRight[index].Y + masRectsRight[index].Height >= m.Y)
                    return true;
            }
            if (nameWindow == "Top")
            {
                if (masRectsTop[index].X <= m.X && masRectsTop[index].Y <= m.Y && masRectsTop[index].X + masRectsTop[index].Width >= m.X && masRectsTop[index].Y + masRectsTop[index].Height >= m.Y)
                    return true;
            }
            return false;
        }
        public void ChanginSelectedPointByIndex(string nameWindow, Point newPoint)
        {
            #region FrontSide
            if (nameWindow == "Front")
            {
                switch (indexOfSelectRect)
                {
                    case 0: masPointFront[0] = newPoint;
                        masPointFront[4] = newPoint;
                        masRectsFront[0] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointRight[5] = new Point(masPointRight[5].X, newPoint.Y);
                        masPointRight[9] = masPointRight[5];
                        masRectsRight[4] = new Rectangle(masPointRight[5].X - 2, masPointRight[5].Y - 2, 4, 4);

                        masPointTop[3] = new Point(newPoint.X, masPointTop[3].Y);
                        masRectsTop[3] = new Rectangle(masPointTop[3].X - 2, masPointTop[3].Y - 2, 4, 4);
                        break;


                    case 1: masPointFront[1] = newPoint;
                        masRectsFront[1] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointRight[0] = new Point(masPointRight[0].X, newPoint.Y);
                        masPointRight[4] = masPointRight[0];
                        masRectsRight[0] = new Rectangle(masPointRight[4].X - 2, masPointRight[4].Y - 2, 4, 4);

                        masPointTop[2] = new Point(newPoint.X, masPointTop[2].Y);
                        masRectsTop[2] = new Rectangle(masPointTop[2].X - 2, masPointTop[2].Y - 2, 4, 4);
                        break;

                    case 2: masPointFront[2] = newPoint;
                        masRectsFront[2] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointRight[3] = new Point(masPointRight[3].X, newPoint.Y);
                        masRectsRight[3] = new Rectangle(masPointRight[3].X - 2, masPointRight[3].Y - 2, 4, 4);

                        masPointTop[7] = new Point(newPoint.X, masPointTop[7].Y);
                        masRectsTop[6] = new Rectangle(masPointTop[7].X - 2, masPointTop[7].Y - 2, 4, 4);
                        break;

                    case 3: masPointFront[3] = newPoint;
                        masRectsFront[3] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointRight[8] = new Point(masPointRight[8].X, newPoint.Y);
                        masRectsRight[7] = new Rectangle(masPointRight[8].X - 2, masPointRight[8].Y - 2, 4, 4);

                        masPointTop[8] = new Point(newPoint.X, masPointTop[8].Y);
                        masRectsTop[7] = new Rectangle(masPointTop[8].X - 2, masPointTop[8].Y - 2, 4, 4);
                        break;

                    case 4: masPointFront[5] = newPoint;
                        masPointFront[9] = newPoint;
                        masRectsFront[4] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointRight[6] = new Point(masPointRight[6].X, newPoint.Y);
                        masRectsRight[5] = new Rectangle(masPointRight[6].X - 2, masPointRight[6].Y - 2, 4, 4);

                        masPointTop[0] = new Point(newPoint.X, masPointTop[0].Y);
                        masPointTop[4] = masPointTop[0];
                        masRectsTop[0] = new Rectangle(masPointTop[0].X - 2, masPointTop[0].Y - 2, 4, 4);
                        break;

                    case 5: masPointFront[6] = newPoint;
                        masRectsFront[5] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointRight[1] = new Point(masPointRight[1].X, newPoint.Y);
                        masRectsRight[1] = new Rectangle(masPointRight[1].X - 2, masPointRight[1].Y - 2, 4, 4);

                        masPointTop[1] = new Point(newPoint.X, masPointTop[1].Y);
                        masRectsTop[1] = new Rectangle(masPointTop[1].X - 2, masPointTop[1].Y - 2, 4, 4);
                        break;

                    case 6: masPointFront[7] = newPoint;
                        masRectsFront[6] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointRight[2] = new Point(masPointRight[2].X, newPoint.Y);
                        masRectsRight[2] = new Rectangle(masPointRight[2].X - 2, masPointRight[2].Y - 2, 4, 4);

                        masPointTop[6] = new Point(newPoint.X, masPointTop[6].Y);
                        masRectsTop[5] = new Rectangle(masPointTop[6].X - 2, masPointTop[6].Y - 2, 4, 4);
                        break;

                    case 7: masPointFront[8] = newPoint;
                        masRectsFront[7] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointRight[7] = new Point(masPointRight[7].X, newPoint.Y);
                        masRectsRight[6] = new Rectangle(masPointRight[7].X - 2, masPointRight[7].Y - 2, 4, 4);

                        masPointTop[5] = new Point(newPoint.X, masPointTop[5].Y);
                        masPointTop[9] = masPointTop[5];
                        masRectsTop[4] = new Rectangle(masPointTop[5].X - 2, masPointTop[5].Y - 2, 4, 4);
                        break;

                }
            }
            #endregion
            #region RightSide
            if (nameWindow == "Right")
            {
                switch (indexOfSelectRect)
                {
                    case 0: masPointRight[0] = newPoint;
                        masPointRight[4] = newPoint;
                        masRectsRight[0] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointFront[1] = new Point(masPointFront[1].X, newPoint.Y);
                        masRectsFront[1] = new Rectangle(masPointFront[1].X - 2, masPointFront[1].Y - 2, 4, 4);

                        masPointTop[2] = new Point(masPointTop[2].X, Form1.panelWidth - newPoint.X);
                        masRectsTop[2] = new Rectangle(masPointTop[2].X - 2, masPointTop[2].Y - 2, 4, 4);

                        break;

                    case 1: masPointRight[1] = newPoint;
                        masRectsRight[1] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointFront[6] = new Point(masPointFront[6].X, newPoint.Y);
                        masRectsFront[5] = new Rectangle(masPointFront[6].X - 2, masPointFront[6].Y - 2, 4, 4);

                        masPointTop[1] = new Point(masPointTop[1].X, Form1.panelWidth - newPoint.X);
                        masRectsTop[1] = new Rectangle(masPointTop[1].X - 2, masPointTop[1].Y - 2, 4, 4);
                        break;

                    case 2: masPointRight[2] = newPoint;
                        masRectsRight[2] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointFront[7] = new Point(masPointFront[7].X, newPoint.Y);
                        masRectsFront[6] = new Rectangle(masPointFront[7].X - 2, masPointFront[7].Y - 2, 4, 4);

                        masPointTop[6] = new Point(masPointTop[6].X, Form1.panelWidth - newPoint.X);
                        masRectsTop[5] = new Rectangle(masPointTop[6].X - 2, masPointTop[6].Y - 2, 4, 4);
                        break;

                    case 3: masPointRight[3] = newPoint;
                        masRectsRight[3] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointFront[2] = new Point(masPointFront[2].X, newPoint.Y);
                        masRectsFront[2] = new Rectangle(masPointFront[2].X - 2, masPointFront[2].Y - 2, 4, 4);

                        masPointTop[7] = new Point(masPointTop[7].X, Form1.panelWidth - newPoint.X);
                        masRectsTop[6] = new Rectangle(masPointTop[7].X - 2, masPointTop[7].Y - 2, 4, 4);
                        break;

                    case 4: masPointRight[5] = newPoint;
                        masPointRight[9] = newPoint;
                        masRectsRight[4] = new Rectangle(masPointRight[5].X - 2, masPointRight[5].Y - 2, 4, 4);

                        masPointFront[0] = new Point(masPointFront[0].X, newPoint.Y);
                        masPointFront[4] = masPointFront[0];
                        masRectsFront[0] = new Rectangle(masPointFront[0].X - 2, masPointFront[0].Y - 2, 4, 4);

                        masPointTop[3] = new Point(masPointTop[3].X, Form1.panelWidth - newPoint.X);
                        masRectsTop[3] = new Rectangle(masPointTop[3].X - 2, masPointTop[3].Y - 2, 4, 4);
                        break;

                    case 5: masPointRight[6] = newPoint;
                        masRectsRight[5] = new Rectangle(masPointRight[6].X - 2, masPointRight[6].Y - 2, 4, 4);

                        masPointFront[5] = new Point(masPointFront[5].X, newPoint.Y);
                        masPointFront[9] = masPointFront[5];
                        masRectsFront[4] = new Rectangle(masPointFront[5].X - 2, masPointFront[5].Y - 2, 4, 4);

                        masPointTop[0] = new Point(masPointTop[0].X, Form1.panelWidth - newPoint.X);
                        masPointTop[4] = masPointTop[0];
                        masRectsTop[0] = new Rectangle(masPointTop[0].X - 2, masPointTop[0].Y - 2, 4, 4);
                        break;

                    case 6: masPointRight[7] = newPoint;
                        masRectsRight[6] = new Rectangle(masPointRight[7].X - 2, masPointRight[7].Y - 2, 4, 4);

                        masPointFront[8] = new Point(masPointFront[8].X, newPoint.Y);
                        masRectsFront[7] = new Rectangle(masPointFront[8].X - 2, masPointFront[8].Y - 2, 4, 4);

                        masPointTop[5] = new Point(masPointTop[5].X, Form1.panelWidth - newPoint.X);
                        masPointTop[9] = masPointTop[5];
                        masRectsTop[4] = new Rectangle(masPointTop[5].X - 2, masPointTop[5].Y - 2, 4, 4);

                        break;

                    case 7: masPointRight[8] = newPoint;
                        masRectsRight[7] = new Rectangle(masPointRight[8].X - 2, masPointRight[8].Y - 2, 4, 4);

                        masPointFront[3] = new Point(masPointFront[3].X, newPoint.Y);
                        masRectsFront[3] = new Rectangle(masPointFront[3].X - 2, masPointFront[3].Y - 2, 4, 4);

                        masPointTop[8] = new Point(masPointTop[8].X, Form1.panelWidth - newPoint.X);
                        masRectsTop[7] = new Rectangle(masPointTop[8].X - 2, masPointTop[8].Y - 2, 4, 4);
                        break;
                }
            }
            #endregion
            #region TopSide
            if (nameWindow == "Top")
            {
                switch (indexOfSelectRect)
                {
                    case 0: masPointTop[0] = newPoint;
                        masPointTop[4] = newPoint;
                        masRectsTop[0] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointFront[5] = new Point(newPoint.X, masPointFront[5].Y);
                        masPointFront[9] = masPointFront[5];
                        masRectsFront[4] = new Rectangle(masPointFront[5].X - 2, masPointFront[5].Y - 2, 4, 4);

                        masPointRight[6] = new Point(Form1.panelHeight - newPoint.Y, masPointRight[6].Y);
                        masRectsRight[5] = new Rectangle(masPointRight[6].X - 2, masPointRight[6].Y - 2, 4, 4);
                        break;

                    case 1: masPointTop[1] = newPoint;
                        masRectsTop[1] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointFront[6] = new Point(newPoint.X, masPointFront[6].Y);
                        masRectsFront[5] = new Rectangle(masPointFront[6].X - 2, masPointFront[6].Y - 2, 4, 4);

                        masPointRight[1] = new Point(Form1.panelHeight - newPoint.Y, masPointRight[1].Y);
                        masRectsRight[1] = new Rectangle(masPointRight[1].X - 2, masPointRight[1].Y - 2, 4, 4);
                        break;

                    case 2: masPointTop[2] = newPoint;
                        masRectsTop[2] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointFront[1] = new Point(newPoint.X, masPointFront[1].Y);
                        masRectsFront[1] = new Rectangle(masPointFront[1].X - 2, masPointFront[1].Y - 2, 4, 4);

                        masPointRight[0] = new Point(Form1.panelHeight - newPoint.Y, masPointRight[0].Y);
                        masPointRight[4] = masPointRight[0];
                        masRectsRight[0] = new Rectangle(masPointRight[0].X - 2, masPointRight[0].Y - 2, 4, 4);
                        break;

                    case 3: masPointTop[3] = newPoint;
                        masRectsTop[3] = new Rectangle(newPoint.X - 2, newPoint.Y - 2, 4, 4);

                        masPointFront[0] = new Point(newPoint.X, masPointFront[0].Y);
                        masPointFront[4] = masPointFront[0];
                        masRectsFront[0] = new Rectangle(masPointFront[0].X - 2, masPointFront[0].Y - 2, 4, 4);

                        masPointRight[5] = new Point(Form1.panelHeight - newPoint.Y, masPointRight[5].Y);
                        masPointRight[9] = masPointRight[5];
                        masRectsRight[4] = new Rectangle(masPointRight[5].X - 2, masPointRight[5].Y - 2, 4, 4);
                        break;

                    case 4: masPointTop[5] = newPoint;
                        masPointTop[9] = newPoint;
                        masRectsTop[4] = new Rectangle(masPointTop[5].X - 2, masPointTop[5].Y - 2, 4, 4);

                        masPointFront[8] = new Point(newPoint.X, masPointFront[8].Y);
                        masRectsFront[7] = new Rectangle(masPointFront[8].X - 2, masPointFront[8].Y - 2, 4, 4);

                        masPointRight[7] = new Point(Form1.panelHeight - newPoint.Y, masPointRight[7].Y);
                        masRectsRight[6] = new Rectangle(masPointRight[7].X - 2, masPointRight[7].Y - 2, 4, 4);
                        break;

                    case 5: masPointTop[6] = newPoint;
                        masRectsTop[5] = new Rectangle(masPointTop[6].X - 2, masPointTop[6].Y - 2, 4, 4);

                        masPointFront[7] = new Point(newPoint.X, masPointFront[7].Y);
                        masRectsFront[6] = new Rectangle(masPointFront[7].X - 2, masPointFront[7].Y - 2, 4, 4);

                        masPointRight[2] = new Point(Form1.panelHeight - newPoint.Y, masPointRight[2].Y);
                        masRectsRight[2] = new Rectangle(masPointRight[2].X - 2, masPointRight[2].Y - 2, 4, 4);
                        break;

                    case 6: masPointTop[7] = newPoint;
                        masRectsTop[6] = new Rectangle(masPointTop[7].X - 2, masPointTop[7].Y - 2, 4, 4);

                        masPointFront[2] = new Point(newPoint.X, masPointFront[2].Y);
                        masRectsFront[2] = new Rectangle(masPointFront[2].X - 2, masPointFront[2].Y - 2, 4, 4);

                        masPointRight[3] = new Point(Form1.panelHeight - newPoint.Y, masPointRight[3].Y);
                        masRectsRight[3] = new Rectangle(masPointRight[3].X - 2, masPointRight[3].Y - 2, 4, 4);
                        break;

                    case 7: masPointTop[8] = newPoint;
                        masRectsTop[7] = new Rectangle(masPointTop[8].X - 2, masPointTop[8].Y - 2, 4, 4);

                        masPointFront[3] = new Point(newPoint.X, masPointFront[3].Y);
                        masRectsFront[3] = new Rectangle(masPointFront[3].X - 2, masPointFront[3].Y - 2, 4, 4);

                        masPointRight[8] = new Point(Form1.panelHeight - newPoint.Y, masPointRight[8].Y);
                        masRectsRight[7] = new Rectangle(masPointRight[8].X - 2, masPointRight[8].Y - 2, 4, 4);
                        break;
                }
            }
            #endregion
        }

        #endregion
    }
}