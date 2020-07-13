import * as React from 'react';
import ReactDOM from 'react-dom';
import EventCard from './components/EventCard'
import VideoCard from './components/VideoCard'
import EventPage from './components/EventPage'
import VideoWall from './components/VideoWall'
import VideoPage from './components/VideoPage'
import ProfilePage from './components/ProfilePage'
import soccer from './soccer.png';
import sampleCover from './sampleCover.png';
import sampleVideoPath from './become_wind.mp4';
import {initializeIcons} from 'office-ui-fabric-react';
import { BrowserRouter as Router, Route, Link, HashRouter, withRouter  } from "react-router-dom";
import InterestEventPicker from './components/InterestEventPicker'
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
      <a href="?route=eventPage">{"eventPage"}</a>
      <br/>
      <a href="?route=profilePage">{"ProfilePage"}</a>
      <br/>
      <a href="?route=videoCard">{"VideoCard"}</a>
      <br/>
      <a href="?route=videoPlayer">{"VideoPlayer"}</a>
      <br/>
      <a href="?route=videoWall">{"ViedoWall"}</a>
      <br/>
      <a href="?route=videoPage">{"ViedoPage"}</a>
      <p>{"cookie string: "+ this.state.cookie}</p>
      <button onClick={()=> {
        document.cookie="hello"+(new Date()).getTime();
        this.setState({cookie: document.cookie});
      }}>{"test cookie"}</button>
      <button onClick={()=>{
        document.cookie = "username=<logged_out>";
        this.setState({cookie:document.cookie});
      }}>{"log out"}</button>
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
  "eventPage":<EventPage/>,
  "home":<IndexPage/>,
  "videoCard": <VideoCard 
    title="Title" 
    videoInfo=""
    img={soccer}
    tag={null}
    videoId="123"
    videoPath=""
    playVideoFunc={null}/>,
  "videoPlayer": <VideoPlayer showPlayer={true}/>,
  "videoWall": <VideoWall videos={[...Array(10).keys()].map(i=> sampleVideo)}/>,
  "videoPage": <VideoPage/>,
  "profilePage":<ProfilePage/>
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