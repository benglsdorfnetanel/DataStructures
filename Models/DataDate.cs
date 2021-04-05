using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class DataDate
    {
        public uint _x;
        public uint _y;
        public DateTime buyDateTime;
        public DataDate(uint x, uint y)
        {
            _x = x;
            _y = y;
            buyDateTime = DateTime.Now;
        }
        public void UpdateToNow() => buyDateTime = DateTime.Now;
    }
}
