using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WiseCartLogic;
using WiseCartLogic.Entities;


namespace WiseCartView
{
    public partial class MainWindow
    {
        public bool LoadedCart { get; private set; }
        public WiseCartManager WiseCartManager { get; private set; }
        public UserDBManager UserDBManager { get; set; }
        public UserLoginManager UserLoginManager { get; set; }
        public CartComparer CartComparer { get; private set; }
        public CartSerializer CartSerializer { get; private set; }
        public LoginWindow LoginWindow { get; private set; }
        public UserDetailsViewModel UserDetailsViewModel { get; set; }

        public MainWindow()
        {
            CartComparer = new CartComparer();
            UserDBManager = new UserDBManager();
            UserLoginManager = new UserLoginManager(UserDBManager);

            LoginWindow = new LoginWindow(UserLoginManager);
            LoginWindow.ShowDialog();

            UserDetailsViewModel = new UserDetailsViewModel(UserLoginManager.LoggedInUser.Username);
            DataContext = UserDetailsViewModel;

            InitializeComponent();

            WiseCartManager = new WiseCartManager();

            Task T1 = Task.Factory.StartNew(() => WiseCartManager.InitializeData());

            Task T2 = T1.ContinueWith(t => ProductsDataGridLoaded());
        }

        private void ProductsDataGridLoaded()
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                ProductsDataGrid.ItemsSource = WiseCartManager.AllProvidersIntersected.Products;

                ProductsDataGrid.Columns[6].Visibility = Visibility.Hidden;

                for (int i = 0; i < 5; i++)
                {
                    ProductsDataGrid.Columns[i].IsReadOnly = true;
                }
            }));
        }

        private void ProductsDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private async void AddOrEditCartButton_Click(object sender, RoutedEventArgs e)
        {
            WiseCartManager.ShoppingCart.BuildCart(WiseCartManager.AllProvidersIntersected.Products);

            Task task1 = Task.Factory.StartNew(() => UpdateCartDetailsListBox());
            Debug.WriteLine("Task 1 completed");

            List<Tuple<string, List<Product>>> cheapestProducts = CartComparer.GetCheapestProductsPerProvider(WiseCartManager.ShoppingCart, WiseCartManager.Providers, 3);
            List<Tuple<string, List<Product>>> mostExpensiveProducts = CartComparer.GetMostExpensivetProductsPerProvider(WiseCartManager.ShoppingCart, WiseCartManager.Providers, 3);

            Task task2 = Task.Factory.StartNew(() => UpdateProductsListBox(CheapestProductsListBox, cheapestProducts));
            Debug.WriteLine("Task 2 completed");

            Task task3 = Task.Factory.StartNew(() => UpdateProductsListBox(MostExpensiveProductsListBox, mostExpensiveProducts));

            await Task.WhenAll(new Task[] {task1, task2, task3});
            Debug.WriteLine("Task 3 completed");

            MessageBox.Show(("Edited items successfully."));
            if (WiseCartManager.ShoppingCart.Products.Count > 0)
            {
                SaveCartButton.IsEnabled = true;
            }
            else
            {
                SaveCartButton.IsEnabled = false;
            }
        }

        private void SaveCartButton_Click(object sender, RoutedEventArgs e)
        {
            CartSerializer = new CartSerializer(WiseCartManager.ShoppingCart);
            CartSerializer.SaveCart();
            MessageBox.Show("Cart successfully saved.");

            LoadUnloadCartButton.IsEnabled = true;
        }

        private void LoadUnloadCartButton_Click(object sender, RoutedEventArgs e)
        {
            if (!LoadedCart)
            {
                CartSerializer.LoadCart();
                ProductsDataGrid.ItemsSource = WiseCartManager.ShoppingCart.Products;
                MessageBox.Show("Cart successfully loaded to DataGrid. Clicking again will reload all products.");
                LoadedCart = true;
            }
            else
            {
                ProductsDataGrid.ItemsSource = WiseCartManager.AllProvidersIntersected.Products;
                MessageBox.Show("Successfully unloaded cart.");
                LoadedCart = false;
            }
        }

        private void UpdateProductsListBox(ListBox listbox, List<Tuple<string, List<Product>>> products)
        {
            Dispatcher.BeginInvoke(new Action(delegate
            {
                listbox.Items.Clear();

                foreach (Tuple<string, List<Product>> tuple in products)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(string.Format("Provider : {0}", tuple.Item1));

                    foreach (Product product in tuple.Item2)
                    {
                        sb.AppendLine(string.Format("Product Name : {0}{1}Price : {2}", product.ItemName, Environment.NewLine, product.Price));
                    }

                    listbox.Items.Add(sb);
                }
            }));
        }

        private void UpdateCartDetailsListBox()
        {
            List<Tuple<string, double>> cartPrices = CartComparer.GetCartsTotalPrices(WiseCartManager.ShoppingCart, WiseCartManager.Providers);

            Dispatcher.BeginInvoke(new Action(delegate
            {
                CartDetailsListBox.Items.Clear();

                foreach (Tuple<string, double> cartPrice in cartPrices)
                {
                    CartDetailsListBox.Items.Add(string.Format("Provider : {0}{1}Cart Price : {2}", cartPrice.Item1,
                        Environment.NewLine, cartPrice.Item2));
                }
            }));
        }

        private void LoginDifferentUserButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow.ShowDialog();
            UserDetailsViewModel.Username = UserLoginManager.LoggedInUser.Username;

            UserDetailsViewModel = new UserDetailsViewModel(UserLoginManager.LoggedInUser.Username);
            DataContext = UserDetailsViewModel;
        }

        private void ExcelChartButton_Click(object sender, RoutedEventArgs e)
        {
            List<Tuple<string, double>> cartPrices = CartComparer.GetCartsTotalPrices(WiseCartManager.ShoppingCart, WiseCartManager.Providers);

            ExcelsheetProducer producer = new ExcelsheetProducer();
            
            producer.ProduceExcelsheet(cartPrices);

            MessageBox.Show("Excel file created in solution folder\\Resources");
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            LoginWindow.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            //base.OnClosed(e);
            Application.Current.Shutdown();
        }
    }
}
