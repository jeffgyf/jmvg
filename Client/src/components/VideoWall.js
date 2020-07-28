import * as React from 'react';
import ReactDOM from 'react-dom';
import './VideoWall.css';
import SimpleBar from 'simplebar-react'; //https://github.com/Grsmto/simplebar/tree/master/packages/simplebar-react
import 'simplebar/dist/simplebar.min.css';
import VideoCard from './VideoCard';
import VisibilitySensor from 'react-visibility-sensor';
import sampleCover from '../sampleCover.jpg';
import sampleVideoPath from '../sampleVideo.mp4';
import $ from 'jquery';
import config from "../config.json"

let sampleVideo={
  Title:"Sample",
  VideoInfo:null,
  CoverImg:sampleCover,
  Tags:null,
  VideoId:123,
  VideoPath:sampleVideoPath,
};
const sampleVideos=[...Array(20).keys()].map(i=> sampleVideo);

/* parameters
videos
playVideoFunc
*/
export default class VideoWall extends React.Component{
  constructor(props){
    super(props);
    this.state={
      showPlayer: false,
      videos: []
    };
    this.startVideoId = 1;
    this.loadCompleted = false;
    this.loadFalure = null;
  }
  render(){
    var colNum=this.props.colNum;
    const videoRows=((n, a)=>[...Array(Math.floor((a.length+n-1)/n)).keys()].map(i=>a.slice(i*n, (i+1)*n)))(colNum, this.state.videos);
    //console.log(videoRows);
    return (
     
      <div className="VideoWall"> 
        <SimpleBar className="SimpleBar" style={{ width: colNum*(VideoCard.Width+25)+15+'px' }} >
          <table>
            <tbody>
              {videoRows.map(i=>
                <tr>{i.map(k=>
                  <td className="VideoEntry">
                      <VideoCard 
                        title={k.Title} 
                        videoInfo={k.VideoInfo} 
                        coverImg={k.CoverImg} 
                        tags={k.Tags} 
                        videoId={k.VideoId} 
                        playVideoFunc={()=>this.props.playVideoFunc(k.VideoPath)}/>
                  </td>)}
                </tr>)}
                {this.loadCompleted?"":
                  <tr>
                    {this.loadFalure?<div>{`HTTP ${this.loadFalure.status} error`}</div>:<VisibilitySensor delayedCall={true} onChange={v=>this.visibilityOnChange(v)}>
                      <div>loading...</div>
                    </VisibilitySensor>}
                  </tr>}
              </tbody>
          </table>
        </SimpleBar>
      </div>
    );
  }

  visibilityOnChange(isVisible){
    if(isVisible){
      this.loadMoreVideos();
    }
  }

  async getVideoListAsync(){
    const videoNumPerRequest = 20;
    const retryNumLimit=3;
    var retryCnt=0;
    var exception = null;
    while(retryCnt<retryNumLimit){
      ++retryCnt;
      try{
        if(config.localDebug){
          return sampleVideos
        }
        let videos=await $.get(config.serverUrl+`/api/getVideoList?start=${this.startVideoId}&count=${videoNumPerRequest}`);
        if(videos.length==videoNumPerRequest){
          this.startVideoId=videos[videos.length-1].VideoId+1;
        }
        else{
          this.loadCompleted=true;
        }
        return videos;
      }
      catch(e){
        console.log("failed to get video list", e);
        exception=e;
      }
    }
    //alert("failed to get video list, show samples instead");
    this.loadFalure=exception;
    return [];
  }

  loadMoreVideos(){
    this.getVideoListAsync().then(d=>
      {
        var videoList = this.state.videos;
        videoList=videoList.concat(d);
        this.setState({videos:videoList})
      });
  }
}
