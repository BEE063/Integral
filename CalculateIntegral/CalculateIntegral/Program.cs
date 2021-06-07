using System;
using System.Threading;

namespace CalculateIntegral
{
    class Program
    {
        public delegate double Function(double x);
        static void Main(string[] args)
        {
            double a = 1;
            double b = 2;

            var integral = new Integral( f, a, b, 0.001);

            integral.StartCalculate();

            Thread.Sleep(3000);
            Console.WriteLine($"Результат: {integral.result}");

            
            int n = 1000;
            double[] y = new double[n];
            double h = (b - a) / (n - 1);
            for (int i = 0; i < n; i++)
            {
                double x = i * h;
                y[i] = Math.Cos(x);
            }
            var result = Trapezoidal(f, a, b, n);
            Console.WriteLine("Результат: {0}", result);

            Console.ReadKey();
        }
        static double f(double x)
        {
            return Math.Cos(x);
        }
        public static double Trapezoidal(Function f, double a, double b, int n)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            double sum = 0;
            double h = (b - a) / n;
            for (int i = 0; i < n; i++)
            {
                sum += 0.5 * h * (f(a + i * h) + f(a + (i + 1) * h));
            }
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Время вычисления в однопоточном режиме: " + elapsedMs);
            return sum;
        }
    }
}

