using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Tekcari.MailN.Tests
{
	[TestClass]
	public class EqualityTest
	{
		[TestMethod]
		public void Can_evaluate_address_equality()
		{
			Address a = new Address("123 Main Street", null, "Tarry Town", "Eldin", "52963", "Hyrule");
			Address b = new Address();
			Address c = new Address("123 main street", null, "tarry town", "eldin", "52963", "hyrule");

			(a == c).ShouldBeTrue();
			(a == b).ShouldBeFalse();

			(a != c).ShouldBeFalse();
			(a != b).ShouldBeTrue();

			(a.Equals(c)).ShouldBeTrue();
			(a.Equals(b)).ShouldBeFalse();

			(a.GetHashCode() == c.GetHashCode()).ShouldBeTrue();
			(a.GetHashCode() == b.GetHashCode()).ShouldBeFalse();
		}

		[TestMethod]
		public void Can_evaluate_name_equality()
		{
			// Arrange
			var empty = new FullName();
			var john = new FullName("john");
			var John = new FullName("John");
			var john_smith = new FullName("john", "smith");

			(john == John).ShouldBeTrue();
			(john == john_smith).ShouldBeFalse();
			(john != john_smith).ShouldBeTrue();

			empty.GetHashCode().ShouldNotBe(john.GetHashCode());
		}
	}
}
