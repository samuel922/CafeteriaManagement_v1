namespace CafeteriaManagement.Utilities
{
    public static class GenerateOrderNo
    {
        public static string GenerateOrderNumber()
        {
            string timeFormat = DateTime.Now.ToString("HHmmss");
            return $"ORD{timeFormat}";
        }
    }
}
