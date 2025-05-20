namespace DataGridView  // ✅ Ensure namespace is defined
{
    using Gtk;

    public class HeatTransferApp : Window
    {
        public HeatTransferApp() : base("Heat Transfer Matrix")
        {
            SetDefaultSize(600, 400);
            ShowAll();
        }
    }
}

