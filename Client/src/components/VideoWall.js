import * as React from 'react';
import ReactDOM from 'react-dom';
import './VideoWall.css';
import SimpleBar from 'simplebar-react';
import 'simplebar/dist/simplebar.min.css';
import VideoCard from './VideoCard';

/* parameters
videos
playVideoFunc
*/
export default class VideoWall extends React.Component{
  constructor(props){
    super(props);
    this.state={
      showPlayer: false
    };
  }
  render(){
    var colNum=this.props.colNum;
    const videoRows=((n, a)=>[...Array(Math.floor((a.length+n-1)/n)).keys()].map(i=>a.slice(i*n, (i+1)*n)))(colNum, this.props.videos);
    console.log(videoRows);
    return (
     
      <div className="VideoWall"> 
        <SimpleBar className="SimpleBar" style={{ width: colNum*(VideoCard.Width+25)+'px' }}>
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
              </tbody>
          </table>
        </SimpleBar>
      </div>
    );
  }
}
