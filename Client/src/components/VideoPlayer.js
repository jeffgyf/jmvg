import React from 'react';
import { Player } from 'video-react';
import "../../node_modules/video-react/dist/video-react.css";
import './VideoPlayer.css';
import { Dialog } from 'office-ui-fabric-react';

/* parameters
showPlayer:bool
videoSrc:string
*/
export default class VideoPlayer extends React.PureComponent {

  constructor(props){
    super(props);
  }
  render() {
    return (
      <Dialog 
        className="VideoPlayer"
        maxWidth="50%"
        onDismiss={()=>this.props.closePlayer()}
        hidden={!this.props.showPlayer}>
        <Player ref={player => { this.player = player }}
          src={this.props.videoSrc}
          autoPlay
        />
      </Dialog>
    )
  }



 
}

