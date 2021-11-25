namespace DDDCourse.Logic.Common {
    public interface IHandler<T> where T : IDomainEvent {
        void Handle(T domainEvent);
    }
}