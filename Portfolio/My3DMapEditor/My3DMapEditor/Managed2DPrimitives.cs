using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;

namespace My3DMapEditor
{
    class Managed2DPrimitives:IDisposable
    {
        private RectPrimitive currentPrimitive = null;
        private RectPrimitive copyPrimitive = null;
        private RectPrimitive convertedPrimitive = null;

        private Player redPlayer = null;
        private Player bluePlayer = null;

        private Hashtable list2DPrimitives = new Hashtable();
        private int indexOfSelectedPrimitive = -1;
        public SimpleSelection simpSel = null;
        public LinesSelection lineSel = null;
        public int key = 0;

        public Managed2DPrimitives(SimpleSelection ss,LinesSelection ls)
        {
            simpSel = ss;
            lineSel = ls;
        }
		~Managed2DPrimitives()
		{
			Dispose();
		}
		public void Dispose()
		{
			list2DPrimitives = null;
			if(currentPrimitive != null)
				currentPrimitive.Dispose();
			if (copyPrimitive != null)
				copyPrimitive.Dispose();
			if (convertedPrimitive != null)
				convertedPrimitive.Dispose();
			if(bluePlayer != null)
				bluePlayer.Dispose();
			if(redPlayer != null)
				redPlayer.Dispose();
			if(simpSel != null)
				simpSel.Dispose();
			if(lineSel != null)
				lineSel.Dispose();

			GC.SuppressFinalize(this);
        }
        #region Properties

        public int IndexOfSelectedPrimitive
        {
            get { return indexOfSelectedPrimitive; }
            set { indexOfSelectedPrimitive = value; }
        }
        /// <summary>
        /// Get list2DPrimitives
        /// </summary>
        public Hashtable List2DPrimitives
        {
            get { return list2DPrimitives; }
        }
        /// <summary>
        /// Get or set currentPrimitive
        /// </summary>
        public RectPrimitive CurrentPrimitive
        {
            get { return currentPrimitive; }
            set { currentPrimitive = value; }
        }
        public RectPrimitive CopyPrimitive
        {
            get { return copyPrimitive; }
            set { copyPrimitive = value; }
        }
        public RectPrimitive ConvertedPrimitive
        {
            get { return convertedPrimitive; }
            set { convertedPrimitive = value; }
        }
        public Player RedPlayer
        {
            get { return redPlayer; }
            set { redPlayer = value; }
        }
        public Player BluePlayer
        {
            get { return bluePlayer; }
            set { bluePlayer = value; }
        }

        #endregion

        public void RemooveRectPrimitive()
        {
            CurrentPrimitive = null;
        }
        public int AddCurrentPrimitiveToList()
        {
            int k = GetNextKey();
            list2DPrimitives.Add(k, currentPrimitive);
            RemooveRectPrimitive();
            return k;
        }
        public int AddPrimitiveToList(RectPrimitive r)
        {
            int k = GetNextKey();
            list2DPrimitives.Add(k , r);
            return k;
        }
        public int GetNextKey()
        {
            key++;
            return key;
        }
        /// <summary>
        /// Функция создания прямоугольного примитива в проекции Front
        /// </summary>
        /// <param name="p_begin">начальная точка</param>
        /// <param name="p_end">конечная точка</param>
        public void CreateRectFront(Point p_begin, Point p_end, int startedDeepOfPrimitive)
        {
            Point tmpP = new Point(0, 0);
            Size tmpS = new Size(0, 0);

            if (p_end.X >= p_begin.X)
            {
                tmpP.X = p_begin.X;
                tmpS.Width = p_end.X - p_begin.X;
            }
            if (p_begin.X > p_end.X)
            {
                tmpP.X = p_end.X;
                tmpS.Width = p_begin.X - p_end.X;
            }

            if (p_end.Y >= p_begin.Y)
            {
                tmpP.Y = p_begin.Y;
                tmpS.Height = p_end.Y - p_begin.Y;
            }
            if (p_begin.Y > p_end.Y)
            {
                tmpP.Y = p_end.Y; tmpS.Height = p_begin.Y - p_end.Y;
            }

            if (currentPrimitive != null)
            {
                #region ProjectionFront

                #region LeftTop

                currentPrimitive.MasLinesFront[0].X = tmpP.X;
                currentPrimitive.MasLinesFront[4].X = tmpP.X;
                currentPrimitive.MasLinesFront[5].X = tmpP.X;
                currentPrimitive.MasLinesFront[9].X = tmpP.X;

                currentPrimitive.MasLinesFront[0].Y = tmpP.Y;
                currentPrimitive.MasLinesFront[4].Y = tmpP.Y;
                currentPrimitive.MasLinesFront[5].Y = tmpP.Y;
                currentPrimitive.MasLinesFront[9].Y = tmpP.Y;

                #endregion

                #region RightTop

                currentPrimitive.MasLinesFront[1].X = tmpP.X + tmpS.Width;
                currentPrimitive.MasLinesFront[6].X = tmpP.X + tmpS.Width;

                currentPrimitive.MasLinesFront[1].Y = tmpP.Y;
                currentPrimitive.MasLinesFront[6].Y = tmpP.Y;

                #endregion

                #region RightBottom

                currentPrimitive.MasLinesFront[2].X = tmpP.X + tmpS.Width;
                currentPrimitive.MasLinesFront[7].X = tmpP.X + tmpS.Width;

                currentPrimitive.MasLinesFront[2].Y = tmpP.Y + tmpS.Height;
                currentPrimitive.MasLinesFront[7].Y = tmpP.Y + tmpS.Height;

                #endregion

                #region leftBottom

                currentPrimitive.MasLinesFront[3].X = tmpP.X;
                currentPrimitive.MasLinesFront[8].X = tmpP.X;

                currentPrimitive.MasLinesFront[3].Y = tmpP.Y + tmpS.Height;
                currentPrimitive.MasLinesFront[8].Y = tmpP.Y + tmpS.Height;

                #endregion

                #endregion

                #region ProjectionRight

                #region LeftTop

                currentPrimitive.MasLinesRight[0].X = Form1.panelWidth / 2;
                currentPrimitive.MasLinesRight[4].X = Form1.panelWidth / 2;
                currentPrimitive.MasLinesRight[5].X = Form1.panelWidth / 2;
                currentPrimitive.MasLinesRight[9].X = Form1.panelWidth / 2;

                currentPrimitive.MasLinesRight[0].Y = tmpP.Y;
                currentPrimitive.MasLinesRight[4].Y = tmpP.Y;
                currentPrimitive.MasLinesRight[5].Y = tmpP.Y;
                currentPrimitive.MasLinesRight[9].Y = tmpP.Y;

                #endregion

                #region RightTop

                currentPrimitive.MasLinesRight[1].X = currentPrimitive.MasLinesRight[0].X + startedDeepOfPrimitive;
                currentPrimitive.MasLinesRight[6].X = currentPrimitive.MasLinesRight[5].X + startedDeepOfPrimitive;

                currentPrimitive.MasLinesRight[1].Y = tmpP.Y;
                currentPrimitive.MasLinesRight[6].Y = tmpP.Y;

                #endregion

                #region RightBottom

                currentPrimitive.MasLinesRight[2].X = currentPrimitive.MasLinesRight[1].X;
                currentPrimitive.MasLinesRight[7].X = currentPrimitive.MasLinesRight[6].X;

                currentPrimitive.MasLinesRight[2].Y = currentPrimitive.MasLinesRight[1].Y + tmpS.Height;
                currentPrimitive.MasLinesRight[7].Y = currentPrimitive.MasLinesRight[6].Y + tmpS.Height;

                #endregion

                #region leftBottom

                currentPrimitive.MasLinesRight[3].X = currentPrimitive.MasLinesRight[0].X;
                currentPrimitive.MasLinesRight[8].X = currentPrimitive.MasLinesRight[5].X;

                currentPrimitive.MasLinesRight[3].Y = currentPrimitive.MasLinesRight[0].Y + tmpS.Height;
                currentPrimitive.MasLinesRight[8].Y = currentPrimitive.MasLinesRight[5].Y + tmpS.Height;

                #endregion

                #endregion

                #region ProjectionTop

                #region LeftTop

                currentPrimitive.MasLinesTop[0].X = tmpP.X;
                currentPrimitive.MasLinesTop[4].X = tmpP.X;
                currentPrimitive.MasLinesTop[5].X = tmpP.X;
                currentPrimitive.MasLinesTop[9].X = tmpP.X;

                currentPrimitive.MasLinesTop[0].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;
                currentPrimitive.MasLinesTop[4].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;
                currentPrimitive.MasLinesTop[5].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;
                currentPrimitive.MasLinesTop[9].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;

                #endregion

                #region RightTop

                currentPrimitive.MasLinesTop[1].X = currentPrimitive.MasLinesTop[0].X + tmpS.Width;
                currentPrimitive.MasLinesTop[6].X = currentPrimitive.MasLinesTop[5].X + tmpS.Width;

                currentPrimitive.MasLinesTop[1].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;
                currentPrimitive.MasLinesTop[6].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;

                #endregion

                #region RightBottom

                currentPrimitive.MasLinesTop[2].X = currentPrimitive.MasLinesTop[1].X;
                currentPrimitive.MasLinesTop[7].X = currentPrimitive.MasLinesTop[6].X;

                currentPrimitive.MasLinesTop[2].Y = Form1.panelHeight / 2;
                currentPrimitive.MasLinesTop[7].Y = Form1.panelHeight / 2;

                #endregion

                #region leftBottom

                currentPrimitive.MasLinesTop[3].X = currentPrimitive.MasLinesTop[0].X;
                currentPrimitive.MasLinesTop[8].X = currentPrimitive.MasLinesTop[5].X;

                currentPrimitive.MasLinesTop[3].Y = Form1.panelHeight / 2;
                currentPrimitive.MasLinesTop[8].Y = Form1.panelHeight / 2;

                #endregion

                #endregion

                //currentPrimitive.RandColorPrimitive = GenerateRandomColor();

                //currentPrimitive.RectOfX = new Rectangle(tmpP, tmpS);
                //currentPrimitive.RectOfY = new Rectangle(500, tmpP.Y, startedDeepOfPrimitive, tmpS.Height);
                //currentPrimitive.RectOfZ = new Rectangle(tmpP.X, 500 - startedDeepOfPrimitive, tmpS.Width, startedDeepOfPrimitive);
            }
        }
        /// <summary>
        /// Функция создания прямоугольного примитива в проекции Right
        /// </summary>
        /// <param name="p_begin">начальная точка</param>
        /// <param name="p_end">конечная точка</param>
        public void CreateRectRight(Point p_begin, Point p_end, int startedDeepOfPrimitive)
        {
            Point tmpP = new Point(0, 0);
            Size tmpS = new Size(0, 0);

            if (p_end.X >= p_begin.X)
            {
                tmpP.X = p_begin.X;
                tmpS.Width = p_end.X - p_begin.X;
            }
            if (p_begin.X > p_end.X)
            {
                tmpP.X = p_end.X;
                tmpS.Width = p_begin.X - p_end.X;
            }

            if (p_end.Y >= p_begin.Y)
            {
                tmpP.Y = p_begin.Y;
                tmpS.Height = p_end.Y - p_begin.Y;
            }
            if (p_begin.Y > p_end.Y)
            {
                tmpP.Y = p_end.Y; tmpS.Height = p_begin.Y - p_end.Y;
            }
            if (currentPrimitive != null)
            {
                #region ProjectionRight

                #region LeftTop

                currentPrimitive.MasLinesRight[0].X = tmpP.X;
                currentPrimitive.MasLinesRight[4].X = tmpP.X;
                currentPrimitive.MasLinesRight[5].X = tmpP.X;
                currentPrimitive.MasLinesRight[9].X = tmpP.X;

                currentPrimitive.MasLinesRight[0].Y = tmpP.Y;
                currentPrimitive.MasLinesRight[4].Y = tmpP.Y;
                currentPrimitive.MasLinesRight[5].Y = tmpP.Y;
                currentPrimitive.MasLinesRight[9].Y = tmpP.Y;

                #endregion

                #region RightTop

                currentPrimitive.MasLinesRight[1].X = tmpP.X + tmpS.Width;
                currentPrimitive.MasLinesRight[6].X = tmpP.X + tmpS.Width;

                currentPrimitive.MasLinesRight[1].Y = tmpP.Y;
                currentPrimitive.MasLinesRight[6].Y = tmpP.Y;

                #endregion

                #region RightBottom

                currentPrimitive.MasLinesRight[2].X = tmpP.X + tmpS.Width;
                currentPrimitive.MasLinesRight[7].X = tmpP.X + tmpS.Width;

                currentPrimitive.MasLinesRight[2].Y = tmpP.Y + tmpS.Height;
                currentPrimitive.MasLinesRight[7].Y = tmpP.Y + tmpS.Height;

                #endregion

                #region leftBottom

                currentPrimitive.MasLinesRight[3].X = tmpP.X;
                currentPrimitive.MasLinesRight[8].X = tmpP.X;

                currentPrimitive.MasLinesRight[3].Y = tmpP.Y + tmpS.Height;
                currentPrimitive.MasLinesRight[8].Y = tmpP.Y + tmpS.Height;

                #endregion

                #endregion

                #region ProjectionFront

                #region LeftTop

                currentPrimitive.MasLinesFront[0].X = Form1.panelWidth / 2;
                currentPrimitive.MasLinesFront[4].X = Form1.panelWidth / 2; 
                currentPrimitive.MasLinesFront[5].X = Form1.panelWidth / 2;
                currentPrimitive.MasLinesFront[9].X = Form1.panelWidth / 2;

                currentPrimitive.MasLinesFront[0].Y = tmpP.Y;
                currentPrimitive.MasLinesFront[4].Y = tmpP.Y;
                currentPrimitive.MasLinesFront[5].Y = tmpP.Y;
                currentPrimitive.MasLinesFront[9].Y = tmpP.Y;

                #endregion

                #region RightTop

                currentPrimitive.MasLinesFront[1].X = currentPrimitive.MasLinesFront[0].X + startedDeepOfPrimitive;
                currentPrimitive.MasLinesFront[6].X = currentPrimitive.MasLinesFront[5].X + startedDeepOfPrimitive;

                currentPrimitive.MasLinesFront[1].Y = tmpP.Y;
                currentPrimitive.MasLinesFront[6].Y = tmpP.Y;

                #endregion

                #region RightBottom

                currentPrimitive.MasLinesFront[2].X = currentPrimitive.MasLinesFront[1].X;
                currentPrimitive.MasLinesFront[7].X = currentPrimitive.MasLinesFront[6].X;

                currentPrimitive.MasLinesFront[2].Y = currentPrimitive.MasLinesFront[1].Y + tmpS.Height;
                currentPrimitive.MasLinesFront[7].Y = currentPrimitive.MasLinesFront[6].Y + tmpS.Height;

                #endregion

                #region leftBottom

                currentPrimitive.MasLinesFront[3].X = currentPrimitive.MasLinesFront[0].X;
                currentPrimitive.MasLinesFront[8].X = currentPrimitive.MasLinesFront[5].X;

                currentPrimitive.MasLinesFront[3].Y = currentPrimitive.MasLinesFront[0].Y + tmpS.Height;
                currentPrimitive.MasLinesFront[8].Y = currentPrimitive.MasLinesFront[5].Y + tmpS.Height;

                #endregion

                #endregion

                #region ProjectionTop

                #region LeftTop

                currentPrimitive.MasLinesTop[0].X = Form1.panelWidth / 2; ;
                currentPrimitive.MasLinesTop[4].X = Form1.panelWidth / 2; ;
                currentPrimitive.MasLinesTop[5].X = Form1.panelWidth / 2; ;
                currentPrimitive.MasLinesTop[9].X = Form1.panelWidth / 2; ;

                currentPrimitive.MasLinesTop[0].Y = Form1.panelHeight - currentPrimitive.MasLinesRight[6].X;
                currentPrimitive.MasLinesTop[4].Y = Form1.panelHeight - currentPrimitive.MasLinesRight[6].X;
                currentPrimitive.MasLinesTop[5].Y = Form1.panelHeight - currentPrimitive.MasLinesRight[6].X;
                currentPrimitive.MasLinesTop[9].Y = Form1.panelHeight - currentPrimitive.MasLinesRight[6].X;

                #endregion

                #region RightTop

                currentPrimitive.MasLinesTop[1].X = currentPrimitive.MasLinesTop[0].X + startedDeepOfPrimitive;
                currentPrimitive.MasLinesTop[6].X = currentPrimitive.MasLinesTop[5].X + startedDeepOfPrimitive;

                currentPrimitive.MasLinesTop[1].Y = currentPrimitive.MasLinesTop[0].Y;
                currentPrimitive.MasLinesTop[6].Y = currentPrimitive.MasLinesTop[5].Y;

                #endregion

                #region RightBottom

                currentPrimitive.MasLinesTop[2].X = currentPrimitive.MasLinesTop[1].X;
                currentPrimitive.MasLinesTop[7].X = currentPrimitive.MasLinesTop[6].X;

                currentPrimitive.MasLinesTop[2].Y = Form1.panelHeight - currentPrimitive.MasLinesRight[0].X;
                currentPrimitive.MasLinesTop[7].Y = Form1.panelHeight - currentPrimitive.MasLinesRight[5].X;

                #endregion

                #region leftBottom

                currentPrimitive.MasLinesTop[3].X = currentPrimitive.MasLinesTop[0].X;
                currentPrimitive.MasLinesTop[8].X = currentPrimitive.MasLinesTop[5].X;

                currentPrimitive.MasLinesTop[3].Y = Form1.panelHeight - currentPrimitive.MasLinesRight[0].X;
                currentPrimitive.MasLinesTop[8].Y = Form1.panelHeight - currentPrimitive.MasLinesRight[5].X;

                #endregion

                #endregion

                //currentPrimitive.RandColorPrimitive = GenerateRandomColor();

                //currentPrimitive.RectOfY = new Rectangle(tmpP, tmpS);
                //currentPrimitive.RectOfX = new Rectangle(500, tmpP.Y, startedDeepOfPrimitive, tmpS.Height);
                //currentPrimitive.RectOfZ = new Rectangle(currentPrimitive.RectOfX_X, 1000 - (tmpP.X + tmpS.Width), currentPrimitive.RectOfX_Width, tmpS.Width);
            }
        }
        /// <summary>
        /// Функция создания прямоугольного примитива в проекции Top
        /// </summary>
        /// <param name="p_begin">начальная точка</param>
        /// <param name="p_end">конечная точка</param>
        public void CreateRectTop(Point p_begin, Point p_end, int startedDeepOfPrimitive)
        {
            Point tmpP = new Point(0, 0);
            Size tmpS = new Size(0, 0);

            if (p_end.X >= p_begin.X)
            {
                tmpP.X = p_begin.X;
                tmpS.Width = p_end.X - p_begin.X;
            }
            if (p_begin.X > p_end.X)
            {
                tmpP.X = p_end.X;
                tmpS.Width = p_begin.X - p_end.X;
            }

            if (p_end.Y >= p_begin.Y)
            {
                tmpP.Y = p_begin.Y;
                tmpS.Height = p_end.Y - p_begin.Y;
            }
            if (p_begin.Y > p_end.Y)
            {
                tmpP.Y = p_end.Y; 
				tmpS.Height = p_begin.Y - p_end.Y;
            }
            if (currentPrimitive != null)
            {
                #region ProjectionTop

                #region LeftTop

                currentPrimitive.MasLinesTop[0].X = tmpP.X;
                currentPrimitive.MasLinesTop[4].X = tmpP.X;
                currentPrimitive.MasLinesTop[5].X = tmpP.X;
                currentPrimitive.MasLinesTop[9].X = tmpP.X;

                currentPrimitive.MasLinesTop[0].Y = tmpP.Y;
                currentPrimitive.MasLinesTop[4].Y = tmpP.Y;
                currentPrimitive.MasLinesTop[5].Y = tmpP.Y;
                currentPrimitive.MasLinesTop[9].Y = tmpP.Y;

                #endregion

                #region RightTop

                currentPrimitive.MasLinesTop[1].X = tmpP.X + tmpS.Width;
                currentPrimitive.MasLinesTop[6].X = tmpP.X + tmpS.Width;

                currentPrimitive.MasLinesTop[1].Y = tmpP.Y;
                currentPrimitive.MasLinesTop[6].Y = tmpP.Y;

                #endregion

                #region RightBottom

                currentPrimitive.MasLinesTop[2].X = tmpP.X + tmpS.Width;
                currentPrimitive.MasLinesTop[7].X = tmpP.X + tmpS.Width;

                currentPrimitive.MasLinesTop[2].Y = tmpP.Y + tmpS.Height;
                currentPrimitive.MasLinesTop[7].Y = tmpP.Y + tmpS.Height;

                #endregion

                #region leftBottom

                currentPrimitive.MasLinesTop[3].X = tmpP.X;
                currentPrimitive.MasLinesTop[8].X = tmpP.X;

                currentPrimitive.MasLinesTop[3].Y = tmpP.Y + tmpS.Height;
                currentPrimitive.MasLinesTop[8].Y = tmpP.Y + tmpS.Height;

                #endregion

                #endregion

                #region ProjectionRight

                #region LeftTop


				currentPrimitive.MasLinesRight[0].X = Form1.panelWidth - (tmpP.Y + tmpS.Height);
				currentPrimitive.MasLinesRight[4].X = Form1.panelWidth - (tmpP.Y + tmpS.Height);
				currentPrimitive.MasLinesRight[5].X = Form1.panelWidth - (tmpP.Y + tmpS.Height);
				currentPrimitive.MasLinesRight[9].X = Form1.panelWidth - (tmpP.Y + tmpS.Height);

				currentPrimitive.MasLinesRight[0].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;
				currentPrimitive.MasLinesRight[4].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;
				currentPrimitive.MasLinesRight[5].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;
				currentPrimitive.MasLinesRight[9].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;


				//currentPrimitive.MasLinesRight[0].X = Form1.rightHScrollValue + (Form1.sizeOfRightPanel.Width / 2) - tmpS.Height;// -(tmpP.Y + tmpS.Height);
				//currentPrimitive.MasLinesRight[4].X = Form1.rightHScrollValue + (Form1.sizeOfRightPanel.Width / 2) - tmpS.Height;// -(tmpP.Y + tmpS.Height);
				//currentPrimitive.MasLinesRight[5].X = Form1.rightHScrollValue + (Form1.sizeOfRightPanel.Width / 2) - tmpS.Height;// - (tmpP.Y + tmpS.Height);
				//currentPrimitive.MasLinesRight[9].X = Form1.rightHScrollValue + (Form1.sizeOfRightPanel.Width / 2) - tmpS.Height;// - (tmpP.Y + tmpS.Height);

				//currentPrimitive.MasLinesRight[0].Y = (Form1.rightVScrollValue + (Form1.sizeOfRightPanel.Height/ 2) ) - startedDeepOfPrimitive;
				//currentPrimitive.MasLinesRight[4].Y = (Form1.rightVScrollValue + (Form1.sizeOfRightPanel.Height/ 2) ) - startedDeepOfPrimitive;
				//currentPrimitive.MasLinesRight[5].Y = (Form1.rightVScrollValue + (Form1.sizeOfRightPanel.Height/ 2) ) - startedDeepOfPrimitive;
				//currentPrimitive.MasLinesRight[9].Y = (Form1.rightVScrollValue + (Form1.sizeOfRightPanel.Height/ 2) ) - startedDeepOfPrimitive;

                #endregion

                #region RightTop

				currentPrimitive.MasLinesRight[1].X = currentPrimitive.MasLinesRight[0].X + tmpS.Height;
				currentPrimitive.MasLinesRight[6].X = currentPrimitive.MasLinesRight[5].X + tmpS.Height;

				currentPrimitive.MasLinesRight[1].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;
				currentPrimitive.MasLinesRight[6].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;

				//currentPrimitive.MasLinesRight[1].X = currentPrimitive.MasLinesRight[0].X + tmpS.Height;
				//currentPrimitive.MasLinesRight[6].X = currentPrimitive.MasLinesRight[5].X + tmpS.Height;

				//currentPrimitive.MasLinesRight[1].Y = (Form1.rightVScrollValue + (Form1.sizeOfRightPanel.Height / 2)) - startedDeepOfPrimitive;
				//currentPrimitive.MasLinesRight[6].Y = (Form1.rightVScrollValue + (Form1.sizeOfRightPanel.Height / 2)) - startedDeepOfPrimitive;

                #endregion

                #region RightBottom

				currentPrimitive.MasLinesRight[2].X = currentPrimitive.MasLinesRight[1].X;
				currentPrimitive.MasLinesRight[7].X = currentPrimitive.MasLinesRight[6].X;

				currentPrimitive.MasLinesRight[2].Y = Form1.panelHeight / 2;
				currentPrimitive.MasLinesRight[7].Y = Form1.panelHeight / 2;

				//currentPrimitive.MasLinesRight[2].X = currentPrimitive.MasLinesRight[1].X;
				//currentPrimitive.MasLinesRight[7].X = currentPrimitive.MasLinesRight[6].X;

				//currentPrimitive.MasLinesRight[2].Y = (Form1.rightVScrollValue + (Form1.sizeOfRightPanel.Height / 2));
				//currentPrimitive.MasLinesRight[7].Y = (Form1.rightVScrollValue + (Form1.sizeOfRightPanel.Height / 2));

                #endregion

                #region leftBottom

				currentPrimitive.MasLinesRight[3].X = currentPrimitive.MasLinesRight[0].X;
				currentPrimitive.MasLinesRight[8].X = currentPrimitive.MasLinesRight[5].X;

				currentPrimitive.MasLinesRight[3].Y = Form1.panelHeight / 2;
				currentPrimitive.MasLinesRight[8].Y = Form1.panelHeight / 2;


				//currentPrimitive.MasLinesRight[3].X = currentPrimitive.MasLinesRight[0].X;
				//currentPrimitive.MasLinesRight[8].X = currentPrimitive.MasLinesRight[5].X;

				//currentPrimitive.MasLinesRight[3].Y = (Form1.rightVScrollValue + (Form1.sizeOfRightPanel.Height / 2));
				//currentPrimitive.MasLinesRight[8].Y = (Form1.rightVScrollValue + (Form1.sizeOfRightPanel.Height / 2));

                #endregion

                #endregion

                #region ProjectionFront

                #region LeftTop

                currentPrimitive.MasLinesFront[0].X = tmpP.X;
                currentPrimitive.MasLinesFront[4].X = tmpP.X;
                currentPrimitive.MasLinesFront[5].X = tmpP.X;
                currentPrimitive.MasLinesFront[9].X = tmpP.X;

                currentPrimitive.MasLinesFront[0].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;
                currentPrimitive.MasLinesFront[4].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;
                currentPrimitive.MasLinesFront[5].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;
                currentPrimitive.MasLinesFront[9].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;

                #endregion

                #region RightTop

                currentPrimitive.MasLinesFront[1].X = currentPrimitive.MasLinesFront[0].X + tmpS.Width;
                currentPrimitive.MasLinesFront[6].X = currentPrimitive.MasLinesFront[5].X + tmpS.Width;

                currentPrimitive.MasLinesFront[1].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;
                currentPrimitive.MasLinesFront[6].Y = Form1.panelHeight / 2 - startedDeepOfPrimitive;

                #endregion

                #region RightBottom

                currentPrimitive.MasLinesFront[2].X = currentPrimitive.MasLinesFront[1].X;
                currentPrimitive.MasLinesFront[7].X = currentPrimitive.MasLinesFront[6].X;

                currentPrimitive.MasLinesFront[2].Y = Form1.panelHeight / 2;
                currentPrimitive.MasLinesFront[7].Y = Form1.panelHeight / 2;

                #endregion

                #region leftBottom

                currentPrimitive.MasLinesFront[3].X = currentPrimitive.MasLinesFront[0].X;
                currentPrimitive.MasLinesFront[8].X = currentPrimitive.MasLinesFront[5].X;

                currentPrimitive.MasLinesFront[3].Y = Form1.panelHeight / 2;
                currentPrimitive.MasLinesFront[8].Y = Form1.panelHeight / 2;

                #endregion

                #endregion

                //currentPrimitive.RandColorPrimitive = GenerateRandomColor();

                //currentPrimitive.RectOfZ = new Rectangle(tmpP, tmpS);
                //currentPrimitive.RectOfX = new Rectangle(tmpP.X, 500 - startedDeepOfPrimitive, tmpS.Width, startedDeepOfPrimitive);
                //currentPrimitive.RectOfY = new Rectangle(1000 - (tmpP.Y + tmpS.Height), 500 - startedDeepOfPrimitive, tmpS.Height, startedDeepOfPrimitive);
            }
        }

        public void DrawAllPrimitives(Graphics g,string nameProjection)
        {
            Pen redPen = new Pen(Color.Red);
            Pen bluePen = new Pen(Color.Aqua);

            if (list2DPrimitives.Count != 0)
            {
                foreach (int key in list2DPrimitives.Keys)
                {
                    RectPrimitive rp = (RectPrimitive)list2DPrimitives[key];
                    if (!rp.IsSelected)
                    {
                        if (nameProjection == "Front")
                        {
                            Pen p = new Pen(rp.RandColorPrimitive);

                            g.DrawLine(p, rp.MasLinesFront[5], rp.MasLinesFront[6]);
                            g.DrawLine(p, rp.MasLinesFront[6], rp.MasLinesFront[7]);
                            g.DrawLine(p, rp.MasLinesFront[7], rp.MasLinesFront[8]);
                            g.DrawLine(p, rp.MasLinesFront[8], rp.MasLinesFront[9]);

                            g.DrawLine(p, rp.MasLinesFront[0], rp.MasLinesFront[1]);
                            g.DrawLine(p, rp.MasLinesFront[1], rp.MasLinesFront[2]);
                            g.DrawLine(p, rp.MasLinesFront[2], rp.MasLinesFront[3]);
                            g.DrawLine(p, rp.MasLinesFront[3], rp.MasLinesFront[4]);

                            g.DrawLine(p, rp.MasLinesFront[0], rp.MasLinesFront[5]);
                            g.DrawLine(p, rp.MasLinesFront[1], rp.MasLinesFront[6]);
                            g.DrawLine(p, rp.MasLinesFront[2], rp.MasLinesFront[7]);
                            g.DrawLine(p, rp.MasLinesFront[3], rp.MasLinesFront[8]);
                        }
                        if (nameProjection == "Right")
                        {
                            Pen p = new Pen(rp.RandColorPrimitive);

                            g.DrawLine(p, rp.MasLinesRight[5], rp.MasLinesRight[6]);
                            g.DrawLine(p, rp.MasLinesRight[6], rp.MasLinesRight[7]);
                            g.DrawLine(p, rp.MasLinesRight[7], rp.MasLinesRight[8]);
                            g.DrawLine(p, rp.MasLinesRight[8], rp.MasLinesRight[9]);

                            g.DrawLine(p, rp.MasLinesRight[0], rp.MasLinesRight[1]);
                            g.DrawLine(p, rp.MasLinesRight[1], rp.MasLinesRight[2]);
                            g.DrawLine(p, rp.MasLinesRight[2], rp.MasLinesRight[3]);
                            g.DrawLine(p, rp.MasLinesRight[3], rp.MasLinesRight[4]);

                            g.DrawLine(p, rp.MasLinesRight[0], rp.MasLinesRight[5]);
                            g.DrawLine(p, rp.MasLinesRight[1], rp.MasLinesRight[6]);
                            g.DrawLine(p, rp.MasLinesRight[2], rp.MasLinesRight[7]);
                            g.DrawLine(p, rp.MasLinesRight[3], rp.MasLinesRight[8]);
                        }
                        if (nameProjection == "Top")
                        {
                            Pen p = new Pen(rp.RandColorPrimitive);

                            g.DrawLine(p, rp.MasLinesTop[5], rp.MasLinesTop[6]);
                            g.DrawLine(p, rp.MasLinesTop[6], rp.MasLinesTop[7]);
                            g.DrawLine(p, rp.MasLinesTop[7], rp.MasLinesTop[8]);
                            g.DrawLine(p, rp.MasLinesTop[8], rp.MasLinesTop[9]);

                            g.DrawLine(p, rp.MasLinesTop[0], rp.MasLinesTop[1]);
                            g.DrawLine(p, rp.MasLinesTop[1], rp.MasLinesTop[2]);
                            g.DrawLine(p, rp.MasLinesTop[2], rp.MasLinesTop[3]);
                            g.DrawLine(p, rp.MasLinesTop[3], rp.MasLinesTop[4]);

                            g.DrawLine(p, rp.MasLinesTop[0], rp.MasLinesTop[5]);
                            g.DrawLine(p, rp.MasLinesTop[1], rp.MasLinesTop[6]);
                            g.DrawLine(p, rp.MasLinesTop[2], rp.MasLinesTop[7]);
                            g.DrawLine(p, rp.MasLinesTop[3], rp.MasLinesTop[8]);
                        }
                    }
                }
            }
            if (redPlayer != null)
            {
                if (!redPlayer.isPlayerSelected)
                {
                    redPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

                    if (nameProjection == "Front")
                        g.DrawRectangle(redPen, RedPlayer.rectFront);

                    if (nameProjection == "Right")
                        g.DrawRectangle(redPen, RedPlayer.rectRight);

                    if (nameProjection == "Top")
                        g.DrawRectangle(redPen, RedPlayer.rectTop);
                }
            }
            if (bluePlayer != null)
            {
                if (!bluePlayer.isPlayerSelected)
                {
                    bluePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;

                    if (nameProjection == "Front")
                        g.DrawRectangle(bluePen, BluePlayer.rectFront);

                    if (nameProjection == "Right")
                        g.DrawRectangle(bluePen, BluePlayer.rectRight);

                    if (nameProjection == "Top")
                        g.DrawRectangle(bluePen, BluePlayer.rectTop);
                }
            }
			if (redPlayer != null)
			{
				if (redPlayer.isPlayerSelected)
				{
					redPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

					if (nameProjection == "Front")
						g.DrawRectangle(redPen, RedPlayer.rectFront);

					if (nameProjection == "Right")
						g.DrawRectangle(redPen, RedPlayer.rectRight);

					if (nameProjection == "Top")
						g.DrawRectangle(redPen, RedPlayer.rectTop);
				}
			}
			if (bluePlayer != null)
			{
				if (bluePlayer.isPlayerSelected)
				{
					bluePen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

					if (nameProjection == "Front")
						g.DrawRectangle(bluePen, BluePlayer.rectFront);

					if (nameProjection == "Right")
						g.DrawRectangle(bluePen, BluePlayer.rectRight);

					if (nameProjection == "Top")
						g.DrawRectangle(bluePen, BluePlayer.rectTop);
				}
			}
        }
        public LinesSelection DrawSelectedPrimitive_TransformMode(Graphics g,string nameProjection,LinesSelection sel)
        {
            if (currentPrimitive != null)
            {
                LinesSelection sel1 = sel;
                if(sel1 == null)
                    sel1 = new LinesSelection(currentPrimitive); 
                Pen p = new Pen(Color.Red);
                p.Width = 1;
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                p.DashCap = System.Drawing.Drawing2D.DashCap.Flat;

                Pen p_solid = new Pen(Color.Red);
                p_solid.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                p_solid.Width = 1;

                Brush br = new SolidBrush(Color.Cyan);

                if (nameProjection == "Front")
                {

                    g.DrawLine(p, sel1.MasPointFront[5], sel1.MasPointFront[6]);
                    g.DrawLine(p, sel1.MasPointFront[6], sel1.MasPointFront[7]);
                    g.DrawLine(p, sel1.MasPointFront[7], sel1.MasPointFront[8]);
                    g.DrawLine(p, sel1.MasPointFront[8], sel1.MasPointFront[9]);

                    g.DrawLine(p_solid, sel1.MasPointFront[0], sel1.MasPointFront[1]);
                    g.DrawLine(p_solid, sel1.MasPointFront[1], sel1.MasPointFront[2]);
                    g.DrawLine(p_solid, sel1.MasPointFront[2], sel1.MasPointFront[3]);
                    g.DrawLine(p_solid, sel1.MasPointFront[3], sel1.MasPointFront[4]);

                    g.DrawLine(p, sel1.MasPointFront[0], sel1.MasPointFront[5]);
                    g.DrawLine(p, sel1.MasPointFront[1], sel1.MasPointFront[6]);
                    g.DrawLine(p, sel1.MasPointFront[2], sel1.MasPointFront[7]);
                    g.DrawLine(p, sel1.MasPointFront[3], sel1.MasPointFront[8]);

                    g.DrawRectangles(new Pen(Color.Cyan), sel1.MasRectsFront);
                    
                    if(sel1.IndexOfSelectRect != -1)
                        g.FillRectangle(br,sel1.MasRectsFront[sel1.IndexOfSelectRect]);
                }
                if (nameProjection == "Right")
                {
                    g.DrawLine(p, sel1.MasPointRight[5], sel1.MasPointRight[6]);
                    g.DrawLine(p, sel1.MasPointRight[6], sel1.MasPointRight[7]);
                    g.DrawLine(p, sel1.MasPointRight[7], sel1.MasPointRight[8]);
                    g.DrawLine(p, sel1.MasPointRight[8], sel1.MasPointRight[9]);

                    g.DrawLine(p_solid, sel1.MasPointRight[0], sel1.MasPointRight[1]);
                    g.DrawLine(p_solid, sel1.MasPointRight[1], sel1.MasPointRight[2]);
                    g.DrawLine(p_solid, sel1.MasPointRight[2], sel1.MasPointRight[3]);
                    g.DrawLine(p_solid, sel1.MasPointRight[3], sel1.MasPointRight[4]);

                    g.DrawLine(p, sel1.MasPointRight[0], sel1.MasPointRight[5]);
                    g.DrawLine(p, sel1.MasPointRight[1], sel1.MasPointRight[6]);
                    g.DrawLine(p, sel1.MasPointRight[2], sel1.MasPointRight[7]);
                    g.DrawLine(p, sel1.MasPointRight[3], sel1.MasPointRight[8]);

                    g.DrawRectangles(new Pen(Color.Cyan), sel1.MasRectsRight);
                    if (sel1.IndexOfSelectRect != -1)
                        g.FillRectangle(br, sel1.MasRectsRight[sel1.IndexOfSelectRect]);
                }
                if (nameProjection == "Top")
                {
                    g.DrawLines(p, sel1.MasPointTop);

                    g.DrawLine(p, sel1.MasPointTop[5], sel1.MasPointTop[6]);
                    g.DrawLine(p, sel1.MasPointTop[6], sel1.MasPointTop[7]);
                    g.DrawLine(p, sel1.MasPointTop[7], sel1.MasPointTop[8]);
                    g.DrawLine(p, sel1.MasPointTop[8], sel1.MasPointTop[9]);

                    g.DrawLine(p_solid, sel1.MasPointTop[0], sel1.MasPointTop[1]);
                    g.DrawLine(p_solid, sel1.MasPointTop[1], sel1.MasPointTop[2]);
                    g.DrawLine(p_solid, sel1.MasPointTop[2], sel1.MasPointTop[3]);
                    g.DrawLine(p_solid, sel1.MasPointTop[3], sel1.MasPointTop[4]);

                    g.DrawLine(p, sel1.MasPointTop[0], sel1.MasPointTop[5]);
                    g.DrawLine(p, sel1.MasPointTop[1], sel1.MasPointTop[6]);
                    g.DrawLine(p, sel1.MasPointTop[2], sel1.MasPointTop[7]);
                    g.DrawLine(p, sel1.MasPointTop[3], sel1.MasPointTop[8]);

                    g.DrawRectangles(new Pen(Color.Cyan), sel1.MasRectsTop);
                    if (sel1.IndexOfSelectRect != -1)
                        g.FillRectangle(br, sel1.MasRectsTop[sel1.IndexOfSelectRect]);
                }
                return sel1;
            }
            else
            {
                LinesSelection sel1 = sel;

                Pen p = new Pen(Color.Red);
                p.Width = 1;
                p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                p.DashCap = System.Drawing.Drawing2D.DashCap.Flat;

                Pen p_solid = new Pen(Color.Red);
                p_solid.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                p_solid.Width = 1;

                Brush br = new SolidBrush(Color.Cyan);
                
                if (list2DPrimitives.Count != 0)
                {
                    foreach (int key in list2DPrimitives.Keys)
                    {
                        RectPrimitive rp = (RectPrimitive)list2DPrimitives[key];
                        if (rp.IsSelected)
                        {
                            if (sel1 == null)
                                sel1 = new LinesSelection(rp);
                            else
                            {
                                if (nameProjection == "Front")
                                {
                                    g.DrawLine(p, sel1.MasPointFront[5], sel1.MasPointFront[6]);
                                    g.DrawLine(p, sel1.MasPointFront[6], sel1.MasPointFront[7]);
                                    g.DrawLine(p, sel1.MasPointFront[7], sel1.MasPointFront[8]);
                                    g.DrawLine(p, sel1.MasPointFront[8], sel1.MasPointFront[9]);

                                    g.DrawLine(p_solid, sel1.MasPointFront[0], sel1.MasPointFront[1]);
                                    g.DrawLine(p_solid, sel1.MasPointFront[1], sel1.MasPointFront[2]);
                                    g.DrawLine(p_solid, sel1.MasPointFront[2], sel1.MasPointFront[3]);
                                    g.DrawLine(p_solid, sel1.MasPointFront[3], sel1.MasPointFront[4]);

                                    g.DrawLine(p, sel1.MasPointFront[0], sel1.MasPointFront[5]);
                                    g.DrawLine(p, sel1.MasPointFront[1], sel1.MasPointFront[6]);
                                    g.DrawLine(p, sel1.MasPointFront[2], sel1.MasPointFront[7]);
                                    g.DrawLine(p, sel1.MasPointFront[3], sel1.MasPointFront[8]);

                                    g.DrawRectangles(new Pen(Color.Cyan), sel1.MasRectsFront);

                                    if (sel1.IndexOfSelectRect != -1)
                                        g.FillRectangle(br, sel1.MasRectsFront[sel1.IndexOfSelectRect]);
                                }
                                if (nameProjection == "Right")
                                {
                                    g.DrawLine(p, sel1.MasPointRight[5], sel1.MasPointRight[6]);
                                    g.DrawLine(p, sel1.MasPointRight[6], sel1.MasPointRight[7]);
                                    g.DrawLine(p, sel1.MasPointRight[7], sel1.MasPointRight[8]);
                                    g.DrawLine(p, sel1.MasPointRight[8], sel1.MasPointRight[9]);

                                    g.DrawLine(p_solid, sel1.MasPointRight[0], sel1.MasPointRight[1]);
                                    g.DrawLine(p_solid, sel1.MasPointRight[1], sel1.MasPointRight[2]);
                                    g.DrawLine(p_solid, sel1.MasPointRight[2], sel1.MasPointRight[3]);
                                    g.DrawLine(p_solid, sel1.MasPointRight[3], sel1.MasPointRight[4]);

                                    g.DrawLine(p, sel1.MasPointRight[0], sel1.MasPointRight[5]);
                                    g.DrawLine(p, sel1.MasPointRight[1], sel1.MasPointRight[6]);
                                    g.DrawLine(p, sel1.MasPointRight[2], sel1.MasPointRight[7]);
                                    g.DrawLine(p, sel1.MasPointRight[3], sel1.MasPointRight[8]);

                                    g.DrawRectangles(new Pen(Color.Cyan), sel1.MasRectsRight);
                                    if (sel1.IndexOfSelectRect != -1)
                                        g.FillRectangle(br, sel1.MasRectsRight[sel1.IndexOfSelectRect]);
                                }
                                if (nameProjection == "Top")
                                {
                                    g.DrawLines(p, sel1.MasPointTop);

                                    g.DrawLine(p, sel1.MasPointTop[5], sel1.MasPointTop[6]);
                                    g.DrawLine(p, sel1.MasPointTop[6], sel1.MasPointTop[7]);
                                    g.DrawLine(p, sel1.MasPointTop[7], sel1.MasPointTop[8]);
                                    g.DrawLine(p, sel1.MasPointTop[8], sel1.MasPointTop[9]);

                                    g.DrawLine(p_solid, sel1.MasPointTop[0], sel1.MasPointTop[1]);
                                    g.DrawLine(p_solid, sel1.MasPointTop[1], sel1.MasPointTop[2]);
                                    g.DrawLine(p_solid, sel1.MasPointTop[2], sel1.MasPointTop[3]);
                                    g.DrawLine(p_solid, sel1.MasPointTop[3], sel1.MasPointTop[4]);

                                    g.DrawLine(p, sel1.MasPointTop[0], sel1.MasPointTop[5]);
                                    g.DrawLine(p, sel1.MasPointTop[1], sel1.MasPointTop[6]);
                                    g.DrawLine(p, sel1.MasPointTop[2], sel1.MasPointTop[7]);
                                    g.DrawLine(p, sel1.MasPointTop[3], sel1.MasPointTop[8]);

                                    g.DrawRectangles(new Pen(Color.Cyan), sel1.MasRectsTop);
                                    if (sel1.IndexOfSelectRect != -1)
                                        g.FillRectangle(br, sel1.MasRectsTop[sel1.IndexOfSelectRect]);
                                }
                            }
                        }
                    }
                    return sel1;
                }
            }
            return null;
        }
        public void DrawSelectedPrimitive_SimpleMode(Graphics g, string nameProjection,out SimpleSelection sel)
        {
            Pen p = new Pen(Color.Red);
            p.Width = 1;
            p.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            p.DashCap = System.Drawing.Drawing2D.DashCap.Flat;

            

            Brush br = new SolidBrush(Color.Cyan);

            if (currentPrimitive != null)
            {
                Pen p_solid = new Pen(currentPrimitive.RandColorPrimitive);
                p_solid.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                p_solid.Width = 1;

                Pen p_dash = new Pen(currentPrimitive.RandColorPrimitive);
                p_dash.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                p_dash.Width = 1;

                SimpleSelection sel1 = new SimpleSelection(currentPrimitive); 
 
                if (nameProjection == "Front")
                {
                    g.DrawLine(p_dash, currentPrimitive.MasLinesFront[5], currentPrimitive.MasLinesFront[6]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesFront[6], currentPrimitive.MasLinesFront[7]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesFront[7], currentPrimitive.MasLinesFront[8]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesFront[8], currentPrimitive.MasLinesFront[9]);

                    g.DrawLine(p_solid, currentPrimitive.MasLinesFront[0], currentPrimitive.MasLinesFront[1]);
                    g.DrawLine(p_solid, currentPrimitive.MasLinesFront[1], currentPrimitive.MasLinesFront[2]);
                    g.DrawLine(p_solid, currentPrimitive.MasLinesFront[2], currentPrimitive.MasLinesFront[3]);
                    g.DrawLine(p_solid, currentPrimitive.MasLinesFront[3], currentPrimitive.MasLinesFront[4]);

                    g.DrawLine(p_dash, currentPrimitive.MasLinesFront[0], currentPrimitive.MasLinesFront[5]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesFront[1], currentPrimitive.MasLinesFront[6]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesFront[2], currentPrimitive.MasLinesFront[7]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesFront[3], currentPrimitive.MasLinesFront[8]);

                    g.DrawRectangle(p, sel1.RectSelFront);
                    g.DrawRectangles(new Pen(Color.Cyan), sel1.MasRectOfSelFront);
                }
                if (nameProjection == "Right")
                {
                    g.DrawLine(p_dash, currentPrimitive.MasLinesRight[5], currentPrimitive.MasLinesRight[6]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesRight[6], currentPrimitive.MasLinesRight[7]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesRight[7], currentPrimitive.MasLinesRight[8]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesRight[8], currentPrimitive.MasLinesRight[9]);

                    g.DrawLine(p_solid, currentPrimitive.MasLinesRight[0], currentPrimitive.MasLinesRight[1]);
                    g.DrawLine(p_solid, currentPrimitive.MasLinesRight[1], currentPrimitive.MasLinesRight[2]);
                    g.DrawLine(p_solid, currentPrimitive.MasLinesRight[2], currentPrimitive.MasLinesRight[3]);
                    g.DrawLine(p_solid, currentPrimitive.MasLinesRight[3], currentPrimitive.MasLinesRight[4]);

                    g.DrawLine(p_dash, currentPrimitive.MasLinesRight[0], currentPrimitive.MasLinesRight[5]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesRight[1], currentPrimitive.MasLinesRight[6]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesRight[2], currentPrimitive.MasLinesRight[7]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesRight[3], currentPrimitive.MasLinesRight[8]);

                    g.DrawRectangle(p, sel1.RectSelRight);
                    g.DrawRectangles(new Pen(Color.Cyan), sel1.MasRectOfSelRight);
                }
                if (nameProjection == "Top")
                {
                    g.DrawLine(p_dash, currentPrimitive.MasLinesTop[5], currentPrimitive.MasLinesTop[6]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesTop[6], currentPrimitive.MasLinesTop[7]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesTop[7], currentPrimitive.MasLinesTop[8]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesTop[8], currentPrimitive.MasLinesTop[9]);

                    g.DrawLine(p_solid, currentPrimitive.MasLinesTop[0], currentPrimitive.MasLinesTop[1]);
                    g.DrawLine(p_solid, currentPrimitive.MasLinesTop[1], currentPrimitive.MasLinesTop[2]);
                    g.DrawLine(p_solid, currentPrimitive.MasLinesTop[2], currentPrimitive.MasLinesTop[3]);
                    g.DrawLine(p_solid, currentPrimitive.MasLinesTop[3], currentPrimitive.MasLinesTop[4]);

                    g.DrawLine(p_dash, currentPrimitive.MasLinesTop[0], currentPrimitive.MasLinesTop[5]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesTop[1], currentPrimitive.MasLinesTop[6]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesTop[2], currentPrimitive.MasLinesTop[7]);
                    g.DrawLine(p_dash, currentPrimitive.MasLinesTop[3], currentPrimitive.MasLinesTop[8]);

                    g.DrawRectangle(p, sel1.RectSelTop);
                    g.DrawRectangles(new Pen(Color.Cyan), sel1.MasRectOfSelTop);
                }
                sel = sel1;
                return;
            }
            else
            {
                if (list2DPrimitives.Count != 0)
                {
                    foreach (int key in list2DPrimitives.Keys)
                    {
                        RectPrimitive rp = (RectPrimitive)list2DPrimitives[key];
                        if (rp.IsSelected)
                        {
                            Pen p_solid = new Pen(rp.RandColorPrimitive);
                            p_solid.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
                            p_solid.Width = 1;

                            Pen p_dash = new Pen(rp.RandColorPrimitive);
                            p_dash.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
                            p_dash.Width = 1;

                            SimpleSelection sel1 = new SimpleSelection(rp); 

                            if (nameProjection == "Front")
                            {
                                g.DrawLine(p_dash, rp.MasLinesFront[5], rp.MasLinesFront[6]);
                                g.DrawLine(p_dash, rp.MasLinesFront[6], rp.MasLinesFront[7]);
                                g.DrawLine(p_dash, rp.MasLinesFront[7], rp.MasLinesFront[8]);
                                g.DrawLine(p_dash, rp.MasLinesFront[8], rp.MasLinesFront[9]);

                                g.DrawLine(p_solid, rp.MasLinesFront[0], rp.MasLinesFront[1]);
                                g.DrawLine(p_solid, rp.MasLinesFront[1], rp.MasLinesFront[2]);
                                g.DrawLine(p_solid, rp.MasLinesFront[2], rp.MasLinesFront[3]);
                                g.DrawLine(p_solid, rp.MasLinesFront[3], rp.MasLinesFront[4]);

                                g.DrawLine(p_dash, rp.MasLinesFront[0], rp.MasLinesFront[5]);
                                g.DrawLine(p_dash, rp.MasLinesFront[1], rp.MasLinesFront[6]);
                                g.DrawLine(p_dash, rp.MasLinesFront[2], rp.MasLinesFront[7]);
                                g.DrawLine(p_dash, rp.MasLinesFront[3], rp.MasLinesFront[8]);

                                g.DrawRectangle(p, sel1.RectSelFront);
                                g.DrawRectangles(new Pen(Color.Cyan), sel1.MasRectOfSelFront);
                            }
                            if (nameProjection == "Right")
                            {
                                g.DrawLine(p_dash, rp.MasLinesRight[5], rp.MasLinesRight[6]);
                                g.DrawLine(p_dash, rp.MasLinesRight[6], rp.MasLinesRight[7]);
                                g.DrawLine(p_dash, rp.MasLinesRight[7], rp.MasLinesRight[8]);
                                g.DrawLine(p_dash, rp.MasLinesRight[8], rp.MasLinesRight[9]);

                                g.DrawLine(p_solid, rp.MasLinesRight[0], rp.MasLinesRight[1]);
                                g.DrawLine(p_solid, rp.MasLinesRight[1], rp.MasLinesRight[2]);
                                g.DrawLine(p_solid, rp.MasLinesRight[2], rp.MasLinesRight[3]);
                                g.DrawLine(p_solid, rp.MasLinesRight[3], rp.MasLinesRight[4]);

                                g.DrawLine(p_dash, rp.MasLinesRight[0], rp.MasLinesRight[5]);
                                g.DrawLine(p_dash, rp.MasLinesRight[1], rp.MasLinesRight[6]);
                                g.DrawLine(p_dash, rp.MasLinesRight[2], rp.MasLinesRight[7]);
                                g.DrawLine(p_dash, rp.MasLinesRight[3], rp.MasLinesRight[8]);

                                g.DrawRectangle(p, sel1.RectSelRight);
                                g.DrawRectangles(new Pen(Color.Cyan), sel1.MasRectOfSelRight);
                            }
                            if (nameProjection == "Top")
                            {
                                g.DrawLine(p_dash, rp.MasLinesTop[5], rp.MasLinesTop[6]);
                                g.DrawLine(p_dash, rp.MasLinesTop[6], rp.MasLinesTop[7]);
                                g.DrawLine(p_dash, rp.MasLinesTop[7], rp.MasLinesTop[8]);
                                g.DrawLine(p_dash, rp.MasLinesTop[8], rp.MasLinesTop[9]);

                                g.DrawLine(p_solid, rp.MasLinesTop[0], rp.MasLinesTop[1]);
                                g.DrawLine(p_solid, rp.MasLinesTop[1], rp.MasLinesTop[2]);
                                g.DrawLine(p_solid, rp.MasLinesTop[2], rp.MasLinesTop[3]);
                                g.DrawLine(p_solid, rp.MasLinesTop[3], rp.MasLinesTop[4]);

                                g.DrawLine(p_dash, rp.MasLinesTop[0], rp.MasLinesTop[5]);
                                g.DrawLine(p_dash, rp.MasLinesTop[1], rp.MasLinesTop[6]);
                                g.DrawLine(p_dash, rp.MasLinesTop[2], rp.MasLinesTop[7]);
                                g.DrawLine(p_dash, rp.MasLinesTop[3], rp.MasLinesTop[8]);

                                g.DrawRectangle(p, sel1.RectSelTop);
                                g.DrawRectangles(new Pen(Color.Cyan), sel1.MasRectOfSelTop);
                            }
                            sel = sel1;
                            return;
                        }
                    }
                }
            }
            sel = null;
            return;
        }
        public Color GenerateRandomColor()
        {
            Color col;
            Random rnd = new Random();
            int r = rnd.Next(60, 200);
            int g = rnd.Next(80, 255);
            int b = rnd.Next(100, 255);

            return col = Color.FromArgb(r, g, b);
        }
        public RectPrimitive GetSelectedPrimitive()
        {
            if (currentPrimitive != null)
                return currentPrimitive;
            else
            {
                if (list2DPrimitives.Count != 0)
                {
                    foreach (int key in list2DPrimitives.Keys)
                    {
                        RectPrimitive rp = (RectPrimitive)list2DPrimitives[key];
                        if (rp.IsSelected)
                            return rp;
                    }
                }
            }

            return null;
        }
        public void Clear()
        {
			if (list2DPrimitives != null)
			{
				foreach (int key in list2DPrimitives.Keys)
				{
					RectPrimitive rp = ((RectPrimitive)list2DPrimitives[key]);
					//list2DPrimitives.Remove(key);
					rp = null;
				}

				list2DPrimitives = null;
			}
            list2DPrimitives = new Hashtable();
			if (currentPrimitive != null)
				currentPrimitive = null;
			if (convertedPrimitive != null)
				convertedPrimitive = null;
			if (copyPrimitive != null)
				copyPrimitive = null;
			bluePlayer = null;
			redPlayer = null;
        }
    }
}
