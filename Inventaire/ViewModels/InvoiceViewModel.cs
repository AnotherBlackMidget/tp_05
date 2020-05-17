
using BillingManagement.Models;
using BillingManagement.UI.Contexts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace BillingManagement.UI.ViewModels
{
	class InvoiceViewModel : BaseViewModel
	{
		public CustomerContext db;


		private Invoice selectedInvoice;
		private ObservableCollection<Invoice> invoices;

		public InvoiceViewModel(CustomerContext db)
		{
			Invoices = new ObservableCollection<Invoice>(db.Invoices);
			this.db = db;
		}

		public Invoice SelectedInvoice
		{
			get { return selectedInvoice; }
			set { 
				selectedInvoice = value;
				OnPropertyChanged();
			}
		}

		public ObservableCollection<Invoice> Invoices { 
			get => invoices;
			set { 
				invoices = value;
				OnPropertyChanged();
			}
		}

	}
}
