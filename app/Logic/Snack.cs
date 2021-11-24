namespace DDDCourse.Logic
{
    public class Snack : AggregateRoot
    {
        public static readonly Snack None = new Snack(0, "None");
        public static readonly Snack Chocolate = new Snack(1, "Chocolate");
        public static readonly Snack Soda = new Snack(2, "Soda");
        public static readonly Snack Gum = new Snack(3, "Gum");

        // virtual is only required to make NHibernate work with this entity
        // setter is protected and not private here because NHibernate would need this
        public virtual string Name { get; protected set; }

        // Empty constructor is only required to make NHibernate work with this entity
        protected Snack() { }

        private Snack(long id, string name)
            : this()
        {
            Id = id;
            Name = name;
        }
    }
}