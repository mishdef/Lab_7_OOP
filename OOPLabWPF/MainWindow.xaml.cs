using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;
using Newtonsoft.Json;

namespace OOPLabWPF
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static int carLimit = 0;
        public static List<Car> cars = new List<Car>();

        public MainWindow()
        {
            InitializeComponent();
        }

        public static string ToCsv(List<Car> cars)
        {
            string csvContent = "";
            csvContent += "Mark;Model;Color;HorsePower;Weight;Milage;FuelCapacity;ProductionDate;FuelConsumptionPer100km;NumberOfDoors\n";

            foreach (var car in cars)
            {
                csvContent += car.ToString() + "\n";
            }
            return csvContent.ToString();
        }

        public static List<Car> FromCsv(string csvContent)
        {
            var newCars = new List<Car>();
            var lines = csvContent.Split(new[] { '\r', '\n' }, System.StringSplitOptions.RemoveEmptyEntries);

            for (int i = 1; i < lines.Length; i++)
            {
                if (Car.TryParse(lines[i], out Car validCar))
                {
                    newCars.Add(validCar);
                }
            }
            return newCars;
        }

        public static string ToJson(List<Car> cars)
        {
            return JsonConvert.SerializeObject(cars, Formatting.Indented);
        }

        public static List<Car> FromJson(string jsonContent)
        {
            List<Car> importedCars, validCars;

            try { 
                importedCars = JsonConvert.DeserializeObject<List<Car>>(jsonContent);
                validCars = new List<Car>();
            }
            catch (JsonException)
            {
                return new List<Car>();
            }

            if (importedCars != null)
            {
                foreach (var car in importedCars)
                {
                    try
                    {
                        if (Car.TryParse(car.ToString(), out Car validCar))
                        {
                            validCars.Add(validCar);
                        }
                    }
                    catch (ArgumentException _)
                    {
                        continue;
                    }
                }
            }
            return validCars;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CarLimitMenu carLimitMenu = null;
            do {
                carLimitMenu = new CarLimitMenu() { Owner = this };
                if (carLimitMenu.ShowDialog() == true) break;
                MessageBoxResult result = MessageBox.Show("Please, set up car capacity! Or click cancel to close the program.", "Message", MessageBoxButton.OKCancel, MessageBoxImage.Stop);
                if (result == MessageBoxResult.Cancel)
                {
                    this.Close();
                    return;
                }
            } while (true);
            carLimitMenu.Close();
            carLimit = carLimitMenu.submitedValue;
            UpdateCarDataGrid();
            carLimitMenu = null;
        }

        private void Test_Command_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            MessageBox.Show("Test command executed");
        }

        private void AddMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (cars.Count >= carLimit)
            {
                MessageBox.Show("Storage is full, cannot add more cars.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            CarAddWindow carAddMenu = new CarAddWindow() { Owner = this };
            carAddMenu.ShowDialog();
        }

        private void AddSeedDataMenuItem_Click(object sender, RoutedEventArgs e)
        {
            AddSeedData();
            UpdateCarDataGrid();
        }

        public void UpdateCarDataGrid()
        {
            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = cars;
            labelRemain.Content = $"Remain: {carLimit - cars.Count}";
            labelCount.Content = $"Count: {cars.Count}";
            if (carLimit - cars.Count == 1) labelRemain.Background = Brushes.LightYellow;
            else if (carLimit - cars.Count == 0) labelRemain.Background = Brushes.LightCoral;
            else labelRemain.Background = Brushes.Transparent;

            if (cars.Count != 0)
            {
                ClearAllCarsMenuItem.IsEnabled = true;
                ExportMenuItem.IsEnabled = true;
            }
            else 
            { 
                ClearAllCarsMenuItem.IsEnabled = false; 
                ExportMenuItem.IsEnabled = false;
            }
        }

        static public void AddSeedData()
        {
            if (cars.Count >= carLimit)
            {
                MessageBox.Show("Storage is full, cannot add seed data.");
                return;
            }

            var seedDataItems = new[]
            {
                new { Mark = "Audi", Model = "A4", Color = Color.Red, HorsePower = 150f, Weight = 1550m, Milage = 12000.0, FuelConsumptionPer100km = 10.0, FuelCapacity = 60.0, ProductionDate = new DateTime(2022, 1, 1), NumberOfDoors = 4 },
                new { Mark = "Audi", Model = "A6", Color = Color.Black, HorsePower = 250f, Weight = 1800m, Milage = 0.0, FuelConsumptionPer100km = 14.0, FuelCapacity = 70.0, ProductionDate = new DateTime(2020, 6, 12), NumberOfDoors = 4 },
                new { Mark = "BMW", Model = "M3", Color = Color.Blue, HorsePower = 420f, Weight = 1600m, Milage = 85000.0, FuelConsumptionPer100km = 16.0, FuelCapacity = 63.0, ProductionDate = new DateTime(2008, 5, 17), NumberOfDoors = 2 },
                new { Mark = "Mini", Model = "Cooper", Color = Color.Green, HorsePower = 40f, Weight = 650m, Milage = 0.0, FuelConsumptionPer100km = 6.0, FuelCapacity = 30.0, ProductionDate = new DateTime(1995, 3, 4), NumberOfDoors = 3 },
                new { Mark = "Ford", Model = "F-150", Color = Color.Black, HorsePower = 400f, Weight = 2500m, Milage = 25000.0, FuelConsumptionPer100km = 24.0, FuelCapacity = 120.0, ProductionDate = new DateTime(2021, 1, 1), NumberOfDoors = 4 }
            };

            int initialCarsCount = cars.Count;
            int carsAddedCount = 0;

            foreach (var item in seedDataItems)
            {
                if (cars.Count < carLimit)
                {
                    cars.Add(new Car(item.Mark, item.Model, item.Color, item.HorsePower, item.Weight, item.Milage, item.FuelCapacity, item.ProductionDate, item.FuelConsumptionPer100km, item.NumberOfDoors));
                    carsAddedCount++;
                }
                else
                {
                    MessageBox.Show("Storage became full. Not all seed data cars were added.");
                    break;
                }
            }

            if (carsAddedCount > 0)
            {
                MessageBox.Show($"{carsAddedCount} cars added from seed data.");
            }
            else if (initialCarsCount == cars.Count)
            {
                MessageBox.Show("No seed data cars could be added due to storage capacity.");
            }
        }

        private void dataGrid_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.SelectedItem is Car selectedCar)
            {
                CarBehaviourWindow carBehaviourWindow = new CarBehaviourWindow(selectedCar) { Owner = this };
                carBehaviourWindow.ShowDialog();
            }
        }

        private void dataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                if (dataGrid.SelectedItem is Car selectedCar)
                {
                    selectedCar = (Car)dataGrid.SelectedItem;
                    var result = MessageBox.Show($"Are you sure you want to delete the selected car: {selectedCar.MarkAndModel}?", "Confirm Deletion", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        cars.Remove(selectedCar);
                        var player = new System.Media.SoundPlayer("Sounds/CarDeleteSound.wav");
                        player.Play();
                        UpdateCarDataGrid();
                    }
                }
            }
            if (e.Key == Key.Enter)
            {
                dataGrid_PreviewMouseDoubleClick(sender, null);
            }
        }


        #region Ai
        //сброс выделения при клике вне строки
        public static T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            return parentObject is T parent ? parent : FindParent<T>(parentObject);
        }

        private void Window_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var src = e.OriginalSource as DependencyObject;

            if (src == null) return;

            if (FindParent<DataGridRow>(src) != null || FindParent<ScrollBar>(src) != null)
            {
                return;
            }
            dataGrid.SelectedItem = null;
        }
        #endregion


        private void SearchMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (SearchCanvas.Visibility == Visibility.Hidden)
            {
                dataGrid.Margin = new Thickness(0, 30, 2, 0);
                SearchCanvas.Visibility = Visibility.Visible;

                SearchMenuItem.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#e0e0e0");
            }
            else 
            {
                dataGrid.Margin = new Thickness(0, 0, 2, 0);
                SearchCanvas.Visibility = Visibility.Hidden;

                SearchMenuItem.Background = (System.Windows.Media.Brush)new System.Windows.Media.BrushConverter().ConvertFrom("#f0f0f0");
            }
        }

        private void SearchTypePickerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SearchPromptStringTextBox == null || ColorPickerComboBox == null)
            {
                return;
            }
            if (SearchTypePickerComboBox.SelectedIndex == 0)
            {
                SearchPromptStringTextBox.Text = "";
                SearchPromptStringTextBox.Visibility = Visibility.Visible;
                ColorPickerComboBox.Visibility = Visibility.Hidden;
                SearchClearStringButton.Visibility = Visibility.Hidden;
                SearchClearColorButton.Visibility = Visibility.Hidden;
                UpdateCarDataGrid();
            }
            if(SearchTypePickerComboBox.SelectedIndex == 1)
            {
                ColorPickerComboBox.SelectedIndex = -1;
                ColorPickerComboBox.Visibility = Visibility.Visible;
                SearchPromptStringTextBox.Visibility = Visibility.Hidden;
                SearchClearStringButton.Visibility = Visibility.Hidden;
                SearchClearColorButton.Visibility = Visibility.Hidden;
                UpdateCarDataGrid();
            }
        }

        private void SearchPromptStringTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchPromptStringTextBox.Text != "")
            {
                SearchClearStringButton.Visibility = Visibility.Visible;
            }
            else
            {
                SearchClearStringButton.Visibility = Visibility.Hidden;
            }
            List<Car> filteredCars = cars.FindAll(car => car.MarkAndModel.ToLower().Contains(SearchPromptStringTextBox.Text.ToLower()));

            dataGrid.ItemsSource = null;
            dataGrid.ItemsSource = filteredCars;
        }

        private void ColorPickerComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ColorPickerComboBox.SelectedIndex != -1)
            {
                SearchClearColorButton.Visibility = Visibility.Visible;

                List<Car> filteredCars = cars.FindAll(car => car.Color == (Color)ColorPickerComboBox.SelectedIndex);

                dataGrid.ItemsSource = null;
                dataGrid.ItemsSource = filteredCars;
            }
        }

        private void SearchClearButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateCarDataGrid();
            SearchPromptStringTextBox.Text = "";
            ColorPickerComboBox.SelectedIndex = -1;

            SearchClearStringButton.Visibility = Visibility.Hidden;
            SearchClearColorButton.Visibility = Visibility.Hidden;
        }

        private void StaticMenuItem_Click(object sender, RoutedEventArgs e)
        {
            CarDemonstrateStatic carDemonstrateStatic = new CarDemonstrateStatic() { Owner = this };
            carDemonstrateStatic.ShowDialog();
        }

        private void FileExportToCSVMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            saveFileDialog.DefaultExt = "csv";
            if (saveFileDialog.ShowDialog() == true)
            {
                string csvContent = ToCsv(cars);
                try
                {
                    File.WriteAllText(saveFileDialog.FileName, csvContent);
                    MessageBox.Show("Data exported successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void FileExportToJSONMenuItem_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*";
            saveFileDialog.DefaultExt = "json";
            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    string jsonString = ToJson(cars);
                    File.WriteAllText(saveFileDialog.FileName, jsonString);
                    MessageBox.Show("Data exported successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void FileImportMenuItem_Click(object sender, RoutedEventArgs e)
        {
            bool extendStorage = false;

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "JSON files (*.json)|*.json|CSV files (*.csv)|*.csv|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    string fileContent = File.ReadAllText(openFileDialog.FileName);
                    int initialCarsCount = cars.Count;
                    List<Car> newCars = new List<Car>();
                    int carsAddedCount = 0;
                    if (openFileDialog.FileName.EndsWith(".json"))
                    {
                        newCars = FromJson(fileContent);
                    }
                    else if (openFileDialog.FileName.EndsWith(".csv"))
                    {
                        extendStorage = false;
                        newCars = FromCsv(fileContent);
                    }
                    if (newCars.Count+ carLimit > carLimit)
                    {
                        MessageBoxResult res = MessageBox.Show("Not enough space to import all cars. Extend storage?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (res == MessageBoxResult.Yes)
                        {
                            extendStorage = true;
                        }
                    }
                    if (extendStorage)
                    {
                        foreach (var car in newCars)
                        {
                            cars.Add(car);
                            carsAddedCount++;
                        }
                        carLimit = cars.Count;
                    }
                    else
                    {
                        int spaceAvailable = carLimit - cars.Count;
                        int carsToImport = newCars.Count;
                        int limit = Math.Min(spaceAvailable, carsToImport);

                        for (int i = 0; i < limit; i++)
                        {
                            cars.Add(newCars[i]);
                            carsAddedCount++;
                        }
                    }
                    if (carsAddedCount > 0)
                    {
                        MessageBox.Show($"{carsAddedCount} cars imported successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (initialCarsCount == cars.Count)
                    {
                        MessageBox.Show("No valid cars were imported or storage is full.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    UpdateCarDataGrid();
                    newCars.Clear();
                    newCars = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error reading file: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearAllCarsMenuItem_Click(object sender, RoutedEventArgs e)
        { 
            MessageBoxResult res = MessageBox.Show("Are you sure you want to clear all cars?", "Question", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.Yes)
            {
                cars = new List<Car>();
                UpdateCarDataGrid();
            }
        }
    }
}