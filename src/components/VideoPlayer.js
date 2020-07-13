import React from 'react';
import { Player } from 'video-react';
import "../../node_modules/video-react/dist/video-react.css";
import sampleVideo from '../become_wind.mp4';
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
        maxWidth="80%"
        onDismiss={()=>this.props.closePlayer()}
        hidden={!this.props.showPlayer}>
        <Player
          src={this.props.videoSrc}
          autoPlay
        />
      </Dialog>
    )
  }



 
}

