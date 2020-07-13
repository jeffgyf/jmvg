import * as React from 'react';
import UploadVideoButton from './UploadVideoButton'
import VideoWall from './VideoWall'
import VideoPlayer from './VideoPlayer'
import './VideoPage.css';
import sampleCover from '../sampleCover.png';
import sampleVideoPath from '../become_wind.mp4';
import $ from 'jquery';
import config from '../config';
function sleep(ms) {
  return new Promise(resolve => setTimeout(resolve, ms));
}

let sampleVideo={
  Title:"Title",
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
        
        //let parsedEvents=await $.get(config.BackEndAPIUrl+"/getjoinedevents?username="+userName);
        //console.log("parsedEvent");
        //console.log(parsedEvents);
        
        return sampleVideos;
      }
      catch(error){
        alert("failed to get video list")
        console.log(error);
        return [];
      }
    }




}

