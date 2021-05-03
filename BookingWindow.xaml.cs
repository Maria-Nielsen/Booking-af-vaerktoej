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
using ConstructionMarketClassLibrary;




namespace EmployeeWpfApp
{
    /// <summary>
    /// Interaction logic for BookingWindow.xaml
    /// </summary>
    public partial class BookingWindow : Window
    {
        public BookingWindow()
        {
            InitializeComponent();
        }

        public void Show(Booking b)
        {
            BookingIDTB.DataContext = b;
            StartDateDP.DataContext = b;
            EndDateDP.DataContext = b;
            if (b.Status == CBIReserved.Content.ToString()) { CBIReserved.IsSelected = true; }
            else if (b.Status == CBIHandedOver.Content.ToString()) { CBIHandedOver.IsSelected = true; }
            else { CBIReturned.IsSelected = true; }
            if(b.Tool.Name == CBIHavetromler.Content.ToString()) { CBIHavetromler.IsSelected = true; }
            else if(b.Tool.Name == CBIKompostkværn.Content.ToString()) { CBIKompostkværn.IsSelected = true; }
            else if (b.Tool.Name == CBIVinkelsliber.Content.ToString()) { CBIVinkelsliber.IsSelected = true; }
            else if (b.Tool.Name == CBIGulvslibemaskine.Content.ToString()) { CBIGulvslibemaskine.IsSelected = true; }
            else if (b.Tool.Name == CBIMotorsav.Content.ToString()) { CBIMotorsav.IsSelected = true; }


            //StatusCB.SelectedItem = StatusCB.Items.Equals(b.Status);
            //StatusCB.DataContext = b;
            //ToolCB.DataContext = b;
            Show();
        }



        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            //????????????????
            //MainWindow.ctx.SaveChanges();
            this.Close();
        }
    }
}
