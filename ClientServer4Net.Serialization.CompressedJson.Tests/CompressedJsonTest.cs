using ClientServer4Net.Serialization.CompressedJson.Tests.Messages;
using NUnit.Framework;

namespace ClientServer4Net.Serialization.CompressedJson.Tests
{
    [TestFixture]
    public class CompressedJsonTest
    {
        [Test]
        public void TestSerializeAndDeserialize()
        {
            var serializer = new Serializer();
            var message = new SimpleMessage();
            message.Field1 = "asdasdasdas";
            message.Field2 = 252324;
            var data = serializer.Serialize(message);
            var msg = serializer.Deserialize(data);
            Assert.IsTrue(msg is SimpleMessage);
            var simpleMessage = (SimpleMessage) msg;
            Assert.AreEqual(message.Field1, simpleMessage.Field1);
            Assert.AreEqual(message.Field2, simpleMessage.Field2);
        }
    }
}
