using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace posWebApp.Models
{
    public class ItemUnitViewModel
    {
        public IEnumerable<ItemUnitModel> ItemsUnits { get; set; }
        public int branchId { get; set; }
        public int ItemPerPage { get; set; } = Global.rowsInPage;
        public int CurrentPage { get; set; }

        public int PageCount()
        {
            return Convert.ToInt32(Math.Ceiling(ItemsUnits.Count() / (double)ItemPerPage));
        }
        public IEnumerable<ItemUnitModel> PaginatedBlogs()
        {
            int start = (CurrentPage - 1) * ItemPerPage;
            return ItemsUnits.OrderBy(b => b.itemName).Skip(start).Take(ItemPerPage);
        }
    }
}