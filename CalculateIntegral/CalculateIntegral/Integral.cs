using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace CalculateIntegral
{
    class Integral
    {
        private Func<double, double> func;

        private double _leftPoint;
        private double _rightPoint;
        private double _eps;
        private Semaphore _semaphore;

        public double result { get; set; }

        public Integral(Func<double, double> f, double a, double b, double e)
        {
            func = f;
            _semaphore = new Semaphore(8, 8);
            _leftPoint = a;
            _rightPoint = b;
            _eps = e;
        }

        private void CalculateIntegral(object args)
        {
            _semaphore.WaitOne();

            var left = (double)((object[])args)[0];
            var right = (double)((object[])args)[1];

            var length = right - left;

            var h = length / 2;
            var middle = left + h;

            if (length < _eps)
            {
                result += func(middle) * length;
            }
            else
            {
                var threadLeft = new Thread(CalculateIntegral);
                threadLeft.Start(new object[] { left, middle });

                var threadRight = new Thread(CalculateIntegral);
                threadRight.Start(new object[] { middle, right });
            }

            _semaphore.Release();
        }
        public void StartCalculate()
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var thread = new Thread(CalculateIntegral);
            thread.Start(new object[] { _leftPoint, _rightPoint });
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Время вычисления в многопоточном режиме: " + elapsedMs);
        }
    }
}
