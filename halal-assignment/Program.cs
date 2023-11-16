using polygon_problem_hill_climbing;
using hotspot_search_dbscan;
using image_segmentation_k_means;

Console.WriteLine("Smallest Boundary Polygon Problem with Hill Climbing algorithm (Stochastic):\n");

SmallestBoundaryPolygonProblem sbpp = new SmallestBoundaryPolygonProblem(200.0, 15.0, 0.00001f, 1000);
sbpp.RunHC_Stachostic("points.txt");

Console.WriteLine("\n\n");
Console.WriteLine("Image Segmentation with K-Means algorithm: \n\n");


ImageSegmentation img = new ImageSegmentation();
img.LoadImageFromFile("crazy.png");
Console.WriteLine("Image loaded..");
KMeansSegmentation kmeans = new KMeansSegmentation(4);
Console.WriteLine("Clustering in process..");
kmeans.RunKMeans(img);
img.SavePixClusterToFile("crazy.png");
Console.WriteLine("Image done!");

Console.WriteLine("\n\n");
Console.WriteLine("Hotspot Searching with DBSCAN algorithm: \n");

HotspotSearch hssDbscan = new HotspotSearch(7, 3);
hssDbscan.DoHotspotSearch("hspoints.txt");