using System;
using System.Collections.Generic;
using System.Text;
using DS;

namespace Models
{
    public class DataY : IComparable<DataY>
    {
        public uint Y { get; set; }
        public uint Amount { get; set; }
        public LinkdList<DataDate>.Node Date { get; set; }
        public DataY(uint y,uint amount)
        {
            Y = y;
            Amount = amount;
        }
        public DataY(uint y)
        {
            Y = y;
        }

        public int CompareTo(DataY other)
        {
            return Y.CompareTo(other.Y);
        }
        public void UpdateAmount(uint amount,uint min, uint max, bool add = true)
        {
            if (add == true) Amount += amount;
            else Amount -= amount;

            if (Amount > max) Amount = max;
            if (Amount < 0) Amount = 0;
        }
    }
}
