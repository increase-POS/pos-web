using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace posWebApp.Models
{
    public class InvoiceViewModel
    {
        public IEnumerable<InvoiceModel> Invoices { get; set; }
        public AgentModel Agent { get; set; }
        public int InvoicePerPage { get; set; } = Global.rowsInPage;
        public int CurrentPage { get; set; }

        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(Invoices.Count() / (double)InvoicePerPage));
        }
        public IEnumerable<InvoiceModel> PaginatedBlogs()
        {
            int start = (CurrentPage - 1) * InvoicePerPage;
            return Invoices.Skip(start).Take(InvoicePerPage);
        }
    }
}