using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Domain.Entities
{
	public class Wholesaler
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<WholesalerStock> Stock { get; set; } = new List<WholesalerStock>();
	}
}