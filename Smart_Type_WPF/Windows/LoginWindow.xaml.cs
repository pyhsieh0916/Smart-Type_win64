using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes;
using MaterialDesignColors;
using System.Windows.Forms;

namespace Smart_Type_WPF.Windows
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    /// [STAThreadAttribute]
    public partial class LoginWindow : Window
    {
        bool flag = false;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {

            if (flag)
            {
                Box_LoginAccount.Visibility = Visibility.Visible;
                Box_LoginPassword.Visibility = Visibility.Visible;
                Box_RegisterAccount.Visibility = Visibility.Hidden;
                Box_RegisterPassword_1.Visibility = Visibility.Hidden;
                Box_RegisterPassword_2.Visibility = Visibility.Hidden;
                flag = false;
            }
            else
            {
                PasswordBox box_LoginPassword = new PasswordBox();
                box_LoginPassword.Password = Box_LoginPassword.Password.ToString();
                sql_Login(Box_LoginAccount.Text, box_LoginPassword.Password);
            }
        }

        private void btn_free_Click(object sender, RoutedEventArgs e)
        {
            TypeWindow typewindow = new TypeWindow();
            typewindow.Show();
            this.Close();
        }

        private void btn_register_Click(object sender, RoutedEventArgs e)
        {
            if (!flag)
            {
                Box_LoginAccount.Visibility = Visibility.Hidden;
                Box_LoginPassword.Visibility = Visibility.Hidden;
                Box_RegisterAccount.Visibility = Visibility.Visible;
                Box_RegisterPassword_1.Visibility = Visibility.Visible;
                Box_RegisterPassword_2.Visibility = Visibility.Visible;
                flag = true;
            }
            else
            {
                sql_Register(Box_RegisterAccount.Text, Box_RegisterPassword_1.Password, Box_RegisterPassword_2.Password);
            }
        }
        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // 統一處理 TextBox 的拖曳。 TextBlock\Label 等不影響
            if (e.OriginalSource is System.Windows.Controls.TextBox)
            {
                return;
            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private async void sql_Login(string Account, string password)
        {
            string Password = (string)password;
            string Login = "Login";
            {
                try
                {
                    var httpClient = new HttpClient();
                    var url = new Uri("http://smarttype2023.sytes.net/smart_type/smart_type_sql_Hash_L_R_S.php");
                    var body = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "x_code", Login },
                        { "Account", Account },
                        { "Password", Password },
                });


                    //H_del.PopupHotkeyHandler();
                    HttpResponseMessage response2 = await httpClient.PostAsync(url, body);  //*****
                    string resultstr = await response2.Content.ReadAsStringAsync();
                    if (resultstr == "ok")
                    {
                        Global.Account = Account;
                        Global.Password = Password;
                        TypeWindow typewindow = new TypeWindow();
                        typewindow.Show();
                        this.Close();  
                    }
                    else
                    {
                        Lable_Resultstr.Content = resultstr;
                    }
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private async void sql_Register(string Account, string password, string password2)
        {
            string Password = (string)password;
            string Password2 = (string)password2;
            string Register = "Register";
            if (Password.Equals(Password2))
            {
                try
                {
                    var httpClient = new HttpClient();
                    var url = new Uri("http://smarttype2023.sytes.net/smart_type/smart_type_sql_Hash_L_R_S.php");
                    var body = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "x_code", Register },
                        { "Account", Account },
                        { "Password", Password },
                });
                    HttpResponseMessage response2 = await httpClient.PostAsync(url, body);  //*****
                    string resultstr = await response2.Content.ReadAsStringAsync();
                    if (resultstr == "ok")
                    {
                        Box_LoginAccount.Visibility = Visibility.Visible;
                        Box_LoginPassword.Visibility = Visibility.Visible;
                        Box_RegisterAccount.Visibility = Visibility.Hidden;
                        Box_RegisterPassword_1.Visibility = Visibility.Hidden;
                        Box_RegisterPassword_2.Visibility = Visibility.Hidden;
                        flag = false;
                        Lable_Resultstr.Content = "申請成功！請直接登入。";
                    }
                    else if (resultstr == "oked")
                    {
                        Box_LoginAccount.Visibility = Visibility.Visible;
                        Box_LoginPassword.Visibility = Visibility.Visible;
                        Box_RegisterAccount.Visibility = Visibility.Hidden;
                        Box_RegisterPassword_1.Visibility = Visibility.Hidden;
                        Box_RegisterPassword_2.Visibility = Visibility.Hidden;
                        flag = false;
                        Lable_Resultstr.Content = "帳號已註冊，請直接登入。";
                    }
                    else
                    {
                        Lable_Resultstr.Content = "密碼輸入錯誤，請重新確認。";
                    }
                }
                catch (Exception ex)
                {
                    Lable_Resultstr.Content = "密碼輸入錯誤，請重新確認。";
                }
            }

            else if (!Password.Equals(Password2))
            {
                Lable_Resultstr.Content = "密碼輸入錯誤，請重新確認。";
            }
        }

    }
}
