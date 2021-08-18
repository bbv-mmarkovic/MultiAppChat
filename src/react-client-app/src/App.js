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
      clientId: '',
      type: '',
      message: '',
      messages: [],
      hubConnection: null,
    };
  }

  componentDidMount = () => {
    const clientId = Date.length;
    this.setState({ clientId });

    const hubConnection = new HubConnectionBuilder()
      .withUrl('http://localhost:5000/MessageHub')
      .build();

    this.setState({ hubConnection }, () => {
      this.state.hubConnection
        .start()
        .then(() => console.log('Connection started!'))
        .catch(err => console.log('Error while establishing connection :('));

      this.state.hubConnection.on('MessageReceived', (receivedMessage) => {
        console.log('Message received: ' + receivedMessage.message);
        const messages = this.state.messages.concat([receivedMessage]);
        this.setState({ messages });
      });
    });
  };

  sendMessage = () => {
    var message = { 
      clientuniqueid: this.state.clientId, 
      type: 'sent',
      message: this.state.message,
      date: Date.now()
    };

    this.state.hubConnection.invoke('NewMessage', message);
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
            <span class="time_date">{msg.date}</span>
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

  renderSendMessage() {
    return (
      <div className="type_msg">
        <div className="input_msg_write">
          <input type="text" className="write_msg" placeholder="Type a message" />
          <button className="msg_send_btn" type="button">
            <FontAwesomeIcon icon={faPaperPlane} />
          </button>
        </div>
      </div>
    );
  }

  render() {
    return (
      <div className="container">
        <h3 className="text-center chat_header"><img src={reactImg} width="50" height="50" alt="" /> MultiChat</h3>
        <div className="messaging">
          <div className="inbox_msg">
            <div className="mesgs">
              <div className="msg_history">
                {this.state.messages.map((message, index) => (
                  <div>
                    {message.type === 'received'
                      ? this.renderIncoming(message)
                      : this.renderSent(message)
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
