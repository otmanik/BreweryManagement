using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Domain.Entities
{
	public class WholesalerStock
	{
		public int Id { get; set; }
		public int WholesalerId { get; set; }
		public Wholesaler Wholesaler { get; set; }
		public int BeerId { get; set; }
		public Beer Beer { get; set; }
		public int Quantity { get; set; }
	}
}