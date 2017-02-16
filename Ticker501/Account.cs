using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    class Account
    {
        public double tradeFee = 9.99;
        public double transferFee = 4.99;
        public double totalBalance = 0;
        public double cash = 0;
        public Portfolio[] files = new Portfolio[3];
        public int size;

        //1 portfolio account constuctor
        public Account()
        {

        }

        public Account(Portfolio[] _file)
        {
            files = _file;

        }


        //adds the name to the portfolio within the correct positioning in array
        public void addPortfolio(string _name, int size)
        {
            size = this.size++;
            Portfolio p = new Ticker501.Portfolio(_name);
            bool increase = false;
            if (size < 1)
            {
                files[0] = p;
                files[0].pName = _name;
                increase = true;
            }
            if (size == 1)
            {
                files[1] = p;
                files[1].pName = _name;
                increase = true;
            }
            if (size == 2)
            {
                files[2] = p;
                files[2].pName = _name;
                increase = true;
            }
            //restricts the size of portfolios an account can have
            if (size >= 3)
            {
                Console.WriteLine("You already have the maximum amount of portfolios.\n"
                    + "If you'd like to create a new one, you must first delete a portfolio.");
                increase = false;
            }
            if (increase == true)
                size++;

        }

        public void removePortfolio(string name)
        {
            for (int i = 0; i < 3; i++)
            {
                if (name.Equals(files[i].pName))
                {
                    files[i].pName = null;

                    for (int x = 0; x < 25; x++)
                    {
                        files[i].quantity[x] = 0;
                    }
                    size--;
                    break;
                }
            }

        }//end remove portfolio method

        public void subtract(double amount)
        {
            cash -= amount;
        }

        public void subTradeFee()
        {
            cash -= tradeFee;
        }

        public double getBalance(StockMarket sm)
        {
            totalBalance = cash;
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < 25; j++)
                {
                    if (files[i].quantity[j] != 0)
                    {
                        totalBalance += (files[i].quantity[j] * sm.getPrice(j));
                    }
                }
            }

            return totalBalance;

        }

        public void Deposit(double amt)
        {
            cash += amt;
            totalBalance += amt;
            //deducting the fee for transfering money to account.
            subtract(transferFee);
        }

        public void Withdraw(double amt)
        {
            double amount = amt + transferFee;
            if (cash >= amount)
                cash -= amount;
            else
                Console.WriteLine("You do not have enough spendable funds to withdraw that amount");

        }




        public void addCash(double amount)
        {
            cash += amount;
        }


        public string getName(int pos)
        {

            return (files[pos].pName);
        }

        public bool checkName(string name, int pos)
        {
            bool result = false;
            if (files[pos].pName.Equals(name))
                result = true;
            return result;
        }

        //never is used but left in just in case, not neccessary
        public int getSize()
        {
            return size;
        }



        public void getResults(StockMarket sm)
        {
            double initial = 0;
            double final = 0;
            double diff = 0;
            double total = getBalance(sm);
            for (int k = 0; k < size; k++)
            {
                initial = files[k].iWorth;
                final = sm.setPortfolioWorth(files[k].quantity);
                diff = final - initial;
            }
            if (diff < 1)
            {
                diff = (diff * (-1));
            }
            else if (diff == 0)
            {
                Console.WriteLine("You've come out even currently in your active portfolios' careers.");
            }

            double result = total - diff;
            result = total - result;
            if (result > 1)
                Console.WriteLine("You are currently looking profitable with: $" + result + " gains.");
            else if (result == 0)
                Console.WriteLine("You've come out even currently in your active portfolios' careers.");
            else
                Console.WriteLine("You are currently down from inital purchases by: $" + ((-1) * result));



        }//end getResults method


    }
}
