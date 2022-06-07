using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace posWebApp.Models
{
    public class CashTransferViewModel
    {
        public IEnumerable<CashTransferModel> CashTransfers { get; set; }
        public AgentModel Agent { get; set; }
        public int CashPerPage { get; set; } = Global.rowsInPage;
        public int CurrentPage { get; set; }

        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(CashTransfers.Count() / (double)CashPerPage));
        }
        public IEnumerable<CashTransferModel> PaginatedBlogs()
        {
            int start = (CurrentPage - 1) * CashPerPage;
            return CashTransfers.OrderBy(b => b.transNum).Skip(start).Take(CashPerPage);
        }
    }
}