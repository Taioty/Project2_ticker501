using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    //Class will control the price updates
    class StockMarket
    {
        public static string[] names = new string[25];
        public static string[] titles = new string[25];
        public static double[] prices = new double[25];
        public double updateRate;

        //constructor for the Stock Market
        public StockMarket(string[] _name, string[] _title, double[] _price)
        {
            names = _name;
            titles = _title;
            prices = _price;



        }


        //putting in the Stock Market classs (entiredy)
        public void myStock(int[] quantity, double worth)
        {
            double pWorth = worth;
            Console.WriteLine("Here are your currently owned stocks: ");
            for (int i = 0; i < 25; i++)
            {
                //skips the stocks that they don't own

                if (quantity[i] != 0)
                {

                    double var = Math.Round(quantity[i] * prices[i], 2, MidpointRounding.AwayFromZero);
                    double percent = Math.Round(((var / pWorth) * 100), 2, MidpointRounding.AwayFromZero);
                    //get price returned from stock market, you pass quantity to it
                    Console.WriteLine("You own " + quantity[i] + " shares in " + names[i]
                        + ", worth $" + var + " or " + percent + "% worth of your total portfolio.");

                }//if statement
            }//for loop
        }

        public double getStockWorth(int quantity, int pos)
        {
            return quantity * prices[pos];
        }

        public double setPortfolioWorth(int[] quantity)
        {
            double worth = 0;
            for (int i = 0; i < quantity.Length; i++)
            {
                if (quantity[i] != 0)
                {
                    worth += (quantity[i] * prices[i]);
                }

                if (i == 25)
                    break;

            }
            return worth;
        }

        public double getPrice(int pos)
        {
            return prices[pos];
        }
        public void printStock()
        {
            for (int i = 0; i < 25; i++)
            {
                prices[i] = Math.Round(prices[i], 2, MidpointRounding.AwayFromZero);
                Console.WriteLine((i + 1) + ")  " + titles[i] + " " + prices[i]);
            }
        }

        public double sellAllStock(int[] quantity)
        {
            double cashReturned = 0;

            for (int n = 0; n < quantity.Length; n++)
            {
                if (quantity[n] == 0)
                {
                    n++;
                    if (n == 25)
                        break;
                }
                else
                    cashReturned += (quantity[n] * prices[n]);
            }
            return cashReturned;
        }

        //searches for the position of the name of the stock
        public int searchPos(string name)
        {
            int counter = 0;
            for (int i = 0; i < 25; i++)
            {
                if (name.Equals(names[i]))
                    return counter;
                else
                    counter++;
            }
            return counter;
        }

        public double[] getUpdatedPrices()
        {
            return prices;
        }

        public double[] changePrice(double rate)
        {

            return prices;
        }



        public double[] determineRate(int vol)
        {
            //determining random percentages
            Random random = new Random();
            double irate;

            if (vol == 3)
            {
                int max = 15;
                int min = 3;
                irate = Double.Parse((random.Next(min, max)).ToString());
            }
            else if (vol == 2)
            {
                int max = 8;
                int min = 2;
                irate = Double.Parse((random.Next(min, max)).ToString());
            }
            else
            {
                int max = 4;
                int min = 1;
                irate = Double.Parse((random.Next(min, max)).ToString());
            }
            //maximum randomness
            for (int x = 0; x < 25; x++)
            {
                int slope = random.Next(0, 100);
                if (slope < 50)
                    slope = -1;
                else slope = 1;

                double rate = irate / 100;
                updateRate = (rate * slope);

                double[] temp = new double[25];
                temp[x] += prices[x] * updateRate;
                prices[x] += temp[x];
            }

            return prices;

        }//end determine rate method





    }
}