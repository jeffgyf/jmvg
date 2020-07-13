import React from 'react';
import { 
  Text,
  DocumentCard,
  DocumentCardPreview,
  ActionButton} from 'office-ui-fabric-react';
import './UploadVideoButton.css';
import { ImageFit } from 'office-ui-fabric-react/lib/Image';
import $ from 'jquery';
import config from '../config';
import CookieCheck from './CookieCheck';
import sampleCover from '../sampleCover.png';
var logo="https://img-prod-cms-rt-microsoft-com.akamaized.net/cms/api/am/imageFileData/RE1Mu3b?ver=5c31";


export default class UploadVideoButton extends React.PureComponent {
  render() {
    return (
      <ActionButton className="UploadButton" iconProps={{ iconName: 'PageAdd' }} onClick={()=>alert("not implemented")}/>
    );
  }

}
