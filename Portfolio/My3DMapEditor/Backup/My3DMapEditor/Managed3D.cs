using System;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.IO;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using Microsoft.DirectX.DirectInput;
using AVGame;

namespace My3DMapEditor
{
    public class MyObject:IDisposable
    {
        public VertexBuffer buf_FrontSide;
        public VertexBuffer buf_RightSide;
        public VertexBuffer buf_BackSide;
        public VertexBuffer buf_LeftSide;
        public VertexBuffer buf_UpSide;
        public VertexBuffer buf_DownSide;


		//public Texture[] textures;

        public string[] textureNames;

        public CustomVertex.PositionNormalTextured[] mas_CV_PosNormTex;

        public int index;

		public MyObject()
		{

		}


		public void Dispose()
		{
            try
            {
                buf_FrontSide.Dispose();
                buf_RightSide.Dispose();
                buf_BackSide.Dispose();
                buf_LeftSide.Dispose();
                buf_UpSide.Dispose();
                buf_DownSide.Dispose();

                mas_CV_PosNormTex = null;

                GC.SuppressFinalize(this);
            }
            catch
            {
                return;
            }
		}
		~MyObject()
		{
			Dispose();
		}
		
    }
		public delegate void MouseClick3DDelegate();

    public class Managed3D:IDisposable
    {
		

        public Microsoft.DirectX.Direct3D.Device device = null;
        public Microsoft.DirectX.DirectInput.Device keyboard = null;
        public Microsoft.DirectX.DirectInput.Device mouse = null;
		Microsoft.DirectX.Direct3D.Font text;

        public Material mat = new Material();
        public Material matSel = new Material();
        public Material matSideSel = new Material();
		public Material matPlayerSel = new Material();

		static public event MouseClick3DDelegate MouseClick3D;

		public Player playerRed = null;
		public Player playerBlue = null;

        int indexSideSel = -1;

        public int selectedIndex = -1;

        public bool modeView3D = false;
		public bool canMouseClick = true;

        public MATW orientation = null;
        public Matrix mat_view;
        public Matrix mat_proj;

        public Hashtable objectsList = new Hashtable();
		public Hashtable textures = new Hashtable();

        private bool isSelectionHide = false;

		static public VertexBuffer vbsel;

		private Texture texRedPlayer;
		private Texture texBluePlayer;

		public Sprite cross;
		public Texture crossTex;

        public Managed3D(IntPtr window3DPtr, Form f)
        {
            try
            {
                Environment.CurrentDirectory = Form1.startingPath;

                InitalGraphics(window3DPtr, f);

                mat.Ambient = Color.White;
                mat.Diffuse = Color.White;

                matSel.Ambient = Color.Yellow;
                matSel.Diffuse = Color.Yellow;

                matSideSel.Ambient = Color.Aqua;
                matSideSel.Diffuse = Color.Aqua;

                matPlayerSel.Ambient = Color.Lime;
                matPlayerSel.Diffuse = Color.Lime;

                TexturesSettings.Apply += new ApplyDelegate(TexturesSettings_Apply);

                TexturesSettings.ChangeTexturePosition += new ChangeTexturePositionDelegate(TexturesSettings_ChangeTexturePosition);

                TexturesSettings.HideSelection += new HideSelectionDelegate(TexturesSettings_HideSelection);

                Form1.Update3DPrimitiveCoords += new Update3DPrimitiveCoordsDelegate(Form1_Update3DPrimitiveCoords);

                texBluePlayer = TextureLoader.FromFile(device, @"Textures\SkeletB.jpg");
                texRedPlayer = TextureLoader.FromFile(device, @"Textures\SkeletR.jpg");

                cross = new Sprite(device);
                crossTex = TextureLoader.FromFile(device, "Cross.bmp", 25, 25, 0, Usage.None, Format.Unknown, Pool.Default, Filter.None, Filter.None, Color.FromArgb(255, 0, 0, 255).ToArgb());

                vbsel = new VertexBuffer(typeof(CustomVertex.PositionNormalColored), 2,
                            device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);

                text = new Microsoft.DirectX.Direct3D.Font(device, new System.Drawing.Font("Verdana", 12));
            }
            catch
            {
                return;
            }
        }
		~Managed3D()
		{
			Dispose();
		}
		public void Dispose()
		{
            try
            {
                if (objectsList != null)
                {
                    foreach (int key in objectsList.Keys)
                    {
                        MyObject ob = ((MyObject)objectsList[key]);
                        ob.Dispose();
                    }
                    objectsList = null;
                }
                orientation.Dispose();
                vbsel.Dispose();
                texRedPlayer.Dispose();
                texBluePlayer.Dispose();
                crossTex.Dispose();
                cross.Dispose();
                keyboard.Dispose();
                mouse.Dispose();
                device.Dispose();

                GC.SuppressFinalize(this);
            }
            catch
            {
                return;
            }
		}
		static public void RecreateSelectionLineBuffer(Vector3 begin,Vector3 end)
		{
            try
            {
                vbsel.SetData(CreateSelLineVerts(begin, end), 0, LockFlags.None);
            }
            catch
            {
                return;
            }
		}
		public static CustomVertex.PositionNormalColored[] CreateSelLineVerts(Vector3 begin,Vector3 end)
		{
			CustomVertex.PositionNormalColored[] verts = new CustomVertex.PositionNormalColored[2];
			verts[0] = new CustomVertex.PositionNormalColored(begin, new Vector3(1,1,1), Color.Yellow.ToArgb());
			verts[1] = new CustomVertex.PositionNormalColored(end, new Vector3(1, 1, 1), Color.Yellow.ToArgb());
			return verts;
		}
        void Form1_Update3DPrimitiveCoords(RectPrimitive convertedPr, int selIndex)
        {
            try
            {
                if (selectedIndex != -1)
                {
                    foreach (int key in objectsList.Keys)
                    {
                        MyObject obj = (MyObject)objectsList[key];
                        if (obj.index == selIndex)
                        {
                            CustomVertex.PositionNormalTextured[] pnt = new CustomVertex.PositionNormalTextured[6];

                            pnt[0] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[0].X, convertedPr.MasLinesFront[0].Y, convertedPr.MasLinesTop[3].Y)
                                , new Vector3(convertedPr.MasLinesFront[0].X, convertedPr.MasLinesFront[0].Y, convertedPr.MasLinesTop[3].Y - 1)
                                , obj.mas_CV_PosNormTex[0].Tu, obj.mas_CV_PosNormTex[0].Tv);
                            pnt[1] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[1].X, convertedPr.MasLinesFront[1].Y, convertedPr.MasLinesTop[2].Y)
                                , new Vector3(convertedPr.MasLinesFront[1].X, convertedPr.MasLinesFront[1].Y, convertedPr.MasLinesTop[2].Y - 1)
                                , obj.mas_CV_PosNormTex[1].Tu, obj.mas_CV_PosNormTex[1].Tv);
                            pnt[2] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[2].X, convertedPr.MasLinesFront[2].Y, convertedPr.MasLinesTop[7].Y)
                                , new Vector3(convertedPr.MasLinesFront[2].X, convertedPr.MasLinesFront[2].Y, convertedPr.MasLinesTop[7].Y - 1)
                                , obj.mas_CV_PosNormTex[2].Tu, obj.mas_CV_PosNormTex[2].Tv);
                            pnt[3] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[0].X, convertedPr.MasLinesFront[0].Y, convertedPr.MasLinesTop[3].Y)
                                , new Vector3(convertedPr.MasLinesFront[0].X, convertedPr.MasLinesFront[0].Y, convertedPr.MasLinesTop[3].Y - 1)
                                , obj.mas_CV_PosNormTex[3].Tu, obj.mas_CV_PosNormTex[3].Tv);
                            pnt[4] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[2].X, convertedPr.MasLinesFront[2].Y, convertedPr.MasLinesTop[7].Y)
                                , new Vector3(convertedPr.MasLinesFront[2].X, convertedPr.MasLinesFront[2].Y, convertedPr.MasLinesTop[7].Y - 1)
                                , obj.mas_CV_PosNormTex[4].Tu, obj.mas_CV_PosNormTex[4].Tv);
                            pnt[5] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[3].X, convertedPr.MasLinesFront[3].Y, convertedPr.MasLinesTop[8].Y)
                                , new Vector3(convertedPr.MasLinesFront[3].X, convertedPr.MasLinesFront[3].Y, convertedPr.MasLinesTop[8].Y - 1)
                                , obj.mas_CV_PosNormTex[5].Tu, obj.mas_CV_PosNormTex[5].Tv);

                            obj.mas_CV_PosNormTex[0] = pnt[0];
                            obj.mas_CV_PosNormTex[1] = pnt[1];
                            obj.mas_CV_PosNormTex[2] = pnt[2];
                            obj.mas_CV_PosNormTex[3] = pnt[3];
                            obj.mas_CV_PosNormTex[4] = pnt[4];
                            obj.mas_CV_PosNormTex[5] = pnt[5];

                            obj.buf_FrontSide.SetData(pnt, 0, LockFlags.None);

                            pnt[0] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[1].X, convertedPr.MasLinesRight[0].Y, convertedPr.MasLinesTop[2].Y)
                                , new Vector3(1 + convertedPr.MasLinesFront[1].X, convertedPr.MasLinesRight[0].Y, convertedPr.MasLinesTop[2].Y)
                                , obj.mas_CV_PosNormTex[6].Tu, obj.mas_CV_PosNormTex[6].Tv);
                            pnt[1] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[6].X, convertedPr.MasLinesRight[1].Y, convertedPr.MasLinesTop[1].Y)
                                , new Vector3(1 + convertedPr.MasLinesFront[6].X, convertedPr.MasLinesRight[1].Y, convertedPr.MasLinesTop[1].Y)
                                , obj.mas_CV_PosNormTex[7].Tu, obj.mas_CV_PosNormTex[7].Tv);
                            pnt[2] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[7].X, convertedPr.MasLinesRight[2].Y, convertedPr.MasLinesTop[6].Y)
                                , new Vector3(1 + convertedPr.MasLinesFront[7].X, convertedPr.MasLinesRight[2].Y, convertedPr.MasLinesTop[6].Y)
                                , obj.mas_CV_PosNormTex[8].Tu, obj.mas_CV_PosNormTex[8].Tv);
                            pnt[3] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[1].X, convertedPr.MasLinesRight[0].Y, convertedPr.MasLinesTop[2].Y)
                                , new Vector3(1 + convertedPr.MasLinesFront[1].X, convertedPr.MasLinesRight[0].Y, convertedPr.MasLinesTop[2].Y)
                                , obj.mas_CV_PosNormTex[9].Tu, obj.mas_CV_PosNormTex[9].Tv);
                            pnt[4] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[7].X, convertedPr.MasLinesRight[2].Y, convertedPr.MasLinesTop[6].Y)
                                , new Vector3(1 + convertedPr.MasLinesFront[7].X, convertedPr.MasLinesRight[2].Y, convertedPr.MasLinesTop[6].Y)
                                , obj.mas_CV_PosNormTex[10].Tu, obj.mas_CV_PosNormTex[10].Tv);
                            pnt[5] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[2].X, convertedPr.MasLinesRight[3].Y, convertedPr.MasLinesTop[7].Y)
                                , new Vector3(1 + convertedPr.MasLinesFront[2].X, convertedPr.MasLinesRight[3].Y, convertedPr.MasLinesTop[7].Y)
                                , obj.mas_CV_PosNormTex[11].Tu, obj.mas_CV_PosNormTex[11].Tv);

                            obj.mas_CV_PosNormTex[6] = pnt[0];
                            obj.mas_CV_PosNormTex[7] = pnt[1];
                            obj.mas_CV_PosNormTex[8] = pnt[2];
                            obj.mas_CV_PosNormTex[9] = pnt[3];
                            obj.mas_CV_PosNormTex[10] = pnt[4];
                            obj.mas_CV_PosNormTex[11] = pnt[5];

                            obj.buf_RightSide.SetData(pnt, 0, LockFlags.None);

                            pnt[0] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[6].X, convertedPr.MasLinesFront[6].Y, convertedPr.MasLinesTop[1].Y)
                                , new Vector3(convertedPr.MasLinesFront[6].X, convertedPr.MasLinesFront[6].Y, convertedPr.MasLinesTop[1].Y + 1)
                                , obj.mas_CV_PosNormTex[12].Tu, obj.mas_CV_PosNormTex[12].Tv);
                            pnt[1] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[5].X, convertedPr.MasLinesFront[5].Y, convertedPr.MasLinesTop[0].Y)
                                , new Vector3(convertedPr.MasLinesFront[5].X, convertedPr.MasLinesFront[5].Y, convertedPr.MasLinesTop[0].Y + 1)
                                , obj.mas_CV_PosNormTex[13].Tu, obj.mas_CV_PosNormTex[13].Tv);
                            pnt[2] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[8].X, convertedPr.MasLinesFront[8].Y, convertedPr.MasLinesTop[5].Y)
                                , new Vector3(convertedPr.MasLinesFront[8].X, convertedPr.MasLinesFront[8].Y, convertedPr.MasLinesTop[5].Y + 1)
                                , obj.mas_CV_PosNormTex[14].Tu, obj.mas_CV_PosNormTex[14].Tv);
                            pnt[3] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[6].X, convertedPr.MasLinesFront[6].Y, convertedPr.MasLinesTop[1].Y)
                                , new Vector3(convertedPr.MasLinesFront[6].X, convertedPr.MasLinesFront[6].Y, convertedPr.MasLinesTop[1].Y + 1)
                                , obj.mas_CV_PosNormTex[15].Tu, obj.mas_CV_PosNormTex[15].Tv);
                            pnt[4] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[8].X, convertedPr.MasLinesFront[8].Y, convertedPr.MasLinesTop[5].Y)
                                , new Vector3(convertedPr.MasLinesFront[8].X, convertedPr.MasLinesFront[8].Y, convertedPr.MasLinesTop[5].Y + 1)
                                , obj.mas_CV_PosNormTex[16].Tu, obj.mas_CV_PosNormTex[16].Tv);
                            pnt[5] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[7].X, convertedPr.MasLinesFront[7].Y, convertedPr.MasLinesTop[6].Y)
                                , new Vector3(convertedPr.MasLinesFront[7].X, convertedPr.MasLinesFront[7].Y, convertedPr.MasLinesTop[6].Y + 1)
                                , obj.mas_CV_PosNormTex[17].Tu, obj.mas_CV_PosNormTex[17].Tv);

                            obj.mas_CV_PosNormTex[12] = pnt[0];
                            obj.mas_CV_PosNormTex[13] = pnt[1];
                            obj.mas_CV_PosNormTex[14] = pnt[2];
                            obj.mas_CV_PosNormTex[15] = pnt[3];
                            obj.mas_CV_PosNormTex[16] = pnt[4];
                            obj.mas_CV_PosNormTex[17] = pnt[5];

                            obj.buf_BackSide.SetData(pnt, 0, LockFlags.None);

                            pnt[0] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[5].X, convertedPr.MasLinesRight[6].Y, convertedPr.MasLinesTop[0].Y)
                                , new Vector3(convertedPr.MasLinesFront[5].X - 1, convertedPr.MasLinesRight[6].Y, convertedPr.MasLinesTop[0].Y)
                                , obj.mas_CV_PosNormTex[18].Tu, obj.mas_CV_PosNormTex[18].Tv);
                            pnt[1] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[0].X, convertedPr.MasLinesRight[5].Y, convertedPr.MasLinesTop[3].Y)
                                , new Vector3(convertedPr.MasLinesFront[0].X - 1, convertedPr.MasLinesRight[5].Y, convertedPr.MasLinesTop[3].Y)
                                , obj.mas_CV_PosNormTex[19].Tu, obj.mas_CV_PosNormTex[19].Tv);
                            pnt[2] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[3].X, convertedPr.MasLinesRight[8].Y, convertedPr.MasLinesTop[8].Y)
                                , new Vector3(convertedPr.MasLinesFront[3].X - 1, convertedPr.MasLinesRight[8].Y, convertedPr.MasLinesTop[8].Y)
                                , obj.mas_CV_PosNormTex[20].Tu, obj.mas_CV_PosNormTex[20].Tv);
                            pnt[3] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[5].X, convertedPr.MasLinesRight[6].Y, convertedPr.MasLinesTop[0].Y)
                                , new Vector3(convertedPr.MasLinesFront[5].X - 1, convertedPr.MasLinesRight[6].Y, convertedPr.MasLinesTop[0].Y)
                                , obj.mas_CV_PosNormTex[21].Tu, obj.mas_CV_PosNormTex[21].Tv);
                            pnt[4] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[3].X, convertedPr.MasLinesRight[8].Y, convertedPr.MasLinesTop[8].Y)
                                , new Vector3(convertedPr.MasLinesFront[3].X - 1, convertedPr.MasLinesRight[8].Y, convertedPr.MasLinesTop[8].Y)
                                , obj.mas_CV_PosNormTex[22].Tu, obj.mas_CV_PosNormTex[22].Tv);
                            pnt[5] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesFront[8].X, convertedPr.MasLinesRight[7].Y, convertedPr.MasLinesTop[5].Y)
                                , new Vector3(convertedPr.MasLinesFront[8].X - 1, convertedPr.MasLinesRight[7].Y, convertedPr.MasLinesTop[5].Y)
                                , obj.mas_CV_PosNormTex[23].Tu, obj.mas_CV_PosNormTex[23].Tv);

                            obj.mas_CV_PosNormTex[18] = pnt[0];
                            obj.mas_CV_PosNormTex[19] = pnt[1];
                            obj.mas_CV_PosNormTex[20] = pnt[2];
                            obj.mas_CV_PosNormTex[21] = pnt[3];
                            obj.mas_CV_PosNormTex[22] = pnt[4];
                            obj.mas_CV_PosNormTex[23] = pnt[5];

                            obj.buf_LeftSide.SetData(pnt, 0, LockFlags.None);

                            pnt[0] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesTop[0].X, convertedPr.MasLinesFront[5].Y, convertedPr.MasLinesTop[0].Y),
                                new Vector3(convertedPr.MasLinesTop[0].X, convertedPr.MasLinesFront[5].Y + 1, convertedPr.MasLinesTop[0].Y),
                                obj.mas_CV_PosNormTex[24].Tu, obj.mas_CV_PosNormTex[24].Tv);
                            pnt[1] =
                                new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesTop[1].X, convertedPr.MasLinesFront[6].Y, convertedPr.MasLinesTop[1].Y),
                                new Vector3(convertedPr.MasLinesTop[1].X, convertedPr.MasLinesFront[6].Y + 1, convertedPr.MasLinesTop[1].Y)
                                , obj.mas_CV_PosNormTex[25].Tu, obj.mas_CV_PosNormTex[25].Tv);
                            pnt[2] =
                                new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesTop[2].X, convertedPr.MasLinesFront[1].Y, convertedPr.MasLinesTop[2].Y),
                                new Vector3(convertedPr.MasLinesTop[2].X, convertedPr.MasLinesFront[1].Y + 1, convertedPr.MasLinesTop[2].Y)
                                , obj.mas_CV_PosNormTex[26].Tu, obj.mas_CV_PosNormTex[26].Tv);
                            pnt[3] =
                                new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesTop[0].X, convertedPr.MasLinesFront[5].Y, convertedPr.MasLinesTop[0].Y),
                                new Vector3(convertedPr.MasLinesTop[0].X, convertedPr.MasLinesFront[5].Y + 1, convertedPr.MasLinesTop[0].Y)
                                , obj.mas_CV_PosNormTex[27].Tu, obj.mas_CV_PosNormTex[27].Tv);
                            pnt[4] =
                                new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesTop[2].X, convertedPr.MasLinesFront[1].Y, convertedPr.MasLinesTop[2].Y),
                                new Vector3(convertedPr.MasLinesTop[2].X, convertedPr.MasLinesFront[1].Y + 1, convertedPr.MasLinesTop[2].Y)
                                , obj.mas_CV_PosNormTex[28].Tu, obj.mas_CV_PosNormTex[28].Tv);
                            pnt[5] =
                                new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesTop[3].X, convertedPr.MasLinesFront[0].Y, convertedPr.MasLinesTop[3].Y),
                                new Vector3(convertedPr.MasLinesTop[3].X, convertedPr.MasLinesFront[0].Y + 1, convertedPr.MasLinesTop[3].Y)
                                , obj.mas_CV_PosNormTex[29].Tu, obj.mas_CV_PosNormTex[29].Tv);

                            obj.mas_CV_PosNormTex[24] = pnt[0];
                            obj.mas_CV_PosNormTex[25] = pnt[1];
                            obj.mas_CV_PosNormTex[26] = pnt[2];
                            obj.mas_CV_PosNormTex[27] = pnt[3];
                            obj.mas_CV_PosNormTex[28] = pnt[4];
                            obj.mas_CV_PosNormTex[29] = pnt[5];

                            obj.buf_UpSide.SetData(pnt, 0, LockFlags.None);

                            pnt[0] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesTop[8].X, convertedPr.MasLinesFront[3].Y, convertedPr.MasLinesTop[8].Y)
                                , new Vector3(convertedPr.MasLinesTop[8].X, convertedPr.MasLinesFront[3].Y + 1, convertedPr.MasLinesTop[8].Y)
                                , obj.mas_CV_PosNormTex[30].Tu, obj.mas_CV_PosNormTex[30].Tv);
                            pnt[1] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesTop[7].X, convertedPr.MasLinesFront[2].Y, convertedPr.MasLinesTop[7].Y)
                                , new Vector3(convertedPr.MasLinesTop[7].X, convertedPr.MasLinesFront[2].Y + 1, convertedPr.MasLinesTop[7].Y)
                                , obj.mas_CV_PosNormTex[31].Tu, obj.mas_CV_PosNormTex[31].Tv);
                            pnt[2] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesTop[6].X, convertedPr.MasLinesFront[7].Y, convertedPr.MasLinesTop[6].Y)
                                , new Vector3(convertedPr.MasLinesTop[6].X, convertedPr.MasLinesFront[7].Y + 1, convertedPr.MasLinesTop[6].Y)
                                , obj.mas_CV_PosNormTex[32].Tu, obj.mas_CV_PosNormTex[32].Tv);
                            pnt[3] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesTop[8].X, convertedPr.MasLinesFront[3].Y, convertedPr.MasLinesTop[8].Y)
                                , new Vector3(convertedPr.MasLinesTop[8].X, convertedPr.MasLinesFront[3].Y + 1, convertedPr.MasLinesTop[8].Y)
                                , obj.mas_CV_PosNormTex[33].Tu, obj.mas_CV_PosNormTex[33].Tv);
                            pnt[4] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesTop[6].X, convertedPr.MasLinesFront[7].Y, convertedPr.MasLinesTop[6].Y)
                                , new Vector3(convertedPr.MasLinesTop[6].X, convertedPr.MasLinesFront[7].Y + 1, convertedPr.MasLinesTop[6].Y)
                                , obj.mas_CV_PosNormTex[34].Tu, obj.mas_CV_PosNormTex[34].Tv);
                            pnt[5] = new CustomVertex.PositionNormalTextured(
                                new Vector3(convertedPr.MasLinesTop[5].X, convertedPr.MasLinesFront[8].Y, convertedPr.MasLinesTop[5].Y)
                                , new Vector3(convertedPr.MasLinesTop[5].X, convertedPr.MasLinesFront[8].Y + 1, convertedPr.MasLinesTop[5].Y)
                                , obj.mas_CV_PosNormTex[35].Tu, obj.mas_CV_PosNormTex[35].Tv);

                            obj.mas_CV_PosNormTex[30] = pnt[0];
                            obj.mas_CV_PosNormTex[31] = pnt[1];
                            obj.mas_CV_PosNormTex[32] = pnt[2];
                            obj.mas_CV_PosNormTex[33] = pnt[3];
                            obj.mas_CV_PosNormTex[34] = pnt[4];
                            obj.mas_CV_PosNormTex[35] = pnt[5];

                            obj.buf_DownSide.SetData(pnt, 0, LockFlags.None);
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }

        void TexturesSettings_HideSelection(bool isSelHide)
        {
            isSelectionHide = isSelHide;
        }

        void TexturesSettings_ChangeTexturePosition(float x, float y, float sx, float sy,int side,RectPrimitive rpr)
        {
            //MessageBox.Show("x : "+ x.ToString()+"|y : "+y.ToString()+"|sx : "+sx.ToString()+"|sy : "+sy.ToString()+"|side : "+side.ToString()+"\nSelectedIndex : "+selectedIndex.ToString());
            try
            {
                if (selectedIndex == -1 || rpr == null)
                    return;

                //MyObject obj = (MyObject)objectsList[selectedIndex];

                int ind = 0;

                foreach (int key in objectsList.Keys)
                {
                    MyObject obj = (MyObject)objectsList[key];
                    if (obj.index == selectedIndex)
                    {
                        CustomVertex.PositionNormalTextured[] pnt = new CustomVertex.PositionNormalTextured[6];

                        switch (side)
                        {
                            case 0:
                                pnt[0] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y)
                                    , new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y - 1)
                                    , x, y);
                                pnt[1] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y)
                                    , new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y - 1)
                                    , sx, y);
                                pnt[2] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y)
                                    , new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y - 1)
                                    , sx, sy);
                                pnt[3] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y)
                                    , new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y - 1)
                                    , x, y);
                                pnt[4] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y)
                                    , new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y - 1)
                                    , sx, sy);
                                pnt[5] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y)
                                    , new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y - 1)
                                    , x, sy);

                                obj.mas_CV_PosNormTex[0] = pnt[0];
                                obj.mas_CV_PosNormTex[1] = pnt[1];
                                obj.mas_CV_PosNormTex[2] = pnt[2];
                                obj.mas_CV_PosNormTex[3] = pnt[3];
                                obj.mas_CV_PosNormTex[4] = pnt[4];
                                obj.mas_CV_PosNormTex[5] = pnt[5];

                                obj.buf_FrontSide.SetData(pnt, 0, LockFlags.None);

                                pnt[0] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                                    , new Vector3(1 + rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                                    , x, y);
                                pnt[1] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesRight[1].Y, rpr.MasLinesTop[1].Y)
                                    , new Vector3(1 + rpr.MasLinesFront[6].X, rpr.MasLinesRight[1].Y, rpr.MasLinesTop[1].Y)
                                    , sx, y);
                                pnt[2] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                                    , new Vector3(1 + rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                                    , sx, sy);
                                pnt[3] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                                    , new Vector3(1 + rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                                    , x, y);
                                pnt[4] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                                    , new Vector3(1 + rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                                    , sx, sy);
                                pnt[5] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesRight[3].Y, rpr.MasLinesTop[7].Y)
                                    , new Vector3(1 + rpr.MasLinesFront[2].X, rpr.MasLinesRight[3].Y, rpr.MasLinesTop[7].Y)
                                    , x, sy);


                                obj.mas_CV_PosNormTex[6] = pnt[0];
                                obj.mas_CV_PosNormTex[7] = pnt[1];
                                obj.mas_CV_PosNormTex[8] = pnt[2];
                                obj.mas_CV_PosNormTex[9] = pnt[3];
                                obj.mas_CV_PosNormTex[10] = pnt[4];
                                obj.mas_CV_PosNormTex[11] = pnt[5];

                                obj.buf_RightSide.SetData(pnt, 0, LockFlags.None);

                                pnt[0] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y)
                                    , new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y + 1)
                                    , x, y);
                                pnt[1] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y)
                                    , new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y + 1)
                                    , sx, y);
                                pnt[2] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y)
                                    , new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y + 1)
                                    , sx, sy);
                                pnt[3] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y)
                                    , new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y + 1)
                                    , x, y);
                                pnt[4] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y)
                                    , new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y + 1)
                                    , sx, sy);
                                pnt[5] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y)
                                    , new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y + 1)
                                    , x, sy);

                                obj.mas_CV_PosNormTex[12] = pnt[0];
                                obj.mas_CV_PosNormTex[13] = pnt[1];
                                obj.mas_CV_PosNormTex[14] = pnt[2];
                                obj.mas_CV_PosNormTex[15] = pnt[3];
                                obj.mas_CV_PosNormTex[16] = pnt[4];
                                obj.mas_CV_PosNormTex[17] = pnt[5];

                                obj.buf_BackSide.SetData(pnt, 0, LockFlags.None);

                                pnt[0] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                                    , new Vector3(rpr.MasLinesFront[5].X - 1, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                                    , x, y);
                                pnt[1] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesRight[5].Y, rpr.MasLinesTop[3].Y)
                                    , new Vector3(rpr.MasLinesFront[0].X - 1, rpr.MasLinesRight[5].Y, rpr.MasLinesTop[3].Y)
                                    , sx, y);
                                pnt[2] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                                    , new Vector3(rpr.MasLinesFront[3].X - 1, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                                    , sx, sy);
                                pnt[3] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                                    , new Vector3(rpr.MasLinesFront[5].X - 1, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                                    , x, y);
                                pnt[4] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                                    , new Vector3(rpr.MasLinesFront[3].X - 1, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                                    , sx, sy);
                                pnt[5] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesRight[7].Y, rpr.MasLinesTop[5].Y)
                                    , new Vector3(rpr.MasLinesFront[8].X - 1, rpr.MasLinesRight[7].Y, rpr.MasLinesTop[5].Y)
                                    , x, sy);

                                obj.mas_CV_PosNormTex[18] = pnt[0];
                                obj.mas_CV_PosNormTex[19] = pnt[1];
                                obj.mas_CV_PosNormTex[20] = pnt[2];
                                obj.mas_CV_PosNormTex[21] = pnt[3];
                                obj.mas_CV_PosNormTex[22] = pnt[4];
                                obj.mas_CV_PosNormTex[23] = pnt[5];

                                obj.buf_LeftSide.SetData(pnt, 0, LockFlags.None);

                                pnt[0] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[0].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y)
                                    , new Vector3(rpr.MasLinesTop[0].X, 1, 0)
                                    , x, y);
                                pnt[1] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[1].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y)
                                    , new Vector3(0, rpr.MasLinesFront[6].Y + 1, rpr.MasLinesTop[1].Y)
                                    , sx, y);
                                pnt[2] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[2].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y)
                                    , new Vector3(rpr.MasLinesTop[2].X, rpr.MasLinesFront[1].Y + 1, rpr.MasLinesTop[2].Y)
                                    , sx, sy);
                                pnt[3] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[0].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y)
                                    , new Vector3(0, 1, 0)
                                    , x, y);
                                pnt[4] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[2].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y)
                                    , new Vector3(rpr.MasLinesTop[0].X, rpr.MasLinesFront[1].Y + 1, rpr.MasLinesTop[2].Y)
                                    , sx, sy);
                                pnt[5] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[3].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y)
                                    , new Vector3(rpr.MasLinesTop[3].X, rpr.MasLinesFront[0].Y + 1, rpr.MasLinesTop[3].Y)
                                    , x, sy);

                                obj.mas_CV_PosNormTex[24] = pnt[0];
                                obj.mas_CV_PosNormTex[25] = pnt[1];
                                obj.mas_CV_PosNormTex[26] = pnt[2];
                                obj.mas_CV_PosNormTex[27] = pnt[3];
                                obj.mas_CV_PosNormTex[28] = pnt[4];
                                obj.mas_CV_PosNormTex[29] = pnt[5];

                                obj.buf_UpSide.SetData(pnt, 0, LockFlags.None);

                                pnt[0] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y)
                                    , new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y + 1, rpr.MasLinesTop[8].Y)
                                    , x, y);
                                pnt[1] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[7].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y)
                                    , new Vector3(rpr.MasLinesTop[7].X, rpr.MasLinesFront[2].Y + 1, rpr.MasLinesTop[7].Y)
                                    , sx, y);
                                pnt[2] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y)
                                    , new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y + 1, rpr.MasLinesTop[6].Y)
                                    , sx, sy);
                                pnt[3] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y)
                                    , new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y + 1, rpr.MasLinesTop[8].Y)
                                    , x, y);
                                pnt[4] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y)
                                    , new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y + 1, rpr.MasLinesTop[6].Y)
                                    , sx, sy);
                                pnt[5] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[5].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y)
                                    , new Vector3(rpr.MasLinesTop[5].X, rpr.MasLinesFront[8].Y + 1, rpr.MasLinesTop[5].Y)
                                    , x, sy);

                                obj.mas_CV_PosNormTex[30] = pnt[0];
                                obj.mas_CV_PosNormTex[31] = pnt[1];
                                obj.mas_CV_PosNormTex[32] = pnt[2];
                                obj.mas_CV_PosNormTex[33] = pnt[3];
                                obj.mas_CV_PosNormTex[34] = pnt[4];
                                obj.mas_CV_PosNormTex[35] = pnt[5];

                                obj.buf_DownSide.SetData(pnt, 0, LockFlags.None);
                                indexSideSel = 0;
                                break;
                            case 1:
                                pnt[0] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y)
                                    , new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y - 1)
                                    , x, y);
                                pnt[1] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y)
                                    , new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y - 1)
                                    , sx, y);
                                pnt[2] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y)
                                    , new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y - 1)
                                    , sx, sy);
                                pnt[3] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y)
                                    , new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y - 1)
                                    , x, y);
                                pnt[4] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y)
                                    , new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y - 1)
                                    , sx, sy);
                                pnt[5] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y)
                                    , new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y - 1)
                                    , x, sy);

                                obj.mas_CV_PosNormTex[0] = pnt[0];
                                obj.mas_CV_PosNormTex[1] = pnt[1];
                                obj.mas_CV_PosNormTex[2] = pnt[2];
                                obj.mas_CV_PosNormTex[3] = pnt[3];
                                obj.mas_CV_PosNormTex[4] = pnt[4];
                                obj.mas_CV_PosNormTex[5] = pnt[5];

                                obj.buf_FrontSide.SetData(pnt, 0, LockFlags.None);
                                indexSideSel = 1;
                                break;
                            case 2:
                                pnt[0] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                                    , new Vector3(1 + rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                                    , x, y);
                                pnt[1] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesRight[1].Y, rpr.MasLinesTop[1].Y)
                                    , new Vector3(1 + rpr.MasLinesFront[6].X, rpr.MasLinesRight[1].Y, rpr.MasLinesTop[1].Y)
                                    , sx, y);
                                pnt[2] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                                    , new Vector3(1 + rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                                    , sx, sy);
                                pnt[3] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                                    , new Vector3(1 + rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                                    , x, y);
                                pnt[4] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                                    , new Vector3(1 + rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                                    , sx, sy);
                                pnt[5] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesRight[3].Y, rpr.MasLinesTop[7].Y)
                                    , new Vector3(1 + rpr.MasLinesFront[2].X, rpr.MasLinesRight[3].Y, rpr.MasLinesTop[7].Y)
                                    , x, sy);

                                obj.mas_CV_PosNormTex[6] = pnt[0];
                                obj.mas_CV_PosNormTex[7] = pnt[1];
                                obj.mas_CV_PosNormTex[8] = pnt[2];
                                obj.mas_CV_PosNormTex[9] = pnt[3];
                                obj.mas_CV_PosNormTex[10] = pnt[4];
                                obj.mas_CV_PosNormTex[11] = pnt[5];

                                obj.buf_RightSide.SetData(pnt, 0, LockFlags.None);
                                indexSideSel = 2;
                                break;
                            case 3:
                                pnt[0] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y)
                                    , new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y + 1)
                                    , x, y);
                                pnt[1] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y)
                                    , new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y + 1)
                                    , sx, y);
                                pnt[2] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y)
                                    , new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y + 1)
                                    , sx, sy);
                                pnt[3] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y)
                                    , new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y + 1)
                                    , x, y);
                                pnt[4] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y)
                                    , new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y + 1)
                                    , sx, sy);
                                pnt[5] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y)
                                    , new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y + 1)
                                    , x, sy);

                                obj.mas_CV_PosNormTex[12] = pnt[0];
                                obj.mas_CV_PosNormTex[13] = pnt[1];
                                obj.mas_CV_PosNormTex[14] = pnt[2];
                                obj.mas_CV_PosNormTex[15] = pnt[3];
                                obj.mas_CV_PosNormTex[16] = pnt[4];
                                obj.mas_CV_PosNormTex[17] = pnt[5];

                                obj.buf_BackSide.SetData(pnt, 0, LockFlags.None);
                                indexSideSel = 3;
                                break;
                            case 4:
                                pnt[0] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                                    , new Vector3(rpr.MasLinesFront[5].X - 1, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                                    , x, y);
                                pnt[1] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesRight[5].Y, rpr.MasLinesTop[3].Y)
                                    , new Vector3(rpr.MasLinesFront[0].X - 1, rpr.MasLinesRight[5].Y, rpr.MasLinesTop[3].Y)
                                    , sx, y);
                                pnt[2] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                                    , new Vector3(rpr.MasLinesFront[3].X - 1, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                                    , sx, sy);
                                pnt[3] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                                    , new Vector3(rpr.MasLinesFront[5].X - 1, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                                    , x, y);
                                pnt[4] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                                    , new Vector3(rpr.MasLinesFront[3].X - 1, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                                    , sx, sy);
                                pnt[5] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesRight[7].Y, rpr.MasLinesTop[5].Y)
                                    , new Vector3(rpr.MasLinesFront[8].X - 1, rpr.MasLinesRight[7].Y, rpr.MasLinesTop[5].Y)
                                    , x, sy);

                                obj.mas_CV_PosNormTex[18] = pnt[0];
                                obj.mas_CV_PosNormTex[19] = pnt[1];
                                obj.mas_CV_PosNormTex[20] = pnt[2];
                                obj.mas_CV_PosNormTex[21] = pnt[3];
                                obj.mas_CV_PosNormTex[22] = pnt[4];
                                obj.mas_CV_PosNormTex[23] = pnt[5];

                                obj.buf_LeftSide.SetData(pnt, 0, LockFlags.None);
                                indexSideSel = 4;
                                break;
                            case 5:
                                pnt[0] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[0].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y)
                                    , new Vector3(rpr.MasLinesTop[0].X, rpr.MasLinesFront[5].Y + 1, rpr.MasLinesTop[0].Y)
                                    , x, y);
                                pnt[1] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[1].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y)
                                    , new Vector3(rpr.MasLinesTop[1].X, rpr.MasLinesFront[6].Y + 1, rpr.MasLinesTop[1].Y)
                                    , sx, y);
                                pnt[2] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[2].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y)
                                    , new Vector3(rpr.MasLinesTop[2].X, rpr.MasLinesFront[1].Y + 1, rpr.MasLinesTop[2].Y)
                                    , sx, sy);
                                pnt[3] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[0].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y)
                                    , new Vector3(rpr.MasLinesTop[0].X, rpr.MasLinesFront[5].Y + 1, rpr.MasLinesTop[0].Y)
                                    , x, y);
                                pnt[4] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[2].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y)
                                    , new Vector3(rpr.MasLinesTop[2].X, rpr.MasLinesFront[1].Y + 1, rpr.MasLinesTop[2].Y)
                                    , sx, sy);
                                pnt[5] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[3].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y)
                                    , new Vector3(rpr.MasLinesTop[3].X, rpr.MasLinesFront[0].Y + 1, rpr.MasLinesTop[3].Y)
                                    , x, sy);

                                obj.mas_CV_PosNormTex[24] = pnt[0];
                                obj.mas_CV_PosNormTex[25] = pnt[1];
                                obj.mas_CV_PosNormTex[26] = pnt[2];
                                obj.mas_CV_PosNormTex[27] = pnt[3];
                                obj.mas_CV_PosNormTex[28] = pnt[4];
                                obj.mas_CV_PosNormTex[29] = pnt[5];

                                obj.buf_UpSide.SetData(pnt, 0, LockFlags.None);
                                indexSideSel = 5;
                                break;
                            case 6:
                                pnt[0] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y)
                                    , new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y + 1, rpr.MasLinesTop[8].Y)
                                    , x, y);
                                pnt[1] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[7].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y)
                                    , new Vector3(rpr.MasLinesTop[7].X, rpr.MasLinesFront[2].Y + 1, rpr.MasLinesTop[7].Y)
                                    , sx, y);
                                pnt[2] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y)
                                    , new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y + 1, rpr.MasLinesTop[6].Y)
                                    , sx, sy);
                                pnt[3] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y)
                                    , new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y + 1, rpr.MasLinesTop[8].Y)
                                    , x, y);
                                pnt[4] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y)
                                    , new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y + 1, rpr.MasLinesTop[6].Y)
                                    , sx, sy);
                                pnt[5] = new CustomVertex.PositionNormalTextured(
                                    new Vector3(rpr.MasLinesTop[5].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y)
                                    , new Vector3(rpr.MasLinesTop[5].X, rpr.MasLinesFront[8].Y + 1, rpr.MasLinesTop[5].Y)
                                    , x, sy);

                                obj.mas_CV_PosNormTex[30] = pnt[0];
                                obj.mas_CV_PosNormTex[31] = pnt[1];
                                obj.mas_CV_PosNormTex[32] = pnt[2];
                                obj.mas_CV_PosNormTex[33] = pnt[3];
                                obj.mas_CV_PosNormTex[34] = pnt[4];
                                obj.mas_CV_PosNormTex[35] = pnt[5];

                                obj.buf_DownSide.SetData(pnt, 0, LockFlags.None);
                                indexSideSel = 6;
                                break;
                        }
                        return;
                    }
                    ind++;
                }
            }
            catch
            {
                return;
            }
        }

        void TexturesSettings_Apply(string nameText, int indexSide)
        {
            try
            {
                //MessageBox.Show(nameText+" " + selIndex.ToString() + " "+indexSide.ToString());
                UpdateTextures(nameText, indexSide);
            }
            catch
            {
                return;
            }
        }
        private void UpdateTextures(string nameText, int indexSide)
        {
            try
            {
                Environment.CurrentDirectory = Form1.startingPath;

                if (selectedIndex != -1)
                {
                    //MessageBox.Show(nameText+" "+indexSide.ToString());

                    if (!File.Exists("Textures\\" + nameText))
                    {
                        //MessageBox.Show("Name of texture is invalid : " + nameText + "\nerror line 428");
                        return;
                    }

                    foreach (int key in objectsList.Keys)
                    {
                        MyObject obj = (MyObject)objectsList[key];
                        if (obj.index == selectedIndex)
                        {
                            switch (indexSide)
                            {
                                case 0:
                                    obj.textureNames[0] = "Textures\\" + nameText;
                                    obj.textureNames[1] = "Textures\\" + nameText;
                                    obj.textureNames[2] = "Textures\\" + nameText;
                                    obj.textureNames[3] = "Textures\\" + nameText;
                                    obj.textureNames[4] = "Textures\\" + nameText;
                                    obj.textureNames[5] = "Textures\\" + nameText;

                                    if (!textures.Contains(obj.textureNames[0].ToString()))
                                        textures.Add(obj.textureNames[0].ToString(), TextureLoader.FromFile(device, obj.textureNames[0].ToString()));

                                    indexSideSel = 0;

                                    break;

                                case 1:
                                    obj.textureNames[0] = "Textures\\" + nameText;

                                    if (!textures.Contains(obj.textureNames[0].ToString()))
                                        textures.Add(obj.textureNames[0].ToString(), TextureLoader.FromFile(device, obj.textureNames[0].ToString()));

                                    indexSideSel = 1;
                                    break;
                                case 2:
                                    obj.textureNames[1] = "Textures\\" + nameText;

                                    if (!textures.Contains(obj.textureNames[1].ToString()))
                                        textures.Add(obj.textureNames[1].ToString(), TextureLoader.FromFile(device, obj.textureNames[1].ToString()));

                                    indexSideSel = 2;
                                    break;
                                case 3:
                                    obj.textureNames[2] = "Textures\\" + nameText;

                                    if (!textures.Contains(obj.textureNames[2].ToString()))
                                        textures.Add(obj.textureNames[2].ToString(), TextureLoader.FromFile(device, obj.textureNames[2].ToString()));

                                    indexSideSel = 3;
                                    break;
                                case 4:
                                    obj.textureNames[3] = "Textures\\" + nameText;

                                    if (!textures.Contains(obj.textureNames[3].ToString()))
                                        textures.Add(obj.textureNames[3].ToString(), TextureLoader.FromFile(device, obj.textureNames[3].ToString()));

                                    indexSideSel = 4;
                                    break;
                                case 5:
                                    obj.textureNames[4] = "Textures\\" + nameText;

                                    if (!textures.Contains(obj.textureNames[4].ToString()))
                                        textures.Add(obj.textureNames[4].ToString(), TextureLoader.FromFile(device, obj.textureNames[4].ToString()));

                                    indexSideSel = 5;
                                    break;
                                case 6:
                                    obj.textureNames[5] = "Textures\\" + nameText;

                                    if (!textures.Contains(obj.textureNames[5].ToString()))
                                        textures.Add(obj.textureNames[5].ToString(), TextureLoader.FromFile(device, obj.textureNames[5].ToString()));

                                    indexSideSel = 6;
                                    break;
                            }

                            break;
                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }
        private void InitalGraphics(IntPtr window3DPtr, Form f)
        {
            PresentParameters presentParams = new PresentParameters();
            presentParams.Windowed = true;
            presentParams.SwapEffect = SwapEffect.Discard;
            presentParams.AutoDepthStencilFormat = DepthFormat.D24X8;
            presentParams.EnableAutoDepthStencil = true;

            try
            {
                device = new Microsoft.DirectX.Direct3D.Device(0, Microsoft.DirectX.Direct3D.DeviceType.Hardware, window3DPtr, CreateFlags.SoftwareVertexProcessing, presentParams);

                keyboard = new Microsoft.DirectX.DirectInput.Device(Microsoft.DirectX.DirectInput.SystemGuid.Keyboard);
                keyboard.SetCooperativeLevel(f, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
                keyboard.Acquire();

                mouse = new Microsoft.DirectX.DirectInput.Device(Microsoft.DirectX.DirectInput.SystemGuid.Mouse);
                mouse.SetCooperativeLevel(f, CooperativeLevelFlags.Background | CooperativeLevelFlags.NonExclusive);
                mouse.Acquire();

                orientation = new MATW(out mat_view, out mat_proj);
                //orientation.Position = new Vector3(0, 0, -200);
                orientation.MoveSpeed = 5;
                orientation.Sensitivity = 1.5f;
                orientation.VerticalMovenment = true;
                //orientation.MoveBack(out mat_view, out mat_proj);

                device.Lights[0].Type = LightType.Directional;
                device.Lights[0].Position = new Vector3(5, 5000, -5);
                device.Lights[0].Diffuse = Color.White;
                device.Lights[0].Ambient = Color.White;
                device.Lights[0].Attenuation0 = 0.2f;
                device.Lights[0].Range = 10000.0f;
                device.Lights[0].Enabled = true;


            }
            catch (DirectXException)
            {
                //MessageBox.Show("Device Error!!!");
                return;
            }
        }

        public void Redraw3D()
        {
            try
            {
                if (!device.Disposed && device != null && crossTex != null)
                {

                    #region Check and set filtering textures
                    if (device.DeviceCaps.TextureFilterCaps.SupportsMinifyAnisotropic)
                    {
                        device.SamplerState[0].MinFilter = TextureFilter.Anisotropic;
                    }
                    else if (device.DeviceCaps.TextureFilterCaps.SupportsMinifyLinear)
                    {
                        device.SamplerState[0].MinFilter = TextureFilter.Linear;
                    }
                    if (device.DeviceCaps.TextureFilterCaps.SupportsMagnifyAnisotropic)
                    {
                        device.SamplerState[0].MagFilter = TextureFilter.Anisotropic;
                    }
                    else if (device.DeviceCaps.TextureFilterCaps.SupportsMagnifyLinear)
                    {
                        device.SamplerState[0].MagFilter = TextureFilter.Linear;
                    }
                    #endregion

                    device.Clear(ClearFlags.ZBuffer | ClearFlags.Target, Color.Black, 1, 0);

                    if (modeView3D)
                    {
                        Cursor.Position = new Point(Screen.PrimaryScreen.WorkingArea.Width / 2, Screen.PrimaryScreen.WorkingArea.Height / 2);

                        UpdateKeyStatus();

                        UpdateMouseStatus();
                    }

                    device.Transform.Projection = mat_proj;
                    device.Transform.View = mat_view;

                    device.BeginScene();

                    device.VertexFormat = CustomVertex.PositionNormalTextured.Format;

                    #region DrawPrimitives

                    foreach (int key in objectsList.Keys)
                    {
                        MyObject obj = (MyObject)objectsList[key];

                        #region CheckSelectionAndSelectionSide
                        if (selectedIndex == -1)
                            device.Material = mat;
                        else
                        {
                            if (!isSelectionHide)
                            {
                                if (obj.index == selectedIndex)
                                {
                                    if (indexSideSel == 1)
                                        device.Material = matSideSel;
                                    else
                                        device.Material = matSel;
                                }
                                else
                                    device.Material = mat;
                            }
                            else
                            {
                                device.Material = mat;
                            }
                        }
                        #endregion

                        device.SetStreamSource(0, obj.buf_FrontSide, 0);
                        device.SetTexture(0, (Texture)textures[obj.textureNames[0].ToString()]);
                        device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

                        #region CheckSelectionAndSelectionSide
                        if (selectedIndex == -1)
                            device.Material = mat;
                        else
                        {
                            if (!isSelectionHide)
                            {
                                if (obj.index == selectedIndex)
                                {
                                    if (indexSideSel == 2)
                                        device.Material = matSideSel;
                                    else
                                        device.Material = matSel;
                                }
                                else
                                    device.Material = mat;
                            }
                            else
                            {
                                device.Material = mat;
                            }
                        }
                        #endregion

                        device.SetStreamSource(0, obj.buf_RightSide, 0);
                        device.SetTexture(0, (Texture)textures[obj.textureNames[1].ToString()]);
                        device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

                        #region CheckSelectionAndSelectionSide
                        if (selectedIndex == -1)
                            device.Material = mat;
                        else
                        {
                            if (!isSelectionHide)
                            {
                                if (obj.index == selectedIndex)
                                {
                                    if (indexSideSel == 3)
                                        device.Material = matSideSel;
                                    else
                                        device.Material = matSel;
                                }
                                else
                                    device.Material = mat;
                            }
                            else
                            {
                                device.Material = mat;
                            }
                        }
                        #endregion

                        device.SetStreamSource(0, obj.buf_BackSide, 0);
                        device.SetTexture(0, (Texture)textures[obj.textureNames[2].ToString()]);
                        device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

                        #region CheckSelectionAndSelectionSide
                        if (selectedIndex == -1)
                            device.Material = mat;
                        else
                        {
                            if (!isSelectionHide)
                            {
                                if (obj.index == selectedIndex)
                                {
                                    if (indexSideSel == 4)
                                        device.Material = matSideSel;
                                    else
                                        device.Material = matSel;
                                }
                                else
                                    device.Material = mat;
                            }
                            else
                            {
                                device.Material = mat;
                            }
                        }
                        #endregion

                        device.SetStreamSource(0, obj.buf_LeftSide, 0);
                        device.SetTexture(0, (Texture)textures[obj.textureNames[3].ToString()]);
                        device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

                        #region CheckSelectionAndSelectionSide
                        if (selectedIndex == -1)
                            device.Material = mat;
                        else
                        {
                            if (!isSelectionHide)
                            {
                                if (obj.index == selectedIndex)
                                {
                                    if (indexSideSel == 5)
                                        device.Material = matSideSel;
                                    else
                                        device.Material = matSel;
                                }
                                else
                                    device.Material = mat;
                            }
                            else
                            {
                                device.Material = mat;
                            }
                        }
                        #endregion

                        device.SetStreamSource(0, obj.buf_UpSide, 0);
                        device.SetTexture(0, (Texture)textures[obj.textureNames[4].ToString()]);
                        device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);

                        #region CheckSelectionAndSelectionSide
                        if (selectedIndex == -1)
                            device.Material = mat;
                        else
                        {
                            if (!isSelectionHide)
                            {
                                if (obj.index == selectedIndex)
                                {
                                    if (indexSideSel == 6)
                                        device.Material = matSideSel;
                                    else
                                        device.Material = matSel;
                                }
                                else
                                    device.Material = mat;
                            }
                            else
                            {
                                device.Material = mat;
                            }
                        }
                        #endregion

                        device.SetStreamSource(0, obj.buf_DownSide, 0);
                        device.SetTexture(0, (Texture)textures[obj.textureNames[5].ToString()]);
                        device.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
                    }
                    #endregion
                    #region DrawPlayers

                    if (playerBlue != null)
                    {
                        if (playerBlue.isPlayerSelected)
                        {
                            device.Material = matPlayerSel;
                            device.SetStreamSource(0, playerBlue.vBuf, 0);
                            device.SetTexture(0, texBluePlayer);
                            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 12);
                        }
                        else
                        {
                            device.Material = mat;
                            device.SetStreamSource(0, playerBlue.vBuf, 0);
                            device.SetTexture(0, texBluePlayer);
                            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 12);
                        }
                    }
                    if (playerRed != null)
                    {
                        if (playerRed.isPlayerSelected)
                        {
                            device.Material = matPlayerSel;
                            device.SetStreamSource(0, playerRed.vBuf, 0);
                            device.SetTexture(0, texRedPlayer);
                            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 12);
                        }
                        else
                        {
                            device.Material = mat;
                            device.SetStreamSource(0, playerRed.vBuf, 0);
                            device.SetTexture(0, texRedPlayer);
                            device.DrawPrimitives(PrimitiveType.TriangleList, 0, 12);
                        }
                    }

                    #endregion


                    cross.Begin(SpriteFlags.AlphaBlend);
                    cross.Draw(crossTex, new Rectangle(0, 0, 25, 25), new Vector3(-(device.Viewport.Width / 2 - 12), -(device.Viewport.Height / 2 - 12), 0), new Vector3(), Color.White.ToArgb());
                    cross.End();

                    //if (vbsel != null)
                    //{
                    //    device.VertexFormat = CustomVertex.PositionNormalColored.Format;
                    //    device.SetStreamSource(0, vbsel, 0);
                    //    device.DrawPrimitives(PrimitiveType.LineList, 0, 1);
                    //}

                    //text.DrawText(null, "Position", new Point(10, 10), Color.Lime);
                    //text.DrawText(null,MATW.m_position.ToString(),new Point(10,20),Color.Lime);
                    //text.DrawText(null, "LookAt", new Point(10, 70), Color.Lime);
                    //text.DrawText(null, MATW.m_lookAt.ToString(), new Point(10, 90), Color.Lime);

                    device.EndScene();

                    device.Present();
                }
            }
            catch
            {
                return;
            }
        }
        public void AddObject(RectPrimitive rpr, string name,int indx)
        {
            try
            {
                Environment.CurrentDirectory = Form1.startingPath;

                if (rpr == null)
                {
                    //MessageBox.Show("Object not added!");
                    return;
                }

                if (!File.Exists(name))
                {
                    //MessageBox.Show("Name of texture is invalid : " + name + "\nerror line 610");
                    return;
                }

                MyObject obj = new MyObject();
                obj.textureNames = new string[6];
                if (!textures.Contains(name))
                    textures.Add(name, TextureLoader.FromFile(device, name));

                obj.mas_CV_PosNormTex = new CustomVertex.PositionNormalTextured[36];

                obj.buf_FrontSide = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 6,
                            device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);

                obj.buf_RightSide = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 6,
                            device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);

                obj.buf_BackSide = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 6,
                            device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);

                obj.buf_LeftSide = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 6,
                            device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);

                obj.buf_UpSide = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 6,
                            device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);

                obj.buf_DownSide = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 6,
                            device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);

                obj.textureNames[0] = name;
                obj.textureNames[1] = name;
                obj.textureNames[2] = name;
                obj.textureNames[3] = name;
                obj.textureNames[4] = name;
                obj.textureNames[5] = name;

                obj.index = indx;

                CustomVertex.PositionNormalTextured[] pnt = new CustomVertex.PositionNormalTextured[6];

                pnt[0] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y)
                    , new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y - 1)
                    , 0, 0);
                pnt[1] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y)
                    , new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y - 1)
                    , 1, 0);
                pnt[2] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y)
                    , new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y - 1)
                    , 1, 1);
                pnt[3] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y)
                    , new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y - 1)
                    , 0, 0);
                pnt[4] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y)
                    , new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y - 1)
                    , 1, 1);
                pnt[5] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y)
                    , new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y - 1)
                    , 0, 1);

                obj.mas_CV_PosNormTex[0] = pnt[0];
                obj.mas_CV_PosNormTex[1] = pnt[1];
                obj.mas_CV_PosNormTex[2] = pnt[2];
                obj.mas_CV_PosNormTex[3] = pnt[3];
                obj.mas_CV_PosNormTex[4] = pnt[4];
                obj.mas_CV_PosNormTex[5] = pnt[5];

                obj.buf_FrontSide.SetData(pnt, 0, LockFlags.None);

                pnt[0] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                    , new Vector3(1 + rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                    , 0, 0);
                pnt[1] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesRight[1].Y, rpr.MasLinesTop[1].Y)
                    , new Vector3(1 + rpr.MasLinesFront[6].X, rpr.MasLinesRight[1].Y, rpr.MasLinesTop[1].Y)
                    , 1, 0);
                pnt[2] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                    , new Vector3(1 + rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                    , 1, 1);
                pnt[3] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                    , new Vector3(1 + rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                    , 0, 0);
                pnt[4] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                    , new Vector3(1 + rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                    , 1, 1);
                pnt[5] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesRight[3].Y, rpr.MasLinesTop[7].Y)
                    , new Vector3(1 + rpr.MasLinesFront[2].X, rpr.MasLinesRight[3].Y, rpr.MasLinesTop[7].Y)
                    , 0, 1);

                obj.mas_CV_PosNormTex[6] = pnt[0];
                obj.mas_CV_PosNormTex[7] = pnt[1];
                obj.mas_CV_PosNormTex[8] = pnt[2];
                obj.mas_CV_PosNormTex[9] = pnt[3];
                obj.mas_CV_PosNormTex[10] = pnt[4];
                obj.mas_CV_PosNormTex[11] = pnt[5];

                obj.buf_RightSide.SetData(pnt, 0, LockFlags.None);

                pnt[0] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y)
                    , new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y + 1)
                    , 0, 0);
                pnt[1] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y)
                    , new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y + 1)
                    , 1, 0);
                pnt[2] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y)
                    , new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y + 1)
                    , 1, 1);
                pnt[3] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y)
                    , new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y + 1)
                    , 0, 0);
                pnt[4] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y)
                    , new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y + 1)
                    , 1, 1);
                pnt[5] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y)
                    , new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y + 1)
                    , 0, 1);

                obj.mas_CV_PosNormTex[12] = pnt[0];
                obj.mas_CV_PosNormTex[13] = pnt[1];
                obj.mas_CV_PosNormTex[14] = pnt[2];
                obj.mas_CV_PosNormTex[15] = pnt[3];
                obj.mas_CV_PosNormTex[16] = pnt[4];
                obj.mas_CV_PosNormTex[17] = pnt[5];

                obj.buf_BackSide.SetData(pnt, 0, LockFlags.None);

                pnt[0] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                    , new Vector3(rpr.MasLinesFront[5].X - 1, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                    , 0, 0);
                pnt[1] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesRight[5].Y, rpr.MasLinesTop[3].Y)
                    , new Vector3(rpr.MasLinesFront[0].X - 1, rpr.MasLinesRight[5].Y, rpr.MasLinesTop[3].Y)
                    , 1, 0);
                pnt[2] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                    , new Vector3(rpr.MasLinesFront[3].X - 1, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                    , 1, 1);
                pnt[3] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                    , new Vector3(rpr.MasLinesFront[5].X - 1, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                    , 0, 0);
                pnt[4] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                    , new Vector3(rpr.MasLinesFront[3].X - 1, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                    , 1, 1);
                pnt[5] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesRight[7].Y, rpr.MasLinesTop[5].Y)
                    , new Vector3(rpr.MasLinesFront[8].X - 1, rpr.MasLinesRight[7].Y, rpr.MasLinesTop[5].Y)
                    , 0, 1);

                obj.mas_CV_PosNormTex[18] = pnt[0];
                obj.mas_CV_PosNormTex[19] = pnt[1];
                obj.mas_CV_PosNormTex[20] = pnt[2];
                obj.mas_CV_PosNormTex[21] = pnt[3];
                obj.mas_CV_PosNormTex[22] = pnt[4];
                obj.mas_CV_PosNormTex[23] = pnt[5];

                obj.buf_LeftSide.SetData(pnt, 0, LockFlags.None);

                pnt[0] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[0].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y)
                    , new Vector3(rpr.MasLinesTop[0].X, rpr.MasLinesFront[5].Y + 1, rpr.MasLinesTop[0].Y)
                    , 0, 0);
                pnt[1] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[1].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y)
                    , new Vector3(rpr.MasLinesTop[1].X, rpr.MasLinesFront[6].Y + 1, rpr.MasLinesTop[1].Y)
                    , 1, 0);
                pnt[2] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[2].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y)
                    , new Vector3(rpr.MasLinesTop[2].X, rpr.MasLinesFront[1].Y + 1, rpr.MasLinesTop[2].Y)
                    , 1, 1);
                pnt[3] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[0].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y)
                    , new Vector3(rpr.MasLinesTop[0].X, rpr.MasLinesFront[5].Y + 1, rpr.MasLinesTop[0].Y)
                    , 0, 0);
                pnt[4] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[2].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y)
                    , new Vector3(rpr.MasLinesTop[2].X, rpr.MasLinesFront[1].Y + 1, rpr.MasLinesTop[2].Y)
                    , 1, 1);
                pnt[5] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[3].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y)
                    , new Vector3(rpr.MasLinesTop[3].X, rpr.MasLinesFront[0].Y + 1, rpr.MasLinesTop[3].Y)
                    , 0, 1);

                obj.mas_CV_PosNormTex[24] = pnt[0];
                obj.mas_CV_PosNormTex[25] = pnt[1];
                obj.mas_CV_PosNormTex[26] = pnt[2];
                obj.mas_CV_PosNormTex[27] = pnt[3];
                obj.mas_CV_PosNormTex[28] = pnt[4];
                obj.mas_CV_PosNormTex[29] = pnt[5];

                obj.buf_UpSide.SetData(pnt, 0, LockFlags.None);

                pnt[0] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y)
                    , new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y + 1, rpr.MasLinesTop[8].Y)
                    , 0, 0);
                pnt[1] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[7].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y)
                    , new Vector3(rpr.MasLinesTop[7].X, rpr.MasLinesFront[2].Y + 1, rpr.MasLinesTop[7].Y)
                    , 1, 0);
                pnt[2] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y)
                    , new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y + 1, rpr.MasLinesTop[6].Y)
                    , 1, 1);
                pnt[3] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y)
                    , new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y + 1, rpr.MasLinesTop[8].Y)
                    , 0, 0);
                pnt[4] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y)
                    , new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y + 1, rpr.MasLinesTop[6].Y)
                    , 1, 1);
                pnt[5] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[5].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y)
                    , new Vector3(rpr.MasLinesTop[5].X, rpr.MasLinesFront[8].Y + 1, rpr.MasLinesTop[5].Y)
                    , 0, 1);

                obj.mas_CV_PosNormTex[30] = pnt[0];
                obj.mas_CV_PosNormTex[31] = pnt[1];
                obj.mas_CV_PosNormTex[32] = pnt[2];
                obj.mas_CV_PosNormTex[33] = pnt[3];
                obj.mas_CV_PosNormTex[34] = pnt[4];
                obj.mas_CV_PosNormTex[35] = pnt[5];

                obj.buf_DownSide.SetData(pnt, 0, LockFlags.None);

                objectsList.Add(indx, obj);
            }
            catch
            {
                return;
            }

        }
        public void AddObject(RectPrimitive rpr, string[] name, int indx, float[] tpF, float[] tpR, float[] tpB, float[] tpL, float[] tpU, float[] tpD)
        {
            try
            {
                Environment.CurrentDirectory = Form1.startingPath;
                if (rpr == null)
                {
                    //MessageBox.Show("Object not added!");
                    return;
                }

                //if (!File.Exists(name))
                //{
                //    //MessageBox.Show("Name of texture is invalid : " + name + "\nerror line 610");
                //    return;
                //}

                MyObject obj = new MyObject();
                obj.textureNames = new string[6];

                obj.mas_CV_PosNormTex = new CustomVertex.PositionNormalTextured[36];

                obj.buf_FrontSide = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 6,
                            device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);

                obj.buf_RightSide = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 6,
                            device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);

                obj.buf_BackSide = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 6,
                            device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);

                obj.buf_LeftSide = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 6,
                            device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);

                obj.buf_UpSide = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 6,
                            device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);

                obj.buf_DownSide = new VertexBuffer(typeof(CustomVertex.PositionNormalTextured), 6,
                            device, Usage.Dynamic | Usage.WriteOnly, CustomVertex.PositionNormalTextured.Format, Pool.Default);


                obj.textureNames[0] = name[0];
                obj.textureNames[1] = name[1];
                obj.textureNames[2] = name[2];
                obj.textureNames[3] = name[3];
                obj.textureNames[4] = name[4];
                obj.textureNames[5] = name[5];

                obj.index = indx;

                CustomVertex.PositionNormalTextured[] pnt = new CustomVertex.PositionNormalTextured[6];

                pnt[0] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y)
                    , new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y - 1)
                    , tpF[0], tpF[1]);
                pnt[1] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y)
                    , new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y - 1)
                    , tpF[2], tpF[3]);
                pnt[2] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y)
                    , new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y - 1)
                    , tpF[4], tpF[5]);
                pnt[3] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y)
                    , new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y - 1)
                    , tpF[6], tpF[7]);
                pnt[4] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y)
                    , new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y - 1)
                    , tpF[8], tpF[9]);
                pnt[5] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y)
                    , new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y - 1)
                    , tpF[10], tpF[11]);

                obj.mas_CV_PosNormTex[0] = pnt[0];
                obj.mas_CV_PosNormTex[1] = pnt[1];
                obj.mas_CV_PosNormTex[2] = pnt[2];
                obj.mas_CV_PosNormTex[3] = pnt[3];
                obj.mas_CV_PosNormTex[4] = pnt[4];
                obj.mas_CV_PosNormTex[5] = pnt[5];

                obj.buf_FrontSide.SetData(pnt, 0, LockFlags.None);

                pnt[0] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                    , new Vector3(1 + rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                    , tpR[0], tpR[1]);
                pnt[1] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesRight[1].Y, rpr.MasLinesTop[1].Y)
                    , new Vector3(1 + rpr.MasLinesFront[6].X, rpr.MasLinesRight[1].Y, rpr.MasLinesTop[1].Y)
                    , tpR[2], tpR[3]);
                pnt[2] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                    , new Vector3(1 + rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                    , tpR[4], tpR[5]);
                pnt[3] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                    , new Vector3(1 + rpr.MasLinesFront[1].X, rpr.MasLinesRight[0].Y, rpr.MasLinesTop[2].Y)
                    , tpR[6], tpR[7]);
                pnt[4] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                    , new Vector3(1 + rpr.MasLinesFront[7].X, rpr.MasLinesRight[2].Y, rpr.MasLinesTop[6].Y)
                    , tpR[8], tpR[9]);
                pnt[5] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[2].X, rpr.MasLinesRight[3].Y, rpr.MasLinesTop[7].Y)
                    , new Vector3(1 + rpr.MasLinesFront[2].X, rpr.MasLinesRight[3].Y, rpr.MasLinesTop[7].Y)
                    , tpR[10], tpR[11]);

                obj.mas_CV_PosNormTex[6] = pnt[0];
                obj.mas_CV_PosNormTex[7] = pnt[1];
                obj.mas_CV_PosNormTex[8] = pnt[2];
                obj.mas_CV_PosNormTex[9] = pnt[3];
                obj.mas_CV_PosNormTex[10] = pnt[4];
                obj.mas_CV_PosNormTex[11] = pnt[5];

                obj.buf_RightSide.SetData(pnt, 0, LockFlags.None);

                pnt[0] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y)
                    , new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y + 1)
                    , tpB[0], tpB[1]);
                pnt[1] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y)
                    , new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y + 1)
                    , tpB[2], tpB[3]);
                pnt[2] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y)
                    , new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y + 1)
                    , tpB[4], tpB[5]);
                pnt[3] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y)
                    , new Vector3(rpr.MasLinesFront[6].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y + 1)
                    , tpB[6], tpB[7]);
                pnt[4] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y)
                    , new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y + 1)
                    , tpB[8], tpB[9]);
                pnt[5] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y)
                    , new Vector3(rpr.MasLinesFront[7].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y + 1)
                    , tpB[10], tpB[11]);

                obj.mas_CV_PosNormTex[12] = pnt[0];
                obj.mas_CV_PosNormTex[13] = pnt[1];
                obj.mas_CV_PosNormTex[14] = pnt[2];
                obj.mas_CV_PosNormTex[15] = pnt[3];
                obj.mas_CV_PosNormTex[16] = pnt[4];
                obj.mas_CV_PosNormTex[17] = pnt[5];

                obj.buf_BackSide.SetData(pnt, 0, LockFlags.None);

                pnt[0] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                    , new Vector3(rpr.MasLinesFront[5].X - 1, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                    , tpL[0], tpL[1]);
                pnt[1] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[0].X, rpr.MasLinesRight[5].Y, rpr.MasLinesTop[3].Y)
                    , new Vector3(rpr.MasLinesFront[0].X - 1, rpr.MasLinesRight[5].Y, rpr.MasLinesTop[3].Y)
                    , tpL[2], tpL[3]);
                pnt[2] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                    , new Vector3(rpr.MasLinesFront[3].X - 1, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                    , tpL[4], tpL[5]);
                pnt[3] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[5].X, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                    , new Vector3(rpr.MasLinesFront[5].X - 1, rpr.MasLinesRight[6].Y, rpr.MasLinesTop[0].Y)
                    , tpL[6], tpL[7]);
                pnt[4] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[3].X, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                    , new Vector3(rpr.MasLinesFront[3].X - 1, rpr.MasLinesRight[8].Y, rpr.MasLinesTop[8].Y)
                    , tpL[8], tpL[9]);
                pnt[5] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesFront[8].X, rpr.MasLinesRight[7].Y, rpr.MasLinesTop[5].Y)
                    , new Vector3(rpr.MasLinesFront[8].X - 1, rpr.MasLinesRight[7].Y, rpr.MasLinesTop[5].Y)
                    , tpL[10], tpL[11]);

                obj.mas_CV_PosNormTex[18] = pnt[0];
                obj.mas_CV_PosNormTex[19] = pnt[1];
                obj.mas_CV_PosNormTex[20] = pnt[2];
                obj.mas_CV_PosNormTex[21] = pnt[3];
                obj.mas_CV_PosNormTex[22] = pnt[4];
                obj.mas_CV_PosNormTex[23] = pnt[5];

                obj.buf_LeftSide.SetData(pnt, 0, LockFlags.None);

                pnt[0] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[0].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y)
                    , new Vector3(rpr.MasLinesTop[0].X, rpr.MasLinesFront[5].Y + 1, rpr.MasLinesTop[0].Y)
                    , tpU[0], tpU[1]);
                pnt[1] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[1].X, rpr.MasLinesFront[6].Y, rpr.MasLinesTop[1].Y)
                    , new Vector3(rpr.MasLinesTop[1].X, rpr.MasLinesFront[6].Y + 1, rpr.MasLinesTop[1].Y)
                    , tpU[2], tpU[3]);
                pnt[2] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[2].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y)
                    , new Vector3(rpr.MasLinesTop[2].X, rpr.MasLinesFront[1].Y + 1, rpr.MasLinesTop[2].Y)
                    , tpU[4], tpU[5]);
                pnt[3] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[0].X, rpr.MasLinesFront[5].Y, rpr.MasLinesTop[0].Y)
                    , new Vector3(rpr.MasLinesTop[0].X, rpr.MasLinesFront[5].Y + 1, rpr.MasLinesTop[0].Y)
                    , tpU[6], tpU[7]);
                pnt[4] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[2].X, rpr.MasLinesFront[1].Y, rpr.MasLinesTop[2].Y)
                    , new Vector3(rpr.MasLinesTop[2].X, rpr.MasLinesFront[1].Y + 1, rpr.MasLinesTop[2].Y)
                    , tpU[8], tpU[9]);
                pnt[5] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[3].X, rpr.MasLinesFront[0].Y, rpr.MasLinesTop[3].Y)
                    , new Vector3(rpr.MasLinesTop[3].X, rpr.MasLinesFront[0].Y + 1, rpr.MasLinesTop[3].Y)
                    , tpU[10], tpU[11]);

                obj.mas_CV_PosNormTex[24] = pnt[0];
                obj.mas_CV_PosNormTex[25] = pnt[1];
                obj.mas_CV_PosNormTex[26] = pnt[2];
                obj.mas_CV_PosNormTex[27] = pnt[3];
                obj.mas_CV_PosNormTex[28] = pnt[4];
                obj.mas_CV_PosNormTex[29] = pnt[5];

                obj.buf_UpSide.SetData(pnt, 0, LockFlags.None);

                pnt[0] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y)
                    , new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y + 1, rpr.MasLinesTop[8].Y)
                    , tpD[0], tpD[1]);
                pnt[1] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[7].X, rpr.MasLinesFront[2].Y, rpr.MasLinesTop[7].Y)
                    , new Vector3(rpr.MasLinesTop[7].X, rpr.MasLinesFront[2].Y + 1, rpr.MasLinesTop[7].Y)
                    , tpD[2], tpD[3]);
                pnt[2] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y)
                    , new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y + 1, rpr.MasLinesTop[6].Y)
                    , tpD[4], tpD[5]);
                pnt[3] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y, rpr.MasLinesTop[8].Y)
                    , new Vector3(rpr.MasLinesTop[8].X, rpr.MasLinesFront[3].Y + 1, rpr.MasLinesTop[8].Y)
                    , tpD[6], tpD[7]);
                pnt[4] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y, rpr.MasLinesTop[6].Y)
                    , new Vector3(rpr.MasLinesTop[6].X, rpr.MasLinesFront[7].Y + 1, rpr.MasLinesTop[6].Y)
                    , tpD[8], tpD[9]);
                pnt[5] = new CustomVertex.PositionNormalTextured(
                    new Vector3(rpr.MasLinesTop[5].X, rpr.MasLinesFront[8].Y, rpr.MasLinesTop[5].Y)
                    , new Vector3(rpr.MasLinesTop[5].X, rpr.MasLinesFront[8].Y + 1, rpr.MasLinesTop[5].Y)
                    , tpD[10], tpD[11]);

                obj.mas_CV_PosNormTex[30] = pnt[0];
                obj.mas_CV_PosNormTex[31] = pnt[1];
                obj.mas_CV_PosNormTex[32] = pnt[2];
                obj.mas_CV_PosNormTex[33] = pnt[3];
                obj.mas_CV_PosNormTex[34] = pnt[4];
                obj.mas_CV_PosNormTex[35] = pnt[5];

                obj.buf_DownSide.SetData(pnt, 0, LockFlags.None);

                objectsList.Add(indx, obj);
            }
            catch
            {
                return;
            }
        }

        public void UpdateKeyStatus()
        {
            try
            {
                if (keyboard != null)
                {
                    foreach (Key key in keyboard.GetPressedKeys())
                    {
                        switch (key)
                        {
                            case Key.A:
                                orientation.MoveLeft(out mat_view, out mat_proj);
                                break;

                            case Key.D:
                                orientation.MoveRight(out mat_view, out mat_proj);
                                break;

                            case Key.W:
                                orientation.MoveForward(out mat_view, out mat_proj);
                                break;

                            case Key.S:
                                orientation.MoveBack(out mat_view, out mat_proj);
                                break;
                            case Key.LeftShift:
                                orientation.MoveDown(out mat_view, out mat_proj);
                                break;

                            case Key.Space:
                                orientation.MoveUp(out mat_view, out mat_proj);
                                break;

                        }
                    }
                }
            }
            catch
            {
                return;
            }
        }
        public void UpdateMouseStatus()
        {
            try
            {
                if (mouse != null)
                {
                    byte[] bt;

                    MouseState state = mouse.CurrentMouseState;
                    bt = state.GetMouseButtons();
                    if (bt[0] != 0)
                    {
                        if (canMouseClick)
                        {
                            canMouseClick = false;
                            if (MouseClick3D != null)
                            {
                                MouseClick3D();
                            }
                        }
                    }
                    else
                        canMouseClick = true;

                    if (state.X > 0)
                    {
                        orientation.RotateRight(out mat_view, out mat_proj, state.X);
                        //Cursor.Position = new Point(device.DisplayMode.Width / 2, device.DisplayMode.Height / 2);
                    }
                    if (state.X < 0)
                    {
                        orientation.RotateLeft(out mat_view, out mat_proj, state.X);
                        //Cursor.Position = new Point(device.DisplayMode.Width / 2, device.DisplayMode.Height / 2);
                    }
                    if (state.Y > 0)
                    {
                        orientation.RotateUp(out mat_view, out mat_proj, state.Y);
                        //Cursor.Position = new Point(device.DisplayMode.Width / 2, device.DisplayMode.Height / 2);
                    }
                    if (state.Y < 0)
                    {
                        orientation.RotateDown(out mat_view, out mat_proj, state.Y);
                        //Cursor.Position = new Point(device.DisplayMode.Width / 2, device.DisplayMode.Height / 2);
                    }
                }
            }
            catch
            {
                return;
            }
        }

        public void Clear()
        {
            try
            {
                if (objectsList != null)
                {
                    foreach (int key in objectsList.Keys)
                    {
                        MyObject ob = ((MyObject)objectsList[key]);
                        //objectsList.Remove(key);
                        ob.Dispose();
                    }
                    objectsList = null;
                }
                objectsList = new Hashtable();
                playerBlue = null;
                playerRed = null;
            }
            catch
            {
                return;
            }
        }
    }
}
