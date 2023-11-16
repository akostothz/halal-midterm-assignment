using polygon_problem_hill_climbing;
using hotspot_search_dbscan;
using image_segmentation_k_means;

SmallestBoundaryPolygonProblem sbpp = new SmallestBoundaryPolygonProblem(200.0, 15.0, 0.00001f, 1000);
sbpp.RunHC_Stachostic("points.txt");

HotspotSearch hssDbscan = new HotspotSearch(7, 3);
hssDbscan.DoHotspotSearch("points.txt");

ImageSegmentation img = new ImageSegmentation();
img.LoadImageFromFile("crazy.png");
KMeansSegmentation kmeans = new KMeansSegmentation(4);
kmeans.RunKMeans(img);
img.SavePixClusterToFile("crazy.png");