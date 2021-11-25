using DDDCourse.Logic.Common;
using DDDCourse.Logic.Management.Db;

namespace DDDCourse.Logic.Utils
{
    public static class Initer
    {
        public static void init(string connectionString)
        {
            // SessionFactory.Init(connectionString);
            HeadOfficeInstance.Init();
            DomainEvents.Init();
        }
    }
}