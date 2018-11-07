using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.DrawingCore;
using System.DrawingCore.Imaging;

namespace Bzway.Common.Utility
{
    public class ValidateCodeGenerator
    {
        private static Random random = new Random();
        private static string[] allCharArray = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F,G,H,J,K,L,M,N,P,Q,R,S,T,U,W,X,Y,Z".Split(',');
        public static MemoryStream CreateImage(string checkCode)
        {
            int iwidth = (int)((checkCode.Length + 2) * 10);
            using (Bitmap image = new Bitmap(iwidth, 20))
            {
                using (Graphics g = Graphics.FromImage(image))
                {
                    Font font = new Font("Arial", 10, FontStyle.Bold);
                    Brush brush = new SolidBrush(Color.White);

                    g.Clear(Color.Gray);
                    g.DrawString(checkCode, font, brush, 3, 2);
                    MemoryStream ms = new MemoryStream();
                    image.Save(ms, ImageFormat.Jpeg);
                    font.Dispose();
                    brush.Dispose();
                    return ms;
                }
            }
        }

        public static string CreateRandomCode(int codeCount, bool IsDigital)
        {
            var maxIndex = IsDigital ? 9 : allCharArray.Length - 1;
            StringBuilder builder = new StringBuilder(codeCount);
            for (int i = 0; i < codeCount; i++)
            {

                int t = random.Next(maxIndex);
                builder.Append(allCharArray[t]);
            }
            return builder.ToString();
        }

        public static byte[] NextBytes(int length)
        {
            byte[] buffer = new byte[length];
            random.NextBytes(buffer);
            return buffer;
        }
    }
}