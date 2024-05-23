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
    public partial class Loading : Window
    {
        public Loading()
        {
            InitializeComponent();
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
    }
}
