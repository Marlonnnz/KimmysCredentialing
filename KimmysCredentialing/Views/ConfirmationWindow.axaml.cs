using Avalonia.Controls;
using Avalonia.Interactivity;



namespace KimmysCredentialing.Views;


public partial class ConfirmationWindow : Window
{
    public bool Confirmed { get; private set; }

    public ConfirmationWindow()
    {
        InitializeComponent();
    }

    public ConfirmationWindow(string message) : this ()
    {
        MessageText.Text = message;
    }

    private void Yes_Click(object? sender, RoutedEventArgs e)
    {
        Confirmed = true;
        Close();
    }

    private void No_Click(object? sender, RoutedEventArgs e)
    {
        Confirmed = false;
        Close();
    }
}
