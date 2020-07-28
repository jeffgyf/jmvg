import * as React from 'react';
import ReactDOM from 'react-dom';
import VideoCard from './components/VideoCard'
import VideoWall from './components/VideoWall'
import VideoPage from './components/VideoPage'
import sampleCover from './sampleCover.jpg';
import sampleVideoPath from './sampleVideo.mp4';
import {initializeIcons} from 'office-ui-fabric-react';
import { BrowserRouter as Router, Route, Link, HashRouter, withRouter  } from "react-router-dom";
import $ from 'jquery';
import VideoPlayer from './components/VideoPlayer';
class IndexPage extends React.Component{
  constructor(){
    super();
    this.state={cookie:document.cookie};
  }
  render(){
    return (
    <div>
      <p>JMVG Debugging page</p>
      <a href="?route=videoCard">{"VideoCard"}</a>
      <br/>
      <a href="?route=videoPlayer">{"VideoPlayer"}</a>
      <br/>
      <a href="?route=videoWall">{"ViedoWall"}</a>
      <br/>
      <a href="?route=videoPage">{"ViedoPage"}</a>
    </div>);
  }
}
function getUrlParam(name){
	var result = new RegExp('[\?&]' + name + '=([^&#]*)').exec(window.location.href);
	return result==null?"home":result[1];
};
initializeIcons();
console.log($.urlParms);
let sampleVideo={
  Title:"Title",
  VideoInfo:null,
  CoverImg:sampleCover,
  Tags:null,
  VideoId:123,
  VideoPath:sampleVideoPath,
  playVideoFunc:null
};
let routeDict={
  "home":<IndexPage/>,
  "videoCard": <VideoCard 
    title="Title" 
    videoInfo={["Composer: Ayano Tsuji", `From "The Cat Returns"`, "Violin"]}
    img={sampleCover}
    tag={null}
    videoId="123"
    videoPath={sampleVideo}
    playVideoFunc={null}/>,
  "videoPlayer": <VideoPlayer showPlayer={true} videoSrc={sampleVideoPath}/>,
  "videoWall": <VideoWall colNum={2} videos={[...Array(20).keys()].map(i=> sampleVideo)}/>,
  "videoPage": <VideoPage/>,
}
//const eventList=[...Array(15).keys()].map(i=>({title:"test"+i, eventInfo:eventInfoSample, img:soccer}));
//const InterestList=["Tag1", "Tag2", "Tag3"]
//<Profile image={user} name={"Mr Mario"} Interest_tag={InterestList} events={eventList} />,
ReactDOM.render(
  routeDict[getUrlParam("route")],
  document.getElementById('root')
);
//  <SlideBar events={eventList}/>
//   <EventCard title="Sample Event" eventInfo={eventInfoSample} img={soccer}/>