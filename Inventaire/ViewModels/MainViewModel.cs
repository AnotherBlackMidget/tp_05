using BillingManagement.Models;
using BillingManagement.UI.Contexts;
using BillingManagement.UI.ViewModels.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BillingManagement.UI.ViewModels
{
    class MainViewModel : BaseViewModel
    {
		public CustomerContext db = new CustomerContext();


		private BaseViewModel _vm;

		public BaseViewModel VM
		{
			get { return _vm; }
			set {
				_vm = value;
				OnPropertyChanged();
			}
		}

		private string searchCriteria;

		public string SearchCriteria
		{
			get { return searchCriteria; }
			set { 
				searchCriteria = value;
				OnPropertyChanged();
			}
		}


		CustomerViewModel customerViewModel;
		InvoiceViewModel invoiceViewModel;

		public ChangeViewCommand ChangeViewCommand { get; set; }

		public DelegateCommand<object> AddNewItemCommand { get; private set; }

		public DelegateCommand<Invoice> DisplayInvoiceCommand { get; private set; }
		public DelegateCommand<Customer> DisplayCustomerCommand { get; private set; }

		public DelegateCommand<Customer> AddInvoiceToCustomerCommand { get; private set; }

		public RelayCommand<IClosable> ExitCommand { get; private set; }

		public RelayCommand<object> SaveCommand { get; private set; }


		public MainViewModel()
		{
			ChangeViewCommand = new ChangeViewCommand(ChangeView);
			DisplayInvoiceCommand = new DelegateCommand<Invoice>(DisplayInvoice);
			DisplayCustomerCommand = new DelegateCommand<Customer>(DisplayCustomer);
			ExitCommand = new RelayCommand<IClosable>(Exit);
			SaveCommand = new RelayCommand<object>(Save);

			AddNewItemCommand = new DelegateCommand<object>(AddNewItem, CanAddNewItem);
			AddInvoiceToCustomerCommand = new DelegateCommand<Customer>(AddInvoiceToCustomer);

			customerViewModel = new CustomerViewModel(db);
			invoiceViewModel = new InvoiceViewModel(db);


			VM = customerViewModel;

			//initValues();

		}

		private void ChangeView(string vm)
		{
			switch (vm)
			{
				case "customers":
					VM = customerViewModel;
					break;
				case "invoices":
					VM = invoiceViewModel;
					break;
			}
		}

		private void DisplayInvoice(Invoice invoice)
		{
			invoiceViewModel.SelectedInvoice = invoice;
			VM = invoiceViewModel;
		}

		private void DisplayCustomer(Customer customer)
		{
			customerViewModel.SelectedCustomer = customer;
			VM = customerViewModel;
		}

		private void AddInvoiceToCustomer(Customer c)
		{
			var invoice = new Invoice(c);
			db.Invoices.Add(invoice);
			db.SaveChanges();

			invoiceViewModel.Invoices.Clear();
			invoiceViewModel.Invoices = new ObservableCollection<Invoice>(db.Invoices);

			DisplayInvoice(invoice);
		}

		private void AddNewItem (object item)
		{
			if (VM == customerViewModel)
			{
				var c = new Customer() { Name = "TBD", LastName = "TBD" };
				db.Customers.Add(c);
				db.SaveChanges();

				customerViewModel.Customers.Clear();
				customerViewModel.Customers = new ObservableCollection<Customer>(db.Customers.OrderBy(c => c.LastName));

				customerViewModel.SelectedCustomer = c;
			}
		}

		private bool CanAddNewItem(object o)
		{
			bool result = false;

			result = VM == customerViewModel;
			return result;
		}


		private void Exit(IClosable window)
		{
			if (window != null)
			{
				window.Close();
			}
		}

		private void Save(object o)
		{
			db.SaveChanges();
		}
	}
}
