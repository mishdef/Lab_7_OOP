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

namespace OOPLabWPF
{
    /// <summary>
    /// Логика взаимодействия для CarDemonstrateStatic.xaml
    /// </summary>
    public partial class CarDemonstrateStatic : Window
    {
        bool fuelPriceEditing = false;

        public CarDemonstrateStatic()
        {
            InitializeComponent();
            StatisticsLabel.Content = Car.ShowCountres();
            FuelPriceTextBox.Text = Car.FuelPrice.ToString();
        }

        private void FuelEditButton_Click(object sender, RoutedEventArgs e)
        {
            if (!fuelPriceEditing)
            {
                FuelEditButton.Content = "Save";
                FuelPriceTextBox.IsEnabled = true;
                fuelPriceEditing = true;
                FuelPriceTextBox.Focus();
            }
            else
            {
                if (double.TryParse(FuelPriceTextBox.Text, out double newPrice) && newPrice >= 0)
                {
                    Car.FuelPrice = newPrice;
                    FuelEditButton.Content = "Edit";
                    FuelPriceTextBox.IsEnabled = false;
                    fuelPriceEditing = false;
                }
                else
                {
                    MessageBox.Show("Please enter a valid non-negative number for the fuel price.", "Invalid Input", MessageBoxButton.OK, MessageBoxImage.Warning);
                    FuelPriceTextBox.Text = Car.FuelPrice.ToString();
                }
            }
        }

        private void FuelPriceTextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && fuelPriceEditing)
            {
                FuelEditButton_Click(sender, e);
            }
        }
    }
}
