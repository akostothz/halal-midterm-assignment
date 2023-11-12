using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml.Schema;

namespace image_segmentation_k_means
{
    public class ImageSegmentation
    {
        public int width, height;
        public byte[] rawColor;
        private byte[] rawGrayscale;
        private byte[] gsHistogram;
        private uint[] pixCluster;

        public ImageSegmentation()
        {
        }

        public void LoadImageFromFile(string filename)
        {
            using (Bitmap bitmap = new Bitmap(filename))
            {
                width = bitmap.Width;
                height = bitmap.Height;

                rawColor = new byte[width * height * 3];
                rawGrayscale = new byte[width * height];
                pixCluster = new uint[width * height];

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color pixelColor = bitmap.GetPixel(x, y);
                        int index = (y * width + x) * 3;

                        rawColor[index] = pixelColor.R;
                        rawColor[index + 1] = pixelColor.G;
                        rawColor[index + 2] = pixelColor.B;

                        rawGrayscale[y * width + x] = (byte)((pixelColor.R + pixelColor.G + pixelColor.B) / 3);
                        
                    }
                }
            }

            // Create histogram
            gsHistogram = new byte[256];
            for (int i = 0; i < 255; i++)
                gsHistogram[i] = 0;
            for (int i = 0; i < width * height; i++)
                gsHistogram[rawGrayscale[i]]++;



        }

        public void SavePixClusterToFile(string filename)
        {
            Bitmap bitmap = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;
                    uint clusterValue = pixCluster[index];

                    /*
                     1. megoldas
                     byte alpha = (byte)((clusterValue & 0xFF000000) >> 24);
                    byte red = (byte)((clusterValue & 0x00FF0000) >> 16);
                    byte green = (byte)((clusterValue & 0x0000FF00) >> 8);
                    byte blue = (byte)(clusterValue & 0x000000FF);

                    Color pixelColor = Color.FromArgb(alpha, red, green, blue);
                     
                     */

                    /*
                     2. megoldas
                    Color pixelColor = Color.FromArgb((int)clusterValue);
                     
                     */

                    Color pixelColor;
                    switch (clusterValue)
                    {
                        case 0:
                            pixelColor = Color.Red;
                            break;
                        case 1:
                            pixelColor = Color.Green;
                            break;
                        case 2:
                            pixelColor = Color.Blue;
                            break;
                        case 3:
                            pixelColor = Color.Magenta;
                            break;
                        default:
                            pixelColor = Color.Black;
                            break;
                    }


                    Console.WriteLine("x: " + x + ";\t y: " + y + ";\t COLOR: " + pixelColor + ";\t clusterValue: " + clusterValue);
                    bitmap.SetPixel(x, y, pixelColor);
                }
            }

            using (FileStream stream2 = new FileStream(filename, FileMode.Create))
            {
                bitmap.Save(stream2, ImageFormat.Png);
            }
        }

        public void SetPixCluster(int x, int y, uint clusterIndex)
        {
            Console.WriteLine("SetPixCluster: " + "x: " + x + ";\t y: " + y + ";\t clusterIndex: " + clusterIndex);
            int index = y * width + x;
            pixCluster[index] = clusterIndex;
        }
    }
}
