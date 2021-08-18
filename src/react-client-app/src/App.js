import React, { Component } from 'react';
import './App.css';
import reactImg from './react.png';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPaperPlane } from '@fortawesome/free-regular-svg-icons';
import { HubConnectionBuilder } from '@aspnet/signalr';

class App extends Component {

  constructor(props) {
    super(props);

    this.state = {
      clientId: Date.length,
      type: '',
      currentMessage: '',
      messages: [],
      hubConnection: null,
    };
  }

  componentDidMount = () => {
    this.startConnection();
    //this.registerOnServerEvents();
  };

  startConnection = () => {
    const hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/MessageHub')
      .build();

    this.setState({ hubConnection }, () => {
      this.state.hubConnection
        .start()
        .then(() => console.log('Connection started!'))
        .catch(err => {
          console.log('Error while establishing connection :(' + err);
          setTimeout(function() { this.startConnection(); }, 5000);
        });

        this.state.hubConnection
          .on('MessageReceived', (receivedMessage) => {
            console.log('Message received: ' + receivedMessage.message);
            const messages = this.state.messages.concat(receivedMessage);
            this.setState({ messages });
        });
    });
  };

  registerOnServerEvents = () => {
    this.state.hubConnection
      .on('MessageReceived', (receivedMessage) => {
        console.log('Message received: ' + receivedMessage.message);
        const messages = this.state.messages.concat(receivedMessage);
        this.setState({ messages });
    });
  };

  sendMessage = () => {
    const newMessage = { 
      clientuniqueid: this.state.clientId, 
      type: 'sent',
      message: this.state.currentMessage,
      date: Date.now()
    };

    console.log('hub state: ' + this.state.hubConnection.state);
    console.log('sending: ' + newMessage.message);

    this.state.hubConnection.invoke('NewMessage', newMessage); /*
      .catch(err => console.log(err))*/;

    const currentMessage = '';
    this.setState({ currentMessage });
  };

  renderIncoming(msg) {
    return (
      <div className="incoming_msg">
        <div className="incoming_msg_img"> </div>
        <div className="received_msg">
          <div className="received_withd_msg">
            <p>
              {msg.message}
            </p>
            <span className="time_date">{msg.date}</span>
          </div>
        </div>
      </div>
    );
  }

  renderSent(msg) {
    return (
      <div className="outgoing_msg">
        <div className="sent_msg">
          <p>
            {msg.message}
          </p>
          <span className="time_date">{msg.date}</span>
        </div>
      </div>
    );
  }

  handleKeyDown = (e) => {
    if (e.key === 'Enter' || e.key === 'NumpadEnter') {
      this.sendMessage();
    }
  };

  renderSendMessage() {
    return (
      <div className="type_msg">
        <div className="input_msg_write">
          <input type="text" className="write_msg" placeholder="Type a message"
            value={this.state.currentMessage}
            onKeyDown={this.handleKeyDown}
            onChange={e => this.setState({ currentMessage: e.target.value })}
          />
          <button className="msg_send_btn" type="button" onClick={this.sendMessage}>
            <FontAwesomeIcon icon={faPaperPlane} />
          </button>
        </div>
      </div>
    );
  }

  render() {
    return (
      <div className="container">
        <h1 className="text-center chat_header"><img src={reactImg} width="50" height="50" alt="" /> MultiChat</h1>
        <div className="messaging">
          <div className="inbox_msg">
            <div className="mesgs">
              <div className="msg_history">
                {this.state.messages.map((message, _) => (
                  <div>
                    {message.clientuniqueid === this.clientId
                      ? this.renderSent(message)
                      : this.renderIncoming(message)
                    }
                  </div>
                ))}
              </div>
              {this.renderSendMessage()}
            </div>
          </div>
        </div>
      </div>
    );
  }
}

export default App;
