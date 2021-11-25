using DDDCourse.Logic.Common;

namespace DDDCourse.Logic.SnackMachines
{
    public class Slot : Entity
    {
        // virtual is only required to make NHibernate work with this entity
        // setter is protected and not private here because NHibernate would need this
        public virtual SnackPile SnackPile { get; set; }
        public virtual SnackMachine SnackMachine { get; protected set; }
        public virtual int Position { get; protected set; }

        // Empty constructor is only required to make NHibernate work with this entity
        protected Slot() { }

        public Slot(SnackMachine snackMachine, int position)
        {
            SnackMachine = snackMachine;
            Position = position;
            SnackPile = SnackPile.Empty;
        }
    }
}