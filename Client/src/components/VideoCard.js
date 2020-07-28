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
import sampleCover from '../sampleCover.png';
var logo="https://img-prod-cms-rt-microsoft-com.akamaized.net/cms/api/am/imageFileData/RE1Mu3b?ver=5c31";



/* Parameters
  title="Title" 
  videoInfo=""
  coverImg={soccer}
  tags={null}
  videoId="123"
  videoPath=""
  playVideoFunc={null}*/
export default class VideoCard extends React.PureComponent {
  static Width=350;
  static ImageSize=100;
  render() {
    const previewProps= {
      previewImages: [{
          previewImageSrc: this.props.coverImg ? this.props.coverImg : sampleCover,
          imageFit: ImageFit.centerContain,
          width: VideoCard.ImageSize,
          height: VideoCard.ImageSize
        }
      ]
    };
    return (
      <DocumentCard className="VideoCard" style={{ width: VideoCard.Width+'px' , height: VideoCard.ImageSize+'px'}}>
        <div className="cardImage"><DocumentCardPreview {...previewProps} /></div>
        <div className="cardContent">
          <p className="title">{this.props.title}</p>
            <div className="videoInfo">
              {this.props.videoInfo? Object.keys(this.props.videoInfo).map(i=> <p><Text>{this.props.videoInfo[i]}</Text></p>):null}
              {this.props.tags? <p>{this.props.tags.reduce((s, i)=> s+";"+i)}</p>:null}
            </div>
            <div className="playButton" hidden={false}>
              <ActionButton data-automation-id="test" iconProps={{ iconName: 'Play' }} onClick={()=>{this.props.playVideoFunc()}}>
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

}
