using System.Windows;
using System.Windows.Controls;

namespace View.Helpers
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached(
                "Password",
                typeof(string),
                typeof(PasswordBoxHelper),
                new FrameworkPropertyMetadata(
                    string.Empty,
                    FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    OnPasswordChanged)
            );

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static void OnPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;
                passwordBox.Password = e.NewValue?.ToString() ?? "";
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;

                // ПРИНУДИТЕЛЬНОЕ ОБНОВЛЕНИЕ ИСТОЧНИКА
                var expression = passwordBox.GetBindingExpression(PasswordProperty);
                expression?.UpdateSource();
            }
        }

        private static void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetPassword(passwordBox, passwordBox.Password);

                // ПРИНУДИТЕЛЬНОЕ ОБНОВЛЕНИЕ ИСТОЧНИКА
                var expression = passwordBox.GetBindingExpression(PasswordProperty);
                expression?.UpdateSource();
            }
        }
    }
}