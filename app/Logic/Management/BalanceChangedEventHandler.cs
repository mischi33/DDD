using DDDCourse.Logic.Common;
using DDDCourse.Logic.Management.Db;
using DDDCourse.Logic.Atm;

namespace DDDCourse.Logic.Management {
    public class BalanceChangedEventHandler : IHandler<BalanceChangedEvent>
    {
        public void Handle(BalanceChangedEvent domainEvent)
        {
            HeadOffice headOffice = HeadOfficeInstance.Instance;
            headOffice.ChangeBalance(domainEvent.Delta);
        }
    }
}