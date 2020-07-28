using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace My3DMapEditor
{
    public delegate void SelectObjectDelegate(RectPrimitive rp,RectPrimitive converted);
    public delegate void Update3DPrimitiveCoordsDelegate(RectPrimitive convertedPr, int selIndex);

    public partial class Form1 : Form
    {
        #region Members

		public static int frontHScrollValue = 0;
		public static int frontVScrollValue = 0;
		public static int rightHScrollValue = 0;
		public static int rightVScrollValue = 0;
		public static int topHScrollValue = 0;
		public static int topVScrollValue = 0;

		public static Size sizeOfFrontPanel = new Size();
		public static Size sizeOfRightPanel = new Size();
		public static Size sizeOfTopPanel = new Size();

        public static string startingPath;

        public static event SelectObjectDelegate SelectObject;

        public delegate void myDelecateRedraw2D();

        public static event Update3DPrimitiveCoordsDelegate Update3DPrimitiveCoords;

        public event DelegatePanelScrolling EventPanelScrolling;
        /// <summary>
        /// сообщение любой из панелей о необходимости скролирования в ту или иную сторону
        /// </summary>
        /// <param name="sender">в нашем случае этот параметр будет представлять обЪект панель</param>
        /// <param name="isToNegateveSide">определяет направление движения скроллера (true)-> к нулю</param>
        /// <param name="isHorisontalScroll">оределяет какой из скроллеров должен быть проскроллирован</param>
        public delegate void DelegatePanelScrolling(object sender, bool isToNegateveSide, bool isHorisontalScroll);

        static public int panelWidth;
        static public int panelHeight;

        private BufferedGraphics bufGrTop;
        private BufferedGraphics bufGrRight;
        private BufferedGraphics bufGrFront;

        private BufferedGraphicsContext context;

        //private IAsyncResult asResultTop;
        //private IAsyncResult asResultRight;
        //private IAsyncResult asResultFront;

        private Point mousePositionDown;
        private Point mousePositionUp;
        private Point mouseCurrentPosition;

        private Point mousePositionDownSnap;
        private Point mousePositionUpSnap;
        private Point mouseCurrentPositionSnap;

		static public string lastTexture = "aaatrigger.bmp";

        private bool isMousePressed = false;

        private bool isSnapEnabled = true;

        private bool isRectEnabled = false;

        private bool isTransformSelectionEnabled = false;

        //private bool isMouseHoverFront = false;
        //private bool isMouseHoverRight = false;
        //private bool isMouseHoverTop = false;

        private PickObject pickObj = null;

        private Grid2D grid2D = null;

        private Managed3D managed3D = null;

        private LinesSelection lineSelection = null;

        private SimpleSelection selection = null;

        private Managed2DPrimitives managed2D = null;

        private TexturesSettings textureSettings = null;

        private string nameOfActionChanges = "None";

        private SolidBrush blackBrush = new SolidBrush(Color.Black);

        #endregion


		static public Size Box3D;


        public Form1()
        {
            InitializeComponent();

            startingPath = Environment.CurrentDirectory;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InitAllWindow();

            managed3D = new Managed3D(pictureBox3D.Handle, this);
            timer3D.Enabled = true;
			panel11.Focus();
        }
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			managed3D.device.Dispose();
		}
        private void InitAllWindow()
        {
            this.WindowState = FormWindowState.Maximized;

            this.CenterToScreen();

            panelWidth = 5000;
            panelHeight = 5000;

            int w = ((panelWidth/2)-(panelFront.Width/2));
            int h = ((panelHeight/2)-(panelFront.Height/2));

            panelFront.AutoScrollMinSize = new Size(panelWidth, panelHeight);
            panelFront.AutoScrollPosition = new Point(w, h);

            panelTop.AutoScrollMinSize = new Size(panelWidth, panelHeight);
            panelTop.AutoScrollPosition = new Point(w, h);

            panelRight.AutoScrollMinSize = new Size(panelWidth, panelHeight);
            panelRight.AutoScrollPosition = new Point(w, h);


			topHScrollValue = panelTop.HorizontalScroll.Value;
			topVScrollValue = panelTop.VerticalScroll.Value;
			frontHScrollValue = panelFront.HorizontalScroll.Value;
			frontVScrollValue = panelFront.VerticalScroll.Value;
			rightHScrollValue = panelRight.HorizontalScroll.Value;
			rightVScrollValue = panelRight.VerticalScroll.Value;


			sizeOfFrontPanel = panelFront.Size;
			sizeOfRightPanel = panelRight.Size;
			sizeOfTopPanel = panelTop.Size;



            context = BufferedGraphicsManager.Current;
            context.MaximumBuffer = new Size(panelWidth, panelHeight);



            bufGrTop = context.Allocate(pictureBoxTop.CreateGraphics(),
                 new Rectangle(0, 0, panelWidth, panelHeight));

            bufGrTop.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            bufGrRight = context.Allocate(pictureBoxRight.CreateGraphics(),
                 new Rectangle(0, 0, panelWidth, panelHeight));

            bufGrRight.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            bufGrFront = context.Allocate(pictureBoxFront.CreateGraphics(),
                 new Rectangle(0, 0, panelWidth, panelHeight));

            bufGrFront.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

            grid2D = new Grid2D();

			managed2D = new Managed2DPrimitives(selection, lineSelection);

            pickObj = new PickObject();

            int s = 4;
            for (int i = 0; i < grid2D.sizeGrid; i++, s *= 2) ;
            labelGridSize.Text = s.ToString();

            EventPanelScrolling += new DelegatePanelScrolling(Form1_EventPanelScrolling);

            textureSettings = new TexturesSettings();
            textureSettings.FormClosed += new FormClosedEventHandler(textureSettings_FormClosed);
            textureSettings.TextureFormKeyDown += new TextureFormKeyDownDelegate(textureSettings_TextureFormKeyDown);
			

			Managed3D.MouseClick3D += new MouseClick3DDelegate(Managed3D_MouseClick3D);
        }


        #region FunctionsDrawToBuffers
        private void DrawToBufferTop()
        {

            int hv = panelTop.HorizontalScroll.Value;
            int vv = panelTop.VerticalScroll.Value;

            bufGrTop.Graphics.FillRectangle(blackBrush, new Rectangle(hv - 50, vv - 50, panelTop.Width + 50, panelTop.Height + 50));

            grid2D.DrawGrid(bufGrTop.Graphics, hv, vv, panelTop.ClientRectangle);

            bufGrTop.Graphics.DrawString("Top", new System.Drawing.Font("Arial", 12), new SolidBrush(Color.Yellow), new PointF(panelTop.HorizontalScroll.Value + 10, panelTop.VerticalScroll.Value + 10));

            managed2D.DrawAllPrimitives(bufGrTop.Graphics,"Top");
            if (isTransformSelectionEnabled)
                lineSelection = managed2D.DrawSelectedPrimitive_TransformMode(bufGrTop.Graphics, "Top", lineSelection);
            else
            {
                managed2D.DrawSelectedPrimitive_SimpleMode(bufGrTop.Graphics, "Top", out selection);
            }

        }
        private void DrawToBufferRight()
        {
            int hv = panelRight.HorizontalScroll.Value;
            int vv = panelRight.VerticalScroll.Value;

            bufGrRight.Graphics.FillRectangle(blackBrush, new Rectangle(hv - 50, vv - 50, panelRight.Width + 50, panelRight.Height + 50));

            grid2D.DrawGrid(bufGrRight.Graphics, hv, vv, panelRight.ClientRectangle);

            bufGrRight.Graphics.DrawString("Right", new System.Drawing.Font("Arial", 12), new SolidBrush(Color.Yellow), new PointF(panelRight.HorizontalScroll.Value + 10, panelRight.VerticalScroll.Value + 10));

            managed2D.DrawAllPrimitives(bufGrRight.Graphics, "Right");
            if (isTransformSelectionEnabled)
                lineSelection = managed2D.DrawSelectedPrimitive_TransformMode(bufGrRight.Graphics, "Right",lineSelection);
            else
                managed2D.DrawSelectedPrimitive_SimpleMode(bufGrRight.Graphics, "Right", out selection);
        }
        private void DrawToBufferFront()
        {
            int hv = panelFront.HorizontalScroll.Value;
            int vv = panelFront.VerticalScroll.Value;

            bufGrFront.Graphics.FillRectangle(blackBrush, new Rectangle(hv - 50, vv - 50, panelFront.Width + 50, panelFront.Height + 50));

            grid2D.DrawGrid(bufGrFront.Graphics, hv, vv,panelFront.ClientRectangle);

            bufGrFront.Graphics.DrawString("Front", new System.Drawing.Font("Arial", 12), new SolidBrush(Color.Yellow), new PointF(panelFront.HorizontalScroll.Value + 10, panelFront.VerticalScroll.Value + 10));

            managed2D.DrawAllPrimitives(bufGrFront.Graphics, "Front");
            if (isTransformSelectionEnabled)
                lineSelection = managed2D.DrawSelectedPrimitive_TransformMode(bufGrFront.Graphics, "Front", lineSelection);
            else
                managed2D.DrawSelectedPrimitive_SimpleMode(bufGrFront.Graphics, "Front", out selection);

            
        }
        #endregion
        #region FunctionsTimers
        private void timerFront_Tick(object sender, EventArgs e)
        {
            DrawToBufferFront();
            bufGrFront.Render();
        }

        private void timerRight_Tick(object sender, EventArgs e)
        {
            DrawToBufferRight();
            bufGrRight.Render();
        }

        private void timerTop_Tick(object sender, EventArgs e)
        {
            DrawToBufferTop();
            bufGrTop.Render();
        }
        private void timer3D_Tick(object sender, EventArgs e)
        {
            managed3D.Redraw3D();
            CalculateSizeObject(managed2D.GetSelectedPrimitive());
        }

        #endregion
        #region FunctionsScrolling

        private void panelTop_Scroll(object sender, ScrollEventArgs e)
        {
			topHScrollValue = panelTop.HorizontalScroll.Value;
			topVScrollValue = panelTop.VerticalScroll.Value;

			pictureBoxTop.Invalidate();
        }

        private void panelFront_Scroll(object sender, ScrollEventArgs e)
        {
			frontHScrollValue = panelFront.HorizontalScroll.Value;
			frontVScrollValue = panelFront.VerticalScroll.Value;

			pictureBoxFront.Invalidate();
        }

        private void panelRight_Scroll(object sender, ScrollEventArgs e)
        {
			rightHScrollValue = panelRight.HorizontalScroll.Value;
			rightVScrollValue = panelRight.VerticalScroll.Value;

			pictureBoxRight.Invalidate();
        }
        /// <summary>
        /// сообщение любой из панелей о необходимости скролирования в ту или иную сторону
        /// </summary>
        /// <param name="sender">в нашем случае этот параметр будет представлять обЪект панель</param>
        /// <param name="isToNegateveSide">определяет направление движения скроллера (true)-> к нулю</param>
        /// <param name="isHorisontalScroll">оределяет какой из скроллеров должен быть проскроллирован</param>
        void Form1_EventPanelScrolling(object sender, bool isToNegateveSide, bool isHorisontalScroll)
        {
            if (isHorisontalScroll)
            {
                if (isToNegateveSide)
                {
                    if (((Panel)sender).HorizontalScroll.Value != 0)
                        ((Panel)sender).HorizontalScroll.Value -= 1;
				}
                else
                {
                    if (((Panel)sender).HorizontalScroll.Value != Form1.panelWidth)
                        ((Panel)sender).HorizontalScroll.Value += 1;
                }
				#region UpdateStatickMembersofScrolls

				if (((Panel)sender).Name == "panelFront")
					frontHScrollValue = ((Panel)sender).HorizontalScroll.Value;

				if (((Panel)sender).Name == "panelRight")
					rightHScrollValue = ((Panel)sender).HorizontalScroll.Value;

				if (((Panel)sender).Name == "panelTop")
					topHScrollValue = ((Panel)sender).HorizontalScroll.Value;

				#endregion
            }
            else
            {
                if (isToNegateveSide)
                {
                    if(((Panel)sender).VerticalScroll.Value != 0)
                        ((Panel)sender).VerticalScroll.Value -= 1;
                }
                else
                {
                    if (((Panel)sender).VerticalScroll.Value != Form1.panelHeight)
                     ((Panel)sender).VerticalScroll.Value += 1;
                }
				#region UpdateStatickMembersofScrolls

				if (((Panel)sender).Name == "panelFront")
					frontVScrollValue = ((Panel)sender).VerticalScroll.Value;

				if (((Panel)sender).Name == "panelRight")
					rightVScrollValue = ((Panel)sender).VerticalScroll.Value;

				if (((Panel)sender).Name == "panelTop")
					topVScrollValue = ((Panel)sender).VerticalScroll.Value;
				#endregion
            }
        }
        void ComputeScrolling(string nameProjection,Point e)
        {
            if (nameProjection == "Front")
            {
                #region ComputeScrolling
                {
                    if (e.X >= 0 && e.X <= Form1.panelWidth)
                    {
                        //label1.Text = "X : " + e.X.ToString();

                        if (isMousePressed)
                        {
                            int tm = (panelFront.DisplayRectangle.Left * -1) + panelFront.ClientRectangle.Right;
                            if (e.X >= tm)
                            {
                                int i = e.X - tm;
                                int j = 0;
                                while (j < i)
                                {
                                    EventPanelScrolling(panelFront, false, true);
                                    j++;
                                }
                            }
                            else
                            {
                                tm = (panelFront.DisplayRectangle.Left * -1) + panelFront.ClientRectangle.X;
                                if (e.X <= tm)
                                {
                                    int i = tm - e.X;
                                    int j = 0;
                                    while (j < i)
                                    {
                                        EventPanelScrolling(panelFront, true, true);
                                        j++;
                                    }
                                }
                            }
                        }
                    }

                    if (e.Y >= 0 && e.Y <= Form1.panelHeight)
                    {
                        //label2.Text = "Y : " + e.Y.ToString();
                        if (isMousePressed)
                        {
                            int tm = (panelFront.DisplayRectangle.Top * -1) + panelFront.ClientRectangle.Height;
                            if (e.Y >= tm)
                            {
                                int i = e.Y - tm;
                                int j = 0;
                                while (j < i)
                                {
                                    EventPanelScrolling(panelFront, false, false);
                                    j++;
                                }
                            }
                            else
                            {
                                tm = (panelFront.DisplayRectangle.Top * -1) + panelFront.ClientRectangle.Top;
                                if (e.Y <= tm)
                                {
                                    int i = tm - e.Y;
                                    int j = 0;
                                    while (j < i)
                                    {
                                        EventPanelScrolling(panelFront, true, false);
                                        j++;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            if (nameProjection == "Top")
            {
                #region ComputeScrolling
                {
                    if (e.X >= 0 && e.X <= Form1.panelWidth)
                    {
                        //label5.Text = "X : " + e.X.ToString();
                        if (isMousePressed)
                        {
                            int tm = (panelTop.DisplayRectangle.Left * -1) + panelTop.ClientRectangle.Right;
                            if (e.X >= tm)
                            {
                                int i = e.X - tm;
                                int j = 0;
                                while (j < i)
                                {
                                    EventPanelScrolling(panelTop, false, true);
                                    j++;
                                }
                            }
                            else
                            {
                                tm = (panelTop.DisplayRectangle.Left * -1) + panelTop.ClientRectangle.X;
                                if (e.X <= tm)
                                {
                                    int i = tm - e.X;
                                    int j = 0;
                                    while (j < i)
                                    {
                                        EventPanelScrolling(panelTop, true, true);
                                        j++;
                                    }
                                }
                            }
                        }
                    }
                    if (e.Y >= 0 && e.Y <= Form1.panelHeight)
                    {
                        //label6.Text = "Y : " + e.Y.ToString();
                        if (isMousePressed)
                        {
                            int tm = (panelTop.DisplayRectangle.Top * -1) + panelTop.ClientRectangle.Height;
                            if (e.Y >= tm)
                            {
                                int i = e.Y - tm;
                                int j = 0;
                                while (j < i)
                                {
                                    EventPanelScrolling(panelTop, false, false);
                                    j++;
                                }
                            }
                            else
                            {
                                tm = (panelTop.DisplayRectangle.Top * -1) + panelTop.ClientRectangle.Top;
                                if (e.Y <= tm)
                                {
                                    int i = tm - e.Y;
                                    int j = 0;
                                    while (j < i)
                                    {
                                        EventPanelScrolling(panelTop, true, false);
                                        j++;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            if (nameProjection == "Right")
            {
                #region ComputeScrolling
                {
                    if (e.X >= 0 && e.X <= Form1.panelWidth)
                    {
                        //label3.Text = "X : " + e.X.ToString();
                        if (isMousePressed)
                        {
                            int tm = (panelRight.DisplayRectangle.Left * -1) + panelRight.ClientRectangle.Right;
                            if (e.X >= tm)
                            {
                                int i = e.X - tm;
                                int j = 0;
                                while (j < i)
                                {
                                    EventPanelScrolling(panelRight, false, true);
                                    j++;
                                }
                            }
                            else
                            {
                                tm = (panelRight.DisplayRectangle.Left * -1) + panelRight.ClientRectangle.X;
                                if (e.X <= tm)
                                {
                                    int i = tm - e.X;
                                    int j = 0;
                                    while (j < i)
                                    {
                                        EventPanelScrolling(panelRight, true, true);
                                        j++;
                                    }
                                }
                            }
                        }
                    }
                    if (e.Y >= 0 && e.Y <= Form1.panelHeight)
                    {
                        //label4.Text = "Y : " + e.Y.ToString();
                        if (isMousePressed)
                        {
                            int tm = (panelRight.DisplayRectangle.Top * -1) + panelRight.ClientRectangle.Height;
                            if (e.Y >= tm)
                            {
                                int i = e.Y - tm;
                                int j = 0;
                                while (j < i)
                                {
                                    EventPanelScrolling(panelRight, false, false);
                                    j++;
                                }
                            }
                            else
                            {
                                tm = (panelRight.DisplayRectangle.Top * -1) + panelRight.ClientRectangle.Top;
                                if (e.Y <= tm)
                                {
                                    int i = tm - e.Y;
                                    int j = 0;
                                    while (j < i)
                                    {
                                        EventPanelScrolling(panelRight, true, false);
                                        j++;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
        }
        #endregion
        #region FunctionsMouseDown
        private void pictureBoxTop_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mousePositionDown = e.Location;
                mousePositionDownSnap = CalcSnappingPoint(e.Location);
                if (isRectEnabled)
                {
                    if (managed2D.CurrentPrimitive == null)
                        managed2D.CurrentPrimitive = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());
                }
                else
                {
                    if (checkBoxPointer.Checked && nameOfActionChanges == "None")
                    {
						#region SelectingPlayer

						if (managed2D.BluePlayer != null)
						{
							if (e.X > managed2D.BluePlayer.rectTop.X
								&& e.X < managed2D.BluePlayer.rectTop.X + managed2D.BluePlayer.rectTop.Width
								&& e.Y > managed2D.BluePlayer.rectTop.Y
								&& e.Y < managed2D.BluePlayer.rectTop.Y + managed2D.BluePlayer.rectTop.Height)
							{
								managed2D.BluePlayer.isPlayerSelected = true;
								managed3D.playerBlue.isPlayerSelected = true;
								DeselectAllPrimitives(-1);
								isMousePressed = true;
								return;
							}
							else
							{
								managed2D.BluePlayer.isPlayerSelected = false;
								managed3D.playerBlue.isPlayerSelected = false;
							}
						}
						if (managed2D.RedPlayer != null)
						{
							if (e.X > managed2D.RedPlayer.rectTop.X
								&& e.X < managed2D.RedPlayer.rectTop.X + managed2D.RedPlayer.rectTop.Width
								&& e.Y > managed2D.RedPlayer.rectTop.Y
								&& e.Y < managed2D.RedPlayer.rectTop.Y + managed2D.RedPlayer.rectTop.Height)
							{
								managed2D.RedPlayer.isPlayerSelected = true;
								managed3D.playerRed.isPlayerSelected = true;
								DeselectAllPrimitives(-1);
								isMousePressed = true;
								return;
							}
							else
							{
								managed2D.RedPlayer.isPlayerSelected = false;
								managed3D.playerRed.isPlayerSelected = false;
							}
						}

						#endregion
                        #region SelectingPrimitive

                        if (isTransformSelectionEnabled)
                        {
                            if (managed2D.List2DPrimitives.Count != 0)
                            {
                                foreach (int key in managed2D.List2DPrimitives.Keys)
                                {
                                    RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];

                                    int maxX = -panelWidth;
                                    int maxY = -panelHeight;
                                    int minX = panelWidth;
                                    int minY = panelHeight;

                                    for (int i = 0; i < 10; i++)
                                    {
                                        if (maxX < rp.MasLinesTop[i].X)
                                            maxX = rp.MasLinesTop[i].X;
                                        if (maxY < rp.MasLinesTop[i].Y)
                                            maxY = rp.MasLinesTop[i].Y;
                                        if (minX > rp.MasLinesTop[i].X)
                                            minX = rp.MasLinesTop[i].X;
                                        if (minY > rp.MasLinesTop[i].Y)
                                            minY = rp.MasLinesTop[i].Y;
                                    }
                                    if (minX < e.X && maxX > e.X && minY < e.Y && maxY > e.Y)
                                    {
                                        DeselectAllPrimitives(key);
                                        rp.IsSelected = true;

                                        managed3D.selectedIndex = key;

                                        if (lineSelection == null)
                                            lineSelection = new LinesSelection(rp);
                                        managed2D.IndexOfSelectedPrimitive = key;

                                        managed2D.ConvertedPrimitive = GetConvertedPrimitiveToLocalCoords(managed2D.CurrentPrimitive);
                                        if (SelectObject != null)
                                            SelectObject(rp, managed2D.ConvertedPrimitive);

                                        break;
                                    }
                                    else
                                    {
                                        DeselectAllPrimitives(-1);
                                        rp.IsSelected = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (managed2D.List2DPrimitives.Count != 0)
                            {
                                foreach (int key in managed2D.List2DPrimitives.Keys)
                                {
                                    RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];
                                    int maxX = -panelWidth;
                                    int maxY = -panelHeight;
                                    int minX = panelWidth;
                                    int minY = panelHeight;

                                    for (int i = 0; i < 10; i++)
                                    {
                                        if (maxX < rp.MasLinesTop[i].X)
                                            maxX = rp.MasLinesTop[i].X;
                                        if (maxY < rp.MasLinesTop[i].Y)
                                            maxY = rp.MasLinesTop[i].Y;
                                        if (minX > rp.MasLinesTop[i].X)
                                            minX = rp.MasLinesTop[i].X;
                                        if (minY > rp.MasLinesTop[i].Y)
                                            minY = rp.MasLinesTop[i].Y;
                                    }
                                    if (minX < e.X && maxX > e.X && minY < e.Y && maxY > e.Y)
                                    {
                                        DeselectAllPrimitives(key);
                                        rp.IsSelected = true;

                                        managed3D.selectedIndex = key;

                                        selection = new SimpleSelection(rp);
                                        managed2D.IndexOfSelectedPrimitive = key;

                                        managed2D.ConvertedPrimitive = GetConvertedPrimitiveToLocalCoords(managed2D.CurrentPrimitive);
                                        if (SelectObject != null)
                                            SelectObject(rp, managed2D.ConvertedPrimitive);

                                        break;
                                    }
                                    else
                                    {
                                        DeselectAllPrimitives(-1);
                                        rp.IsSelected = false;
                                    }
                                }
                            }
                        }

                        #endregion
						
                    }
                    else
                    {
						if (checkBoxPlayerRed.Checked)
						{
							if (checkBoxSnap.Checked)
							{
								managed2D.RedPlayer = new Player(mousePositionDownSnap, "Top", grid2D, managed3D.device);
								
								managed2D.RedPlayer.isPlayerSelected = true;
								checkBoxPlayerRed.Checked = false;
								checkBoxPlayerRed.Enabled = false;
								managed3D.playerRed = managed2D.RedPlayer;
							}
							else
							{
								managed2D.RedPlayer = new Player(e.Location, "Top", grid2D, managed3D.device);

								managed2D.RedPlayer.isPlayerSelected = true;
								checkBoxPlayerRed.Checked = false;
								checkBoxPlayerRed.Enabled = false;
								managed3D.playerRed = managed2D.RedPlayer;
							}
						}
						if (checkBoxPlayerBlue.Checked)
						{
							if (checkBoxSnap.Checked)
							{
								managed2D.BluePlayer = new Player(mousePositionDownSnap, "Top", grid2D, managed3D.device);
								managed2D.BluePlayer.isPlayerRed = false;
								managed2D.BluePlayer.isPlayerSelected = true;
								checkBoxPlayerBlue.Checked = false;
								checkBoxPlayerBlue.Enabled = false;
								managed3D.playerBlue = managed2D.BluePlayer;
							}
							else
							{
								managed2D.BluePlayer = new Player(e.Location, "Top", grid2D, managed3D.device);
								managed2D.BluePlayer.isPlayerRed = false;
								managed2D.BluePlayer.isPlayerSelected = true;
								checkBoxPlayerBlue.Checked = false;
								checkBoxPlayerBlue.Enabled = false;
								managed3D.playerBlue = managed2D.BluePlayer;
							}
						}
                    }
                }
                isMousePressed = true;
            }
        }
        private void pictureBoxFront_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mousePositionDown = e.Location;
                mousePositionDownSnap = CalcSnappingPoint(e.Location);
                if (isRectEnabled)
                {
                    if (managed2D.CurrentPrimitive == null)
                        managed2D.CurrentPrimitive = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());
                }
                else
                {
                    if (checkBoxPointer.Checked && nameOfActionChanges == "None")
                    {
						#region SelectingPlayer

						if (managed2D.BluePlayer != null)
						{
							if (e.X > managed2D.BluePlayer.rectFront.X
								&& e.X < managed2D.BluePlayer.rectFront.X + managed2D.BluePlayer.rectFront.Width
								&& e.Y > managed2D.BluePlayer.rectFront.Y
								&& e.Y < managed2D.BluePlayer.rectFront.Y + managed2D.BluePlayer.rectFront.Height)
							{
								managed2D.BluePlayer.isPlayerSelected = true;
								managed3D.playerBlue.isPlayerSelected = true;
								DeselectAllPrimitives(-1);
								isMousePressed = true;
								return;
							}
							else
							{
								managed2D.BluePlayer.isPlayerSelected = false;
								managed3D.playerBlue.isPlayerSelected = false;
							}
						}
						if (managed2D.RedPlayer != null)
						{
							if (e.X > managed2D.RedPlayer.rectFront.X
								&& e.X < managed2D.RedPlayer.rectFront.X + managed2D.RedPlayer.rectFront.Width
								&& e.Y > managed2D.RedPlayer.rectFront.Y
								&& e.Y < managed2D.RedPlayer.rectFront.Y + managed2D.RedPlayer.rectFront.Height)
							{
								managed2D.RedPlayer.isPlayerSelected = true;
								managed3D.playerRed.isPlayerSelected = true;
								DeselectAllPrimitives(-1);
								isMousePressed = true;
								return;
							}
							else
							{
								managed2D.RedPlayer.isPlayerSelected = false;
								managed3D.playerRed.isPlayerSelected = false;
							}
						}

						#endregion
						
                        #region SelectingPrimitive

                        if (isTransformSelectionEnabled)
                        {
                            if (managed2D.List2DPrimitives.Count != 0)
                            {
                                foreach (int key in managed2D.List2DPrimitives.Keys)
                                {
                                    RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];

                                    int maxX = -panelWidth;
                                    int maxY = -panelHeight;
                                    int minX = panelWidth;
                                    int minY = panelHeight;

                                    for(int i = 0;i<10;i++)
                                    {
                                        if(maxX < rp.MasLinesFront[i].X)
                                            maxX = rp.MasLinesFront[i].X;
                                        if(maxY < rp.MasLinesFront[i].Y)
                                            maxY = rp.MasLinesFront[i].Y;
                                        if(minX > rp.MasLinesFront[i].X)
                                            minX = rp.MasLinesFront[i].X;
                                        if(minY > rp.MasLinesFront[i].Y)
                                            minY = rp.MasLinesFront[i].Y;
                                    }
                                    if (minX < e.X && maxX > e.X && minY < e.Y && maxY > e.Y)
                                    {
                                        DeselectAllPrimitives(key);
                                        rp.IsSelected = true;

                                        managed3D.selectedIndex = key;
                                        

                                        if (lineSelection == null)
                                            lineSelection = new LinesSelection(rp);
                                        managed2D.IndexOfSelectedPrimitive = key;

                                        managed2D.ConvertedPrimitive = GetConvertedPrimitiveToLocalCoords(managed2D.CurrentPrimitive);
                                        if (SelectObject != null)
                                            SelectObject(rp, managed2D.ConvertedPrimitive);
                                        break;
                                    }
                                    else
                                    {
                                        DeselectAllPrimitives(-1);
                                        rp.IsSelected = false;

                                        if (SelectObject != null)
                                            SelectObject(null,null);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (managed2D.List2DPrimitives.Count != 0)
                            {
                                foreach (int key in managed2D.List2DPrimitives.Keys)
                                {
                                    RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];

                                    int maxX = -panelWidth;
                                    int maxY = -panelHeight;
                                    int minX = panelWidth;
                                    int minY = panelHeight;

                                    for(int i = 0;i<10;i++)
                                    {
                                        if(maxX < rp.MasLinesFront[i].X)
                                            maxX = rp.MasLinesFront[i].X;
                                        if(maxY < rp.MasLinesFront[i].Y)
                                            maxY = rp.MasLinesFront[i].Y;
                                        if(minX > rp.MasLinesFront[i].X)
                                            minX = rp.MasLinesFront[i].X;
                                        if(minY > rp.MasLinesFront[i].Y)
                                            minY = rp.MasLinesFront[i].Y;
                                    }
                                    if (minX < e.X && maxX > e.X && minY < e.Y && maxY > e.Y)
                                    {
                                        DeselectAllPrimitives(key);
                                        rp.IsSelected = true;

                                        managed3D.selectedIndex = key;
                                        

                                        selection = new SimpleSelection(rp);
                                        managed2D.IndexOfSelectedPrimitive = key;

                                        managed2D.ConvertedPrimitive = GetConvertedPrimitiveToLocalCoords(managed2D.CurrentPrimitive);
                                        if (SelectObject != null)
                                            SelectObject(rp, managed2D.ConvertedPrimitive);

                                        break;
                                    }
                                    else
                                    {
                                        DeselectAllPrimitives(-1);
                                        rp.IsSelected = false;

                                        if (SelectObject != null)
                                            SelectObject(null,null);
                                    }
                                }
                            }
                        }

                        #endregion
						
                    }
                    else
                    {
						if (checkBoxPlayerRed.Checked)
						{
							if (checkBoxSnap.Checked)
							{
								managed2D.RedPlayer = new Player(mousePositionDownSnap, "Front", grid2D, managed3D.device);
								managed2D.RedPlayer.isPlayerSelected = true;
								checkBoxPlayerRed.Checked = false;
								checkBoxPlayerRed.Enabled = false;
								managed3D.playerRed = managed2D.RedPlayer;
							}
							else
							{
								managed2D.RedPlayer = new Player(e.Location, "Front", grid2D, managed3D.device);
								managed2D.RedPlayer.isPlayerSelected = true;
								checkBoxPlayerRed.Checked = false;
								checkBoxPlayerRed.Enabled = false;
								managed3D.playerRed = managed2D.RedPlayer;
							}
						}
						if (checkBoxPlayerBlue.Checked)
						{
							if (checkBoxSnap.Checked)
							{
								managed2D.BluePlayer = new Player(mousePositionDownSnap, "Front", grid2D, managed3D.device);
								managed2D.BluePlayer.isPlayerSelected = true;
								checkBoxPlayerBlue.Checked = false;
								checkBoxPlayerBlue.Enabled = false;
								managed3D.playerBlue = managed2D.BluePlayer;
							}
							else
							{
								managed2D.BluePlayer = new Player(e.Location, "Front", grid2D, managed3D.device);
								managed2D.BluePlayer.isPlayerSelected = true;
								checkBoxPlayerBlue.Checked = false;
								checkBoxPlayerBlue.Enabled = false;
								managed3D.playerBlue = managed2D.BluePlayer;
							}
						}
                    }
                }
                isMousePressed = true;
            }
        }
        private void pictureBoxRight_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mousePositionDown = e.Location;
                mousePositionDownSnap = CalcSnappingPoint(e.Location);
                if (isRectEnabled)
                {
                    if (managed2D.CurrentPrimitive == null)
                        managed2D.CurrentPrimitive = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());
                }
                else
                {
                    if (checkBoxPointer.Checked && nameOfActionChanges == "None")
                    {
						#region SelectingPlayer

						if (managed2D.BluePlayer != null)
						{
							if (e.X > managed2D.BluePlayer.rectRight.X
								&& e.X < managed2D.BluePlayer.rectRight.X + managed2D.BluePlayer.rectRight.Width
								&& e.Y > managed2D.BluePlayer.rectRight.Y
								&& e.Y < managed2D.BluePlayer.rectRight.Y + managed2D.BluePlayer.rectRight.Height)
							{
								managed2D.BluePlayer.isPlayerSelected = true;
								managed3D.playerBlue.isPlayerSelected = true;
								DeselectAllPrimitives(-1);
								isMousePressed = true;
								return;
							}
							else
							{
								managed2D.BluePlayer.isPlayerSelected = false;
								managed3D.playerBlue.isPlayerSelected = false;
							}
						}
						if (managed2D.RedPlayer != null)
						{
							if (e.X > managed2D.RedPlayer.rectRight.X
								&& e.X < managed2D.RedPlayer.rectRight.X + managed2D.RedPlayer.rectRight.Width
								&& e.Y > managed2D.RedPlayer.rectRight.Y
								&& e.Y < managed2D.RedPlayer.rectRight.Y + managed2D.RedPlayer.rectRight.Height)
							{
								managed2D.RedPlayer.isPlayerSelected = true;
								managed3D.playerRed.isPlayerSelected = true;
								DeselectAllPrimitives(-1);
								isMousePressed = true;
								return;
							}
							else
							{
								managed2D.RedPlayer.isPlayerSelected = false;
								managed3D.playerRed.isPlayerSelected = false;
							}
						}

						#endregion
                        #region SelectingPrimitive

                        if (isTransformSelectionEnabled)
                        {
                            if (managed2D.List2DPrimitives.Count != 0)
                            {
                                foreach (int key in managed2D.List2DPrimitives.Keys)
                                {
                                    RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];

                                    int maxX = -panelWidth;
                                    int maxY = -panelHeight;
                                    int minX = panelWidth;
                                    int minY = panelHeight;

                                    for (int i = 0; i < 10; i++)
                                    {
                                        if (maxX < rp.MasLinesRight[i].X)
                                            maxX = rp.MasLinesRight[i].X;
                                        if (maxY < rp.MasLinesRight[i].Y)
                                            maxY = rp.MasLinesRight[i].Y;
                                        if (minX > rp.MasLinesRight[i].X)
                                            minX = rp.MasLinesRight[i].X;
                                        if (minY > rp.MasLinesRight[i].Y)
                                            minY = rp.MasLinesRight[i].Y;
                                    }
                                    if (minX < e.X && maxX > e.X && minY < e.Y && maxY > e.Y)
                                    {
                                        DeselectAllPrimitives(key);
                                        rp.IsSelected = true;

                                        managed3D.selectedIndex = key;

                                        if (lineSelection == null)
                                            lineSelection = new LinesSelection(rp);
                                        managed2D.IndexOfSelectedPrimitive = key;

                                        managed2D.ConvertedPrimitive = GetConvertedPrimitiveToLocalCoords(managed2D.CurrentPrimitive);
                                        if (SelectObject != null)
                                            SelectObject(rp, managed2D.ConvertedPrimitive);

                                        break;
                                    }
                                    else
                                    {
                                        DeselectAllPrimitives(-1);
                                        rp.IsSelected = false;
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (managed2D.List2DPrimitives.Count != 0)
                            {
                                foreach (int key in managed2D.List2DPrimitives.Keys)
                                {
                                    RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];
                                    int maxX = -panelWidth;
                                    int maxY = -panelHeight;
                                    int minX = panelWidth;
                                    int minY = panelHeight;

                                    for (int i = 0; i < 10; i++)
                                    {
                                        if (maxX < rp.MasLinesRight[i].X)
                                            maxX = rp.MasLinesRight[i].X;
                                        if (maxY < rp.MasLinesRight[i].Y)
                                            maxY = rp.MasLinesRight[i].Y;
                                        if (minX > rp.MasLinesRight[i].X)
                                            minX = rp.MasLinesRight[i].X;
                                        if (minY > rp.MasLinesRight[i].Y)
                                            minY = rp.MasLinesRight[i].Y;
                                    }
                                    if (minX < e.X && maxX > e.X && minY < e.Y && maxY > e.Y)
                                    {
                                        DeselectAllPrimitives(key);
                                        rp.IsSelected = true;

                                        managed3D.selectedIndex = key;

                                        selection = new SimpleSelection(rp);
                                        managed2D.IndexOfSelectedPrimitive = key;

                                        managed2D.ConvertedPrimitive = GetConvertedPrimitiveToLocalCoords(managed2D.CurrentPrimitive);
                                        if (SelectObject != null)
                                            SelectObject(rp, managed2D.ConvertedPrimitive);

                                        break;
                                    }
                                    else
                                    {
                                        DeselectAllPrimitives(-1);
                                        rp.IsSelected = false;
                                    }
                                }
                            }
                        }

                        #endregion
						
					}
                    else
                    {
                        if (checkBoxPlayerRed.Checked)
                        {
							if (checkBoxSnap.Checked)
							{
								managed2D.RedPlayer = new Player(mousePositionDownSnap, "Right", grid2D, managed3D.device);
								managed2D.RedPlayer.isPlayerSelected = true;
								checkBoxPlayerRed.Checked = false;
								checkBoxPlayerRed.Enabled = false;
								managed3D.playerRed = managed2D.RedPlayer;
							}
							else
							{
								managed2D.RedPlayer = new Player(e.Location, "Right", grid2D, managed3D.device);
								managed2D.RedPlayer.isPlayerSelected = true;
								checkBoxPlayerRed.Checked = false;
								checkBoxPlayerRed.Enabled = false;
								managed3D.playerRed = managed2D.RedPlayer;
							}
                        }
                        if (checkBoxPlayerBlue.Checked)
                        {
							if (checkBoxSnap.Checked)
							{
								managed2D.BluePlayer = new Player(mousePositionDownSnap, "Right", grid2D, managed3D.device);
								managed2D.BluePlayer.isPlayerSelected = true;
								checkBoxPlayerBlue.Checked = false;
								checkBoxPlayerBlue.Enabled = false;
								managed3D.playerBlue = managed2D.BluePlayer;
							}
							else
							{
								managed2D.BluePlayer = new Player(e.Location, "Right", grid2D, managed3D.device);
								managed2D.BluePlayer.isPlayerSelected = true;
								checkBoxPlayerBlue.Checked = false;
								checkBoxPlayerBlue.Enabled = false;
								managed3D.playerBlue = managed2D.BluePlayer;
							}
                        }
                    }
                }
                isMousePressed = true;
            }
        }
		private void pictureBox3D_MouseDown(object sender, MouseEventArgs e)
		{
            string playerName = "NoPlayer";
			int key = pickObj.PickVector(managed3D.device, managed3D.objectsList,managed2D.RedPlayer,managed2D.BluePlayer,out playerName);

			DeselectAllPrimitives(key);
			
			managed2D.IndexOfSelectedPrimitive = key;
			RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];
			SelectObject(rp, GetConvertedPrimitiveToLocalCoords(rp));
			managed3D.selectedIndex = key;
            if (key != -1)
            {
                ((RectPrimitive)managed2D.List2DPrimitives[key]).IsSelected = true;
                if (managed2D.BluePlayer != null)
                    managed2D.BluePlayer.isPlayerSelected = false;

                if (managed2D.RedPlayer != null)
                    managed2D.RedPlayer.isPlayerSelected = false;
            }
            else
            {
                DeselectAllPrimitives(-1);
                if (playerName != "NoPlayer")
                {
                    if (playerName == "red")
                        managed2D.RedPlayer.isPlayerSelected = true;

                    if (playerName == "blue")
                        managed2D.BluePlayer.isPlayerSelected = true;
                }
            }
			timerFront_Tick(null, null);
			timerRight_Tick(null, null);
			timerTop_Tick(null, null);

		}
		void Managed3D_MouseClick3D()
		{
			pictureBox3D_MouseDown(null, null);
		}
        #endregion
        #region FunctionsMouseMove
        private void pictureBoxTop_MouseMove(object sender, MouseEventArgs e)
        {
            mouseCurrentPosition = e.Location;
            mouseCurrentPositionSnap = CalcSnappingPoint(e.Location);

            
            if (isMousePressed)
            {
                #region WorkWithScrollers

                if (nameOfActionChanges == "Move" && pictureBoxTop.Cursor == Cursors.SizeAll && managed2D.IndexOfSelectedPrimitive == -1 && managed2D.CurrentPrimitive == null)
                {
					if ((managed2D.RedPlayer == null || !managed2D.RedPlayer.isPlayerSelected)
						&& (managed2D.BluePlayer == null || !managed2D.BluePlayer.isPlayerSelected))
					{
						if (e.X > mousePositionDown.X)
						{
							MoveHolst(panelTop, false, true);
							mousePositionDown = mouseCurrentPosition;
						}
						if (e.X < mousePositionDown.X)
						{
							MoveHolst(panelTop, true, true);
							mousePositionDown = mouseCurrentPosition;
						}
						if (e.Y > mousePositionDown.Y)
						{
							MoveHolst(panelTop, false, false);
							mousePositionDown = mouseCurrentPosition;
						}
						if (e.Y < mousePositionDown.Y)
						{
							MoveHolst(panelTop, true, false);
							mousePositionDown = mouseCurrentPosition;
						}
					}
                }
                else
                    ComputeScrolling("Top", e.Location);

                #endregion

                if (nameOfActionChanges == "None")
                {
                    if (!isSnapEnabled)
                    {
                        if (isRectEnabled)
                            managed2D.CreateRectTop(mousePositionDown, mouseCurrentPosition, grid2D.mainSizeGrid);
                    }
                    else
                    {
                        if (isRectEnabled)
                            managed2D.CreateRectTop(mousePositionDownSnap, mouseCurrentPositionSnap, grid2D.mainSizeGrid);
                    }
                }
                else
                {
                    if (nameOfActionChanges == "Transform" && pictureBoxTop.Cursor == Cursors.NoMove2D)
                    {
                        lineSelection.ChanginSelectedPointByIndex("Top", grid2D.SnapingPoint(e.Location));

                        if (managed2D.CurrentPrimitive != null)
                        {
                            managed2D.CurrentPrimitive.IsTransformed = true;
                            textureSettings.convertedPrimitive = GetConvertedPrimitiveToLocalCoords(managed2D.CurrentPrimitive);
                        }
                        else
                        {
                            foreach (int key in managed2D.List2DPrimitives.Keys)
                            {
                                RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];
                                if (rp.IsSelected)
                                {
                                    rp.IsTransformed = true;
                                    textureSettings.convertedPrimitive = GetConvertedPrimitiveToLocalCoords(rp);
                                }
                            }
                        }
                    }
                    else
                    {
                        ExecuteComandCursors("Top");

						if (nameOfActionChanges == "Move" && pictureBoxTop.Cursor == Cursors.SizeAll)
						{
							if (managed2D.BluePlayer != null)
								MoovePlayer(managed2D.BluePlayer, "Top");

							if (managed2D.RedPlayer != null)
								MoovePlayer(managed2D.RedPlayer, "Top");
						}
                    }

                    if (Update3DPrimitiveCoords != null)
                    {
                        RectPrimitive rp = managed2D.GetSelectedPrimitive();
                        if(rp != null)
                        {
                            RectPrimitive convPr = GetConvertedPrimitiveToLocalCoords(rp);
                            if(convPr != null)
                                Update3DPrimitiveCoords(convPr,managed2D.IndexOfSelectedPrimitive);
                        }
                    }
                }
            }
            else
            {
                if (nameOfActionChanges != "Move")
                {
                    Point centerRegion = new Point();
                    if (isTransformSelectionEnabled)
                    {
                        if (lineSelection != null)
                        {
                            pictureBoxTop.Cursor = lineSelection.isMouseUnderRegionSel(mouseCurrentPosition, out nameOfActionChanges, out centerRegion, "Top");
                        }
                    }
                    else
                    {
                        if (selection != null)
                            pictureBoxTop.Cursor = selection.isMouseUnderRegionSel(mouseCurrentPosition, out nameOfActionChanges, out  centerRegion, "Top");
                    }
                    if (Update3DPrimitiveCoords != null)
                    {
                        RectPrimitive rp = managed2D.GetSelectedPrimitive();
                        if (rp != null)
                        {
                            RectPrimitive convPr = GetConvertedPrimitiveToLocalCoords(rp);
                            if (convPr != null)
                                Update3DPrimitiveCoords(convPr, managed2D.IndexOfSelectedPrimitive);
                        }
                    }
                }
            }
        }
        private void pictureBoxFront_MouseMove(object sender, MouseEventArgs e)
        {
            mouseCurrentPosition = e.Location;
            mouseCurrentPositionSnap = CalcSnappingPoint(e.Location);

            

            if (isMousePressed)
            {
                #region WorkWithScrollers

                if (nameOfActionChanges == "Move" && pictureBoxFront.Cursor == Cursors.SizeAll && managed2D.IndexOfSelectedPrimitive == -1 && managed2D.CurrentPrimitive == null)
                {
					if ((managed2D.RedPlayer == null || !managed2D.RedPlayer.isPlayerSelected)
						&& (managed2D.BluePlayer == null || !managed2D.BluePlayer.isPlayerSelected))
					{
						if (e.X > mousePositionDown.X)
						{
							MoveHolst(panelFront, false, true);
							mousePositionDown = mouseCurrentPosition;
						}
						if (e.X < mousePositionDown.X)
						{
							MoveHolst(panelFront, true, true);
							mousePositionDown = mouseCurrentPosition;
						}
						if (e.Y > mousePositionDown.Y)
						{
							MoveHolst(panelFront, false, false);
							mousePositionDown = mouseCurrentPosition;
						}
						if (e.Y < mousePositionDown.Y)
						{
							MoveHolst(panelFront, true, false);
							mousePositionDown = mouseCurrentPosition;
						}
					}
                }
                else
                    ComputeScrolling("Front", e.Location);

                #endregion

                if (nameOfActionChanges == "None")
                {
                    if (!isSnapEnabled)
                    {
                        if (isRectEnabled)
                            managed2D.CreateRectFront(mousePositionDown, mouseCurrentPosition, grid2D.mainSizeGrid);
                    }
                    else
                    {
                        if (isRectEnabled)
                            managed2D.CreateRectFront(mousePositionDownSnap, mouseCurrentPositionSnap, grid2D.mainSizeGrid);
                    }
                }
                else
                {
                    if (nameOfActionChanges == "Transform" && pictureBoxFront.Cursor == Cursors.NoMove2D)
                    {
                        lineSelection.ChanginSelectedPointByIndex("Front", grid2D.SnapingPoint(e.Location));

                        if (managed2D.CurrentPrimitive != null)
                        {
                            managed2D.CurrentPrimitive.IsTransformed = true;
                            textureSettings.convertedPrimitive = GetConvertedPrimitiveToLocalCoords(managed2D.CurrentPrimitive);
                        }
                        else
                        {
                            foreach (int key in managed2D.List2DPrimitives.Keys)
                            {
                                RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];
                                if (rp.IsSelected)
                                {
                                    rp.IsTransformed = true;
                                    textureSettings.convertedPrimitive = GetConvertedPrimitiveToLocalCoords(rp);
                                }
                            }
                        }
                    }
                    else
                    {
                        ExecuteComandCursors("Front");

						if (nameOfActionChanges == "Move" && pictureBoxFront.Cursor == Cursors.SizeAll)
						{
							if (managed2D.BluePlayer != null)
								MoovePlayer(managed2D.BluePlayer, "Front");

							if (managed2D.RedPlayer != null)
								MoovePlayer(managed2D.RedPlayer, "Front");
						}
                    }
                    if (Update3DPrimitiveCoords != null)
                    {
                        RectPrimitive rp = managed2D.GetSelectedPrimitive();
                        if (rp != null)
                        {
                            RectPrimitive convPr = GetConvertedPrimitiveToLocalCoords(rp);
                            if (convPr != null)
                                Update3DPrimitiveCoords(convPr, managed2D.IndexOfSelectedPrimitive);
                        }
                    }
                }
            }
            else
            {
                if (nameOfActionChanges != "Move")
                {
                    Point centerRegion = new Point();
                    if (isTransformSelectionEnabled)
                    {
                        if (lineSelection != null)
                            pictureBoxFront.Cursor = lineSelection.isMouseUnderRegionSel(mouseCurrentPosition, out nameOfActionChanges, out centerRegion, "Front");
                    }
                    else
                    {
                        if (selection != null)
                            pictureBoxFront.Cursor = selection.isMouseUnderRegionSel(mouseCurrentPosition, out nameOfActionChanges, out  centerRegion, "Front");
                    }
                    if (Update3DPrimitiveCoords != null)
                    {
                        RectPrimitive rp = managed2D.GetSelectedPrimitive();
                        if (rp != null)
                        {
                            RectPrimitive convPr = GetConvertedPrimitiveToLocalCoords(rp);
                            if (convPr != null)
                                Update3DPrimitiveCoords(convPr, managed2D.IndexOfSelectedPrimitive);
                        }
                    }
                }
            }
        }
        private void pictureBoxRight_MouseMove(object sender, MouseEventArgs e)
        {
            mouseCurrentPosition = e.Location;
            mouseCurrentPositionSnap = CalcSnappingPoint(e.Location);

            

            if (isMousePressed)
            {
                #region WorkWithScrollers

                if (nameOfActionChanges == "Move" && pictureBoxRight.Cursor == Cursors.SizeAll && managed2D.IndexOfSelectedPrimitive == -1 && managed2D.CurrentPrimitive == null)
                {
					if ((managed2D.RedPlayer == null || !managed2D.RedPlayer.isPlayerSelected)
						&& (managed2D.BluePlayer == null || !managed2D.BluePlayer.isPlayerSelected))
					{
						if (e.X > mousePositionDown.X)
						{
							MoveHolst(panelRight, false, true);
							mousePositionDown = mouseCurrentPosition;
						}
						if (e.X < mousePositionDown.X)
						{
							MoveHolst(panelRight, true, true);
							mousePositionDown = mouseCurrentPosition;
						}
						if (e.Y > mousePositionDown.Y)
						{
							MoveHolst(panelRight, false, false);
							mousePositionDown = mouseCurrentPosition;
						}
						if (e.Y < mousePositionDown.Y)
						{
							MoveHolst(panelRight, true, false);
							mousePositionDown = mouseCurrentPosition;
						}
					}
                }
                else
                    ComputeScrolling("Right", e.Location);

                #endregion

                if (nameOfActionChanges == "None")
                {
                    if (!isSnapEnabled)
                    {
                        if (isRectEnabled)
                            managed2D.CreateRectRight(mousePositionDown, mouseCurrentPosition, grid2D.mainSizeGrid);
                    }
                    else
                    {
                        if (isRectEnabled)
                            managed2D.CreateRectRight(mousePositionDownSnap, mouseCurrentPositionSnap, grid2D.mainSizeGrid);
                    }
                }
                else
                {
                    if (nameOfActionChanges == "Transform" && pictureBoxRight.Cursor == Cursors.NoMove2D)
                    {
                        lineSelection.ChanginSelectedPointByIndex("Right", grid2D.SnapingPoint(e.Location));

                        if (managed2D.CurrentPrimitive != null)
                        {
                            managed2D.CurrentPrimitive.IsTransformed = true;
                            textureSettings.convertedPrimitive = GetConvertedPrimitiveToLocalCoords(managed2D.CurrentPrimitive);
                        }
                        else
                        {
                            foreach (int key in managed2D.List2DPrimitives.Keys)
                            {
                                RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];
                                if (rp.IsSelected)
                                {
                                    rp.IsTransformed = true;
                                    textureSettings.convertedPrimitive = GetConvertedPrimitiveToLocalCoords(rp);
                                }
                            }
                        }
                    }
                    else
                    {
                        ExecuteComandCursors("Right");

						if (nameOfActionChanges == "Move" && pictureBoxRight.Cursor == Cursors.SizeAll)
						{
							if (managed2D.BluePlayer != null)
								MoovePlayer(managed2D.BluePlayer, "Right");

							if (managed2D.RedPlayer != null)
								MoovePlayer(managed2D.RedPlayer, "Right");
						}
                    }

                    if (Update3DPrimitiveCoords != null)
                    {
                        RectPrimitive rp = managed2D.GetSelectedPrimitive();
                        if (rp != null)
                        {
                            RectPrimitive convPr = GetConvertedPrimitiveToLocalCoords(rp);
                            if (convPr != null)
                                Update3DPrimitiveCoords(convPr, managed2D.IndexOfSelectedPrimitive);
                        }
                    }

                }
                
            }
            else
            {
                if (nameOfActionChanges != "Move")
                {
                    Point centerRegion = new Point();
                    if (isTransformSelectionEnabled)
                    {
                        if (lineSelection != null)
                            pictureBoxRight.Cursor = lineSelection.isMouseUnderRegionSel(mouseCurrentPosition, out nameOfActionChanges, out centerRegion, "Right");
                    }
                    else
                    {
                        if (selection != null)
                            pictureBoxRight.Cursor = selection.isMouseUnderRegionSel(mouseCurrentPosition, out nameOfActionChanges, out  centerRegion, "Right");
                    }
                }
                if (Update3DPrimitiveCoords != null)
                {
                    RectPrimitive rp = managed2D.GetSelectedPrimitive();
                    if (rp != null)
                    {
                        RectPrimitive convPr = GetConvertedPrimitiveToLocalCoords(rp);
                        if (convPr != null)
                            Update3DPrimitiveCoords(convPr, managed2D.IndexOfSelectedPrimitive);
                    }
                }
            }
        }
        #endregion
        #region FunctionsMouseUp
        private void pictureBoxTop_MouseUp(object sender, MouseEventArgs e)
        {
            mousePositionUp = e.Location;
            mousePositionUpSnap = CalcSnappingPoint(e.Location);
			isMousePressed = false; 
        }

        private void pictureBoxFront_MouseUp(object sender, MouseEventArgs e)
        {
            mousePositionUp = e.Location;
            mousePositionUpSnap = CalcSnappingPoint(e.Location);
            isMousePressed = false;
        }

        private void pictureBoxRight_MouseUp(object sender, MouseEventArgs e)
        {
            mousePositionUp = e.Location;
            mousePositionUpSnap = CalcSnappingPoint(e.Location);
            isMousePressed = false;
        }
        #endregion
		#region EventsAllButtons
		private void checkBoxSnap_CheckedChanged(object sender, EventArgs e)
        {
            isSnapEnabled = isSnapEnabled ? false : true;
            panel11.Focus();
        }
        private void checkBoxRect_CheckedChanged(object sender, EventArgs e)
        {
            isRectEnabled = isRectEnabled ? false : true;
            if (isRectEnabled)
            {
                if (checkBoxPointer.Checked)
                {
					//UncheckedAllCheckFuncButtons();
					//checkBoxPointer.Checked = true;
					

                    if (selection != null)
                        selection = null;
                    if (lineSelection != null)
                        lineSelection = null;
                    if (managed2D.IndexOfSelectedPrimitive != -1)
                        managed2D.IndexOfSelectedPrimitive = -1;
                    DeselectAllPrimitives(-1);
					
					
                }
				isTransformSelectionEnabled = false;
				checkBoxSelectionMode.Checked = false;


				checkBoxPointer.Checked = false;
				checkBoxPlayerRed.Checked = false;
				checkBoxPlayerBlue.Checked = false;
            }
            panel11.Focus();
        }
        private void checkBoxSelectionMode_CheckedChanged(object sender, EventArgs e)
        {
            isTransformSelectionEnabled = isTransformSelectionEnabled ? false : true;
            if (managed2D.CurrentPrimitive != null)
                lineSelection = new LinesSelection(managed2D.CurrentPrimitive);
            else
            {
				if (managed2D.CurrentPrimitive == null && selection == null)
				{
					isTransformSelectionEnabled = false;
					checkBoxSelectionMode.Checked = false;
				}
                if (managed2D.List2DPrimitives.Count != 0)
                {
                    foreach (int key in managed2D.List2DPrimitives.Keys)
                    {
                        RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];
                        if (rp.IsSelected)
                        {
                            lineSelection = new LinesSelection(rp);
                            break;
                        }
                    }
                }
            }
            //selection = isTransformSelectionEnabled ? null : null;

            //if (checkBoxSelectionMode.Checked)
            //{
            //    checkBoxRect.Checked = false;
            //    checkBoxPointer.Checked = false;
            //}

            panel11.Focus();
        }
        private void checkBoxPointer_CheckedChanged(object sender, EventArgs e)
        {
			if (checkBoxPointer.Checked)
			{
				//UncheckedAllCheckFuncButtons();
				checkBoxRect.Checked = false;
				checkBoxPlayerRed.Checked = false;
				checkBoxPlayerBlue.Checked = false;

				#region CallPressEscape
				managed2D.CurrentPrimitive = null;

				if (lineSelection != null)
					lineSelection = null;

				if (selection != null)
					selection = null;

				checkBoxSelectionMode.Checked = false;

				if (managed2D.IndexOfSelectedPrimitive != -1)
				{
					if (((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]) != null)
						((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).IsSelected = false;

				}
				#endregion
			}
            panel11.Focus();
        }
        private void checkBoxTextures_CheckedChanged(object sender, EventArgs e)
        {
			if (selection != null || lineSelection != null)
			{
				if (checkBoxTextures.Checked)
				{
					
					textureSettings.radioButtonNone.Checked = true;
					textureSettings.radioButtonScale.Checked = true;
					textureSettings.Deactivate += new EventHandler(textureSettings_Deactivate);
					textureSettings.FormClosed += new FormClosedEventHandler(textureSettings_FormClosed);
					//textureSettings.TextureFormKeyDown += new TextureFormKeyDownDelegate(textureSettings_TextureFormKeyDown);

					textureSettings.Show();
					textureSettings.Focus();
				}
				else
				{
					textureSettings.Hide();
					panel11.Focus();
				}
			}
			else
			{
				checkBoxTextures.Checked = false;
				panel11.Focus();
			}
        }
        private void checkBoxPlayer_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPlayerRed.Checked)
            {
                checkBoxPlayerBlue.Checked = false;
                checkBoxPointer.Checked = false;
                checkBoxRect.Checked = false;

				if (managed2D.BluePlayer != null)
				{
					managed2D.BluePlayer.isPlayerSelected = false;
				}
            }
            else
            {
            }
			panel11.Focus();
        }
        private void checkBoxPlayerBlue_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxPlayerBlue.Checked)
            {
                checkBoxPlayerRed.Checked = false;
                checkBoxPointer.Checked = false;
                checkBoxRect.Checked = false;

				if (managed2D.RedPlayer != null)
				{
					managed2D.RedPlayer.isPlayerSelected = false;
				}
            }
            else
            {

            }
			panel11.Focus();
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            HollowObject();
            panel11.Focus();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CurveObject();
            panel11.Focus();
        }


        #region GridPlus_Minus
        private void button1_Click(object sender, EventArgs e)
        {
            if (grid2D.sizeGrid > 0)
            {
                grid2D.sizeGrid--;
                if (grid2D.sizeGrid == 0)
                    buttonGridMinus.Enabled = false;
                else
                    buttonGridMinus.Enabled = true;

                buttonGridPlus.Enabled = true;
                int s = 4;
                for (int i = 0; i < grid2D.sizeGrid; i++, s *= 2) ;
                labelGridSize.Text = s.ToString();
                grid2D.mainSizeGrid = s;
            }
            panel1.Focus();
        }

        private void buttonGridPrus_Click(object sender, EventArgs e)
        {
            if (grid2D.sizeGrid < grid2D.img.Length)
            {
                grid2D.sizeGrid++;
                if (grid2D.sizeGrid == grid2D.img.Length - 1)
                    buttonGridPlus.Enabled = false;
                else
                    buttonGridPlus.Enabled = true;

                buttonGridMinus.Enabled = true;

                int s = 4;
                for (int i = 0; i < grid2D.sizeGrid; i++, s *= 2) ;
                labelGridSize.Text = s.ToString();
                grid2D.mainSizeGrid = s;
            }
            panel1.Focus();
        }
        
        #endregion
        #endregion
        #region EventsKeys
        void textureSettings_TextureFormKeyDown(object sender, KeyEventArgs e)
        {
            Form1_KeyUp(sender, e);
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.KeyCode == Keys.Space)
            {
                pictureBoxFront.Cursor = Cursors.SizeAll;
                pictureBoxRight.Cursor = Cursors.SizeAll;
                pictureBoxTop.Cursor = Cursors.SizeAll;

                nameOfActionChanges = "Move";
            }
            
            if (e.Control)
            {
                if (e.KeyCode == Keys.C)
                {
                    foreach (int key in managed2D.List2DPrimitives.Keys)
                    {
                        RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];
                        if (rp.IsSelected)
                        {
                            //rp.IsSelected = false;
                            managed2D.CopyPrimitive = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());
							managed2D.CopyPrimitive.IsSelected = false;
							managed2D.CopyPrimitive.IsTransformed = rp.IsTransformed;
                            for (int i = 0; i < 10; i++)
                            {
                                managed2D.CopyPrimitive.MasLinesFront[i] = rp.MasLinesFront[i];

                                managed2D.CopyPrimitive.MasLinesRight[i] = rp.MasLinesRight[i];

                                managed2D.CopyPrimitive.MasLinesTop[i] = rp.MasLinesTop[i];
                            }
                            break;
                        }
                    }
                }
                if (e.KeyCode == Keys.V)
                {
					RectPrimitive r = SlidePositionPrimitive(managed2D.CopyPrimitive);
					//RectPrimitive r = managed2D.CopyPrimitive;
                    if (r != null)
                    {
                        //int i = managed2D.List2DPrimitives.Add((void)managed2D.GetNextKey(), r);
                        int k = managed2D.AddPrimitiveToList(r);

						RectPrimitive rpj = managed2D.CopyPrimitive;
						managed2D.CopyPrimitive = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());
						for (int ix = 0; ix < 10; ix++)
						{
							managed2D.CopyPrimitive.MasLinesFront[ix] = rpj.MasLinesFront[ix];

							managed2D.CopyPrimitive.MasLinesRight[ix] = rpj.MasLinesRight[ix];

							managed2D.CopyPrimitive.MasLinesTop[ix] = rpj.MasLinesTop[ix];
						}

						managed2D.ConvertedPrimitive = GetConvertedPrimitiveToLocalCoords(r);
						textureSettings.convertedPrimitive = managed2D.ConvertedPrimitive;
						managed3D.AddObject(managed2D.ConvertedPrimitive, "Textures\\" + lastTexture, k);
                    }
                }
				if (e.KeyCode == Keys.O)
				{
					OpenMapFromFile();
				}
				if (e.KeyCode == Keys.S)
				{
					saveMapToFile();
				}
				if (e.KeyCode == Keys.N)
				{
					managed2D.Clear();
					managed3D.Clear();
					textureSettings.convertedPrimitive = null;
				}
            }
            else
            {
                if (e.KeyCode == Keys.T)
                {
                    //isTransformSelectionEnabled = isTransformSelectionEnabled ? false : true;

                    checkBoxSelectionMode.Checked = checkBoxSelectionMode.Checked ? false : true;

                    if (managed2D.CurrentPrimitive != null)
                        lineSelection = new LinesSelection(managed2D.CurrentPrimitive);
                    else
                    {
                        if (managed2D.List2DPrimitives.Count != 0)
                        {
                            foreach (int key in managed2D.List2DPrimitives.Keys)
                            {
                                RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];
                                if (rp.IsSelected)
                                {
                                    lineSelection = new LinesSelection(rp);
                                    break;
                                }
                            }
                        }
                    }
                    selection = null;
                }
                if (e.KeyCode == Keys.N)
                {
                    if (checkBoxTextures.Checked)
                        checkBoxTextures.Checked = false;
                    else
                        checkBoxTextures.Checked = true;
                }
                if (e.KeyCode == Keys.P)
                {
                    if (checkBoxPointer.Checked)
                        checkBoxPointer.Checked = false;
                    else
                        checkBoxPointer.Checked = true;
                }
                if (e.KeyCode == Keys.B)
                {
                    if (checkBoxRect.Checked)
                        checkBoxRect.Checked = false;
                    else
                    {
                        checkBoxRect.Checked = true;
                        checkBoxPointer.Checked = false;
                    }
                }
				if (e.KeyCode == Keys.U)
				{
					if (checkBoxPlayerBlue.Enabled)
					{
						if (managed2D.BluePlayer == null)
						{
							UncheckedAllCheckFuncButtons();
							checkBoxPlayerBlue.Checked = true;
							//checkBoxPlayerBlue_CheckedChanged(null, null);
							
						}
					}
				}
				if (e.KeyCode == Keys.I)
				{
					if (checkBoxPlayerRed.Enabled)
					{
						if (managed2D.RedPlayer == null)
						{
							UncheckedAllCheckFuncButtons();
							checkBoxPlayerRed.Checked = true;
							//checkBoxPlayer_CheckedChanged(null, null);
							
						}
					}
				}

				if (e.KeyCode == Keys.OemOpenBrackets)
				{
					button1_Click(null, null);
				}
				if (e.KeyCode == Keys.OemCloseBrackets)
				{
					buttonGridPrus_Click(null, null);
				}

				
            }
        }
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (managed2D.CurrentPrimitive != null)
                {
                    managed2D.CurrentPrimitive.IsSelected = false;
                    managed2D.ConvertedPrimitive = GetConvertedPrimitiveToLocalCoords(managed2D.CurrentPrimitive);
                    //int i = managed2D.List2DPrimitives.Add(managed2D.CurrentPrimitive);
                    int k = managed2D.AddCurrentPrimitiveToList();
                    managed3D.AddObject(managed2D.ConvertedPrimitive, "Textures\\" + lastTexture, k);
                    managed2D.CurrentPrimitive = null;

                    if (lineSelection != null)
                        lineSelection = null;

                    if (selection != null)
                        selection = null;

					managed3D.selectedIndex = -1;
					managed2D.IndexOfSelectedPrimitive = -1;
                }
                else
                {
                    if (managed2D.IndexOfSelectedPrimitive != -1)
                    {
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).IsSelected = false;
						managed3D.selectedIndex = -1;
						managed2D.IndexOfSelectedPrimitive = -1;
                    }
                }
                //isTransformSelectionEnabled = false;
                checkBoxSelectionMode.Checked = false;
            }
            if (e.KeyCode == Keys.Escape)
            {
                managed2D.CurrentPrimitive = null;

                if (lineSelection != null)
                    lineSelection = null;

                if (selection != null)
                    selection = null;

                checkBoxSelectionMode.Checked = false;

                if (managed2D.IndexOfSelectedPrimitive != -1)
                {
                    if (((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]) != null)
                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).IsSelected = false;

                }
				CheckSelectedPlayerAndDelete();
            }
            if (e.KeyCode == Keys.Space)
            {
                pictureBoxFront.Cursor = Cursors.Cross;
                pictureBoxRight.Cursor = Cursors.Cross;
                pictureBoxTop.Cursor = Cursors.Cross;

                nameOfActionChanges = "None";
            }
            
            if (e.KeyCode == Keys.Delete)
            {
                foreach (int key in managed2D.List2DPrimitives.Keys)
                {
                    RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];
                    if(rp.IsSelected)
                    {
                        managed3D.objectsList.Remove(key);
                        managed2D.List2DPrimitives.Remove(key);
                        managed3D.selectedIndex = -1;
                        managed2D.IndexOfSelectedPrimitive = -1;
                        break;
                    }
                }

				CheckSelectedPlayerAndDelete();
            }
            if(e.KeyCode == Keys.H)
            {
                HollowObject();
            }
			if (e.KeyCode == Keys.Q)
			{
				managed3D.modeView3D = managed3D.modeView3D ? false : true;
				if (managed3D.modeView3D)
				{
					if(!checkBoxPointer.Checked)
						checkBoxPointer.Checked = true;
					Cursor.Hide();
					timerRight.Enabled = timerTop.Enabled = timerFront.Enabled = false;
				}
				else
				{
					Cursor.Show();
					timerRight.Enabled = timerTop.Enabled = timerFront.Enabled = true;
				}
			}
        }
        #endregion
        #region OtherFunctions

		private void CheckSelectedPlayerAndDelete()
		{
			if (managed2D.RedPlayer != null)
			{
				if (managed2D.RedPlayer.isPlayerSelected)
				{
					managed2D.RedPlayer = null;
					managed3D.playerRed = null;
					checkBoxPlayerRed.Checked = false;
					checkBoxPlayerRed.Enabled = true;
				}
			}

			if (managed2D.BluePlayer != null)
			{
				if (managed2D.BluePlayer.isPlayerSelected)
				{
					managed2D.BluePlayer = null;
					managed3D.playerBlue = null;
					checkBoxPlayerBlue.Checked = false;
					checkBoxPlayerBlue.Enabled = true;
				}
			}
		}
        private void ExecuteComandCursors(string nameWindow)
        {

            Point[] lines = GetMasLinesForNamePrimitive(nameWindow);
            if (lines == null)
                return;
            Point mouseBegin = GetMousePointForSnapping("Begin");
            Point mouseCur = GetMousePointForSnapping("Current");


            if (GetWindowForName(nameWindow).Cursor == Cursors.SizeNWSE)//влево - вверх, вправо - вниз
            {
                if (nameOfActionChanges == "LeftUp")
                {
                    lines[0].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[0].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[4].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[4].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[5].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[5].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[9].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[9].Y -= (mouseCur.Y - mouseBegin.Y) * -1;

                    lines[1].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[6].Y -= (mouseCur.Y - mouseBegin.Y) * -1;

                    lines[3].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[8].X -= (mouseCur.X - mouseBegin.X) * -1;
                }
                if (nameOfActionChanges == "RightDown")
                {
                    lines[2].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[2].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[7].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[7].Y -= (mouseCur.Y - mouseBegin.Y) * -1;

                    lines[1].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[6].X -= (mouseCur.X - mouseBegin.X) * -1;

                    lines[3].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[8].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                }
            }
            if (GetWindowForName(nameWindow).Cursor == Cursors.SizeWE)//вправо - влево
            {
                if (nameOfActionChanges == "CenterRight")
                {
                    lines[1].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[2].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[6].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[7].X -= (mouseCur.X - mouseBegin.X) * -1;
                }
                if (nameOfActionChanges == "CenterLeft")
                {
                    lines[0].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[4].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[5].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[9].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[3].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[8].X -= (mouseCur.X - mouseBegin.X) * -1;
                }
            }
            if (GetWindowForName(nameWindow).Cursor == Cursors.SizeNS)//вверх - вниз
            {
                if (nameOfActionChanges == "CenterUp")
                {
                    lines[0].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[4].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[5].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[9].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[1].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[6].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                }
                if (nameOfActionChanges == "CenterDown")
                {
                    lines[2].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[7].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[3].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[8].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                }

            }
            if (GetWindowForName(nameWindow).Cursor == Cursors.SizeNESW)//вправо - вверх,влево - вниз
            {
                if (nameOfActionChanges == "RightUp")
                {
                    lines[1].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[1].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[6].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[6].Y -= (mouseCur.Y - mouseBegin.Y) * -1;

                    lines[0].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[4].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[5].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[9].Y -= (mouseCur.Y - mouseBegin.Y) * -1;

                    lines[2].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[7].X -= (mouseCur.X - mouseBegin.X) * -1;
                }
                if (nameOfActionChanges == "LeftDown")
                {
                    lines[3].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[3].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[8].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[8].Y -= (mouseCur.Y - mouseBegin.Y) * -1;

                    lines[0].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[4].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[5].X -= (mouseCur.X - mouseBegin.X) * -1;
                    lines[9].X -= (mouseCur.X - mouseBegin.X) * -1;

                    lines[2].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                    lines[7].Y -= (mouseCur.Y - mouseBegin.Y) * -1;
                }
            }

            if (GetWindowForName(nameWindow).Cursor == Cursors.SizeAll)
            {
                if (nameOfActionChanges == "Move")
                {
                    if(nameWindow == "Front")
                    {
                        if (managed2D.CurrentPrimitive != null)
                            MoovePrimitive(true, "Front");
                        else
                            MoovePrimitive(false, "Front");

						
                    }
                    if (nameWindow == "Right")
                    {
                        if (managed2D.CurrentPrimitive != null)
                            MoovePrimitive(true, "Right");
                        else
                            MoovePrimitive(false, "Right");

						if (managed2D.BluePlayer != null)
							MoovePlayer(managed2D.BluePlayer, "Right");

						if (managed2D.RedPlayer != null)
							MoovePlayer(managed2D.RedPlayer, "Right");
                    }
                    if (nameWindow == "Top")
                    {
                        if (managed2D.CurrentPrimitive != null)
                            MoovePrimitive(true, "Top");
                        else
                            MoovePrimitive(false, "Top");

						if (managed2D.BluePlayer != null)
							MoovePlayer(managed2D.BluePlayer, "Top");

						if (managed2D.RedPlayer != null)
							MoovePlayer(managed2D.RedPlayer, "Top");
                    }
                }
            }

            if (isSnapEnabled)
            {
                mousePositionDownSnap = mouseCurrentPositionSnap;
            }
            else
            {
                mousePositionDown = mouseCurrentPosition;
            }

            if (nameWindow == "Front")
            {
                if (managed2D.CurrentPrimitive != null)
                {
                    managed2D.CurrentPrimitive.MasLinesFront = lines;

                    #region ProjectionY

                    #region LeftTop

                    managed2D.CurrentPrimitive.MasLinesRight[0].Y = managed2D.CurrentPrimitive.MasLinesFront[1].Y;
                    managed2D.CurrentPrimitive.MasLinesRight[4].Y = managed2D.CurrentPrimitive.MasLinesFront[1].Y;
                    managed2D.CurrentPrimitive.MasLinesRight[5].Y = managed2D.CurrentPrimitive.MasLinesFront[0].Y;
                    managed2D.CurrentPrimitive.MasLinesRight[9].Y = managed2D.CurrentPrimitive.MasLinesFront[0].Y;

                    #endregion

                    #region RightTop

                    managed2D.CurrentPrimitive.MasLinesRight[1].Y = managed2D.CurrentPrimitive.MasLinesFront[6].Y;
                    managed2D.CurrentPrimitive.MasLinesRight[6].Y = managed2D.CurrentPrimitive.MasLinesFront[5].Y;

                    #endregion

                    #region RightBottom

                    managed2D.CurrentPrimitive.MasLinesRight[2].Y = managed2D.CurrentPrimitive.MasLinesFront[7].Y;
                    managed2D.CurrentPrimitive.MasLinesRight[7].Y = managed2D.CurrentPrimitive.MasLinesFront[8].Y;

                    #endregion

                    #region leftBottom

                    managed2D.CurrentPrimitive.MasLinesRight[3].Y = managed2D.CurrentPrimitive.MasLinesFront[2].Y;
                    managed2D.CurrentPrimitive.MasLinesRight[8].Y = managed2D.CurrentPrimitive.MasLinesFront[3].Y;


                    #endregion

                    #endregion

                    #region ProjectionZ

                    #region LeftTop

                    managed2D.CurrentPrimitive.MasLinesTop[0].X = managed2D.CurrentPrimitive.MasLinesFront[5].X;
                    managed2D.CurrentPrimitive.MasLinesTop[4].X = managed2D.CurrentPrimitive.MasLinesFront[5].X;
                    managed2D.CurrentPrimitive.MasLinesTop[5].X = managed2D.CurrentPrimitive.MasLinesFront[8].X;
                    managed2D.CurrentPrimitive.MasLinesTop[9].X = managed2D.CurrentPrimitive.MasLinesFront[8].X;


                    #endregion

                    #region RightTop

                    managed2D.CurrentPrimitive.MasLinesTop[1].X = managed2D.CurrentPrimitive.MasLinesFront[6].X;
                    managed2D.CurrentPrimitive.MasLinesTop[6].X = managed2D.CurrentPrimitive.MasLinesFront[7].X;


                    #endregion

                    #region RightBottom

                    managed2D.CurrentPrimitive.MasLinesTop[2].X = managed2D.CurrentPrimitive.MasLinesFront[1].X;
                    managed2D.CurrentPrimitive.MasLinesTop[7].X = managed2D.CurrentPrimitive.MasLinesFront[2].X;


                    #endregion

                    #region leftBottom

                    managed2D.CurrentPrimitive.MasLinesTop[3].X = managed2D.CurrentPrimitive.MasLinesFront[0].X;
                    managed2D.CurrentPrimitive.MasLinesTop[8].X = managed2D.CurrentPrimitive.MasLinesFront[3].X;


                    #endregion

                    #endregion
                }
                else
                {
                    if (managed2D.IndexOfSelectedPrimitive != -1)
                    {
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront = lines;

                        #region ProjectionY

                        #region LeftTop

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[0].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[1].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[4].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[1].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[5].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[0].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[9].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[0].Y;

                        #endregion

                        #region RightTop

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[1].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[6].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[6].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[5].Y;

                        #endregion

                        #region RightBottom

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[2].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[7].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[7].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[8].Y;

                        #endregion

                        #region leftBottom

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[3].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[2].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[8].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[3].Y;


                        #endregion

                        #endregion

                        #region ProjectionZ

                        #region LeftTop

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[0].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[5].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[4].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[5].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[5].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[8].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[9].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[8].X;


                        #endregion

                        #region RightTop

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[1].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[6].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[6].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[7].X;


                        #endregion

                        #region RightBottom

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[2].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[1].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[7].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[2].X;


                        #endregion

                        #region leftBottom

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[3].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[0].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[8].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[3].X;


                        #endregion

                        #endregion
                    }
                }
            }

            if (nameWindow == "Right")
            {
                if (managed2D.CurrentPrimitive != null)
                {
                    managed2D.CurrentPrimitive.MasLinesRight = lines;

                    #region ProjectionX

                    #region LeftTop

                    managed2D.CurrentPrimitive.MasLinesFront[0].Y = managed2D.CurrentPrimitive.MasLinesRight[5].Y;
                    managed2D.CurrentPrimitive.MasLinesFront[4].Y = managed2D.CurrentPrimitive.MasLinesRight[5].Y;
                    managed2D.CurrentPrimitive.MasLinesFront[5].Y = managed2D.CurrentPrimitive.MasLinesRight[6].Y;
                    managed2D.CurrentPrimitive.MasLinesFront[9].Y = managed2D.CurrentPrimitive.MasLinesRight[6].Y;

                    #endregion

                    #region RightTop

                    managed2D.CurrentPrimitive.MasLinesFront[1].Y = managed2D.CurrentPrimitive.MasLinesRight[0].Y;
                    managed2D.CurrentPrimitive.MasLinesFront[6].Y = managed2D.CurrentPrimitive.MasLinesRight[1].Y;

                    #endregion

                    #region RightBottom

                    managed2D.CurrentPrimitive.MasLinesFront[2].Y = managed2D.CurrentPrimitive.MasLinesRight[3].Y;
                    managed2D.CurrentPrimitive.MasLinesFront[7].Y = managed2D.CurrentPrimitive.MasLinesRight[2].Y;

                    #endregion

                    #region leftBottom

                    managed2D.CurrentPrimitive.MasLinesFront[3].Y = managed2D.CurrentPrimitive.MasLinesRight[8].Y;
                    managed2D.CurrentPrimitive.MasLinesFront[8].Y = managed2D.CurrentPrimitive.MasLinesRight[7].Y;

                    #endregion

                    #endregion

                    #region ProjectionZ

                    #region LeftTop

                    managed2D.CurrentPrimitive.MasLinesTop[0].Y = panelHeight - managed2D.CurrentPrimitive.MasLinesRight[6].X;
                    managed2D.CurrentPrimitive.MasLinesTop[4].Y = panelHeight - managed2D.CurrentPrimitive.MasLinesRight[6].X;
                    managed2D.CurrentPrimitive.MasLinesTop[5].Y = panelHeight - managed2D.CurrentPrimitive.MasLinesRight[7].X;
                    managed2D.CurrentPrimitive.MasLinesTop[9].Y = panelHeight - managed2D.CurrentPrimitive.MasLinesRight[7].X;

                    #endregion

                    #region RightTop

                    managed2D.CurrentPrimitive.MasLinesTop[1].Y = panelHeight - managed2D.CurrentPrimitive.MasLinesRight[1].X;
                    managed2D.CurrentPrimitive.MasLinesTop[6].Y = panelHeight - managed2D.CurrentPrimitive.MasLinesRight[2].X;

                    #endregion

                    #region RightBottom

                    managed2D.CurrentPrimitive.MasLinesTop[2].Y = panelHeight - managed2D.CurrentPrimitive.MasLinesRight[0].X;
                    managed2D.CurrentPrimitive.MasLinesTop[7].Y = panelHeight - managed2D.CurrentPrimitive.MasLinesRight[3].X;

                    #endregion

                    #region leftBottom

                    managed2D.CurrentPrimitive.MasLinesTop[3].Y = panelHeight - managed2D.CurrentPrimitive.MasLinesRight[5].X;
                    managed2D.CurrentPrimitive.MasLinesTop[8].Y = panelHeight - managed2D.CurrentPrimitive.MasLinesRight[8].X;

                    #endregion

                    #endregion
                }
                else
                {
                    if (managed2D.IndexOfSelectedPrimitive != -1)
                    {
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight = lines;

                        #region ProjectionX

                        #region LeftTop

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[0].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[5].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[4].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[5].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[5].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[6].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[9].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[6].Y;

                        #endregion

                        #region RightTop

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[1].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[0].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[6].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[1].Y;

                        #endregion

                        #region RightBottom

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[2].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[3].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[7].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[2].Y;

                        #endregion

                        #region leftBottom

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[3].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[8].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[8].Y = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[7].Y;

                        #endregion

                        #endregion

                        #region ProjectionZ

                        #region LeftTop

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[0].Y = panelHeight - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[6].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[4].Y = panelHeight - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[6].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[5].Y = panelHeight - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[7].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[9].Y = panelHeight - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[7].X;

                        #endregion

                        #region RightTop

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[1].Y = panelHeight - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[1].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[6].Y = panelHeight - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[2].X;

                        #endregion

                        #region RightBottom

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[2].Y = panelHeight - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[0].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[7].Y = panelHeight - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[3].X;

                        #endregion

                        #region leftBottom

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[3].Y = panelHeight - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[5].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[8].Y = panelHeight - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[8].X;

                        #endregion

                        #endregion
                    }
                }
            }

            if (nameWindow == "Top")
            {
                if (managed2D.CurrentPrimitive != null)
                {
                    managed2D.CurrentPrimitive.MasLinesTop = lines;

                    #region ProjectionY

                    #region LeftTop

                    managed2D.CurrentPrimitive.MasLinesRight[0].X = panelWidth - managed2D.CurrentPrimitive.MasLinesTop[2].Y;
                    managed2D.CurrentPrimitive.MasLinesRight[4].X = panelWidth - managed2D.CurrentPrimitive.MasLinesTop[2].Y;
                    managed2D.CurrentPrimitive.MasLinesRight[5].X = panelWidth - managed2D.CurrentPrimitive.MasLinesTop[3].Y;
                    managed2D.CurrentPrimitive.MasLinesRight[9].X = panelWidth - managed2D.CurrentPrimitive.MasLinesTop[3].Y;

                    #endregion

                    #region RightTop

                    managed2D.CurrentPrimitive.MasLinesRight[1].X = panelWidth - managed2D.CurrentPrimitive.MasLinesTop[1].Y;
                    managed2D.CurrentPrimitive.MasLinesRight[6].X = panelWidth - managed2D.CurrentPrimitive.MasLinesTop[0].Y;

                    #endregion

                    #region RightBottom

                    managed2D.CurrentPrimitive.MasLinesRight[2].X = panelWidth - managed2D.CurrentPrimitive.MasLinesTop[6].Y;
                    managed2D.CurrentPrimitive.MasLinesRight[7].X = panelWidth - managed2D.CurrentPrimitive.MasLinesTop[5].Y;

                    #endregion

                    #region leftBottom

                    managed2D.CurrentPrimitive.MasLinesRight[3].X = panelWidth - managed2D.CurrentPrimitive.MasLinesTop[7].Y;
                    managed2D.CurrentPrimitive.MasLinesRight[8].X = panelWidth - managed2D.CurrentPrimitive.MasLinesTop[8].Y;

                    #endregion

                    #endregion

                    #region ProjectionX

                    #region LeftTop

                    managed2D.CurrentPrimitive.MasLinesFront[0].X = managed2D.CurrentPrimitive.MasLinesTop[3].X;
                    managed2D.CurrentPrimitive.MasLinesFront[4].X = managed2D.CurrentPrimitive.MasLinesTop[3].X;
                    managed2D.CurrentPrimitive.MasLinesFront[5].X = managed2D.CurrentPrimitive.MasLinesTop[0].X;
                    managed2D.CurrentPrimitive.MasLinesFront[9].X = managed2D.CurrentPrimitive.MasLinesTop[0].X;

                    #endregion

                    #region RightTop

                    managed2D.CurrentPrimitive.MasLinesFront[1].X = managed2D.CurrentPrimitive.MasLinesTop[2].X;
                    managed2D.CurrentPrimitive.MasLinesFront[6].X = managed2D.CurrentPrimitive.MasLinesTop[1].X;

                    #endregion

                    #region RightBottom

                    managed2D.CurrentPrimitive.MasLinesFront[2].X = managed2D.CurrentPrimitive.MasLinesTop[7].X;
                    managed2D.CurrentPrimitive.MasLinesFront[7].X = managed2D.CurrentPrimitive.MasLinesTop[6].X;

                    #endregion

                    #region leftBottom

                    managed2D.CurrentPrimitive.MasLinesFront[3].X = managed2D.CurrentPrimitive.MasLinesTop[8].X;
                    managed2D.CurrentPrimitive.MasLinesFront[8].X = managed2D.CurrentPrimitive.MasLinesTop[5].X;

                    #endregion

                    #endregion
                }
                else
                {
                    if (managed2D.IndexOfSelectedPrimitive != -1)
                    {
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop = lines;

                        #region ProjectionY

                        #region LeftTop

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[0].X = panelWidth - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[2].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[4].X = panelWidth - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[2].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[5].X = panelWidth - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[3].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[9].X = panelWidth - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[3].Y;

                        #endregion

                        #region RightTop

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[1].X = panelWidth - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[1].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[6].X = panelWidth - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[0].Y;

                        #endregion

                        #region RightBottom

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[2].X = panelWidth - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[6].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[7].X = panelWidth - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[5].Y;

                        #endregion

                        #region leftBottom

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[3].X = panelWidth - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[7].Y;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[8].X = panelWidth - ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[8].Y;

                        #endregion

                        #endregion

                        #region ProjectionX

                        #region LeftTop

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[0].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[3].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[4].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[3].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[5].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[0].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[9].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[0].X;

                        #endregion

                        #region RightTop

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[1].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[2].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[6].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[1].X;

                        #endregion

                        #region RightBottom

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[2].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[7].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[7].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[6].X;

                        #endregion

                        #region leftBottom

                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[3].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[8].X;
                        ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[8].X = ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[5].X;

                        #endregion

                        #endregion
                    }
                }
            }
            if (managed2D.CurrentPrimitive != null)
                managed2D.ConvertedPrimitive = GetConvertedPrimitiveToLocalCoords(managed2D.CurrentPrimitive);
            else
            {
                if (managed2D.IndexOfSelectedPrimitive != -1)
                {
                   managed2D.ConvertedPrimitive = GetConvertedPrimitiveToLocalCoords(((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]));
                }
            }
        }
        private Point[] GetMasLinesForNamePrimitive(string name)
        {
            if (name == "Front")
            {
                if (managed2D.CurrentPrimitive != null)
                    return managed2D.CurrentPrimitive.MasLinesFront;
                else
                {
                    if (managed2D.IndexOfSelectedPrimitive != -1)
                    {
                        return ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront;
                    }
                }
            }

            if (name == "Right")
            {
                if (managed2D.CurrentPrimitive != null)
                    return managed2D.CurrentPrimitive.MasLinesRight;
                else
                {
                    if (managed2D.IndexOfSelectedPrimitive != -1)
                    {
                        return ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight;
                    }
                }
            }

            if (name == "Top")
            {
                if (managed2D.CurrentPrimitive != null)
                    return managed2D.CurrentPrimitive.MasLinesTop;
                else
                {
                    if (managed2D.IndexOfSelectedPrimitive != -1)
                    {
                        return ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop;
                    }
                }
            }
            return null;
        }
        private Point CalcSnappingPoint(Point pt)
        {
            int s = int.Parse(labelGridSize.Text);

            int newXCoord = -panelWidth;
            int newYCoord = -panelHeight;

            if (pt.X > panelWidth / 2)
            {
                int t = pt.X - panelWidth / 2;
                t /= s;
                newXCoord = panelWidth / 2 + (t * s);
            }
            else
            {
                int t = panelWidth / 2 - pt.X;
                t /= s;
                newXCoord = panelWidth / 2 - (t * s);

            }

            if (pt.Y > panelWidth / 2)
            {
                int t = pt.Y - panelWidth / 2;
                t /= s;
                newYCoord = (panelWidth / 2 + (t * s));
            }
            else
            {
                int t = panelWidth / 2 - pt.Y;
                t /= s;

                newYCoord = (panelWidth / 2 - (t * s));

            }

            return new Point(newXCoord, newYCoord);
        }
        private Point GetMousePointForSnapping(string namePoint)
        {
            Point p = new Point(0, 0);

            if (isSnapEnabled)
            {
                if (namePoint == "Begin")
                    p = mousePositionDownSnap;

                if (namePoint == "End")
                    p = mousePositionUpSnap;

                if (namePoint == "Current")
                    p = mouseCurrentPositionSnap;
            }
            else
            {
                if (namePoint == "Begin")
                    p = mousePositionDown;

                if (namePoint == "End")
                    p = mousePositionUp;

                if (namePoint == "Current")
                    p = mouseCurrentPosition;
            }

            return p;
        }
        private PictureBox GetWindowForName(string nameWindow)
        {
            PictureBox p = null;

            if (nameWindow == "Front")
                return pictureBoxFront;

            if (nameWindow == "Right")
                return pictureBoxRight;

            if (nameWindow == "Top")
                return pictureBoxTop;

            return p;
        }
		private void MoovePlayer(Player p, string name2DProjection)
		{
			if (p != null)
			{
				if (p.isPlayerSelected)
				{
					if (name2DProjection == "Front")
					{
						if (isSnapEnabled)
						{
							p.rectFront.X += mouseCurrentPositionSnap.X - mousePositionDownSnap.X;
							p.rectFront.Y += mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y;

							p.rectTop.X += mouseCurrentPositionSnap.X - mousePositionDownSnap.X;
							p.rectRight.Y += mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y;
						}
						else
						{
							p.rectFront.X += mouseCurrentPosition.X - mousePositionDown.X;
							p.rectFront.Y += mouseCurrentPosition.Y - mousePositionDown.Y;

							p.rectTop.X += mouseCurrentPosition.X - mousePositionDown.X;
							p.rectRight.Y += mouseCurrentPosition.Y - mousePositionDown.Y;
						}
					}
					if (name2DProjection == "Right")
					{
						if (isSnapEnabled)
						{
							p.rectRight.X += mouseCurrentPositionSnap.X - mousePositionDownSnap.X;
							p.rectRight.Y += mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y;

							p.rectTop.Y += -1 * (mouseCurrentPositionSnap.X - mousePositionDownSnap.X);
							p.rectFront.Y += mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y;
						}
						else
						{
							p.rectRight.X += mouseCurrentPosition.X - mousePositionDown.X;
							p.rectRight.Y += mouseCurrentPosition.Y - mousePositionDown.Y;

							p.rectTop.Y += -1 * (mouseCurrentPosition.X - mousePositionDown.X);
							p.rectFront.Y += mouseCurrentPosition.Y - mousePositionDown.Y;
						}
					}
					if (name2DProjection == "Top")
					{
						if (isSnapEnabled)
						{
							p.rectTop.X += mouseCurrentPositionSnap.X - mousePositionDownSnap.X;
							p.rectTop.Y += mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y;

							p.rectFront.X += mouseCurrentPositionSnap.X - mousePositionDownSnap.X;
							p.rectRight.X += -1 * (mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y);
						}
						else
						{
							p.rectTop.X += mouseCurrentPosition.X - mousePositionDown.X;
							p.rectTop.Y += mouseCurrentPosition.Y - mousePositionDown.Y;

							p.rectFront.X += mouseCurrentPosition.X - mousePositionDown.X;
							p.rectRight.X += -1 * (mouseCurrentPosition.Y - mousePositionDown.Y);
						}
					}
					mousePositionDown = mouseCurrentPosition;
					mousePositionDownSnap = mouseCurrentPositionSnap;
					p.ReCreateBuffer(managed3D.device,grid2D);
				}
			}
		}
        private void MoovePrimitive(bool isCurrentPrimitive, string name2DProjection)
        {
            if (isCurrentPrimitive)
            {
                #region CurPrimitive

                if (isSnapEnabled)
                {
                    #region SnapEnabled

                    for (int i = 0; i < 10; i++)
                    {
                        if (name2DProjection == "Front")
                        {
                            managed2D.CurrentPrimitive.MasLinesFront[i].X += mouseCurrentPositionSnap.X - mousePositionDownSnap.X;
                            managed2D.CurrentPrimitive.MasLinesFront[i].Y += mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y;

                            managed2D.CurrentPrimitive.MasLinesRight[i].Y += mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y;
                            managed2D.CurrentPrimitive.MasLinesTop[i].X += mouseCurrentPositionSnap.X - mousePositionDownSnap.X;
                        }
                        if (name2DProjection == "Right")
                        {
                            managed2D.CurrentPrimitive.MasLinesRight[i].X += mouseCurrentPositionSnap.X - mousePositionDownSnap.X;
                            managed2D.CurrentPrimitive.MasLinesRight[i].Y += mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y;

                            managed2D.CurrentPrimitive.MasLinesFront[i].Y += mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y;
                            managed2D.CurrentPrimitive.MasLinesTop[i].Y += (mouseCurrentPositionSnap.X - mousePositionDownSnap.X) * -1;
                        }
                        if (name2DProjection == "Top")
                        {
                            managed2D.CurrentPrimitive.MasLinesTop[i].X += mouseCurrentPositionSnap.X - mousePositionDownSnap.X;
                            managed2D.CurrentPrimitive.MasLinesTop[i].Y += mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y;

                            managed2D.CurrentPrimitive.MasLinesFront[i].X += mouseCurrentPositionSnap.X - mousePositionDownSnap.X;
                            managed2D.CurrentPrimitive.MasLinesRight[i].X += (mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y) * -1;
                        }
                    }
                    if (isTransformSelectionEnabled)
                        lineSelection = new LinesSelection(managed2D.CurrentPrimitive);
                    else
                        selection = new SimpleSelection(managed2D.CurrentPrimitive);

					
                    #endregion
                }
                else
                {
                    #region SnapDisabled

                    for (int i = 0; i < 10; i++)
                    {
                        if (name2DProjection == "Front")
                        {
                            managed2D.CurrentPrimitive.MasLinesFront[i].X += mouseCurrentPosition.X - mousePositionDown.X;
                            managed2D.CurrentPrimitive.MasLinesFront[i].Y += mouseCurrentPosition.Y - mousePositionDown.Y;

                            managed2D.CurrentPrimitive.MasLinesRight[i].Y += mouseCurrentPosition.Y - mousePositionDown.Y;
                            managed2D.CurrentPrimitive.MasLinesTop[i].X += mouseCurrentPosition.X - mousePositionDown.X;
                        }
                        if (name2DProjection == "Right")
                        {
                            managed2D.CurrentPrimitive.MasLinesRight[i].X += mouseCurrentPosition.X - mousePositionDown.X;
                            managed2D.CurrentPrimitive.MasLinesRight[i].Y += mouseCurrentPosition.Y - mousePositionDown.Y;

                            managed2D.CurrentPrimitive.MasLinesFront[i].Y += mouseCurrentPosition.Y - mousePositionDown.Y;
                            managed2D.CurrentPrimitive.MasLinesTop[i].Y += (mouseCurrentPosition.X - mousePositionDown.X) * -1;
                        }
                        if (name2DProjection == "Top")
                        {
                            managed2D.CurrentPrimitive.MasLinesTop[i].X += mouseCurrentPosition.X - mousePositionDown.X;
                            managed2D.CurrentPrimitive.MasLinesTop[i].Y += mouseCurrentPosition.Y - mousePositionDown.Y;

                            managed2D.CurrentPrimitive.MasLinesFront[i].X += mouseCurrentPosition.X - mousePositionDown.X;
                            managed2D.CurrentPrimitive.MasLinesRight[i].X += (mouseCurrentPosition.Y - mousePositionDown.Y) * -1;
                        }
                    }
                    if (isTransformSelectionEnabled)
                        lineSelection = new LinesSelection(managed2D.CurrentPrimitive);
                    else
                        selection = new SimpleSelection(managed2D.CurrentPrimitive);

                    #endregion
                }

				textureSettings.convertedPrimitive = GetConvertedPrimitiveToLocalCoords(managed2D.CurrentPrimitive);


                #endregion
            }
            else
            {
                if (isSnapEnabled)
                {
                    if (managed2D.IndexOfSelectedPrimitive != -1)
                    {
                            for (int i = 0; i < 10; i++)
                            {
                                if (name2DProjection == "Front")
                                {
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[i].X += mouseCurrentPositionSnap.X - mousePositionDownSnap.X;
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[i].Y += mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y;

                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[i].Y += mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y;
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[i].X += mouseCurrentPositionSnap.X - mousePositionDownSnap.X;
                                }
                                if (name2DProjection == "Right")
                                {
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[i].X += mouseCurrentPositionSnap.X - mousePositionDownSnap.X;
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[i].Y += mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y;

                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[i].Y += mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y; ;
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[i].Y += (mouseCurrentPositionSnap.X - mousePositionDownSnap.X) * -1;
                                }
                                if (name2DProjection == "Top")
                                {
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[i].X += mouseCurrentPositionSnap.X - mousePositionDownSnap.X;
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[i].Y += mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y;

                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[i].X += mouseCurrentPositionSnap.X - mousePositionDownSnap.X;
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[i].X += (mouseCurrentPositionSnap.Y - mousePositionDownSnap.Y) * -1;
                                }
                            }

                            if (isTransformSelectionEnabled)
                                lineSelection = new LinesSelection(((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]));
                            else
                                selection = new SimpleSelection(((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]));

							textureSettings.convertedPrimitive = GetConvertedPrimitiveToLocalCoords(((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]));
                    }
                }
                else
                {
                    if (managed2D.IndexOfSelectedPrimitive != -1)
                    {
                            for (int i = 0; i < 10; i++)
                            {
                                if (name2DProjection == "Front")
                                {
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[i].X += mouseCurrentPosition.X - mousePositionDown.X;
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[i].Y += mouseCurrentPosition.Y - mousePositionDown.Y;

                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[i].Y += mouseCurrentPosition.Y - mousePositionDown.Y;
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[i].X += mouseCurrentPosition.X - mousePositionDown.X;
                                }
                                if (name2DProjection == "Right")
                                {
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[i].X += mouseCurrentPosition.X - mousePositionDown.X;
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[i].Y += mouseCurrentPosition.Y - mousePositionDown.Y;

                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[i].Y += mouseCurrentPosition.Y - mousePositionDown.Y;
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[i].Y += (mouseCurrentPosition.X - mousePositionDown.X) * -1;
                                }
                                if (name2DProjection == "Top")
                                {
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[i].X += mouseCurrentPosition.X - mousePositionDown.X;
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesTop[i].Y += mouseCurrentPosition.Y - mousePositionDown.Y;

                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesFront[i].X += mouseCurrentPosition.X - mousePositionDown.X;
                                    ((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).MasLinesRight[i].X += (mouseCurrentPosition.Y - mousePositionDown.Y) * -1;
                                }
                            }
                            if (isTransformSelectionEnabled)
                                lineSelection = new LinesSelection(((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]));
                            else
                                selection = new SimpleSelection(((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]));

							textureSettings.convertedPrimitive = GetConvertedPrimitiveToLocalCoords(((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]));
                    }
                }
            }
            mousePositionDown = mouseCurrentPosition;
            mousePositionDownSnap = mouseCurrentPositionSnap;
        }
        private void DeselectAllPrimitives(int keyOfNewSelectedPrimitive)
        {
            managed3D.selectedIndex = -1;

            if (keyOfNewSelectedPrimitive != -1)
            {
                foreach (int k in managed2D.List2DPrimitives.Keys)
                {
                    RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[k];
                    if (k != keyOfNewSelectedPrimitive)
                    {
                        rp.IsSelected = false;
                        if (selection != null)
                            selection = null;
                        if (lineSelection != null)
                            lineSelection = null;
                        managed2D.IndexOfSelectedPrimitive = -1;
						managed3D.selectedIndex = -1;
                    }
                }
            }
            else
            {
                foreach (int key in managed2D.List2DPrimitives.Keys)
                {
                    RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];
                    rp.IsSelected = false;
                }
                if (selection != null)
                    selection = null;
                if (lineSelection != null)
                    lineSelection = null;
                managed2D.IndexOfSelectedPrimitive = -1;
				managed3D.selectedIndex = -1;
            }
        }
        private RectPrimitive GetConvertedPrimitiveToLocalCoords(RectPrimitive rp)
        {
            if (rp != null)
            {
                RectPrimitive r_temp = new RectPrimitive(new Rectangle(0, 0, 0, 0), new Rectangle(0, 0, 0, 0), new Rectangle(0, 0, 0, 0), rp.RandColorPrimitive);

                for (int i = 0; i < 10; i++)
                {
                    r_temp.MasLinesFront[i] = grid2D.ConvertGlobalCoordsToLocal(rp.MasLinesFront[i]);
                    r_temp.MasLinesRight[i] = grid2D.ConvertGlobalCoordsToLocal(rp.MasLinesRight[i]);
                    r_temp.MasLinesTop[i] = grid2D.ConvertGlobalCoordsToLocal(rp.MasLinesTop[i]);
                }

                return r_temp;
            }
            else
            {
                if (managed2D.List2DPrimitives.Count != 0)
                {
                    foreach (int key in managed2D.List2DPrimitives.Keys)
                    {
                        RectPrimitive rPr = (RectPrimitive)managed2D.List2DPrimitives[key];
                        if (rPr.IsSelected)
                        {
                            RectPrimitive r_temp = new RectPrimitive(new Rectangle(0, 0, 0, 0), new Rectangle(0, 0, 0, 0), new Rectangle(0, 0, 0, 0), rPr.RandColorPrimitive);

                            for (int i = 0; i < 10; i++)
                            {
                                r_temp.MasLinesFront[i] = grid2D.ConvertGlobalCoordsToLocal(rPr.MasLinesFront[i]);
                                r_temp.MasLinesRight[i] = grid2D.ConvertGlobalCoordsToLocal(rPr.MasLinesRight[i]);
                                r_temp.MasLinesTop[i] = grid2D.ConvertGlobalCoordsToLocal(rPr.MasLinesTop[i]);
                            }

                            return r_temp;
                        }
                    }
                }
            }
            return null;
        }
        void textureSettings_Deactivate(object sender, EventArgs e)
        {
            checkBoxTextures.Checked = false;

            textureSettings.GenerateHideSelection();
        }
        private void CalculateSizeObject(RectPrimitive rp)
        {
            if (rp != null)
            {
                RectPrimitive rConvert = GetConvertedPrimitiveToLocalCoords(rp);

                int maxX = rp.MasLinesFront[0].X;
                int minX = rp.MasLinesFront[0].X;

                int maxY = rp.MasLinesFront[0].Y;
                int minY = rp.MasLinesFront[0].Y;

                int minZ = rp.MasLinesTop[0].Y;
                int maxZ = rp.MasLinesTop[0].Y;

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

                    if (maxZ < rp.MasLinesTop[i].Y)
                        maxZ = rp.MasLinesTop[i].Y;
                    if (minZ > rp.MasLinesTop[i].Y)
                        minZ = rp.MasLinesTop[i].Y;
                }

                int w = maxX - minX;
                int h = maxY - minY;
                int z = maxZ - minZ; ;

                labelWidth.Text = "w : " + w.ToString();
                labelHeight.Text = "h : " + h.ToString();
                labelLength.Text = "z : " + z.ToString();
            }
            else
            {
                labelWidth.Text = "w : ";
                labelHeight.Text = "h : ";
                labelLength.Text = "z : ";
            }
        }
        private void saveMapToFile()
        {
            //timer2.Enabled = false;

            SaveFileDialog dlg = new SaveFileDialog();
            dlg.Filter = "map files (*.map)|*.map";
            dlg.OverwritePrompt = true;
            dlg.ValidateNames = true;
            string path;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                path = dlg.FileName;
                using (StreamWriter sw = new StreamWriter(path))
                {
                    sw.WriteLine("[Object]");
                    foreach (int key in managed3D.objectsList.Keys)
                    {
                        MyObject obj = (MyObject)managed3D.objectsList[key];
                        sw.WriteLine("{");


                        int i = 0;
                        foreach (CustomVertex.PositionNormalTextured pt in obj.mas_CV_PosNormTex)
                        {
                            if (i != 35)
                                sw.Write(pt.X.ToString() + ":");
                            else
                                sw.WriteLine(pt.X.ToString() + "");

                            i++;
                        }

                        i = 0;
                        foreach (CustomVertex.PositionNormalTextured pt in obj.mas_CV_PosNormTex)
                        {
                            if (i != 35)
                                sw.Write(pt.Y.ToString() + ":");
                            else
                                sw.WriteLine(pt.Y.ToString() + "");

                            i++;
                        }

                        i = 0;
                        foreach (CustomVertex.PositionNormalTextured pt in obj.mas_CV_PosNormTex)
                        {
                            if (i != 35)
                                sw.Write(pt.Z.ToString() + ":");
                            else
                                sw.WriteLine(pt.Z.ToString() + "");

                            i++;
                        }

                        i = 0;
                        foreach (CustomVertex.PositionNormalTextured pt in obj.mas_CV_PosNormTex)
                        {
                            if (i != 35)
                                sw.Write(pt.Normal.X.ToString() + ":");
                            else
                                sw.WriteLine(pt.Normal.X.ToString() + "");

                            i++;
                        }

                        i = 0;
                        foreach (CustomVertex.PositionNormalTextured pt in obj.mas_CV_PosNormTex)
                        {
                            if (i != 35)
                                sw.Write(pt.Normal.Y.ToString() + ":");
                            else
                                sw.WriteLine(pt.Normal.Y.ToString() + "");

                            i++;
                        }

                        i = 0;
                        foreach (CustomVertex.PositionNormalTextured pt in obj.mas_CV_PosNormTex)
                        {
                            if (i != 35)
                                sw.Write(pt.Normal.Z.ToString() + ":");
                            else
                                sw.WriteLine(pt.Normal.Z.ToString() + "");

                            i++;
                        }

                        i = 0;
                        foreach (CustomVertex.PositionNormalTextured pt in obj.mas_CV_PosNormTex)
                        {
                            if (i != 10 && i != 22 && i != 34 && i != 46 && i != 58 && i != 70)
                                sw.Write(pt.Tu.ToString() + ":" + pt.Tv.ToString() + ":");

                            if (i == 10 || i == 22 || i == 34 || i == 46 || i == 58 || i == 70)
                                sw.WriteLine(pt.Tu.ToString() + ":" + pt.Tv.ToString() + "");

                            i += 2;
                        }

                        sw.WriteLine((string)obj.textureNames[0]);
                        sw.WriteLine((string)obj.textureNames[1]);
                        sw.WriteLine((string)obj.textureNames[2]);
                        sw.WriteLine((string)obj.textureNames[3]);
                        sw.WriteLine((string)obj.textureNames[4]);
                        sw.WriteLine((string)obj.textureNames[5]);

                        sw.WriteLine("}");
                    }
					sw.WriteLine("[]");
					if (managed2D.RedPlayer != null)
					{
						sw.WriteLine("[PlayerR]");
						sw.WriteLine("{");
						int i = 0;
						foreach (CustomVertex.PositionNormalTextured pt in managed3D.playerRed.cv_PnTex)
						{
							sw.Write(pt.X.ToString());
							if(i != 35)
								sw.Write(":");
							i++;
						}
						sw.WriteLine("");

						i = 0;
						foreach (CustomVertex.PositionNormalTextured pt in managed3D.playerRed.cv_PnTex)
						{
							sw.Write(pt.Y.ToString());
							if (i != 35)
								sw.Write(":");
							i++;
						}
						sw.WriteLine("");

						i = 0;
						foreach (CustomVertex.PositionNormalTextured pt in managed3D.playerRed.cv_PnTex)
						{
							sw.Write(pt.Z.ToString());
							if (i != 35)
								sw.Write(":");
							i++;
						}
						sw.WriteLine("");


						sw.WriteLine("}");
						sw.WriteLine("[]");
					}

					if (managed2D.BluePlayer != null)
					{
						sw.WriteLine("[PlayerB]");
						sw.WriteLine("{");
						int i = 0;
						for(;i<36;i++) 
						{
							CustomVertex.PositionNormalTextured pt = managed3D.playerBlue.cv_PnTex[i];
							sw.Write(pt.X.ToString());
							if (i != 35)
								sw.Write(":");
						}
						sw.WriteLine("");

						i = 0;
						for(;i<36;i++) 
						{
							CustomVertex.PositionNormalTextured pt = managed3D.playerBlue.cv_PnTex[i];
							sw.Write(pt.Y.ToString());
							if (i != 35)
								sw.Write(":");
						}
						sw.WriteLine("");

						i = 0;
						for (; i < 36; i++)
						{
							CustomVertex.PositionNormalTextured pt = managed3D.playerBlue.cv_PnTex[i];
							sw.Write(pt.Z.ToString());
							if (i != 35)
								sw.Write(":");
						}
						sw.WriteLine("");


						sw.WriteLine("}");
						sw.WriteLine("[]");
					}
                }
            }
        }
        private void OpenMapFromFile()
        {
            string path;
            string tempString = "";
            OpenFileDialog dlg = new OpenFileDialog();
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                path = dlg.FileName;
                if (File.Exists(path))
                {
                    FileStream file = File.Open(path, FileMode.Open);
                    StreamReader sr = new StreamReader(file);

                    while (!sr.EndOfStream)
                    {
                        tempString = sr.ReadLine();

                        switch (tempString.ToLower())
                        {
                            case "[object]": ReadObjects(ref sr);
                                break;
                            case "[mesh]":
                                break;
                            case "[light]":
                                break;
							case "[playerr]": ReadPlayers(ref sr,true);
								break;
							case "[playerb]": ReadPlayers(ref sr,false);
								break;
                        }
                    }

                    sr.Close();
                    file.Close();
                }
            }
        }
		private void ReadPlayers(ref StreamReader sr,bool isPlayerRed)
		{
			if (isPlayerRed)
			{
				string s = sr.ReadLine();
				string[] masSplitX;
				string[] masSplitY;
				string[] masSplitZ;

				if (s == "{")
				{
					s = sr.ReadLine();

					masSplitX = s.Split(':');


					s = sr.ReadLine();


					masSplitY = s.Split(':');



					s = sr.ReadLine();


					masSplitZ = s.Split(':');

					managed3D.playerRed = new Player(new Point(0, 0), "Front", grid2D, managed3D.device);

					for (int i = 0; i < 36; i++)
					{
						managed3D.playerRed.cv_PnTex[i].X = float.Parse(masSplitX[i]);
					}

					for (int i = 0; i < 36; i++)
					{
						managed3D.playerRed.cv_PnTex[i].Y = float.Parse(masSplitY[i]);
					}

					for (int i = 0; i < 36; i++)
					{
						managed3D.playerRed.cv_PnTex[i].Z = float.Parse(masSplitZ[i]);
					}
					managed3D.playerRed.ReCreateBufferFromNewCustomVertsMas(managed3D.device);

					managed3D.playerRed.localRectFront = new Rectangle(new Point((int)managed3D.playerRed.cv_PnTex[0].X, (int)managed3D.playerRed.cv_PnTex[0].Y), new Size(32, 80));
					managed3D.playerRed.localRectRight = new Rectangle(new Point((int)managed3D.playerRed.cv_PnTex[6].Z, (int)managed3D.playerRed.cv_PnTex[6].Y), new Size(32, 80));
					managed3D.playerRed.localRectTop = new Rectangle(new Point((int)managed3D.playerRed.cv_PnTex[24].X, (int)managed3D.playerRed.cv_PnTex[24].Z), new Size(32, 32));

					managed3D.playerRed.rectFront = new Rectangle(new Point((int)grid2D.ConvertLocalCoordsToGlobal(managed3D.playerRed.cv_PnTex[0].X, 0), (int)grid2D.ConvertLocalCoordsToGlobal(managed3D.playerRed.cv_PnTex[0].Y, 1)), new Size(32, 80));
					managed3D.playerRed.rectRight = new Rectangle(new Point((int)grid2D.ConvertLocalCoordsToGlobal(managed3D.playerRed.cv_PnTex[6].Z, 0), (int)grid2D.ConvertLocalCoordsToGlobal(managed3D.playerRed.cv_PnTex[6].Y, 1)), new Size(32, 80));
					managed3D.playerRed.rectTop = new Rectangle(new Point((int)grid2D.ConvertLocalCoordsToGlobal(managed3D.playerRed.cv_PnTex[24].X, 0), (int)grid2D.ConvertLocalCoordsToGlobal(managed3D.playerRed.cv_PnTex[24].Z, 2)), new Size(32, 32));

					managed2D.RedPlayer = managed3D.playerRed;

				}

				s = sr.ReadLine();
				if (s == "}")
				{
					s = sr.ReadLine();
					if (s == "[]")
						return;
				}
			}
			else
			{
				string s = sr.ReadLine();
				string[] masSplitX;
				string[] masSplitY;
				string[] masSplitZ;

				if (s == "{")
				{
					s = sr.ReadLine();


					masSplitX = s.Split(':');


					s = sr.ReadLine();


					masSplitY = s.Split(':');



					s = sr.ReadLine();


					masSplitZ = s.Split(':');

					managed3D.playerBlue = new Player(new Point(0, 0), "Front", grid2D, managed3D.device);

					for (int i = 0; i < 36; i++)
					{
						managed3D.playerBlue.cv_PnTex[i].X = float.Parse(masSplitX[i]);
					}

					for (int i = 0; i < 36; i++)
					{
						managed3D.playerBlue.cv_PnTex[i].Y = float.Parse(masSplitY[i]);
					}

					for (int i = 0; i < 36; i++)
					{
						managed3D.playerBlue.cv_PnTex[i].Z = float.Parse(masSplitZ[i]);
					}
					managed3D.playerBlue.ReCreateBufferFromNewCustomVertsMas(managed3D.device);

					managed3D.playerBlue.localRectFront = new Rectangle(new Point((int)managed3D.playerBlue.cv_PnTex[0].X, (int)managed3D.playerBlue.cv_PnTex[0].Y), new Size(32, 80));
					managed3D.playerBlue.localRectRight = new Rectangle(new Point((int)managed3D.playerBlue.cv_PnTex[6].Z, (int)managed3D.playerBlue.cv_PnTex[6].Y), new Size(32, 80));
					managed3D.playerBlue.localRectTop = new Rectangle(new Point((int)managed3D.playerBlue.cv_PnTex[24].X, (int)managed3D.playerBlue.cv_PnTex[24].Z), new Size(32, 32));

					managed3D.playerBlue.rectFront = new Rectangle(new Point((int)grid2D.ConvertLocalCoordsToGlobal(managed3D.playerBlue.cv_PnTex[0].X, 0), (int)grid2D.ConvertLocalCoordsToGlobal(managed3D.playerBlue.cv_PnTex[0].Y, 1)), new Size(32, 80));
					managed3D.playerBlue.rectRight = new Rectangle(new Point((int)grid2D.ConvertLocalCoordsToGlobal(managed3D.playerBlue.cv_PnTex[6].Z, 0), (int)grid2D.ConvertLocalCoordsToGlobal(managed3D.playerBlue.cv_PnTex[6].Y, 1)), new Size(32, 80));
					managed3D.playerBlue.rectTop = new Rectangle(new Point((int)grid2D.ConvertLocalCoordsToGlobal(managed3D.playerBlue.cv_PnTex[24].X, 0), (int)grid2D.ConvertLocalCoordsToGlobal(managed3D.playerBlue.cv_PnTex[24].Z, 2)), new Size(32, 32));

					managed2D.BluePlayer = managed3D.playerBlue;

				}

				s = sr.ReadLine();
				if (s == "}")
				{
					s = sr.ReadLine();
					if (s == "[]")
						return;
				}
			}
		}
        private void ReadObjects(ref StreamReader sr)
        {
			Environment.CurrentDirectory = Form1.startingPath;

            string tempStr = "";
            string[] masSplit;

            float[] kvx = new float[36];
            float[] kvy = new float[36];
            float[] kvz = new float[36];

            float[] c_kvx = new float[36];
            float[] c_kvy = new float[36];
            float[] c_kvz = new float[36];

            float[] nrmlx = new float[36];
            float[] nrmly = new float[36];
            float[] nrmlz = new float[36];

            float[] tFront = new float[12];
            float[] tRight = new float[12];
            float[] tBack = new float[12];
            float[] tLeft = new float[12];
            float[] tUp = new float[12];
            float[] tDown = new float[12];

            string[] texturesNames = new string[6];

            while (!sr.EndOfStream)
            {
                tempStr = sr.ReadLine();
                if (tempStr == "{")
                {
                    continue;
                }
                if (tempStr == "}")
                {
                    Point[] masPointsFront = new Point[10];
                    Point[] masPointsTop = new Point[10];
                    Point[] masPointsRight = new Point[10];

                    for (int i = 0; i < 18; i++)
                    {
                        c_kvx[i] = grid2D.ConvertLocalCoordsToGlobal(kvx[i], 0);
                        c_kvy[i] = grid2D.ConvertLocalCoordsToGlobal(kvy[i], 1);
                        c_kvz[i] = grid2D.ConvertLocalCoordsToGlobal(kvz[i], 2);

						c_kvx[i + 18] = grid2D.ConvertLocalCoordsToGlobal(kvx[i + 18], 0);
						c_kvy[i + 18] = grid2D.ConvertLocalCoordsToGlobal(kvy[i + 18], 1);
						c_kvz[i + 18] = grid2D.ConvertLocalCoordsToGlobal(kvz[i + 18], 2);

                    }
                    #region CreateFrontMas

                    masPointsFront[0] = new Point((int)c_kvx[0], (int)c_kvy[0]);
                    masPointsFront[1] = new Point((int)c_kvx[1], (int)c_kvy[1]);
                    masPointsFront[2] = new Point((int)c_kvx[2], (int)c_kvy[2]);
                    masPointsFront[3] = new Point((int)c_kvx[5], (int)c_kvy[5]);
                    masPointsFront[4] = new Point((int)c_kvx[0], (int)c_kvy[0]);

                    masPointsFront[5] = new Point((int)c_kvx[13], (int)c_kvy[13]);
                    masPointsFront[6] = new Point((int)c_kvx[12], (int)c_kvy[12]);
                    masPointsFront[7] = new Point((int)c_kvx[17], (int)c_kvy[17]);
                    masPointsFront[8] = new Point((int)c_kvx[14], (int)c_kvy[14]);
                    masPointsFront[9] = new Point((int)c_kvx[13], (int)c_kvy[13]);

                    #endregion

                    #region CreateRightMas

                    masPointsRight[0] = new Point(Form1.panelWidth - (int)c_kvz[6], (int)c_kvy[6]);
                    masPointsRight[1] = new Point(Form1.panelWidth - (int)c_kvz[7], (int)c_kvy[7]);
                    masPointsRight[2] = new Point(Form1.panelWidth - (int)c_kvz[8], (int)c_kvy[8]);
                    masPointsRight[3] = new Point(Form1.panelWidth - (int)c_kvz[11], (int)c_kvy[11]);
                    masPointsRight[4] = new Point(Form1.panelWidth - (int)c_kvz[6], (int)c_kvy[6]);

                    masPointsRight[5] = new Point(Form1.panelWidth - (int)c_kvz[19], (int)c_kvy[19]);
                    masPointsRight[6] = new Point(Form1.panelWidth - (int)c_kvz[18], (int)c_kvy[18]);
                    masPointsRight[7] = new Point(Form1.panelWidth - (int)c_kvz[23], (int)c_kvy[23]);
                    masPointsRight[8] = new Point(Form1.panelWidth - (int)c_kvz[20], (int)c_kvy[20]);
                    masPointsRight[9] = new Point(Form1.panelWidth - (int)c_kvz[19], (int)c_kvy[19]);

                    #endregion

                    #region CreateTopMas

                    masPointsTop[0] = new Point((int)c_kvx[13], (int)c_kvz[13]);
                    masPointsTop[1] = new Point((int)c_kvx[12], (int)c_kvz[12]);
                    masPointsTop[2] = new Point((int)c_kvx[1], (int)c_kvz[1]);
                    masPointsTop[3] = new Point((int)c_kvx[0], (int)c_kvz[0]);
                    masPointsTop[4] = new Point((int)c_kvx[13], (int)c_kvz[13]);

                    masPointsTop[5] = new Point((int)c_kvx[14], (int)c_kvz[14]);
                    masPointsTop[6] = new Point((int)c_kvx[17], (int)c_kvz[17]);
                    masPointsTop[7] = new Point((int)c_kvx[2], (int)c_kvz[2]);
                    masPointsTop[8] = new Point((int)c_kvx[5], (int)c_kvz[5]);
                    masPointsTop[9] = new Point((int)c_kvx[14], (int)c_kvz[14]);

                    #endregion


                    RectPrimitive tmpRp = new RectPrimitive(masPointsFront, masPointsRight, masPointsTop, managed2D.GenerateRandomColor());

                    int key = managed2D.AddPrimitiveToList(tmpRp);


                    managed3D.AddObject(GetConvertedPrimitiveToLocalCoords(tmpRp), texturesNames, key, tFront, tRight, tBack, tLeft, tUp, tDown);


                    continue;
                }
                if (tempStr == "[]")
                    return;
                #region Read Object Body

                #region Read X Coords

                masSplit = tempStr.Split(':');

                for (int i = 0,c = 18; i < 18; i++,c++)
                {
                    kvx[i] = int.Parse(masSplit[i]);
					kvx[c] = int.Parse(masSplit[c]);
                }
                #endregion

                tempStr = sr.ReadLine();
                #region Read Y Coords

                masSplit = tempStr.Split(':');

                for (int i = 0,c= 18; i < 18; i++,c++)
                {
                    kvy[i] = float.Parse(masSplit[i]);
					kvy[c] = float.Parse(masSplit[c]);
                }
                #endregion

                tempStr = sr.ReadLine();
                #region Read Z Coords

                masSplit = tempStr.Split(':');

                for (int i = 0; i < 18; i++)
                {
                    kvz[i] = float.Parse(masSplit[i]);
					kvz[i+18] = float.Parse(masSplit[i+18]);
                }
                #endregion


                tempStr = sr.ReadLine();
                #region Read X Normals

                masSplit = tempStr.Split(':');
                for (int i = 0; i < 18; i++)
                {
                    nrmlx[i] = float.Parse(masSplit[i]);
					nrmlx[i+18] = float.Parse(masSplit[i+18]);
                }

                #endregion

                tempStr = sr.ReadLine();
                #region Read Y Normals

                masSplit = tempStr.Split(':');
                for (int i = 0; i < 18; i++)
                {
                    nrmly[i] = float.Parse(masSplit[i]);
					nrmly[i+18] = float.Parse(masSplit[i+18]);
                }

                #endregion

                tempStr = sr.ReadLine();
                #region Read Z Normals

                masSplit = tempStr.Split(':');
                for (int i = 0; i < 18; i++)
                {
                    nrmlz[i] = float.Parse(masSplit[i]);
					nrmlz[i+18] = float.Parse(masSplit[i+18]);
                }

                #endregion


                tempStr = sr.ReadLine();
                #region Read Front Textures Positions

                masSplit = tempStr.Split(':');
                for (int i = 0; i < 6; i++)
                {
                    tFront[i] = float.Parse(masSplit[i]);
					tFront[i+6] = float.Parse(masSplit[i+6]);
                }

                #endregion

                tempStr = sr.ReadLine();
                #region Read Right Textures Positions

                masSplit = tempStr.Split(':');
                for (int i = 0; i < 6; i++)
                {
                    tRight[i] = float.Parse(masSplit[i]);
					tRight[i+6] = float.Parse(masSplit[i+6]);
                }

                #endregion

                tempStr = sr.ReadLine();
                #region Read Back Textures Positions

                masSplit = tempStr.Split(':');
                for (int i = 0; i < 6; i++)
                {
                    tBack[i] = float.Parse(masSplit[i]);
					tBack[i+6] = float.Parse(masSplit[i+6]);
                }

                #endregion

                tempStr = sr.ReadLine();
                #region Read Left Textures Positions

                masSplit = tempStr.Split(':');
                for (int i = 0; i < 6; i++)
                {
                    tLeft[i] = float.Parse(masSplit[i]);
					tLeft[i+6] = float.Parse(masSplit[i+6]);
                }

                #endregion

                tempStr = sr.ReadLine();
                #region Read Up Textures Positions

                masSplit = tempStr.Split(':');
                for (int i = 0; i < 6; i++)
                {
                    tUp[i] = float.Parse(masSplit[i]);
					tUp[i+6] = float.Parse(masSplit[i+6]);
                }

                #endregion

                tempStr = sr.ReadLine();
                #region Read Down Textures Positions

                masSplit = tempStr.Split(':');
                for (int i = 0; i < 6; i++)
                {
                    tDown[i] = float.Parse(masSplit[i]);
					tDown[i+6] = float.Parse(masSplit[i+6]);
                }

                #endregion

                #region Read Textures Names

                for (int i = 0; i < 6; i++)
                {
                    tempStr = sr.ReadLine();

                    texturesNames[i] = tempStr;
					if (!managed3D.textures.Contains(tempStr))
					{
						string s = Environment.CurrentDirectory;
						Texture t = TextureLoader.FromFile(managed3D.device, tempStr);
						managed3D.textures.Add(tempStr, t);
					}
                }

                #endregion

                #endregion
            }
        }
        private void HollowObject()
        {
            if (managed2D.IndexOfSelectedPrimitive != -1 && managed3D.selectedIndex != -1)
            {
                Hollow hdlg = new Hollow(grid2D.mainSizeGrid);
                if (hdlg.ShowDialog() == DialogResult.OK)
                {
                    if (!((RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive]).IsTransformed)
                    {
                        managed2D.CurrentPrimitive = (RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive];

                        managed2D.List2DPrimitives.Remove(managed2D.IndexOfSelectedPrimitive);
                        managed3D.objectsList.Remove(managed3D.selectedIndex);

                        int sizeWall = -1;
                        sizeWall = int.Parse(hdlg.textBox1.Text);

                        RectPrimitive rpTemp = managed2D.CurrentPrimitive;
                        if (sizeWall != -1)
                        {
                            if (
                                   (rpTemp.MasLinesFront[1].X - sizeWall) - (rpTemp.MasLinesFront[0].X + sizeWall) >= 0
                                && (rpTemp.MasLinesFront[2].X - sizeWall) - (rpTemp.MasLinesFront[3].X + sizeWall) >= 0
                                && (rpTemp.MasLinesFront[3].Y - sizeWall) - (rpTemp.MasLinesFront[0].Y + sizeWall) >= 0
                                && (rpTemp.MasLinesFront[2].Y - sizeWall) - (rpTemp.MasLinesFront[1].Y + sizeWall) >= 0

                                && (rpTemp.MasLinesRight[1].X - sizeWall) - (rpTemp.MasLinesRight[0].X + sizeWall) >= 0
                                && (rpTemp.MasLinesRight[2].X - sizeWall) - (rpTemp.MasLinesRight[3].X + sizeWall) >= 0
                                && (rpTemp.MasLinesRight[3].Y - sizeWall) - (rpTemp.MasLinesRight[0].Y + sizeWall) >= 0
                                && (rpTemp.MasLinesRight[2].Y - sizeWall) - (rpTemp.MasLinesRight[1].Y + sizeWall) >= 0

                                && (rpTemp.MasLinesTop[1].X - sizeWall) - (rpTemp.MasLinesTop[0].X + sizeWall) >= 0
                                && (rpTemp.MasLinesTop[2].X - sizeWall) - (rpTemp.MasLinesTop[3].X + sizeWall) >= 0
                                && (rpTemp.MasLinesTop[3].Y - sizeWall) - (rpTemp.MasLinesTop[0].Y + sizeWall) >= 0
                                && (rpTemp.MasLinesTop[2].Y - sizeWall) - (rpTemp.MasLinesTop[1].Y + sizeWall) >= 0
                                )
                            {

                                RectPrimitive rpFront;

                                #region CreateFrontWall

                                rpFront = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                rpFront.MasLinesFront[0].X = rpTemp.MasLinesFront[0].X;
                                rpFront.MasLinesFront[0].Y = rpTemp.MasLinesFront[0].Y + sizeWall;
                                rpFront.MasLinesFront[1].X = rpTemp.MasLinesFront[1].X;
                                rpFront.MasLinesFront[1].Y = rpTemp.MasLinesFront[1].Y + sizeWall;
                                rpFront.MasLinesFront[2].X = rpTemp.MasLinesFront[2].X;
                                rpFront.MasLinesFront[2].Y = rpTemp.MasLinesFront[2].Y - sizeWall;
                                rpFront.MasLinesFront[3].X = rpTemp.MasLinesFront[3].X;
                                rpFront.MasLinesFront[3].Y = rpTemp.MasLinesFront[3].Y - sizeWall;
                                rpFront.MasLinesFront[4].X = rpTemp.MasLinesFront[4].X;
                                rpFront.MasLinesFront[4].Y = rpTemp.MasLinesFront[4].Y + sizeWall;

                                rpFront.MasLinesFront[5].X = rpTemp.MasLinesFront[5].X;
                                rpFront.MasLinesFront[5].Y = rpTemp.MasLinesFront[5].Y + sizeWall;
                                rpFront.MasLinesFront[6].X = rpTemp.MasLinesFront[6].X;
                                rpFront.MasLinesFront[6].Y = rpTemp.MasLinesFront[6].Y + sizeWall;
                                rpFront.MasLinesFront[7].X = rpTemp.MasLinesFront[7].X;
                                rpFront.MasLinesFront[7].Y = rpTemp.MasLinesFront[7].Y - sizeWall;
                                rpFront.MasLinesFront[8].X = rpTemp.MasLinesFront[8].X;
                                rpFront.MasLinesFront[8].Y = rpTemp.MasLinesFront[8].Y - sizeWall;
                                rpFront.MasLinesFront[9].X = rpTemp.MasLinesFront[9].X;
                                rpFront.MasLinesFront[9].Y = rpTemp.MasLinesFront[9].Y + sizeWall;

                                rpFront.MasLinesRight[0].X = rpTemp.MasLinesRight[0].X;
                                rpFront.MasLinesRight[0].Y = rpTemp.MasLinesRight[0].Y + sizeWall;
                                rpFront.MasLinesRight[1].X = rpTemp.MasLinesRight[0].X + sizeWall;
                                rpFront.MasLinesRight[1].Y = rpTemp.MasLinesRight[0].Y + sizeWall;
                                rpFront.MasLinesRight[2].X = rpTemp.MasLinesRight[3].X + sizeWall;
                                rpFront.MasLinesRight[2].Y = rpTemp.MasLinesRight[3].Y - sizeWall;
                                rpFront.MasLinesRight[3].X = rpTemp.MasLinesRight[3].X;
                                rpFront.MasLinesRight[3].Y = rpTemp.MasLinesRight[3].Y - sizeWall;
                                rpFront.MasLinesRight[4].X = rpTemp.MasLinesRight[4].X;
                                rpFront.MasLinesRight[4].Y = rpTemp.MasLinesRight[4].Y + sizeWall;

                                rpFront.MasLinesRight[5].X = rpTemp.MasLinesRight[5].X;
                                rpFront.MasLinesRight[5].Y = rpTemp.MasLinesRight[5].Y + sizeWall;
                                rpFront.MasLinesRight[6].X = rpTemp.MasLinesRight[5].X + sizeWall;
                                rpFront.MasLinesRight[6].Y = rpTemp.MasLinesRight[5].Y + sizeWall;
                                rpFront.MasLinesRight[7].X = rpTemp.MasLinesRight[8].X + sizeWall;
                                rpFront.MasLinesRight[7].Y = rpTemp.MasLinesRight[8].Y - sizeWall;
                                rpFront.MasLinesRight[8].X = rpTemp.MasLinesRight[8].X;
                                rpFront.MasLinesRight[8].Y = rpTemp.MasLinesRight[8].Y - sizeWall;
                                rpFront.MasLinesRight[9].X = rpTemp.MasLinesRight[9].X;
                                rpFront.MasLinesRight[9].Y = rpTemp.MasLinesRight[9].Y + sizeWall;

                                rpFront.MasLinesTop[0].X = rpTemp.MasLinesTop[3].X;
                                rpFront.MasLinesTop[0].Y = rpTemp.MasLinesTop[3].Y - sizeWall;
                                rpFront.MasLinesTop[1].X = rpTemp.MasLinesTop[2].X;
                                rpFront.MasLinesTop[1].Y = rpTemp.MasLinesTop[2].Y - sizeWall;
                                rpFront.MasLinesTop[2] = rpTemp.MasLinesTop[2];
                                rpFront.MasLinesTop[3] = rpTemp.MasLinesTop[3];
                                rpFront.MasLinesTop[4].X = rpTemp.MasLinesTop[3].X;
                                rpFront.MasLinesTop[4].Y = rpTemp.MasLinesTop[3].Y - sizeWall;

                                rpFront.MasLinesTop[5].X = rpTemp.MasLinesTop[8].X;
                                rpFront.MasLinesTop[5].Y = rpTemp.MasLinesTop[8].Y - sizeWall;
                                rpFront.MasLinesTop[6].X = rpTemp.MasLinesTop[7].X;
                                rpFront.MasLinesTop[6].Y = rpTemp.MasLinesTop[7].Y - sizeWall;
                                rpFront.MasLinesTop[7] = rpTemp.MasLinesTop[7];
                                rpFront.MasLinesTop[8] = rpTemp.MasLinesTop[8];
                                rpFront.MasLinesTop[9].X = rpTemp.MasLinesTop[8].X;
                                rpFront.MasLinesTop[9].Y = rpTemp.MasLinesTop[8].Y - sizeWall;



                                int key = managed2D.GetNextKey();
                                managed2D.List2DPrimitives.Add(key, rpFront);
                                RectPrimitive conv = GetConvertedPrimitiveToLocalCoords(rpFront);
                                managed3D.AddObject(conv, "Textures\\" + lastTexture, key);

                                #endregion

                                RectPrimitive rpRight;
                                #region CreateRightWall

                                rpRight = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                rpRight.MasLinesFront[0].X = rpTemp.MasLinesFront[1].X - sizeWall;
                                rpRight.MasLinesFront[0].Y = rpTemp.MasLinesFront[1].Y + sizeWall;
                                rpRight.MasLinesFront[1].X = rpTemp.MasLinesFront[1].X;
                                rpRight.MasLinesFront[1].Y = rpTemp.MasLinesFront[1].Y + sizeWall;
                                rpRight.MasLinesFront[2].X = rpTemp.MasLinesFront[2].X;
                                rpRight.MasLinesFront[2].Y = rpTemp.MasLinesFront[2].Y - sizeWall;
                                rpRight.MasLinesFront[3].X = rpTemp.MasLinesFront[2].X - sizeWall;
                                rpRight.MasLinesFront[3].Y = rpTemp.MasLinesFront[2].Y - sizeWall;
                                rpRight.MasLinesFront[4].X = rpTemp.MasLinesFront[1].X - sizeWall;
                                rpRight.MasLinesFront[4].Y = rpTemp.MasLinesFront[1].Y + sizeWall;

                                rpRight.MasLinesFront[5].X = rpTemp.MasLinesFront[6].X - sizeWall;
                                rpRight.MasLinesFront[5].Y = rpTemp.MasLinesFront[6].Y + sizeWall;
                                rpRight.MasLinesFront[6].X = rpTemp.MasLinesFront[6].X;
                                rpRight.MasLinesFront[6].Y = rpTemp.MasLinesFront[6].Y + sizeWall;
                                rpRight.MasLinesFront[7].X = rpTemp.MasLinesFront[7].X;
                                rpRight.MasLinesFront[7].Y = rpTemp.MasLinesFront[7].Y - sizeWall;
                                rpRight.MasLinesFront[8].X = rpTemp.MasLinesFront[7].X - sizeWall;
                                rpRight.MasLinesFront[8].Y = rpTemp.MasLinesFront[7].Y - sizeWall;
                                rpRight.MasLinesFront[9].X = rpTemp.MasLinesFront[6].X - sizeWall;
                                rpRight.MasLinesFront[9].Y = rpTemp.MasLinesFront[6].Y + sizeWall;

                                rpRight.MasLinesTop[0].X = rpTemp.MasLinesTop[1].X - sizeWall;
                                rpRight.MasLinesTop[0].Y = rpTemp.MasLinesTop[1].Y;
                                rpRight.MasLinesTop[1] = rpTemp.MasLinesTop[1];
                                rpRight.MasLinesTop[2].X = rpTemp.MasLinesTop[2].X;
                                rpRight.MasLinesTop[2].Y = rpTemp.MasLinesTop[2].Y - sizeWall;
                                rpRight.MasLinesTop[3].X = rpTemp.MasLinesTop[2].X - sizeWall;
                                rpRight.MasLinesTop[3].Y = rpTemp.MasLinesTop[2].Y - sizeWall;
                                rpRight.MasLinesTop[4].X = rpTemp.MasLinesTop[1].X - sizeWall;
                                rpRight.MasLinesTop[4].Y = rpTemp.MasLinesTop[1].Y;

                                rpRight.MasLinesTop[5].X = rpTemp.MasLinesTop[6].X - sizeWall;
                                rpRight.MasLinesTop[5].Y = rpTemp.MasLinesTop[6].Y;
                                rpRight.MasLinesTop[6] = rpTemp.MasLinesTop[6];
                                rpRight.MasLinesTop[7].X = rpTemp.MasLinesTop[7].X;
                                rpRight.MasLinesTop[7].Y = rpTemp.MasLinesTop[7].Y - sizeWall;
                                rpRight.MasLinesTop[8].X = rpTemp.MasLinesTop[7].X - sizeWall;
                                rpRight.MasLinesTop[8].Y = rpTemp.MasLinesTop[7].Y - sizeWall;
                                rpRight.MasLinesTop[9].X = rpTemp.MasLinesTop[6].X - sizeWall;
                                rpRight.MasLinesTop[9].Y = rpTemp.MasLinesTop[6].Y;

                                rpRight.MasLinesRight[0].X = rpTemp.MasLinesRight[0].X + sizeWall;
                                rpRight.MasLinesRight[0].Y = rpTemp.MasLinesRight[0].Y + sizeWall;
                                rpRight.MasLinesRight[1].X = rpTemp.MasLinesRight[1].X;
                                rpRight.MasLinesRight[1].Y = rpTemp.MasLinesRight[1].Y + sizeWall;
                                rpRight.MasLinesRight[2].X = rpTemp.MasLinesRight[2].X;
                                rpRight.MasLinesRight[2].Y = rpTemp.MasLinesRight[2].Y - sizeWall;
                                rpRight.MasLinesRight[3].X = rpTemp.MasLinesRight[3].X + sizeWall;
                                rpRight.MasLinesRight[3].Y = rpTemp.MasLinesRight[3].Y - sizeWall;
                                rpRight.MasLinesRight[4].X = rpTemp.MasLinesRight[4].X + sizeWall;
                                rpRight.MasLinesRight[4].Y = rpTemp.MasLinesRight[4].Y + sizeWall;

                                rpRight.MasLinesRight[5].X = rpTemp.MasLinesRight[5].X + sizeWall;
                                rpRight.MasLinesRight[5].Y = rpTemp.MasLinesRight[5].Y + sizeWall;
                                rpRight.MasLinesRight[6].X = rpTemp.MasLinesRight[6].X;
                                rpRight.MasLinesRight[6].Y = rpTemp.MasLinesRight[6].Y + sizeWall;
                                rpRight.MasLinesRight[7].X = rpTemp.MasLinesRight[7].X;
                                rpRight.MasLinesRight[7].Y = rpTemp.MasLinesRight[7].Y - sizeWall;
                                rpRight.MasLinesRight[8].X = rpTemp.MasLinesRight[8].X + sizeWall;
                                rpRight.MasLinesRight[8].Y = rpTemp.MasLinesRight[8].Y - sizeWall;
                                rpRight.MasLinesRight[9].X = rpTemp.MasLinesRight[9].X + sizeWall;
                                rpRight.MasLinesRight[9].Y = rpTemp.MasLinesRight[9].Y + sizeWall;

                                key = managed2D.GetNextKey();
                                managed2D.List2DPrimitives.Add(key, rpRight);
                                conv = GetConvertedPrimitiveToLocalCoords(rpRight);
                                managed3D.AddObject(conv, "Textures\\" + lastTexture, key);


                                #endregion

                                RectPrimitive rpBack;
                                #region CreateBackWall

                                rpBack = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                rpBack.MasLinesFront[0].X = rpTemp.MasLinesFront[0].X;
                                rpBack.MasLinesFront[0].Y = rpTemp.MasLinesFront[0].Y + sizeWall;
                                rpBack.MasLinesFront[1].X = rpTemp.MasLinesFront[1].X - sizeWall;
                                rpBack.MasLinesFront[1].Y = rpTemp.MasLinesFront[1].Y + sizeWall;
                                rpBack.MasLinesFront[2].X = rpTemp.MasLinesFront[2].X - sizeWall;
                                rpBack.MasLinesFront[2].Y = rpTemp.MasLinesFront[2].Y - sizeWall;
                                rpBack.MasLinesFront[3].X = rpTemp.MasLinesFront[3].X;
                                rpBack.MasLinesFront[3].Y = rpTemp.MasLinesFront[3].Y - sizeWall;
                                rpBack.MasLinesFront[4].X = rpTemp.MasLinesFront[4].X;
                                rpBack.MasLinesFront[4].Y = rpTemp.MasLinesFront[4].Y + sizeWall;

                                rpBack.MasLinesFront[5].X = rpTemp.MasLinesFront[5].X;
                                rpBack.MasLinesFront[5].Y = rpTemp.MasLinesFront[5].Y + sizeWall;
                                rpBack.MasLinesFront[6].X = rpTemp.MasLinesFront[6].X - sizeWall;
                                rpBack.MasLinesFront[6].Y = rpTemp.MasLinesFront[6].Y + sizeWall;
                                rpBack.MasLinesFront[7].X = rpTemp.MasLinesFront[7].X - sizeWall;
                                rpBack.MasLinesFront[7].Y = rpTemp.MasLinesFront[7].Y - sizeWall;
                                rpBack.MasLinesFront[8].X = rpTemp.MasLinesFront[8].X;
                                rpBack.MasLinesFront[8].Y = rpTemp.MasLinesFront[8].Y - sizeWall;
                                rpBack.MasLinesFront[9].X = rpTemp.MasLinesFront[9].X;
                                rpBack.MasLinesFront[9].Y = rpTemp.MasLinesFront[9].Y + sizeWall;

                                rpBack.MasLinesRight[0].X = rpTemp.MasLinesRight[1].X - sizeWall;
                                rpBack.MasLinesRight[0].Y = rpTemp.MasLinesRight[1].Y + sizeWall;
                                rpBack.MasLinesRight[1].X = rpTemp.MasLinesRight[1].X;
                                rpBack.MasLinesRight[1].Y = rpTemp.MasLinesRight[1].Y + sizeWall;
                                rpBack.MasLinesRight[2].X = rpTemp.MasLinesRight[2].X;
                                rpBack.MasLinesRight[2].Y = rpTemp.MasLinesRight[2].Y - sizeWall;
                                rpBack.MasLinesRight[3].X = rpTemp.MasLinesRight[2].X - sizeWall;
                                rpBack.MasLinesRight[3].Y = rpTemp.MasLinesRight[2].Y - sizeWall;
                                rpBack.MasLinesRight[4].X = rpTemp.MasLinesRight[1].X - sizeWall;
                                rpBack.MasLinesRight[4].Y = rpTemp.MasLinesRight[1].Y + sizeWall;

                                rpBack.MasLinesRight[5].X = rpTemp.MasLinesRight[6].X - sizeWall;
                                rpBack.MasLinesRight[5].Y = rpTemp.MasLinesRight[6].Y + sizeWall;
                                rpBack.MasLinesRight[6].X = rpTemp.MasLinesRight[6].X;
                                rpBack.MasLinesRight[6].Y = rpTemp.MasLinesRight[6].Y + sizeWall;
                                rpBack.MasLinesRight[7].X = rpTemp.MasLinesRight[7].X;
                                rpBack.MasLinesRight[7].Y = rpTemp.MasLinesRight[7].Y - sizeWall;
                                rpBack.MasLinesRight[8].X = rpTemp.MasLinesRight[7].X - sizeWall;
                                rpBack.MasLinesRight[8].Y = rpTemp.MasLinesRight[7].Y - sizeWall;
                                rpBack.MasLinesRight[9].X = rpTemp.MasLinesRight[6].X - sizeWall;
                                rpBack.MasLinesRight[9].Y = rpTemp.MasLinesRight[6].Y + sizeWall;

                                rpBack.MasLinesTop[0] = rpTemp.MasLinesTop[0];
                                rpBack.MasLinesTop[1].X = rpTemp.MasLinesTop[1].X - sizeWall;
                                rpBack.MasLinesTop[1].Y = rpTemp.MasLinesTop[0].Y;
                                rpBack.MasLinesTop[2].X = rpTemp.MasLinesTop[2].X - sizeWall;
                                rpBack.MasLinesTop[2].Y = rpTemp.MasLinesTop[1].Y + sizeWall;
                                rpBack.MasLinesTop[3].X = rpTemp.MasLinesTop[3].X;
                                rpBack.MasLinesTop[3].Y = rpTemp.MasLinesTop[0].Y + sizeWall;
                                rpBack.MasLinesTop[4] = rpTemp.MasLinesTop[4];

                                rpBack.MasLinesTop[5] = rpTemp.MasLinesTop[5];
                                rpBack.MasLinesTop[6].X = rpTemp.MasLinesTop[6].X - sizeWall;
                                rpBack.MasLinesTop[6].Y = rpTemp.MasLinesTop[5].Y;
                                rpBack.MasLinesTop[7].X = rpTemp.MasLinesTop[7].X - sizeWall;
                                rpBack.MasLinesTop[7].Y = rpTemp.MasLinesTop[6].Y + sizeWall;
                                rpBack.MasLinesTop[8].X = rpTemp.MasLinesTop[8].X;
                                rpBack.MasLinesTop[8].Y = rpTemp.MasLinesTop[5].Y + sizeWall;
                                rpBack.MasLinesTop[9] = rpTemp.MasLinesTop[9];


                                key = managed2D.GetNextKey();
                                managed2D.List2DPrimitives.Add(key, rpBack);
                                conv = GetConvertedPrimitiveToLocalCoords(rpBack);
                                managed3D.AddObject(conv, "Textures\\" + lastTexture, key);

                                #endregion

                                RectPrimitive rpLeft;
                                #region CreateLeftWall

                                rpLeft = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                rpLeft.MasLinesFront[0].X = rpTemp.MasLinesFront[0].X;
                                rpLeft.MasLinesFront[0].Y = rpTemp.MasLinesFront[0].Y + sizeWall;
                                rpLeft.MasLinesFront[1].X = rpTemp.MasLinesFront[0].X + sizeWall;
                                rpLeft.MasLinesFront[1].Y = rpTemp.MasLinesFront[0].Y + sizeWall;
                                rpLeft.MasLinesFront[2].X = rpTemp.MasLinesFront[3].X + sizeWall;
                                rpLeft.MasLinesFront[2].Y = rpTemp.MasLinesFront[3].Y - sizeWall;
                                rpLeft.MasLinesFront[3].X = rpTemp.MasLinesFront[3].X;
                                rpLeft.MasLinesFront[3].Y = rpTemp.MasLinesFront[3].Y - sizeWall;
                                rpLeft.MasLinesFront[4].X = rpTemp.MasLinesFront[4].X;
                                rpLeft.MasLinesFront[4].Y = rpTemp.MasLinesFront[4].Y + sizeWall;

                                rpLeft.MasLinesFront[5].X = rpTemp.MasLinesFront[5].X;
                                rpLeft.MasLinesFront[5].Y = rpTemp.MasLinesFront[5].Y + sizeWall;
                                rpLeft.MasLinesFront[6].X = rpTemp.MasLinesFront[5].X + sizeWall;
                                rpLeft.MasLinesFront[6].Y = rpTemp.MasLinesFront[5].Y + sizeWall;
                                rpLeft.MasLinesFront[7].X = rpTemp.MasLinesFront[8].X + sizeWall;
                                rpLeft.MasLinesFront[7].Y = rpTemp.MasLinesFront[8].Y - sizeWall;
                                rpLeft.MasLinesFront[8].X = rpTemp.MasLinesFront[8].X;
                                rpLeft.MasLinesFront[8].Y = rpTemp.MasLinesFront[8].Y - sizeWall;
                                rpLeft.MasLinesFront[9].X = rpTemp.MasLinesFront[9].X;
                                rpLeft.MasLinesFront[9].Y = rpTemp.MasLinesFront[9].Y + sizeWall;

                                rpLeft.MasLinesRight[0].X = rpTemp.MasLinesRight[0].X + sizeWall;
                                rpLeft.MasLinesRight[0].Y = rpTemp.MasLinesRight[0].Y + sizeWall;
                                rpLeft.MasLinesRight[1].X = rpTemp.MasLinesRight[1].X - sizeWall;
                                rpLeft.MasLinesRight[1].Y = rpTemp.MasLinesRight[1].Y + sizeWall;
                                rpLeft.MasLinesRight[2].X = rpTemp.MasLinesRight[2].X - sizeWall;
                                rpLeft.MasLinesRight[2].Y = rpTemp.MasLinesRight[2].Y - sizeWall;
                                rpLeft.MasLinesRight[3].X = rpTemp.MasLinesRight[3].X + sizeWall;
                                rpLeft.MasLinesRight[3].Y = rpTemp.MasLinesRight[3].Y - sizeWall;
                                rpLeft.MasLinesRight[4].X = rpTemp.MasLinesRight[0].X + sizeWall;
                                rpLeft.MasLinesRight[4].Y = rpTemp.MasLinesRight[0].Y + sizeWall;

                                rpLeft.MasLinesRight[5].X = rpTemp.MasLinesRight[5].X + sizeWall;
                                rpLeft.MasLinesRight[5].Y = rpTemp.MasLinesRight[5].Y + sizeWall;
                                rpLeft.MasLinesRight[6].X = rpTemp.MasLinesRight[6].X - sizeWall;
                                rpLeft.MasLinesRight[6].Y = rpTemp.MasLinesRight[6].Y + sizeWall;
                                rpLeft.MasLinesRight[7].X = rpTemp.MasLinesRight[7].X - sizeWall;
                                rpLeft.MasLinesRight[7].Y = rpTemp.MasLinesRight[7].Y - sizeWall;
                                rpLeft.MasLinesRight[8].X = rpTemp.MasLinesRight[8].X + sizeWall;
                                rpLeft.MasLinesRight[8].Y = rpTemp.MasLinesRight[8].Y - sizeWall;
                                rpLeft.MasLinesRight[9].X = rpTemp.MasLinesRight[5].X + sizeWall;
                                rpLeft.MasLinesRight[9].Y = rpTemp.MasLinesRight[5].Y + sizeWall;

                                rpLeft.MasLinesTop[0].X = rpTemp.MasLinesTop[0].X;
                                rpLeft.MasLinesTop[0].Y = rpTemp.MasLinesTop[0].Y + sizeWall;
                                rpLeft.MasLinesTop[1].X = rpTemp.MasLinesTop[0].X + sizeWall;
                                rpLeft.MasLinesTop[1].Y = rpTemp.MasLinesTop[0].Y + sizeWall;
                                rpLeft.MasLinesTop[2].X = rpTemp.MasLinesTop[3].X + sizeWall;
                                rpLeft.MasLinesTop[2].Y = rpTemp.MasLinesTop[3].Y - sizeWall;
                                rpLeft.MasLinesTop[3].X = rpTemp.MasLinesTop[3].X;
                                rpLeft.MasLinesTop[3].Y = rpTemp.MasLinesTop[3].Y - sizeWall;
                                rpLeft.MasLinesTop[4].X = rpTemp.MasLinesTop[4].X;
                                rpLeft.MasLinesTop[4].Y = rpTemp.MasLinesTop[4].Y + sizeWall;

                                rpLeft.MasLinesTop[5].X = rpTemp.MasLinesTop[5].X;
                                rpLeft.MasLinesTop[5].Y = rpTemp.MasLinesTop[5].Y + sizeWall;
                                rpLeft.MasLinesTop[6].X = rpTemp.MasLinesTop[5].X + sizeWall;
                                rpLeft.MasLinesTop[6].Y = rpTemp.MasLinesTop[5].Y + sizeWall;
                                rpLeft.MasLinesTop[7].X = rpTemp.MasLinesTop[8].X + sizeWall;
                                rpLeft.MasLinesTop[7].Y = rpTemp.MasLinesTop[8].Y - sizeWall;
                                rpLeft.MasLinesTop[8].X = rpTemp.MasLinesTop[8].X;
                                rpLeft.MasLinesTop[8].Y = rpTemp.MasLinesTop[8].Y - sizeWall;
                                rpLeft.MasLinesTop[9].X = rpTemp.MasLinesTop[9].X;
                                rpLeft.MasLinesTop[9].Y = rpTemp.MasLinesTop[9].Y + sizeWall;

                                key = managed2D.GetNextKey();
                                managed2D.List2DPrimitives.Add(key, rpLeft);
                                conv = GetConvertedPrimitiveToLocalCoords(rpLeft);
                                managed3D.AddObject(conv, "Textures\\" + lastTexture, key);

                                #endregion

                                RectPrimitive rpUp;
                                #region CreateUpWall

                                rpUp = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                rpUp.MasLinesFront[0].X = rpTemp.MasLinesFront[0].X;
                                rpUp.MasLinesFront[0].Y = rpTemp.MasLinesFront[0].Y;
                                rpUp.MasLinesFront[1].X = rpTemp.MasLinesFront[1].X;
                                rpUp.MasLinesFront[1].Y = rpTemp.MasLinesFront[1].Y;
                                rpUp.MasLinesFront[2].X = rpTemp.MasLinesFront[1].X;
                                rpUp.MasLinesFront[2].Y = rpTemp.MasLinesFront[1].Y + sizeWall;
                                rpUp.MasLinesFront[3].X = rpTemp.MasLinesFront[0].X;
                                rpUp.MasLinesFront[3].Y = rpTemp.MasLinesFront[0].Y + sizeWall;
                                rpUp.MasLinesFront[4].X = rpTemp.MasLinesFront[0].X;
                                rpUp.MasLinesFront[4].Y = rpTemp.MasLinesFront[0].Y;

                                rpUp.MasLinesFront[5].X = rpTemp.MasLinesFront[5].X;
                                rpUp.MasLinesFront[5].Y = rpTemp.MasLinesFront[5].Y;
                                rpUp.MasLinesFront[6].X = rpTemp.MasLinesFront[6].X;
                                rpUp.MasLinesFront[6].Y = rpTemp.MasLinesFront[6].Y;
                                rpUp.MasLinesFront[7].X = rpTemp.MasLinesFront[6].X;
                                rpUp.MasLinesFront[7].Y = rpTemp.MasLinesFront[6].Y + sizeWall;
                                rpUp.MasLinesFront[8].X = rpTemp.MasLinesFront[5].X;
                                rpUp.MasLinesFront[8].Y = rpTemp.MasLinesFront[5].Y + sizeWall;
                                rpUp.MasLinesFront[9].X = rpTemp.MasLinesFront[5].X;
                                rpUp.MasLinesFront[9].Y = rpTemp.MasLinesFront[5].Y;

                                rpUp.MasLinesRight[0].X = rpTemp.MasLinesRight[0].X;
                                rpUp.MasLinesRight[0].Y = rpTemp.MasLinesRight[0].Y;
                                rpUp.MasLinesRight[1].X = rpTemp.MasLinesRight[1].X;
                                rpUp.MasLinesRight[1].Y = rpTemp.MasLinesRight[1].Y;
                                rpUp.MasLinesRight[2].X = rpTemp.MasLinesRight[1].X;
                                rpUp.MasLinesRight[2].Y = rpTemp.MasLinesRight[1].Y + sizeWall;
                                rpUp.MasLinesRight[3].X = rpTemp.MasLinesRight[0].X;
                                rpUp.MasLinesRight[3].Y = rpTemp.MasLinesRight[0].Y + sizeWall;
                                rpUp.MasLinesRight[4].X = rpTemp.MasLinesRight[4].X;
                                rpUp.MasLinesRight[4].Y = rpTemp.MasLinesRight[4].Y;

                                rpUp.MasLinesRight[5].X = rpTemp.MasLinesRight[5].X;
                                rpUp.MasLinesRight[5].Y = rpTemp.MasLinesRight[5].Y;
                                rpUp.MasLinesRight[6].X = rpTemp.MasLinesRight[6].X;
                                rpUp.MasLinesRight[6].Y = rpTemp.MasLinesRight[6].Y;
                                rpUp.MasLinesRight[7].X = rpTemp.MasLinesRight[6].X;
                                rpUp.MasLinesRight[7].Y = rpTemp.MasLinesRight[6].Y + sizeWall;
                                rpUp.MasLinesRight[8].X = rpTemp.MasLinesRight[5].X;
                                rpUp.MasLinesRight[8].Y = rpTemp.MasLinesRight[5].Y + sizeWall;
                                rpUp.MasLinesRight[9].X = rpTemp.MasLinesRight[9].X;
                                rpUp.MasLinesRight[9].Y = rpTemp.MasLinesRight[9].Y;

                                rpUp.MasLinesTop = rpTemp.MasLinesTop;


                                key = managed2D.GetNextKey();
                                managed2D.List2DPrimitives.Add(key, rpUp);
                                conv = GetConvertedPrimitiveToLocalCoords(rpUp);
                                managed3D.AddObject(conv, "Textures\\" + lastTexture, key);

                                #endregion

                                RectPrimitive rpDown;
                                #region CreateDownWall

                                rpDown = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                rpDown.MasLinesFront[0].X = rpTemp.MasLinesFront[3].X;
                                rpDown.MasLinesFront[0].Y = rpTemp.MasLinesFront[3].Y - sizeWall;
                                rpDown.MasLinesFront[1].X = rpTemp.MasLinesFront[2].X;
                                rpDown.MasLinesFront[1].Y = rpTemp.MasLinesFront[2].Y - sizeWall;
                                rpDown.MasLinesFront[2].X = rpTemp.MasLinesFront[2].X;
                                rpDown.MasLinesFront[2].Y = rpTemp.MasLinesFront[2].Y;
                                rpDown.MasLinesFront[3].X = rpTemp.MasLinesFront[3].X;
                                rpDown.MasLinesFront[3].Y = rpTemp.MasLinesFront[3].Y;
                                rpDown.MasLinesFront[4].X = rpTemp.MasLinesFront[3].X;
                                rpDown.MasLinesFront[4].Y = rpTemp.MasLinesFront[3].Y - sizeWall;

                                rpDown.MasLinesFront[5].X = rpTemp.MasLinesFront[8].X;
                                rpDown.MasLinesFront[5].Y = rpTemp.MasLinesFront[8].Y - sizeWall;
                                rpDown.MasLinesFront[6].X = rpTemp.MasLinesFront[7].X;
                                rpDown.MasLinesFront[6].Y = rpTemp.MasLinesFront[7].Y - sizeWall;
                                rpDown.MasLinesFront[7].X = rpTemp.MasLinesFront[7].X;
                                rpDown.MasLinesFront[7].Y = rpTemp.MasLinesFront[7].Y;
                                rpDown.MasLinesFront[8].X = rpTemp.MasLinesFront[8].X;
                                rpDown.MasLinesFront[8].Y = rpTemp.MasLinesFront[8].Y;
                                rpDown.MasLinesFront[9].X = rpTemp.MasLinesFront[8].X;
                                rpDown.MasLinesFront[9].Y = rpTemp.MasLinesFront[8].Y - sizeWall;

                                rpDown.MasLinesRight[0].X = rpTemp.MasLinesRight[3].X;
                                rpDown.MasLinesRight[0].Y = rpTemp.MasLinesRight[3].Y - sizeWall;
                                rpDown.MasLinesRight[1].X = rpTemp.MasLinesRight[2].X;
                                rpDown.MasLinesRight[1].Y = rpTemp.MasLinesRight[2].Y - sizeWall;
                                rpDown.MasLinesRight[2].X = rpTemp.MasLinesRight[2].X;
                                rpDown.MasLinesRight[2].Y = rpTemp.MasLinesRight[2].Y;
                                rpDown.MasLinesRight[3].X = rpTemp.MasLinesRight[3].X;
                                rpDown.MasLinesRight[3].Y = rpTemp.MasLinesRight[3].Y;
                                rpDown.MasLinesRight[4].X = rpTemp.MasLinesRight[3].X;
                                rpDown.MasLinesRight[4].Y = rpTemp.MasLinesRight[3].Y - sizeWall;

                                rpDown.MasLinesRight[5].X = rpTemp.MasLinesRight[8].X;
                                rpDown.MasLinesRight[5].Y = rpTemp.MasLinesRight[8].Y - sizeWall;
                                rpDown.MasLinesRight[6].X = rpTemp.MasLinesRight[7].X;
                                rpDown.MasLinesRight[6].Y = rpTemp.MasLinesRight[7].Y - sizeWall;
                                rpDown.MasLinesRight[7].X = rpTemp.MasLinesRight[7].X;
                                rpDown.MasLinesRight[7].Y = rpTemp.MasLinesRight[7].Y;
                                rpDown.MasLinesRight[8].X = rpTemp.MasLinesRight[8].X;
                                rpDown.MasLinesRight[8].Y = rpTemp.MasLinesRight[8].Y;
                                rpDown.MasLinesRight[9].X = rpTemp.MasLinesRight[8].X;
                                rpDown.MasLinesRight[9].Y = rpTemp.MasLinesRight[8].Y - sizeWall;

                                rpDown.MasLinesTop = rpTemp.MasLinesTop;


                                key = managed2D.GetNextKey();
                                managed2D.List2DPrimitives.Add(key, rpDown);
                                conv = GetConvertedPrimitiveToLocalCoords(rpDown);
                                managed3D.AddObject(conv, "Textures\\" + lastTexture, key);


                                #endregion

                                managed2D.CurrentPrimitive = null;

                            }
                            else
                            {
                                MessageBox.Show("The object must have empty spaces betwean the walls");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("This object was transformed");
                    }
                }
            }
        }
        void textureSettings_FormClosed(object sender, FormClosedEventArgs e)
        {
            textureSettings = new TexturesSettings();
            textureSettings.FormClosed += new FormClosedEventHandler(textureSettings_FormClosed);
			textureSettings.TextureFormKeyDown += new TextureFormKeyDownDelegate(textureSettings_TextureFormKeyDown);
			textureSettings.Deactivate += new EventHandler(textureSettings_Deactivate);
            checkBoxTextures.Checked = false;
        }
        private RectPrimitive SlidePositionPrimitive(RectPrimitive rp)
        {
            if (rp != null)
            {
                for (int i = 0; i < 10; i++)
                {
                    rp.MasLinesFront[i].X += grid2D.mainSizeGrid;
                    rp.MasLinesFront[i].Y += grid2D.mainSizeGrid;

                    rp.MasLinesRight[i].X += grid2D.mainSizeGrid;
                    rp.MasLinesRight[i].Y += grid2D.mainSizeGrid;

                    rp.MasLinesTop[i].X += grid2D.mainSizeGrid;
                    rp.MasLinesTop[i].Y += grid2D.mainSizeGrid;
                }
            }
            return rp;
        }
        private void CurveObject()
        {
            if (managed2D.IndexOfSelectedPrimitive != -1 && managed3D.selectedIndex != -1)
            {
                if (MessageBox.Show("Do you want cerve objects?\nUndo no bossible, are you sure?", "Warning!", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    RectPrimitive rpSel = (RectPrimitive)managed2D.List2DPrimitives[managed2D.IndexOfSelectedPrimitive];

                    if (!rpSel.IsTransformed)
                    {
                        ArrayList masKeysOfPrimitivesForDelete = new ArrayList();
                        Hashtable primitivesForAdd = new Hashtable();

                        foreach (int key in managed2D.List2DPrimitives.Keys)
                        {
                            if (key != managed2D.IndexOfSelectedPrimitive)
                            {
                                RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];
                                if (!rp.IsTransformed)
                                {
                                    #region CheckFrontProjectionToInsideAllPoints
                                    if (
                                           IsPointInsideTheObject(rp,rpSel.MasLinesFront[0],"Front")
                                        && IsPointInsideTheObject(rp,rpSel.MasLinesFront[1],"Front")
                                        && IsPointInsideTheObject(rp,rpSel.MasLinesFront[2],"Front")
                                        && IsPointInsideTheObject(rp,rpSel.MasLinesFront[3],"Front")

                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesRight[0], "Right")
                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesRight[1], "Right")
                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesRight[2], "Right")
                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesRight[3], "Right")

                                        && rpSel.MasLinesRight[0].X < rp.MasLinesRight[0].X
                                        && rpSel.MasLinesRight[1].X > rp.MasLinesRight[1].X
                                        && rpSel.MasLinesRight[2].X > rp.MasLinesRight[2].X
                                        && rpSel.MasLinesRight[3].X < rp.MasLinesRight[3].X

                                        //&& !IsPointInsideTheObject(rp, rpSel.MasLinesFront[0], "Top")
                                        //&& !IsPointInsideTheObject(rp, rpSel.MasLinesFront[1], "Top")
                                        //&& !IsPointInsideTheObject(rp, rpSel.MasLinesFront[2], "Top")
                                        //&& !IsPointInsideTheObject(rp, rpSel.MasLinesFront[3], "Top")

                                        && rpSel.MasLinesTop[0].Y < rp.MasLinesTop[0].Y
                                        && rpSel.MasLinesTop[1].Y < rp.MasLinesTop[1].Y
                                        && rpSel.MasLinesTop[2].Y > rp.MasLinesTop[2].Y
                                        && rpSel.MasLinesTop[3].Y > rp.MasLinesTop[3].Y
                                        )
                                    {
                                        #region CreateLeftPrimitive

                                        RectPrimitive rpLeft = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                        #region FrontMassive
                                        rpLeft.MasLinesFront[0] = rp.MasLinesFront[0];
                                        rpLeft.MasLinesFront[3] = rp.MasLinesFront[3];
                                        rpLeft.MasLinesFront[4] = rp.MasLinesFront[4];
                                        rpLeft.MasLinesFront[5] = rp.MasLinesFront[5];
                                        rpLeft.MasLinesFront[8] = rp.MasLinesFront[8];
                                        rpLeft.MasLinesFront[9] = rp.MasLinesFront[9];

                                        rpLeft.MasLinesFront[1].X = rpSel.MasLinesFront[0].X;
                                        rpLeft.MasLinesFront[1].Y = rpLeft.MasLinesFront[0].Y;
                                        rpLeft.MasLinesFront[2].X = rpSel.MasLinesFront[3].X;
                                        rpLeft.MasLinesFront[2].Y = rpLeft.MasLinesFront[3].Y;

                                        rpLeft.MasLinesFront[6].X = rpSel.MasLinesFront[5].X;
                                        rpLeft.MasLinesFront[6].Y = rpLeft.MasLinesFront[5].Y;
                                        rpLeft.MasLinesFront[7].X = rpSel.MasLinesFront[8].X;
                                        rpLeft.MasLinesFront[7].Y = rpLeft.MasLinesFront[8].Y;
                                        #endregion

                                        #region RightMassive

                                        rpLeft.MasLinesRight = rp.MasLinesRight;

                                        #endregion

                                        #region TopMassive

                                        rpLeft.MasLinesTop[0] = rp.MasLinesTop[0];
                                        rpLeft.MasLinesTop[3] = rp.MasLinesTop[3];
                                        rpLeft.MasLinesTop[4] = rp.MasLinesTop[4];
                                        rpLeft.MasLinesTop[5] = rp.MasLinesTop[5];
                                        rpLeft.MasLinesTop[8] = rp.MasLinesTop[8];
                                        rpLeft.MasLinesTop[9] = rp.MasLinesTop[9];

                                        rpLeft.MasLinesTop[1].X = rpSel.MasLinesTop[0].X;
                                        rpLeft.MasLinesTop[1].Y = rpLeft.MasLinesTop[0].Y;
                                        rpLeft.MasLinesTop[2].X = rpSel.MasLinesTop[3].X;
                                        rpLeft.MasLinesTop[2].Y = rpLeft.MasLinesTop[3].Y;

                                        rpLeft.MasLinesTop[6].X = rpSel.MasLinesTop[5].X;
                                        rpLeft.MasLinesTop[6].Y = rpLeft.MasLinesTop[5].Y;
                                        rpLeft.MasLinesTop[7].X = rpSel.MasLinesTop[8].X;
                                        rpLeft.MasLinesTop[7].Y = rpLeft.MasLinesTop[8].Y;

                                        #endregion


                                        int k = managed2D.GetNextKey();
                                        primitivesForAdd.Add(k, rpLeft);



                                        #endregion

                                        #region CreateTopPrimitive

                                        RectPrimitive rpTop = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                        #region FrontMassive

                                        rpTop.MasLinesFront[0].X = rpSel.MasLinesFront[0].X;
                                        rpTop.MasLinesFront[0].Y = rp.MasLinesFront[0].Y;
                                        rpTop.MasLinesFront[1].X = rpSel.MasLinesFront[1].X;
                                        rpTop.MasLinesFront[1].Y = rp.MasLinesFront[1].Y;
                                        rpTop.MasLinesFront[2].X = rpSel.MasLinesFront[1].X;
                                        rpTop.MasLinesFront[2].Y = rpSel.MasLinesFront[1].Y;
                                        rpTop.MasLinesFront[3].X = rpSel.MasLinesFront[0].X;
                                        rpTop.MasLinesFront[3].Y = rpSel.MasLinesFront[0].Y;
                                        rpTop.MasLinesFront[4].X = rpSel.MasLinesFront[4].X;
                                        rpTop.MasLinesFront[4].Y = rp.MasLinesFront[4].Y;

                                        rpTop.MasLinesFront[5].X = rpSel.MasLinesFront[5].X;
                                        rpTop.MasLinesFront[5].Y = rp.MasLinesFront[5].Y;
                                        rpTop.MasLinesFront[6].X = rpSel.MasLinesFront[6].X;
                                        rpTop.MasLinesFront[6].Y = rp.MasLinesFront[6].Y;
                                        rpTop.MasLinesFront[7].X = rpSel.MasLinesFront[6].X;
                                        rpTop.MasLinesFront[7].Y = rpSel.MasLinesFront[6].Y;
                                        rpTop.MasLinesFront[8].X = rpSel.MasLinesFront[5].X;
                                        rpTop.MasLinesFront[8].Y = rpSel.MasLinesFront[5].Y;
                                        rpTop.MasLinesFront[9].X = rpSel.MasLinesFront[9].X;
                                        rpTop.MasLinesFront[9].Y = rp.MasLinesFront[9].Y;

                                        #endregion

                                        #region RightMassive

                                        rpTop.MasLinesRight[0] = rp.MasLinesRight[0];
                                        rpTop.MasLinesRight[1] = rp.MasLinesRight[1];
                                        rpTop.MasLinesRight[4] = rp.MasLinesRight[4];

                                        rpTop.MasLinesRight[5] = rp.MasLinesRight[5];
                                        rpTop.MasLinesRight[6] = rp.MasLinesRight[6];
                                        rpTop.MasLinesRight[9] = rp.MasLinesRight[9];

                                        rpTop.MasLinesRight[2].X = rp.MasLinesRight[2].X;
                                        rpTop.MasLinesRight[2].Y = rpSel.MasLinesRight[1].Y;
                                        rpTop.MasLinesRight[3].X = rp.MasLinesRight[3].X;
                                        rpTop.MasLinesRight[3].Y = rpSel.MasLinesRight[0].Y;

                                        rpTop.MasLinesRight[7].X = rp.MasLinesRight[7].X;
                                        rpTop.MasLinesRight[7].Y = rpSel.MasLinesRight[6].Y;
                                        rpTop.MasLinesRight[8].X = rp.MasLinesRight[8].X;
                                        rpTop.MasLinesRight[8].Y = rpSel.MasLinesRight[5].Y;

                                        #endregion

                                        #region TopMassive

                                        rpTop.MasLinesTop[0].X = rpSel.MasLinesTop[0].X;
                                        rpTop.MasLinesTop[0].Y = rp.MasLinesTop[0].Y;
                                        rpTop.MasLinesTop[1].X = rpSel.MasLinesTop[1].X;
                                        rpTop.MasLinesTop[1].Y = rp.MasLinesTop[1].Y;
                                        rpTop.MasLinesTop[2].X = rpSel.MasLinesTop[2].X;
                                        rpTop.MasLinesTop[2].Y = rp.MasLinesTop[2].Y;
                                        rpTop.MasLinesTop[3].X = rpSel.MasLinesTop[3].X;
                                        rpTop.MasLinesTop[3].Y = rp.MasLinesTop[3].Y;
                                        rpTop.MasLinesTop[4].X = rpSel.MasLinesTop[4].X;
                                        rpTop.MasLinesTop[4].Y = rp.MasLinesTop[4].Y;

                                        rpTop.MasLinesTop[5].X = rpSel.MasLinesTop[5].X;
                                        rpTop.MasLinesTop[5].Y = rp.MasLinesTop[5].Y;
                                        rpTop.MasLinesTop[6].X = rpSel.MasLinesTop[6].X;
                                        rpTop.MasLinesTop[6].Y = rp.MasLinesTop[6].Y;
                                        rpTop.MasLinesTop[7].X = rpSel.MasLinesTop[7].X;
                                        rpTop.MasLinesTop[7].Y = rp.MasLinesTop[7].Y;
                                        rpTop.MasLinesTop[8].X = rpSel.MasLinesTop[8].X;
                                        rpTop.MasLinesTop[8].Y = rp.MasLinesTop[8].Y;
                                        rpTop.MasLinesTop[9].X = rpSel.MasLinesTop[9].X;
                                        rpTop.MasLinesTop[9].Y = rp.MasLinesTop[9].Y;

                                        #endregion


                                        k = managed2D.GetNextKey();
                                        primitivesForAdd.Add(k, rpTop);

                                        #endregion

                                        #region CreateRightPrimitive

                                        RectPrimitive rpRight = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                        #region FrontMassive

                                        rpRight.MasLinesFront[1] = rp.MasLinesFront[1];
                                        rpRight.MasLinesFront[2] = rp.MasLinesFront[2];
                                        rpRight.MasLinesFront[6] = rp.MasLinesFront[6];
                                        rpRight.MasLinesFront[7] = rp.MasLinesFront[7];

                                        rpRight.MasLinesFront[0].X = rpSel.MasLinesFront[1].X;
                                        rpRight.MasLinesFront[0].Y = rp.MasLinesFront[1].Y;
                                        rpRight.MasLinesFront[3].X = rpSel.MasLinesFront[2].X;
                                        rpRight.MasLinesFront[3].Y = rp.MasLinesFront[2].Y;
                                        rpRight.MasLinesFront[4].X = rpSel.MasLinesFront[1].X;
                                        rpRight.MasLinesFront[4].Y = rp.MasLinesFront[1].Y;
                                        rpRight.MasLinesFront[5].X = rpSel.MasLinesFront[6].X;
                                        rpRight.MasLinesFront[5].Y = rp.MasLinesFront[6].Y;
                                        rpRight.MasLinesFront[8].X = rpSel.MasLinesFront[7].X;
                                        rpRight.MasLinesFront[8].Y = rp.MasLinesFront[7].Y;
                                        rpRight.MasLinesFront[9].X = rpSel.MasLinesFront[6].X;
                                        rpRight.MasLinesFront[9].Y = rp.MasLinesFront[6].Y;


                                        #endregion

                                        #region RightMassive

                                        rpRight.MasLinesRight = rp.MasLinesRight;

                                        #endregion

                                        #region TopMassive

                                        rpRight.MasLinesTop[1] = rp.MasLinesTop[1];
                                        rpRight.MasLinesTop[2] = rp.MasLinesTop[2];
                                        rpRight.MasLinesTop[6] = rp.MasLinesTop[6];
                                        rpRight.MasLinesTop[7] = rp.MasLinesTop[7];

                                        rpRight.MasLinesTop[0].X = rpSel.MasLinesTop[1].X;
                                        rpRight.MasLinesTop[0].Y = rp.MasLinesTop[1].Y;
                                        rpRight.MasLinesTop[3].X = rpSel.MasLinesTop[2].X;
                                        rpRight.MasLinesTop[3].Y = rp.MasLinesTop[2].Y;
                                        rpRight.MasLinesTop[4].X = rpSel.MasLinesTop[1].X;
                                        rpRight.MasLinesTop[4].Y = rp.MasLinesTop[1].Y;

                                        rpRight.MasLinesTop[5].X = rpSel.MasLinesTop[1].X;
                                        rpRight.MasLinesTop[5].Y = rp.MasLinesTop[1].Y;
                                        rpRight.MasLinesTop[8].X = rpSel.MasLinesTop[7].X;
                                        rpRight.MasLinesTop[8].Y = rp.MasLinesTop[7].Y;
                                        rpRight.MasLinesTop[9].X = rpSel.MasLinesTop[6].X;
                                        rpRight.MasLinesTop[9].Y = rp.MasLinesTop[6].Y;

                                        #endregion


                                        k = managed2D.GetNextKey();
                                        primitivesForAdd.Add(k, rpRight);

                                        #endregion

                                        #region CreateDownPrimitive

                                        RectPrimitive rpDown = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                        #region FrontMassive

                                        rpDown.MasLinesFront[0] = rpSel.MasLinesFront[3];
                                        rpDown.MasLinesFront[1] = rpSel.MasLinesFront[2];
                                        rpDown.MasLinesFront[4] = rpSel.MasLinesFront[3];
                                        rpDown.MasLinesFront[5] = rpSel.MasLinesFront[8];
                                        rpDown.MasLinesFront[6] = rpSel.MasLinesFront[7];
                                        rpDown.MasLinesFront[9] = rpSel.MasLinesFront[8];

                                        rpDown.MasLinesFront[2].X = rpSel.MasLinesFront[2].X;
                                        rpDown.MasLinesFront[2].Y = rp.MasLinesFront[2].Y;
                                        rpDown.MasLinesFront[3].X = rpSel.MasLinesFront[3].X;
                                        rpDown.MasLinesFront[3].Y = rp.MasLinesFront[3].Y;

                                        rpDown.MasLinesFront[7].X = rpSel.MasLinesFront[7].X;
                                        rpDown.MasLinesFront[7].Y = rp.MasLinesFront[7].Y;
                                        rpDown.MasLinesFront[8].X = rpSel.MasLinesFront[8].X;
                                        rpDown.MasLinesFront[8].Y = rp.MasLinesFront[8].Y;


                                        #endregion

                                        #region RightMassive

                                        rpDown.MasLinesRight[2] = rp.MasLinesRight[2];
                                        rpDown.MasLinesRight[3] = rp.MasLinesRight[3];
                                        rpDown.MasLinesRight[7] = rp.MasLinesRight[7];
                                        rpDown.MasLinesRight[8] = rp.MasLinesRight[8];

                                        rpDown.MasLinesRight[0].X = rp.MasLinesRight[3].X;
                                        rpDown.MasLinesRight[0].Y = rpSel.MasLinesRight[3].Y;
                                        rpDown.MasLinesRight[1].X = rp.MasLinesRight[2].X;
                                        rpDown.MasLinesRight[1].Y = rpSel.MasLinesRight[2].Y;
                                        rpDown.MasLinesRight[4].X = rp.MasLinesRight[3].X;
                                        rpDown.MasLinesRight[4].Y = rpSel.MasLinesRight[3].Y;

                                        rpDown.MasLinesRight[5].X = rp.MasLinesRight[8].X;
                                        rpDown.MasLinesRight[5].Y = rpSel.MasLinesRight[8].Y;
                                        rpDown.MasLinesRight[6].X = rp.MasLinesRight[7].X;
                                        rpDown.MasLinesRight[6].Y = rpSel.MasLinesRight[7].Y;
                                        rpDown.MasLinesRight[9].X = rp.MasLinesRight[8].X;
                                        rpDown.MasLinesRight[9].Y = rpSel.MasLinesRight[8].Y;

                                        #endregion

                                        #region TopMassive

                                        rpDown.MasLinesTop[0].X = rpSel.MasLinesTop[0].X;
                                        rpDown.MasLinesTop[0].Y = rp.MasLinesTop[0].Y;
                                        rpDown.MasLinesTop[1].X = rpSel.MasLinesTop[1].X;
                                        rpDown.MasLinesTop[1].Y = rp.MasLinesTop[1].Y;
                                        rpDown.MasLinesTop[2].X = rpSel.MasLinesTop[2].X;
                                        rpDown.MasLinesTop[2].Y = rp.MasLinesTop[2].Y;
                                        rpDown.MasLinesTop[3].X = rpSel.MasLinesTop[3].X;
                                        rpDown.MasLinesTop[3].Y = rp.MasLinesTop[3].Y;
                                        rpDown.MasLinesTop[4].X = rpSel.MasLinesTop[4].X;
                                        rpDown.MasLinesTop[4].Y = rp.MasLinesTop[4].Y;

                                        rpDown.MasLinesTop[5].X = rpSel.MasLinesTop[5].X;
                                        rpDown.MasLinesTop[5].Y = rp.MasLinesTop[5].Y;
                                        rpDown.MasLinesTop[6].X = rpSel.MasLinesTop[6].X;
                                        rpDown.MasLinesTop[6].Y = rp.MasLinesTop[6].Y;
                                        rpDown.MasLinesTop[7].X = rpSel.MasLinesTop[7].X;
                                        rpDown.MasLinesTop[7].Y = rp.MasLinesTop[7].Y;
                                        rpDown.MasLinesTop[8].X = rpSel.MasLinesTop[8].X;
                                        rpDown.MasLinesTop[8].Y = rp.MasLinesTop[8].Y;
                                        rpDown.MasLinesTop[9].X = rpSel.MasLinesTop[9].X;
                                        rpDown.MasLinesTop[9].Y = rp.MasLinesTop[9].Y;

                                        #endregion


                                        k = managed2D.GetNextKey();
                                        primitivesForAdd.Add(k, rpDown);

                                        #endregion

                                        masKeysOfPrimitivesForDelete.Add(key);
                                    }
                                    #endregion
                                    #region CheckTopProjectionToInsideAllPoints

                                    if (
                                           !IsPointInsideTheObject(rp, rpSel.MasLinesFront[0], "Front")
                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesFront[1], "Front")
                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesFront[2], "Front")
                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesFront[3], "Front")

                                        && rpSel.MasLinesFront[0].Y < rp.MasLinesFront[0].Y
                                        && rpSel.MasLinesFront[1].Y < rp.MasLinesFront[1].Y
                                        && rpSel.MasLinesFront[2].Y > rp.MasLinesFront[2].Y
                                        && rpSel.MasLinesFront[3].Y > rp.MasLinesFront[3].Y

                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesRight[0], "Right")
                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesRight[1], "Right")
                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesRight[2], "Right")
                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesRight[3], "Right")

                                        && rpSel.MasLinesRight[0].Y < rp.MasLinesRight[0].Y
                                        && rpSel.MasLinesRight[1].Y < rp.MasLinesRight[1].Y
                                        && rpSel.MasLinesRight[2].Y > rp.MasLinesRight[2].Y
                                        && rpSel.MasLinesRight[3].Y > rp.MasLinesRight[3].Y

                                        && IsPointInsideTheObject(rp, rpSel.MasLinesTop[0], "Top")
                                        && IsPointInsideTheObject(rp, rpSel.MasLinesTop[1], "Top")
                                        && IsPointInsideTheObject(rp, rpSel.MasLinesTop[2], "Top")
                                        && IsPointInsideTheObject(rp, rpSel.MasLinesTop[3], "Top")
                                        )
                                    {
                                        #region CreateFrontPrimirtive

                                        RectPrimitive rpFront = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                        #region FrontMassive

                                        rpFront.MasLinesFront = rp.MasLinesFront;

                                        #endregion
                                        #region RightMassive

                                        rpFront.MasLinesRight[0] = rp.MasLinesRight[0];
                                        rpFront.MasLinesRight[3] = rp.MasLinesRight[3];
                                        rpFront.MasLinesRight[4] = rp.MasLinesRight[0];
                                        rpFront.MasLinesRight[5] = rp.MasLinesRight[0];
                                        rpFront.MasLinesRight[8] = rp.MasLinesRight[8];
                                        rpFront.MasLinesRight[9] = rp.MasLinesRight[9];

                                        rpFront.MasLinesRight[1].X = rpSel.MasLinesRight[0].X;
                                        rpFront.MasLinesRight[1].Y = rp.MasLinesRight[0].Y;
                                        rpFront.MasLinesRight[2].X = rpSel.MasLinesRight[3].X;
                                        rpFront.MasLinesRight[2].Y = rp.MasLinesRight[3].Y;

                                        rpFront.MasLinesRight[6].X = rpSel.MasLinesRight[0].X;
                                        rpFront.MasLinesRight[6].Y = rp.MasLinesRight[0].Y;
                                        rpFront.MasLinesRight[7].X = rpSel.MasLinesRight[3].X;
                                        rpFront.MasLinesRight[7].Y = rp.MasLinesRight[3].Y;

                                        #endregion
                                        #region TopMassive

                                        rpFront.MasLinesTop[0].X = rp.MasLinesTop[3].X;
                                        rpFront.MasLinesTop[0].Y = rpSel.MasLinesTop[3].Y;
                                        rpFront.MasLinesTop[1].X = rp.MasLinesTop[2].X;
                                        rpFront.MasLinesTop[1].Y = rpSel.MasLinesTop[2].Y;
                                        rpFront.MasLinesTop[2] = rp.MasLinesTop[2];
                                        rpFront.MasLinesTop[3] = rp.MasLinesTop[3];
                                        rpFront.MasLinesTop[4].X = rp.MasLinesTop[3].X;
                                        rpFront.MasLinesTop[4].Y = rpSel.MasLinesTop[3].Y;

                                        rpFront.MasLinesTop[5].X = rp.MasLinesTop[8].X;
                                        rpFront.MasLinesTop[5].Y = rpSel.MasLinesTop[8].Y;
                                        rpFront.MasLinesTop[6].X = rp.MasLinesTop[7].X;
                                        rpFront.MasLinesTop[6].Y = rpSel.MasLinesTop[7].Y;
                                        rpFront.MasLinesTop[7] = rp.MasLinesTop[7];
                                        rpFront.MasLinesTop[8] = rp.MasLinesTop[8];
                                        rpFront.MasLinesTop[9].X = rp.MasLinesTop[8].X;
                                        rpFront.MasLinesTop[9].Y = rpSel.MasLinesTop[8].Y;

                                        #endregion

                                        int k = managed2D.GetNextKey();
                                        primitivesForAdd.Add(k, rpFront);

                                        #endregion
                                        #region CreateRightPrimitive

                                        RectPrimitive rpRight = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                        #region FronMassive

                                        rpRight.MasLinesFront[0].X = rpSel.MasLinesFront[1].X;
                                        rpRight.MasLinesFront[0].Y = rp.MasLinesFront[1].Y;
                                        rpRight.MasLinesFront[1] = rp.MasLinesFront[1];
                                        rpRight.MasLinesFront[2] = rp.MasLinesFront[2];
                                        rpRight.MasLinesFront[3].X = rpSel.MasLinesFront[2].X;
                                        rpRight.MasLinesFront[3].Y = rp.MasLinesFront[2].Y;
                                        rpRight.MasLinesFront[4].X = rpSel.MasLinesFront[1].X;
                                        rpRight.MasLinesFront[4].Y = rp.MasLinesFront[1].Y;

                                        rpRight.MasLinesFront[5].X = rpSel.MasLinesFront[1].X;
                                        rpRight.MasLinesFront[5].Y = rp.MasLinesFront[1].Y;
                                        rpRight.MasLinesFront[6] = rp.MasLinesFront[1];
                                        rpRight.MasLinesFront[7] = rp.MasLinesFront[2];
                                        rpRight.MasLinesFront[8].X = rpSel.MasLinesFront[2].X;
                                        rpRight.MasLinesFront[8].Y = rp.MasLinesFront[2].Y;
                                        rpRight.MasLinesFront[9].X = rpSel.MasLinesFront[1].X;
                                        rpRight.MasLinesFront[9].Y = rp.MasLinesFront[1].Y;

                                        #endregion
                                        #region RightMassive

                                        rpRight.MasLinesRight[0].X = rpSel.MasLinesRight[0].X;
                                        rpRight.MasLinesRight[0].Y = rp.MasLinesRight[0].Y;
                                        rpRight.MasLinesRight[1] = rp.MasLinesRight[1];
                                        rpRight.MasLinesRight[2] = rp.MasLinesRight[2];
                                        rpRight.MasLinesRight[3].X = rpSel.MasLinesRight[3].X;
                                        rpRight.MasLinesRight[3].Y = rp.MasLinesRight[3].Y;
                                        rpRight.MasLinesRight[4].X = rpSel.MasLinesRight[0].X;
                                        rpRight.MasLinesRight[4].Y = rp.MasLinesRight[0].Y;

                                        rpRight.MasLinesRight[5].X = rpSel.MasLinesRight[0].X;
                                        rpRight.MasLinesRight[5].Y = rp.MasLinesRight[0].Y;
                                        rpRight.MasLinesRight[6] = rp.MasLinesRight[1];
                                        rpRight.MasLinesRight[7] = rp.MasLinesRight[2];
                                        rpRight.MasLinesRight[8].X = rpSel.MasLinesRight[3].X;
                                        rpRight.MasLinesRight[8].Y = rp.MasLinesRight[3].Y;
                                        rpRight.MasLinesRight[9].X = rpSel.MasLinesRight[0].X;
                                        rpRight.MasLinesRight[9].Y = rp.MasLinesRight[0].Y;

                                        #endregion
                                        #region TopMassive

                                        rpRight.MasLinesTop[0].X = rpSel.MasLinesTop[1].X;
                                        rpRight.MasLinesTop[0].Y = rp.MasLinesTop[1].Y;
                                        rpRight.MasLinesTop[1] = rp.MasLinesTop[1];
                                        rpRight.MasLinesTop[2].X = rp.MasLinesTop[1].X;
                                        rpRight.MasLinesTop[2].Y = rpSel.MasLinesTop[2].Y;
                                        rpRight.MasLinesTop[3] = rpSel.MasLinesTop[2];
                                        rpRight.MasLinesTop[4].X = rpSel.MasLinesTop[1].X;
                                        rpRight.MasLinesTop[4].Y = rp.MasLinesTop[1].Y;

                                        rpRight.MasLinesTop[5].X = rpSel.MasLinesTop[1].X;
                                        rpRight.MasLinesTop[5].Y = rp.MasLinesTop[1].Y;
                                        rpRight.MasLinesTop[6] = rp.MasLinesTop[1];
                                        rpRight.MasLinesTop[7].X = rp.MasLinesTop[1].X;
                                        rpRight.MasLinesTop[7].Y = rpSel.MasLinesTop[2].Y;
                                        rpRight.MasLinesTop[8] = rpSel.MasLinesTop[2];
                                        rpRight.MasLinesTop[9].X = rpSel.MasLinesTop[1].X;
                                        rpRight.MasLinesTop[9].Y = rp.MasLinesTop[1].Y;


                                        #endregion

                                        k = managed2D.GetNextKey();
                                        primitivesForAdd.Add(k, rpRight);

                                        #endregion
                                        #region CreteBackPrimitive

                                        RectPrimitive rpBack = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                        #region FrontMassive

                                        rpBack.MasLinesFront[0] = rp.MasLinesFront[0];
                                        rpBack.MasLinesFront[1].X = rpSel.MasLinesFront[1].X;
                                        rpBack.MasLinesFront[1].Y = rp.MasLinesFront[1].Y;
                                        rpBack.MasLinesFront[2].X = rpSel.MasLinesFront[2].X;
                                        rpBack.MasLinesFront[2].Y = rp.MasLinesFront[2].Y;
                                        rpBack.MasLinesFront[3] = rp.MasLinesFront[3];
                                        rpBack.MasLinesFront[4] = rp.MasLinesFront[0];

                                        rpBack.MasLinesFront[5] = rp.MasLinesFront[0];
                                        rpBack.MasLinesFront[6].X = rpSel.MasLinesFront[1].X;
                                        rpBack.MasLinesFront[6].Y = rp.MasLinesFront[1].Y;
                                        rpBack.MasLinesFront[7].X = rpSel.MasLinesFront[2].X;
                                        rpBack.MasLinesFront[7].Y = rp.MasLinesFront[2].Y;
                                        rpBack.MasLinesFront[8] = rp.MasLinesFront[3];
                                        rpBack.MasLinesFront[9] = rp.MasLinesFront[0];

                                        #endregion
                                        #region RightMassive

                                        rpBack.MasLinesRight[0].X = rpSel.MasLinesRight[1].X;
                                        rpBack.MasLinesRight[0].Y = rp.MasLinesRight[1].Y;
                                        rpBack.MasLinesRight[1] = rp.MasLinesRight[1];
                                        rpBack.MasLinesRight[2] = rp.MasLinesRight[2];
                                        rpBack.MasLinesRight[3].X = rpSel.MasLinesRight[2].X;
                                        rpBack.MasLinesRight[3].Y = rp.MasLinesRight[2].Y;
                                        rpBack.MasLinesRight[4].X = rpSel.MasLinesRight[1].X;
                                        rpBack.MasLinesRight[4].Y = rp.MasLinesRight[1].Y;

                                        rpBack.MasLinesRight[5].X = rpSel.MasLinesRight[1].X;
                                        rpBack.MasLinesRight[5].Y = rp.MasLinesRight[1].Y;
                                        rpBack.MasLinesRight[6] = rp.MasLinesRight[1];
                                        rpBack.MasLinesRight[7] = rp.MasLinesRight[2];
                                        rpBack.MasLinesRight[8].X = rpSel.MasLinesRight[2].X;
                                        rpBack.MasLinesRight[8].Y = rp.MasLinesRight[2].Y;
                                        rpBack.MasLinesRight[9].X = rpSel.MasLinesRight[1].X;
                                        rpBack.MasLinesRight[9].Y = rp.MasLinesRight[1].Y;

                                        #endregion
                                        #region TopMassive

                                        rpBack.MasLinesTop[0] = rp.MasLinesTop[0];
                                        rpBack.MasLinesTop[1].X = rpSel.MasLinesTop[1].X;
                                        rpBack.MasLinesTop[1].Y = rp.MasLinesTop[1].Y;
                                        rpBack.MasLinesTop[2] = rpSel.MasLinesTop[1];
                                        rpBack.MasLinesTop[3].X = rp.MasLinesTop[0].X;
                                        rpBack.MasLinesTop[3].Y = rpSel.MasLinesTop[0].Y;
                                        rpBack.MasLinesTop[4] = rp.MasLinesTop[0];

                                        rpBack.MasLinesTop[5] = rp.MasLinesTop[0];
                                        rpBack.MasLinesTop[6].X = rpSel.MasLinesTop[1].X;
                                        rpBack.MasLinesTop[6].Y = rp.MasLinesTop[1].Y;
                                        rpBack.MasLinesTop[7] = rpSel.MasLinesTop[1];
                                        rpBack.MasLinesTop[8].X = rp.MasLinesTop[0].X;
                                        rpBack.MasLinesTop[8].Y = rpSel.MasLinesTop[0].Y;
                                        rpBack.MasLinesTop[9] = rp.MasLinesTop[0];

                                        #endregion

                                        k = managed2D.GetNextKey();
                                        primitivesForAdd.Add(k, rpBack);

                                        #endregion
                                        #region CreateLeftPrimitive

                                        RectPrimitive rpLeft = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                        #region FrontMassive

                                        rpLeft.MasLinesFront[0] = rp.MasLinesFront[0];
                                        rpLeft.MasLinesFront[1].X = rpSel.MasLinesFront[0].X;
                                        rpLeft.MasLinesFront[1].Y = rp.MasLinesFront[0].Y;
                                        rpLeft.MasLinesFront[2].X = rpSel.MasLinesFront[3].X;
                                        rpLeft.MasLinesFront[2].Y = rp.MasLinesFront[3].Y;
                                        rpLeft.MasLinesFront[3] = rp.MasLinesFront[3];
                                        rpLeft.MasLinesFront[4] = rp.MasLinesFront[0];

                                        rpLeft.MasLinesFront[5] = rp.MasLinesFront[0];
                                        rpLeft.MasLinesFront[6].X = rpSel.MasLinesFront[0].X;
                                        rpLeft.MasLinesFront[6].Y = rp.MasLinesFront[0].Y;
                                        rpLeft.MasLinesFront[7].X = rpSel.MasLinesFront[3].X;
                                        rpLeft.MasLinesFront[7].Y = rp.MasLinesFront[3].Y;
                                        rpLeft.MasLinesFront[8] = rp.MasLinesFront[3];
                                        rpLeft.MasLinesFront[9] = rp.MasLinesFront[0];

                                        #endregion
                                        #region RightMassive

                                        rpLeft.MasLinesRight[0].X = rpSel.MasLinesRight[0].X;
                                        rpLeft.MasLinesRight[0].Y = rp.MasLinesRight[0].Y;
                                        rpLeft.MasLinesRight[1].X = rpSel.MasLinesRight[1].X;
                                        rpLeft.MasLinesRight[1].Y = rp.MasLinesRight[1].Y;
                                        rpLeft.MasLinesRight[2].X = rpSel.MasLinesRight[2].X;
                                        rpLeft.MasLinesRight[2].Y = rp.MasLinesRight[2].Y;
                                        rpLeft.MasLinesRight[3].X = rpSel.MasLinesRight[3].X;
                                        rpLeft.MasLinesRight[3].Y = rp.MasLinesRight[3].Y;
                                        rpLeft.MasLinesRight[4].X = rpSel.MasLinesRight[0].X;
                                        rpLeft.MasLinesRight[4].Y = rp.MasLinesRight[0].Y;

                                        rpLeft.MasLinesRight[5].X = rpSel.MasLinesRight[0].X;
                                        rpLeft.MasLinesRight[5].Y = rp.MasLinesRight[0].Y;
                                        rpLeft.MasLinesRight[6].X = rpSel.MasLinesRight[1].X;
                                        rpLeft.MasLinesRight[6].Y = rp.MasLinesRight[1].Y;
                                        rpLeft.MasLinesRight[7].X = rpSel.MasLinesRight[2].X;
                                        rpLeft.MasLinesRight[7].Y = rp.MasLinesRight[2].Y;
                                        rpLeft.MasLinesRight[8].X = rpSel.MasLinesRight[3].X;
                                        rpLeft.MasLinesRight[8].Y = rp.MasLinesRight[3].Y;
                                        rpLeft.MasLinesRight[9].X = rpSel.MasLinesRight[0].X;
                                        rpLeft.MasLinesRight[9].Y = rp.MasLinesRight[0].Y;

                                        #endregion
                                        #region TopMassive

                                        rpLeft.MasLinesTop[0].X = rp.MasLinesTop[0].X;
                                        rpLeft.MasLinesTop[0].Y = rpSel.MasLinesTop[0].Y;
                                        rpLeft.MasLinesTop[1] = rpSel.MasLinesTop[0];
                                        rpLeft.MasLinesTop[2] = rpSel.MasLinesTop[3];
                                        rpLeft.MasLinesTop[3].X = rp.MasLinesTop[3].X;
                                        rpLeft.MasLinesTop[3].Y = rpSel.MasLinesTop[3].Y;
                                        rpLeft.MasLinesTop[4].X = rp.MasLinesTop[0].X;
                                        rpLeft.MasLinesTop[4].Y = rpSel.MasLinesTop[0].Y;

                                        rpLeft.MasLinesTop[5].X = rp.MasLinesTop[0].X;
                                        rpLeft.MasLinesTop[5].Y = rpSel.MasLinesTop[0].Y;
                                        rpLeft.MasLinesTop[6] = rpSel.MasLinesTop[0];
                                        rpLeft.MasLinesTop[7] = rpSel.MasLinesTop[3];
                                        rpLeft.MasLinesTop[8].X = rp.MasLinesTop[3].X;
                                        rpLeft.MasLinesTop[8].Y = rpSel.MasLinesTop[3].Y;
                                        rpLeft.MasLinesTop[9].X = rp.MasLinesTop[0].X;
                                        rpLeft.MasLinesTop[9].Y = rpSel.MasLinesTop[0].Y;

                                        #endregion

                                        k = managed2D.GetNextKey();
                                        primitivesForAdd.Add(k, rpLeft);

                                        #endregion


                                        masKeysOfPrimitivesForDelete.Add(key);
                                    }

                                    #endregion
                                    #region CheckRightProjectionToInsideAllPoints

                                    if (
                                           !IsPointInsideTheObject(rp, rpSel.MasLinesFront[0], "Front")
                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesFront[1], "Front")
                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesFront[2], "Front")
                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesFront[3], "Front")

                                        && rpSel.MasLinesFront[0].X < rp.MasLinesFront[0].X
                                        && rpSel.MasLinesFront[1].X > rp.MasLinesFront[1].X
                                        && rpSel.MasLinesFront[2].X > rp.MasLinesFront[2].X
                                        && rpSel.MasLinesFront[3].X < rp.MasLinesFront[3].X

                                        && IsPointInsideTheObject(rp, rpSel.MasLinesRight[0], "Right")
                                        && IsPointInsideTheObject(rp, rpSel.MasLinesRight[0], "Right")
                                        && IsPointInsideTheObject(rp, rpSel.MasLinesRight[0], "Right")
                                        && IsPointInsideTheObject(rp, rpSel.MasLinesRight[0], "Right")

                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesTop[0], "Top")
                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesTop[1], "Top")
                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesTop[2], "Top")
                                        && !IsPointInsideTheObject(rp, rpSel.MasLinesTop[3], "Top")

                                        && rpSel.MasLinesTop[0].X < rp.MasLinesTop[0].X
                                        && rpSel.MasLinesTop[1].X > rp.MasLinesTop[1].X
                                        && rpSel.MasLinesTop[2].X > rp.MasLinesTop[2].X
                                        && rpSel.MasLinesTop[3].X < rp.MasLinesTop[3].X
                                        )
                                    {
                                        #region CreateFrontPrimitive

                                        RectPrimitive rpFront = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                        #region FrontMassive

                                        rpFront.MasLinesFront = rp.MasLinesFront;

                                        #endregion
                                        #region RightMassive

                                        rpFront.MasLinesRight[0] = rp.MasLinesRight[0];
                                        rpFront.MasLinesRight[1].X = rpSel.MasLinesRight[0].X;
                                        rpFront.MasLinesRight[1].Y = rp.MasLinesRight[0].Y;
                                        rpFront.MasLinesRight[2].X = rpSel.MasLinesRight[3].X;
                                        rpFront.MasLinesRight[2].Y = rp.MasLinesRight[3].Y;
                                        rpFront.MasLinesRight[3] = rp.MasLinesRight[3];
                                        rpFront.MasLinesRight[4] = rp.MasLinesRight[0];

                                        rpFront.MasLinesRight[5] = rp.MasLinesRight[0];
                                        rpFront.MasLinesRight[6].X = rpSel.MasLinesRight[0].X;
                                        rpFront.MasLinesRight[6].Y = rp.MasLinesRight[0].Y;
                                        rpFront.MasLinesRight[7].X = rpSel.MasLinesRight[3].X;
                                        rpFront.MasLinesRight[7].Y = rp.MasLinesRight[3].Y;
                                        rpFront.MasLinesRight[8] = rp.MasLinesRight[3];
                                        rpFront.MasLinesRight[9] = rp.MasLinesRight[0];


                                        #endregion
                                        #region TopMassive

                                        rpFront.MasLinesTop[0].X = rp.MasLinesTop[3].X;
                                        rpFront.MasLinesTop[0].Y = rpSel.MasLinesTop[3].Y;
                                        rpFront.MasLinesTop[1].X = rp.MasLinesTop[2].X;
                                        rpFront.MasLinesTop[1].Y = rpSel.MasLinesTop[2].Y;
                                        rpFront.MasLinesTop[2] = rp.MasLinesTop[2];
                                        rpFront.MasLinesTop[3] = rp.MasLinesTop[3];
                                        rpFront.MasLinesTop[4].X = rp.MasLinesTop[3].X;
                                        rpFront.MasLinesTop[4].Y = rpSel.MasLinesTop[3].Y;

                                        rpFront.MasLinesTop[5].X = rp.MasLinesTop[3].X;
                                        rpFront.MasLinesTop[5].Y = rpSel.MasLinesTop[3].Y;
                                        rpFront.MasLinesTop[6].X = rp.MasLinesTop[2].X;
                                        rpFront.MasLinesTop[6].Y = rpSel.MasLinesTop[2].Y;
                                        rpFront.MasLinesTop[7] = rp.MasLinesTop[2];
                                        rpFront.MasLinesTop[8] = rp.MasLinesTop[3];
                                        rpFront.MasLinesTop[9].X = rp.MasLinesTop[3].X;
                                        rpFront.MasLinesTop[9].Y = rpSel.MasLinesTop[3].Y;

                                        #endregion

                                        int k = managed2D.GetNextKey();
                                        primitivesForAdd.Add(k, rpFront);

                                        #endregion
                                        #region CreateBackPrimitive

                                        RectPrimitive rpBack = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                        #region FrontMassive

                                        rpBack.MasLinesFront = rp.MasLinesFront;

                                        #endregion
                                        #region RightMassive

                                        rpBack.MasLinesRight[0].X = rpSel.MasLinesRight[1].X;
                                        rpBack.MasLinesRight[0].Y = rp.MasLinesRight[1].Y;
                                        rpBack.MasLinesRight[1] = rp.MasLinesRight[1];
                                        rpBack.MasLinesRight[2] = rp.MasLinesRight[2];
                                        rpBack.MasLinesRight[3].X = rpSel.MasLinesRight[2].X;
                                        rpBack.MasLinesRight[3].Y = rp.MasLinesRight[2].Y;
                                        rpBack.MasLinesRight[4].X = rpSel.MasLinesRight[1].X;
                                        rpBack.MasLinesRight[4].Y = rp.MasLinesRight[1].Y;

                                        rpBack.MasLinesRight[5].X = rpSel.MasLinesRight[1].X;
                                        rpBack.MasLinesRight[5].Y = rp.MasLinesRight[1].Y;
                                        rpBack.MasLinesRight[6] = rp.MasLinesRight[1];
                                        rpBack.MasLinesRight[7] = rp.MasLinesRight[2];
                                        rpBack.MasLinesRight[8].X = rpSel.MasLinesRight[2].X;
                                        rpBack.MasLinesRight[8].Y = rp.MasLinesRight[2].Y;
                                        rpBack.MasLinesRight[9].X = rpSel.MasLinesRight[1].X;
                                        rpBack.MasLinesRight[9].Y = rp.MasLinesRight[1].Y;

                                        #endregion
                                        #region TopMassive

                                        rpBack.MasLinesTop[0] = rp.MasLinesTop[0];
                                        rpBack.MasLinesTop[1] = rp.MasLinesTop[1];
                                        rpBack.MasLinesTop[2].X = rp.MasLinesTop[1].X;
                                        rpBack.MasLinesTop[2].Y = rpSel.MasLinesTop[1].Y;
                                        rpBack.MasLinesTop[3].X = rp.MasLinesTop[0].X;
                                        rpBack.MasLinesTop[3].Y = rpSel.MasLinesTop[0].Y;
                                        rpBack.MasLinesTop[4] = rp.MasLinesTop[0];

                                        rpBack.MasLinesTop[5] = rp.MasLinesTop[0];
                                        rpBack.MasLinesTop[6] = rp.MasLinesTop[1];
                                        rpBack.MasLinesTop[7].X = rp.MasLinesTop[1].X;
                                        rpBack.MasLinesTop[7].Y = rpSel.MasLinesTop[1].Y;
                                        rpBack.MasLinesTop[8].X = rp.MasLinesTop[0].X;
                                        rpBack.MasLinesTop[8].Y = rpSel.MasLinesTop[0].Y;
                                        rpBack.MasLinesTop[9] = rp.MasLinesTop[0];

                                        #endregion

                                        k = managed2D.GetNextKey();
                                        primitivesForAdd.Add(k, rpBack);

                                        #endregion
                                        #region CreateUpPrimitive

                                        RectPrimitive rpUp = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                        #region FrontMassive

                                        rpUp.MasLinesFront[0] = rp.MasLinesFront[0];
                                        rpUp.MasLinesFront[1] = rp.MasLinesFront[1];
                                        rpUp.MasLinesFront[2].X = rp.MasLinesFront[1].X;
                                        rpUp.MasLinesFront[2].Y = rpSel.MasLinesFront[1].Y;
                                        rpUp.MasLinesFront[3].X = rp.MasLinesFront[0].X;
                                        rpUp.MasLinesFront[3].Y = rpSel.MasLinesFront[0].Y;
                                        rpUp.MasLinesFront[4] = rp.MasLinesFront[4];

                                        rpUp.MasLinesFront[5] = rp.MasLinesFront[0];
                                        rpUp.MasLinesFront[6] = rp.MasLinesFront[1];
                                        rpUp.MasLinesFront[7].X = rp.MasLinesFront[1].X;
                                        rpUp.MasLinesFront[7].Y = rpSel.MasLinesFront[1].Y;
                                        rpUp.MasLinesFront[8].X = rp.MasLinesFront[0].X;
                                        rpUp.MasLinesFront[8].Y = rpSel.MasLinesFront[0].Y;
                                        rpUp.MasLinesFront[9] = rp.MasLinesFront[4];


                                        #endregion
                                        #region RightMassive

                                        rpUp.MasLinesRight[0].X = rpSel.MasLinesRight[0].X;
                                        rpUp.MasLinesRight[0].Y = rp.MasLinesRight[0].Y;
                                        rpUp.MasLinesRight[1].X = rpSel.MasLinesRight[1].X;
                                        rpUp.MasLinesRight[1].Y = rp.MasLinesRight[1].Y;
                                        rpUp.MasLinesRight[2] = rpSel.MasLinesRight[1];
                                        rpUp.MasLinesRight[3] = rpSel.MasLinesRight[0];
                                        rpUp.MasLinesRight[4].X = rpSel.MasLinesRight[0].X;
                                        rpUp.MasLinesRight[4].Y = rp.MasLinesRight[0].Y;

                                        rpUp.MasLinesRight[5].X = rpSel.MasLinesRight[0].X;
                                        rpUp.MasLinesRight[5].Y = rp.MasLinesRight[0].Y;
                                        rpUp.MasLinesRight[6].X = rpSel.MasLinesRight[1].X;
                                        rpUp.MasLinesRight[6].Y = rp.MasLinesRight[1].Y;
                                        rpUp.MasLinesRight[7] = rpSel.MasLinesRight[1];
                                        rpUp.MasLinesRight[8] = rpSel.MasLinesRight[0];
                                        rpUp.MasLinesRight[9].X = rpSel.MasLinesRight[0].X;
                                        rpUp.MasLinesRight[9].Y = rp.MasLinesRight[0].Y;


                                        #endregion
                                        #region TopMassive

                                        rpUp.MasLinesTop[0].X = rp.MasLinesTop[0].X;
                                        rpUp.MasLinesTop[0].Y = rpSel.MasLinesTop[0].Y;
                                        rpUp.MasLinesTop[1].X = rp.MasLinesTop[1].X;
                                        rpUp.MasLinesTop[1].Y = rpSel.MasLinesTop[1].Y;
                                        rpUp.MasLinesTop[2].X = rp.MasLinesTop[2].X;
                                        rpUp.MasLinesTop[2].Y = rpSel.MasLinesTop[2].Y;
                                        rpUp.MasLinesTop[3].X = rp.MasLinesTop[3].X;
                                        rpUp.MasLinesTop[3].Y = rpSel.MasLinesTop[3].Y;
                                        rpUp.MasLinesTop[4].X = rp.MasLinesTop[0].X;
                                        rpUp.MasLinesTop[4].Y = rpSel.MasLinesTop[0].Y;

                                        rpUp.MasLinesTop[5].X = rp.MasLinesTop[0].X;
                                        rpUp.MasLinesTop[5].Y = rpSel.MasLinesTop[0].Y;
                                        rpUp.MasLinesTop[6].X = rp.MasLinesTop[1].X;
                                        rpUp.MasLinesTop[6].Y = rpSel.MasLinesTop[1].Y;
                                        rpUp.MasLinesTop[7].X = rp.MasLinesTop[2].X;
                                        rpUp.MasLinesTop[7].Y = rpSel.MasLinesTop[2].Y;
                                        rpUp.MasLinesTop[8].X = rp.MasLinesTop[3].X;
                                        rpUp.MasLinesTop[8].Y = rpSel.MasLinesTop[3].Y;
                                        rpUp.MasLinesTop[9].X = rp.MasLinesTop[0].X;
                                        rpUp.MasLinesTop[9].Y = rpSel.MasLinesTop[0].Y;

                                        #endregion

                                        k = managed2D.GetNextKey();
                                        primitivesForAdd.Add(k, rpUp);

                                        #endregion
                                        #region Create DownPrimitive

                                        RectPrimitive rpDown = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());

                                        #region FrontMassive

                                        rpDown.MasLinesFront[0].X = rp.MasLinesFront[3].X;
                                        rpDown.MasLinesFront[0].Y = rpSel.MasLinesFront[3].Y;
                                        rpDown.MasLinesFront[1].X = rp.MasLinesFront[2].X;
                                        rpDown.MasLinesFront[1].Y = rpSel.MasLinesFront[2].Y;
                                        rpDown.MasLinesFront[2] = rp.MasLinesFront[2];
                                        rpDown.MasLinesFront[3] = rp.MasLinesFront[3];
                                        rpDown.MasLinesFront[4].X = rp.MasLinesFront[3].X;
                                        rpDown.MasLinesFront[4].Y = rpSel.MasLinesFront[3].Y;

                                        rpDown.MasLinesFront[5].X = rp.MasLinesFront[3].X;
                                        rpDown.MasLinesFront[5].Y = rpSel.MasLinesFront[3].Y;
                                        rpDown.MasLinesFront[6].X = rp.MasLinesFront[2].X;
                                        rpDown.MasLinesFront[6].Y = rpSel.MasLinesFront[2].Y;
                                        rpDown.MasLinesFront[7] = rp.MasLinesFront[2];
                                        rpDown.MasLinesFront[8] = rp.MasLinesFront[3];
                                        rpDown.MasLinesFront[9].X = rp.MasLinesFront[3].X;
                                        rpDown.MasLinesFront[9].Y = rpSel.MasLinesFront[3].Y;

                                        #endregion
                                        #region RightMassive

                                        rpDown.MasLinesRight[0] = rpSel.MasLinesRight[3];
                                        rpDown.MasLinesRight[1] = rpSel.MasLinesRight[2];
                                        rpDown.MasLinesRight[2].X = rpSel.MasLinesRight[2].X;
                                        rpDown.MasLinesRight[2].Y = rp.MasLinesRight[2].Y;
                                        rpDown.MasLinesRight[3].X = rpSel.MasLinesRight[3].X;
                                        rpDown.MasLinesRight[3].Y = rp.MasLinesRight[3].Y;
                                        rpDown.MasLinesRight[4] = rpSel.MasLinesRight[3];

                                        rpDown.MasLinesRight[5] = rpSel.MasLinesRight[3];
                                        rpDown.MasLinesRight[6] = rpSel.MasLinesRight[2];
                                        rpDown.MasLinesRight[7].X = rpSel.MasLinesRight[2].X;
                                        rpDown.MasLinesRight[7].Y = rp.MasLinesRight[2].Y;
                                        rpDown.MasLinesRight[8].X = rpSel.MasLinesRight[3].X;
                                        rpDown.MasLinesRight[8].Y = rp.MasLinesRight[3].Y;
                                        rpDown.MasLinesRight[9] = rpSel.MasLinesRight[3];

                                        #endregion
                                        #region TopMassive

                                        rpDown.MasLinesTop[0].X = rp.MasLinesTop[0].X;
                                        rpDown.MasLinesTop[0].Y = rpSel.MasLinesTop[0].Y;
                                        rpDown.MasLinesTop[1].X = rp.MasLinesTop[1].X;
                                        rpDown.MasLinesTop[1].Y = rpSel.MasLinesTop[1].Y;
                                        rpDown.MasLinesTop[2].X = rp.MasLinesTop[2].X;
                                        rpDown.MasLinesTop[2].Y = rpSel.MasLinesTop[2].Y;
                                        rpDown.MasLinesTop[3].X = rp.MasLinesTop[3].X;
                                        rpDown.MasLinesTop[3].Y = rpSel.MasLinesTop[3].Y;
                                        rpDown.MasLinesTop[4].X = rp.MasLinesTop[0].X;
                                        rpDown.MasLinesTop[4].Y = rpSel.MasLinesTop[0].Y;

                                        rpDown.MasLinesTop[5].X = rp.MasLinesTop[0].X;
                                        rpDown.MasLinesTop[5].Y = rpSel.MasLinesTop[0].Y;
                                        rpDown.MasLinesTop[6].X = rp.MasLinesTop[1].X;
                                        rpDown.MasLinesTop[6].Y = rpSel.MasLinesTop[1].Y;
                                        rpDown.MasLinesTop[7].X = rp.MasLinesTop[2].X;
                                        rpDown.MasLinesTop[7].Y = rpSel.MasLinesTop[2].Y;
                                        rpDown.MasLinesTop[8].X = rp.MasLinesTop[3].X;
                                        rpDown.MasLinesTop[8].Y = rpSel.MasLinesTop[3].Y;
                                        rpDown.MasLinesTop[9].X = rp.MasLinesTop[0].X;
                                        rpDown.MasLinesTop[9].Y = rpSel.MasLinesTop[0].Y;

                                        #endregion

                                        k = managed2D.GetNextKey();
                                        primitivesForAdd.Add(k, rpDown);

                                        #endregion


                                        masKeysOfPrimitivesForDelete.Add(key);
                                    }

                                    #endregion
                                }
                            }
                        }
                        foreach (int k in primitivesForAdd.Keys)
                        {
                            RectPrimitive rAdd = (RectPrimitive)primitivesForAdd[k];
                            managed2D.List2DPrimitives.Add(k,rAdd );
                            RectPrimitive conv = GetConvertedPrimitiveToLocalCoords(rAdd);
                            managed3D.AddObject(conv, "Textures\\" + lastTexture, k);
                        }
                        foreach (int k in masKeysOfPrimitivesForDelete)
                        {
                            managed2D.List2DPrimitives.Remove(k);
                            managed3D.objectsList.Remove(k);
                        }
                    }
                }
            }
        }
        private bool IsPointInsideTheObject(RectPrimitive rp,Point p,string nameSide)
        {
            if (nameSide == "Front")
            {
                if (
                       p.X > rp.MasLinesFront[0].X
                    && p.X < rp.MasLinesFront[1].X
                    && p.Y > rp.MasLinesFront[0].Y
                    && p.Y < rp.MasLinesFront[3].Y
                    )
                    return true;
            }

            if (nameSide == "Right")
            {
                if (
                       p.X > rp.MasLinesRight[0].X
                    && p.X < rp.MasLinesRight[1].X
                    && p.Y > rp.MasLinesRight[0].Y
                    && p.Y < rp.MasLinesRight[3].Y
                    )
                    return true;
            }

            if (nameSide == "Top")
            {
                if (
                       p.X > rp.MasLinesTop[0].X
                    && p.X < rp.MasLinesTop[1].X
                    && p.Y > rp.MasLinesTop[0].Y
                    && p.Y < rp.MasLinesTop[3].Y
                    )
                    return true;
            }
            return false;
        }
        private void MoveHolst(object sender, bool isToNegativeSide, bool isHorizontalScroll)
        {
            if (isHorizontalScroll)
            {
                if (isToNegativeSide)
                {
                    if (((Panel)sender).HorizontalScroll.Value - 20 > 0)
                        ((Panel)sender).HorizontalScroll.Value -= 20;
                    else
                        ((Panel)sender).HorizontalScroll.Value = 0;
                }
                else
                {
                    if (((Panel)sender).HorizontalScroll.Value + 20 < panelWidth)
                        ((Panel)sender).HorizontalScroll.Value += 20;
                    else
                        ((Panel)sender).HorizontalScroll.Value = panelWidth;
                }
            }
            else
            {
                if (isToNegativeSide)
                {
                    if (((Panel)sender).VerticalScroll.Value - 20 > 0)
                        ((Panel)sender).VerticalScroll.Value -= 20;
                    else
                        ((Panel)sender).VerticalScroll.Value = 0;
                }
                else
                {
                    if (((Panel)sender).VerticalScroll.Value + 20 < panelHeight)
                        ((Panel)sender).VerticalScroll.Value += 20;
                    else
                        ((Panel)sender).VerticalScroll.Value = panelHeight;
                }
            }
        }
		private void UncheckedAllCheckFuncButtons()
		{
			checkBoxRect.Checked = false;
			checkBoxPlayerRed.Checked = false;
			checkBoxPlayerBlue.Checked = false;
			checkBoxPointer.Checked = false;
		}

        #endregion
        #region FunctionsEventsToolStripMenu

        private void saveMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveMapToFile();
        }

        private void openMapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenMapFromFile();
			timerRight.Enabled = timerTop.Enabled = timerFront.Enabled = true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
			timerRight.Enabled = timerTop.Enabled = timerFront.Enabled = false;

			managed2D.Dispose();
			managed3D.Dispose();

			//GC.Collect(g1);
			//GC.Collect(g2);

			long sizeMemory = GC.GetTotalMemory(true);

			managed2D = new Managed2DPrimitives(selection, lineSelection);
			managed3D = new Managed3D(pictureBox3D.Handle, this);
			timerRight.Enabled = timerTop.Enabled = timerFront.Enabled = true;
        }
		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
            this.Close();
		}

        #endregion
		#region EventsResize
		private void panelTop_Resize(object sender, EventArgs e)
		{
			sizeOfTopPanel = panelTop.Size;
		}

		private void panelFront_Resize(object sender, EventArgs e)
		{
			sizeOfFrontPanel = panelFront.Size;
		}

		private void panelRight_Resize(object sender, EventArgs e)
		{
			sizeOfRightPanel = panelRight.Size;
		}
		private void pictureBox3D_Resize(object sender, EventArgs e)
		{
			Box3D = pictureBox3D.Size;
		}
		#endregion

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (int key in managed2D.List2DPrimitives.Keys)
            {
                RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];
                if (rp.IsSelected)
                {
                    //rp.IsSelected = false;
                    managed2D.CopyPrimitive = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());
                    managed2D.CopyPrimitive.IsSelected = false;
                    managed2D.CopyPrimitive.IsTransformed = rp.IsTransformed;
                    for (int i = 0; i < 10; i++)
                    {
                        managed2D.CopyPrimitive.MasLinesFront[i] = rp.MasLinesFront[i];

                        managed2D.CopyPrimitive.MasLinesRight[i] = rp.MasLinesRight[i];

                        managed2D.CopyPrimitive.MasLinesTop[i] = rp.MasLinesTop[i];
                    }
                    break;
                }
            }
        }

        private void pastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RectPrimitive r = SlidePositionPrimitive(managed2D.CopyPrimitive);
            //RectPrimitive r = managed2D.CopyPrimitive;
            if (r != null)
            {
                //int i = managed2D.List2DPrimitives.Add((void)managed2D.GetNextKey(), r);
                int k = managed2D.AddPrimitiveToList(r);

                RectPrimitive rpj = managed2D.CopyPrimitive;
                managed2D.CopyPrimitive = new RectPrimitive(new Rectangle(), new Rectangle(), new Rectangle(), managed2D.GenerateRandomColor());
                for (int ix = 0; ix < 10; ix++)
                {
                    managed2D.CopyPrimitive.MasLinesFront[ix] = rpj.MasLinesFront[ix];

                    managed2D.CopyPrimitive.MasLinesRight[ix] = rpj.MasLinesRight[ix];

                    managed2D.CopyPrimitive.MasLinesTop[ix] = rpj.MasLinesTop[ix];
                }

                managed2D.ConvertedPrimitive = GetConvertedPrimitiveToLocalCoords(r);
                textureSettings.convertedPrimitive = managed2D.ConvertedPrimitive;
                managed3D.AddObject(managed2D.ConvertedPrimitive, "Textures\\" + lastTexture, k);
            }
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (int key in managed2D.List2DPrimitives.Keys)
            {
                RectPrimitive rp = (RectPrimitive)managed2D.List2DPrimitives[key];
                if (rp.IsSelected)
                {
                    managed3D.objectsList.Remove(key);
                    managed2D.List2DPrimitives.Remove(key);
                    managed3D.selectedIndex = -1;
                    managed2D.IndexOfSelectedPrimitive = -1;
                    break;
                }
            }
        }

	}
}
