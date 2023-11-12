using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace image_segmentation_k_means
{
    public class KMeansSegmentation
    {
        private int k;
        private Random random;

        public KMeansSegmentation(int numClusters)
        {
            k = numClusters;
            random = new Random();
        }

        public void RunKMeans(ImageSegmentation imageSegmentation)
        {

            List<Color> clusterCenters = InitializeRandomClusterCenters(imageSegmentation.rawColor, k);

            List<List<Color>> clusters = new List<List<Color>>();

            bool converged = false;
            while (!converged)
            {

                clusters = AssignPixelsToClusters(clusterCenters, imageSegmentation.rawColor);


                List<Color> newClusterCenters = UpdateClusterCenters(clusters);


                converged = CheckConvergence(clusterCenters, newClusterCenters);

                clusterCenters = newClusterCenters;
            }


            ApplyClusterColors(clusters, imageSegmentation);
        }

        private List<Color> InitializeRandomClusterCenters(byte[] rawColor, int numClusters)
        {

            List<Color> pixels = ConvertBytesToColors(rawColor);
            List<Color> randomCenters = pixels.OrderBy(_ => random.Next()).Take(numClusters).ToList();

            return randomCenters;
        }

        private List<Color> ConvertBytesToColors(byte[] rawColor)
        {
            List<Color> pixels = new List<Color>();

            for (int i = 0; i < rawColor.Length; i += 3)
            {
                Color pixel = Color.FromArgb(rawColor[i], rawColor[i + 1], rawColor[i + 2]);
                pixels.Add(pixel);
            }

            return pixels;
        }

        private List<List<Color>> AssignPixelsToClusters(List<Color> clusterCenters, byte[] rawColor)
        {

            List<List<Color>> clusters = new List<List<Color>>(k);

            for (int i = 0; i < k; i++)
            {
                clusters.Add(new List<Color>());
            }

            List<Color> pixels = ConvertBytesToColors(rawColor);

            foreach (var pixel in pixels)
            {
                int nearestCluster = FindNearestCluster(pixel, clusterCenters);
                clusters[nearestCluster].Add(pixel);
            }

            return clusters;
        }

        private int FindNearestCluster(Color pixel, List<Color> clusterCenters)
        {

            double minDistance = double.MaxValue;
            int nearestCluster = 0;

            for (int i = 0; i < clusterCenters.Count; i++)
            {
                double distance = CalculateEuclideanDistance(pixel, clusterCenters[i]);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCluster = i;
                }
            }

            return nearestCluster;
        }

        private double CalculateEuclideanDistance(Color a, Color b)
        {
            double dR = a.R - b.R;
            double dG = a.G - b.G;
            double dB = a.B - b.B;

            return Math.Sqrt(dR * dR + dG * dG + dB * dB);
        }

        private List<Color> UpdateClusterCenters(List<List<Color>> clusters)
        {

            List<Color> newCenters = new List<Color>();

            foreach (var cluster in clusters)
            {
                if (cluster.Count == 0)
                {

                    newCenters.Add(Color.Black);
                }
                else
                {
                    int sumR = 0, sumG = 0, sumB = 0;

                    foreach (var pixel in cluster)
                    {
                        sumR += pixel.R;
                        sumG += pixel.G;
                        sumB += pixel.B;
                    }

                    int meanR = sumR / cluster.Count;
                    int meanG = sumG / cluster.Count;
                    int meanB = sumB / cluster.Count;

                    newCenters.Add(Color.FromArgb(meanR, meanG, meanB));
                }
            }

            return newCenters;
        }

        private bool CheckConvergence(List<Color> oldCenters, List<Color> newCenters)
        {

            double tolerance = 1.0;

            for (int i = 0; i < oldCenters.Count; i++)
            {
                double distance = CalculateEuclideanDistance(oldCenters[i], newCenters[i]);
                if (distance > tolerance)
                {
                    return false;
                }
            }

            return true;
        }

        private void ApplyClusterColors(List<List<Color>> clusters, ImageSegmentation imageSegmentation)
        {

            int width = imageSegmentation.width;
            int height = imageSegmentation.height;
            int index = 0;

            for (int i = 0; i < width * height; i++)
            {
                int clusterIndex = FindClusterIndex(clusters, imageSegmentation.rawColor, i);
                imageSegmentation.SetPixCluster(index % width, index / width, (uint)clusterIndex);
                index++;
            }
        }

        private int FindClusterIndex(List<List<Color>> clusters, byte[] rawColor, int pixelIndex)
        {
            Color pixel = ConvertBytesToColors(rawColor)[pixelIndex];

            for (int i = 0; i < clusters.Count; i++)
            {
                if (clusters[i].Contains(pixel))
                {
                    return i;
                }
            }

            return 0;
        }

    }
}
