import React from 'react';
import { Player } from 'video-react';
import "../../node_modules/video-react/dist/video-react.css";
import sampleVideo from '../become_wind.mp4';
import { Dialog } from 'office-ui-fabric-react';


export default class VideoPlayer extends React.PureComponent {
  constructor(props){
    super(props);
    
    this.state={
      hideDialog: false
    };

  }
  render() {
    return (
      <Dialog 
        maxWidth="80%"
        onDismiss={()=>this.setState({hideDialog:true})}
        hidden={this.state.hideDialog}>
        <Player
          src={sampleVideo}
          autoPlay
        />
      </Dialog>
    )
  }



 
}

