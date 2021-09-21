using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;
using System;
using System.Collections.Generic;

namespace Tekcari.Usaddress.Tests
{
	[TestClass]
	public class SerializationTest
	{
		[TestMethod]
		[DynamicData(nameof(GetAddressDeserializationTestCases), DynamicDataSourceType.Method)]
		public void Can_parse_address(string text, string street1, string street2, string city, string state, string country, string zip)
		{
			var sut = Address.Parse(text);

			ShouldBeStringTestExtensions.ShouldBe(sut.Line1, street1);
			ShouldBeStringTestExtensions.ShouldBe(sut.Line2, street2);
			ShouldBeStringTestExtensions.ShouldBe(sut.City, city);
			ShouldBeStringTestExtensions.ShouldBe(sut.State, state);
			ShouldBeStringTestExtensions.ShouldBe(sut.Country, country);
			ShouldBeStringTestExtensions.ShouldBe(sut.PostalCode, zip);
		}

		[TestMethod]
		[DynamicData(nameof(GetAddressFormats), DynamicDataSourceType.Method)]
		public void Can_format_address(string format, string expected)
		{
			var address = new Address("a", "b", "c", "d", "f", "e");
			address.ToString(format).ShouldBe(expected);
		}

		[TestMethod]
		[DynamicData(nameof(GetNames), DynamicDataSourceType.Method)]
		public void Can_parse_a_name(string text, Action<FullName> assert)
		{
			assert.Invoke(FullName.Parse(text));
		}

		#region Backing Members

		private static IEnumerable<object[]> GetAddressFormats()
		{
			yield return new object[] { "1", "a" };
			yield return new object[] { "0", "a b" };
			yield return new object[] { "G", "STREET1=a;STREET2=b;CITY=c;STATE=d;COUNTRY=e;ZIP=f" };
		}

		private static IEnumerable<object[]> GetAddressDeserializationTestCases()
		{
			yield return new object[] { null, null, null, null, null, null, null };
			yield return new object[] { string.Empty, null, null, null, null, null, null };
			yield return new object[] { "PO Box 8790;;Domino City;Marley;US;00878", null, null, null, null, null, null };
			yield return new object[] { "st1=123 Conny Avenue;st2=apt #45;cty=Domino City;s=Marley;co=US;zip=00878", "123 Conny Avenue", "apt #45", "Domino City", "Marley", "US", "00878" };
			yield return new object[] { "street1=PO Box 8790;street2=;city=Domino City;state=Marley;country=US;zip=00878", "PO Box 8790", string.Empty, "Domino City", "Marley", "US", "00878" };

			yield return new object[] { "st1=a", "a", null, null, null, null, null };
			yield return new object[] { "st2=b", null, "b", null, null, null, null };
			yield return new object[] { "cty=c", null, null, "c", null, null, null };
			yield return new object[] { "zip=f", null, null, null, null, null, "f" };
			yield return new object[] { "street1=a;state=d", "a", null, null, "d", null, null };
		}

		private static IEnumerable<object[]> GetNames()
		{
			yield return new object[]
			{
				"a",
				new Action<FullName>(x=> {
					x.FirstName.ShouldBe("a");
					x.LastName.ShouldBeNull();
					x.MiddleName.ShouldBeNull();
					x.Suffix.ShouldBeNull();
				})
			};

			yield return new object[]
			{
				"a b",
				new Action<FullName>(x=> {
					x.FirstName.ShouldBe("a");
					x.LastName.ShouldBe("b");
					x.MiddleName.ShouldBeNull();
					x.Suffix.ShouldBeNull();
				})
			};

			yield return new object[]
			{
				"john smith Jr.",
				new Action<FullName>(x=> {
					x.FirstName.ShouldBe("john");
					x.LastName.ShouldBe("smith");
					x.MiddleName.ShouldBeNull();
					x.Suffix.ShouldBe("Jr.");
				})
			};

			yield return new object[]
			{
				"john avery smith",
				new Action<FullName>(x=> {
					x.FirstName.ShouldBe("john");
					x.LastName.ShouldBe("smith");
					x.MiddleName.ShouldBe("avery");
					x.Suffix.ShouldBeNull();
				})
			};

			yield return new object[]
			{
				"john abe smith",
				new Action<FullName>( x => {
					x.FirstName.ShouldBe("john");
					x.LastName.ShouldBe("smith");
					x.MiddleName.ShouldBe("abe");
					x.Suffix.ShouldBeNull();
				})
			};

			yield return new object[]
			{
				"john a. smith",
				new Action<FullName>( x => {
					x.FirstName.ShouldBe("john");
					x.LastName.ShouldBe("smith");
					x.MiddleName.ShouldBe("a.");
					x.Suffix.ShouldBeNull();
				})
			};

			yield return new object[]
			{
				"john smith jr.",
				new Action<FullName>( x => {
					x.FirstName.ShouldBe("john");
					x.LastName.ShouldBe("smith");
					x.MiddleName.ShouldBeNull();
					x.Suffix.ShouldBe("jr.");
				})
			};

			yield return new object[]
			{
				"john avery smith CPA",
				new Action<FullName>( x => {
					x.FirstName.ShouldBe("john");
					x.LastName.ShouldBe("smith");
					x.MiddleName.ShouldBe("avery");
					x.Suffix.ShouldBe("CPA");
				})
			};
		}

		#endregion Backing Members
	}
}
