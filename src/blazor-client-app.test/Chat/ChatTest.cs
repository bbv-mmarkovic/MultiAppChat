namespace blazor_client_app.Chat
{
    using System;

    using AngleSharp.Dom;
    using Bunit;
    using BTestContext = Bunit.TestContext;
    using FakeItEasy;
    using FluentAssertions;
    using NUnit.Framework;

    public class ChatTest
    {
        private BTestContext testContext;

        [SetUp]
        public void Setup()
        {
            this.testContext = new BTestContext();
        }

        [TearDown]
        public void TearDown()
        {
            this.testContext.Dispose();
        }

        [Test]
        public void RendersComponent()
        {
            var chatService = A.Fake<IChatService>();

            var chatComponent = this.SetupComponent(chatService);

            chatComponent.Markup.Contains("MultiChat");
        }

        [Test]
        public void ConnectsToTheChatServiceOnStartup()
        {
            var chatService = A.Fake<IChatService>();

            var chatComponent = this.SetupComponent(chatService);

            A.CallTo(() => chatService.InitializeAsync(A<Action<ChatMessage>>._)).MustHaveHappened();
        }

        [Test]
        public void SendsAMessageToTheChatServiceAndClearsTheInput()
        {
            const string SampleMessage = "Hello from Bunit!";

            var chatService = A.Fake<IChatService>();

            var chatComponent = this.SetupComponent(chatService);

            IElement textInputElement = chatComponent.Find("#textInput");
            textInputElement.Should().NotBeNull();

            IElement sendButtonElement = chatComponent.Find("#sendButton");
            sendButtonElement.Should().NotBeNull();

            textInputElement.Change(SampleMessage);
            textInputElement.GetAttribute("value").Should().Be(SampleMessage);

            sendButtonElement.Click();

            A.CallTo(() => chatService.SendAsync(SampleMessage)).MustHaveHappened();
            textInputElement.GetAttribute("value").Should().BeEmpty();
        }

        private IRenderedComponent<Chat> SetupComponent(IChatService chatService)
        {
            this.testContext.Services.AddFallbackServiceProvider(new FallbackTestServiceProvider(chatService));
            return testContext.RenderComponent<Chat>();
        }

        public class FallbackTestServiceProvider : IServiceProvider
        {
            private readonly object serviceObject;

            public FallbackTestServiceProvider(object serviceObject)
            {
                this.serviceObject = serviceObject;
            }

            public object GetService(Type serviceType)
            {
                return this.serviceObject;
            }
        }
    }
}