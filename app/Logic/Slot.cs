namespace DDDCourse.Logic
{
    public class Slot : Entity
    {
        // virtual is only required to make NHibernate work with this entity
        // setter is protected and not private here because NHibernate would need this
        public virtual Snack Snack { get; set; }
        public virtual int Quantity { get; set; }
        public virtual decimal Price { get; set; }
        public virtual SnackMachine SnackMachine { get; protected set; }
        public virtual int Position { get; protected set; }
        
        // Empty constructor is only required to make NHibernate work with this entity
        protected Slot() { }

        public Slot(SnackMachine snackMachine, int position, Snack snack, int quantity, decimal price)
        {
            SnackMachine = snackMachine;
            Position = position;
            Snack = snack;
            Quantity = quantity;
            price = Price;
        }
    }
}