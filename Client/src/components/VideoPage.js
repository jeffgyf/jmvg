import * as React from 'react';
import UploadVideoButton from './UploadVideoButton'
import VideoWall from './VideoWall'
import VideoPlayer from './VideoPlayer'
import './VideoPage.css';
import sampleCover from '../sampleCover.png';
import sampleVideoPath from '../sampleVideo.mp4';
import $ from 'jquery';
import config from "../config.json"
function sleep(ms) {
  return new Promise(resolve => setTimeout(resolve, ms));
}

let sampleVideo={
  Title:"Sample",
  VideoInfo:null,
  CoverImg:sampleCover,
  Tags:null,
  VideoId:123,
  VideoPath:sampleVideoPath,
};
const sampleVideos=[...Array(10).keys()].map(i=> sampleVideo);


export default class VideoPage extends React.Component {
    constructor(props){
      super(props);
      
      this.state={
        videoWallList:[],
        showVideoPlayer:false,
        videoSrc:""
      };
      this.refreshVideoList();
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
                <VideoWall videos={this.state.videoWallList} playVideoFunc={v=>this.playVideo(v)}/>
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

    async getVideoListAsync(){
      try{
        if(config.localDebug){
          return sampleVideos
        }
        let videos=await $.get(config.serverUrl+"/api/getVideoList");
        console.log("videos");
        console.log(videos);
        
        return videos;
      }
      catch(error){
        alert("failed to get video list, show samples instead")
        console.log(error);
        return sampleVideos;
      }
    }




}

