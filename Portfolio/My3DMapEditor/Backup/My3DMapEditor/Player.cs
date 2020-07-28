using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace My3DMapEditor
{
    public class Player:IDisposable
    {
        public bool isPlayerRed = true;
        public bool isPlayerSelected = false;
        private int h = 80;
        private int w = 32;
        public Rectangle rectFront = new Rectangle();
        public Rectangle rectRight = new Rectangle();
        public Rectangle rectTop = new Rectangle();

		public Rectangle localRectFront;
		public Rectangle localRectRight;
		public Rectangle localRectTop;

		public VertexBuffer vBuf = null;

		public CustomVertex.PositionNormalTextured[] cv_PnTex = new CustomVertex.PositionNormalTextured[36];

        public Player(Point p,string nameProjection,Grid2D grid,Device device)
        {
			
            if (nameProjection == "Front")
            {
                rectFront = new Rectangle(p, new Size(w,h));
                rectRight = new Rectangle(new Point(Form1.panelWidth/2,p.Y), new Size(w, h));
                rectTop = new Rectangle(new Point(p.X, Form1.panelHeight/2 - w), new Size(w, w));
			}
            if (nameProjection == "Right")
            {
                rectRight = new Rectangle(p, new Size(w, h));
                rectFront = new Rectangle(new Point(Form1.panelWidth/2,p.Y), new Size(w, h));
                rectTop = new Rectangle(new Point(p.X, Form1.panelHeight / 2 - w), new Size(w, w));
            }
            if (nameProjection == "Top")
            {
                rectTop = new Rectangle(p, new Size(w, w));
                rectFront = new Rectangle(new Point(p.X, (Form1.panelHeight/2) - 80), new Size(w, h));
                rectRight = new Rectangle(new Point((Form1.panelHeight - p.Y) - w, (Form1.panelHeight / 2) - 80), new Size(w, h));
            }

			ReCreateBuffer(device,grid);
        }
		public void Dispose()
		{
			vBuf.Dispose();
			cv_PnTex = null;
			GC.SuppressFinalize(this);
		}
		~Player()
		{
			Dispose();
		}
		public void ReCreateBuffer(Device device,Grid2D grid)
		{
			Point p1 = grid.ConvertGlobalCoordsToLocal(rectFront.Location);
			localRectFront = new Rectangle(p1, new Size(w, h));
			p1 = grid.ConvertGlobalCoordsToLocal(rectRight.Location);
			localRectRight = new Rectangle(p1, new Size(w, h));
			p1 = grid.ConvertGlobalCoordsToLocal(rectTop.Location);
			localRectTop = new Rectangle(p1, new Size(w, w));

			CustomVertex.PositionNormalTextured[] vertex = new CustomVertex.PositionNormalTextured[36];

			#region FrontSide
			vertex[0] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectFront.Y, localRectTop.Y - localRectTop.Height, 0, 0, -1, 0, 0);
			vertex[1] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectFront.Y, localRectTop.Y - localRectTop.Height, 0, 0, -1, 1, 0);
			vertex[2] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectFront.Y - localRectFront.Height, localRectTop.Y - localRectTop.Height, 0, 0, -1, 1, 1);

			vertex[3] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectFront.Y, localRectTop.Y - localRectTop.Height, 0, 0, -1, 0, 0);
			vertex[4] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectFront.Y - localRectFront.Height, localRectTop.Y - localRectTop.Height, 0, 0, -1, 1, 1);
			vertex[5] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectFront.Y - localRectFront.Height, localRectTop.Y - localRectTop.Height, 0, 0, -1, 0, 1);
			#endregion
			#region RightSide
			vertex[6] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectFront.Y, localRectTop.Y - localRectTop.Height, 1, 0, 0, 0, 0);
			vertex[7] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectRight.Y, localRectTop.Y, 1, 0, 0, 1, 0);
			vertex[8] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectRight.Y - localRectRight.Height, localRectTop.Y, 1, 0, 0, 1, 1);

			vertex[9] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectFront.Y, localRectTop.Y - localRectTop.Height, 1, 0, 0, 0, 0);
			vertex[10] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectRight.Y - localRectRight.Height, localRectTop.Y, 1, 0, 0, 1, 1);
			vertex[11] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectRight.Y - localRectRight.Height, localRectTop.Y - localRectTop.Height, 1, 0, 0, 0, 1);
			#endregion
			#region BackSide
			vertex[12] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectFront.Y, localRectTop.Y, 0, 0, 1, 0, 0);
			vertex[13] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectRight.Y, localRectTop.Y, 0, 0, 1, 1, 0);
			vertex[14] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectRight.Y - localRectRight.Height, localRectTop.Y, 0, 0, 1, 1, 1);

			vertex[15] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectFront.Y, localRectTop.Y, 0, 0, 1, 0, 0);
			vertex[16] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectRight.Y - localRectRight.Height, localRectTop.Y, 0, 0, 1, 1, 1);
			vertex[17] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectRight.Y - localRectRight.Height, localRectTop.Y, 0, 0, 1, 0, 1);
			#endregion
			#region LeftSide
			vertex[18] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectFront.Y, localRectTop.Y, -1, 0, 0, 0, 0);
			vertex[19] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectFront.Y, localRectTop.Y - localRectTop.Height, -1, 0, 0, 1, 0);
			vertex[20] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectFront.Y - localRectFront.Height, localRectTop.Y - localRectTop.Height, -1, 0, 0, 1, 1);

			vertex[21] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectFront.Y, localRectTop.Y, -1, 0, 0, 0, 0);
			vertex[22] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectFront.Y - localRectFront.Height, localRectTop.Y - localRectTop.Height, -1, 0, 0, 1, 1);
			vertex[23] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectFront.Y - localRectFront.Height, localRectTop.Y, -1, 0, 0, 0, 1);
			#endregion
			#region Upside
			vertex[24] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectRight.Y, localRectTop.Y, 0, 1, 0, 0, 0);
			vertex[25] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectFront.Y, localRectTop.Y, 0, 1, 0, 1, 0);
			vertex[26] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectFront.Y, localRectTop.Y - localRectTop.Height, 0, 1, 0, 1, 1);

			vertex[27] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectRight.Y, localRectTop.Y, 0, 1, 0, 0, 0);
			vertex[28] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectFront.Y, localRectTop.Y - localRectTop.Height, 0, 1, 0, 1, 1);
			vertex[29] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectFront.Y, localRectTop.Y - localRectTop.Height, 0, 1, 0, 0, 1);
			#endregion
			#region DownSide
			vertex[30] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectFront.Y - localRectFront.Height, localRectTop.Y - localRectTop.Height, 0, -1, 0, 0, 0);
			vertex[31] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectFront.Y - localRectFront.Height, localRectTop.Y - localRectTop.Height, 0, -1, 0, 1, 0);
			vertex[32] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectFront.Y - localRectFront.Height, localRectTop.Y, 0, -1, 0, 1, 1);

			vertex[33] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectFront.Y - localRectFront.Height, localRectTop.Y - localRectTop.Height, 0, -1, 0, 0, 0);
			vertex[34] = new CustomVertex.PositionNormalTextured(localRectFront.X + localRectFront.Width, localRectFront.Y - localRectFront.Height, localRectTop.Y, 0, -1, 0, 1, 1);
			vertex[35] = new CustomVertex.PositionNormalTextured(localRectFront.X, localRectFront.Y - localRectFront.Height, localRectTop.Y, 0, -1, 0, 0, 1);
			#endregion

			cv_PnTex = vertex;

			vBuf = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 36,
						device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);
			vBuf.SetData(vertex, 0, LockFlags.None);
		}
		public void ReCreateBufferFromNewCustomVertsMas(Device device)
		{
			vBuf = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 36,
						device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);
			vBuf.SetData(cv_PnTex, 0, LockFlags.None);
		}
    }
}
