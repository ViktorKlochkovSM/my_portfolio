using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using AVGame;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;

namespace My3DMapEditor
{

    class PickObject
    {
        Vector3 look = new Vector3();
        Vector3 tmp = new Vector3();
        Vector3 pos = new Vector3();

        public PickObject()
        {
        }

        public int PickVector(Microsoft.DirectX.Direct3D.Device device, Hashtable ht,Player red,Player blue,out string NameSelectedplayer)
        {


            look = MATW.m_position;
            tmp = MATW.m_look;



            while (true)// (int i = 1; i < 80000; i++)
            {
                if (look.Z > 5000 ||
                    look.Z < -5000)
                {
                    NameSelectedplayer = "NoPlayer";
                    return -1;
                }

                if (look.Y > 5000 ||
                    look.Y < -5000)
                {
                    NameSelectedplayer = "NoPlayer";
                    return -1;
                }

                if (look.X > 5000 ||
                    look.X < -5000)
                {
                    NameSelectedplayer = "NoPlayer";
                    return -1;
                }

                //tmp *= 2.5f;
                look += tmp;
                //Managed3D.RecreateSelectionLineBuffer(MATW.m_position, look);

                #region PickPlayers
                if (red != null)
                {
                    if (MATW.m_position.Z < MATW.m_lookAt.Z)
                    {
                        if (look.Z - 3 < red.cv_PnTex[0].Z)
                        {
                            if (look.X > red.cv_PnTex[0].X && look.X < red.cv_PnTex[1].X)
                            {
                                if (look.Y < red.cv_PnTex[0].Y && look.Y > red.cv_PnTex[5].Y)
                                {
                                    if (look.Z > red.cv_PnTex[0].Z)
                                    {
                                        NameSelectedplayer = "red";
                                        return -1;
                                    }
                                }
                            }
                        }
                    }
                    //Pravo
                    if (MATW.m_position.X < MATW.m_lookAt.X)
                    {
                        if (look.X - 3 < red.cv_PnTex[18].X)
                        {
                            if (look.Z < red.cv_PnTex[18].Z && look.Z > red.cv_PnTex[19].Z)
                            {
                                if (look.Y < red.cv_PnTex[18].Y && look.Y > red.cv_PnTex[23].Y)
                                {
                                    if (look.X > red.cv_PnTex[18].X)
                                    {
                                        NameSelectedplayer = "red";
                                        return -1;
                                    }
                                }
                            }
                        }
                    }
                    //Levo
                    if (MATW.m_position.X > MATW.m_lookAt.X)
                    {
                        if (look.X + 3 > red.cv_PnTex[6].X)
                        {
                            if (look.Z > red.cv_PnTex[6].Z && look.Z < red.cv_PnTex[7].Z)
                            {
                                if (look.Y < red.cv_PnTex[6].Y && look.Y > red.cv_PnTex[11].Y)
                                {
                                    if (look.X < red.cv_PnTex[6].X)
                                    {
                                        NameSelectedplayer = "red";
                                        return -1;
                                    }
                                }
                            }
                        }
                    }
                    //Zad
                    if (MATW.m_position.Z > MATW.m_lookAt.Z)
                    {
                        if (look.Z + 3 > red.cv_PnTex[12].Z)
                        {
                            if (look.X < red.cv_PnTex[12].X && look.X > red.cv_PnTex[13].X)
                            {
                                if (look.Y < red.cv_PnTex[12].Y && look.Y > red.cv_PnTex[17].Y)
                                {
                                    if (look.Z < red.cv_PnTex[12].Z)
                                    {
                                        NameSelectedplayer = "red";
                                        return -1;
                                    }
                                }
                            }
                        }
                    }
                    //Niz
                    if (MATW.m_position.Y > MATW.m_lookAt.Y)
                    {
                        if (look.Y + 3 > red.cv_PnTex[24].Y)
                        {
                            if (look.X > red.cv_PnTex[24].X && look.X < red.cv_PnTex[25].X)
                            {
                                if (look.Z < red.cv_PnTex[24].Z && look.Z > red.cv_PnTex[29].Z)
                                {
                                    if (look.Y < red.cv_PnTex[24].Y)
                                    {
                                        NameSelectedplayer = "red";
                                        return -1;
                                    }
                                }
                            }
                        }
                    }

                    if (MATW.m_position.Y < MATW.m_lookAt.Y)
                    {
                        if (look.Y - 3 < red.cv_PnTex[30].Y)
                        {
                            if (look.X > red.cv_PnTex[30].X && look.X < red.cv_PnTex[31].X)
                            {
                                if (look.Z > red.cv_PnTex[30].Z && look.Z < red.cv_PnTex[35].Z)
                                {
                                    if (look.Y > red.cv_PnTex[30].Y)
                                    {
                                        NameSelectedplayer = "red";
                                        return -1;
                                    }
                                }
                            }
                        }
                    }
                }
                if (blue != null)
                {
                    if (MATW.m_position.Z < MATW.m_lookAt.Z)
                    {
                        if (look.Z - 3 < blue.cv_PnTex[0].Z)
                        {
                            if (look.X > blue.cv_PnTex[0].X && look.X < blue.cv_PnTex[1].X)
                            {
                                if (look.Y < blue.cv_PnTex[0].Y && look.Y > blue.cv_PnTex[5].Y)
                                {
                                    if (look.Z > blue.cv_PnTex[0].Z)
                                    {
                                        NameSelectedplayer = "blue";
                                        return -1;
                                    }
                                }
                            }
                        }
                    }
                    //Pravo
                    if (MATW.m_position.X < MATW.m_lookAt.X)
                    {
                        if (look.X - 3 < blue.cv_PnTex[18].X)
                        {
                            if (look.Z < blue.cv_PnTex[18].Z && look.Z > blue.cv_PnTex[19].Z)
                            {
                                if (look.Y < blue.cv_PnTex[18].Y && look.Y > blue.cv_PnTex[23].Y)
                                {
                                    if (look.X > blue.cv_PnTex[18].X)
                                    {
                                        NameSelectedplayer = "blue";
                                        return -1;
                                    }
                                }
                            }
                        }
                    }
                    //Levo
                    if (MATW.m_position.X > MATW.m_lookAt.X)
                    {
                        if (look.X + 3 > blue.cv_PnTex[6].X)
                        {
                            if (look.Z > blue.cv_PnTex[6].Z && look.Z < blue.cv_PnTex[7].Z)
                            {
                                if (look.Y < blue.cv_PnTex[6].Y && look.Y > blue.cv_PnTex[11].Y)
                                {
                                    if (look.X < blue.cv_PnTex[6].X)
                                    {
                                        NameSelectedplayer = "blue";
                                        return -1;
                                    }
                                }
                            }
                        }
                    }
                    //Zad
                    if (MATW.m_position.Z > MATW.m_lookAt.Z)
                    {
                        if (look.Z + 3 > blue.cv_PnTex[12].Z)
                        {
                            if (look.X < blue.cv_PnTex[12].X && look.X > blue.cv_PnTex[13].X)
                            {
                                if (look.Y < blue.cv_PnTex[12].Y && look.Y > blue.cv_PnTex[17].Y)
                                {
                                    if (look.Z < blue.cv_PnTex[12].Z)
                                    {
                                        NameSelectedplayer = "blue";
                                        return -1;
                                    }
                                }
                            }
                        }
                    }
                    //Niz
                    if (MATW.m_position.Y > MATW.m_lookAt.Y)
                    {
                        if (look.Y + 3 > blue.cv_PnTex[24].Y)
                        {
                            if (look.X > blue.cv_PnTex[24].X && look.X < blue.cv_PnTex[25].X)
                            {
                                if (look.Z < blue.cv_PnTex[24].Z && look.Z > blue.cv_PnTex[29].Z)
                                {
                                    if (look.Y < blue.cv_PnTex[24].Y)
                                    {
                                        NameSelectedplayer = "blue";
                                        return -1;
                                    }
                                }
                            }
                        }
                    }

                    if (MATW.m_position.Y < MATW.m_lookAt.Y)
                    {
                        if (look.Y - 3 < blue.cv_PnTex[30].Y)
                        {
                            if (look.X > blue.cv_PnTex[30].X && look.X < blue.cv_PnTex[31].X)
                            {
                                if (look.Z > blue.cv_PnTex[30].Z && look.Z < blue.cv_PnTex[35].Z)
                                {
                                    if (look.Y > blue.cv_PnTex[30].Y)
                                    {
                                        NameSelectedplayer = "blue";
                                        return -1;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                foreach (int key in ht.Keys)
                {
                    MyObject obj = (MyObject)ht[key];

                    //Pered
                    if (MATW.m_position.Z < MATW.m_lookAt.Z)
                    {
                        if (look.Z - 3 < obj.mas_CV_PosNormTex[0].Z)
                        {
                            if (look.X > obj.mas_CV_PosNormTex[0].X && look.X < obj.mas_CV_PosNormTex[1].X)
                            {
                                if (look.Y < obj.mas_CV_PosNormTex[0].Y && look.Y > obj.mas_CV_PosNormTex[5].Y)
                                {
                                    if (look.Z > obj.mas_CV_PosNormTex[0].Z)
                                    {
                                        NameSelectedplayer = "NoPlayer";
                                        return key;
                                    }
                                }
                            }
                        }
                    }
                    //Pravo
                    if (MATW.m_position.X < MATW.m_lookAt.X)
                    {
                        if (look.X - 3 < obj.mas_CV_PosNormTex[18].X)
                        {
                            if (look.Z < obj.mas_CV_PosNormTex[18].Z && look.Z > obj.mas_CV_PosNormTex[19].Z)
                            {
                                if (look.Y < obj.mas_CV_PosNormTex[18].Y && look.Y > obj.mas_CV_PosNormTex[23].Y)
                                {
                                    if (look.X > obj.mas_CV_PosNormTex[18].X)
                                    {
                                        NameSelectedplayer = "NoPlayer";
                                        return key;
                                    }
                                }
                            }
                        }
                    }
                    //Levo
                    if (MATW.m_position.X > MATW.m_lookAt.X)
                    {
                        if (look.X + 3 > obj.mas_CV_PosNormTex[6].X)
                        {
                            if (look.Z > obj.mas_CV_PosNormTex[6].Z && look.Z < obj.mas_CV_PosNormTex[7].Z)
                            {
                                if (look.Y < obj.mas_CV_PosNormTex[6].Y && look.Y > obj.mas_CV_PosNormTex[11].Y)
                                {
                                    if (look.X < obj.mas_CV_PosNormTex[6].X)
                                    {
                                        NameSelectedplayer = "NoPlayer";
                                        return key;
                                    }
                                }
                            }
                        }
                    }
                    //Zad
                    if (MATW.m_position.Z > MATW.m_lookAt.Z)
                    {
                        if (look.Z + 3 > obj.mas_CV_PosNormTex[12].Z)
                        {
                            if (look.X < obj.mas_CV_PosNormTex[12].X && look.X > obj.mas_CV_PosNormTex[13].X)
                            {
                                if (look.Y < obj.mas_CV_PosNormTex[12].Y && look.Y > obj.mas_CV_PosNormTex[17].Y)
                                {
                                    if (look.Z < obj.mas_CV_PosNormTex[12].Z)
                                    {
                                        NameSelectedplayer = "NoPlayer";
                                        return key;
                                    }
                                }
                            }
                        }
                    }
                    //Niz
                    if (MATW.m_position.Y > MATW.m_lookAt.Y)
                    {
                        if (look.Y + 3 > obj.mas_CV_PosNormTex[24].Y)
                        {
                            if (look.X > obj.mas_CV_PosNormTex[24].X && look.X < obj.mas_CV_PosNormTex[25].X)
                            {
                                if (look.Z < obj.mas_CV_PosNormTex[24].Z && look.Z > obj.mas_CV_PosNormTex[29].Z)
                                {
                                    if (look.Y < obj.mas_CV_PosNormTex[24].Y)
                                    {
                                        NameSelectedplayer = "NoPlayer";
                                        return key;
                                    }
                                }
                            }
                        }
                    }

                    if (MATW.m_position.Y < MATW.m_lookAt.Y)
                    {
                        if (look.Y - 3 < obj.mas_CV_PosNormTex[30].Y)
                        {
                            if (look.X > obj.mas_CV_PosNormTex[30].X && look.X < obj.mas_CV_PosNormTex[31].X)
                            {
                                if (look.Z > obj.mas_CV_PosNormTex[30].Z && look.Z < obj.mas_CV_PosNormTex[35].Z)
                                {
                                    if (look.Y > obj.mas_CV_PosNormTex[30].Y)
                                    {
                                        NameSelectedplayer = "NoPlayer";
                                        return key;
                                    }
                                }
                            }
                        }
                    }
                }
                
            }
            return -1;
        }
    }
}
