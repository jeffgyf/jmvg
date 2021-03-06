import * as React from 'react';
import UploadVideoButton from './UploadVideoButton'
import VideoWall from './VideoWall'
import VideoPlayer from './VideoPlayer'
import './VideoPage.css';


export default class VideoPage extends React.Component {
  static windowWidthThreshold = 860;
  constructor(props){
    super(props);
    
    this.state={
      showVideoPlayer:false,
      videoSrc:"",
      videoWallColNum:2
    };
    if(window.innerWidth<=VideoPage.windowWidthThreshold){
      this.state.videoWallColNum=1;
    }
    //this.refreshVideoList();
    window.addEventListener('resize', ()=>this.onPageResize());
  }

  refreshVideoList(){
    this.getVideoListAsync().then(d=>this.setState({videoWallList:d}));
  }
  
  render() {
      return (
        <div className="VideoPage">
            <div className="Title">J.M.V.G</div>
            <div className="Body">
              <UploadVideoButton/>
              <VideoWall videos={this.state.videoWallList} loadMore={()=>this.loadMoreVideos()} playVideoFunc={v=>this.playVideo(v)} colNum={this.state.videoWallColNum}/>
            </div>
            <VideoPlayer showPlayer={this.state.showVideoPlayer} videoSrc={this.state.videoSrc} closePlayer={()=>this.closeVideoPlayer()}/>
        </div>
      );
  }

  playVideo(videoSrc){
    this.setState({videoSrc:videoSrc, showVideoPlayer:true});
  }

  closeVideoPlayer(){
    this.setState({showVideoPlayer:false});
  }

  onPageResize(){
    if(window.innerWidth<=VideoPage.windowWidthThreshold){
      if(this.state.videoWallColNum>1){
        this.setState({videoWallColNum:1});
      }
    }
    else if(this.state.videoWallColNum==1){
      this.setState({videoWallColNum:2});
    }
  }

}

