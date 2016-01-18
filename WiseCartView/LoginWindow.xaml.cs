using System.ComponentModel;
using System.Windows;
using WiseCartLogic;

namespace WiseCartView
{
    public partial class LoginWindow
    {
        public UserLoginManager UserLoginManager { get; set; }

        public LoginWindow(UserLoginManager userLoginManager)
        {
            UserLoginManager = userLoginManager;

            InitializeComponent();
        }

        private void SignInButton_Click(object sender, RoutedEventArgs e)
        {
            UserSignInResult result = UserLoginManager.SignIn(UsernameTextBox.Text, PasswordTextBox.Text);

            if (result == UserSignInResult.UsernameAndPasswordCorrect)
            {
                MessageBox.Show(string.Format("Signed in successfully. Welcome {0} !", UsernameTextBox.Text));
                Hide();
            }
            else if(result == UserSignInResult.BadPassword)
            {
                MessageBox.Show("Wrong password.");
            }
            else if (result == UserSignInResult.UsernameNotExists)
            {
                MessageBox.Show("Username does not exist.");
            }
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            UserSignUpResult result = UserLoginManager.SignUp(UsernameTextBox.Text, PasswordTextBox.Text);

            if (result == UserSignUpResult.UsernameAndPasswordGoodCharacters)
            {
                MessageBox.Show(string.Format("Signed up successfully. Welcome {0} !", UsernameTextBox.Text));
                Hide();
            }
            else if (result.HasFlag(UserSignUpResult.UsernameOrPasswordBadCharacters))
            {
                MessageBox.Show("Bad Username or Password. Enter again.");
            }
            else if (result == UserSignUpResult.UsernameAlreadyExists)
            {
                MessageBox.Show(string.Format("The username '{0}' already exists. Please try another.", UsernameTextBox.Text));
            }
        }

        private void UnregisteredButton_Click(object sender, RoutedEventArgs e)
        {
            UserLoginManager.UnregisteredLogin();
            Hide();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            UserLoginManager.UnregisteredLogin();
            e.Cancel = true;
            Hide();
        }
    }
}
