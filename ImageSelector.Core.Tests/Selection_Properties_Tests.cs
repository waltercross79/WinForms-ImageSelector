using System.Linq;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ImageSelector.Core.Tests
{
    [TestClass]
    public class Selection_Properties_Tests
    {
        Selection _s;

        [TestInitialize]
        public void Initialize()
        {
            _s = new Selection(new Rectangle(0, 0, 100, 100), 0);
        }

        [TestMethod]
        public void test_TopEdge()
        {
            Assert.AreEqual(new Point(0, 0), _s.TopEdge.Points.First());
            Assert.AreEqual(new Point(100, 0), _s.TopEdge.Points.Skip(1).First());
        }

        [TestMethod]
        public void test_BottomEdge()
        {
            Assert.AreEqual(new Point(0, 100), _s.BottomEdge.Points.First());
            Assert.AreEqual(new Point(100, 100), _s.BottomEdge.Points.Skip(1).First());
        }

        [TestMethod]
        public void test_LeftEdge()
        {
            Assert.AreEqual(new Point(0, 0), _s.LeftEdge.Points.First());
            Assert.AreEqual(new Point(0, 100), _s.LeftEdge.Points.Skip(1).First());
        }

        [TestMethod]
        public void test_RigthEdge()
        {
            Assert.AreEqual(new Point(100, 0), _s.RightEdge.Points.First());
            Assert.AreEqual(new Point(100, 100), _s.RightEdge.Points.Skip(1).First());
        }

        [TestMethod]
        public void test_NWCorner()
        {
            Assert.AreEqual(new Point(0, 0), _s.NWCorner.Coordinates);
        }

        [TestMethod]
        public void test_SWCorner()
        {
            Assert.AreEqual(new Point(0, 100), _s.SWCorner.Coordinates);
        }

        [TestMethod]
        public void test_SECorner()
        {
            Assert.AreEqual(new Point(100, 100), _s.SECorner.Coordinates);
        }

        [TestMethod]
        public void test_NECorner()
        {
            Assert.AreEqual(new Point(100, 0), _s.NECorner.Coordinates);
        }
    }
}
