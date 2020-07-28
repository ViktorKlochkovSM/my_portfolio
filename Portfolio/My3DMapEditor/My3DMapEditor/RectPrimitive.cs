using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace My3DMapEditor
{
    public class RectPrimitive:IDisposable
    {
        #region Members


        private Point[] linesTop = new Point[10];
        private Point[] linesRight = new Point[10];
        private Point[] linesFront = new Point[10];

        private bool isSelected = false;
        private bool isTransformed = false;

        private Color colorOfPrimitive;

        #endregion

        public RectPrimitive(Rectangle rX, Rectangle rY, Rectangle rZ, Color col)
        {
            colorOfPrimitive = col;

            SplitRectToLines("X", rX);
            SplitRectToLines("Y", rY);
            SplitRectToLines("Z", rZ);
        }
        public RectPrimitive(Point[] masLinesFront, Point[] masLinesRight, Point[] masLinesTop, Color col)
        {
            colorOfPrimitive = col;

            linesFront = masLinesFront;
            linesRight = masLinesRight;
            linesTop = masLinesTop;
        }
		public void Dispose()
		{
			linesTop = null;
			linesRight = null;
			linesFront = null;
		}
		~RectPrimitive()
		{
			Dispose();
			GC.SuppressFinalize(this);
		}

        #region Properties

        /// <summary>
        /// Get or set IsSelected if true - selected
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; }
        }
        /// <summary>
        /// Get цвет линий примитива в 3-х проекциях
        /// </summary>
        public Color RandColorPrimitive
        {
            get { return colorOfPrimitive; }
        }

        public bool IsTransformed
        {
            get { return isTransformed; }
            set { isTransformed = value; }
        }
        /// <summary>
        /// Get massive of LinesTop
        /// </summary>
        public Point[] MasLinesTop
        {
            get { return linesTop; }
            set { linesTop = value; }
        }
        /// <summary>
        /// Get massive of LinesRight
        /// </summary>
        public Point[] MasLinesRight
        {
            get { return linesRight; }
            set { linesRight = value; }
        }
        /// <summary>
        /// Get massive of LinesFront
        /// </summary>
        public Point[] MasLinesFront
        {
            get { return linesFront; }
            set { linesFront = value; }
        }




        #endregion

        public void SplitRectToLines(string nameRect, Rectangle rect)
        {
            if (nameRect == "X")
            {
                linesFront[0] = rect.Location;
                linesFront[1] = new Point(rect.Left + rect.Width, rect.Top);
                linesFront[2] = new Point(rect.Left + rect.Width, rect.Y + rect.Height);
                linesFront[3] = new Point(rect.Left, rect.Y + rect.Height);
                linesFront[4] = rect.Location;

                linesFront[5] = rect.Location;
                linesFront[6] = new Point(rect.Left + rect.Width, rect.Top);
                linesFront[7] = new Point(rect.Left + rect.Width, rect.Y + rect.Height);
                linesFront[8] = new Point(rect.Left, rect.Y + rect.Height);
                linesFront[9] = rect.Location;
            }
            if (nameRect == "Y")
            {
                linesRight[0] = rect.Location;
                linesRight[1] = new Point(rect.Left + rect.Width, rect.Top);
                linesRight[2] = new Point(rect.X + rect.Width, rect.Y + rect.Height);
                linesRight[3] = new Point(rect.X, rect.Y + rect.Height);
                linesRight[4] = rect.Location;

                linesRight[5] = rect.Location;
                linesRight[6] = new Point(rect.Left + rect.Width, rect.Top);
                linesRight[7] = new Point(rect.X + rect.Width, rect.Y + rect.Height);
                linesRight[8] = new Point(rect.X, rect.Y + rect.Height);
                linesRight[9] = rect.Location;
            }
            if (nameRect == "Z")
            {
                linesTop[0] = rect.Location;
                linesTop[1] = new Point(rect.Left + rect.Width, rect.Top);
                linesTop[2] = new Point(rect.X + rect.Width, rect.Y + rect.Height);
                linesTop[3] = new Point(rect.X, rect.Y + rect.Height);
                linesTop[4] = rect.Location;

                linesTop[5] = rect.Location;
                linesTop[6] = new Point(rect.Left + rect.Width, rect.Top);
                linesTop[7] = new Point(rect.X + rect.Width, rect.Y + rect.Height);
                linesTop[8] = new Point(rect.X, rect.Y + rect.Height);
                linesTop[9] = rect.Location;
            }
        }

		
    }
}
