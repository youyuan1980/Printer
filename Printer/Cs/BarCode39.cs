using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Collections;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing.Text;
namespace Printer.Cs
{

    public class BarCode39
    {
        public enum AlignType
        {
            Left, Center, Right
        }

        public enum BarCodeWeight
        {
            Small = 1, Medium, Large
        }

        private AlignType align = AlignType.Center;
        private String code = "";
        private int leftMargin = 10;
        private int topMargin = 10;
        private int Height = 40;
        private int Width = 230;
        private bool showHeader;
        private bool showFooter = true;
        private String headerText = "";
        private BarCodeWeight weight = BarCodeWeight.Small;
        private Font headerFont = new Font("宋体", 18);
        private Font footerFont = new Font("宋体", 12);


        public AlignType VertAlign
        {
            get { return align; }
            set { align = value; }
        }

        public String BarCode
        {
            get { return code; }
            set { code = value.ToUpper(); }
        }

        public int BarCodeHeight
        {
            get { return Height; }
            set { Height = value; }
        }

        public int BarCodeWidth
        {
            get { return Width; }
            set { Width = value; }
        }

        public int LeftMargin
        {
            get { return leftMargin; }
            set { leftMargin = value; }
        }

        public int TopMargin
        {
            get { return topMargin; }
            set { topMargin = value; }
        }

        public bool ShowHeader
        {
            get { return showHeader; }
            set { showHeader = value; }
        }

        public bool ShowFooter
        {
            get { return showFooter; }
            set { showFooter = value; }
        }

        public String HeaderText
        {
            get { return headerText; }
            set { headerText = value; }
        }

        public BarCodeWeight Weight
        {
            get { return weight; }
            set { weight = value; }
        }

        public Font HeaderFont
        {
            get { return headerFont; }
            set { headerFont = value; }
        }

        public Font FooterFont
        {
            get { return footerFont; }
            set { footerFont = value; }
        }


        String alphabet39 = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ-. $/+%*";

        String[] coded39Char = 
		{
			/* 0 */ "000110100", 
			/* 1 */ "100100001", 
			/* 2 */ "001100001", 
			/* 3 */ "101100000",
			/* 4 */ "000110001", 
			/* 5 */ "100110000", 
			/* 6 */ "001110000", 
			/* 7 */ "000100101",
			/* 8 */ "100100100", 
			/* 9 */ "001100100", 
			/* A */ "100001001", 
			/* B */ "001001001",
			/* C */ "101001000", 
			/* D */ "000011001", 
			/* E */ "100011000", 
			/* F */ "001011000",
			/* G */ "000001101", 
			/* H */ "100001100", 
			/* I */ "001001100", 
			/* J */ "000011100",
			/* K */ "100000011", 
			/* L */ "001000011", 
			/* M */ "101000010", 
			/* N */ "000010011",
			/* O */ "100010010", 
			/* P */ "001010010", 
			/* Q */ "000000111", 
			/* R */ "100000110",
			/* S */ "001000110", 
			/* T */ "000010110", 
			/* U */ "110000001", 
			/* V */ "011000001",
			/* W */ "111000000", 
			/* X */ "010010001", 
			/* Y */ "110010000", 
			/* Z */ "011010000",
			/* - */ "010000101", 
			/* . */ "110000100", 
			/*' '*/ "011000100",
			/* $ */ "010101000",
			/* / */ "010100010", 
			/* + */ "010001010", 
			/* % */ "000101010", 
			/* * */ "010010100" 
		};

        public Bitmap getBarCode()
        {

            String intercharacterGap = "0";
            String str = '*' + code.ToUpper() + '*';
            int strLength = str.Length;

            for (int i = 0; i < code.Length; i++)
            {
                if (alphabet39.IndexOf(code[i]) == -1 || code[i] == '*')
                {
                    //g.DrawString("INVALID BAR CODE TEXT", footerFont, Brushes.Red, 10, 10);
                    return null;
                }
            }

            String encodedString = "";

            for (int i = 0; i < strLength; i++)
            {
                if (i > 0)
                    encodedString += intercharacterGap;

                encodedString += coded39Char[alphabet39.IndexOf(str[i])];
            }

            int encodedStringLength = encodedString.Length;
            int widthOfBarCodeString = 0;
            double wideToNarrowRatio = 2;


            if (align != AlignType.Left)
            {
                for (int i = 0; i < encodedStringLength; i++)
                {
                    if (encodedString[i] == '1')
                        widthOfBarCodeString += (int)(wideToNarrowRatio * (int)weight);
                    else
                        widthOfBarCodeString += (int)weight;
                }
            }
            Width = widthOfBarCodeString + leftMargin * 2;
            Bitmap bmp = new Bitmap(Width, 80, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            Graphics g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.White, 0, 0, Width, 80);
            g.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
            //g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            int x = 0;
            int wid = 0;
            int yTop = 0;
            SizeF hSize = g.MeasureString(headerText, headerFont);
            SizeF fSize = g.MeasureString(code, footerFont);

            int headerX = 0;
            int footerX = 0;

            if (align == AlignType.Left)
            {
                x = leftMargin;
                headerX = leftMargin;
                footerX = leftMargin;
            }
            else if (align == AlignType.Center)
            {
                x = (Width - widthOfBarCodeString) / 2;
                headerX = (Width - (int)hSize.Width) / 2;
                //footerX = (Width - (int)fSize.Width) / 2;
                footerX = x;
            }
            else
            {
                x = Width - widthOfBarCodeString - leftMargin;
                headerX = Width - (int)hSize.Width - leftMargin;
                //footerX = Width - (int)fSize.Width - leftMargin;
                footerX = x;
            }

            if (showHeader)
            {
                yTop = (int)hSize.Height + topMargin;
                g.DrawString(headerText, headerFont, Brushes.Black, headerX, topMargin);
            }
            else
            {
                yTop = topMargin;
            }

            for (int i = 0; i < encodedStringLength; i++)
            {
                if (encodedString[i] == '1')
                    wid = (int)(wideToNarrowRatio * (int)weight);
                else
                    wid = (int)weight;

                g.FillRectangle(i % 2 == 0 ? Brushes.Black : Brushes.White, x, yTop, wid, Height);

                x += wid;
            }

            yTop += Height;


            if (showFooter)
            {
                int p = (widthOfBarCodeString - (int)fSize.Width) / str.Length;
                int f = (int)fSize.Width / str.Length;
                for (int i = 0; i < str.Length; i++)
                {
                    footerX += p / 2;
                    g.DrawString(Convert.ToString(str[i]), footerFont, Brushes.Black, footerX, yTop);
                    footerX += f + p / 2;
                }
            }
            g.Dispose();
            return bmp;
        }
    }
}