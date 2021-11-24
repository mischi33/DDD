using DDDCourse.Logic.Common;
using DDDCourse.Logic.Shared;
using static DDDCourse.Logic.Shared.Money;

namespace DDDCourse.Logic.SnackMachine
{
    public class SnackMachine : AggregateRoot
    {
        // virtual is only required to make NHibernate work with this entity
        // setter is protected and not private here because NHibernate would need this
        public virtual Money MoneyInside { get; protected set; }
        public virtual decimal MoneyInTransaction { get; protected set; }
        // NHibernate requires IList here
        protected virtual IList<Slot> Slots { get; set; }

        public SnackMachine()
        {
            MoneyInside = None;
            MoneyInTransaction = 0m;

            Slots = new List<Slot> {
                new Slot(this, 1),
                new Slot(this, 2),
                new Slot(this, 3),
            };
        }

        public virtual void InsertMoney(Money money)
        {
            Money[] coinsAndNotes = { Cent, TenCent, Quarter, Dollar, FiveDollar, TwentyDollar };

            // Would this make more sense to be implemented as a method in Money? because value objects should contain most of the domain logic?
            if (!coinsAndNotes.Contains(money)) throw new InvalidOperationException();

            MoneyInTransaction += money.Amount;
            MoneyInside += money;
        }

        public virtual void ReturnMoney()
        {
            Money moneyToReturn = MoneyInside.Allocate(MoneyInTransaction);
            MoneyInside -= moneyToReturn;
            MoneyInTransaction = 0;
        }

        public virtual void BuySnack(int position)
        {
            if (CanBuySnack(position) != string.Empty) throw new InvalidOperationException();

            Slot slot = GetSlot(position);

            // New assignment because of immutability
            // But shouldn't SnackPile be an entity anyways?
            slot.SnackPile = slot.SnackPile.SubstractOne();

            Money change = MoneyInside.Allocate(MoneyInTransaction - slot.SnackPile.Price);
            MoneyInside -= change;
            MoneyInTransaction = 0;
        }

        public virtual string CanBuySnack(int position)
        {
            SnackPile snackPile = GetSnackPile(position);
            if (snackPile.Quantity == 0) return "The snack is sold out";
            if (MoneyInTransaction < snackPile.Price) return "Not enough money";
            // TODO check why this is not working
            if (!MoneyInside.CanAllocate(MoneyInTransaction - snackPile.Price)) return "Not enough change";
            return string.Empty;
        }

        public virtual void LoadSnacks(int position, SnackPile snackPile)
        {
            Slot slot = GetSlot(position);
            slot.SnackPile = snackPile;
        }

        public virtual SnackPile GetSnackPile(int position)
        {
            return GetSlot(position).SnackPile;
        }

        private Slot GetSlot(int position)
        {
            return Slots.Single(x => x.Position == position);
        }

        public virtual void LoadMoney(Money money)
        {
            MoneyInside += money;
        }
    }
}