using DS;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Logic
{
    public class Manager
    {
        uint maxAmount = 50;
        uint minAmount = 30;

        public BST<DataX> BSTX_maneger { get; set; }
        public Manager() => BSTX_maneger = new BST<DataX>();
        public LinkdList<Box> lnkList = new LinkdList<Box>();
        public void AddBox(uint x,uint y, uint amount)
        {
            DataX dataX = new DataX(x);
            BSTX_maneger.SearchAndAdd(dataX,out DataX foundedx);
            DataY dataY = new DataY(y,maxAmount);
            foundedx.BSTY.SearchAndAdd(dataY, out DataY foundedy);
            foundedy.UpdateAmount(amount, minAmount, maxAmount);
        }
        public void CreateOrder(uint x, uint y, uint amount, out List<Box> boxesToreturn)
        {
            var boxes = new List<Box>();
            FindBestMatch();
            boxesToreturn = boxes;

            void FindBestMatch()
            {
                // finds matching square
                BSTX_maneger.FindBestMatch(new DataX(x), out var nodeX);
                if (DoesXExist() == false) return;

                // finds matching height, in case our x did not have one move to next x
                nodeX.data.BSTY.FindBestMatch(new DataY(y), out var nodeY);
                FindNextX(); // incase nodey is null it will keep going to next x and
                if (DoesXExist() == false) return;

                // buying operation
                uint amountOrdered = CalculateAmountBought(amount, nodeY.data.Amount);
                boxes.Add(new Box(nodeX.data.X, nodeY.data.Y, amountOrdered));
                //nodeY.data.UpdateAmount(amount);

                // inform about found box
                amount += amountOrdered;

                // as long as your asked amount is not filled try to find better matches 
                FindAllMatchingBoxes(out var boxesTmp);
                foreach (var item in boxesTmp)
                    boxes.Add(item);

                uint CalculateAmountBought(uint wanted, uint existing)
                {
                    if (Math.Abs(wanted) > existing) return existing;
                    else return (uint)Math.Abs(wanted);
                }
                bool DoesXExist() => nodeX != null; // incase x null return false || x val return true
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
                        // finds next matching y, in case our x does not have one move to next x
                        nodeX.data.BSTY.FindNextBestMatch(nodeY, out nodeY);
                        FindNextX();
                        if (DoesXExist() == false) return;

                        amountOrdered = CalculateAmountBought(amount, nodeY.data.Amount);

                        //buying operation
                        boxesInner.Add(new Box(nodeX.data.X, nodeY.data.Y, amountOrdered));
                        //nodeY.data.UpdateAmount(amount);

                        amount += amountOrdered;
                    }
                }
            }
        }
        public void BuyBox(List<Box> boxes)
        {
            if (boxes.Count == 0) return;
            for (int i = 0; i < boxes.Count - 2; i++)
            {
                var x = BSTX_maneger.GetNode(new DataX(boxes.ElementAt(i).X));
                x.BSTY.Remove(new DataY(boxes.ElementAt(i).Y));
                if (x.BSTY.IsEmpty() == true) BSTX_maneger.Remove(x);
                // remove from linked list
            }
            var lastBox = boxes.Last();
            var xBox = BSTX_maneger.GetNode(new DataX(lastBox.X));
            var yBox = xBox.BSTY.GetNode(new DataY(lastBox.Y));
            if(yBox.Amount == lastBox.Amount)
            {
                xBox.BSTY.Remove(yBox);
                if (xBox.BSTY.IsEmpty() == true) BSTX_maneger.Remove(xBox);
                // remove from linked list
            }
            else
            {
                // update date
                //yBox.UpdateToNow();
                yBox.UpdateAmount(lastBox.Amount, minAmount, maxAmount);
            }
        }

    }
}
