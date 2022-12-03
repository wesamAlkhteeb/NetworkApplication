using System.Text.RegularExpressions;

namespace NAPApi.Help
{
    public class ValidateInputHelper
    {
        private static ValidateInputHelper validateInput;

        private ValidateInputHelper()
        {
        }

        public static ValidateInputHelper getInstance()
        {
            if (validateInput == null)
            {
                validateInput = new ValidateInputHelper();
            }
            return validateInput;
        }


        // validate email
        public bool isValidEmail(String email)
        {
            Regex regex = new Regex("^[a-zA-Z0-9_]+@(gmail|hotmail)\\.(com)$");
            return regex.IsMatch(email);
        }
        //valid password
        public bool isValidPassword(String password)
        {
            return new Regex("[a-z]").IsMatch(password) &&
                    new Regex("[A-Z]").IsMatch(password) &&
                    new Regex("[0-9]").IsMatch(password) &&
                    new Regex("[^A-Za-z0-9]").IsMatch(password) ;
        }




    }
}
