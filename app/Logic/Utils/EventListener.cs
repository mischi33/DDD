using DDDCourse.Logic.Common;
using NHibernate.Event;

namespace DDDCourse.Logic.Utils
{
    public class EventListener 
    // : 
    //     IPostInsertEventListener, 
    //     IPostDeleteEventListener, 
    //     IPostUpdateEventListener, 
    //     IPostCollectionUpdateEventListener
    {
        // public void OnPostDelete(PostDeleteEvent ev)
        // {
        //     DispatchEvents(ev.Entity as AggregateRoot);
        // }

        // public Task OnPostDeleteAsync(PostDeleteEvent ev, CancellationToken cancellationToken)
        // {
        //     throw new NotImplementedException();
        // }

        // public void OnPostInsert(PostInsertEvent ev)
        // {
        //    DispatchEvents(ev.Entity as AggregateRoot);
        // }

        // public Task OnPostInsertAsync(PostInsertEvent ev, CancellationToken cancellationToken)
        // {
        //     throw new NotImplementedException();
        // }

        // public void OnPostUpdate(PostUpdateEvent ev)
        // {
        //      DispatchEvents(ev.Entity as AggregateRoot);
        // }

        // public Task OnPostUpdateAsync(PostUpdateEvent ev, CancellationToken cancellationToken)
        // {
        //     throw new NotImplementedException();
        // }

        // public void OnPostUpdateCollection(PostCollectionUpdateEvent ev)
        // {
        //      DispatchEvents(ev.AffectedOwnerOrNull as AggregateRoot);
        // }

        // public Task OnPostUpdateCollectionAsync(PostCollectionUpdateEvent ev, CancellationToken cancellationToken)
        // {
        //     throw new NotImplementedException();
        // }

        // This should be private and not static but since I do not have a database in use I need to raise this method manually
        public static void DispatchEvents(AggregateRoot aggregateRoot)
        {
            if (aggregateRoot == null)
                return;

            foreach (IDomainEvent domainEvent in aggregateRoot.DomainEvents)
            {
                DomainEvents.Dispatch(domainEvent);
            }

            aggregateRoot.ClearEvents();
        }
    }
}
