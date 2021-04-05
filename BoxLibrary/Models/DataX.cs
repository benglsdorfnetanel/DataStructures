using DS;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace Models
{
    public class DataX: IComparable<DataX>
    {
        public uint X { get; set; }
        public BST<DataY> BSTY { get; set; }
        public DataX(uint x)
        {
            X = x;
            BSTY = new BST<DataY>();
        }
        public int CompareTo(DataX other)
        {
            return X.CompareTo(other.X);
        }

        public void AddY(DataY y, uint maxAmount, uint minAmount)
        {
            BSTY.Add(y);
            var tmp = BSTY.GetData(y);
            tmp.UpdateAmount(y.Amount, minAmount, maxAmount);
        }
    }
}
