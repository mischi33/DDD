using FluentNHibernate.Mapping;

namespace DDDCourse.Logic.SnackMachines.Db
{
    public class SnackMap : ClassMap<Snack>
    {
        public SnackMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
        }
    }
}
