namespace Apache.Infrastructure
{
    public static class StaticFields
    {
        public static Staffs currrentStaff { get; set; } = new();
        public static decimal currentServicePrice { get; set; } = 0m;
    }
}
