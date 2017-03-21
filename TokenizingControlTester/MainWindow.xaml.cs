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
                                                 return text.Substring(0, text.Length - 1).Trim();
                                             }

                                             return null;
                                         };
        }

        private void extractTokens_Click(object sender, RoutedEventArgs e)
        {
            extractedTokens.Text = "Tags are: ";
            foreach (TextBlock tb in FindVisualChildren<TextBlock>(Tokenizer))
            {
                extractedTokens.Text += tb.Text + "; " ;
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
            string text = "special;";
            Tokenizer.CaretPosition.InsertTextInRun(text);
            if (Tokenizer.TokenMatcher != null)
            {
                var token = Tokenizer.TokenMatcher(text);
                if (token != null)
                {
                    Tokenizer.ReplaceTextWithToken(text, token);
                }
            }
        }

        private void Tokenizer_TextChanged(object sender, TextChangedEventArgs e)
        {
            Tokenizer.UpdateLayout();
            UpdateColors();
        }

        private void UpdateColors()
        {
            foreach (TextBlock tb in FindVisualChildren<TextBlock>(Tokenizer))
            {
                if (tb.Text.ToLower() == "special")
                {
                    Border g = FindParent<Border>(tb);
                    g.Background = Brushes.LightBlue;
                }            }
        }

        private T FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            //get parent item
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

    }
}
