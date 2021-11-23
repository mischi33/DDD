using NHibernate.Proxy;

namespace DDDCourse.Logic
{
    public abstract class Entity
    {
        public virtual long Id { get; protected set; }

        public override bool Equals(object? obj)
        {
            var other = obj as Entity;

            // check for reference equality
            if (ReferenceEquals(other, null)) return false;
            if (ReferenceEquals(this, other)) return true;

            // check for type equality
            if (GetRealType() != other.GetRealType()) return false;

            // check for identifier equality
            if (Id == 0 || other.Id == 0) return false;
            return Id == other.Id;
        }

        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(a, null) && ReferenceEquals(b, null)) return true;
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;
            return a.Equals(b);
        }

        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return (GetRealType().ToString() + Id).GetHashCode();
        }

        private Type GetRealType() {
            return NHibernateProxyHelper.GetClassWithoutInitializingProxy(this);
        }
    }
}