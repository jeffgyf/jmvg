import * as React from 'react';
import ReactDOM from 'react-dom';
import EventWall from './EventWall'
import SlideBar from './SlideBar'
import './EventPage.css';
import soccer from '../soccer.png';
import CreateEventDialog from './CreateEventDialog';
import $ from 'jquery';
import config from '../config';
import CookieCheck from "./CookieCheck";
function sleep(ms) {
  return new Promise(resolve => setTimeout(resolve, ms));
}

const eventInfoSample={
  location:"1 Microsoft Way, Redmond, WA",
  date:"Friday, July 12, 2019",
  time:"5:25pm"
}
const fakeEvent={
  "eventid": "1111-1111-1111-2222",
  "name": "read book",
  "ownerid": "2222-2222-2222-2222",
  "starttime": "2017-01-01 00:00:00",
  "location": "microsoft building 76",
  "ParticipantCount": 5,
  "Description": "read books in library"
};
const fakeEvents=[
  ...Array(10).keys()
].map(i=> fakeEvent);


export default class EventPage extends React.Component {
    constructor(props){
      super(props);
      
      this.state={
        eventWallEventList:[],
        SlideBarEventList:[]
      };
      this.getEventsToJoinAsync().then(d=>this.setState({eventWallEventList:d}));
      this.refreshJoinedEvents();

    }

    refreshJoinedEvents(){
      this.getEventsJoinedAsync().then(d=>this.setState({SlideBarEventList:d}));
    }
    
    render() {
        return (
        <div className="EventPage">
            <CookieCheck/>
            <EventWall events={this.state.eventWallEventList} refreshJoinedHandler={()=>this.refreshJoinedEvents()}/>
            <SlideBar events={this.state.SlideBarEventList}/>
            <CreateEventDialog />
        </div>
      );
    }

    async getEventsToJoinAsync(){
      try{
        let parsedEvents=await $.get(config.BackEndAPIUrl+"/getallevents");
        //console.log(parsedEvents);
        let eventList=parsedEvents.map(ff=>({
          title: ff.Event.Name,
          eventInfo:{
            location: ff.Event.Location, 
            time: ff.Event.StartTime,
          },
          interests:ff.Interests,
          img: soccer,
          eventId:ff.Event.EventID,
        }));
        return eventList;
      }
      catch(error){
        return [];
      }
      //let eventList=[...Array(10).keys()].map(i=> ({title:"Test"+i, eventInfo:eventInfoSample, img:soccer}));
    }

    async getEventsJoinedAsync(){
      let userName = await CookieCheck.UserNamePromise;
      try{
        
        let parsedEvents=await $.get(config.BackEndAPIUrl+"/getjoinedevents?username="+userName);
        console.log("parsedEvent");
        console.log(parsedEvents);
        let eventList=parsedEvents==null?[]:parsedEvents.map(ff=>({
          title: ff.Name,
          eventInfo:{
            location: ff.Location, 
            time: ff.StartTime
          },
          img: soccer
        }));
        
        return eventList;
      }
      catch(error){
        alert("failed to get joined events for "+userName)
        console.log(error);
        return [];
      }
    }


}

