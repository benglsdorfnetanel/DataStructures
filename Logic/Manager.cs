using DS;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;

namespace Logic
{
    public delegate void ExpiredEventHendler(object sender, ExpiredEventArgs EEA);

    public class ExpiredEventArgs : EventArgs
    {
        public string result;
        public ExpiredEventArgs(string str)
        {
            result = str;
        }
    }

    public class Manager
    {
        public event ExpiredEventHendler delegateEEH;
        uint maxAmount = 50;
        uint minAmount = 30;
        uint maxAllowedExistenceSecondsAfterBuying = 5;
        private System.Timers.Timer _timer;
        public BST<DataX> BSTX_maneger { get; set; }
        public LinkdList<DataDate> lnkList = new LinkdList<DataDate>();
        public Manager()
        {
            BSTX_maneger = new BST<DataX>();
            _timer = new System.Timers.Timer();
            _timer.Interval = 3600000; // one minute - 60000, one second - 1000, one milisecond - 1
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }
        
        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // check if linkedlist or bst empty if yes return
            // get oldest value from linked list 
            // check if-- this value went over allowed time, if he did remove from both bst and linked list
            // note: do above step in while so if we have 2 (or more) values
            // that are old they will be deleted at the same time and not in next interval
            string str = UpdateTime();
            if (str != "")
            {
                delegateEEH.Invoke(this,new ExpiredEventArgs(str));
            }
        }

        public void AddBox(uint x,uint y, uint amount)
        {
            DataX dataX = new DataX(x);
            BSTX_maneger.SearchAndAdd(dataX,out DataX foundedx);
            DataY dataY = new DataY(y, 0);
            var isNewOrOld = foundedx.BSTY.GetNode(dataY);
            if (isNewOrOld == null) lnkList.AddFirst(new DataDate(x, y));
            else lnkList.MoveNodeToFirst(isNewOrOld.Date);
            foundedx.BSTY.SearchAndAdd(dataY, out DataY foundedy);
            foundedy.UpdateAmount(amount, minAmount, maxAmount);
        }
        //CreateOrder - Check if we have enough boxes
        public void CreateOrder(uint x, uint y, uint amount, out List<Box> boxesToreturn)
        {
            var boxes = new List<Box>();
            FindBestMatch();
            boxesToreturn = boxes;

            void FindBestMatch()
            {
                BSTX_maneger.FindBestMatch(new DataX(x), out var nodeX);
                if (DoesXExist() == false) return;

                nodeX.data.BSTY.FindBestMatch(new DataY(y), out var nodeY);
                FindNextX(); 
                if (DoesXExist() == false) return;

                uint amountOrdered = CalculateAmountBought(amount, nodeY.data.Amount);
                boxes.Add(new Box(nodeX.data.X, nodeY.data.Y, amountOrdered));
                amount -= amountOrdered;
                FindAllMatchingBoxes(out var boxesTmp);
                foreach (var item in boxesTmp)
                    boxes.Add(item);

                uint CalculateAmountBought(uint wanted, uint existing)
                {
                    if (Math.Abs(wanted) > existing) return existing;
                    else return (uint)Math.Abs(wanted);
                }
                bool DoesXExist() => nodeX != null;
                void FindNextX()
                {
                    while (nodeY == null)
                    {
                        BSTX_maneger.FindNextBestMatch(nodeX, out nodeX);
                        if (nodeX == null) break;
                        nodeX.data.BSTY.FindBestMatch(new DataY(y), out nodeY);
                    }
                }
                void FindAllMatchingBoxes(out List<Box> boxesInner)
                {
                    boxesInner = new List<Box>();
                    while (amount != 0)
                    {
                        nodeX.data.BSTY.FindNextBestMatch(nodeY, out nodeY);
                        FindNextX();
                        if (DoesXExist() == false) return;
                        amountOrdered = CalculateAmountBought(amount, nodeY.data.Amount);
                        boxesInner.Add(new Box(nodeX.data.X, nodeY.data.Y, amountOrdered));
                        amount -= amountOrdered;
                    }
                }
            }
        }
        //BuyBox - give to customer the box
        public void BuyBox(List<Box> boxes)
        {
            if (boxes.Count == 0) return;
            for (int i = 0; i < boxes.Count - 1; i++)
            {
                var x = BSTX_maneger.GetNode(new DataX(boxes.ElementAt(i).X));
                var tmpY = new DataY(boxes.ElementAt(i).Y);
                lnkList.RemoveNode(tmpY.Date);
                x.BSTY.Delete(tmpY);
                if (x.BSTY.IsEmpty() == true) BSTX_maneger.Delete(x);
            }
            var lastBox = boxes.Last();
            var xBox = BSTX_maneger.GetNode(new DataX(lastBox.X));
            var yBox = xBox.BSTY.GetNode(new DataY(lastBox.Y));
            if(yBox.Amount == lastBox.Amount)
            {
                lnkList.RemoveNode(yBox.Date);
                xBox.BSTY.Delete(yBox);
                if (xBox.BSTY.IsEmpty() == true) BSTX_maneger.Delete(xBox);
            }
            else
            {
                lnkList.MoveNodeToFirst(yBox.Date);
                yBox.UpdateAmount(lastBox.Amount, minAmount, maxAmount, false);
            }
        }
        public bool CheckInput(string text, out uint num)
        {
            num = default;
            try
            {
                uint output = Convert.ToUInt32(text);
                num = output;
                return true;
            }
            catch
            {
                return false;
            }
        }
        //UpdateTime - check if we dont have box in the warehouse more than 30 days
        public string UpdateTime()
        {
            string result = "";
            DataDate tmp = lnkList.last.data;
            while (tmp != null)
            {
                DateTime d = tmp.buyDateTime.AddDays(30);
                if (d < DateTime.Now)
                {
                    lnkList.RemoveTheLast(out tmp);
                    DataX dataX = new DataX(tmp._x);
                    DataY dataY = new DataY(tmp._y);
                    BSTX_maneger.FindBestMatch(dataX, out var nodeX);
                    nodeX.data.BSTY.Delete(dataY);
                    if (nodeX.data.BSTY.IsEmpty())
                    {
                        BSTX_maneger.Delete(dataX);
                    }
                    result += $"{tmp._x} and {tmp._y} was deleted";
                }
                else break;
            }
            return result;
        }
        //ShowBoxes - make sure that we have all the boxes that the coustmer asks
        public string ShowBoxes(uint x, uint y)
        {
            DataX newX = new DataX(x);
            DataY newY = new DataY(y);
            BSTX_maneger.FindBestMatch(newX,out var node);
            if (node is null)
            {
                return "We cant find that box";
            }
            else
            {
                node.data.BSTY.FindBestMatch(newY, out var nodeY);
                if (nodeY is null)
                {
                    return "We cant find that box(Y)";
                }
                else
                {
                    return "we found the box";
                }
            }
        }
    }
}
