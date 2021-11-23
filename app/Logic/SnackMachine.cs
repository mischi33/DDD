using static DDDCourse.Logic.Money;
namespace DDDCourse.Logic
{
    public class SnackMachine : Entity
    {
        public virtual Money MoneyInside { get; protected set; } = None;
        public virtual Money MoneyInTransaction { get; protected set; } = None;

        public virtual void InsertMoney(Money money)
        {
            Money[] coinsAndNotes = { Cent, TenCent, Quarter, Dollar, FiveDollar, TwentyDollar };

            // Would this make more sense to be implemented as a method in Money? because value objects should contain most of the domain logic?
            if (!coinsAndNotes.Contains(money)) throw new InvalidOperationException();

            MoneyInTransaction += money;
        }

        public virtual void ReturnMoney()
        {
            // No e.g money.Clear() function here because this would violate immutability of value objects
            MoneyInTransaction = None;
        }

        public virtual void BuySnack()
        {
            MoneyInside += MoneyInTransaction;
            MoneyInTransaction = None;
        }
    }
}