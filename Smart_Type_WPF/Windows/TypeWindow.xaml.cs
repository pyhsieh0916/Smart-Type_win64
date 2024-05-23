using ControlzEx.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Resources;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Smart_Type_WPF.Hot;
using System.Windows.Forms;
using KeyEventHandler = System.Windows.Forms.KeyEventHandler;
using Microsoft.Win32;  //寫入注冊表時要用到
using MySql.Data.MySqlClient;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Net.Http;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using System.Security.Policy;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;
using System.Web.UI.WebControls;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using System.Windows.Threading;
using MS.Internal.WindowsBase;
using System.Security;
using System.Threading;
using Control = System.Windows.Controls.Control;
using MaterialDesignThemes;
using MaterialDesignColors;
using MessageBox = System.Windows.Forms.MessageBox;
using Org.BouncyCastle.Utilities;
using MySqlX.XDevAPI;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Principal;
using System.Windows.Markup;
using Microsoft.Extensions.Options;
using System.Runtime.CompilerServices;
using System.IO;
using Application = System.Windows.Application;

namespace Smart_Type_WPF.Windows
{
    public class SqlDBCol
    {
        public int ch_num { set;get;}
        public string ch_1 { set; get; }
        public string ch_2 { set; get; }
        public string ch_3 { set; get; }
        public string ch_4 { set; get; }
        public int user_use { set; get; }

        public SqlDBCol()
        {

        }
    }

    public partial class TypeWindow : Window
    {
        public List<SqlDBCol> g_DB_DATA = new List<SqlDBCol>();
        private List<char> g_Datas = new List<char>();
        private List<string> sql_back = new List<string>();
        private List<string> sql_num = new List<string>();
        private object _object = new object();
        //Thread
        Thread Sqldownload = null;

        //HotkeyDelegates H_del = new HotkeyDelegates();
        Dictionary_spell D_s = new Dictionary_spell();
        public static KeyboardHook k_hook = new KeyboardHook();

        System.Windows.Forms.NotifyIcon nIcon = new System.Windows.Forms.NotifyIcon();
        System.Windows.Forms.ContextMenu nIconMenu = new System.Windows.Forms.ContextMenu();
        System.Windows.Forms.MenuItem informations = new System.Windows.Forms.MenuItem();
        System.Windows.Forms.MenuItem setting = new System.Windows.Forms.MenuItem();
        System.Windows.Forms.MenuItem close = new System.Windows.Forms.MenuItem();
        
        private string Show = "";
        private string Spell = "";

        int x = 0, y = 0;
        int z = 0;
        int pageIndex = 1;

        bool flag_keyboard = true; 
        
        public TypeWindow()
        {
            InitializeComponent(); 
            this.Topmost = true;

            Sqldownload = new Thread(QuerySql);
            Sqldownload.IsBackground = true;
            Sqldownload.Start();

            Loading loading = new Loading();
            loading.Show();

            while (g_DB_DATA.Count==0){ }

            loading.Close();

            Program_.k_hook.KeyDownEvent += new KeyEventHandler(hook_KeyDown);   //鉤住鍵按下
            Program_.k_hook.Start(); //安裝鍵盤鉤子

            NotifyIcon();

        }

        private void NotifyIcon()
        {
            try
            {
                nIcon.Icon = new Icon(Application.GetResourceStream(new Uri("pack://application:,,,/Windows/st.ico")).Stream);
                nIcon.Text = "Smart Type";
                nIcon.Visible = true;
                nIcon.DoubleClick += new System.EventHandler(NotifyIcon1_Click);
                informations.Index = 0;
                informations.Text = "相關資訊";
                informations.Click += new System.EventHandler(informations_Click);
                nIconMenu.MenuItems.Add(informations);

                if (Global.Account != null)
                {
                    setting.Index = 1;
                    setting.Text = "設定";
                    setting.Click += new System.EventHandler(setting_Click);
                    nIconMenu.MenuItems.Add(setting);

                    //btn_heart.;
                }

                close.Index = 2;
                close.Text = "關閉程式";
                close.Click += new System.EventHandler(close_Click);
                nIconMenu.MenuItems.Add(close);

                nIcon.ContextMenu = nIconMenu;
            }
            catch (Exception ex)
            {
                // 處理例外狀況，例如顯示錯誤訊息或記錄日誌
                //MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        #region " Thread "
        private void QuerySql()
        {
            while(true)
            {
                Excute_Down_DB();
                Thread.Sleep(20000);
            }
        }

        private async void Excute_Down_DB()
        {
            try
            {
                var httpClient = new HttpClient();
                var url = new Uri("http://smarttype2023.sytes.net/smart_type/smart_type_sql_Search_db.php");
                HttpResponseMessage response2 = await httpClient.GetAsync(url);
                string resultstr = await response2.Content.ReadAsStringAsync();
                List<SqlDBCol> a = JsonConvert.DeserializeObject<List<SqlDBCol>>(resultstr);
                lock(_object)
                {
                    g_DB_DATA = a.ToList();
                }
            }
            catch
            {
                
            }
        }
        #endregion

        private void NotifyIcon1_Click(object sender, System.EventArgs e)
        {
            Type_Window.Visibility = Visibility.Hidden;
            if (this.WindowState == WindowState.Minimized)
            {
                this.Show();
                this.WindowState = WindowState.Normal;
            }
            else if (this.WindowState != WindowState.Minimized)
            {
                this.Hide();
                //this.WindowState = FormWindowState.Normal;
                this.WindowState = WindowState.Minimized;
            }
        }

        private void informations_Click(object sender, System.EventArgs e)
        {
            MessageBox.Show("Smart_Type\n版本：s6.0.1");
        }

        private void setting_Click(object sender, System.EventArgs e)
        {
            SetWindow setWindow = new SetWindow();
            if (setWindow.WindowState == WindowState.Minimized)
            {
                setWindow.Show();
            }
            else if (setWindow.WindowState != WindowState.Minimized)
            {
                setWindow.Hide();
            }
            setWindow.Show();
        }

        private void close_Click(object sender, System.EventArgs e)
        {
            nIcon.Dispose();
            System.Windows.Application.Current.Shutdown(); 
        }

        private void Print()
        {
            Spell = "";
            Show = "";
            foreach (char s in g_Datas)
            {
                Show += s;
                Spell += D_s.spell_Data[s];
            }
            lblInput.Content = Show;
            lblspell.Text = Spell;

            sql_Search();
        }

        private void KeyBordHook_OnPaged(int obj)
        {
            pageIndex = pageIndex + obj;
            if (pageIndex < 1)
                pageIndex = 1;
            sql_Search();

        }


        private void txtShow_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtShow.Text.Length > 0)
            {
                char[] charArr = txtShow.Text.ToCharArray();
                g_Datas = charArr.ToList();
                Dispatcher.BeginInvoke(new Action(() => Print()));
            }
            else
            {
                txtShow.Text = null;
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

        private void btn_heart_Click(object sender, RoutedEventArgs e)
        {
            sql_SearchString(Global.Account);
        }

        private void btn_stop_Click(object sender, RoutedEventArgs e)
        {
            if (flag_keyboard)
            {
                k_hook.Stop();
                flag_keyboard = false;
                btn_stop.ToolTip = "繼續使用";
                btn_pop_kind.Kind = MaterialDesignThemes.Wpf.PackIconKind.BlockHelper;
            }
            else
            {
                Program_.k_hook.Start(); //安裝鍵盤鉤子
                flag_keyboard = true;
                btn_stop.ToolTip = "暫停使用";
                btn_pop_kind.Kind = MaterialDesignThemes.Wpf.PackIconKind.CogOutline;
            }
        }

        private void sql_Search()
        {
            {
                try
                {
                    //https://blog.csdn.net/BADAO_LIUMANG_QIZHI/article/details/124157296

                    var httpClient = new HttpClient();
                    //var url = new Uri("http://smarttype2023.sytes.net/smart_type/smart_type_sql_Search.php");
                    //var body = new FormUrlEncodedContent(new Dictionary<string, string> { { "Spell", this.Spell }, });
                    //// response  ch_1
                    //var response = httpClient.PostAsync(url, body).Result;
                    //var data = response.Content.ReadAsStringAsync().Result;
                    //string del_head = data.Substring(10);  //刪開頭 {"ch_1":[["
                    //string del_left = del_head.Replace("[", "");
                    //string del_right = del_left.Replace("]", "");
                    //string del_right_ = del_right.Replace("}", "");
                    //string del_char = del_right_.Replace("\"", "");
                    //string[] strings = del_char.Split(new char[] { ',' });
                    string[] strings = null;
                    List<SqlDBCol> temp = new List<SqlDBCol>();
                    for(int i = 0; i < 3; i++)
                    {
                        if (i == 0)
                        {
                            temp = g_DB_DATA.FindAll(var => var.ch_2 == this.Spell);
                            if (temp != null && temp.Count > 0)
                            {
                                sql_back.Clear();
                                sql_num.Clear();
                                foreach (SqlDBCol item in temp)
                                {
                                    sql_back.Add(item.ch_1.ToString());
                                    sql_num.Add(item.ch_num.ToString());
                                }
                                break;
                            }
                        }
                        else if (i==1)
                        {
                            temp = g_DB_DATA.FindAll(var => var.ch_3 == this.Spell);
                            if (temp != null && temp.Count > 0)
                            {
                                sql_back.Clear();
                                sql_num.Clear();
                                foreach (SqlDBCol item in temp)
                                {
                                    sql_back.Add(item.ch_1.ToString());
                                    sql_num.Add(item.ch_num.ToString());
                                }
                                break;
                            }
                        }
                        else if (i == 2)
                        {
                            temp = g_DB_DATA.FindAll(var => var.ch_4 == this.Spell);
                            if (temp != null && temp.Count > 0)
                            {
                                sql_back.Clear();
                                sql_num.Clear();
                                foreach (SqlDBCol item in temp)
                                {
                                    sql_back.Add(item.ch_1.ToString());
                                    sql_num.Add(item.ch_num.ToString());
                                }
                                break;
                            }
                        }
                    }

                    //sql_back.Clear();
                    //sql_back = new List<string>(strings);

                    if (sql_back != null && sql_back.Count > 0)
                    {
                        this.listView1.Items.Clear();
                        for (int i = 0; i < 8; i++)
                        {
                            this.listView1.Items.Add((i + 1) + "." + sql_back[i]);
                        }
                    }
                    else
                        pageIndex--;

                    //Num
                    //var ur2 = new Uri("http://smarttype2023.sytes.net/smart_type/smart_type_sql_Num.php");
                    //var body2 = new FormUrlEncodedContent(new Dictionary<string, string> { { "Spell", this.Spell }, });
                    //var response2 = httpClient.PostAsync(ur2, body2).Result;
                    //var data2 = response2.Content.ReadAsStringAsync().Result;
                    //string del_head2 = data2.Substring(13);  //刪開頭 {"ch_num":[["
                    //string del_left2 = del_head2.Replace("[", "");
                    //string del_right2 = del_left2.Replace("]", "");
                    //string del_right_2 = del_right2.Replace("}", "");
                    //string del_char2 = del_right_2.Replace("\"", "");
                    //string[] strings2 = del_char2.Split(new char[] { ',' });
                    //sql_num.Clear();
                    //sql_num = new List<string>(strings2);
                    //H_del.PopupHotkeyHandler();
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    //MessageBox.Show(ex.Message);
                }
            }
        }
        private async void sql_Update(int i)
        {
            string Update = sql_num[i];
            {
                try
                {
                    var httpClient = new HttpClient();
                    var url = new Uri("http://smarttype2023.sytes.net/smart_type/smart_type_sql_Update.php");
                    var body = new FormUrlEncodedContent(new Dictionary<string, string>
                    { { "Update", Update } });
                    //H_del.PopupHotkeyHandler();
                    HttpResponseMessage response2 = await httpClient.PostAsync(url, body);
                    string resultstr = await response2.Content.ReadAsStringAsync();
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("失敗，請重新嘗試", "系统提示");
                }
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
                    string[] strings = del_char.Split(new char[] { ',' });
                    sql_back.Clear();
                    sql_back = new List<string>(strings);

                    if (strings != null && strings.Any() && Global.Account!=null)
                    {
                        this.listView1.Items.Clear();
                        for (int i = 0; i < 8; i++)
                        {
                            this.listView1.Items.Add((i + 1) + "." + sql_back[i]);
                        }
                    }
                    else
                        pageIndex--;
                }
                catch (Exception ex)
                {
                    // Show any error message.
                    //MessageBox.Show(ex.Message);
                }
            }
        }

        int num_choice;

        //keys列單:https://learn.microsoft.com/zh-tw/dotnet/api/system.windows.forms.keys?view=windowsdesktop-7.0
        private void hook_KeyDown(object sender, KeyEventArgs e)
        {
            if (!Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(() => hook_KeyDown(sender, e)));
            }
            else
            {
                //3.判斷輸入鍵值（實現KeyDown事件）

                //注意幾種不同的鍵值判斷：
                //1>.單普通鍵（例如A）
                //2>.單控制鍵+單普通鍵（例如Ctrl+A）
                //3>.多控制鍵+單普通鍵（例如Ctrl+Alt+A）
                //判斷按下的鍵（Alt + A）
                //(e.KeyCode == Keys.Oemtilde && (e.Control))
                //(e.KeyValue == Keys.Control && e.KeyValue == (int)Keys.Oemtilde)
                //(e.KeyCode == Keys.F1 && (e.Alt || e.Control || e.Shift))
                //e.KeyCode == Keys.LControlKey


                if (e.KeyCode == Keys.Oemtilde && (e.Control))
                {
                    Program_.k_hook.Start();
                }
                else if (e.KeyValue == (int)Keys.LShiftKey ) //(int)Keys.D1 && (int)Control.ModifierKeys == (int)Keys.Alt)
                {
                    //    if (flag_keyboard)
                    //    {
                    //        k_hook.Stop();
                    //        flag_keyboard = false;
                    //        btn_stop.ToolTip = "繼續使用";
                    //        btn_pop_kind.Kind = MaterialDesignThemes.Wpf.PackIconKind.CogOutline;
                    //    }
                    //    else
                    //    {
                    //        Program_.k_hook.Start(); //安裝鍵盤鉤子
                    //        flag_keyboard = true;
                    //        btn_stop.ToolTip = "暫停使用";
                    //        btn_pop_kind.Kind = MaterialDesignThemes.Wpf.PackIconKind.BlockHelper;
                    //    }
                }

                else if (e.KeyValue == (int)Keys.PageDown) // PageDown
                {
                    z++;
                    this.listView1.Items.Clear();
                    for (int i = 0; i < 8; i++)
                    {
                        this.listView1.Items.Add((i + 1) + "." + sql_back[i + z * 8]);
                    }
                }

                else if (e.KeyValue == (int)Keys.PageUp) // PageUp
                {
                    if (z != 0)
                    {
                        z--;
                        this.listView1.Items.Clear();
                        for (int i = 0; i < 8; i++)
                        {
                            this.listView1.Items.Add((i + 1) + "." + sql_back[i + z * 8]);
                        }
                    }
                }

                else if (e.KeyValue == (int)Keys.Back)
                {
                    if (txtShow.Text.Length > 1)
                    {
                        txtShow.Text = txtShow.Text.Substring(0, txtShow.Text.Length - 1);
                    }
                    else if (txtShow.Text.Length > 0)
                    {
                        txtShow.Text = txtShow.Text.Substring(0, txtShow.Text.Length - 1);
                        txtShow.Text = "";
                        lblInput.Content = "";
                        lblspell.Text = "";
                        this.listView1.Items.Clear();
                        //pick_min.Visible = false;
                    }
                    else
                    {
                        txtShow.Text = "";
                        lblInput.Content = "";
                        lblspell.Text = "";
                        this.listView1.Items.Clear();
                    }
                }

                else if (e.KeyValue == (int)Keys.NumPad1)
                {
                    //if (sql_back != null && sql_back.Count == 0)
                    //    return;

                    num_choice = z * 8;
                    String temp = sql_back[num_choice];
                    Program_.k_hook.Send(temp);  //丟上選擇的

                    if (sql_back[num_choice].Length == lblspell.Text.Length || lblspell.Text.Length==0)
                    {
                        txtShow.Text = "";
                        lblInput.Content = "";
                        lblspell.Text = "";
                        this.listView1.Items.Clear();
                    }
                    else
                    {
                        lblspell.Text = lblspell.Text.Substring(sql_back[num_choice].Length);
                        txtShow.Text = lblspell.Text;
                        Show = Show.Substring(sql_back[num_choice].Length);
                    }
                    sql_Update(num_choice);
                }
                else if (e.KeyValue == (int)Keys.NumPad2)
                {
                    if (sql_back != null && sql_back.Count <= 1 )
                        return;
                    num_choice = z * 8+1;
                    String temp = sql_back[num_choice];
                    Program_.k_hook.Send(temp);  //丟上選擇的

                    if (sql_back[num_choice].Length == lblspell.Text.Length || lblspell.Text.Length == 0)
                    {
                        txtShow.Text = "";
                        lblInput.Content = "";
                        lblspell.Text = "";
                        this.listView1.Items.Clear();
                    }
                    else
                    {
                        lblspell.Text = lblspell.Text.Substring(sql_back[num_choice].Length);
                        txtShow.Text = lblspell.Text;
                        Show = Show.Substring(sql_back[num_choice].Length);
                    }
                    sql_Update(num_choice);
                }
                else if (e.KeyValue == (int)Keys.NumPad3)
                {
                    if (sql_back != null && sql_back.Count <= 2)
                        return;
                    num_choice = z * 8+2;
                    String temp = sql_back[num_choice];
                    Program_.k_hook.Send(temp);  //丟上選擇的

                    if (sql_back[num_choice].Length == lblspell.Text.Length || lblspell.Text.Length == 0)
                    {
                        txtShow.Text = "";
                        lblInput.Content = "";
                        lblspell.Text = "";
                        this.listView1.Items.Clear();
                    }
                    else
                    {
                        lblspell.Text = lblspell.Text.Substring(sql_back[num_choice].Length);
                        txtShow.Text = lblspell.Text;
                        Show = Show.Substring(sql_back[num_choice].Length);
                    }
                    sql_Update(num_choice);
                }
                else if (e.KeyValue == (int)Keys.NumPad4)
                {
                    if (sql_back != null && sql_back.Count <= 3)
                        return;
                    num_choice = z * 8+3;
                    String temp = sql_back[num_choice];
                    Program_.k_hook.Send(temp);  //丟上選擇的

                    if (sql_back[num_choice].Length == lblspell.Text.Length || lblspell.Text.Length == 0)
                    {
                        txtShow.Text = "";
                        lblInput.Content = "";
                        lblspell.Text = "";
                        this.listView1.Items.Clear();
                    }
                    else
                    {
                        lblspell.Text = lblspell.Text.Substring(sql_back[num_choice].Length);
                        txtShow.Text = lblspell.Text;
                        Show = Show.Substring(sql_back[num_choice].Length);
                    }
                    sql_Update(num_choice);
                }
                else if (e.KeyValue == (int)Keys.NumPad5)
                {
                    if (sql_back != null && sql_back.Count <= 4)
                        return;
                    num_choice = z * 8+4;
                    String temp = sql_back[num_choice];
                    Program_.k_hook.Send(temp);  //丟上選擇的

                    if (sql_back[num_choice].Length == lblspell.Text.Length || lblspell.Text.Length == 0)
                    {
                        txtShow.Text = "";
                        lblInput.Content = "";
                        lblspell.Text = "";
                        this.listView1.Items.Clear();
                    }
                    else
                    {
                        lblspell.Text = lblspell.Text.Substring(sql_back[num_choice].Length);
                        txtShow.Text = lblspell.Text;
                        Show = Show.Substring(sql_back[num_choice].Length);
                    }
                    sql_Update(num_choice);
                }
                else if (e.KeyValue == (int)Keys.NumPad6)
                {
                    if (sql_back != null && sql_back.Count <= 5)
                        return;
                    num_choice = z * 8+5;
                    String temp = sql_back[num_choice];
                    Program_.k_hook.Send(temp);  //丟上選擇的

                    if (sql_back[num_choice].Length == lblspell.Text.Length || lblspell.Text.Length == 0)
                    {
                        txtShow.Text = "";
                        lblInput.Content = "";
                        lblspell.Text = "";
                        this.listView1.Items.Clear();
                    }
                    else
                    {
                        lblspell.Text = lblspell.Text.Substring(sql_back[num_choice].Length);
                        txtShow.Text = lblspell.Text;
                        Show = Show.Substring(sql_back[num_choice].Length);
                    }
                    sql_Update(num_choice);
                }
                else if (e.KeyValue == (int)Keys.NumPad7)
                {
                    if (sql_back != null && sql_back.Count <= 6)
                        return;
                    num_choice = z * 8+6;
                    String temp = sql_back[num_choice];
                    Program_.k_hook.Send(temp);  //丟上選擇的

                    if (sql_back[num_choice].Length == lblspell.Text.Length || lblspell.Text.Length == 0)
                    {
                        txtShow.Text = "";
                        lblInput.Content = "";
                        lblspell.Text = "";
                        this.listView1.Items.Clear();
                    }
                    else
                    {
                        lblspell.Text = lblspell.Text.Substring(sql_back[num_choice].Length);
                        txtShow.Text = lblspell.Text;
                        Show = Show.Substring(sql_back[num_choice].Length);
                    }
                    sql_Update(num_choice);
                }
                else if (e.KeyValue == (int)Keys.NumPad8)
                {
                    if (sql_back != null && sql_back.Count <= 7)
                        return;
                    num_choice = z * 8+7;
                    String temp = sql_back[num_choice];
                    Program_.k_hook.Send(temp);  //丟上選擇的

                    if (sql_back[num_choice].Length == lblspell.Text.Length || lblspell.Text.Length == 0)
                    {
                        txtShow.Text = "";
                        lblInput.Content = "";
                        lblspell.Text = "";
                        this.listView1.Items.Clear();
                    }
                    else
                    {
                        lblspell.Text = lblspell.Text.Substring(sql_back[num_choice].Length);
                        txtShow.Text = lblspell.Text;
                        Show = Show.Substring(sql_back[num_choice].Length);
                    }
                    sql_Update(num_choice);
                }


                //

                else if (e.KeyValue == (int)Keys.D1)
                {
                    // System.Windows.Forms.MessageBox.Show("按下了指定快捷鍵組合");
                    // panel1.Visible = true;
                    txtShow.AppendText("1");
                }
                else if (e.KeyValue == (int)Keys.D2)
                {
                    txtShow.AppendText("2");
                }
                else if (e.KeyValue == (int)Keys.D3)
                {
                    txtShow.AppendText("3");
                }
                else if (e.KeyValue == (int)Keys.D4)
                {
                    txtShow.AppendText("4");
                }
                else if (e.KeyValue == (int)Keys.D5)
                {
                    txtShow.AppendText("5");
                }
                else if (e.KeyValue == (int)Keys.D6)
                {
                    txtShow.AppendText("6");
                }
                else if (e.KeyValue == (int)Keys.D7)
                {
                    txtShow.AppendText("7");
                }
                else if (e.KeyValue == (int)Keys.D8)
                {
                    txtShow.AppendText("8");
                }
                else if (e.KeyValue == (int)Keys.D9)
                {
                    txtShow.AppendText("9");
                }
                else if (e.KeyValue == (int)Keys.D0)
                {
                    txtShow.AppendText("0");
                }
                else if (e.KeyValue == (int)Keys.Q)
                {
                    txtShow.AppendText("q");
                }
                else if (e.KeyValue == (int)Keys.A)
                {
                    txtShow.AppendText("a");
                }
                else if (e.KeyValue == (int)Keys.B)
                {
                    txtShow.AppendText("b");
                }
                else if (e.KeyValue == (int)Keys.Z)
                {
                    txtShow.AppendText("z");
                }
                else if (e.KeyValue == (int)Keys.W)
                {
                    txtShow.AppendText("w");
                }
                else if (e.KeyValue == (int)Keys.S)
                {
                    txtShow.AppendText("s");
                }
                else if (e.KeyValue == (int)Keys.X)
                {
                    txtShow.AppendText("x");
                }
                else if (e.KeyValue == (int)Keys.E)
                {
                    txtShow.AppendText("e");
                }
                else if (e.KeyValue == (int)Keys.D)
                {
                    txtShow.AppendText("d");
                }
                else if (e.KeyValue == (int)Keys.C)
                {
                    txtShow.AppendText("c");
                }
                else if (e.KeyValue == (int)Keys.R)
                {
                    txtShow.AppendText("r");
                }
                else if (e.KeyValue == (int)Keys.F)
                {
                    txtShow.AppendText("f");
                }
                else if (e.KeyValue == (int)Keys.V)
                {
                    txtShow.AppendText("v");
                }
                else if (e.KeyValue == (int)Keys.T)
                {
                    txtShow.AppendText("t");
                }
                else if (e.KeyValue == (int)Keys.G)
                {
                    txtShow.AppendText("g");
                }
                else if (e.KeyValue == (int)Keys.V)
                {
                    txtShow.AppendText("v");
                }
                else if (e.KeyValue == (int)Keys.Y)
                {
                    txtShow.AppendText("y");
                }
                else if (e.KeyValue == (int)Keys.H)
                {
                    txtShow.AppendText("h");
                }
                else if (e.KeyValue == (int)Keys.N)
                {
                    txtShow.AppendText("n");
                }
                else if (e.KeyValue == (int)Keys.U)
                {
                    txtShow.AppendText("u");
                }
                else if (e.KeyValue == (int)Keys.J)
                {
                    txtShow.AppendText("j");
                }
                else if (e.KeyValue == (int)Keys.M)
                {
                    txtShow.AppendText("m");
                }
                else if (e.KeyValue == (int)Keys.I)
                {
                    txtShow.AppendText("i");
                }
                else if (e.KeyValue == (int)Keys.K)
                {
                    txtShow.AppendText("k");
                }
                else if (e.KeyValue == (int)Keys.P)
                {
                    txtShow.AppendText("p");
                }
                else if (e.KeyValue == (int)Keys.L)
                {
                    txtShow.AppendText("l");
                }
                else if (e.KeyValue == (int)Keys.O)
                {
                    txtShow.AppendText("o");
                }
                else if (e.KeyValue == (int)Keys.OemPeriod)
                {
                    txtShow.AppendText(".");
                }
                else if (e.KeyValue == (int)Keys.Oemcomma)
                {
                    txtShow.AppendText(",");
                }
                else if (e.KeyValue == (int)Keys.OemSemicolon)
                {
                    txtShow.AppendText(";");
                }
                else if (e.KeyValue == (int)Keys.OemMinus)
                {
                    txtShow.AppendText("-");
                }
                else if (e.KeyValue == (int)Keys.OemQuestion)
                {
                    txtShow.AppendText("/");
                }
                //else if (e.KeyValue == (int)Keys.)
                //{
                //    txtShow.AppendText("p");
                //}
            }
        
        }
    }
}
