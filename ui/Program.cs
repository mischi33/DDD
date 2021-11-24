// See https://aka.ms/new-console-template for more information
using DDDCourse.Logic;
public class Ui
{
    public static SnackMachine SnackMachine { get; } = new SnackMachine();
    public static void Main(string[] args)
    {
        SnackMachine.LoadMoney(Money.Dollar * 20);
        SnackMachine.LoadSnacks(1, new SnackPile(Snack.Chocolate, 10, 1.75m));
        SnackMachine.LoadSnacks(2, new SnackPile(Snack.Soda, 6, 2.50m));
        SnackMachine.LoadSnacks(3, new SnackPile(Snack.Gum, 15, 0.75m));

        Console.WriteLine("** Hello, this is snackmachine.");
        Console.WriteLine("** You can insert either 1, 10 or a quarter cents or one, five or twenty dollar bills");
        Console.WriteLine("** Type 'cent!1', 'cent!10' or 'cent!25' to insert coins");
        Console.WriteLine("** Type 'dollar!1', 'dollar!5' or 'dollar!20' to insert bills");
        Console.WriteLine("** Type 'buy![snacknumber]' to buy a snack");
        Console.WriteLine("** Type 'machine!return' to return your inserted money.");
        Console.WriteLine("** Type 'machine!snacks' for displaying the available snacks.");
        Console.WriteLine("** Type 'support!change' for calling support that loads machine with more change.");
        ShowSnacks();

        var selectedSnack = "";
        while (true)
        {
            Console.WriteLine("** Please type a command");

            var input = Console.ReadLine().Split("!");

            if (input[0].Contains("cent"))
            {
                Int32 amount = Int32.Parse(input[1]);
                if (amount == 1 || amount == 10 || amount == 25)
                {
                    SnackMachine.InsertMoney(amount == 1 ? Money.Cent : amount == 10 ? Money.TenCent : Money.Quarter);
                    Console.WriteLine($"** Your current total amount of money inserted is: {SnackMachine.MoneyInTransaction}");
                    continue;
                }

                Console.WriteLine("** You can only insert 1, 10 or a quarter cents");
                continue;
            }

            if (input[0].Contains("dollar"))
            {
                Int32 amount = Int32.Parse(input[1]);
                if (amount == 1 || amount == 5 || amount == 20)
                {
                    SnackMachine.InsertMoney(amount == 1 ? Money.Dollar : amount == 5 ? Money.FiveDollar : Money.TwentyDollar);
                    Console.WriteLine($"** Your current total amount of money is: {SnackMachine.MoneyInTransaction}");
                    continue;
                }

                Console.WriteLine("** You can only insert 1, 5 or 10 dollar bills");
                continue;
            }

            if (input[0] == ("machine"))
            {
                if (input[1] == "return")
                {
                    SnackMachine.ReturnMoney();
                    Console.WriteLine($"** Here is your money. Your current total amount of money is: {SnackMachine.MoneyInTransaction}.");
                    Console.WriteLine("** Do you want to continue buying something or not? (Y/N)");
                    string cont = Console.ReadLine();
                    if (cont.ToUpper() == "Y") continue;
                    if (cont.ToUpper() == "N") selectedSnack = "Nothing"; break;
                }

                if (input[1] == "snacks")
                {
                    ShowSnacks();
                }
            }

            if (input[0] == ("support") && input[1] == "change")
            {
                SnackMachine.LoadMoney(Money.TwentyDollar);
                Console.WriteLine($"** Thank you for contacting support. I've loaded the machine with some more money for change. Byeee");
            }

            if (input[0].Contains("buy"))
            {
                Int32 snackNumber = Int32.Parse(input[1]);
                if (snackNumber == 1 || snackNumber == 2 || snackNumber == 3)
                {
                    string error = Buy(snackNumber);
                    if (error == string.Empty)
                    {
                        selectedSnack = SnackMachine.GetSnackPile(snackNumber).Snack.Name;
                        break;
                    }
                    else
                    {
                        Console.WriteLine($"**ERROR {error}");
                        continue;
                    }

                }
            }
        }
        Console.WriteLine($"** Thank you for buying {selectedSnack.ToUpper()}. See you!");
    }


    public static string Buy(int snackPosition)
    {
        string error = SnackMachine.CanBuySnack(snackPosition);
        if (error == string.Empty) SnackMachine.BuySnack(snackPosition);

        return error;
    }

    public static void ShowSnacks()
    {
        Console.WriteLine("************** The following snacks are currently available **************");
        Console.WriteLine($"** 1 {SnackMachine.GetSnackPile(1).Snack.Name}\tPrice: {SnackMachine.GetSnackPile(1).Price}$\t{SnackMachine.GetSnackPile(1).Quantity} pieces left");
        Console.WriteLine($"** 2 {SnackMachine.GetSnackPile(2).Snack.Name}\tPrice: {SnackMachine.GetSnackPile(2).Price}$\t{SnackMachine.GetSnackPile(2).Quantity} pieces left");
        Console.WriteLine($"** 3 {SnackMachine.GetSnackPile(3).Snack.Name}\tPrice: {SnackMachine.GetSnackPile(3).Price}$\t{SnackMachine.GetSnackPile(3).Quantity} pieces left");
        Console.WriteLine("**************************************************************************");
    }
}