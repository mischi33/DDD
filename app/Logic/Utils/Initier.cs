using DDDCourse.Logic.Common;

namespace DDDCourse.Logic.Utils {
    public static class Initer {
        public static void init(string connectionString) {
            SessionFactory.Init(connectionString);
        }
    }
}