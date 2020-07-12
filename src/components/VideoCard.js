import React from 'react';
import { 
  Text,
  DocumentCard,
  DocumentCardPreview,
  ActionButton} from 'office-ui-fabric-react';
import './VideoCard.css';
import { ImageFit } from 'office-ui-fabric-react/lib/Image';
import $ from 'jquery';
import config from '../config';
import CookieCheck from './CookieCheck';
var logo="https://img-prod-cms-rt-microsoft-com.akamaized.net/cms/api/am/imageFileData/RE1Mu3b?ver=5c31";




export default class VideoCard extends React.PureComponent {
  static Width=200;
  render() {
    const previewProps= {
      previewImages: [{
          previewImageSrc: `./soccer.png`,
          imageFit: ImageFit.cover,
          width: VideoCard.Width,
          height: 120
        }
      ]
    };
    return (
      <DocumentCard className="VideoCard" style={{ width: VideoCard.Width+'px' }}>
        {this.props.img ? <DocumentCardPreview {...previewProps}/>:null}
        <div className="content">
          <p className="title">{this.props.title}</p>
            <div className="videoInfo">
              {Object.keys(this.props.videoInfo).map(i=> <p><Text>{this.props.videoInfo[i]}</Text></p>)}
              {this.props.interests? <p>{this.props.interests.reduce((s, i)=> s+";"+i)}</p>:null}
            </div>
            <div className="playButton" hidden={this.props.img==null}>
              <ActionButton data-automation-id="test" iconProps={{ iconName: 'Play' }} onClick={()=>{this.eventJoinHandler()}}>
              </ActionButton>
            </div>
        </div>
        
        {/*<DocumentCardActivity
          activity="Created a few minutes ago"
          people={[{ name: 'Annie Lindqvist', profileImageSrc: null }]}
        />*/}

      </DocumentCard>
    );
  }

  eventJoinHandler(){
    this.postJoinEventAsync(this.props.eventId);
  }

  async postJoinEventAsync(eventId){
    console.log(eventId);
    let username=await CookieCheck.UserNamePromise;
    let data={
      eventId:eventId,
      username:username
    };
    try{
      let resp=await $.post(config.BackEndAPIUrl+"/joinevent", JSON.stringify(data));
      alert("join success");
      console.log(resp);
      this.props.refreshJoinedHandler();
    }
    catch(error){
      alert("join failed");
      console.log(error);
    }
  }
}
