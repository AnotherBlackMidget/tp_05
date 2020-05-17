
using BillingManagement.Models;
using BillingManagement.UI.Contexts;
using BillingManagement.UI.ViewModels.Commands;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace BillingManagement.UI.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        public CustomerContext db;

        private ObservableCollection<Customer> customers;
        private Customer selectedCustomer;
        
        public ObservableCollection<Customer> Customers
        {
            get => customers;
            set
            {
                customers = value;
                OnPropertyChanged();
            }
        }

        public Customer SelectedCustomer
        {
            get => selectedCustomer;
            set
            {
                selectedCustomer = value;
                OnPropertyChanged();
            }
        }

        public RelayCommand<Customer> DeleteCustomerCommand { get; private set; }


        public CustomerViewModel( CustomerContext db)
        {
            DeleteCustomerCommand = new RelayCommand<Customer>(DeleteCustomer, CanDeleteCustomer);

            this.db = db;

            Customers = new ObservableCollection<Customer>(db.Customers.OrderBy(c => c.LastName));

        }



        private void DeleteCustomer(Customer c)
        {
            var currentIndex = Customers.IndexOf(c);

            if (currentIndex > 0) currentIndex--;

            db.Customers.Remove(c);
            db.SaveChanges();

            Customers.Clear();
            Customers = new ObservableCollection<Customer>(db.Customers.OrderBy(c => c.LastName));

            SelectedCustomer = Customers[currentIndex];
        }

        private bool CanDeleteCustomer(Customer c)
        {
            if (c == null) return false;

            
            return c.Invoices.Count == 0;
        }





    }
}
