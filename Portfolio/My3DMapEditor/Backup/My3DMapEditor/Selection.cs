using System;
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace My3DMapEditor
{
    public class SimpleSelection:IDisposable
    {
        private Rectangle rectSelFront = new Rectangle(0, 0, 0, 0);
        private Rectangle rectSelRight = new Rectangle(0, 0, 0, 0);
        private Rectangle rectSelTop = new Rectangle(0, 0, 0, 0);

        private Rectangle[] masRectOfSelFront = new Rectangle[8];
        private Rectangle[] masRectOfSelRight = new Rectangle[8];
        private Rectangle[] masRectOfSelTop = new Rectangle[8];

        public SimpleSelection(My3DMapEditor.RectPrimitive rp)
        {
            CreateSel(rp, "Front");
            CreateSel(rp, "Right");
            CreateSel(rp, "Top");
        }
		public void Dispose()
		{
			masRectOfSelFront = null;
			masRectOfSelRight = null;
			masRectOfSelTop = null;

			GC.SuppressFinalize(this);
		}
		~SimpleSelection()
		{
			Dispose();
		}

        #region Property

        public Rectangle RectSelFront
        {
            get { return rectSelFront; }
        }
        public Rectangle RectSelRight
        {
            get { return rectSelRight; }
        }
        public Rectangle RectSelTop
        {
            get { return rectSelTop; }
        }

        public Rectangle[] MasRectOfSelFront
        {
            get { return masRectOfSelFront; }
        }
        public Rectangle[] MasRectOfSelRight
        {
            get { return masRectOfSelRight; }
        }
        public Rectangle[] MasRectOfSelTop
        {
            get { return masRectOfSelTop; }
        }

        #endregion

        #region Functions

        private void CreateSel(RectPrimitive rp, string name)
        {
            if (rp != null)
            {
                if (name == "Front")
                {
                    int maxX = rp.MasLinesFront[0].X;
                    int minX = rp.MasLinesFront[0].X;
                    int maxY = rp.MasLinesFront[0].Y;
                    int minY = rp.MasLinesFront[0].Y;

                    for (int i = 0; i < 10; i++)
                    {
                        if (maxX < rp.MasLinesFront[i].X)
                            maxX = rp.MasLinesFront[i].X;

                        if (minX > rp.MasLinesFront[i].X)
                            minX = rp.MasLinesFront[i].X;

                        if (maxY < rp.MasLinesFront[i].Y)
                            maxY = rp.MasLinesFront[i].Y;

                        if (minY > rp.MasLinesFront[i].Y)
                            minY = rp.MasLinesFront[i].Y;
                    }

                    rectSelFront = Rectangle.FromLTRB(minX, minY, maxX, maxY);

                    masRectOfSelFront[0].X = rectSelFront.X - 2;
                    masRectOfSelFront[0].Y = rectSelFront.Y - 2;
                    masRectOfSelFront[0].Width = 4;
                    masRectOfSelFront[0].Height = 4;

                    masRectOfSelFront[1].X = rectSelFront.X + rectSelFront.Width / 2 - 2;
                    masRectOfSelFront[1].Y = rectSelFront.Y - 2;
                    masRectOfSelFront[1].Width = 4;
                    masRectOfSelFront[1].Height = 4;

                    masRectOfSelFront[2].X = rectSelFront.X + rectSelFront.Width - 2;
                    masRectOfSelFront[2].Y = rectSelFront.Y - 2;
                    masRectOfSelFront[2].Width = 4;
                    masRectOfSelFront[2].Height = 4;

                    masRectOfSelFront[3].X = rectSelFront.X + rectSelFront.Width - 2;
                    masRectOfSelFront[3].Y = rectSelFront.Y + rectSelFront.Height / 2 - 2;
                    masRectOfSelFront[3].Width = 4;
                    masRectOfSelFront[3].Height = 4;

                    masRectOfSelFront[4].X = rectSelFront.X + rectSelFront.Width - 2;
                    masRectOfSelFront[4].Y = rectSelFront.Y + rectSelFront.Height - 2;
                    masRectOfSelFront[4].Width = 4;
                    masRectOfSelFront[4].Height = 4;

                    masRectOfSelFront[5].X = rectSelFront.X + rectSelFront.Width / 2 - 2;
                    masRectOfSelFront[5].Y = rectSelFront.Y + rectSelFront.Height - 2;
                    masRectOfSelFront[5].Width = 4;
                    masRectOfSelFront[5].Height = 4;

                    masRectOfSelFront[6].X = rectSelFront.X - 2;
                    masRectOfSelFront[6].Y = rectSelFront.Y + rectSelFront.Height - 2;
                    masRectOfSelFront[6].Width = 4;
                    masRectOfSelFront[6].Height = 4;

                    masRectOfSelFront[7].X = rectSelFront.X - 2;
                    masRectOfSelFront[7].Y = rectSelFront.Y + rectSelFront.Height / 2 - 2;
                    masRectOfSelFront[7].Width = 4;
                    masRectOfSelFront[7].Height = 4;
                }
                if (name == "Right")
                {
                    int maxX = rp.MasLinesRight[0].X;
                    int minX = rp.MasLinesRight[0].X;
                    int maxY = rp.MasLinesRight[0].Y;
                    int minY = rp.MasLinesRight[0].Y;

                    for (int i = 0; i < 10; i++)
                    {
                        if (maxX < rp.MasLinesRight[i].X)
                            maxX = rp.MasLinesRight[i].X;

                        if (minX > rp.MasLinesRight[i].X)
                            minX = rp.MasLinesRight[i].X;

                        if (maxY < rp.MasLinesRight[i].Y)
                            maxY = rp.MasLinesRight[i].Y;

                        if (minY > rp.MasLinesRight[i].Y)
                            minY = rp.MasLinesRight[i].Y;
                    }

                    rectSelRight = Rectangle.FromLTRB(minX, minY, maxX, maxY);

                    masRectOfSelRight[0].X = rectSelRight.X - 2;
                    masRectOfSelRight[0].Y = rectSelRight.Y - 2;
                    masRectOfSelRight[0].Width = 4;
                    masRectOfSelRight[0].Height = 4;

                    masRectOfSelRight[1].X = rectSelRight.X + rectSelRight.Width / 2 - 2;
                    masRectOfSelRight[1].Y = rectSelRight.Y - 2;
                    masRectOfSelRight[1].Width = 4;
                    masRectOfSelRight[1].Height = 4;

                    masRectOfSelRight[2].X = rectSelRight.X + rectSelRight.Width - 2;
                    masRectOfSelRight[2].Y = rectSelRight.Y - 2;
                    masRectOfSelRight[2].Width = 4;
                    masRectOfSelRight[2].Height = 4;

                    masRectOfSelRight[3].X = rectSelRight.X + rectSelRight.Width - 2;
                    masRectOfSelRight[3].Y = rectSelRight.Y + rectSelRight.Height / 2 - 2;
                    masRectOfSelRight[3].Width = 4;
                    masRectOfSelRight[3].Height = 4;

                    masRectOfSelRight[4].X = rectSelRight.X + rectSelRight.Width - 2;
                    masRectOfSelRight[4].Y = rectSelRight.Y + rectSelRight.Height - 2;
                    masRectOfSelRight[4].Width = 4;
                    masRectOfSelRight[4].Height = 4;

                    masRectOfSelRight[5].X = rectSelRight.X + rectSelRight.Width / 2 - 2;
                    masRectOfSelRight[5].Y = rectSelRight.Y + rectSelRight.Height - 2;
                    masRectOfSelRight[5].Width = 4;
                    masRectOfSelRight[5].Height = 4;

                    masRectOfSelRight[6].X = rectSelRight.X - 2;
                    masRectOfSelRight[6].Y = rectSelRight.Y + rectSelRight.Height - 2;
                    masRectOfSelRight[6].Width = 4;
                    masRectOfSelRight[6].Height = 4;

                    masRectOfSelRight[7].X = rectSelRight.X - 2;
                    masRectOfSelRight[7].Y = rectSelRight.Y + rectSelRight.Height / 2 - 2;
                    masRectOfSelRight[7].Width = 4;
                    masRectOfSelRight[7].Height = 4;
                }
                if (name == "Top")
                {
                    int maxX = rp.MasLinesTop[0].X;
                    int minX = rp.MasLinesTop[0].X;
                    int maxY = rp.MasLinesTop[0].Y;
                    int minY = rp.MasLinesTop[0].Y;

                    for (int i = 0; i < 10; i++)
                    {
                        if (maxX < rp.MasLinesTop[i].X)
                            maxX = rp.MasLinesTop[i].X;

                        if (minX > rp.MasLinesTop[i].X)
                            minX = rp.MasLinesTop[i].X;

                        if (maxY < rp.MasLinesTop[i].Y)
                            maxY = rp.MasLinesTop[i].Y;

                        if (minY > rp.MasLinesTop[i].Y)
                            minY = rp.MasLinesTop[i].Y;
                    }

                    rectSelTop = Rectangle.FromLTRB(minX, minY, maxX, maxY);

                    masRectOfSelTop[0].X = rectSelTop.X - 2;
                    masRectOfSelTop[0].Y = rectSelTop.Y - 2;
                    masRectOfSelTop[0].Width = 4;
                    masRectOfSelTop[0].Height = 4;

                    masRectOfSelTop[1].X = rectSelTop.X + rectSelTop.Width / 2 - 2;
                    masRectOfSelTop[1].Y = rectSelTop.Y - 2;
                    masRectOfSelTop[1].Width = 4;
                    masRectOfSelTop[1].Height = 4;

                    masRectOfSelTop[2].X = rectSelTop.X + rectSelTop.Width - 2;
                    masRectOfSelTop[2].Y = rectSelTop.Y - 2;
                    masRectOfSelTop[2].Width = 4;
                    masRectOfSelTop[2].Height = 4;

                    masRectOfSelTop[3].X = rectSelTop.X + rectSelTop.Width - 2;
                    masRectOfSelTop[3].Y = rectSelTop.Y + rectSelTop.Height / 2 - 2;
                    masRectOfSelTop[3].Width = 4;
                    masRectOfSelTop[3].Height = 4;

                    masRectOfSelTop[4].X = rectSelTop.X + rectSelTop.Width - 2;
                    masRectOfSelTop[4].Y = rectSelTop.Y + rectSelTop.Height - 2;
                    masRectOfSelTop[4].Width = 4;
                    masRectOfSelTop[4].Height = 4;

                    masRectOfSelTop[5].X = rectSelTop.X + rectSelTop.Width / 2 - 2;
                    masRectOfSelTop[5].Y = rectSelTop.Y + rectSelTop.Height - 2;
                    masRectOfSelTop[5].Width = 4;
                    masRectOfSelTop[5].Height = 4;

                    masRectOfSelTop[6].X = rectSelTop.X - 2;
                    masRectOfSelTop[6].Y = rectSelTop.Y + rectSelTop.Height - 2;
                    masRectOfSelTop[6].Width = 4;
                    masRectOfSelTop[6].Height = 4;

                    masRectOfSelTop[7].X = rectSelTop.X - 2;
                    masRectOfSelTop[7].Y = rectSelTop.Y + rectSelTop.Height / 2 - 2;
                    masRectOfSelTop[7].Width = 4;
                    masRectOfSelTop[7].Height = 4;
                }
            }
        }

        /// <summary>
        /// Функция проверки наведения мыши на один из регионов выделения
        /// </summary>
        /// <param name="mouse"></param>
        /// <param name="s"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public System.Windows.Forms.Cursor isMouseUnderRegionSel(Point mouse, out string s, out Point p,string nameWindow)
        {
            s = "None";
            p = new Point(0, 0);

            if (nameWindow == "Front")
            {
                #region CheckRectRegions

                for (int i = 0; i < 8; i++)
                {
                    if (isMouseUnderIndexesRegion(i, mouse,nameWindow))
                    {
                        if (i == 0 || i == 4)
                        {
                            if (i == 0)
                            {
                                s = "LeftUp";
                                p = new Point(masRectOfSelFront[i].X, masRectOfSelFront[i].Y);
                            }
                            else
                            {
                                s = "RightDown";
                                p = new Point(masRectOfSelFront[i].X, masRectOfSelFront[i].Y);
                            }
                            return System.Windows.Forms.Cursors.SizeNWSE;
                        }
                        if (i == 1 || i == 5)
                        {
                            if (i == 1)
                            {
                                s = "CenterUp";
                                p = new Point(masRectOfSelFront[i].X, masRectOfSelFront[i].Y);
                            }
                            else
                            {
                                s = "CenterDown";
                                p = new Point(masRectOfSelFront[i].X, masRectOfSelFront[i].Y);
                            }
                            return System.Windows.Forms.Cursors.SizeNS;
                        }
                        if (i == 2 || i == 6)
                        {
                            if (i == 2)
                            {
                                s = "RightUp";
                                p = new Point(masRectOfSelFront[i].X, masRectOfSelFront[i].Y);
                            }
                            else
                            {
                                s = "LeftDown";
                                p = new Point(masRectOfSelFront[i].X, masRectOfSelFront[i].Y);
                            }
                            return System.Windows.Forms.Cursors.SizeNESW;
                        }
                        if (i == 3 || i == 7)
                        {
                            if (i == 3)
                            {
                                s = "CenterRight";
                                p = new Point(masRectOfSelFront[i].X, masRectOfSelFront[i].Y);
                            }
                            else
                            {
                                s = "CenterLeft";
                                p = new Point(masRectOfSelFront[i].X, masRectOfSelFront[i].Y);
                            }
                            return System.Windows.Forms.Cursors.SizeWE;
                        }
                    }
                }

                #endregion
            }
            if (nameWindow == "Right")
            {
                #region CheckRectRegions

                for (int i = 0; i < 8; i++)
                {
                    if (isMouseUnderIndexesRegion(i, mouse,nameWindow))
                    {
                        if (i == 0 || i == 4)
                        {
                            if (i == 0)
                            {
                                s = "LeftUp";
                                p = new Point(masRectOfSelRight[i].X, masRectOfSelRight[i].Y);
                            }
                            else
                            {
                                s = "RightDown";
                                p = new Point(masRectOfSelRight[i].X, masRectOfSelRight[i].Y);
                            }
                            return System.Windows.Forms.Cursors.SizeNWSE;
                        }
                        if (i == 1 || i == 5)
                        {
                            if (i == 1)
                            {
                                s = "CenterUp";
                                p = new Point(masRectOfSelRight[i].X, masRectOfSelRight[i].Y);
                            }
                            else
                            {
                                s = "CenterDown";
                                p = new Point(masRectOfSelRight[i].X, masRectOfSelRight[i].Y);
                            }
                            return System.Windows.Forms.Cursors.SizeNS;
                        }
                        if (i == 2 || i == 6)
                        {
                            if (i == 2)
                            {
                                s = "RightUp";
                                p = new Point(masRectOfSelRight[i].X, masRectOfSelRight[i].Y);
                            }
                            else
                            {
                                s = "LeftDown";
                                p = new Point(masRectOfSelRight[i].X, masRectOfSelRight[i].Y);
                            }
                            return System.Windows.Forms.Cursors.SizeNESW;
                        }
                        if (i == 3 || i == 7)
                        {
                            if (i == 3)
                            {
                                s = "CenterRight";
                                p = new Point(masRectOfSelRight[i].X, masRectOfSelRight[i].Y);
                            }
                            else
                            {
                                s = "CenterLeft";
                                p = new Point(masRectOfSelRight[i].X, masRectOfSelRight[i].Y);
                            }
                            return System.Windows.Forms.Cursors.SizeWE;
                        }
                    }
                }

                #endregion
            }
            if (nameWindow == "Top")
            {
                #region CheckRectRegions

                for (int i = 0; i < 8; i++)
                {
                    if (isMouseUnderIndexesRegion(i, mouse,nameWindow))
                    {
                        if (i == 0 || i == 4)
                        {
                            if (i == 0)
                            {
                                s = "LeftUp";
                                p = new Point(masRectOfSelTop[i].X, masRectOfSelTop[i].Y);
                            }
                            else
                            {
                                s = "RightDown";
                                p = new Point(masRectOfSelTop[i].X, masRectOfSelTop[i].Y);
                            }
                            return System.Windows.Forms.Cursors.SizeNWSE;
                        }
                        if (i == 1 || i == 5)
                        {
                            if (i == 1)
                            {
                                s = "CenterUp";
                                p = new Point(masRectOfSelTop[i].X, masRectOfSelTop[i].Y);
                            }
                            else
                            {
                                s = "CenterDown";
                                p = new Point(masRectOfSelTop[i].X, masRectOfSelTop[i].Y);
                            }
                            return System.Windows.Forms.Cursors.SizeNS;
                        }
                        if (i == 2 || i == 6)
                        {
                            if (i == 2)
                            {
                                s = "RightUp";
                                p = new Point(masRectOfSelTop[i].X, masRectOfSelTop[i].Y);
                            }
                            else
                            {
                                s = "LeftDown";
                                p = new Point(masRectOfSelTop[i].X, masRectOfSelTop[i].Y);
                            }
                            return System.Windows.Forms.Cursors.SizeNESW;
                        }
                        if (i == 3 || i == 7)
                        {
                            if (i == 3)
                            {
                                s = "CenterRight";
                                p = new Point(masRectOfSelTop[i].X, masRectOfSelTop[i].Y);
                            }
                            else
                            {
                                s = "CenterLeft";
                                p = new Point(masRectOfSelTop[i].X, masRectOfSelTop[i].Y);
                            }
                            return System.Windows.Forms.Cursors.SizeWE;
                        }
                    }
                }

                #endregion
            }

            return System.Windows.Forms.Cursors.Cross;
        }
        private bool isMouseUnderIndexesRegion(int index, Point m,string nameWindow)
        {
            if (nameWindow == "Front")
            {
                if (masRectOfSelFront[index].X <= m.X && masRectOfSelFront[index].Y <= m.Y && masRectOfSelFront[index].X + masRectOfSelFront[index].Width >= m.X && masRectOfSelFront[index].Y + masRectOfSelFront[index].Height >= m.Y)
                    return true;
            }
            if (nameWindow == "Right")
            {
                if (masRectOfSelRight[index].X <= m.X && masRectOfSelRight[index].Y <= m.Y && masRectOfSelRight[index].X + masRectOfSelRight[index].Width >= m.X && masRectOfSelRight[index].Y + masRectOfSelRight[index].Height >= m.Y)
                    return true;
            }
            if (nameWindow == "Top")
            {
                if (masRectOfSelTop[index].X <= m.X && masRectOfSelTop[index].Y <= m.Y && masRectOfSelTop[index].X + masRectOfSelTop[index].Width >= m.X && masRectOfSelTop[index].Y + masRectOfSelTop[index].Height >= m.Y)
                    return true;
            }
            return false;
        }
        public void RemoveSelection()
        {
            masRectOfSelFront = null;
            rectSelFront = new Rectangle(0, 0, 0, 0);
            masRectOfSelRight = null;
            rectSelRight = new Rectangle(0, 0, 0, 0);
            masRectOfSelTop = null;
            rectSelTop = new Rectangle(0, 0, 0, 0);
        }

        #endregion
    }
}
