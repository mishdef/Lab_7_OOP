using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Media.Media3D;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace OOPLabWPF
{
    /// <summary>
    /// Interaction logic for CarAddWindow.xaml
    /// </summary>
    public partial class CarAddWindow : Window
    {
        public CarAddWindow()
        {
            InitializeComponent();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ClearAllFields();
            if (ComboBox.SelectedIndex == 0)
            {
                GroupBox_Manual.Visibility = Visibility.Visible;
                GroupBox_String.Visibility = Visibility.Hidden;
                GroupBox_Minimal.Visibility = Visibility.Hidden;
                GroupBox_AllParameters.Visibility = Visibility.Hidden;
            }
            if (ComboBox.SelectedIndex == 1)
            {
                GroupBox_Manual.Visibility = Visibility.Hidden;
                GroupBox_String.Visibility = Visibility.Visible;
                GroupBox_Minimal.Visibility = Visibility.Hidden;
                GroupBox_AllParameters.Visibility = Visibility.Hidden;
            }
            if (ComboBox.SelectedIndex == 2)
            {
                GroupBox_Manual.Visibility = Visibility.Hidden;
                GroupBox_String.Visibility = Visibility.Hidden;
                GroupBox_Minimal.Visibility = Visibility.Visible;
                GroupBox_AllParameters.Visibility = Visibility.Hidden;
            }
            if (ComboBox.SelectedIndex == 3)
            {
                GroupBox_Manual.Visibility = Visibility.Hidden;
                GroupBox_String.Visibility = Visibility.Hidden;
                GroupBox_Minimal.Visibility = Visibility.Hidden;
                GroupBox_AllParameters.Visibility = Visibility.Visible;
            }
            if (ComboBox.SelectedIndex == 4)
            {
                GroupBox_Manual.Visibility = Visibility.Hidden;
                GroupBox_String.Visibility = Visibility.Hidden;
                GroupBox_Minimal.Visibility = Visibility.Hidden;
                GroupBox_AllParameters.Visibility = Visibility.Hidden;
            }
        }

        private void ClearAllFields()
        {
            manualMark_TextBox.Clear();
            manualModel_TextBox.Clear();
            manualColor_TextBox.Clear();
            manualHP_TextBox.Clear();
            manualWeight_TextBox.Clear();
            manualMilage_TexttBox.Clear();
            manualFuelCapacity_TextBox.Clear();
            manualProductionDate_TextBox.Clear();
            manualFuelConsumption_TextBox.Clear();
            manualDoors_TextBox.Clear();

            stringData_TextBox.Clear();

            minimalMark_TextBox.Clear();
            minimalModel_TextBox.Clear();
            minimalColor_TextBox.Clear();

            alllMark_TextBox.Clear();
            allModel_TextBox.Clear();
            Color_TextBox.Clear();
            HP_TextBox.Clear();
            Weight_TextBox.Clear();
            Milage_TextBox.Clear();
            FuelCapacity_TextBox.Clear();
            ProductionDate_TextBox.Clear();
            FuelConsumption_TextBox.Clear();
            Doors_TextBox.Clear();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ComboBox.SelectedIndex == 0)
                {
                    Car car = new Car() {
                        Mark = manualMark_TextBox.Text,
                        Model = manualModel_TextBox.Text,
                        Color = (Color)int.Parse(manualColor_TextBox.Text),
                        HorsePower = float.Parse(manualHP_TextBox.Text),
                        Weight = decimal.Parse(manualWeight_TextBox.Text),
                        Milage = double.Parse(manualMilage_TexttBox.Text),
                        FuelCapacity = double.Parse(manualFuelCapacity_TextBox.Text),
                        ProductionDate = DateTime.Parse(manualProductionDate_TextBox.Text),
                        FuelConsumptionPer100km = double.Parse(manualFuelConsumption_TextBox.Text),
                        NumberOfDoors = int.Parse(manualDoors_TextBox.Text)
                    }
                    ; 
                    MainWindow.cars.Add(car);
                    MessageBox.Show("Car added successfully.");
                }
                if (ComboBox.SelectedIndex == 1)
                {
                    string carString = stringData_TextBox.Text;
                    Car car = null;
                    Car.TryParse(carString, out car);
                    if (car == null)
                    {
                        MessageBox.Show("Error parsing car from string. Please check the format.");
                    }
                    else
                    {
                        MainWindow.cars.Add(car);
                    }
                }
                if (ComboBox.SelectedIndex == 2)
                {
                    Car car = new Car(
                        minimalMark_TextBox.Text,
                        minimalModel_TextBox.Text,
                        (Color)int.Parse(minimalColor_TextBox.Text)
                        );
                    MainWindow.cars.Add(car);
                }
                if (ComboBox.SelectedIndex == 3)
                {
                    Car car = new Car(
                        alllMark_TextBox.Text,
                        allModel_TextBox.Text,
                        (Color)int.Parse(Color_TextBox.Text),
                        float.Parse(HP_TextBox.Text),
                        decimal.Parse(Weight_TextBox.Text),
                        double.Parse(Milage_TextBox.Text),
                        double.Parse(FuelCapacity_TextBox.Text),
                        DateTime.Parse(ProductionDate_TextBox.Text),
                        double.Parse(FuelConsumption_TextBox.Text),
                        int.Parse(Doors_TextBox.Text)
                        );
                    MainWindow.cars.Add(car);
                }
                if (ComboBox.SelectedIndex == 4)
                {
                    Car car = new Car();
                    MainWindow.cars.Add(car);
                    Car.Count++;
                }
                if (MainWindow.cars.Count > MainWindow.carLimit)
                {
                    MessageBox.Show("Storage is full, cannot add more cars.");
                    MainWindow.cars.RemoveAt(MainWindow.cars.Count - 1);
                    return;
                }
                ((MainWindow)Owner).UpdateCarDataGrid();
                var player = new System.Media.SoundPlayer("Sounds/CarCreationSound.wav");
                player.Play();
                ClearAllFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding car: {ex.Message}");
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = e.Text.Any(c => !char.IsDigit(c) && c != ',' && c != '-');
        }
    }
}
