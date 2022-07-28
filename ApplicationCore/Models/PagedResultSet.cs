using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationCore.Models
{
	public class PagedResultSet<T> where T : class // generic constraint
	{
		public PagedResultSet(IEnumerable<T> data, int pageIndex, int pageSize, int count)
		{
			PageIndex = pageIndex;
			PageSize = pageSize;
			TotalRowCount = count;
			Data = data;
			TotalPages = (int) Math.Ceiling(count / (double)pageSize);
		}
		public int PageIndex { get;}
		public int PageSize { get;}
		public int TotalPages { get; }
		public int TotalRowCount { get;}
		public IEnumerable<T> Data { get; set; }

		// boolean flags
		public bool HasPreviousPage => PageIndex > 1;
		public bool HasNextPage => PageIndex < TotalPages;
	}
}
