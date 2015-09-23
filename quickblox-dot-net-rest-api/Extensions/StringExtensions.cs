namespace Kaliido.QuickBlox
{
    public static class StringExtensions
    {


        //public static string ByteToString(byte[] buff)
        //{
        //    string sbinary = "";
        //    for (int i = 0; i < buff.Length; i++)
        //        sbinary += buff[i].ToString("x2"); /* hex format  -- **** MUST BE LOWERCASE , dont ask me why*/
        //    return sbinary;
        //}


        public static string ByteToString(this byte[] buff)
        {

            string sbinary = "";
            for (int i = 0; i < buff.Length; i++)
                sbinary += buff[i].ToString("x2"); /* hex format  -- **** MUST BE LOWERCASE , dont ask me why*/
            return sbinary;
        }

    }
}