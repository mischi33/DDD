namespace DDDCourse.Logic {
    public static class Initer {
        public static void init(string connectionString) {
            SessionFactory.Init(connectionString);
        }
    }
}