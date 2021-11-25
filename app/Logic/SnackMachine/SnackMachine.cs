using DDDCourse.Logic.Common;
using DDDCourse.Logic.Shared;
using static DDDCourse.Logic.Shared.Money;

namespace DDDCourse.Logic.SnackMachines
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

        public virtual Money BuySnack(int position)
        {
            if (CanBuySnack(position) != string.Empty) throw new InvalidOperationException();

            Slot slot = GetSlot(position);

            // New assignment because of immutability
            // Should't slot be a value object?
            slot.SnackPile = slot.SnackPile.SubstractOne();

            Money change = MoneyInside.Allocate(MoneyInTransaction - slot.SnackPile.Price);
            MoneyInside -= change;
            MoneyInTransaction = 0;

            // This is violating CQS, how should something like this be done?
            return change;
        }

        public virtual string CanBuySnack(int position)
        {
            SnackPile snackPile = GetSnackPile(position);
            if (snackPile.Quantity == 0) return "The snack is sold out";
            if (MoneyInTransaction < snackPile.Price) return "Not enough money";

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

        public Money UnloadMoney()
        {
            if (MoneyInTransaction > 0)
                throw new InvalidOperationException();

            Money money = MoneyInside;
            MoneyInside = Money.None;
            return money;
        }
    }
}