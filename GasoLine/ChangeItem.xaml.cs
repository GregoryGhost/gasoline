using System;
using System.Collections.Generic;
using System.Linq;
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
using ModelService;

namespace GasoLine
{
    /// <summary>
    /// Логика взаимодействия для ChangeItem.xaml
    /// </summary>
    public partial class ChangeItem : Window
    {
        public ChangeItem()
        {
            InitializeComponent();
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if ((this.DataContext as VehicleViewModel).IsValid == false)
            {
                MessageBox.Show("Исправьте поля записи!");
                return;
            }
            DialogResult = true;
            Close();
        }
    }
}
