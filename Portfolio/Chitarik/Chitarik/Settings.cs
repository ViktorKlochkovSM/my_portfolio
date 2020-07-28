using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Chitarik
{
    public enum ReadMode {Slova = 0, Predlogenie = 1, Stishki = 2, Skorogovorki = 3, Songs = 4, Sloga = 5, Schitalki = 6, Rasskazy = 7 };
    public enum MultiLineContentType {Stishki = 0, Songs = 1, Rasskazy = 2, Skorogovorki = 3, Schitalki = 4  };

    [Serializable()]
    public static class Settings
    {
        /// <summary>
        /// Имя файла настроек для чтения/записи настроек из файла
        /// </summary>
        public static string FileNameSettings = "Settings.bin";

        static Font fontGlobal = new Font("Arial", 72, FontStyle.Bold);
        /// <summary>
        /// Настройки шрифта для выводимого контента
        /// </summary>
        public static Font FontGlobal
        {
            get { return fontGlobal; }
            set { fontGlobal = value; }
        }

        static Color glasColor = Color.Red;
        /// <summary>
        /// цвет гласных букв
        /// </summary>
        public static Color GlasColor
        {
            get { return glasColor; }
            set { glasColor = value; }
        }

        static Color soglasColor = Color.Blue;
        /// <summary>
        /// цвет согласных букв
        /// </summary>
        public static Color SoglasColor
        {
            get { return soglasColor; }
            set { soglasColor = value; }
        }

        static Color backGrountColor = Color.White;
        /// <summary>
        /// Цвет бэк-граунда
        /// </summary>
        public static Color BackGroundColor
        {
            get { return backGrountColor; }
            set { backGrountColor = value; }
        }

        static bool showAccent = true;
        /// <summary>
        /// режим вывода текста с ударением в словах
        /// </summary>
        public static bool ShowAccent
        {
            get { return showAccent; }
            set { showAccent = value; }
        }

        static bool isUpperRegister = true;
        /// <summary>
        /// Режим вывода текста в верхнем регистре
        /// </summary>
        public static bool IsUpperRegister
        {
            get { return isUpperRegister; }
            set { isUpperRegister = value; }
        }

        /// <summary>
        /// шаблон строки символа ударения
        /// </summary>
        public const string AccentStr = "\u0301";

        static HorizontalAlignment textHorizontalAlignment = HorizontalAlignment.Center;
        /// <summary>
        /// Горизонтальное выравнивание контента на экране
        /// </summary>
        public static HorizontalAlignment TextHorizontalAlignment
        {
            get { return textHorizontalAlignment; }
            set { textHorizontalAlignment = value; }
        }

        public static Slovo ExampleSlovo = new Slovo("помидор", 5, null);

        static int upperMargin = 2;
        /// <summary>
        /// отступ от верхнего края (кол-во переводов на новую строку)
        /// </summary>
        public static int UpperMargin
        {
            get { return upperMargin; }
            set { upperMargin = value; }
        }

        static int beforLineMargin = 0;
        /// <summary>
        /// междострочный интервал (кол-во переводов на новую строку)
        /// </summary>
        public static int BeforeLineMargin
        {
            get { return beforLineMargin; }
            set { beforLineMargin = value; }
        }

        static bool mixElementsEnabled = false;
        public static bool MixElementsEnabled
        {
            get { return mixElementsEnabled; }
            set { mixElementsEnabled = value; }
        }

        static ReadMode read_mode = ReadMode.Slova;
        /// <summary>
        /// Режим вывода содержимого (слова, предложения, стишки, скороговоркиб песенки)
        /// </summary>
        public static ReadMode Read_Mode
        {
            get { return read_mode; }
            set { read_mode = value; }
        }

        static int elementsCount = 1;
        /// <summary>
        /// Кол-во элементов, выводимых на экарн
        /// </summary>
        public static int ElementsCount
        {
            get { return elementsCount; }
            set { elementsCount = value; }
        }

        static int bukvFilter = 6;
        /// <summary>
        /// максимальное кол-во букв в слове (используется только при выводе одиночных слов)
        /// </summary>
        public static int BukvFilter
        {
            get { return bukvFilter; }
            set { bukvFilter = value; }
        }

        static bool isColorizerBukvEnabled = true;
        /// <summary>
        /// включена/отключена подсветка букв цветами
        /// </summary>
        public static bool IsColorizerBukvEnabled
        {
            get { return isColorizerBukvEnabled; }
            set { isColorizerBukvEnabled = value; }
        }

        static Color defaultForeColor = Color.Black;
        /// <summary>
        /// Цвет шрифта по-умолчанию
        /// </summary>
        public static Color DefaultForeColor
        {
            get { return defaultForeColor; }
            set { defaultForeColor = value; }
        }

        static SlogType slogCurrentType = SlogType.Slog2_Soglas_Glas;
        /// <summary>
        /// тип выбранного слога
        /// </summary>
        public static SlogType SlogCurrentType
        {
            get { return slogCurrentType; }
            set { slogCurrentType = value; }
        }

        static char? slogFilter = null;
        /// <summary>
        /// символьный фильтр слогов
        /// </summary>
        public static char? SlogFilter
        {
            get { return slogFilter; }
            set { slogFilter = value; }
        }

        static int countLeftIndent = 0;
        /// <summary>
        /// кол-во отступов \t с левой стороны
        /// </summary>
        public static int CountLeftIndent
        {
            get { return countLeftIndent; }
            set { countLeftIndent = value; }
        }
    }
}
