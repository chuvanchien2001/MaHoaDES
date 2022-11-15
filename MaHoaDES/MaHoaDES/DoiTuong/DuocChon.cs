using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaHoaDES.DoiTuong
{
    public class DuocChon
    {
        int xi;
        long Pi;

        public DuocChon()
        {
        }

        public int getXi()
        {
            return xi;
        }

        public long getPi()
        {
            return Pi;
        }

        public void setXi(int xi)
        {
            this.xi = xi;
        }

        public void setPi(long Pi)
        {
            this.Pi = Pi;
        }

        public DuocChon(int xi,long Pi)
        {
            this.xi = xi;
            this.Pi = Pi;
        }
    }
}
