namespace DDDCourse.Logic.Management.Db
{
    public static class HeadOfficeInstance
    {
        private const long HeadOfficeId = 1;

        public static HeadOffice Instance { get; private set; }

        public static void Init()
        {
            // var repository = new HeadOfficeRepository();
            // Instance = repository.GetById(HeadOfficeId);
            Instance = new HeadOffice();
        }
    }
}
