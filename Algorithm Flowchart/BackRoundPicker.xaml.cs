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

namespace Algorithm_Flowchart
{
    /// <summary>
    /// Interaction logic for BackRoundPicker.xaml
    /// </summary>
    public partial class BackRoundPicker : Window
    {
        public Window parent;
        public bool focusState { get; set; }
        public String colorCode  {get;set;}
        public BackRoundPicker(Window parent)
        {
            this.parent = parent;
            focusState = true;
            
            InitializeComponent();
        }

        private void colorPickerClick(object sender, RoutedEventArgs e)
        {            
            //if (!colorCode.Equals(""))
            //colorCode = ColorPicker.SelectedColor.ToString();
            colorCode = picker.SelectedColor.ToString();
            this.Hide();
            this.focusState = false;
        }

        private void OKBT_GotFocus(object sender, RoutedEventArgs e)
        {
            
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            //ColorTextBox.Text =ColorPicker.SelectedColor.ToString();
            //ColorPicker.SelectedColor.
        }
        
    }
}
