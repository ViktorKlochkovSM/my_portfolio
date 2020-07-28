using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using Microsoft.DirectX.Direct3D;
using System.IO;

namespace AVGame
{
    /// <summary>
    /// Класс отвечает за перемещение в пространстве.
    /// </summary>
    public class MATW:IDisposable
    {
        //VARIABLES
        private Matrix m_projection;
        private Matrix m_view;
        public static Vector3 m_right;
        private Vector3 m_up;
        static public Vector3 m_look = new Vector3(0.0f, 0.0f, 1.0f);
        public static Vector3 m_position = new Vector3(0.0f, 0.0f, 0.0f);
        public static Vector3 m_lookAt;
        private Vector3 m_velocity = new Vector3(0.0f, 0.0f, 0.0f);
        public static float m_yaw;
        public static float m_pitch;
        private float m_maxPitch = Geometry.DegreeToRadian(80.0f);
        private float sens = 1.0f;
        private float speed = 0.3f;
        private float fasterStep = 20;
        private float faster = 1.5f;
        private bool verticalMovenment = false;
        private bool ghost = false;
        //bool xx;
        //bool zz;

        /// <summary>
        /// Скорость поворота.
        /// </summary>
        public float Sensitivity
        {
            get { return sens; }
            set { sens = value; }
        }
        /// <summary>
        /// Максимальный угол вертикального обзора.
        /// </summary>
        public float MaxUpAndDownAngle
        {
            get { return m_maxPitch; }
            set { m_maxPitch = (value < 90) ? Geometry.DegreeToRadian(value) : 90; }
        }
        /// <summary>
        /// Скорость перемещения в пространстве.
        /// </summary>
        public float MoveSpeed
        {
            get { return speed; }
            set { speed = value; }
        }
        /// <summary>
        /// Ускорение поворота при увеличении скорости перемещения мыши.
        /// </summary>
        public float FasterSpeed
        {
            get { return faster; }
            set { faster = value; }
        }
        /// <summary>
        /// Порог при котором срабатывает увеличение скорости поворота.
        /// </summary>
        public float FasterStep
        {
            get { return fasterStep; }
            set { fasterStep = value; }
        }
        /// <summary>
        /// Включает или отключает перемещение по вертикальной оси.
        /// </summary>
        public bool VerticalMovenment
        {
            get { return verticalMovenment; }
            set { verticalMovenment = value; }
        }
        /// <summary>
        ///Устанавливает позицию камеры.
        /// </summary>
        public Vector3 Position
        {
            get { return m_position; }
            set { m_position = value; Calculate(); }
        }
        /// <summary>
        /// Возможность проходить через стены
        /// </summary>
        public bool Ghost
        {
            get { return ghost; }
            set { ghost = true; }
        }
        /// <summary>
        /// Возвращает начальные координаты.
        /// </summary>
        /// <param name="view">Матрица вида.(Выходной параметр)</param>
        /// <param name="progection">Матрица проэкции.(Выходной параметр)</param>
        public MATW(out Matrix view, out Matrix progection)
        {
            Calculate();
            view = m_view;
            progection = m_projection;
        }
		public void Dispose()
		{
			GC.SuppressFinalize(this);
		}
		~MATW()
		{
			Dispose();
		}
        /// <summary>
        /// Производит окончательные вычисления над матрицами и векторами.
        /// </summary>
        private void Calculate()
        {
            m_position += m_velocity;
            m_velocity = new Vector3();
            m_lookAt = m_position + m_look;

            m_view = Matrix.LookAtLH(m_position, m_lookAt, new Vector3(0, 1.0f, 0));
            m_projection = Matrix.PerspectiveFovLH((float)Math.PI / 4, 1.3f, 0.5f, 10000.0f);

            m_right.X = m_view.M11;
            m_right.Y = m_view.M21;
            m_right.Z = m_view.M31;
            m_up.X = m_view.M12;
            m_up.Y = m_view.M22;
            m_up.Z = m_view.M32;
            m_look.X = m_view.M13;
            m_look.Y = m_view.M23;
            m_look.Z = m_view.M33;

            float lookLengthOnXZ = (float)Math.Sqrt(m_look.Z * m_look.Z + m_look.X * m_look.X);
            m_pitch = (float)Math.Atan2(m_look.Y, lookLengthOnXZ);
            m_yaw = (float)Math.Atan2(m_look.X, m_look.Z);
        }
        /// <summary>
        /// Сдвигает координаты влево и возвращает готовую матрицу.
        /// </summary>
        /// <param name="view">Матрица вида.(Выходной параметр)</param>
        /// <param name="progection">Матрица проэкции.(Выходной параметр)</param>
        public void MoveLeft(out Matrix view, out Matrix progection)
        {

            m_velocity += new Vector3(m_right.X * -speed, 0.0f, m_right.Z * -speed);

            Calculate();

            view = m_view;
            progection = m_projection;
        }
        /// <summary>
        /// Сдвигает координаты вправо и возвращает готовую матрицу.
        /// </summary>
        /// <param name="view">Матрица вида.(Выходной параметр)</param>
        /// <param name="progection">Матрица проэкции.(Выходной параметр)</param>
        public void MoveRight(out Matrix view, out Matrix progection)
        {
            m_velocity += new Vector3(m_right.X * speed, 0.0f, m_right.Z * speed);

            Calculate();

            view = m_view;
            progection = m_projection;
        }
        /// <summary>
        /// Сдвигает координаты вперёд и возвращает готовую матрицу.
        /// </summary>
        /// <param name="view">Матрица вида.(Выходной параметр)</param>
        /// <param name="progection">Матрица проэкции.(Выходной параметр)</param>
        public void MoveForward(out Matrix view, out Matrix progection)
        {
            if (verticalMovenment)
            {
                m_velocity += m_look * speed;
            }
            else
            {
                Vector3 moveVector = new Vector3();


                moveVector = new Vector3(m_look.X, 0.0f, m_look.Z);

                moveVector.Normalize();
                moveVector *= speed;
                m_velocity += moveVector;
            }

            Calculate();

            view = m_view;
            progection = m_projection;
        }
        /// <summary>
        /// Сдвигает координаты назад и возвращает готовую матрицу.
        /// </summary>
        /// <param name="view">Матрица вида.(Выходной параметр)</param>
        /// <param name="progection">Матрица проэкции.(Выходной параметр)</param>
        /// public void MoveForward(out Matrix view, out Matrix progection)
        public void MoveBack(out Matrix view, out Matrix progection)
        {
            if (verticalMovenment)
            {
                m_velocity += m_look * -speed;
            }
            else
            {
                Vector3 moveVector = new Vector3();


                moveVector = new Vector3(m_look.X, 0.0f, m_look.Z);


                moveVector.Normalize();
                moveVector *= -speed;
                m_velocity += moveVector;
            }

            Calculate();

            view = m_view;
            progection = m_projection;
        }
        /// <summary>
        /// Сдвигает координаты вниз и возвращает готовую матрицу.
        /// </summary>
        /// <param name="view">Матрица вида.(Выходной параметр)</param>
        /// <param name="progection">Матрица проэкции.(Выходной параметр)</param>
        /// public void MoveForward(out Matrix view, out Matrix progection)
        public void MoveDown(out Matrix view, out Matrix progection)
        {
            m_velocity.Y -= speed;

            Calculate();
            view = m_view;
            progection = m_projection;
        }
        /// <summary>
        /// Сдвигает координаты вверх и возвращает готовую матрицу.
        /// </summary>
        /// <param name="view">Матрица вида.(Выходной параметр)</param>
        /// <param name="progection">Матрица проэкции.(Выходной параметр)</param>
        /// public void MoveForward(out Matrix view, out Matrix progection)
        public void MoveUp(out Matrix view, out Matrix progection)
        {
            m_velocity.Y += speed;

            Calculate();
            view = m_view;
            progection = m_projection;
        }
        /// <summary>
        /// Поворачивает координаты влево и возвращает готовую матрицу.
        /// </summary>
        /// <param name="view">Матрица вида.(Выходной параметр)</param>
        /// <param name="progection">Матрица проэкции.(Выходной параметр)</param>
        /// public void MoveForward(out Matrix view, out Matrix progection)
        public void RotateLeft(out Matrix view, out Matrix progection, float x)
        {
            float radians; ;
            if (x < -fasterStep)
                radians = Geometry.DegreeToRadian(-(sens + faster));
            else
                radians = Geometry.DegreeToRadian(-sens);

            Matrix rotation = Matrix.RotationAxis(m_up, radians);
            m_right = Vector3.TransformNormal(m_right, rotation);
            m_look = Vector3.TransformNormal(m_look, rotation);

            Calculate();
            view = m_view;
            progection = m_projection;
        }
        /// <summary>
        /// Поворачивает координаты вправо и возвращает готовую матрицу.
        /// </summary>
        /// <param name="view">Матрица вида.(Выходной параметр)</param>
        /// <param name="progection">Матрица проэкции.(Выходной параметр)</param>
        /// public void MoveForward(out Matrix view, out Matrix progection)
        public void RotateRight(out Matrix view, out Matrix progection, float x)
        {
            float radians; ;
            if (x > fasterStep)
                radians = Geometry.DegreeToRadian(sens + faster);
            else
                radians = Geometry.DegreeToRadian(sens);

            Matrix rotation = Matrix.RotationAxis(m_up, radians);
            m_right = Vector3.TransformNormal(m_right, rotation);
            m_look = Vector3.TransformNormal(m_look, rotation);

            Calculate();
            view = m_view;
            progection = m_projection;
        }
        /// <summary>
        /// Поворачивает координаты вверх и возвращает готовую матрицу.
        /// </summary>
        /// <param name="view">Матрица вида.(Выходной параметр)</param>
        /// <param name="progection">Матрица проэкции.(Выходной параметр)</param>
        /// public void MoveForward(out Matrix view, out Matrix progection)
        public void RotateUp(out Matrix view, out Matrix progection, float y)
        {
            float radians; ;
            if (y > fasterStep)
                radians = Geometry.DegreeToRadian(sens + faster);
            else
                radians = Geometry.DegreeToRadian(sens);

            m_pitch -= radians;
            if (m_pitch > m_maxPitch)
            {
                radians += m_pitch - m_maxPitch;
            }
            else if (m_pitch < -m_maxPitch)
            {
                radians += m_pitch + m_maxPitch;
            }
            Matrix rotation = Matrix.RotationAxis(m_right, radians);
            m_up = Vector3.TransformNormal(m_up, rotation);
            m_look = Vector3.TransformNormal(m_look, rotation);

            Calculate();
            view = m_view;
            progection = m_projection;
        }
        /// <summary>
        /// Поворачивает координаты вниз и возвращает готовую матрицу.
        /// </summary>
        /// <param name="view">Матрица вида.(Выходной параметр)</param>
        /// <param name="progection">Матрица проэкции.(Выходной параметр)</param>
        /// public void MoveForward(out Matrix view, out Matrix progection)
        public void RotateDown(out Matrix view, out Matrix progection, float y)
        {
            float radians; 
            if (y < -fasterStep)
                radians = Geometry.DegreeToRadian(-(sens + faster));
            else
                radians = Geometry.DegreeToRadian(-sens);

            m_pitch -= radians;

            if (m_pitch > m_maxPitch)
            {
                radians += m_pitch - m_maxPitch;
            }
            else if (m_pitch < -m_maxPitch)
            {
                radians += m_pitch + m_maxPitch;
            }
            Matrix rotation = Matrix.RotationAxis(m_right, radians);
            m_up = Vector3.TransformNormal(m_up, rotation);
            m_look = Vector3.TransformNormal(m_look, rotation);

            Calculate();
            view = m_view;
            progection = m_projection;
        }
    }
}
