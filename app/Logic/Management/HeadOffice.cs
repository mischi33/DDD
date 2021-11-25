using DDDCourse.Logic.Common;
using DDDCourse.Logic.Shared;

namespace DDDCourse.Logic.Management
{
    public class HeadOffice : AggregateRoot
    {
        public virtual decimal Balance { get; protected set; }
        public virtual Money Cash { get; protected set; } = Money.None;

        public virtual void ChangeBalance(decimal delta) {
            Balance += delta;
        }
    }
}