using DDDCourse.Logic.Common;
using DDDCourse.Logic.Management.Db;
using DDDCourse.Logic.Atms;

namespace DDDCourse.Logic.Management {
    public class BalanceChangedEventHandler : IHandler<BalanceChangedEvent>
    {
        public void Handle(BalanceChangedEvent domainEvent)
        {
            // var repository = new HeadOfficeRepository();
            HeadOffice headOffice = HeadOfficeInstance.Instance;
            headOffice.ChangeBalance(domainEvent.Delta);
            // repository.Save(headOffice);
        }
    }
}