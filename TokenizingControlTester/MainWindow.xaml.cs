using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TokenizingControlTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += delegate
                          {
                              Tokenizer.Focus();
                          };

            Tokenizer.TokenMatcher = text =>
                                         {
                                             if (text.EndsWith(";"))
                                             {
                                                 // Remove the ';'
                                                 return text.Substring(0, text.Length - 1).Trim().ToUpper();
                                             }

                                             return null;
                                         };
        }

        private void extractTokens_Click(object sender, RoutedEventArgs e)
        {
            extractedTokens.Text = "";
            foreach (TextBlock tb in FindVisualChildren<TextBlock>(Tokenizer))
            {
                extractedTokens.Text += tb.Text + ";" ;
            }

        }
        public IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }

        }


        private void addSpecialToken_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
