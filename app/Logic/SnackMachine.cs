using static DDDCourse.Logic.Money;
namespace DDDCourse.Logic
{
    public class SnackMachine : Entity
    {
        // virtual is only required to make NHibernate work with this entity
        // setter is protected and not private here because NHibernate would need this
        public virtual Money MoneyInside { get; protected set; }
        public virtual Money MoneyInTransaction { get; protected set; }
        
        // NHibernate requires IList here
        public virtual IList<Slot> Slots { get; protected set; }

        public SnackMachine() {
            MoneyInside = None;
            MoneyInTransaction = None;

            Slots = new List<Slot> {
                new Slot(this, 1, null, 0, 0),
                new Slot(this, 2, null, 0, 0),
                new Slot(this, 3, null, 0, 0),
            };
        }

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

        public virtual void BuySnack(int position)
        {
            Slot slot = Slots.Single(x => x.Position == position);
            slot.Quantity--;

            MoneyInside += MoneyInTransaction;
            MoneyInTransaction = None;
        }

        public virtual void LoadSnacks(int position, Snack snack, int quantity, decimal price) {
            Slot slot = Slots.Single(x => x.Position == position);
            slot.Snack = snack;
            slot.Quantity = quantity;
            slot.Price = price;
        }
    }
}