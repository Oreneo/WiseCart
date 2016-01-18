using System.Text.RegularExpressions;
using WiseCartLogic.Entities;

namespace WiseCartLogic
{
    public class UserLoginManager
    {
        public Regex ValidUserNameRegex { get; set; }
        public User LoggedInUser { get; set; }
        public UserDBManager UserDBManager { get; set; }

        public UserLoginManager(UserDBManager userDBManager)
        {
            ValidUserNameRegex = new Regex(@"^(?=[a-zA-Z])[-\w.]{0,23}([a-zA-Z\d]|(?<![-.])_)$");
            LoggedInUser = null;
            UserDBManager = userDBManager;
        }

        public UserSignInResult SignIn(string username, string password)
        {
            UserSignInResult result = UserSignInResult.UsernameNotExists;

            UserDBManager.ReadDB();

            foreach (User user in UserDBManager.UsersDB.Users)
            {
                if (user.Username == username && user.Password == password)
                {
                    LoggedInUser = new User {Username = username, Password = password};
                    result = UserSignInResult.UsernameAndPasswordCorrect;
                    break;
                }
                if (user.Username == username && user.Password != password)
                {
                    result = UserSignInResult.BadPassword;
                    break;
                }
            }
            if ((result != UserSignInResult.UsernameAndPasswordCorrect && result != UserSignInResult.BadPassword) || string.IsNullOrEmpty(username))
            {
                result = UserSignInResult.UsernameNotExists;
            }

            return result;
        }

        public UserSignUpResult SignUp(string username, string password)
        {
            UserSignUpResult result = new UserSignUpResult();

            UserDBManager.ReadDB();

            if (ValidUserNameRegex.IsMatch(username) && !string.IsNullOrEmpty(password))
            {
                foreach (User user in UserDBManager.UsersDB.Users)
                {
                    if (user.Username == username)
                    {
                        result = UserSignUpResult.UsernameAlreadyExists;
                        break;
                    }
                }
            }

            if (!ValidUserNameRegex.IsMatch(username) || string.IsNullOrEmpty(password))
            {
                result = UserSignUpResult.UsernameOrPasswordBadCharacters;
            }

            if (result != UserSignUpResult.UsernameAlreadyExists && result != UserSignUpResult.UsernameOrPasswordBadCharacters)
            {
                UserDBManager.ReadDB();
                LoggedInUser = new User(username, password);

                UserDBManager.UsersDB.Users.Add(LoggedInUser); // should append
                UserDBManager.SaveDB();

                result = UserSignUpResult.UsernameAndPasswordGoodCharacters;
            }

            return result;
        }

        public void UnregisteredLogin()
        {
            LoggedInUser = new User("Guest", string.Empty);
            //if (LoggedInUser == null)
            //{

            //}
        }
    }
}
