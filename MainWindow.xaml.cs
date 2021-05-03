using ConstructionMarketClassLibrary;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EmployeeWpfApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        ConstructionMarketEntities ctx = new ConstructionMarketEntities();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ctx.Customer.ToList();
            customerDG.ItemsSource = ctx.Customer.Local;
        }

        // TAGER DEN VALGTE CUSTOMER OG VISER ALLE DENNES BOOKINGER I ANDET DATAGRID
        private void CustomerSelected(object sender, RoutedEventArgs e)
        {
            var selectedItem = customerDG.SelectedItem;
            
            if (selectedItem is Customer)
            {
                lblTool.Content = "";
                lblDescription.Content = "";
                lblDeposit.Content = "";
                lblDailyPrice.Content = "";
                CBStatus.SelectedItem = null;
                Customer customer = (Customer)selectedItem;
                
                bookingsDG.ItemsSource = customer.Booking.OrderBy(p => p.StartPeriod);
            }
        }

       

        //TAGER DEN VALGTE BOOKING OG VISER INDHOLDET FOR DENNES TOOL
        private void BookingSelected(object sender, RoutedEventArgs e)
        {
            var selectedItem = bookingsDG.SelectedItem;
            if (selectedItem is Booking)
            {
                Booking booking = (Booking)selectedItem;
                lblTool.Content = booking.Tool.Name;
                lblDescription.Content = booking.Tool.Description;
                lblDeposit.Content = booking.Tool.Deposit;
                lblDailyPrice.Content = booking.Tool.DailyPrice;
                if (booking.Status == "Reserved") { CBIReserved.IsSelected = true; CBStatus.SelectedItem = CBIReserved; }
                if (booking.Status == "Handed over") { CBIHandedOver.IsSelected = true; CBStatus.SelectedItem = CBIHandedOver; }
                if (booking.Status == "Returned") { CBIReturned.IsSelected = true; CBStatus.SelectedItem = CBIReturned; }
            }
        }

        //HVIS DER ÆNDRES PÅ COMBOBOXENS DATA SÅ ÆNDRES DETTE OGSÅ FOR DEN VALGTE BOOKINGS STATUS OGSÅ.
        private void CBStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = bookingsDG.SelectedItem;
            Booking booking = (Booking)selectedItem;
            if (CBIReserved.IsSelected == true) { booking.Status = CBIReserved.Content.ToString(); }
            if (CBIHandedOver.IsSelected == true) { booking.Status = CBIHandedOver.Content.ToString(); }
            if (CBIReturned.IsSelected == true) { booking.Status = CBIReturned.Content.ToString(); }

            ctx.SaveChanges();
            bookingsDG.Items.Refresh();
        }

        //SØGER PÅ ALLE CUSTOMERS EMAIL DER HAR NOGET I SIG DER STEMMER OVERENS MED DET INDSKREVNE
        private void SearchCustomer(object sender, TextChangedEventArgs e)
        {
            lblTool.Content = "";
            lblDescription.Content = "";
            lblDeposit.Content = "";
            lblDailyPrice.Content = "";
            CBStatus.SelectedItem = null;
            List<Customer> customers = ctx.Customer.ToList();
            List<Customer> showList = new List<Customer>();
            foreach (var c in customers)
            {
                if (c.Email.Contains(SearchTB.Text))
                {
                    showList.Add(c);
                }
            }
            customerDG.ItemsSource = showList;
        }

        // DENNE SLETTER DEN VALGTE BOOKING OG SPØRG EN EKSTRA GANG OM MAN ER SIKKER PÅ MAN VIL SLETTE
        private void DeleteBooking_Clicked(object sender, RoutedEventArgs e)
        {
            var selectedItem = bookingsDG.SelectedItem;
            if (selectedItem is Booking)
            {
                Booking booking = (Booking)selectedItem;
                MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this booking?", "Delete booking", MessageBoxButton.YesNo);
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        ctx.Booking.Remove(booking);
                        ctx.SaveChanges();
                        bookingsDG.Items.Refresh();
                        MessageBox.Show("The booking has now been deleted!", "Confirmed");
                        break;
                }
            }
            else
            {
                MessageBox.Show("No booking i selected!");
            }
        }

        //SØRGER FOR  AT SORTERERE EFTER DET VALGTE I COMBOBOXEN
        private void BookingStatusSorting_Changed(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = customerDG.SelectedItem;
            if (selectedItem is Customer)
            {
                Customer customer = (Customer)selectedItem;
                var bookings = customer.Booking.ToList();
                List<Booking> bookingsToShow = new List<Booking>();

                if (AllStatusCBSort.IsSelected == true) { bookingsDG.ItemsSource = customer.Booking; }
                else if (ReservedStatusCBSort.IsSelected == true)
                {
                    foreach (var b in bookings)
                    {
                        if (b.Status.Equals("Reserved"))
                        {
                            bookingsToShow.Add(b);
                        }
                    }
                    bookingsDG.ItemsSource = bookingsToShow;
                }
                else if (HandedOverStatusCBSort.IsSelected == true)
                {
                    foreach (var b in bookings)
                    {
                        if (b.Status.Equals("Handed over"))
                        {
                            bookingsToShow.Add(b);
                        }
                    }
                    bookingsDG.ItemsSource = bookingsToShow;
                }
                else if (ReturnedStatusCBSort.IsSelected == true)
                {
                    foreach (var b in bookings)
                    {
                        if (b.Status.Equals("Returned"))
                        {
                            bookingsToShow.Add(b);
                        }
                    }
                    bookingsDG.ItemsSource = bookingsToShow;
                }
                
            }
        }

        private void Edit_Booking_Click(object sender, RoutedEventArgs e)
        {
            var selectedItme = bookingsDG.SelectedItem;
            if(selectedItme is Booking)
            {
                Booking booking = (Booking)selectedItme;
                BookingWindow bw = new BookingWindow();
                bw.Show(booking);
            }
            else
            {
                MessageBox.Show("No booking selected");
            }
        }
    }
}
