namespace UserManagement.Utils
{
    public class response<T>
    {
        public string Message { get; set; }
        public string Code { get; set; }
        public T Data { get; set; }
    }

    public class AuthToken
    {
        public string Token { set; get; }
        public string ExpireAt { set; get; }
        public string RefreshToken { set; get; }
    }

    public class StatsCodes
    {
        public static string SUCCESS = "200";
        public static string FAILED = "01";
        public static string UNAUTHOURIZED = "401";
    }
}
