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
    /// Interaction logic for CarLimitMenu.xaml
    /// </summary>
    public partial class CarLimitMenu : Window
    {
        public int submitedValue;

        public CarLimitMenu()
        {
            InitializeComponent();
            TextBoxValue.Focus();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = e.Text.Any(c => !char.IsDigit(c));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TextBoxValue.Text, out submitedValue))
            {
                DialogResult = true;
                this.Close();
            }
        }

        private void TextBoxValue_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
        }
    }
}
