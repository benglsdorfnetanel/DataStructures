using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Box
    {
        public uint X;
        public uint Y;
        public uint Amount;


        public Box(uint x, uint y)
        {
            X = x;
            Y = y;
        }
        public Box(uint x, uint y, uint amount)
        {
            X = x;
            Y = y;
            Amount = amount;
        }
    }
}
