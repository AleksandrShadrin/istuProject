using Methods;


double[] a =  new double[1] {1};

foreach (var i in a[1..^0])
{
    Console.WriteLine(i);
}
