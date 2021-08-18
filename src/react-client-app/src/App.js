import React, { Component } from 'react';
import './App.css';

class App extends Component {

  constructor(props) {
    super(props);

    this.state = {
      nick: '',
      type: '',
      message: '',
      messages: [],
      hubConnection: null,
    };
  }

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
            <i className="fa fa-paper-plane-o" aria-hidden="true"></i>
          </button>
        </div>
      </div>
    );
  }

  render() {
    return (
      <div className="container">
        <h3 className="text-center chat_header">MultiChat</h3>
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
