﻿using ApplicationCore.Interfaces;

namespace ApplicationCore.Models.Cinema.Films
{
	internal class Person : IFullName
	{
		public string FirstName { get; set; }
		public string MiddleName { get; set; }
		public string LastName { get; set; }
		public string Post { get; set; }

		public Person(string firstName, string middleName, string lastName, string post)
		{
			FirstName = firstName;
			MiddleName = middleName;
			LastName = lastName;
			Post = post;
		}
	}
}