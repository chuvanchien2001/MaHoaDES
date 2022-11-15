using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaHoaDES.DoiTuong
{
    public  class ChiaSeKhoa
    {
        long[] dsX = new long[100];
        long[] dsA = new long[100];
        public List<long> dA = new List<long>();
       List<long> dsK = new List<long>();

        long mul_inv(long a, long b)
        {
            long x1 = 1, x0 = 0;
            long b0 = b, t, q;
            if (b == 1) return 1;
            while (a < 0)
            {
                a += b;
            }
            while (a > 1)
            {
                q = (long)a / b;
                t = b;
                b = a % b;
                a = t;
                t = x0;
                x0 = x1 - q * x0;
                x1 = t;
            }
            if (x1 < 0)
            {
                x1 += b0;
            }
            return x1;
        }

        public List<long> chiakhoa(int k, int p, int m, int t)
        {
            for (int i = 1; i <= m; i++)
            {
                dsX[i] = i;
            }
            int rd = 1;
            for (int i = 1; i < t; i++)
            {
                dsA[i] = rd;
                dA.Add((long)rd);
                rd++;
            }

            for (int i = 1; i <= m; i++)
            {
                long l = 0;
                for (int j = t - 1; j > 0; j--)
                {
                    long h = 1;
                    for (int id = 1; id <= j; id++)
                    {
                        h *= dsX[i];
                    }
                    l = l + (dsA[j] * h) % p;
                }
                long y = (k + l) % p;
                dsK.Add(y);
            }

            return dsK;
        }

        public long khoiphuckhoa(List<DuocChon> ds, int p)
        {
            long t = ds.Count();
            long k = 0;
            long[] x = new long[100], g = new long[100];
            for (int i = 1; i <= t; i++)
            {
                x[i] = ds[i - 1].getXi();
                g[i] = ds[i - 1].getPi();
            }

            for (int l = 1; l <= t; l++)
            {
                long m = 1;
                for (int j = 1; j <= t; j++)
                {
                    if (j != l)
                    {
                        long b = x[j] - x[l];
                        long n = (x[j] % p * mul_inv(b, p) % p) % p;
                        m = (m % p * n % p) % p;
                    }
                }
                k = (k + (g[l] * m) % p) % p;
            }
            return k;
        }

    }
}
