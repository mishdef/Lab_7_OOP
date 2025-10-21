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
using System.Xml.Serialization;

namespace OOPLabWPF
{
    /// <summary>
    /// Логика взаимодействия для CarBehaviourWindow.xaml
    /// </summary>
    public partial class CarBehaviourWindow : Window
    {
        Car car = null;
        MainWindow mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();

        public CarBehaviourWindow(Car car)
        {
            InitializeComponent();
            this.car = car;
            UpdateCar();
        }

        private void UpdateCar()
        {
            Mark_TextBox.Text = car.MarkAndModel;
            Color_TextBox.Text = car.Color.ToString();
            HP_TextBox.Text = car.HorsePower.ToString();
            Weight_TextBox.Text = car.Weight.ToString();
            Milage_TextBox.Text = car.Milage.ToString();
            FuelCapacity_TextBox.Text = car.FuelCapacity.ToString();
            ProductionDate_TextBox.Text = car.ProductionDate.ToShortDateString();
            FuelConsumption_TextBox.Text = car.FuelConsumptionPer100km.ToString();
            Doors_TextBox.Text = car.NumberOfDoors.ToString();
            CurrentFuel_TextBox.Text = car.CurrentFuel.ToString();
            CurrentSpeed_TextBox.Text = car.CurrentSpeed.ToString("F2");
            MaxSpeed_TextBox.Text = car.MaxSpeed.ToString("F2");

            ToString_TextBox.Clear();
            mainWindow.UpdateCarDataGrid();
        }

        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(car.StartEnige());
            UpdateCar();
        }

        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(car.StopEngine());
            UpdateCar();
        }

        private void SpeedUp_Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(car.SpeedUp());
            UpdateCar();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = e.Text.Any(c => !char.IsDigit(c) && c != ',' && c != '-');
        }

        private void SpeedUpXTurbo_Button_Click(object sender, RoutedEventArgs e)
        {
            TryCatch(() => MessageBox.Show(car.SpeedUp(double.Parse(SpeedUpXTurbo_TextBox.Text), TurboCheckBox.IsChecked == true ? true : false)));
            UpdateCar();
        }

        private void SpeedUpX_Button_Click(object sender, RoutedEventArgs e)
        {
            TryCatch(() => MessageBox.Show(car.SpeedUp(double.Parse(SpeedUpX_TextBox.Text))));
            UpdateCar();
        }

        private void SlowDown_Button_Click(object sender, RoutedEventArgs e)
        {
            TryCatch(() => MessageBox.Show(car.SlowDown(double.Parse(SlowDown_TextBox.Text))));
            UpdateCar();
        }

        private void RideCar_Button_Click(object sender, RoutedEventArgs e)
        {
            TryCatch(() => MessageBox.Show(car.RideCar(double.Parse(RideCar_TextBox.Text))));
            UpdateCar();
        }

        private void Reduel_Button_Click(object sender, RoutedEventArgs e)
        {
            TryCatch(() => MessageBox.Show(car.Refuel(double.Parse(Refuel_TextBox.Text))));
            UpdateCar();
        }

        private void ToString_Button_Click(object sender, RoutedEventArgs e)
        {
            ToString_TextBox.Text = car.ToString();
        }

        //прикольный приколяс
        private void TryCatch(Action action)
        {
            try
            {
                action();
            }
            catch (FormatException)
            {
                MessageBox.Show("Input format is incorrect. Please enter a valid number.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            catch (OverflowException)
            {
                MessageBox.Show("Input value is too large or too small. Please enter a reasonable number.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
