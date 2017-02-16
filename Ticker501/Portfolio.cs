using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    class Portfolio
    {
        public string pName;
        public static string[] sNames = new string[25];
        public int[] quantity = new int[25];
        //iWorth is the initial prices spent on all stocks
        public double iWorth;
        //tracker is what determines loss/gains
        public double tracker;
        //worth of a specific portfolio

        //private double initialTotal = 0;
        //private double finalTotal = 0;

        //portfolio constructor
        public Portfolio(string name)
        {
            pName = name;

            //all new portfolios' quantity of stocks are reset to 0
            for (int q = 0; q < 25; q++)
            {
                quantity[q] = 0;
            }
            tracker = 0;
        }

        public double getTracker()
        {
            return tracker;
        }

        //returning true equals a gain
        //returning false equals a loss
        public bool checkToss()
        {
            if (tracker > 1)
                return true;
            else
                return false;
        }


        //handles methods for buying stocks
        public void buyStock(int pos, int amount, double price)
        {
            subTracker(amount, price);
            addStockQuantity(pos, amount);
            iWorth += (amount * price);
        }

        //handles methods for sell stocks
        public void sellStock(int pos, int amount, double price)
        {
            addTracker(amount, price);
            subStockQuantity(pos, amount);

        }
        //adds the amount of stock in the position of the stock
        public void addStockQuantity(int pos, int amount)
        {
            quantity[pos] += amount;

        }
        //substracts the amount in the position of the stock
        public void subStockQuantity(int pos, int amount)
        {
            if (quantity[pos] >= amount)
                quantity[pos] -= amount;
            else
                Console.WriteLine("You do not have enough stocks of that type to sell at the moment.");
        }

        //add and substract method's to keep track of gain/loss
        public void addTracker(int amount, double price)
        {
            tracker += (price * amount);
        }
        //add and substract method's to keep track of gain/loss
        public void subTracker(int amount, double price)
        {
            tracker -= (price * amount);
        }














    }//class
}//namespace
