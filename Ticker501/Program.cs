using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ticker501
{
    class Program
    {
        static void Main(string[] args)
        {
            char delimiter = '-';
            string[] pieces = new string[3];
            string[] name = new string[25];
            string[] title = new string[25];
            double[] price = new double[25];
            string[] sprice = new string[25];

            string line;
            string fileName = "C:/Users/Tyler/Desktop/Ticker.txt";
            int count = 0;
            Account user = new Ticker501.Account();

            //reading in text file to create Stock Market
            Console.WriteLine("reading stock files now...");
            using (StreamReader sr = new StreamReader(fileName))
            {
                while ((line = sr.ReadLine()) != null)
                {
                    pieces = line.Split(delimiter);
                    for (int p = 0; p < 3; p++)
                    {
                        if (p == 0)
                            name[count] = pieces[p];
                        if (p == 1)
                            title[count] = pieces[p];
                        if (p == 2)
                            sprice[count] = pieces[p];
                    }//end for 
                    count++;
                    if (count == name.Length)
                        break;

                }//end while
                sr.Close();
            }//end using
            Console.WriteLine("File read in properly...");

            //converting string to double for price now

            for (int y = 0; y < price.Length; y++)
            {
                string tempLine = sprice[y].Trim('$');
                price[y] = Double.Parse(tempLine);
            }//end for



            //creating initial stock market now
            StockMarket sm = new StockMarket(name, title, price);
            Console.WriteLine("Stock Market has been recreated");




            Console.WriteLine("What would you like to call your initial portfolio?");
            string called = Console.ReadLine();
            int size = 0;

            user.addPortfolio(called, size);
            size++;

            bool exit = false;
            while (exit == false)
            {

                //main menu for user interaction
                Console.WriteLine("Press the corresponding letter to navigate the menu");
                Console.WriteLine("D: Deposit money into account");
                Console.WriteLine("W: withdraw money from account");
                Console.WriteLine("A: Show account's information");
                Console.WriteLine("P: Print out Stock Market list");
                Console.WriteLine("V: Choose market volatility");
                Console.WriteLine("C: Create a portfolio");
                Console.WriteLine("R: Remove a portfolio");
                Console.WriteLine("B: Buy stocks");
                Console.WriteLine("S: Sell stocks");
                Console.WriteLine("E: Exit Program");
                string react = Console.ReadLine();


                int index = 0;
                //main menu navigation 
                switch (react)
                {
                    case "D":
                    case "d":
                        Console.WriteLine("How much would you like to deposit into your account today?");
                        double damount = Double.Parse(Console.ReadLine());
                        user.Deposit(damount);
                        Console.WriteLine("You now have a total of $" + user.cash + " worth of spendable currency.");
                        break;

                    case "W":
                    case "w":
                        Console.WriteLine("How much would you like to withdraw from your account today?");
                        double wamount = Double.Parse(Console.ReadLine());
                        if (user.cash >= wamount)
                        {
                            user.Withdraw(wamount);
                            Console.WriteLine("Your account balance is now: " + user.cash);
                        }
                        else
                            Console.WriteLine("You do not have enough funds to make this withdraw.");


                        break;

                    case "A":
                    case "a":
                        try
                        {

                            double total = user.getBalance(sm);
                            Console.WriteLine("Your current account balance is: $" + user.cash);
                            Console.WriteLine("With all portfolio's combined your account is worth $" + total);
                            user.getResults(sm);


                            size = user.getSize();
                            for (int i = 0; i < size; i++)
                            {
                                double portWorth = 0;
                                for (int j = 0; j < 25; j++)
                                {

                                    int amount = user.files[index].quantity[j];

                                    portWorth += amount * sm.getPrice(j);

                                }//inside loop
                                double perc = Math.Round(((portWorth / total) * 100), 2, MidpointRounding.AwayFromZero);
                                Console.WriteLine("\nYour portfolio " + user.files[index].pName + " is worth $"
                                    + portWorth + " and makes up a total of " + perc + "% of your account");
                                int[] quantity = new int[25];
                                quantity = user.files[index].quantity;
                                double worth = sm.setPortfolioWorth(quantity);
                                sm.myStock(user.files[index].quantity, worth);
                            }//outside loop


                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;


                    case "P":
                    case "p":
                        sm.printStock();
                        break;

                    case "V":
                    case "v":

                        Console.WriteLine("Choose volatility of stock market, between 1-3, 3 being the highest maximum");
                        int numb = Int32.Parse(Console.ReadLine());
                        if (numb >= 3)
                        {
                            numb = 3;
                            Console.WriteLine("Maximum stock volatility chosen. All stock prices altered between (+) or (-) 3-15%");
                        }
                        if (numb == 2)
                        {
                            Console.WriteLine("Medium stock volatility chosen. All stock prices altered between (+) or (-) 2-8%");
                        }
                        else
                        {
                            numb = 1;
                            Console.WriteLine("Minimum stock volatility chosen. All stock prices altered between (+) or (-) 1-4%");
                        }
                        sm = new StockMarket(name, title, sm.determineRate(numb));
                        Console.WriteLine("*****Stock market has been updated*****");
                        break;

                    case "C":
                    case "c":
                        try
                        {

                            bool result = true;
                            if (size > 2)
                            {
                                Console.WriteLine("You already have the maximum amount of portfolios, you must first remove one to create a new one.");
                                result = false;
                            }
                            if (result)
                            {
                                Console.WriteLine("What would you like to name this Portfolio?");
                                string another = Console.ReadLine();
                                user.addPortfolio(another, size);
                                size++;
                                Console.WriteLine("New portfolio has been made, referred to as: " + another);
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred while tring to create your new portfolio.\n" +
                                ex.Message);
                        }
                        break;

                    case "R":
                    case "r":
                        try
                        {
                            Console.WriteLine("What portfolio would you like to remove? Please type the name: ");
                            string rName = Console.ReadLine();

                            for (int m = 0; m < 3; m++)
                            {
                                bool found = false;
                                if (user.checkName(rName, m) == true)
                                {
                                    found = true;
                                    user.cash += sm.sellAllStock(user.files[m].quantity);
                                    user.subTradeFee();
                                    user.removePortfolio(user.files[m].pName);
                                    size--;
                                    Console.WriteLine("Your portfolio has been removed.");
                                    break;
                                }
                                if (m == 3 && found == false)
                                {
                                    Console.WriteLine("Sorry we could not find that portfolio's name, please check your spelling.");
                                    break;
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("An error occurred while trying to remove your portfolio, please double-check spelling\n"
                                + ex.Message);
                        }
                        break;



                    case "b":
                    case "B":

                        try
                        {
                            bool ask = true;


                            bool fineCheck = false;
                            //while loop is so user isnt charged multiple fees per transaction
                            bool verify = false;

                            while (verify == false)
                            {

                                if (user.getSize() == 1)
                                {
                                    index = 0;
                                    ask = false;
                                }



                                while (ask == true)
                                {


                                    //getting portfolio and stock index's
                                    Console.WriteLine("What portfolio will be adding these stocks into today?");
                                    string chosen = Console.ReadLine();
                                    for (int f = 0; f < 3; f++)
                                    {
                                        if (chosen.Equals(user.getName(f)))
                                        {
                                            index = f;
                                            ask = false;
                                            break;
                                        }
                                    }

                                }//end asking while loop


                                sm.printStock();
                                Console.WriteLine("what stock would you like to buy? if you prefer to choose using" +
                                "\nIntegers such as a list 1-25 (1 being top on the list), that will work as well.");
                                string stockName = Console.ReadLine();

                                int stockIndex = Int32.Parse(stockName);
                                if (stockName.Length > 1)
                                {

                                    for (int w = 0; w < 25; w++)
                                    {
                                        if (stockName.Equals(name[w]) || stockName.Equals(title[w]))
                                        {
                                            stockIndex = w;
                                            break;
                                        }
                                    }//end for
                                }//end if statement for string names

                                else if (Int32.Parse(stockName) < 26)
                                {
                                    //make up for 0 position
                                    stockIndex = Int32.Parse(stockName) - 1;
                                }
                                //if (stockIndex == 24)
                                //    stockIndex = 23;
                                if (stockIndex >= 25)
                                    stockIndex = 24;

                                Console.WriteLine("And how much of this stock would you like? Each stock costs: $" + sm.getPrice(stockIndex));
                                int amount = Int32.Parse(Console.ReadLine());

                                //adds the amount of stocks to the user's portfolio
                                double cost = (amount * price[stockIndex]) + user.tradeFee;
                                if (user.cash >= cost)
                                {
                                    //sets the portfolio's quantity of the stock while tracking their payments
                                    user.files[index].buyStock(stockIndex, amount, sm.getPrice(stockIndex));
                                    //I take the tradefee out after the loop finishes, so not to double charge
                                    user.subtract(cost - user.tradeFee);
                                    fineCheck = true;
                                }
                                else
                                {
                                    Console.WriteLine("You do not have enough funds.");
                                    fineCheck = false;
                                }



                                Console.WriteLine("To continue buying stocks in this portfolio, type 'Y'. To exit buying, type 'E': ");
                                string word = Console.ReadLine();
                                if (word == "E" || word == "e")
                                    verify = true;
                                else if (word == "Y" || word == "y")
                                {
                                    verify = false;
                                    ask = false;
                                }

                            }//end while statement (while (verify == false))
                            if (fineCheck)
                                user.subTradeFee();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    //end of buy

                    case "S":
                    case "s":
                        try
                        {
                            bool ask = true;


                            //this keeps track whether they've sold a stock this loop or not
                            bool fine = false;
                            //while loop is so user isnt charged multiple fees per transaction
                            bool aswr = false;
                            while (aswr == false)
                            {

                                if (user.getSize() == 1)
                                {
                                    ask = false;
                                    index = 0;
                                }


                                while (ask == true)
                                {
                                    //getting portfolio index's
                                    Console.WriteLine("What portfolio would you like to sell stocks from?");
                                    string choicer = Console.ReadLine();

                                    for (int f = 0; f < 3; f++)
                                    {
                                        if (choicer.Equals(user.getName(f)))
                                        {
                                            index = f;
                                            ask = false;
                                            break;
                                        }
                                    }

                                }//end ask while loop

                                int[] quantity = new int[25];
                                quantity = user.files[index].quantity;
                                double worth = sm.setPortfolioWorth(quantity);
                                //prints out stock info of that specific portfolio
                                sm.myStock(user.files[index].quantity, worth);
                                Console.WriteLine("What stock would you like to sell?");
                                string sName = Console.ReadLine();

                                //gets index of stock positioning using name
                                int posit = sm.searchPos(sName);
                                Console.WriteLine("How much of this stock would you like to sell?");
                                int amt = Int32.Parse(Console.ReadLine());

                                //checking that they have the proper amount to sell in the first place
                                if (user.files[index].quantity[posit] < amt)
                                {
                                    amt = user.files[index].quantity[posit];
                                }
                                else if (user.files[index].quantity[posit] >= amt)
                                {
                                    //call sell stock method
                                    //substract quantity, track, and check they have enough
                                    user.files[index].sellStock(posit, amt, sm.getPrice(posit));
                                    double cashAmt = (amt * sm.getPrice(posit));
                                    user.addCash(cashAmt);
                                    fine = true;
                                }

                                else
                                {
                                    Console.WriteLine("You do not have enough of that stock to sell.");
                                    fine = false;
                                }

                                Console.WriteLine("To continue selling stocks, type 'Y', To exit selling, type 'E': ");
                                string word = Console.ReadLine();
                                if (word == "E" || word == "e")
                                    aswr = true;
                                else
                                    aswr = false;

                            }//end while statement (while(aswr == false))
                            if (fine)
                                user.subTradeFee();

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;
                    //end of sell





                    case "N":
                    case "n":

                        try
                        {
                            size = user.getSize();

                            Console.WriteLine("Type the portfolio's name that'd you like to see: ");
                            string checker = Console.ReadLine();

                            for (int k = 0; k < 3; k++)
                            {
                                bool found = false;
                                if (user.checkName(checker, k) == true)
                                {
                                    index = k;
                                    found = true;
                                    break;

                                }
                                if (k == 3 && found == false)
                                {
                                    Console.WriteLine("That portfolio was not found, please check your spelling.");
                                    break;
                                }
                            }
                            int[] quantity = new int[25];
                            quantity = user.files[index].quantity;
                            double net = Math.Round(sm.setPortfolioWorth(quantity), 2, MidpointRounding.AwayFromZero);
                            Console.WriteLine("Your portfolio's net worth is: $" + net);

                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        break;

                    case "E":
                    case "e":
                        //asking user if they want to exit the main menu (will exit program)
                        exit = true;

                        break;
                }//end switch





            }//end main menu while loop




        }//end main



    }//end program
}//end namespace
