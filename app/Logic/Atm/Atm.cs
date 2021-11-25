using DDDCourse.Logic.Common;
using DDDCourse.Logic.Shared;
using DDDCourse.Logic.Utils;
using static DDDCourse.Logic.Shared.Money;

namespace DDDCourse.Logic.Atm
{
    public class Atm : AggregateRoot
    {
        private const decimal CommissionRate = 0.01m;

        public virtual Money MoneyInside { get; protected set; } = None;
        public virtual decimal MoneyCharged { get; protected set; }

        public virtual string CanTakeMoney(decimal amount)
        {
            if (amount <= 0m)
                return "Invalid amount";

            if (MoneyInside.Amount < amount)
                return "Not enough money";

            if (!MoneyInside.CanAllocate(amount))
                return "Not enough change";

            return string.Empty;
        }

        public virtual void TakeMoney(decimal amount)
        {
            if (CanTakeMoney(amount) != string.Empty)
                throw new InvalidOperationException();

            Money output = MoneyInside.Allocate(amount);
            MoneyInside -= output;

            decimal amountWithCommission = CaluculateAmountWithCommission(amount);
            MoneyCharged += amountWithCommission;
            
            AddDomainEvent(new BalanceChangedEvent(amountWithCommission));

            // This is wrong, this should be performed automatically after DB update but since I did not setup the DB I need to dispatch the events manually
            EventListener.DispatchEvents(this);
        }

        public virtual decimal CaluculateAmountWithCommission(decimal amount)
        {
            decimal commission = amount * CommissionRate;
            decimal lessThanCent = commission % 0.01m;
            if (lessThanCent > 0)
            {
                commission = commission - lessThanCent + 0.01m;
            }
            return amount + commission;
        }

        public virtual void LoadMoney(Money money)
        {
            MoneyInside += money;
        }
    }
}
