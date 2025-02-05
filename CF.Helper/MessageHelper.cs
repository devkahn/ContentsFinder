using System.Windows;
using MessageBox = System.Windows.MessageBox;

namespace CF.Helpers
{
    public static class MessageHelper
    {
        public static void ShowErrorMessage(string caption, string message)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowSuccessMessage(string caption, string message)
        {
            MessageBox.Show(message, caption, MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
