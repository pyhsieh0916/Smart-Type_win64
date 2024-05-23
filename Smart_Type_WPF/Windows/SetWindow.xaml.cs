using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MaterialDesignThemes;
using MaterialDesignColors;
using System.Windows.Forms;
using System.Web.UI.WebControls;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace Smart_Type_WPF.Windows
{
    public partial class SetWindow : System.Windows.Window
    {
        private List<string> sql_back_String = new List<string>();
        string[] strings;
        string String = "";
        public SetWindow()
        {
            InitializeComponent();
        }

        private void btn_Password_Selected(object sender, RoutedEventArgs e)
        {
            if (Border_Password.Visibility == Visibility.Hidden)
            {
                Border_Password.Visibility = Visibility.Visible;
                Border_String.Visibility = Visibility.Hidden;
                Border_Welcome.Visibility = Visibility.Hidden;
            }
            else
            {
                Border_Password.Visibility = Visibility.Hidden;
                Border_String.Visibility = Visibility.Hidden;
                Border_Welcome.Visibility = Visibility.Visible;
            }
        }

        private void btn_String_Selected(object sender, RoutedEventArgs e)
        {
            if (Border_String.Visibility == Visibility.Hidden)
            {
                Border_Password.Visibility = Visibility.Hidden;
                Border_String.Visibility = Visibility.Visible;
                Border_Welcome.Visibility = Visibility.Hidden;
                sql_SearchString(Global.Account);
            }
            else
            {
                Border_Password.Visibility = Visibility.Hidden;
                Border_String.Visibility = Visibility.Hidden;
                Border_Welcome.Visibility = Visibility.Visible;
            }
        }

        private void nameListBox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            String = nameListBox.SelectedItem.ToString();
            String_set.Text = nameListBox.SelectedItem.ToString();
            btn_set.Visibility = Visibility.Visible;
        }

        private void btn_UpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            string Password = (string)Box_Password.Password;
            string Password_New = (string)Box_Password_New.Password;
            string Password_New2 = (string)Box_Password_New2.Password;
            if (!Password_New.Equals(Password_New2) || Global.Password.Equals(Password_New) || !Global.Password.Equals(Password))
            {
                System.Windows.MessageBox.Show("修改失敗，請重新嘗試", "提示");
            }
            else if (Global.Account.Equals(Password_New))
            {
                System.Windows.MessageBox.Show("帳號與密碼不可相同，請重新嘗試", "提示");
            }
            else
            {
                sql_UpdatePassword(Global.Account, Password_New);
            }
        }

        private void btn_add_Click(object sender, RoutedEventArgs e)
        {
            if (String_set.Text.Equals(""))
            {
                String_set.FontWeight = FontWeights.Bold;
            }
            else if (strings.Length < 6)
            {
                sql_AddString(Global.Account, String_set.Text);
            }
            else
            {
                System.Windows.MessageBox.Show("最多新增 6 個常用字串", "提示");
            }
        }

        private void btn_set_Click(object sender, RoutedEventArgs e)
        {
            if (String_set.Text.Equals(""))
            {
                String_set.FontWeight = FontWeights.Bold;
            }
            else
            {
                sql_SetString(Global.Account, String, String_set.Text);
            }
        }

        private void Window_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {            // 統一處理 TextBox 的拖曳。 TextBlock\Label 等不影響
            if (e.OriginalSource is System.Windows.Controls.TextBox)
            {
                return;
            }
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void sql_SearchString(string Account)
        {
            {
                try
                {
                    var httpClient = new HttpClient();
                    var url = new Uri("http://smarttype2023.sytes.net/smart_type/smart_type_sql_SearchString.php");
                    var body = new FormUrlEncodedContent(new Dictionary<string, string>
                        {
                        { "Account", Account },
                });

                    var response = httpClient.PostAsync(url, body).Result;
                    var data = response.Content.ReadAsStringAsync().Result;
                    string del_head = data.Substring(10);  //刪開頭 {"ch_1":[["
                    string del_left = del_head.Replace("[", "");
                    string del_right = del_left.Replace("]", "");
                    string del_right_ = del_right.Replace("}", "");
                    string del_char = del_right_.Replace("\"", "");
                    strings = del_char.Split(new char[] { ',' });

                    sql_back_String.Clear();
                    sql_back_String = new List<string>(strings);
                    if (strings != null && strings.Any())
                    {
                        this.nameListBox.Items.Clear();
                        for (int i = 0; i < 8; i++)
                        {
                            this.nameListBox.Items.Add(sql_back_String[i]);
                        }
                    }
                    //else
                    //pageIndex--;
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        private async void sql_AddString(string Account, string String_Set)
        {
            try
            {
                var httpClient = new HttpClient();
                var url = new Uri("http://smarttype2023.sytes.net/smart_type/smart_type_sql_AddString.php");
                var body = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "Account", Account },
                        { "String", String_Set },
                });


                //H_del.PopupHotkeyHandler();
                HttpResponseMessage response2 = await httpClient.PostAsync(url, body);  //*****
                string resultstr = await response2.Content.ReadAsStringAsync();
                if (resultstr == "ok")
                {
                    sql_SearchString(Global.Account);
                    String_set.Text = "";
                }
                else
                {
                    System.Windows.MessageBox.Show("新增失敗，請重新嘗試", "提示");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("新增失敗，請重新嘗試", "系统提示");
                // Show any error message.
                //MessageBox.Show(ex.Message);
            }

        }

        private async void sql_SetString(string Account, string String, string String_Set)
        {
                try
                {
                    var httpClient = new HttpClient();
                    var url = new Uri("http://smarttype2023.sytes.net/smart_type/smart_type_sql_SetString.php");
                    var body = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "Account", Account },
                        { "String", String },
                        { "Update", String_Set },
                });


                    //H_del.PopupHotkeyHandler();
                    HttpResponseMessage response2 = await httpClient.PostAsync(url, body);  //*****
                    string resultstr = await response2.Content.ReadAsStringAsync();
                    if (resultstr == "ok")
                    {
                        sql_SearchString(Global.Account);
                        btn_set.Visibility = Visibility.Hidden;
                        String_set.Text = "";
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("修改失敗，請重新嘗試", "提示");
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("修改失敗，請重新嘗試", "系统提示");
                    // Show any error message.
                    //MessageBox.Show(ex.Message);
                }

        }

        private async void sql_UpdatePassword(string Account, string Password_New)
        {
            string UpdatePassword = "UpdatePassword";
            try
            {
                var httpClient = new HttpClient();
                var url = new Uri("http://smarttype2023.sytes.net/smart_type/smart_type_sql_Hash_L_R_S.php");
                var body = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "x_code", UpdatePassword },
                    { "Account", Account },
                    { "Password", Password_New }
                });

                HttpResponseMessage response2 = await httpClient.PostAsync(url, body);
                string resultstr = await response2.Content.ReadAsStringAsync();
                if (resultstr == "ok")
                {
                    Global.Password = Password_New;
                    System.Windows.MessageBox.Show("更新成功！", "提示");
                    Box_Password.Clear();
                    Box_Password_New.Clear();
                    Box_Password_New2.Clear();
                }
                else
                {
                    System.Windows.MessageBox.Show("修改失敗，請重新嘗試。", "提示");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("修改失敗，請重新嘗試", "系统提示");
                // Show any error message.
                //MessageBox.Show(ex.Message);
            }
        }
    }
}
