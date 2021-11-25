// See https://aka.ms/new-console-template for more information
using DDDCourse.Logic.SnackMachines;
using DDDCourse.Logic.Atms;
using DDDCourse.Logic.Management;
using DDDCourse.Logic.Management.Db;
using DDDCourse.Logic.Shared;
using DDDCourse.Logic.Utils;

public class Ui
{
    public static SnackMachine SnackMachine { get; } = new SnackMachine();
    public static Atm Atm { get; } = new Atm();
    public static void Main(string[] args)
    {
        Initer.init("db connection string would go here");
        while (true)
        {
            Console.WriteLine("*** Choose machine: ATM (A) or Snack Machine (S) or Management Terminal (M).");
            string machine = Console.ReadLine();

            if (machine.ToUpper() == "S") ExecuteSnackMachine();
            if (machine.ToUpper() == "A") ExecuteAtm();
            if (machine.ToUpper() == "M") ExecuteManagementTerminal();
        }
    }

    public static void ExecuteSnackMachine()
    {
        SnackMachine.LoadMoney(new Money(100, 100, 50, 15, 13, 5));
        SnackMachine.LoadSnacks(1, new SnackPile(Snack.Chocolate, 10, 1.75m));
        SnackMachine.LoadSnacks(2, new SnackPile(Snack.Soda, 6, 2.50m));
        SnackMachine.LoadSnacks(3, new SnackPile(Snack.Gum, 15, 0.75m));

        Console.WriteLine("*** Hello, this is snack machine.");
        Console.WriteLine("*** You can insert either 1, 10 or a quarter cents or one, five or twenty dollar bills");
        Console.WriteLine("*** Type 'cent!1', 'cent!10' or 'cent!25' to insert coins");
        Console.WriteLine("*** Type 'dollar!1', 'dollar!5' or 'dollar!20' to insert bills");
        Console.WriteLine("*** Type 'buy![snacknumber]' to buy a snack");
        Console.WriteLine("*** Type 'machine!return' to return your inserted money.");
        Console.WriteLine("*** Type 'machine!snacks' for displaying the available snacks.");
        Console.WriteLine("*** Type 'machine!money' to show how much money in total is contained in the machine.");
        Console.WriteLine("*** Type 'machine!transaction' to show how much money you have inserted.");
        Console.WriteLine("*** Type 'support!change' for calling support that loads machine with more change.");
        ShowSnacks();

        while (true)
        {
            Console.WriteLine("*** Please type a command");

            var input = Console.ReadLine().Split("!");

            if (input[0].Contains("cent"))
            {
                Int32 amount = Int32.Parse(input[1]);
                if (amount == 1 || amount == 10 || amount == 25)
                {
                    SnackMachine.InsertMoney(amount == 1 ? Money.Cent : amount == 10 ? Money.TenCent : Money.Quarter);
                    Console.WriteLine($"*** Your current total amount of money inserted is: {SnackMachine.MoneyInTransaction}");
                    continue;
                }

                Console.WriteLine("*** You can only insert 1, 10 or a quarter cents");
                continue;
            }

            if (input[0].Contains("dollar"))
            {
                Int32 amount = Int32.Parse(input[1]);
                if (amount == 1 || amount == 5 || amount == 20)
                {
                    SnackMachine.InsertMoney(amount == 1 ? Money.Dollar : amount == 5 ? Money.FiveDollar : Money.TwentyDollar);
                    Console.WriteLine($"*** Your current total amount of money inserted is: {SnackMachine.MoneyInTransaction}");
                    continue;
                }

                Console.WriteLine("*** You can only insert 1, 5 or 10 dollar bills");
                continue;
            }

            if (input[0] == ("machine"))
            {
                if (input[1] == "return")
                {
                    SnackMachine.ReturnMoney();
                    Console.WriteLine($"*** Here is your money. Your current total amount of money is: {SnackMachine.MoneyInTransaction}$.");
                    if (ContinueBuying()) continue;
                    break;
                }

                if (input[1] == "snacks")
                {
                    ShowSnacks();
                }

                if (input[1] == "money")
                {
                    Console.WriteLine($"*** {SnackMachine.MoneyInside.Amount}$ currently inside the snack machine.");
                }

                if (input[1] == "transaction")
                {
                    Console.WriteLine($"*** You have inserted {SnackMachine.MoneyInTransaction}$ so far.");
                }
            }

            if (input[0] == ("support") && input[1] == "change")
            {
                SnackMachine.LoadMoney(Money.TwentyDollar);
                Console.WriteLine($"*** Thank you for contacting support. I've loaded the machine with some more money for change. Byeee");
            }

            if (input[0].Contains("buy"))
            {
                Int32 snackNumber = Int32.Parse(input[1]);
                if (snackNumber == 1 || snackNumber == 2 || snackNumber == 3)
                {
                    string error = CanBuy(snackNumber);
                    if (error == string.Empty)
                    {
                        Money change = Buy(snackNumber);
                        Console.WriteLine($"*** Thank you for buying {SnackMachine.GetSnackPile(snackNumber).Snack.Name.ToUpper()}.");
                        if (change.Amount > 0) ShowChange(change);

                        if (ContinueBuying()) continue;
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
        Console.WriteLine("Alrighty. See you!");
    }

    public static void ExecuteAtm()
    {
        Atm.LoadMoney(new Money(200, 200, 200, 100, 300, 300));

        Console.WriteLine("*** Hello, this is ATM.");
        Console.WriteLine("*** Type 'take![amount]' to define the amount to withdraw.");
        Console.WriteLine("*** Type 'atm!balance' to show total balance.");
        Console.WriteLine("*** Type 'atm!charged' to show all withdrawal charged charged so far (withdrawals + fees).");
        Console.WriteLine("*** Type 'atm!exit' to leave ATM.");

        while (true)
        {
            Console.WriteLine("*** Please type a command");

            string[] input = Console.ReadLine().Split("!");
            if (input[0] == "take")
            {
                decimal amount = decimal.Parse(input[1]);

                string error = Atm.CanTakeMoney(amount);
                if (error == string.Empty)
                {
                    Atm.TakeMoney(amount);
                    Console.WriteLine($"*** You have withdrawn {amount}$ from your account.");
                    Console.WriteLine($"*** Money left: {Atm.MoneyInside.Amount}$");
                    Console.WriteLine($"*** Money charged: {Atm.MoneyCharged}$");
                }
            }

            if (input[0] == "atm")
            {
                if (input[1] == "balance") Console.WriteLine($"*** Current balance: {Atm.MoneyInside.Amount}$");
                if (input[1] == "charged") Console.WriteLine($"*** Total money charged: {Atm.MoneyCharged}$");
                if (input[1] == "exit") break;
            }
        }
    }

    public static void ExecuteManagementTerminal()
    {
        HeadOffice headOffice = HeadOfficeInstance.Instance;

        Console.WriteLine("*** Hello, this is your Management Terminal.");
        Console.WriteLine("*** Type 'show!balance' to display the current balance.");
        Console.WriteLine("*** Type 'show!cash' to display the current balance.");
        Console.WriteLine("*** Type 'terminal!exit' to leave Management Terminal.");
        Console.WriteLine("*** Type 'terminal!unload' to unload cash from the Snack Machine.");
        Console.WriteLine("*** Type 'terminal!load' to load currently stored cash to ATM.");

        while (true)
        {
            Console.WriteLine("*** Please type a command");

            string[] input = Console.ReadLine().Split("!");

            if (input[0] == "show")
            {
                if (input[1] == "balance") Console.WriteLine($"*** The current balance is at {headOffice.Balance}$");
                if (input[1] == "cash") Console.WriteLine($"*** Current amount of cash is at {headOffice.Cash.Amount}$");
            }

            if (input[0] == "terminal")
            {
                if (input[1] == "unload")
                {
                    headOffice.UnloadCashFromSnackMachine(SnackMachine);
                    Console.WriteLine($"*** Current amount of cash is at {headOffice.Cash.Amount}$");
                }
                if (input[1] == "unload")
                {
                    headOffice.LoadCashToAtm(Atm);
                    Console.WriteLine($"*** Current amount of cash is now again at {headOffice.Cash.Amount}$");
                }
                if (input[1] == "exit") break;
            }
        }
    }

    public static string CanBuy(int snackPosition)
    {
        return SnackMachine.CanBuySnack(snackPosition);
    }

    public static Money Buy(int snackPosition)
    {
        return SnackMachine.BuySnack(snackPosition);
    }

    public static bool ContinueBuying()
    {
        Console.WriteLine("*** Do you want to continue buying something? (Y/N)");
        string cont = Console.ReadLine();
        return cont.ToUpper() == "Y";
    }

    public static void ShowChange(Money change)
    {
        Console.WriteLine($"*** Your change in total is {change.Amount}$");
        Console.WriteLine($"*** One cents: {change.OneCentCount} pieces");
        Console.WriteLine($"*** Ten cents: {change.TenCentCount} pieces");
        Console.WriteLine($"*** Quarter cents: {change.QuarterCount} pieces");
        Console.WriteLine($"*** One dollar: {change.OneDollarCount} pieces");
        Console.WriteLine($"*** Five dollar: {change.FiveDollarCount} pieces");
        Console.WriteLine($"*** Twenty dollar: {change.TwentyDollarCount} pieces");
    }

    public static void ShowSnacks()
    {
        Console.WriteLine("************** The following snacks are currently available **************");
        Console.WriteLine($"*** 1 {SnackMachine.GetSnackPile(1).Snack.Name}\tPrice: {SnackMachine.GetSnackPile(1).Price}$\t{SnackMachine.GetSnackPile(1).Quantity} pieces left");
        Console.WriteLine($"*** 2 {SnackMachine.GetSnackPile(2).Snack.Name}\tPrice: {SnackMachine.GetSnackPile(2).Price}$\t{SnackMachine.GetSnackPile(2).Quantity} pieces left");
        Console.WriteLine($"*** 3 {SnackMachine.GetSnackPile(3).Snack.Name}\tPrice: {SnackMachine.GetSnackPile(3).Price}$\t{SnackMachine.GetSnackPile(3).Quantity} pieces left");
        Console.WriteLine("**************************************************************************");
    }
}