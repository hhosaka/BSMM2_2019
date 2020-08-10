using System;

namespace BSMM2.Models {

	public interface IPlayer
	{
		string Name { get; }
	}

	public interface IOrderedPlayer : IPlayer
	{
		string Description { get; }
		int Order { get; }
    }
}