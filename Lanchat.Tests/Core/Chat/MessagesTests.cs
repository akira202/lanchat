using System.ComponentModel.DataAnnotations;
using Lanchat.Core.API;
using Lanchat.Core.Chat;
using Lanchat.Core.Encryption;
using Lanchat.Core.Models;
using Lanchat.Tests.Mock;
using NUnit.Framework;

namespace Lanchat.Tests.Core.Chat
{
    public class MessagesSendTests
    {
        private PublicKeyEncryption publicKeyEncryption;
        private SymmetricEncryption symmetricEncryption;
        private Messaging messaging;
        private NetworkMock networkMock;
        private NetworkOutput networkOutput;
        private Resolver resolver;
        private MessageHandler messageHandler;

        [SetUp]
        public void Setup()
        {
            var nodeState = new NodeState();
            publicKeyEncryption = new PublicKeyEncryption();
            symmetricEncryption = new SymmetricEncryption(publicKeyEncryption);
            publicKeyEncryption.ImportKey(publicKeyEncryption.ExportKey());
            symmetricEncryption.ImportKey(symmetricEncryption.ExportKey());
            networkMock = new NetworkMock();
            networkOutput = new NetworkOutput(networkMock, nodeState);
            messaging = new Messaging(networkOutput, symmetricEncryption);
            messageHandler = new MessageHandler(messaging);
            resolver = new Resolver(nodeState);
            resolver.RegisterHandler(messageHandler);
        }

        [Test]
        public void ValidMessageSend()
        {
            const string testMessage = "test message";
            var receivedMessage = string.Empty;

            messaging.MessageReceived += (_, s) => { receivedMessage = s; };
            networkMock.DataReceived += (_, s) => resolver.CallHandler(s);

            messaging.SendMessage(testMessage);
            Assert.AreEqual(testMessage, receivedMessage);
        }

        [Test]
        public void PrivateMessageSend()
        {
            const string testMessage = "test message";
            var receivedMessage = string.Empty;

            messaging.PrivateMessageReceived += (_, s) => { receivedMessage = s; };
            networkMock.DataReceived += (_, s) => resolver.CallHandler(s);

            messaging.SendPrivateMessage(testMessage);
            Assert.AreEqual(testMessage, receivedMessage);
        }

        [Test]
        public void WeirdText()
        {
            const string testMessage =
                "ẗ̴̝̱̦̝͉͉̬̩̙́̎e̷̡̧̡̢̮̩͓̯̞̼̖̜̥̭̣̙͕̲̳̰̱̾̈͗̉̈́͐́̿̿̕ş̵̡̣̣̳̺̘̲̦͕̣̹̯̰̘̟̰͕̗̰̦͍̩̩̱̩͖̖͍̈́̊͆̾̀̄̾͐̈̈̍̃̔̉̋̐̔͒̒̍̎̇̏͌̑̚͜t̴͙̭̠͇̹̫͇̗̥̗͍̀̒̈́́͑̈́̃͌̽̈́̏̈̉͘̕̚͜͝ͅͅ";
            var receivedMessage = string.Empty;

            messaging.MessageReceived += (_, s) => { receivedMessage = s; };
            networkMock.DataReceived += (_, s) => resolver.CallHandler(s);

            messaging.SendMessage(testMessage);
            Assert.AreEqual(testMessage, receivedMessage);
        }
        
        [Test]
        public void InvalidFormatCatch()
        {
            messageHandler.Handle(new Message {Content = "not a base64"});
        }

        [Test]
        public void InvalidEncryptionCatch()
        {
            messageHandler.Handle(new Message {Content = "bm90IGVuY3J5cHRlZA=="});
        }
    }
}