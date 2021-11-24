namespace DDDCourse.Logic
{
    public class Snack : Entity
    {
        // virtual is only required to make NHibernate work with this entity
        // setter is protected and not private here because NHibernate would need this
        public virtual string Name { get; protected set; }

        // Empty constructor is only required to make NHibernate work with this entity
        protected Snack() { }

        public Snack(string name) : this()
        {
            Name = name;
        }
    }
}