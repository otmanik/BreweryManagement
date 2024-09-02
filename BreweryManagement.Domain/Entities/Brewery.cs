using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Domain.Entities
{
	public class Brewery
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ICollection<Beer> Beers { get; set; } = new List<Beer>();

	}
}
