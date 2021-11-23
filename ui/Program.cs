// See https://aka.ms/new-console-template for more information
using DDDCourse.Logic;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.AcceptanceCriteria;
using FluentNHibernate.Conventions.Helpers;
using FluentNHibernate.Conventions.Instances;
using NHibernate;

// Setup DB here and fetch snack machine
Initer.init("@Server=.;Database=DDDDB;Trusted_Connection=true");
SnackMachine snackMachine;
using (ISession session = SessionFactory.OpenSession()) {
    snackMachine = session.Get<SnackMachine>(1L);
}

// SnackMachine snackMachine = new SnackMachine();

Console.WriteLine("** Hello, this is snackmachine. I offer the following snacks:");
Console.WriteLine("** 1 Chocolate: 1.75$");
Console.WriteLine("** 2 Fanta: 2.79$");
Console.WriteLine("** 3 Smarties: 1.85$");
Console.WriteLine("** You can insert either 1, 10 or a quarter cents or one, five or twenty dollar bills");
Console.WriteLine("** Type 'cent!1', 'cent!10' or 'cent!25' to insert coins");
Console.WriteLine("** Type 'dollar!1', 'dollar!5' or 'dollar!20' to insert bills");
Console.WriteLine("** Type 'buy![snacknumber]' to buy a snack");
Console.WriteLine("** Type 'return!money' to return your inserted money.");

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
            snackMachine.InsertMoney(amount == 1 ? Money.Cent : amount == 10 ? Money.TenCent : Money.Quarter);
            Console.WriteLine($"** Your current total amount of money is: {snackMachine.MoneyInTransaction.Amount}");
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
            snackMachine.InsertMoney(amount == 1 ? Money.Dollar : amount == 5 ? Money.FiveDollar : Money.TwentyDollar);
            Console.WriteLine($"** Your current total amount of money is: {snackMachine.MoneyInTransaction.Amount}");
            continue;
        }

        Console.WriteLine("** You can only insert 1, 5 or 10 dollar");
        continue;
    }

    if (input[0] == ("return"))
    {
        snackMachine.ReturnMoney();
        Console.WriteLine($"** Here is your money. Your current total amount of money is: {snackMachine.MoneyInTransaction.Amount}.");
        Console.WriteLine("** Do you want to continue buying something or not? (Y/N)");
        string cont = Console.ReadLine();
        if (cont.ToUpper() == "Y") continue;
        if (cont.ToUpper() == "N") selectedSnack = "Nothing"; break;
    }

    if (input[0].Contains("buy"))
    {
        Int32 snackNumber = Int32.Parse(input[1]);
        if (snackNumber == 1)
        {
            if (snackMachine.MoneyInTransaction.Amount > 1.75m)
            {
                using (ISession session = SessionFactory.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(snackMachine);
                    transaction.Commit();
                }
                selectedSnack = "Chocolate";
                break;
            }
            Console.WriteLine("** You do not have enough money to buy this snack. Please insert more money");
            continue;
        }

        if (snackNumber == 2)
        {
            if (snackMachine.MoneyInTransaction.Amount > 2.79m)
            {
                using (ISession session = SessionFactory.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(snackMachine);
                    transaction.Commit();
                }
                selectedSnack = "Fanta";
                break;
            }
            Console.WriteLine("** You do not have enough money to buy this snack. Please insert more money");
            continue;
        }

        if (snackNumber == 3)
        {
            if (snackMachine.MoneyInTransaction.Amount > 1.85m)
            {
                using (ISession session = SessionFactory.OpenSession())
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.SaveOrUpdate(snackMachine);
                    transaction.Commit();
                }
                selectedSnack = "Smarties";
                break;
            }
            Console.WriteLine("** You do not have enough money to buy this snack. Please insert more money");
            continue;
        }
    }
}

Console.WriteLine($"** Thank you for buying {selectedSnack.ToUpper()}. See you!");