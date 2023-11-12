using image_segmentation_k_means;

ImageSegmentation img = new ImageSegmentation();

img.LoadImageFromFile("crazy.png");

KMeansSegmentation kmeans = new KMeansSegmentation(4);

kmeans.RunKMeans(img);

img.SavePixClusterToFile("crazy.png");