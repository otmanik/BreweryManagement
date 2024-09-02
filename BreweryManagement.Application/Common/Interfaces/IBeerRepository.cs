﻿using BreweryManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryManagement.Application.Common.Interfaces
{
	public interface IBeerRepository : IRepository<Beer>
	{
	}
}