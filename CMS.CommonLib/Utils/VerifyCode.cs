using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web.UI;
using System.Drawing.Drawing2D;
using System.IO;

namespace CMS.CommonLib.Utils
{
    /// <summary>
    /// ������֤�����
    /// by sxd
    /// </summary>
    public class ValidateCode
    {
        #region ��֤�볤��(Ĭ��6����֤��ĳ���)

        int length = 6;
        public int Length
        {
            get { return length; }
            set { length = value; }
        }

        #endregion

        #region ��֤�������С(Ϊ����ʾŤ��Ч����Ĭ��40���أ����������޸�)

        int fontSize = 40;
        public int FontSize
        {
            get { return fontSize; }
            set { fontSize = value; }
        }

        #endregion

        #region �߿�(Ĭ��1����)

        int padding = 2;
        public int Padding
        {
            get { return padding; }
            set { padding = value; }
        }

        #endregion

        #region �Ƿ�������(Ĭ�ϲ����)

        bool chaos = true;
        public bool Chaos
        {
            get { return chaos; }
            set { chaos = value; }
        }

        #endregion

        #region ���������ɫ(Ĭ�ϻ�ɫ)

        Color chaosColor = Color.LightGray;
        public Color ChaosColor
        {
            get { return chaosColor; }
            set { chaosColor = value; }
        }

        #endregion

        # region �Ƿ�Ӳ����˾�Ч��
        private bool _isTwist = false;
        public bool IsTwist
        {
            get { return _isTwist; }
            set { _isTwist = value; }
        }
        # endregion

        #region �Զ��屳��ɫ(Ĭ�ϰ�ɫ)

        Color backgroundColor = Color.White;
        public Color BackgroundColor
        {
            get { return backgroundColor; }
            set { backgroundColor = value; }
        }

        #endregion

        #region �Զ��������ɫ����

        Color[] colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
        public Color[] Colors
        {
            get { return colors; }
            set { colors = value; }
        }

        #endregion

        #region �Զ�����������

        string[] fonts = { "Arial", "Georgia" };
        public string[] Fonts
        {
            get { return fonts; }
            set { fonts = value; }
        }

        #endregion

        #region �Զ���������ַ�������(ʹ�ö��ŷָ�)

        string codeSerial = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
        public string CodeSerial
        {
            get { return codeSerial; }
            set { codeSerial = value; }
        }

        #endregion

        #region ���������˾�Ч��

        private const double PI = 3.1415926535897932384626433832795;
        private const double PI2 = 6.283185307179586476925286766559;

        /// <summary>
        /// ��������WaveŤ��ͼƬ��Edit By 51aspx.com��
        /// </summary>
        /// <param name="srcBmp">ͼƬ·��</param>
        /// <param name="bXDir">���Ť����ѡ��ΪTrue</param>
        /// <param name="nMultValue">���εķ��ȱ�����Խ��Ť���ĳ̶�Խ�ߣ�һ��Ϊ3</param>
        /// <param name="dPhase">���ε���ʼ��λ��ȡֵ����[0-2*PI)</param>
        /// <returns></returns>
        public System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            // ��λͼ�������Ϊ��ɫ
            System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();

            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;

            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);

                    // ȡ�õ�ǰ�����ɫ
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    System.Drawing.Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }

            return destBmp;
        }



        #endregion

        #region ����У����ͼƬ

        public Bitmap CreateImageCode(string code,bool isBorder)
        {
            int fSize = FontSize;
            int fWidth = fSize + Padding;

            int imageWidth = (int)(code.Length * fWidth) + 4 + Padding * 2;
            int imageHeight = fSize * 2 + Padding;

            System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageWidth, imageHeight);

            Graphics g = Graphics.FromImage(image);

            g.Clear(BackgroundColor);

            Random rand = new Random();

            //���������������ɵ����
            if (this.Chaos)
            {

                Pen pen = new Pen(ChaosColor, 0);
                int c = Length * 10;

                for (int i = 0; i < c; i++)
                {
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);

                    g.DrawRectangle(pen, x, y, 1, 1);
                }
            }

            int left = 0, top = 0, top1 = 1, top2 = 1;

            int n1 = (imageHeight - FontSize - Padding * 2);
            int n2 = n1 / 4;
            top1 = n2;
            top2 = n2 * 2;

            Font f;
            Brush b;

            int cindex, findex;

            //����������ɫ����֤���ַ�
            for (int i = 0; i < code.Length; i++)
            {
                cindex = rand.Next(Colors.Length - 1);
                findex = rand.Next(Fonts.Length - 1);

                f = new System.Drawing.Font(Fonts[findex], fSize, System.Drawing.FontStyle.Bold);
                b = new System.Drawing.SolidBrush(Colors[cindex]);

                if (i % 2 == 1)
                {
                    top = top2;
                }
                else
                {
                    top = top1;
                }

                left = i * fWidth;

                g.DrawString(code.Substring(i, 1), f, b, left, top);
            }

            //��һ���߿� �߿���ɫΪColor.Gainsboro
            if (isBorder)
            {
                g.DrawRectangle(new Pen(Color.Gainsboro, 0), 0, 0, image.Width - 1, image.Height - 1);
            }
            g.Dispose();
            if (IsTwist)
            {
                //��������
                image = TwistImage(image, true, 8, 4);
            }
            return image;
        }
        #endregion

        #region �������õ�ͼƬ�����ҳ��

        public void CreateImageOnPage(string code,bool isBorder, System.Web.HttpContext context)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            Bitmap image = this.CreateImageCode(code, isBorder);
            try
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                context.Response.ClearContent();
                context.Response.ContentType = "image/Jpeg";
                context.Response.BinaryWrite(ms.GetBuffer());
            }
            finally
            {
                ms.Close();
                ms = null;
                image.Dispose();
                image = null;
            }
        }

        public byte[] CreateImageToByte(string code, bool isBorder)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            Bitmap image = this.CreateImageCode(code, isBorder);
            try
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] byteCode = ms.GetBuffer();
                return byteCode;
            }
            finally
            {
                ms.Close();
                ms = null;
                image.Dispose();
                image = null;
            }
        }

        #endregion

        # region ��������ַ���

        public string CreateVerifyCode(int codeLen)
        {
            if (codeLen == 0)
            {
                codeLen = Length;
            }

            string[] arr = CodeSerial.Split(',');
            string code = "";
            int randValue = -1;

            Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

            for (int i = 0; i < codeLen; i++)
            {
                randValue = rand.Next(0, arr.Length - 1);
                code += arr[randValue];
            }

            return code;
        }
        public string CreateVerifyCode()
        {
            return CreateVerifyCode(0);
        }

        #endregion
    }
}
