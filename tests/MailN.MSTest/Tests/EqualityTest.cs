using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace Acklann.MailN.MSTest.Tests
{
    [TestClass]
    public class EqualityTest
    {
        [TestMethod]
        public void Can_evaluate_address_equality()
        {
            Address a = new Address("123 Main Street", null, "Tarry Town", "Eldin", "Hyrule", "52963");
            Address b = new Address();
            Address c = new Address("123 main street", null, "tarry town", "eldin", "hyrule", "52963");

            (a == c).ShouldBeTrue();
            (a == b).ShouldBeFalse();

            (a != c).ShouldBeFalse();
            (a != b).ShouldBeTrue();

            (a.Equals(c)).ShouldBeTrue();
            (a.Equals(b)).ShouldBeFalse();

            (a.GetHashCode() == c.GetHashCode()).ShouldBeTrue();
            (a.GetHashCode() == b.GetHashCode()).ShouldBeFalse();
        }
    }
}