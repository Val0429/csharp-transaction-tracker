using System;

namespace HttpHandler
{
    public interface IDispatcher
    {
        String Get(String queryString);
        String Post(String queryString, String param);
        String Post(String queryString, IntPtr handle, int length);
        String User { get; set; }
        //String Password { get; set; }
        String Group { get; set; }
    }
}
